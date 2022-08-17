using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class UserChannelsCannotBeEmptyRule : IBusinessRule
    {
        private readonly UserChannels _channels;

        internal UserChannelsCannotBeEmptyRule(UserChannels channels)
        {
            _channels = channels;
        }

        public string Message => "Recipients is empty";
        public bool IsBroken() => _channels.Count == 0;
    }
}
