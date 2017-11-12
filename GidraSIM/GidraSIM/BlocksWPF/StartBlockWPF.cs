using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace GidraSIM.BlocksWPF
{
    public class StartBlockWPF : BlockWPF
    {
        public StartBlockWPF(Point position) : base (position, "")
        {

        }

        protected override void MakeBody()
        {
            // TODO
        }

        protected override void MakeTitle(string processName)
        {
            
        }
    }
}
