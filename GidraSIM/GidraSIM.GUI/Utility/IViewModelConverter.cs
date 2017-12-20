using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSIM.Core.Model;
using System.Windows.Controls;

namespace GidraSIM.GUI.Utility
{
    public interface IViewModelConverter
    {
        /// <summary>
        /// преобразование из графики в процесс
        /// </summary>
        /// <param name="uIElementCollection">набор WPF-блоков</param>
        /// <param name="process">пустой процесс для отображения</param>
        void Map(UIElementCollection uIElementCollection, Process process);
    }
}
