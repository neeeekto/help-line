using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Automations
{
    [TestFixture]
    public class TagConditionTests
    {
        [TestCase(false, true, new [] {"test1"}, new [] {"test1", "test2"}, true)]
        [TestCase(false, true, new [] {"test1"}, new [] {"test2"}, false)]
        [TestCase(true, true, new [] {"test1", "test2"}, new [] {"test1", "test2"}, true)]
        [TestCase(true, true, new [] {"test1", "test2"}, new [] {"test1"}, false)]
        [TestCase(true, false, new [] {"test1", "test2"}, new [] {"test3", "test4"}, true)]
        [TestCase(true, false, new [] {"test1", "test2"}, new [] {"test1"}, false)]
        [TestCase(false, false, new [] {"test1", "test2"}, new [] {"test1"}, true)]
        [TestCase(false, false, new [] {"test1", "test2"}, new [] {"test1", "test2"}, false)]
        public async Task Tests(bool all, bool include, string[] tags, string[] checkTags, bool result)
        {
            var condition = new TagCondition
            {
                All = all,
                Include = include,
                Tags = tags
            };

            Assert.AreEqual(result, condition.Check(checkTags));
        }
    }
}
