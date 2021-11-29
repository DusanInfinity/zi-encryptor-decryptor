using System;
using System.IO;
using System.Text;

namespace ZAD2.App
{
    public class XTEA : ICipherAlgorithm
    {
        private readonly Encoding encoding = Encoding.Unicode;
        private uint[] K;

        public string PlainText { get; set; }
        public string CryptedText { get; set; }

        public XTEA(string key = "sdgwabhe46wv3465b34h63vwcac53QWb")
        {
            SetEncriptionKey(key);
        }

        public bool Encrypt()
        {
            try
            {
                byte[] data = encoding.GetBytes(PlainText);

                uint[] blockBuffer = new uint[2];
                byte[] result = new byte[NextMultipleOf8(data.Length + 4)];
                byte[] lengthBuffer = BitConverter.GetBytes(data.Length);
                Array.Copy(lengthBuffer, result, lengthBuffer.Length);
                Array.Copy(data, 0, result, lengthBuffer.Length, data.Length);
                using (var stream = new MemoryStream(result))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        for (int i = 0; i < result.Length; i += 8)
                        {
                            blockBuffer[0] = BitConverter.ToUInt32(result, i);
                            blockBuffer[1] = BitConverter.ToUInt32(result, i + 4);
                            Encrypt(blockBuffer, K);
                            writer.Write(blockBuffer[0]);
                            writer.Write(blockBuffer[1]);
                        }
                    }
                }

                CryptedText = Convert.ToBase64String(result);
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
                byte[] data = Convert.FromBase64String(CryptedText);

                var blockBuffer = new uint[2];
                var buffer = new byte[data.Length];
                Array.Copy(data, buffer, data.Length);
                using (var stream = new MemoryStream(buffer))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        for (int i = 0; i < buffer.Length; i += 8)
                        {
                            blockBuffer[0] = BitConverter.ToUInt32(buffer, i);
                            blockBuffer[1] = BitConverter.ToUInt32(buffer, i + 4);
                            Decrypt(blockBuffer, K);
                            writer.Write(blockBuffer[0]);
                            writer.Write(blockBuffer[1]);
                        }
                    }
                }
                // verify valid length
                var length = BitConverter.ToUInt32(buffer, 0);
                if (length > buffer.Length - 4) throw new ArgumentException("Invalid encrypted data");
                var result = new byte[length];
                Array.Copy(buffer, 4, result, 0, length);

                PlainText = encoding.GetString(result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetEncriptionKey(string value)
        {
            byte[] key = encoding.GetBytes(value);
            // It might be a better idea to just calculate the MD5 hash of the key: var hash = MD5.Create().ComputeHash(key);
            // But we don't want to depend on the Cryptography namespace, because it would increase the build size for some Unity3d platforms.
            var hash = new byte[16];
            for (int i = 0; i < key.Length; i++)
            {
                hash[i % 16] = (byte)((31 * hash[i % 16]) ^ key[i]);
            }
            for (int i = key.Length; i < hash.Length; i++)
            { // if key was too short
                hash[i] = (byte)(17 * i ^ key[i % key.Length]);
            }

            K = new uint[]
            {
                BitConverter.ToUInt32(hash, 0),
                BitConverter.ToUInt32(hash, 4),
                BitConverter.ToUInt32(hash, 8),
                BitConverter.ToUInt32(hash, 12)
            };
        }

        private static void Encrypt(uint[] v, uint[] key)
        {
            uint v0 = v[0], v1 = v[1], sum = 0, delta = 0x9E3779B9;
            for (uint i = 0; i < 32; i++)
            {
                v0 += (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + key[sum & 3]);
                sum += delta;
                v1 += (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + key[(sum >> 11) & 3]);
            }
            v[0] = v0;
            v[1] = v1;
        }

        private static void Decrypt(uint[] v, uint[] key)
        {
            uint v0 = v[0], v1 = v[1], delta = 0x9E3779B9, sum = delta * 32;
            for (uint i = 0; i < 32; i++)
            {
                v1 -= (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + key[(sum >> 11) & 3]);
                sum -= delta;
                v0 -= (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + key[sum & 3]);
            }
            v[0] = v0;
            v[1] = v1;
        }

        private static int NextMultipleOf8(int length)
        {
            // XTEA is a 64-bit block chiffre, therefore our data must be a multiple of 64 bit
            return (length + 7) / 8 * 8; // this will give us the next multiple of 8
        }

        public string GenerateAndSetNewKey()
        {
            string key = Guid.NewGuid().ToString();
            SetEncriptionKey(key);
            return key;
        }
    }
}
