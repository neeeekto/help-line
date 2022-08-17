using System.Collections.Generic;
using System.Linq;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TagCondition
    {
        public bool Include { get; set; } // false = exclude
        public IEnumerable<string> Tags { get; set; }
        public bool All { get; set; } // false = any

        public bool Check(IEnumerable<string> tags)
        {
            var tagsStatus = Tags.ToDictionary(x => x, tags.Contains);
            return All ? tagsStatus.All(Checker) : tagsStatus.Any(Checker);
        }

        private bool Checker(KeyValuePair<string, bool> item) => item.Value == Include;
    }
}
