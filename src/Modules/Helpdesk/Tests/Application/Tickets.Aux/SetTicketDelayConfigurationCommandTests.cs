using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetTicketDelayConfiguration;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketsDelayConfiguration;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [TestFixture]
    public class SetTicketDelayConfigurationCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(SetTicketDelayConfigurationCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        [Test]
        public async Task SetDelayConfigurationCommand_WhenIsValid_IsSuccessful()
        {
            var src = new InvalidSource();
            await Module.ExecuteCommandAsync(new SetTicketDelayConfigurationCommand(src.ProjectId, src.LifeCycleDelay, src.InactivityDelay, src.FeedbackCompleteDelay));

            var entity = await Module.ExecuteQueryAsync(new GetTicketsDelayConfigurationQuery(ProjectId));

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.ProjectId, Is.EqualTo(ProjectId));
            Assert.That(entity.InactivityDelay, Is.EqualTo(src.InactivityDelay));
            Assert.That(entity.FeedbackCompleteDelay, Is.EqualTo(src.FeedbackCompleteDelay));
            Assert.That(entity.LifeCycleDelay, Is.EqualTo(src.LifeCycleDelay));
        }

        public class InvalidSource
        {
            public string ProjectId = HelpdeskTestBase.ProjectId;

            public ReadOnlyDictionary<TicketLifeCycleType, TimeSpan> LifeCycleDelay =
                new ReadOnlyDictionary<TicketLifeCycleType, TimeSpan>(
                    new Dictionary<TicketLifeCycleType, TimeSpan>()
                    {
                        {TicketLifeCycleType.Closing, new TimeSpan(1)},
                        {TicketLifeCycleType.Feedback, new TimeSpan(1)},
                        {TicketLifeCycleType.Resolving, new TimeSpan(1)}
                    });

            public TimeSpan InactivityDelay = new TimeSpan(1);
            public TimeSpan FeedbackCompleteDelay = new TimeSpan(1);

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = ""
                        }
                    ).SetName("Empty project: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = null
                        }
                    ).SetName("Empty project: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = "asd"
                        }
                    ).SetName("Not exist project");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            InactivityDelay = new TimeSpan(0)
                        }
                    ).SetName("Invalid InactivityDelay");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            FeedbackCompleteDelay = new TimeSpan(0)
                        }
                    ).SetName("Invalid FeedbackCompleteDelay");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            LifeCycleDelay = null
                        }
                    ).SetName("Invalid LifeCycleDelay: null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            LifeCycleDelay = new ReadOnlyDictionary<TicketLifeCycleType, TimeSpan>(new Dictionary<TicketLifeCycleType, TimeSpan>())
                        }
                    ).SetName("Invalid LifeCycleDelay: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            LifeCycleDelay = new ReadOnlyDictionary<TicketLifeCycleType, TimeSpan>(new Dictionary<TicketLifeCycleType, TimeSpan>()
                            {
                                {TicketLifeCycleType.Closing, new TimeSpan(1)},
                                {TicketLifeCycleType.Feedback, new TimeSpan(1)},
                                {TicketLifeCycleType.Resolving, new TimeSpan(0)}
                            })
                        }
                    ).SetName("Invalid LifeCycleDelay: Invalid timespan");

                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SetDelayConfigurationCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SetTicketDelayConfigurationCommand(src.ProjectId, src.LifeCycleDelay, src.InactivityDelay, src.FeedbackCompleteDelay);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
