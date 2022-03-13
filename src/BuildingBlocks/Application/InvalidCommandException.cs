using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpLine.BuildingBlocks.Application
{
    public class InvalidCommandException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public InvalidCommandException(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public InvalidCommandException(params string[] errors)
        {
            Errors = errors.ToList();
        }
    }
}
