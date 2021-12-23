using System;
using System.IO;
using ZAD2.App;
using ZAD3.App.Hash;

namespace ZAD1
{
    class FileLogic
    {
        private CipherAlgorithm Algorithm;
        private string KeysPath;
        private TigerHash tiger;

        public FileLogic(CipherAlgorithm algorithm, string keysPath, bool crtModeActive)
        {
            Algorithm = algorithm;
            KeysPath = keysPath;
            tiger = new TigerHash();
            ToggleCRTMode(crtModeActive);
        }

        public void SetKeysPath(string path)
        {
            KeysPath = path;
        }

        public void EncryptFile(string fileName, string destDirectory)
        {
            string plainText = LoadTextFromFile(fileName);

            Algorithm.PlainText = plainText;
            Tuple<string, string> data = Algorithm.GenerateAndSetNewKey();
            string generatedKey = data.Item1;
            string generatedCTR = data.Item2;
            if (Algorithm.Encrypt())
            {
                string encryptedText = Algorithm.CryptedText;

                string fileNameForDest = Path.GetFileName(fileName);
                SaveTextToFile(encryptedText, Path.Combine(destDirectory, fileNameForDest), true);

                string keyFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.{GetKeyExtension()}";
                SaveTextToFile(generatedKey + "\n" + generatedCTR, Path.Combine(destDirectory, keyFileName));
            }
        }

        public string DecryptFile(string fileName)
        {
            Tuple<string, string> data = LoadTextAndHashFromFile(fileName);
            string hash = data.Item1;
            string encryptedText = data.Item2;

            if (tiger.Hash(encryptedText) != hash)
                throw new Exception("TigerHash teksta i sačuvani hash iz fajla se ne poklapaju!");

            Tuple<string, string> keyData;
            string key;
            string initialCtrString;
            try
            {
                keyData = LoadKeyForFile(fileName);
                key = keyData.Item1;
                initialCtrString = keyData.Item2;
            }
            catch
            {
                throw new Exception("Nije pronadjen fajl sa kljucem!");
            }

            Algorithm.SetEncriptionKey(key);
            Algorithm.CryptedText = encryptedText;
            if (Algorithm.CTRModeActive)
                Algorithm.SetInitialCTR(initialCtrString);

            if (Algorithm.Decrypt())
                return Algorithm.PlainText;

            return "ERROR";
        }

        public void SetAlgorithm(CipherAlgorithm algorithm)
        {
            if (algorithm.GetType() == Algorithm.GetType()) return;

            Algorithm = algorithm;
        }

        public void ToggleCRTMode(bool active)
        {
            Algorithm.CTRModeActive = active;
        }

        public Tuple<string, string> LoadKeyForFile(string filePath)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);
            string keyPath = Path.Combine(KeysPath, $"{name}.{GetKeyExtension()}");
            using StreamReader sr = new StreamReader(keyPath);
            string key = sr.ReadLine();
            string initialCtr = sr.ReadToEnd();
            sr.Close();

            return new Tuple<string, string>(key, initialCtr);
        }

        // Dodajemo prefix radi lakseg prepoznavanja koji algoritam je enkriptovao fajl, sve moze da radi i  sa zajednickom(.key) ekstenzijom.
        private string GetKeyExtension()
        {
            //return "key";
            return $"{Algorithm.GetType().Name.ToLower()}key";
        }


        public void SaveTextToFile(string text, string destFileName, bool saveTigerHash = false)
        {
            if (!string.IsNullOrEmpty(destFileName))
            {
                using StreamWriter sw = new StreamWriter(destFileName, false);
                if (saveTigerHash)
                    sw.WriteLine(tiger.Hash(text));
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

        public static Tuple<string, string> LoadTextAndHashFromFile(string fileName)
        {
            using StreamReader sr = new StreamReader(fileName);
            string tigerHash = sr.ReadLine();
            string text = sr.ReadToEnd();
            sr.Close();

            return new Tuple<string, string>(tigerHash, text);
        }
    }
}
