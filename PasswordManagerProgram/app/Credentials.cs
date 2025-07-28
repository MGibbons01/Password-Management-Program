using System;

namespace PasswordManagerProgram
{
    public class Credentials
    {
        public string Site { get; set; }
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }
        public string PasswordCategory { get; set; }
        public int UserId { get; set; }

        private readonly IEncryptable encryptor;

        public Credentials(string site, string username, string password, string passwordCategory, IEncryptable encryptor, int userId)
        {
            Site = site;
            Username = username;
            PasswordCategory = passwordCategory;
            this.encryptor = encryptor;
            SetPassword(password); // Encrypt and store
            UserId = userId;
        }

        public string GetPassword()
        {
            return encryptor.Decrypt(EncryptedPassword);
        }
        
        public void SetPassword(string input)
        {
            EncryptedPassword = encryptor.Encrypt(input);
        }
    }
}