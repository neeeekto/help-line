using System;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.RemoveUserSessions;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.SaveUserSession;
using HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserSession;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Newtonsoft.Json;

namespace HelpLine.Apps.Identity.Configuration.Authorization
{
    public class ReferenceTokenStore : IReferenceTokenStore
    {
        private readonly IUserAccessModule _userAccess;
        private readonly AdminSettings _adminSettings;
        private readonly InMemoreTokenStore _adminTokenStore = new();

        public ReferenceTokenStore(IUserAccessModule userAccess, AdminSettings adminSettings)
        {
            _userAccess = userAccess;
            _adminSettings = adminSettings;
        }

        public async Task<string> StoreReferenceTokenAsync(Token token)
        {
            var admin = _adminSettings.FindAdmin(token.SubjectId);
            if (admin != null)
                return _adminTokenStore.SaveToken(token);
            var tokenData =
                JsonConvert.SerializeObject(token, new IdentityServer4.Stores.Serialization.ClaimConverter());
            var cmd = new SaveUserSessionCommand(Guid.Parse(token.SubjectId), tokenData,
                TimeSpan.FromSeconds(token.Lifetime));
            var sessionId = await _userAccess.ExecuteCommandAsync(cmd);
            return sessionId.ToString();
        }

        public async Task<Token> GetReferenceTokenAsync(string sessionId)
        {
            var adminToken = _adminTokenStore.FindToken(sessionId);
            if (adminToken != null)
                return adminToken;

            var session = await _userAccess.ExecuteQueryAsync(new GetUserSessionQuery(Guid.Parse(sessionId)));
            if (session == null)
                return null;
            var token = JsonConvert.DeserializeObject<Token>(session.Data,
                new IdentityServer4.Stores.Serialization.ClaimConverter());
            return token;
        }

        public Task RemoveReferenceTokenAsync(string sessionId)
        {
            if (_adminTokenStore.Remove(sessionId))
                return Task.CompletedTask;

            return _userAccess.ExecuteCommandAsync(new RemoveUserSessionCommand(Guid.Parse(sessionId)));
        }

        public Task RemoveReferenceTokensAsync(string subjectId, string clientId)
        {
            if (_adminTokenStore.Remove(subjectId, clientId))
                return Task.CompletedTask;

            return _userAccess.ExecuteCommandAsync(new RemoveUserSessionsCommand(Guid.Parse(subjectId)));
        }
    }
}
