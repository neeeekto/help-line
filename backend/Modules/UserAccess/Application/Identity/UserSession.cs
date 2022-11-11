using System;
using Newtonsoft.Json;

namespace HelpLine.Modules.UserAccess.Application.Identity
{
    public class UserSession
    {
        public Guid SessionId { get; private set; }
        public Guid UserId { get; private set; }
        public string Data { get; private set;  }
        public DateTime CreateDate { get; private set;  }
        public DateTime Expired { get; private set; }

        public UserSession()
        {
        }

        [JsonConstructor]
        public UserSession(Guid sessionId, Guid userId, string data, DateTime createDate, DateTime expired)
        {
            SessionId = sessionId;
            UserId = userId;
            Data = data;
            CreateDate = createDate;
            Expired = expired;
        }
    }
}
