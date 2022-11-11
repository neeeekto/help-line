using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Operators.ViewModels
{
    public class OperatorView
    {
        public Guid Id { get; internal set; }
        public IEnumerable<string> Favorite { get; internal set; }
        public IReadOnlyDictionary<string, IEnumerable<Guid>> Roles { get; internal set; }
    }
}
