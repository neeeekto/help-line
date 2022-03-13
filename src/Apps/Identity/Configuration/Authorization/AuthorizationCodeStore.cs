using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace HelpLine.Apps.Identity.Configuration.Authorization
{
    // DefaultAuthorizationCodeStore didn't work. Problem with json deserialization.
    internal class AuthorizationCodeStore : IAuthorizationCodeStore
    {
        private readonly IHandleGenerationService _handleGenerationService;
        private readonly IPersistedGrantStore _persistedGrantStore;

        private static string _grantType =
            IdentityServerConstants.PersistedGrantTypes.AuthorizationCode;

        public AuthorizationCodeStore(IHandleGenerationService handleGenerationService, IPersistedGrantStore persistedGrantStore)
        {
            _handleGenerationService = handleGenerationService;
            _persistedGrantStore = persistedGrantStore;
        }

        public  async Task<string> StoreAuthorizationCodeAsync(AuthorizationCode code)
        {
            var handle = await _handleGenerationService.GenerateAsync();
            var key = GetHashedKey(handle);
            await _persistedGrantStore.StoreAsync(new HLPersistedGrant()
            {
                Key = key,
                Type = _grantType,
                ClientId = code.ClientId,
                SubjectId = code.Subject.GetSubjectId(),
                SessionId = code.SessionId,
                Description = code.Description,
                CreationTime = code.CreationTime,
                Expiration = code.CreationTime.AddSeconds(code.Lifetime),
                ConsumedTime = null,
                Data = "",
                Code = code
            });
            return handle;
        }

        public async Task<AuthorizationCode?> GetAuthorizationCodeAsync(string code)
        {
            var key = GetHashedKey(code);
            var grant = await _persistedGrantStore.GetAsync(key);
            return ((HLPersistedGrant) grant)?.Code ?? default;
        }

        public async Task RemoveAuthorizationCodeAsync(string code)
        {
            code = GetHashedKey(code);
            await _persistedGrantStore.RemoveAsync(code);
        }

        protected virtual string GetHashedKey(string value)
        {
            return (value + ":" + _grantType).Sha256();
        }
    }
}
