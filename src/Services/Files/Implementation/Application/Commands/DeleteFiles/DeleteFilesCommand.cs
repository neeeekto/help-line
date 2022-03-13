using System.Collections.Generic;
using System.Linq;
using HelpLine.Services.Files.Contracts;

namespace HelpLine.Services.Files.Application.Commands.DeleteFiles
{
    public class DeleteFilesCommand : ICommand
    {
        public IEnumerable<string> FilesIds { get; }

        public DeleteFilesCommand(IEnumerable<string> filesIds)
        {
            FilesIds = filesIds.Distinct();
        }

    }
}
