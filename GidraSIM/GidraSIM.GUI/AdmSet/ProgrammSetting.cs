using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace GidraSIM.GUI.AdmSet
{
    [DataContract]
    public class Settings
    {
        [DataMember(EmitDefaultValue = false)]
        public String NamePC { get; set; } // Имя компьютера 
        // Другие настройки, лол
    }

    public class SettingsReader
    {
        private static string settingsFile = "Settings.json";
        //private static string settingsDirectory = "Adm";
        private static string settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\SaprSIM";

        /// <summary>
        /// попытка чтения с созданием директории при необходимости
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>возвращает true, если удалось прочитать в файл</returns>
        public static bool TryRead(out Settings settings)
        {
            settings = null;
            try
            {
                // Открываем файл
                using (FileStream file = new FileStream(String.Format("{0}//{1}", settingsDirectory, settingsFile), FileMode.Open))
                {
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Settings)); // Создаём сериализатор
                    settings =  (Settings)json.ReadObject(file); // Считываем настройки их файла 
                    if (settings != null)
                        return true;
                    else
                        return false;
                }
            }
            catch (FileNotFoundException) // Если файл не найден
            {
                return false;
            }
            catch (DirectoryNotFoundException) // Если папка не создана
            {
                Directory.CreateDirectory(settingsDirectory); // Создаём папку 
                return false;
            }
        }

        /// <summary>
        /// Считывание настрек из файла 
        /// </summary>
        /// <returns></returns>
        public static Settings Read() //TODO возможно, эта функция слишком много себе позволяет
        {
            try
            {
                // Открываем файл
                using (FileStream file = new FileStream(String.Format("{0}//{1}",settingsDirectory, settingsFile), FileMode.Open))
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
                Directory.CreateDirectory(settingsDirectory); // Создаём папку 
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
            if (!Directory.Exists(settingsDirectory)) Directory.CreateDirectory(settingsDirectory); // Создаём папку, если не создана
            using (FileStream file = new FileStream(String.Format("{0}//{1}", settingsDirectory, settingsFile), FileMode.OpenOrCreate))
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
                //return "metadata = res://*/ResourcesDB.csdl|res://*/ResourcesDB.ssdl|res://*/ResourcesDB.msl;provider=System.Data.SqlClient;provider connection string=';data source=" + Read().NamePC + ";initial catalog=Resources;integrated security=True;multipleactiveresultsets=True;App=EntityFramework'";
                return "Data Source="+ Read().NamePC+";Initial Catalog=SimSapr;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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