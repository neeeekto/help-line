using System;
using System.Collections.Generic;

namespace HelpLine.Services.TemplateRenderer.Models
{
    public class Component
    {
        public string Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Group { get; set; }
        public string Content { get; set; }
        public object? Meta { get; set; } // Only for client engine IDE
    }
}
