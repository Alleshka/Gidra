using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GidraSIM.GUI.Core.BlocksWPF;

namespace GidraSIM.GUI.Utility
{
    public interface IProjectSaver
    {
        /// <summary>
        /// Сохранение целого процесса
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="path">Путь до файла</param>
        void SaveProject(TabControl tabControl, String path);
        ProjectSave LoadProject(String path);

        ProcessSave SaveProcess(UIElementCollection elementCollection, String ProcessName);
        List<ProcedureWPF> LoadProcess(ProcessSave save);
    }
}