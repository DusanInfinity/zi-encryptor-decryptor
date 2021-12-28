using System;

namespace ZAD2.App
{
    public abstract class CipherAlgorithm
    {
        public string PlainText { get; set; } = "";
        public string CryptedText { get; set; } = "";
        public bool CTRModeActive { get; set; } = false;
        public byte[] InitialCounter { get; set; }

        public abstract bool Encrypt();
        public abstract bool Decrypt();

        public abstract void SetEncriptionKey(string value);
        public abstract Tuple<string, string> GenerateAndSetNewKey();

        public void SetInitialCTR(string ctrString)
        {
            if (ctrString.Length == 0)
                throw new Exception("Pokušavate da dekriptujete sa aktivnim CTR modom fajl koji prethodno nije enkriptovan u CTR modu!");

            string[] strgs = ctrString.Split(" ");

            int bytesNum = strgs.Length;
            InitialCounter = new byte[bytesNum];
            for (int i = 0; i < bytesNum; i++)
                InitialCounter[i] = Convert.ToByte(strgs[i]);
        }
    }
}
