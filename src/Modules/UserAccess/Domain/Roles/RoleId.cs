using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Roles
{
    public class RoleId : TypedGuidIdValueBase
    {
        public RoleId(Guid value) : base(value)
        {
        }
    }
}