using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GidraSIM.GUI.Core.BlocksWPF;

namespace GidraSIM.GUI.Utility
{
    public interface IProjectSaver
    {
        /// Сохранение проекта по указанному пути
        void SaveProjectExecute(TabControl tabcontrol, String path);

        /// Считывание файла проекта
        SaveProject LoadProjecExecute(String path);

        /// Прорисовка загруженного процесса из класса сохранения
        void LoadProcessExecute(SaveProcess process, DrawArea area);
    }
}