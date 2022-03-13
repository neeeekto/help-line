using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Queries;
using HelpLine.Modules.UserAccess.Application.Identity.Services;

namespace HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserSession
{
    class GetUserSessionQueryHandler : IQueryHandler<GetUserSessionQuery, UserSession?>, IQueryHandler<GetUserSessionsQuery, IEnumerable<UserSession>>
    {
        private readonly SessionManager _sessionManager;

        public GetUserSessionQueryHandler(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }


        public async Task<UserSession?> Handle(GetUserSessionQuery request, CancellationToken cancellationToken)
        {
            return await _sessionManager.Get(request.SessionId);
        }

        public async Task<IEnumerable<UserSession>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
        {
            return await _sessionManager.GetList(request.UserId);
        }
    }
}
