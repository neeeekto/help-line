namespace HelpLine.Modules.UserAccess.Domain.Users.Contracts
{
    public interface IPasswordManager
    {
        public string Protect(string password);
        public bool Check(string currentPasswordStr, string providedPassword);
    }
}
