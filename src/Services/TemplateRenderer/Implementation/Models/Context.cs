using System;
using System.Collections.Generic;

namespace HelpLine.Services.TemplateRenderer.Models
{
    public class Context
    {
        public string Id { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string? Group { get; set; } // only for client, humanize name
        public object Data { get; set; }
        public string? Alias { get; set; }
        public string? Extend { get; set; }
    }
}
