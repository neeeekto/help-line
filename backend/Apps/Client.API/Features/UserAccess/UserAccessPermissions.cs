using HelpLine.Apps.Client.API.Configuration.PermissionInfo;

namespace HelpLine.Apps.Client.API.Features.UserAccess
{
    [PermissionSource]
    public static class UserAccessPermissions
    {
        [PermissionName("Users")]
        public const string UserAccess = "UserAccess";

        [PermissionsDependOn(UserAccess)]
        [PermissionName("Roles")]
        public const string Roles = "UserAccess.Roles";

        [PermissionsDependOn(Roles)]
        public const string CreateRole = "UserAccess.Roles.Create";

        [PermissionsDependOn(Roles)]
        public const string DeleteRole = "UserAccess.Roles.Delete";

        [PermissionsDependOn(Roles)]
        public const string UpdateRole = "UserAccess.Roles.Update";

        [PermissionsDependOn(Roles)]
        public const string ViewRole = "UserAccess.Roles.View";

        [PermissionsDependOn(UserAccess)]
        [PermissionName("Users")]
        public const string Users = "UserAccess.Users";

        [PermissionsDependOn(Users)]
        public const string CreateUser = "UserAccess.Users.Create";

        [PermissionsDependOn(Users)]
        public const string DeleteUser = "UserAccess.Users.Delete";

        [PermissionsDependOn(Users)]
        public const string UpdateUser = "UserAccess.Users.Update";

        [PermissionsDependOn(Users)]
        public const string ViewUser = "UserAccess.Users.View";

        [PermissionsDependOn(Users)]
        [PermissionName("Roles and Permissions")]
        public const string UpdateUserRolesAndPermissions = "UserAccess.Users.UpdateRolesAndPermissions";

        [PermissionsDependOn(Users)]
        [PermissionName("Set projects")]
        public const string SetUserProjects = "UserAccess.Users.SetProjects";

        [PermissionsDependOn(Users)]
        [PermissionName("Set Password")]
        public const string SetUserPassword = "UserAccess.Users.SetPassword";

    }
}
