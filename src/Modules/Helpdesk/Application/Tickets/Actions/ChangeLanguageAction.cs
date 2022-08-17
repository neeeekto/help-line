

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class ChangeLanguageAction : TicketActionBase
    {
        public string Language { get; set; }

        public ChangeLanguageAction()
        {
        }

        public ChangeLanguageAction(string language)
        {
            Language = language;
        }
    }
}
