using System.Collections.Generic;
using System.Reflection;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Infrastructure;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Infrastructure;
using HelpLine.Tests.Arch.SeedWork;
using MediatR;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Tests.Arch.Modules
{
    [TestFixture]
    public class ModuleTests : TestBase
    {
        [Test]
        public void TicketsModule_DoesNotHave_Dependency_On_Other_Modules()
        {
            var otherModules = new List<string>
            {
                UserAccessNamespace, QualityNamespace
            };
            List<Assembly> ticketsAssemblies = new List<Assembly>
            {
                typeof(HelpdeskModule).Assembly,
                typeof(Ticket).Assembly,
                typeof(IHelpdeskModule).Assembly
            };

            var result = Types.InAssemblies(ticketsAssemblies)
                .That()
                    .DoNotImplementInterface(typeof(INotificationHandler<>))
                    .And().DoNotHaveNameEndingWith("IntegrationEventHandler")
                    .And().DoNotHaveName("EventsBusStartup")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void UserAccessModule_DoesNotHave_Dependency_On_Other_Modules()
        {
            var otherModules = new List<string>
            {
                TicketsNamespace, QualityNamespace
            };
            List<Assembly> userAccessAssemblies = new List<Assembly>
            {
                typeof(UserAccessModule).Assembly,
                typeof(User).Assembly,
                typeof(IUserAccessModule).Assembly
            };

            var result = Types.InAssemblies(userAccessAssemblies)
                .That()
                .DoNotImplementInterface(typeof(INotificationHandler<>))
                .And().DoNotHaveNameEndingWith("IntegrationEventHandler")
                .And().DoNotHaveName("EventsBusStartup")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}
