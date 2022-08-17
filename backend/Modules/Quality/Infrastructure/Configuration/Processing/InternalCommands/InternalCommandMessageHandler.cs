using System;
using Autofac;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration.Processing.InternalCommands
{
    internal class InternalCommandTaskHandler : InternalCommandTaskHandlerBase<InternalCommandTask>
    {
        protected override ILifetimeScope GetScope() => QualityCompositionRoot.BeginLifetimeScope();

        protected override Type GetType(string name) => Assemblies.Application
            .GetType(name);
    }
}
