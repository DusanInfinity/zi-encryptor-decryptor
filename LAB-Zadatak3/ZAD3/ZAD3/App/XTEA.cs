using System;
using System.IO;
using System.Text;

namespace ZAD2.App
{
    // Za implementaciju ovog algoritma su koriscene informacije sa sledecih linkova/dokumenata:
    // - TEA algoritam sa predavanja
    // - https://en.wikipedia.org/wiki/XTEA
    // - https://gist.github.com/InfectedBytes/ff8d5de8592bfc711380801cb29b3915
    public class XTEA : CipherAlgorithm
    {
        private readonly Encoding encoding = Encoding.Unicode;
        private readonly uint[] K = new uint[4]; // 128bit kljuc
        private readonly uint delta = 0x9E3779B9;

        public XTEA(string key = "sdgwabhe46wv3465b34h63vwcac53QWb")
        {
            SetEncriptionKey(key);
        }

        public override bool Encrypt()
        {
            try
            {
                byte[] data = encoding.GetBytes(PlainText);
                byte[] result = new byte[NextMultipleOf8(data.Length + 4)];

                // Injektovanje velicine (broja bajtova plaintext-a) u rezultujuci bafer
                byte[] lengthBuffer = BitConverter.GetBytes(data.Length);
                Array.Copy(lengthBuffer, result, lengthBuffer.Length);
                Array.Copy(data, 0, result, lengthBuffer.Length, data.Length);

                if (CTRModeActive)
                    EncryptWithCTRMode(result);
                else
                    Encrypt(result);


                CryptedText = Convert.ToBase64String(result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void EncryptWithCTRMode(byte[] result)
        {
            uint[] block_V = new uint[2];
            byte[] counter = new byte[8];
            Array.Copy(InitialCounter, counter, 8);

            using (var stream = new MemoryStream(result))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    for (int i = 0; i < result.Length; i += 8) // po 8 bajta - 64bit
                    {
                        block_V[0] = BitConverter.ToUInt32(counter, 0); // prva 4 bajta COUNTERA
                        block_V[1] = BitConverter.ToUInt32(counter, 4); // druga 4 bajta COUNTERA
                        Encrypt(block_V, K);
                        uint firstByte = block_V[0] ^ BitConverter.ToUInt32(result, i);
                        uint secondByte = block_V[1] ^ BitConverter.ToUInt32(result, i + 4);
                        writer.Write(firstByte);
                        writer.Write(secondByte);

                        IncrementByteArray(counter); // Inkrementiranje brojaca
                    }
                }
            }
        }


        private void Encrypt(byte[] result)
        {
            uint[] block_V = new uint[2];
            using (var stream = new MemoryStream(result))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    for (int i = 0; i < result.Length; i += 8) // po 8 bajta - 64bit
                    {
                        block_V[0] = BitConverter.ToUInt32(result, i); // prva 4 bajta (32bit)
                        block_V[1] = BitConverter.ToUInt32(result, i + 4); // druga 4 bajta (32bit) - ukupno 64bit
                        Encrypt(block_V, K);
                        writer.Write(block_V[0]);
                        writer.Write(block_V[1]);
                    }
                }
            }
        }
        private void Encrypt(uint[] v, uint[] key)
        {
            uint v0 = v[0], v1 = v[1], sum = 0;
            for (uint i = 0; i < 32; i++) // 32 runde = 64 feistelovih 
            {
                v0 += (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + key[sum & 3]);
                sum += delta;
                v1 += (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + key[(sum >> 11) & 3]);
            }
            v[0] = v0;
            v[1] = v1;
        }

        public override bool Decrypt()
        {
            try
            {
                byte[] data = Convert.FromBase64String(CryptedText);

                byte[] encryptedMsg = new byte[data.Length];
                Array.Copy(data, encryptedMsg, data.Length);

                if (CTRModeActive)
                    DecryptWithCTRMode(encryptedMsg);
                else
                    Decrypt(encryptedMsg);


                uint length = BitConverter.ToUInt32(encryptedMsg, 0);
                if (length > encryptedMsg.Length - 4) // nevalidna enkripcija
                    return false;

                byte[] result = new byte[length];
                Array.Copy(encryptedMsg, 4, result, 0, length);

                PlainText = encoding.GetString(result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DecryptWithCTRMode(byte[] encryptedMsg)
        {
            uint[] block_V = new uint[2];
            byte[] counter = new byte[8];
            Array.Copy(InitialCounter, counter, 8);

            using (var stream = new MemoryStream(encryptedMsg))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    for (int i = 0; i < encryptedMsg.Length; i += 8) // po 8 bajta - 64bit
                    {
                        block_V[0] = BitConverter.ToUInt32(counter, 0); // prva 4 bajta COUNTERA
                        block_V[1] = BitConverter.ToUInt32(counter, 4); // druga 4 bajta COUNTERA
                        Encrypt(block_V, K); // ENKRIPCIJA!!!! Ne treba dekripcija -> https://en.wikipedia.org/wiki/File:CTR_decryption_2.svg
                        uint firstByte = block_V[0] ^ BitConverter.ToUInt32(encryptedMsg, i);
                        uint secondByte = block_V[1] ^ BitConverter.ToUInt32(encryptedMsg, i + 4);
                        writer.Write(firstByte);
                        writer.Write(secondByte);

                        IncrementByteArray(counter); // Inkrementiranje brojaca
                    }
                }
            }
        }

        public void Decrypt(byte[] encryptedMsg)
        {
            uint[] block_V = new uint[2];
            using (var stream = new MemoryStream(encryptedMsg))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    for (int i = 0; i < encryptedMsg.Length; i += 8) // po 8 bajta - 64bit
                    {
                        block_V[0] = BitConverter.ToUInt32(encryptedMsg, i); // prva 4 bajta (32bit)
                        block_V[1] = BitConverter.ToUInt32(encryptedMsg, i + 4); // druga 4 bajta (32bit) - ukupno 64bit
                        Decrypt(block_V, K);
                        writer.Write(block_V[0]);
                        writer.Write(block_V[1]);
                    }
                }
            }
        }

        private void Decrypt(uint[] v, uint[] key)
        {
            uint v0 = v[0], v1 = v[1], sum = 32 * delta;
            for (uint i = 0; i < 32; i++) // 32 runde = 64 feistelovih 
            {
                v1 -= (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + key[(sum >> 11) & 3]);
                sum -= delta;
                v0 -= (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + key[sum & 3]);
            }
            v[0] = v0;
            v[1] = v1;
        }

        public override void SetEncriptionKey(string value)
        {
            byte[] key = encoding.GetBytes(value);
            byte[] hash = new byte[16];

            for (int i = 0; i < key.Length; i++)
                hash[i % 16] = (byte)((31 * hash[i % 16]) ^ key[i]); // (31*trenutniHashBajt) XOR uneti kljuc

            // Ukoliko je kljuc manji od potrebnog (16 bajtova)
            for (int i = key.Length; i < 16; i++)
                hash[i] = (byte)(17 * i ^ key[i % key.Length]);

            for (int i = 0; i < 4; i++)
                K[i] = BitConverter.ToUInt32(hash, i * 4);
        }

        private static int NextMultipleOf8(int length)
        {
            // XTEA blok = 64bit
            return (length + 7) / 8 * 8; // dobijamo broj deljiv sa 8
        }

        public override Tuple<string, string> GenerateAndSetNewKey()
        {
            string key = "sdgwabhe46wv3465b34h63vwcac53QWb";//Guid.NewGuid().ToString(); // GENERISANJE KLJUCA ISKLJUCENO ZBOG TESTIRANJA CTR moda
            string ctr = "";

            SetEncriptionKey(key);
            InitialCounter = new byte[8];
            for (int i = 0; i < 8; i++)
                InitialCounter[i] = (byte)(new Random().Next(0, 256));

            if (CTRModeActive)
                ctr = string.Join(" ", InitialCounter);

            return new Tuple<string, string>(key, ctr);
        }

        private static void IncrementByteArray(byte[] array)
        {
            int bytesNum = array.Length;
            bool overflow = true;
            for (int i = 0; i < bytesNum; i++)
            {
                if (array[i] != 255)
                {
                    array[i]++;
                    overflow = false;
                    break;
                }
            }

            if (overflow == true)
                for (int i = 0; i < bytesNum; i++)
                    array[i] = 0;
        }
    }
}
