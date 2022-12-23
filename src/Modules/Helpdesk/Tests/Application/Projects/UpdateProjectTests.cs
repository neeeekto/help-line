using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Projects.Commands.CreateProject;
using HelpLine.Modules.Helpdesk.Application.Projects.Commands.UpdateProject;
using HelpLine.Modules.Helpdesk.Application.Projects.DTO;
using HelpLine.Modules.Helpdesk.Application.Projects.Queries.GetProjects;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Projects
{
    [TestFixture]
    public class UpdateProjectTests : HelpdeskTestBase
    {
        protected override string NS => "projects";

        [TestCase(null, "test", new[] {"en"})]
        [TestCase("test", null, new[] {"en"})]
        [TestCase("test", "test", null)]
        [TestCase(null, "test", null)]
        [TestCase("test", null, null)]
        [TestCase(null, null, null)]
        [TestCase("", "test", new[] {"en"})]
        [TestCase("test", "", new[] {"en"})]
        [TestCase("", "", new[] {"en"})]
        [TestCase("test", "test", new[] {""})]
        [TestCase("test", "test", new string[] { })]
        public async Task UpdateProject_WhenDataIsInvalid_ThrowsInvalidCommandException(string id, string name,
            IEnumerable<string> language)
        {
            Assert.CatchAsync<InvalidCommandException>(async () =>
            {
                var cmd = new UpdateProjectCommand(id, new ProjectDataDto(name, "img", language));
                await Module.ExecuteCommandAsync(cmd);
            });
        }

        [Test]
        public async Task UpdateProject_WhenDataIsValid_IsSuccessful()
        {
            var cmd = new CreateProjectCommand("test", new ProjectDataDto( "test", "img", new[] {"en"}));
            await Module.ExecuteCommandAsync(cmd);

            var updateCmd =
                new UpdateProjectCommand(cmd.ProjectId, new ProjectDataDto("test2", "img", new[] {"ru", "de"}));
            await Module.ExecuteCommandAsync(updateCmd);

            var projects = await Module.ExecuteQueryAsync(new GetProjectsQuery());
            var project = projects.FirstOrDefault(x => x.Id == cmd.ProjectId);

            Assert.AreEqual(updateCmd.Data.Name, project.Info.Name);
            Assert.AreEqual(updateCmd.Data.Languages.Count(), project.Languages.Count());
            Assert.IsTrue(updateCmd.Data.Languages.All(x => project.Languages.Contains(x)));
        }

        [Test]
        public async Task UpdateProject_WhenProjectIsNotExist_ThrowsNotFoundException()
        {
            Assert.CatchAsync<NotFoundException>(async () =>
            {
                await Module.ExecuteCommandAsync(
                    new UpdateProjectCommand("test", new ProjectDataDto( "test2", "img", new[] {"ru", "de"})));
            });
        }
    }
}
