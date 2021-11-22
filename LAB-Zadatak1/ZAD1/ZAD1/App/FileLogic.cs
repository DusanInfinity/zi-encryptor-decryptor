using System.IO;

namespace ZAD1
{
    class FileLogic
    {
        private readonly RC4 rc4 = new RC4();
        private string KeysPath;

        public FileLogic(string keysPath)
        {
            KeysPath = keysPath;
        }

        public void SetKeysPath(string path)
        {
            KeysPath = path;
        }

        public void EncryptFile(string fileName, string destDirectory)
        {
            string plainText = LoadTextFromFile(fileName);

            rc4.PlainText = plainText;
            string generatedKey = rc4.GenerateNewKey();
            if (rc4.Encrypt())
            {
                string encryptedText = rc4.CryptedText;

                string fileNameForDest = Path.GetFileName(fileName);
                SaveTextToFile(encryptedText, Path.Combine(destDirectory, fileNameForDest));

                string keyFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.key";
                SaveTextToFile(generatedKey, Path.Combine(destDirectory, keyFileName));
            }
        }

        public string DecryptFile(string fileName)
        {
            string encryptedText = LoadTextFromFile(fileName);

            string key = LoadKeyForFile(fileName);
            rc4.SetEncriptionKey(key);
            rc4.CryptedText = encryptedText;
            if (rc4.Decrypt())
                return rc4.PlainText;

            return "ERROR";
        }

        public string LoadKeyForFile(string filePath)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);
            string keyPath = Path.Combine(KeysPath, $"{name}.key");
            return LoadTextFromFile(keyPath);
        }


        public static void SaveTextToFile(string text, string destFileName)
        {
            if (!string.IsNullOrEmpty(destFileName))
            {
                using StreamWriter sw = new StreamWriter(destFileName, false);
                sw.Write(text);
                sw.Close();
            }
        }

        public static string LoadTextFromFile(string fileName)
        {
            using StreamReader sr = new StreamReader(fileName);
            string text = sr.ReadToEnd();
            sr.Close();

            return text;
        }
    }
}
