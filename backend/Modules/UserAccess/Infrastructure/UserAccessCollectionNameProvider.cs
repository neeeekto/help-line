using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.Inbox;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;
using HelpLine.Modules.UserAccess.Application.Identity;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration;

namespace HelpLine.Modules.UserAccess.Infrastructure
{
    internal class UserAccessCollectionNameProvider : CollectionNameProvider
    {
        public UserAccessCollectionNameProvider() : base(ModuleInfo.NameSpace)
        {
            Add<InternalCommandTaskBase>("InternalCommands");
            Add<OutboxMessage>("OutboxMessages");
            Add<User>("Users");
            Add<Role>("Roles");
            Add<UserSession>("UserSessions");
        }
    }
}
