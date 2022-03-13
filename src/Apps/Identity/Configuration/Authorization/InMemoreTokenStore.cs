using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;

namespace HelpLine.Apps.Identity.Configuration.Authorization
{
    public class InMemoreTokenStore
    {
        private Dictionary<string, Token> _store = new ();

        public string SaveToken(Token token)
        {
            _store.TryAdd(token.SessionId, token);
            return token.SessionId;
        }

        public Token? FindToken(string sessionId)
        {
            var token = _store.FirstOrDefault(x => x.Key == sessionId);
            return token.Value;
        }

        public bool Remove(string sessionId)
        {
            return _store.Remove(sessionId);
        }

        public bool Remove(string subjectId, string clientId)
        {
            var target = _store.FirstOrDefault(x => x.Value.SubjectId == subjectId && x.Value.ClientId == clientId);
            return _store.Remove(target.Key);
        }
    }
}
