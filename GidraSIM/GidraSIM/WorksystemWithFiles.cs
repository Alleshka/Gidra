using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace GidraSIM
{
    public class WorksystemWithFiles
    {
        string WayToProcesses; //Путь к файлу
        string WayToFolder;    //Путь в общую папку проекта        
        string WayToResults;   //Путь в папку с результами  
        // string WayToGraphics;  //Путь в папку с images_in_tabitem и connect
        DataContractSerializer SelialiserProject;              //для сохранения проекта
        //   DataContractSerializer SelialiserProcesses;            //для сохранения процессов

        public WorksystemWithFiles()
        {
            SelialiserProject = new DataContractSerializer(typeof(Project));
        }

        public void UpdateWay(string Way, string NameProject)
        {
            WayToFolder = Way;//путь ко всем файлам
            WayToProcesses = WayToFolder + "\\Processes";
            WayToResults = WayToFolder + "\\Results";
        }

        public bool CheckWay(string Way, string NameProject)//проверка пути
        {
            WayToFolder = Way + "\\" + NameProject;//путь ко всем файлам
            WayToProcesses = WayToFolder + "\\Processes";
            WayToResults = WayToFolder + "\\Results";
            DirectoryInfo MainDirectory = new DirectoryInfo(WayToFolder);
            if (MainDirectory.Exists)//такой каталог уже существует, выдать сообщение об ошибке
                return false;//путь проверку не прошел
            else //если такого каталога еще нет, то создаем его
            {
                MainDirectory.Create();//создаем папку процесса
                DirectoryInfo AllProcessesDirectory = new DirectoryInfo(WayToProcesses);//хранение всех процессов 
                AllProcessesDirectory.Create();
                DirectoryInfo ResultDirectory = new DirectoryInfo(WayToResults);//хранение результатов моделирования
                ResultDirectory.Create();
                return true;
            }
        }

        public void SaveProject(ref Project project)//сохранение проекта
        {
            // сохранение проекта
            FileStream SourceStream = File.Create(WayToFolder + "\\" + project.NameProject + ".gsim");//добавление файла процесса 
            SelialiserProject.WriteObject(SourceStream, project);
            SourceStream.Close(); 
        }

        public Project OpenProject(string way)//открытие проекта
        {
            //открываем файл процесса
            FileStream SourceStream = new FileStream(way, FileMode.Open);
            DataContractSerializer dcs = new DataContractSerializer(typeof(Project));
            XmlDictionaryReader xdr = XmlDictionaryReader.CreateTextReader(SourceStream, new XmlDictionaryReaderQuotas());
            Project p = (Project)dcs.ReadObject(xdr);
            SourceStream.Close();
            return p;
        }
    }
}