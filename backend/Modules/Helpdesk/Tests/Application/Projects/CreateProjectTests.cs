using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Projects.Commands.CreateProject;
using HelpLine.Modules.Helpdesk.Application.Projects.DTO;
using HelpLine.Modules.Helpdesk.Application.Projects.Queries.GetProjects;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Projects
{
    [NonParallelizable]
    [TestFixture]
    public class CreateProjectTests : HelpdeskTestBase
    {
        protected override string NS => "projects";

        [Test]
        public async Task CreateProject_WhenDataIsValid_IsSuccessful()
        {
            var cmd = new CreateProjectCommand("test", new ProjectDataDto( "test", "img", new[] {"en"}));
            var result = await Module.ExecuteCommandAsync(cmd);
            var projects = await Module.ExecuteQueryAsync(new GetProjectsQuery());
            var project = projects.FirstOrDefault(x => x.Id == result);
            Assert.AreEqual(cmd.ProjectId, result);
            Assert.NotNull(project);
            Assert.AreEqual(project.Active, true);
            Assert.AreEqual(project.Info.Name, cmd.Data.Name);
            Assert.IsTrue(project.Languages.Count() == 1);
            Assert.IsTrue(project.Languages.Contains("en"));
        }

        [Test]
        public async Task CreateProject_WhenIdIsNotUnique_ThrowsInvalidCommandException()
        {
            var cmd = new CreateProjectCommand("test", new ProjectDataDto( "test", "img", new[] {"en"}));
            await Module.ExecuteCommandAsync(cmd);

            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }

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
        public async Task CreateProject_WhenDataIsInvalid_ThrowsInvalidCommandException(string id, string name,
            IEnumerable<string> language)
        {
            Assert.CatchAsync<InvalidCommandException>(async () =>
            {
                var cmd = new CreateProjectCommand(id, new ProjectDataDto( name, "img", language));
                await Module.ExecuteCommandAsync(cmd);
            });
        }
    }
}
