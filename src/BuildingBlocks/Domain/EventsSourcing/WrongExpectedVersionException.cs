using System;

namespace HelpLine.BuildingBlocks.Domain.EventsSourcing
{
    public class WrongExpectedVersionException : Exception
    {
        public WrongExpectedVersionException(string message) : base(message)
        {
        }
    }
}