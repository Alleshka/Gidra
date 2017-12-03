using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GidraSIM.Model;
using GidraSIM.BlocksWPF;

namespace GidraSIM.Utility
{
    public class ViewModelConverter : IViewModelConverter
    {
        public void Map(UIElementCollection uIElementCollection, Process process)
        {

            foreach (var element in uIElementCollection)
            {
                if(element is ProcConnectionWPF)
                {
                    var connection = (element as ProcConnectionWPF);
                    //TODO типа можно здесь всё обработать

                }
                else if (element is ResConnectionWPF)
                {
                    var connection = (element as ResConnectionWPF);
                    //TODO типа можно здесь всё обработать

                }
                
            }
        }
    }
}
