using System.Collections.Generic;
using FluentValidation;

namespace HelpLine.BuildingBlocks.Application.Emails
{
    public struct EmailMessage
    {
        public string From { get; }
        public IEnumerable<string> To { get; }
        public string Subject { get; }
        public string Content { get; }
        public IReadOnlyDictionary<string, byte[]> Attachments { get; }
        public EmailMeta? Meta { get; }

        public class EmailMeta
        {
            public IEnumerable<string>? References { get; }
            public IReadOnlyDictionary<string, string>? Vars { get; }

            public EmailMeta(IEnumerable<string> references, IReadOnlyDictionary<string, string> vars)
            {
                References = references;
                Vars = vars;
            }

            public EmailMeta(IEnumerable<string> references)
            {
                References = references;
            }

            public EmailMeta(IReadOnlyDictionary<string, string> vars)
            {
                Vars = vars;
            }
        }

        public EmailMessage(string @from, IEnumerable<string> to, string subject, string content, IReadOnlyDictionary<string, byte[]>? attachments = null, EmailMeta? meta = null)
        {
            From = @from;
            To = to;
            Subject = subject;
            Content = content;
            Attachments = attachments;
            Meta = meta;
            new Validator().ValidateAndThrow(this);
        }

        private class Validator : AbstractValidator<EmailMessage> {
            public Validator()
            {
                RuleFor(x => x.From).NotNull().EmailAddress();
                RuleFor(x => x.To).NotNull().ForEach(x => x.NotNull().EmailAddress());
                RuleFor(x => x.Subject).NotNull().NotEmpty();
                RuleFor(x => x.Content).NotNull().NotEmpty();
            }
        }
    }

}
