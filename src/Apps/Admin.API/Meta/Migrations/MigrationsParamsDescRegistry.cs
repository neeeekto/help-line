using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelpLine.BuildingBlocks.Application.TypeDescription;

namespace HelpLine.Apps.Admin.API.Meta.Migrations
{
    public class MigrationsParamsDescRegistry : ReadOnlyDictionary<string, Description>
    {

        public MigrationsParamsDescRegistry() : base(new Dictionary<string, Description>()
        {

        })
        {

        }


    }
}
