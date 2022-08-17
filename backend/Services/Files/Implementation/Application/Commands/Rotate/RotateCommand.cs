using System.Collections.Generic;
using HelpLine.Services.Files.Application.DTO;
using HelpLine.Services.Files.Contracts;

namespace HelpLine.Services.Files.Application.Commands.Rotate
{
    public class RotateCommand : ICommand
    {
        public IEnumerable<RotateDto> Rotates { get; }

        public RotateCommand(IEnumerable<RotateDto> rotates)
        {
            Rotates = rotates;
        }
    }
}
