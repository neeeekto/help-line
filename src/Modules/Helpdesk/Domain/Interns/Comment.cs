using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Operators;

namespace HelpLine.Modules.Helpdesk.Domain.Interns
{
    public class Comment
    {
        public DateTime DateTime { get; private set; }
        public OperatorId OperatorId { get; private set; }
        public string Message { get; private set; }
        public IEnumerable<string> Attachments { get; private set; }

        public Comment(OperatorId operatorId, string message, IEnumerable<string> attachments)
        {
            OperatorId = operatorId;
            Message = message;
            Attachments = attachments;
            DateTime = DateTime.UtcNow;
        }
    }
}
