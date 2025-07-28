using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace PasswordManagerProgram
{
    public class Encryption : IEncryptable
    {
        private readonly byte[] key = Convert.FromHexString("00112233445566778899AABBCCDDEEFF");
        private readonly byte[] iv = Convert.FromHexString("AABBCCDDEEFF00112233445566778899");

        public string Encrypt(string input)
        {
            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            cipher.Init(true, new ParametersWithIV(new KeyParameter(key), iv));

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var output = new byte[cipher.GetOutputSize(inputBytes.Length)];
            var length = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, output, 0);
            cipher.DoFinal(output, length);

            return Convert.ToBase64String(output);
        }

        public string Decrypt(string input)
        {
            var cipherText = Convert.FromBase64String(input);

            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            cipher.Init(false, new ParametersWithIV(new KeyParameter(key), iv));

            var output = new byte[cipher.GetOutputSize(cipherText.Length)];
            var length = cipher.ProcessBytes(cipherText, 0, cipherText.Length, output, 0);
            cipher.DoFinal(output, length);

            return Encoding.UTF8.GetString(output).TrimEnd('\0');
        }

    }
}