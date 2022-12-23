using System;
using System.Collections.Generic;
using HelpLine.Services.Files.Application.DTO;
using HelpLine.Services.Files.Application.Views;
using HelpLine.Services.Files.Contracts;

namespace HelpLine.Services.Files.Application.Commands.CreateUploadLink
{
    public class CreateUploadLinksCommand : ICommand<IReadOnlyDictionary<string, UploadView>>
    {
        public IEnumerable<FileDto> Files { get; }

        public CreateUploadLinksCommand(IEnumerable<FileDto> files)
        {
            Files = files;
        }
    }
}
