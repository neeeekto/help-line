using System;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.UserAccess.Domain.Users.Contracts;
using HelpLine.Modules.UserAccess.Domain.Users.Events;

namespace HelpLine.Modules.UserAccess.Domain.Users
{
    public class UserSecurity : Entity
    {
        public UserId UserId { get; private set; }
        public bool IsBlocked { get; private set; }

        private string? _password;

        internal UserSecurity(UserId userId)
        {
            UserId = userId;
            IsBlocked = false;
        }

        public void Block()
        {
            IsBlocked = true;
            AddDomainEvent(new UserBlockedDomainEvent(UserId));
        }

        public void Unblock()
        {
            IsBlocked = false;
        }

        public void ChangePassword(IPasswordManager passwordManager, string? password)
        {
            if (string.IsNullOrEmpty(password))
                _password = null;

            _password = passwordManager.Protect(password!);
        }

        public bool CheckPassword(IPasswordManager passwordManager, string password)
        {
            if (IsBlocked) return false;
            return !string.IsNullOrEmpty(_password) && passwordManager.Check(_password, password);
        }
    }
}
