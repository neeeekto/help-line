using HelpLine.BuildingBlocks.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Configuration.Validation
{
    public class NotFoundProblemDetails: ProblemDetails
    {
        public NotFoundProblemDetails(NotFoundException exception)
        {
            Title = "Entity not found";
            Detail = exception.Message;
            Status = StatusCodes.Status404NotFound;
            Type = "https://somedomain/validation-error";
        }
    }
}
