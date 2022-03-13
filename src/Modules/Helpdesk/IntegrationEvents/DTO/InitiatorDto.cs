using System;

namespace HelpLine.Modules.Helpdesk.IntegrationEvents.DTO
{
    public abstract class InitiatorDto
    {

    }

    public class SystemInitiatorDto : InitiatorDto
    {
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
