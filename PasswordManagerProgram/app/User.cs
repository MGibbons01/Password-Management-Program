using System;

namespace PasswordManagerProgram
{
    public abstract class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string AccountPassword { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public PasswordVault Vault { get; set; }

        public User(int userid, string username, string accountPassword, AccessLevel accesslevel, CredentialsDBManager credDb)
        {
            UserId = userid;
            Username = username;
            AccountPassword = accountPassword;
            AccessLevel = accesslevel;
            Vault = new PasswordVault(credDb, userid);
        }

        public abstract void AddCredential(Credentials credentials);

        public virtual void GetAllCredentials()
        {
            foreach (var credential in Vault.GetAllCredentials())
            {
                Console.WriteLine(credential);
            }
        }

        public virtual void FilterCredentials(string category)
        {
            var filtered = Vault.FilterByCategory(category);

            Console.WriteLine($"\nCredentials in category: {category}");

            Console.WriteLine("\n=========================== STORED CREDENTIALS ===========================");
                            Console.WriteLine("| {0,-15} | {1,-15} | {2,-20} | {3,-12} |", "Site", "Username", "Password", "Category");
                            Console.WriteLine(new string('-', 75));

                            foreach (var cred in filtered)
                            {
                                Console.WriteLine("| {0,-15} | {1,-15} | {2,-20} | {3,-12} |",
                                    cred.Site,
                                    cred.Username,
                                    cred.GetPassword(),
                                    cred.PasswordCategory);
                            }
                            Console.WriteLine(new string('-', 75));
        }
    }
}