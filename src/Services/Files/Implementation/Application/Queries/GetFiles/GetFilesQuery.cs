using System;
using System.Collections.Generic;
using System.Linq;
using HelpLine.Services.Files.Application.Views;
using HelpLine.Services.Files.Contracts;

namespace HelpLine.Services.Files.Application.Queries.GetFiles
{
    public class GetFilesQuery : ICommand<IReadOnlyDictionary<string, FileView>>
    {
        public IEnumerable<string> FilesIds { get; }
        public TimeSpan Duration { get; }


        public GetFilesQuery(IEnumerable<string> filesIds, TimeSpan duration)
        {
            FilesIds = filesIds.Distinct().ToArray();
            Duration = duration;
        }
    }
}
