namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class ToggleHardAssigmentAction : TicketActionBase
    {
        public bool HardAssigment { get; set; }

        public ToggleHardAssigmentAction()
        {
        }

        public ToggleHardAssigmentAction(bool hardAssigment)
        {
            HardAssigment = hardAssigment;
        }
    }
}
