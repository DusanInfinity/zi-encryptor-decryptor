using System;

namespace ZAD2.App
{
    public abstract class CipherAlgorithm
    {
        public string PlainText { get; set; } = "";
        public string CryptedText { get; set; } = "";
        public bool CTRModeActive { get; set; } = false;
        public byte InitialCounter { get; set; }

        public abstract bool Encrypt();
        public abstract bool Decrypt();

        public abstract void SetEncriptionKey(string value);
        public virtual Tuple<string, string> GenerateAndSetNewKey()
        {
            string key = "sdgwabhe46wv3465b34h63vwcac53QWb";//Guid.NewGuid().ToString();
            string ctr = "";

            SetEncriptionKey(key);
            InitialCounter = (byte)(new Random().Next(0, 128));
            if (CTRModeActive)
                ctr = InitialCounter.ToString();

            return new Tuple<string, string>(key, ctr);
        }

        public void SetInitialCTR(string ctrString)
        {
            if (ctrString.Length == 0)
                throw new Exception("Pokušavate da dekriptujete sa aktivnim CTR modom fajl koji prethodno nije enkriptovan u CTR modu!");

            InitialCounter = Convert.ToByte(ctrString);
        }

    }
}
