using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace ZAD1.App
{
    class AppConfig
    {
        public string Izvor { get; set; } = "";
        public string Odrediste { get; set; } = "";
        public int Algoritam { get; set; } = 0;

        private static readonly string ConfFile = "config.json";

        public static AppConfig UcitajKonfiguraciju()
        {
            try
            {
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), ConfFile)))
                {
                    using StreamReader sr = new StreamReader(ConfFile);
                    AppConfig config = JsonConvert.DeserializeObject<AppConfig>(sr.ReadToEnd());
                    sr.Close();

                    return config != null ? config : new AppConfig();
                }
            }
            catch { }

            return new AppConfig();
        }

        public void SacuvajKonfiguraciju()
        {
            using StreamWriter sr = new StreamWriter(ConfFile, false);
            sr.Write(JsonConvert.SerializeObject(this));
            sr.Close();
        }
    }

}
