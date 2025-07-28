using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace PasswordManagerProgram
{
    public class UserDBManager
    {
        private readonly string connectionString;
        private readonly IEncryptable encryptor;

        public UserDBManager(string connStr, IEncryptable encryptor)
        {
            connectionString = connStr;
            this.encryptor = encryptor;
        }

        // create user
        public bool SaveUser(User user)
            {
                using var conn = new MySqlConnection(connectionString);
                conn.Open();

                string query = "INSERT INTO User (username, account_password, access_level) VALUES (@username, @password, @access_level); SELECT LAST_INSERT_ID();";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.AccountPassword);
                cmd.Parameters.AddWithValue("@access_level", user.AccessLevel.ToString());


                try
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        user.UserId = Convert.ToInt32(result);
                    }
                    return true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Username already taken. Please choose another.");
                    return false;
                }
            }

        //used for log in
        public User ValidateUser(string username, string password)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT id, account_password, access_level FROM User WHERE username = @username";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string storedPassword = reader.GetString("account_password");
                string accessLevel = reader.GetString("access_level");
                int userId = reader.GetInt32("id");
                if (storedPassword == password)
                {
                    var credDb = new CredentialsDBManager(connectionString, encryptor);
                    if (accessLevel == "Admin")
                    {
                        return new AdminUser(userId, username, password, this, credDb);
                    }
                    else
                    {
                        return new StandardUser(userId, username, password, this, credDb);
                    }
                }
            }
            return null;
        }
    }
}