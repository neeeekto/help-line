using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems
{
    public class TemporaryProblemInfo : ValueObject
    {
        public string Title { get; }
        public string Description { get; }
        public string InitStatus { get; }

        public TemporaryProblemInfo(string title, string description, string initStatus)
        {
            Title = title;
            Description = description;
            InitStatus = initStatus;
        }
    }
}
