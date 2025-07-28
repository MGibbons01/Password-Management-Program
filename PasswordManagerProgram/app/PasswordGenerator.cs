using System;

namespace PasswordManagerProgram
{
    public class PasswordGenerator
    { 
        public static string GenerateStrongPassword(int length = 12)
        {
        const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
        Random rnd = new Random();
        char[] passwordChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            passwordChars[i] = letters[rnd.Next(letters.Length)];
        }

        return new string(passwordChars);
        }

    }
}