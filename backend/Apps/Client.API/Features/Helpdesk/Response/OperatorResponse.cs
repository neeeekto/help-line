using System;
using System.Collections.Generic;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Response
{
    public class OperatorResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public bool Active { get; set; }
    }
}
