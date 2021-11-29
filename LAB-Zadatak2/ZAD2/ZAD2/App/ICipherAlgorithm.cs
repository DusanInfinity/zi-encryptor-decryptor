namespace ZAD2.App
{
    interface ICipherAlgorithm
    {
        public string PlainText { get; set; }
        public string CryptedText { get; set; }

        public bool Encrypt();
        public bool Decrypt();

        public void SetEncriptionKey(string value);
        public string GenerateAndSetNewKey();

    }
}
