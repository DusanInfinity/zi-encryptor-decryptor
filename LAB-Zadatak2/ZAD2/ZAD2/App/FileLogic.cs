using System;
using System.IO;
using ZAD2.App;

namespace ZAD1
{
    class FileLogic
    {
        private ICipherAlgorithm Algorithm;
        private string KeysPath;

        public FileLogic(ICipherAlgorithm algorithm, string keysPath)
        {
            Algorithm = algorithm;
            KeysPath = keysPath;
        }

        public void SetKeysPath(string path)
        {
            KeysPath = path;
        }

        public void EncryptFile(string fileName, string destDirectory)
        {
            string plainText = LoadTextFromFile(fileName);

            Algorithm.PlainText = plainText;
            string generatedKey = Algorithm.GenerateAndSetNewKey();
            if (Algorithm.Encrypt())
            {
                string encryptedText = Algorithm.CryptedText;

                string fileNameForDest = Path.GetFileName(fileName);
                SaveTextToFile(encryptedText, Path.Combine(destDirectory, fileNameForDest));

                string keyFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.{GetKeyExtension()}";
                SaveTextToFile(generatedKey, Path.Combine(destDirectory, keyFileName));
            }
        }

        public string DecryptFile(string fileName)
        {
            string encryptedText = LoadTextFromFile(fileName);

            string key;
            try
            {
                key = LoadKeyForFile(fileName);
            }
            catch
            {
                throw new Exception("Nije pronadjen fajl sa kljucem!");
            }

            Algorithm.SetEncriptionKey(key);
            Algorithm.CryptedText = encryptedText;
            if (Algorithm.Decrypt())
                return Algorithm.PlainText;

            return "ERROR";
        }

        public void SetAlgorithm(ICipherAlgorithm algorithm)
        {
            if (algorithm.GetType() == Algorithm.GetType()) return;

            Algorithm = algorithm;
        }

        public string LoadKeyForFile(string filePath)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);
            string keyPath = Path.Combine(KeysPath, $"{name}.{GetKeyExtension()}");
            return LoadTextFromFile(keyPath);
        }

        // Dodajemo prefix radi lakseg prepoznavanja koji algoritam je enkriptovao fajl, sve moze da radi i  sa zajednickom(.key) ekstenzijom.
        private string GetKeyExtension()
        {
            //return "key";
            return $"{Algorithm.GetType().Name.ToLower()}key";
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
