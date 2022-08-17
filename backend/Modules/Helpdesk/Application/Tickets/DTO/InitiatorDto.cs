using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.DTO
{
    public abstract class InitiatorDto
    {

    }

    public class SystemInitiatorDto : InitiatorDto
    {
        public string? Description { get; set; }
        public IDictionary<string, string>? Meta { get; set; }
    }

    public class OperatorInitiatorDto : InitiatorDto
    {
        public Guid OperatorId { get; set; }

        public OperatorInitiatorDto()
        {
        }

        public OperatorInitiatorDto(Guid operatorId)
        {
            OperatorId = operatorId;
        }
    }

    public class UserInitiatorDto : InitiatorDto
    {
        public string UserId { get; set; }

        public UserInitiatorDto()
        {
        }

        public UserInitiatorDto(string userId)
        {
            UserId = userId;
        }
    }
}
