using System;
using System.Collections.Generic;
using System.Dynamic;

namespace HelpLine.Services.TemplateRenderer.Models
{
    public class Template
    {
        public string Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Name { get; set; } // Using! See client

        public string? Group { get; set; } // Group on client (only for view)
        public IEnumerable<string> Contexts { get; set; }
        public object? Props { get; set; }
        public string Content { get; set; }
        public object? Meta { get; set; } // Only for client engine IDE
    }
}
