using System;
using HelpLine.BuildingBlocks.Application;

namespace HelpLine.Modules.Helpdesk.Application.TemporaryProblems.DTO
{
    public class TemporaryProblemUpdateDto
    {
        public Guid Id { get; set; }
        public LocalizeDictionary<TemporaryProblemUpdateContentDto> Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? ActivationDate { get; set; }
    }
}
