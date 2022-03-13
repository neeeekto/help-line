using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs
{
    internal class JobTasksCollection
    {
        public class TaskDescriptor
        {
            private readonly Type _type;
            public Type? DataType { get; }
            public string Name => _type.FullName;

            public TaskDescriptor(Type type)
            {
                _type = type;
                if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(JobTask<>))
                {
                    var parent = type.BaseType!;
                    DataType = parent.GetGenericArguments().First()!;
                }
            }


            public JobTask Make(Guid id)
            {
                var ctor = _type.GetConstructor(new[] {typeof(Guid)});
                var result = (JobTask) ctor.Invoke(new object[] {id});
                return result;
            }

            public JobTask Make(Guid id, JobDataBase dataBase)
            {
                var result = (JobTask) Activator.CreateInstance(_type,
                    new object[] {id, dataBase})!;
                return result;
            }
        }

        private readonly List<TaskDescriptor> _tasks = new List<TaskDescriptor>();
        public IEnumerable<TaskDescriptor> Tasks => _tasks.AsReadOnly();

        public JobTasksCollection(IEnumerable<Assembly> jobAssemblies)
        {
            foreach (var assembly in jobAssemblies)
            foreach (Type type in assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(JobTask))))
                _tasks.Add(new TaskDescriptor(type));
        }
    }
}
