using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Обединяет ресурсы с процессами, которым они принадлежат
    /// </summary>
    public class ResConnectionWPF : ConnectionWPF
    {
        new protected static Brush stroke = Brushes.Gray;

        private Line line = null;

        public ResConnectionWPF(BlockWPF startBlock, BlockWPF endBlock) : base(startBlock, endBlock)
        {
            // startBlock должен быть ресурсом
            if(!(this.startBlock is ResourceWPF))
            {
                BlockWPF temp = this.startBlock;
                this.startBlock = this.endBlock;
                this.endBlock = temp;
            }
            if (!(this.startBlock is ResourceWPF))
            {
                throw new ArgumentException("Отсутствует ресурс");
            }

            MakeLine();

            // установить ZIndex
            //SetZIngex();
            SetZIndex(this, 1);
        }

        public override void Refresh()
        {
            if (line != null)
            {
                Point startPoint = startBlock.MidPosition;
                Point endPoint = endBlock.MidPosition;
                SetLinePoints(startPoint, endPoint);
            }
        }

        protected override void MakeLine()
        {
            Point startPoint = startBlock.MidPosition;
            Point endPoint = endBlock.MidPosition;

            if (line == null)
            {
                line = new Line();
                line.Stroke = stroke;
                line.StrokeThickness = THICKNESS;
                SetLinePoints(startPoint, endPoint);

                this.Children.Add(line);
            }
            else
            {
                Refresh();
            }
        }

        private void SetLinePoints(Point startPoint, Point endPoint)
        {
            if (line != null)
            {
                line.X1 = startPoint.X;
                line.Y1 = startPoint.Y;
                line.X2 = endPoint.X;
                line.Y2 = endPoint.Y;
            }
        }
    }
}
