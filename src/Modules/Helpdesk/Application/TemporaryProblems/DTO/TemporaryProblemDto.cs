using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Domain.TemporaryProblems;

namespace HelpLine.Modules.Helpdesk.Application.TemporaryProblems.DTO
{
    public class TemporaryProblemDto
    {
        public Guid Id { get;  set; }
        public string ProjectId { get;  set; }
        public string Name { get;  set; }
        public IEnumerable<TemporaryProblemSubscriberDto> Subscribers { get;  set; }
        public IEnumerable<TemporaryProblemUpdateDto> Updates { get;  set; }
        public TemporaryProblemMetaDto Meta { get;  set; }
        public LocalizeDictionary<TemporaryProblemInfoDto> Info { get;  set; }
        public ReadOnlyDictionary<TemporaryProblemStatus, DateTime> StatusesDate { get;  set; }
        public TemporaryProblemStatus Status { get;  set; }
    }
}
