using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HelpLine.Apps.Client.API.Configuration.Middlewares
{
    internal class ProjectMiddleware
    {
        private readonly RequestDelegate _next;
        internal const string ProjectHeaderKey = "Project";

        public ProjectMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
        }
    }
}
