using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Tests.SeedWork
{
    public class TestJobDataNested
    {
        public string Test { get; set; } = "test1";
    }

    public class TestJobDataBase : JobDataBase
    {
        public string Test { get; set; } = "test1";
        public TestJobDataNested TestData { get; set; } = new TestJobDataNested();
    }

    public class TestJobTask : JobTask<TestJobDataBase>
    {
        public TestJobTask(Guid id, TestJobDataBase data) : base(id, data)
        {
        }
    }
}
