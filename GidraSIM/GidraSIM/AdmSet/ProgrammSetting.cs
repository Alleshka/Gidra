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
        /// <summary>
        /// Считывание настрек из файла 
        /// </summary>
        /// <returns></returns>
        public static Settings Read()
        {
            try
            {
                // Открываем файл
                using (FileStream file = new FileStream("Adm//Settings.json", FileMode.Open))
                {
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Settings)); // Создаём сериализатор
                    return (Settings)json.ReadObject(file); // Считываем настройки их файла 
                }
            }
            catch (FileNotFoundException) // Если файл не найден
            {
                SettingsView _set = new SettingsView();
                _set.ShowDialog(); // Открываем окно настроек и просим пользователя указать
                return Read(); // Считываем введённые настройки 
            }
            catch (DirectoryNotFoundException) // Если папка не создана
            {
                Directory.CreateDirectory("Adm"); // Создаём папку 
                SettingsView _set = new SettingsView();
                _set.ShowDialog(); // Просим пользователя ввести настройки 
                return Read();
            }
        }
        /// <summary>
        /// Сохранение настроек
        /// </summary>
        /// <param name="set">Структура с настройками</param>
        public static void Save(Settings set)
        {
            if (!Directory.Exists("Adm")) Directory.CreateDirectory("Adm"); // Создаём папку, если не создана
            using (FileStream file = new FileStream("Adm//Settings.json", FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Settings)); // Создаём сериализатор
                json.WriteObject(file, set); // Записываем в файл
            }
        }

        /// <summary>
        /// Строка подключения к БД с ресурсами 
        /// </summary>
        public static String ResourcesConnectionString
        {
            get
            {
                return "metadata = res://*/ResourcesDB.csdl|res://*/ResourcesDB.ssdl|res://*/ResourcesDB.msl;provider=System.Data.SqlClient;provider connection string=';data source=" + Read().NamePC + ";initial catalog=Resources;integrated security=True;multipleactiveresultsets=True;App=EntityFramework'";
            }
        }

        /// <summary>
        /// Строка подключения к БД с процессами 
        /// </summary>
        public static String ModelingSessionConnectionString
        {
            get
            {
                return "metadata = res://*/ModelingSession.csdl|res://*/ModelingSession.ssdl|res://*/ModelingSession.msl;provider=System.Data.SqlClient;provider connection string=';data source=" + Read().NamePC + ";initial catalog=ModelingSession;integrated security=True;multipleactiveresultsets=True;App=EntityFramework';";
            }
        }
    }
}