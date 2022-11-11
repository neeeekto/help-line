using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.UserAccess.Application.Users.Commands;
using HelpLine.Modules.UserAccess.Application.Users.Commands.CreateUser;
using HelpLine.Modules.UserAccess.Application.Users.DTO;
using HelpLine.Modules.UserAccess.Application.Users.Queries;
using HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.UserAccess.Tests.Application.Users
{
    public class CreateUserCommandTests : UserAccessTestBase
    {
        protected override string NS => nameof(CreateUserCommandTests);

        [Test]
        public async Task CreateUserCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TestData();

            var cmd = new CreateUserCommand(testData.UserInfo, testData.Email, testData.GlobalRoles,
                testData.ProjectsRoles, testData.Permissions, Array.Empty<string>());

            var userId = await Module.ExecuteCommandAsync(cmd);

            var userView = await Module.ExecuteQueryAsync(new GetUserQuery(userId));

            Assert.AreEqual(userId, userView.Id);
            Assert.AreEqual(UserStatus.Active, userView.Status);
            Assert.AreEqual(testData.Email, userView.Email);
            Assert.AreEqual(testData.UserInfo.Language, userView.Info.Language);
            Assert.AreEqual(testData.UserInfo.Photo, userView.Info.Photo);
            Assert.AreEqual(testData.UserInfo.FirstName, userView.Info.FirstName);
            Assert.AreEqual(testData.UserInfo.LastName, userView.Info.LastName);
        }

        public class InvalidSource
        {
            public UserInfoDto UserInfo = new UserInfoDto
            {
                Language = "en",
                Photo = "en",
                FirstName = "fn",
                LastName = "ln"
            };

            public string Email = "test@te.te";
            public IEnumerable<string> Permissions = new string[] { };
            public IEnumerable<Guid> GlobalRoles = new Guid[] { };
            public Dictionary<string, IEnumerable<Guid>> ProjectsRoles = new Dictionary<string, IEnumerable<Guid>>();

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Email = ""
                        }
                    ).SetName("Empty email: Empty");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Email = null
                        }
                    ).SetName("Empty email: Null");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Permissions = new string[] {""}
                        }
                    ).SetName("Empty permission: Empty item");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Permissions = new string[] {null}
                        }
                    ).SetName("Empty permission: Null item");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserInfo = new UserInfoDto
                            {
                                Language = "en",
                                Photo = "en",
                                FirstName = "",
                                LastName = "ln"
                            }
                        }
                    ).SetName("Invalid info: Empty first name");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserInfo = new UserInfoDto
                            {
                                Language = "en",
                                Photo = "en",
                                FirstName = null,
                                LastName = "ln"
                            }
                        }
                    ).SetName("Invalid info: Null first name");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserInfo = new UserInfoDto
                            {
                                Language = "en",
                                Photo = "en",
                                FirstName = "test",
                                LastName = ""
                            }
                        }
                    ).SetName("Invalid info: Empty last name");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserInfo = new UserInfoDto
                            {
                                Language = "en",
                                Photo = "en",
                                FirstName = "test",
                                LastName = null
                            }
                        }
                    ).SetName("Invalid info: Null last name");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserInfo = new UserInfoDto
                            {
                                Language = null,
                                Photo = "en",
                                FirstName = "test",
                                LastName = "test"
                            }
                        }
                    ).SetName("Invalid info: Null language");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserInfo = new UserInfoDto
                            {
                                Language = "",
                                Photo = "en",
                                FirstName = "test",
                                LastName = "test"
                            }
                        }
                    ).SetName("Invalid info: Empty language");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task CreateUserCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(InvalidSource src)
        {
            var cmd = new CreateUserCommand(src.UserInfo, src.Email, src.GlobalRoles, src.ProjectsRoles,
                src.Permissions, ArraySegment<string>.Empty);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
