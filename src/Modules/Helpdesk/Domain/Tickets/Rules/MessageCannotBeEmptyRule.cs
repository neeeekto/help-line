using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class MessageCannotBeEmptyRule : IBusinessRule
    {
        private readonly Message? _message;
        private readonly bool _textRequired;

        internal MessageCannotBeEmptyRule(Message? message, bool textRequired = false)
        {
            _message = message;
            _textRequired = textRequired;
        }

        public string Message => $"Message is empty";

        public bool IsBroken()
        {
            if (_message == null) return true;
            var hasText = !string.IsNullOrEmpty(_message.Text);
            var hasAttach = _message.Attachments?.Any() ?? false;
            if (!hasText && _textRequired) return true;
            return !(hasText || hasAttach);
        }
    }
}
