using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Configuration.Validation
{
    public class InvalidCommandProblemDetails : ProblemDetails
    {
        public List<string> ValidationErrors { get; }
        public InvalidCommandProblemDetails(InvalidCommandException exception)
        {
            Title = "Command validation error";
            Status = StatusCodes.Status400BadRequest;
            Type = "https://somedomain/validation-error";
            ValidationErrors = exception.Errors.ToList();
        }
    }
}
