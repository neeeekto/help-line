namespace HelpLine.Modules.UserAccess.Application.Contracts
{
    public static class CustomClaimTypes
    {
        public const string UserId = "userId";
        public const string FirstName = "firstName";
        public const string LastName = "lastName";
        public const string Photo = "photo";
        public const string Language = "language";
        public const string Permission = "permission";
        public const string IsAdmin = "isAdmin";
        public static string ByProject(string project) => $"{project}.{Permission}";
    }
}
