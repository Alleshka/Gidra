using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GidraSIM.Core.Model;

namespace GidraSIM.GUI.Utility
{
    public interface IProjectSaver
    {
        /// <summary>
        /// Сохраняет проект из TabControla в файл по указанному пути
        /// </summary>
        /// <param name="testTabControl"></param>
        /// <param name="Path"></param>
        void SaveProjectExecute(TabControl testTabControl, String Path, int mainnumber);

        int LoadProjectExecute(String path, TabControl testTabControl, List<DrawArea> drawAreas, List<Process> processes, out Process mainprocess);
    }
}