using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems
{
    public class TemporaryProblemMeta : ValueObject
    {
        public string Name { get; }
        public IEnumerable<LanguageCode> Languages { get; }
        public IEnumerable<string> Platforms { get; }

        public TemporaryProblemMeta(string name, IEnumerable<LanguageCode> languages, IEnumerable<string> platforms)
        {
            Name = name;
            Languages = languages;
            Platforms = platforms;
        }
    }
}
