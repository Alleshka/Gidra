using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace GidraSIM.AdmSet
{
    [DataContract]
    public class Settings
    {
        [DataMember]
        public String NamePC { get; set; } // Имя компьютера 
        // Другие настройки, лол
    }

    public class SettingsReader
    {
        public static Settings Read()
        {
            try
            {
                // Открываем файл
                using (FileStream file = new FileStream("Adm//Settings.json", FileMode.Open))
                {
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Settings));
                    return (Settings)json.ReadObject(file);
                }
            }
            catch (FileNotFoundException)
            {
                SettingsView _set = new SettingsView();
                _set.ShowDialog();
                return Read();
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Adm");
                SettingsView _set = new SettingsView();
                _set.ShowDialog();
                return Read();
            }
        }
        public static void Save(Settings set)
        {
            if (!Directory.Exists("Adm")) Directory.CreateDirectory("Adm");
            using (FileStream file = new FileStream("Adm//Settings.json", FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Settings));
                json.WriteObject(file, set);
            }
        }

    }
}