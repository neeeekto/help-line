using System;
using Autofac;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    internal class InternalCommandTaskHandler : InternalCommandTaskHandlerBase<InternalCommandTask>
    {
        protected override ILifetimeScope GetScope() => UserAccessCompositionRoot.BeginLifetimeScope();

        protected override Type GetType(string name) => Assemblies.Application
            .GetType(name);
    }
}
