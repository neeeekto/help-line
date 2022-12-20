using System.Collections.Generic;
using HelpLine.Services.TemplateRenderer.Contracts;
using HelpLine.Services.TemplateRenderer.Models;

namespace HelpLine.Services.TemplateRenderer.Application.Commands.Save
{
    public abstract class SaveCommand<T> : ICommand
    {
        public IEnumerable<T> Data { get; }

        protected SaveCommand(IEnumerable<T> data)
        {
            Data = data;
        }
    }

    public class SaveComponentCommand : SaveCommand<Component>
    {
        public SaveComponentCommand(params Component[] data) : base(data) {}
        public SaveComponentCommand(IEnumerable<Component> data) : base(data)
        {
        }
    }

    public class SaveContextCommand : SaveCommand<Context>
    {
        public SaveContextCommand(params Context[] data) : base(data) {}
        public SaveContextCommand(IEnumerable<Context> data) : base(data)
        {
        }
    }

    public class SaveTemplateCommand : SaveCommand<Template>
    {
        public SaveTemplateCommand(params Template[] data) : base(data)
        {
        }

        public SaveTemplateCommand(IEnumerable<Template> data) : base(data)
        {
        }
    }
}
