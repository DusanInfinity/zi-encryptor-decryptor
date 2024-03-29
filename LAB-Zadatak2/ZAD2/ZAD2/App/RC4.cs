﻿using System;
using System.Text;
using ZAD2.App;

namespace ZAD1
{
    class RC4 : ICipherAlgorithm
    {
        private string EncryptionKey = "";
        private const int KeyLen = 128; // ASCII tabela - 7bit
        private readonly byte[] ArrayS = new byte[KeyLen]; // niz za sve moguce kombinacije naseg kljuca


        public string PlainText { get; set; } = "";
        public string CryptedText { get; set; } = "";

        public RC4(string key = "sdgwabhe46wv3465b34h63vwcac53QWb")
        {
            SetEncriptionKey(key);
        }

        public bool Encrypt()
        {
            try
            {
                Encoding enc_default = Encoding.Unicode;
                byte[] input = enc_default.GetBytes(PlainText);
                byte[] output = new byte[input.Length];

                byte[] S = new byte[KeyLen];
                this.ArrayS.CopyTo(S, 0);

                int i = 0, j = 0;
                for (int k = 0; k < input.Length; k++)
                {
                    i = (i + 1) % KeyLen;
                    j = (j + S[i]) % KeyLen;

                    // swap(i,j)
                    byte temp = S[i];
                    S[i] = S[j];
                    S[j] = temp;

                    byte a = input[k];
                    byte key = S[(S[i] + S[j]) % KeyLen];
                    output[k] = (byte)(a ^ key); // vrednost na k-toj poziciji u plaintextu XOR vrednost izracunatog kljuca
                }

                char[] outarrchar = new char[enc_default.GetCharCount(output, 0, output.Length)];
                enc_default.GetChars(output, 0, output.Length, outarrchar, 0);
                CryptedText = new string(outarrchar);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Decrypt()
        {
            try
            {
                PlainText = CryptedText;
                CryptedText = "";

                if (Encrypt())
                    PlainText = CryptedText;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GenerateAndSetNewKey()
        {
            string key = Guid.NewGuid().ToString();
            SetEncriptionKey(key);
            return key;
        }

        public void SetEncriptionKey(string value)
        {
            if (EncryptionKey == value) return;

            EncryptionKey = value;

            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;

            // Prevodjenje iz unicode u ASCII
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicode.GetBytes(EncryptionKey));
            char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);

            // Inicijalizacija kljuca (S)
            for (int count = 0; count < KeyLen; count++)
                ArrayS[count] = (byte)count;

            int j = 0;
            int encryptionKeyLength = asciiChars.Length;
            for (int i = 0; i < KeyLen; i++)
            {
                j = (j + ArrayS[i] + asciiChars[i % encryptionKeyLength]) % (KeyLen); // staro j + S[i] + komponenta kljuca na i-toj poziciji

                // swap (i, j)
                byte temp = ArrayS[i];
                ArrayS[i] = ArrayS[j];
                ArrayS[j] = temp;
            }
        }
    }
}
