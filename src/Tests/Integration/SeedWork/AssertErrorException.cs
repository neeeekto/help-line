using System;

namespace HelpLine.Tests.Integration.SeedWork
{
    public class AssertErrorException : Exception
    {
        public AssertErrorException(string message) : base(message)
        {
            
        }
    }
}