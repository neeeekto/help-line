using System.Collections.Generic;
using HelpLine.Services.TemplateRenderer.Contracts;

namespace HelpLine.Services.TemplateRenderer.Application.Commands.Delete
{
    public abstract class DeleteCommand : ICommand
    {
        public IEnumerable<string> ItemsIds { get; }

        protected DeleteCommand(params string[] itemsIds)
        {
            ItemsIds = itemsIds;
        }
    }

    public class DeleteContextsCommand : DeleteCommand
    {
        public DeleteContextsCommand(params string[] itemsIds) : base(itemsIds)
        {
        }
    }

    public class DeleteComponentsCommand : DeleteCommand
    {
        public DeleteComponentsCommand(params string[] itemsIds) : base(itemsIds)
        {
        }
    }

    public class DeleteTemplatesCommand : DeleteCommand
    {
        public DeleteTemplatesCommand(params string[] itemsIds) : base(itemsIds)
        {
        }
    }
}
