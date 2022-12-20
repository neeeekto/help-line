using HelpLine.Services.Jobs.Contracts;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Configuration.Jobs
{
    public interface IJobHandler<in TJob> :
        IRequestHandler<TJob> where TJob : JobTask
    {

    }
}
