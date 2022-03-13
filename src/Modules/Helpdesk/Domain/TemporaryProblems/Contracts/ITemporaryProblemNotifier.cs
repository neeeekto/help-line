using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Contracts
{
    public interface ITemporaryProblemNotifier
    {
        Task NotifyAboutNewUpdate(TemporaryProblemUpdate update, IEnumerable<TemporaryProblemSubscriberEmail> emails);
        Task NotifyAboutSubscription(TemporaryProblemSubscriberEmail email, TemporaryProblem problem);
    }
}
