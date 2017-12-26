using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.Serialization;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModels.Procedures
{
 
    [DataContract]
    public class QualityCheckProcedureViewModel : ProcedureWPF
    {
        public QualityCheckProcedureViewModel(Point position, string processName) : base(position, processName, 1, 2)
        {
        }
    }
}
