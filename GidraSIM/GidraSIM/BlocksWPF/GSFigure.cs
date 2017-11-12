using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Обший родительский класс для блоков и их соединений
    /// </summary>
    public abstract class GSFigure : Canvas
    {
        protected static Brush stroke = Brushes.Black;
    }
}
