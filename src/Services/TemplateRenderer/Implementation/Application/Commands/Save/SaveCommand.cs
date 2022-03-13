using HelpLine.Services.TemplateRenderer.Contracts;
using HelpLine.Services.TemplateRenderer.Models;

namespace HelpLine.Services.TemplateRenderer.Application.Commands.Save
{

    public abstract class SaveCommand<T> : ICommand
    {
        public T Data { get; }

        protected SaveCommand(T data)
        {
            Data = data;
        }
    }
    public class SaveComponentCommand : SaveCommand<Component>
    {
        public SaveComponentCommand(Component data) : base(data)
        {
        }
    }

    public class SaveContextCommand : SaveCommand<Context>
    {
        public SaveContextCommand(Context data) : base(data)
        {
        }
    }

    public class SaveTemplateCommand : SaveCommand<Template>
    {
        public SaveTemplateCommand(Template data) : base(data)
        {
        }
    }
}
