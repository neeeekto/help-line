using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTagsDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTagsDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTagsDescriptions;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [TestFixture]
    public class RemoveTagsDescriptionCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(RemoveTagsDescriptionCommandTests);
        public const string ExistTag = "test";
        public static Guid Audience = Guid.NewGuid();

        public static readonly IEnumerable<TagsDescriptionIssue> DescriptionIssues = new[]
        {
            new TagsDescriptionIssue
            {
                Audience = new[] {Audience},
                Contents = new LocalizeDictionary<TagsDescriptionIssueContent>()
                {
                    {
                        "en", new TagsDescriptionIssueContent
                        {
                            Text = "test"
                        }
                    }
                }
            }
        };

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag, ProjectId, true));
            await Module.ExecuteCommandAsync(new SaveTagsDescriptionCommand(ProjectId, ExistTag, true,
                DescriptionIssues));
        }

        [Test]
        public async Task RemoveTagsDescriptionCommandTests_WhenDataIsValid_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemoveTagsDescriptionCommand(ExistTag, ProjectId));
            var entities = await Module.ExecuteQueryAsync(new GetTagsDescriptionsQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNull(entity);
        }

        [Test]
        public async Task RemoveTagsDescriptionCommandTests_WhenExistPlatformInDifferentProjects_IsSuccessful()
        {
            var otherProjectId = "project2";
            await CreateProject(projectId: otherProjectId);
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag, otherProjectId, true));
            await Module.ExecuteCommandAsync(new SaveTagsDescriptionCommand(otherProjectId, ExistTag, true,
                DescriptionIssues));

            await Module.ExecuteCommandAsync(new RemoveTagsDescriptionCommand(ExistTag, ProjectId));
            var entities = await Module.ExecuteQueryAsync(new GetTagsDescriptionsQuery(otherProjectId));
            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNotNull(entity);
        }

        [Test]
        public async Task RemoveTagsDescriptionCommandTests_WhenProjectNotExist_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemoveTagsDescriptionCommand(ExistTag, "not exist"));
            var entities = await Module.ExecuteQueryAsync(new GetTagsDescriptionsQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNotNull(entity);
        }

        [Test]
        public async Task RemoveTagsDescriptionCommandTests_WhenTagNotExist_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemoveTagsDescriptionCommand("not exist", ProjectId));
            var entities = await Module.ExecuteQueryAsync(new GetTagsDescriptionsQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNotNull(entity);
        }
    }
}
