using HelpLine.BuildingBlocks.Domain;
using Microsoft.AspNetCore.Http;

namespace HelpLine.Apps.Admin.API.Configuration.Validation
{
    public class BusinessRuleValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
        {
            Title = "Business rule broken";
            Status = StatusCodes.Status409Conflict;
            Detail = exception.Message;
            Type = "https://somedomain/business-rule-validation-error";
        }
    }
}
