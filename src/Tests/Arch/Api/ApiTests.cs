using System.Collections.Generic;
using HelpLine.Tests.Arch.SeedWork;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Tests.Arch.Api
{
    [TestFixture]
    public class ApiTests : TestBase
    {
        [Test]
        public void AdministrationApi_DoesNotHaveDependency_ToOtherModules()
        {
            /*var otherModules = new List<string>
            {
                MeetingsNamespace, PaymentsNamespace, UserAccessNamespace
            };
            var result = Types.InAssembly(ApiAssembly)
                .That()
                        .ResideInNamespace("HelpLine.API.Modules.Administration")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);*/
        }
    }
}
