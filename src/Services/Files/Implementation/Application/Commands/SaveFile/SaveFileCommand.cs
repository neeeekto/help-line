using System;
using System.Collections.Generic;
using System.IO;
using HelpLine.Services.Files.Contracts;
using MediatR;

namespace HelpLine.Services.Files.Application.Commands.SaveFile
{
    public class SaveFileCommand : ICommand<string>
    {
        public Stream Content { get; }
        public string Name { get; }
        public string ContentType { get; }

        public SaveFileCommand(Stream content, string name, string contentType)
        {
            Content = content;
            Name = name;
            ContentType = contentType;
        }
    }
}
