using System;

namespace PasswordManagerProgram
{
    public class AdminUser : User
    {
       public AdminUser(int userid, string username, string accountPassword, UserDBManager userDb, CredentialsDBManager credDb)
            : base(userid, username, accountPassword, AccessLevel.Admin, credDb)
        {
        }

        public override void AddCredential(Credentials credential)
        {
            Vault.AddCredential(credential);
        }

        public bool DeleteCredential(string site)
        {
            return Vault.RemoveCredential(site);
        }
    }
}