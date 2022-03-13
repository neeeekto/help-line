using System;
using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application
{
    public class NotFoundException : Exception
    {
        public IEnumerable<object> Ids { get; }

        public NotFoundException(object id) : base($"Entity by id {id} not found")
        {
            Ids = new []{id};
        }

        public NotFoundException(params object[] ids) : base($"Entities by ids {string.Join(", ", ids)} not found")
        {
            Ids = ids;
        }

        public NotFoundException(object id, string message) : base(message)
        {
            Ids = new []{id};
        }
    }
}
