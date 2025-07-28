using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace PasswordManagerProgram
{
    public class CredentialsDBManager
    {
        private readonly string connectionString;
        private readonly IEncryptable encryptor;

        public CredentialsDBManager(string connStr, IEncryptable encryptor)
        {
            connectionString = connStr;
            this.encryptor = encryptor;
        }

        public void SaveCredential(Credentials cred)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = @"INSERT INTO credentials (user_id, site, username, encrypted_password, category)
                             VALUES (@userId, @site, @username, @password, @category)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", cred.UserId);
            cmd.Parameters.AddWithValue("@site", cred.Site);
            cmd.Parameters.AddWithValue("@username", cred.Username);
            cmd.Parameters.AddWithValue("@password", cred.EncryptedPassword);
            cmd.Parameters.AddWithValue("@category", cred.PasswordCategory);
            cmd.ExecuteNonQuery();
        }

        public List<Credentials> LoadAllCredentials(int userid)
        {
            var result = new List<Credentials>();
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT site, username, encrypted_password, category FROM credentials WHERE user_id = @userId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userid);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string site = reader.GetString("site");
                string username = reader.GetString("username");
                string encryptedPassword = reader.GetString("encrypted_password");
                string category = reader.GetString("category");

                var cred = new Credentials(site, username, encryptedPassword, category, encryptor, userid);
                cred.EncryptedPassword = encryptedPassword;
                result.Add(cred);
            }

            return result;
        }

        public void DeleteCredential(string site)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "DELETE FROM credentials WHERE site = @site";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@site", site);
            cmd.ExecuteNonQuery();
        }
    }
}
