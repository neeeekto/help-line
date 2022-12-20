using System;
using System.Collections.Generic;
using System.Linq;
using HelpLine.Services.Files.Contracts;

namespace HelpLine.Services.Files.Application.Queries.GetFilesLink
{
    public class GetFilesLinkQuery : ICommand<IReadOnlyDictionary<string, string>>
    {
        public IEnumerable<string> FilesIds { get; }
        public TimeSpan Duration { get; }

        public GetFilesLinkQuery(IEnumerable<string> filesIds, TimeSpan duration)
        {
            Duration = duration;
            FilesIds = filesIds.Distinct();
        }
    }
}
