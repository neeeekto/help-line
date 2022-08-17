using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems
{
    public class TemporaryProblemUpdateContent : ValueObject
    {
        public string Title { get; }
        public string Message { get; }

        public TemporaryProblemUpdateContent(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
