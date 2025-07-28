using System;

namespace PasswordManagerProgram
{
    public class StandardUser : User
    { public StandardUser(int userid, string username, string accountPassword, UserDBManager userDb, CredentialsDBManager credDb)
            : base(userid, username, accountPassword, AccessLevel.Standard, credDb)
        {
        }

        public override void AddCredential(Credentials credential)
        {
            Vault.AddCredential(credential);
        }
    }
}