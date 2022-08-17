using HelpLine.Apps.Client.API.Configuration.PermissionInfo;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Permissions
{
    [PermissionSource]
    internal class HelpdeskPermissions
    {
        public const string Helpdesk = "Helpdesk";

        [PermissionSource]
        internal static class Tickets
        {
            public const string Root = "Helpdesk.Tickets";
            public const string List = "Helpdesk.Tickets.List";
            public const string Batch = "Helpdesk.Tickets.Batch";
        }

        [PermissionSource]
        internal static class Ticket
        {
            public const string Root = "Helpdesk.Ticket";
        }

        [PermissionSource]
        internal static class Filters
        {
            public const string Root = "Helpdesk.Filters";
            public const string Share = "Helpdesk.Filters.Share";
            public const string Admin = "Helpdesk.Filters.Admin";
        }

        [PermissionSource]
        internal static class Problems
        {
            public const string Root = "Helpdesk.Problems";
        }

        [PermissionSource]
        internal static class Automations
        {
            public const string Root = "Helpdesk.Automations";
        }

        [PermissionSource]
        internal static class Settings
        {
            public const string Root = "Helpdesk.Settings";
        }

        [PermissionSource]
        internal static class Other
        {
            public const string Root = "Helpdesk.Other";
        }

        [PermissionSource]
        internal static class Operators
        {
            public const string Root = "Helpdesk.Operators";
        }
    }
}
