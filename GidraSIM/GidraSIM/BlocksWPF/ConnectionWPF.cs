using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Абстрактный класс соединений между блоками
    /// </summary>
    public abstract class ConnectionWPF : GSFigure
    {
        // Константные параметры блока
        protected const int THICKNESS = 2;
        protected const int ZINDEX = 1;

        // блоки, к которым соединяются
        protected BlockWPF startBlock;
        protected BlockWPF endBlock;

        public ConnectionWPF(BlockWPF startBlock, BlockWPF endBlock)
        {
            this.startBlock = startBlock;
            this.endBlock = endBlock;
        }

        protected abstract void MakeLine();

        protected virtual void SetZIngex()
        {
            foreach (UIElement child in this.Children)
            {
                Canvas.SetZIndex(child, ZINDEX);
            }
        }

        public abstract void Refresh();
    }
}
