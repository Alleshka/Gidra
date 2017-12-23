using GidraSIM.Core.Model.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModels.Procedures
{
    public class FormingDocumentationProcedureViewModel : ProcedureWPF
    {
        public FormingDocumentationProcedureViewModel(Point position, string processName) : base(position, processName, 1, 1)
        {
        }
    }
}
