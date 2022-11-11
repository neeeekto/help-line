using System;
using Autofac;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing.InternalCommands
{
    internal class InternalCommandTaskHandler : InternalCommandTaskHandlerBase<InternalCommandTask>
    {
        protected override ILifetimeScope GetScope() => HelpdeskCompositionRoot.BeginLifetimeScope();

        protected override Type GetType(string name) => Assemblies.Application
            .GetType(name);
    }
}
