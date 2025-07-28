using System.Collections.Generic;
using System.Linq;

namespace PasswordManagerProgram
{
    public class PasswordVault
    {
        private readonly List<Credentials> credentials;
        private readonly CredentialsDBManager credDb;

        public PasswordVault(CredentialsDBManager credentialsDb, int userid)
        {
            this.credDb = credentialsDb;
            credentials = credDb.LoadAllCredentials(userid); // Initial load from DB
        }

        public void AddCredential(Credentials cred)
        {
            credentials.Add(cred);
            credDb.SaveCredential(cred); // Persist to DB
        }
       
        public Credentials FindBySite(string site)
        {
            //find the c (credentail) thats the same as the search
            return credentials.Find(c => c.Site.ToLower() == site.ToLower());
        }

        public List<Credentials> FilterByCategory(string category)
        {
            return credentials
                .Where(c => c.PasswordCategory.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Credentials> GetAllCredentials()
        {
            return new List<Credentials>(credentials);
        }

        public bool RemoveCredential(string site)
        {
            var cred = FindBySite(site);
            if (cred != null)
            {
                credentials.Remove(cred);
                credDb.DeleteCredential(site);
                return true;
            }
            return false;
        }
    }
}
