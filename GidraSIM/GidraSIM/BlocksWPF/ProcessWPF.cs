using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.BlocksWPF
{
    public class ProcessWPF : BlockWPF
    {

        public ProcessWPF(Point position, string processName) : base(position, processName)
        {
            // точка входа
            MakePoint(inPointFill, HEIGHT / 2, 0);

            // точка выхода
            MakePoint(outPointFill, HEIGHT / 2, WIDTH);

            // установить ZIndex
            SetZIngex();          
        }

        protected void MakePoint(Brush fill, double setTop, double setLeft)
        {
            // path точки 
            Path pointPath = new Path();
            pointPath.Fill = fill;
            // позиция
            Canvas.SetTop(pointPath, setTop);
            Canvas.SetLeft(pointPath, setLeft);
            // содержимое path
            pointPath.Data = new EllipseGeometry(new Rect(new Size(POINT_SIZE, POINT_SIZE)));
            // добавление
            this.Children.Add(pointPath);
        }
    }
}
