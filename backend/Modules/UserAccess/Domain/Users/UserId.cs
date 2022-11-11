using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Users
{
    public class UserId : TypedGuidIdValueBase
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}