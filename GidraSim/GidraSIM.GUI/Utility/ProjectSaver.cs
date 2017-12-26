using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.GUI.Core.BlocksWPF;
using GidraSIM.GUI.Core.BlocksWPF.ViewModels.Procedures;
using GidraSIM.GUI.Core.BlocksWPF.ViewModels.Resources;
using System.Runtime.Serialization.Json;
using System.IO;

namespace GidraSIM.GUI.Utility
{
    public class ProjectSaver : IProjectSaver
    {
        private ProjectSave save;

        public List<ProcedureWPF> LoadProcess(ProcessSave save)
        {
            List<ProcedureWPF> list = new List<ProcedureWPF>();

            foreach (var block in save.ProcedureList)
            {

                list.Add(new ProcedureWPF(block.Position, block.BlockName, block.InputsCount, block.OutputsCount));

            }

            return list;
        }

        public ProjectSave LoadProject(string path)
        {
            ProjectSave save;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ProjectSave));
                save = (ProjectSave)ser.ReadObject(stream);
            }

            return save;
        }

        public ProcessSave SaveProcess(UIElementCollection elementCollection, String ProcessName)
        {
            ProcessSave save = new ProcessSave
            {
                NameProcess = ProcessName
            };

            /// Проходим по всем элементам
            foreach (var element in elementCollection)
            {
                ///// Если блок проверки качества 
                //if (element is QualityCheckProcedureViewModel)
                //{
                //    var temp = element as QualityCheckProcedureViewModel;
                //    save.ProcedureList.Add(new LitleBlockWPF()
                //    {
                //        BlockName = temp.BlockName,
                //        Position = temp.Position,
                //        InputsCount = 1,
                //        OutputsCount = 2,
                //    }); // Добавляем блок
                //    continue;
                //}

                if (element is ProcedureWPF)
                {
                    var temp = element as ProcedureWPF;
                    save.ProcedureList.Add(new LitleBlockWPF()
                    {
                        BlockName = temp.BlockName,
                        Position = temp.Position,
                        InputsCount = temp.InputCount,
                        OutputsCount = temp.OutputCount,
                    }); // Добавляем блок
                    continue;
                }
            }
            return save;
        }
        public void SaveProject(TabControl tabControl, string path)
        {
            save = new ProjectSave();

            // Проходим по всем вкладкам
            foreach (var tabitem in tabControl.Items)
            {
                var tab = tabitem as TabItem;
                var drawArea = tab.Content as DrawArea;
                save.SaveList.Add(SaveProcess(drawArea.Children, tab.Header.ToString())); // Добавляем сохранённый процесс
            }

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                var ser = new DataContractJsonSerializer(typeof(ProjectSave));
                ser.WriteObject(stream, save); // Записываем проект
            }
        }
    }

    /// <summary>
    ///  Представляет облегчённый блок
    /// </summary>
    public class LitleBlockWPF
    {
        // Название блока
        public String BlockName { get; set; }

        // Позиция
        public System.Windows.Point Position { get; set; }

        // Количество входов
        public int InputsCount;
       
        // Количество выходов
        public int OutputsCount;
    }

    /// <summary>
    ///  Сохранёныный процесс
    /// </summary>
    public class ProcessSave
    {
        // Имя процесса
        public String NameProcess { get; set; }
        // Список процедур в текущем процессе
        public List<LitleBlockWPF> ProcedureList { get; set; }

        public ProcessSave()
        {
            ProcedureList = new List<LitleBlockWPF>();
        }
    }

    // Сохранённый проект
    public class ProjectSave
    {
        // Набор сохранений процессов
        public List<ProcessSave> SaveList { get; set; }

        public ProjectSave()
        {
            SaveList = new List<ProcessSave>();
        }
    }
}