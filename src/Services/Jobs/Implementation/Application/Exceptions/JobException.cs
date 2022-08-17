using System;

namespace HelpLine.Services.Jobs.Application.Exceptions
{
    public class JobException : Exception
    {
        public JobException(string? message) : base(message)
        {
        }
    }
}
