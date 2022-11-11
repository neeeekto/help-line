using System.Collections.Generic;


namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class ChangeUserMetaAction : TicketActionBase
    {
        public IDictionary<string, string> Meta { get; set; }

        public ChangeUserMetaAction()
        {
        }

        public ChangeUserMetaAction(IDictionary<string, string> meta)
        {
            Meta = meta;
        }
    }
}
