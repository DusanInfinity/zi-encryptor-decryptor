using System;
using System.Text;

namespace ZAD3.App.Hash
{
    public class TigerHash
    {
        private ulong H0;
        private ulong H1;
        private ulong H2;

        private ulong A;
        private ulong B;
        private ulong C;

        private ulong[] w; // UNSIGNED Long - 64bit

        private void Initialize()
        {
            H0 = 0x0123456789ABCDEFUL; // 0x0123456789ABCDEF
            H1 = 0xFEDCBA9876543210UL; // 0xFEDCBA9876543210
            H2 = 0xF096A5B4C3B2E187UL; // 0xF096A5B4C3D2E187

            A = B = C = 0;
            w = new ulong[8];
        }

        public string Hash(string text)
        {
            Encoding enc_default = Encoding.Unicode;
            byte[] input = enc_default.GetBytes(text);
            byte[] output = Hash(input, input.Length);

            char[] outarrchar = new char[enc_default.GetCharCount(output, 0, output.Length)];
            enc_default.GetChars(output, 0, output.Length, outarrchar, 0);
            return new string(outarrchar);
        }

        public byte[] Hash(byte[] array, int numOfBytes)
        {
            Initialize();

            int numOfMessageChunks = numOfBytes / 64; // delimo sa 64 bajta (64*8bit=512bit) - 512 bitni chunk
            for (int i = 0; i < numOfMessageChunks; i++)
            {
                byte[] messageChunk = new byte[64]; // chunk - 512bit(64 bajta)
                Array.Copy(array, i * 64, messageChunk, 0, 64);
                HashMessageChunk(messageChunk);
            }

            int numOfFinalBytes = numOfBytes % 64; // 64bajta = 512bita - ako imamo ostatak pri deljenju to znaci da imamo jos jedan nepotpuni chunk
            if (numOfFinalBytes > 0)
            { // mogli smo i prvo da prosirimo do umnoska od 512 pa sve chunkove da obradimo odjednom
                byte[] finalBytes = new byte[numOfFinalBytes];
                Array.Copy(array, numOfMessageChunks * 64, finalBytes, 0, numOfFinalBytes);
                HashFinalMessageBlock(finalBytes, numOfFinalBytes, numOfBytes);
            }


            // digest
            byte[] digest = ULongToBytes(H0);
            byte[] h1 = ULongToBytes(H1);
            byte[] h2 = ULongToBytes(H2);
            AppendBytes(ref digest, h1);
            AppendBytes(ref digest, h2);
            return digest;
        }

        private void HashMessageChunk(byte[] messageChunk)
        {
            A = H0;
            B = H1;
            C = H2;

            // w - 8 segmenta sa 64bitnim recima
            for (int i = 0; i < 8; i++)
                w[i] = BitConverter.ToUInt64(messageChunk, i * 8);

            // Glavna petlja
            for (int i = 0; i < 4; i++)
            {
                // Svaka runda u okviru grupe
                for (int j = 0; j < 8; j++)
                {
                    C ^= w[j];
                    byte[] cb = ULongToBytes(C); // c (64-bitna reč) se podeli na 8 bajtova: c0, c1, …, c7
                    A -= SBox.S0[cb[0]] ^ SBox.S1[cb[2]] ^ SBox.S2[cb[4]] ^ SBox.S3[cb[6]];
                    B += SBox.S3[cb[1]] ^ SBox.S2[cb[3]] ^ SBox.S1[cb[5]] ^ SBox.S0[cb[7]];
                    B *= (ulong)(i + 1);
                }

                KeySchedule(i);
            }

            H0 += A;//^= A;
            H1 += B;//-= B;
            H2 += C;
        }

        private void KeySchedule(int i)
        {
            if (i == 0)
            {
                // w XOR 0xFFFFFFFFFFFFFFFF je ustvari komplement w(~w), gde su nule bile sada su jedinice, gde su jedinice sada su nule

                w[0] -= w[7] ^ 0xA5A5A5A5A5A5A5A5UL;
                w[1] ^= w[0];
                w[2] += w[1];
                w[3] -= w[2] ^ (~w[1] << 19); // w[3] = w[3] – (w[2] XOR ((w[1] XOR 0xFFFFFFFFFFFFFFFF)<< 19))
                w[4] ^= w[3];
                w[5] += w[4];
                w[6] -= w[5] ^ (~w[4] >> 23); // w[6] – (w[5] XOR ((w[4] XOR 0xFFFFFFFFFFFFFFFF)>> 23))
                w[7] ^= w[6];
            }
            else if (i == 1)
            {
                w[0] += w[7];
                w[1] -= w[0] ^ ~w[7] << 19; // w[1] – (w[0] XOR ((w[0] XOR 0xFFFFFFFFFFFFFFFF)<< 19))
                w[2] ^= w[1];
                w[3] += w[2];
                w[4] -= w[3] ^ ~w[2] >> 23; // w[4] – (w[3] XOR ((w[2] XOR 0xFFFFFFFFFFFFFFFF)>> 23))
                w[5] ^= w[4];
                w[6] += w[5];
                w[7] -= w[6] ^ 0x0123456789ABCDEFUL;
            }
        }


        private void HashFinalMessageBlock(byte[] finalBytes, int numOfFinalBytes, int numOfUnpaddedBytes)
        {

            if (numOfFinalBytes < 56) // 56 bajta * 8 bita = 448 bita (ostavljamo poslednja 64 bita za ulong - broj bajtova bez nadovezivanja izrazen u broju bitova)
            {
                int paddingSize = 56 - numOfFinalBytes;
                byte[] padding = new byte[paddingSize];
                Array.Clear(padding, 0, paddingSize);
                padding[0] = 0x80; // jedinica na pocetku, sve ostalo nule

                AppendBytes(ref finalBytes, padding);

                ulong msgSize = (ulong)(numOfUnpaddedBytes * 8); // bit (bit, not byte) length of unpadded message as 64-bit little-endian integer to message
                byte[] messageSizeBytes = ULongToBytes(msgSize);
                AppendBytes(ref finalBytes, messageSizeBytes);

                HashMessageChunk(finalBytes);
            }
            else // dopunjavamo poslednji chunk jedinicom i nulama i dodajemo novi chunk sa brojem bajtova bez nadovezivanja izrazenim u broj bitova
            {
                // Dopuna chunk-a do kraja
                int paddingSize = 64 - numOfFinalBytes;
                byte[] paddingMsg1 = new byte[paddingSize];
                Array.Clear(paddingMsg1, 0, paddingSize);
                paddingMsg1[0] = 0x80;
                AppendBytes(ref finalBytes, paddingMsg1);

                HashMessageChunk(finalBytes);

                // Novi chunk - prvih 56 bajta sve jedinice
                paddingSize = 56;
                byte[] paddingMsg2 = new byte[paddingSize];
                Array.Clear(paddingMsg2, 0, paddingSize);
                // Poslednja 8 bajta velicina u bitovima
                ulong msgSize = (ulong)(numOfUnpaddedBytes * 8);
                byte[] messageSizeBytes = ULongToBytes(msgSize);
                AppendBytes(ref paddingMsg2, messageSizeBytes);

                HashMessageChunk(paddingMsg2);
            }
        }

        private static byte[] ULongToBytes(ulong number)
        {
            byte[] result = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                result[i] = (byte)(number & 0x00000000000000FF);
                number = number >> 8;
            }

            return result;
        }

        private static void AppendBytes(ref byte[] dst, byte[] src)
        {
            int oldLength = dst.Length;
            Array.Resize(ref dst, dst.Length + src.Length);
            for (int j = 0; j < src.Length; j++)
                dst[oldLength + j] = src[j];
        }
    }
}
