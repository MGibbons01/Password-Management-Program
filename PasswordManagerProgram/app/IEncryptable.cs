using System;

    namespace PasswordManagerProgram
    {
    public interface IEncryptable
    {
        string Encrypt(string input);
        string Decrypt(string input);

    }
}
