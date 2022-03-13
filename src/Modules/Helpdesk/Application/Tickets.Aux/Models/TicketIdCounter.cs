namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TicketIdCounter
    {
        public string ProjectId { get; private set; }
        public int Number { get; private set; }
        public int Last { get; private set; }

        internal TicketIdCounter(string projectId, int number)
        {
            ProjectId = projectId;
            Number = number;
            Last = 0;
        }

        internal int Next()
        {
            Last += 1;
            return Last;
        }
    }
}
