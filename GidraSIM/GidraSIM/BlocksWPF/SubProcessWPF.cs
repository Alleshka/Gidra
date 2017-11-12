using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.BlocksWPF
{
    public class SubProcessWPF : ProcessWPF
    {
        private const int BORDER = 3;

        public SubProcessWPF(Point position, string processName) : base(position, processName)
        {

        }

        protected override void MakeBody()
        {
            // path тела процесса
            Path bodyPath = new Path();
            bodyPath.Stroke = stroke;
            bodyPath.Fill = fill;
            // содержимое path
            GeometryGroup bodyGroup = new GeometryGroup();
            bodyGroup.FillRule = FillRule.Nonzero;
            bodyGroup.Children.Add(new RectangleGeometry(new Rect(new Size(WIDTH, HEIGHT)), RADIUS, RADIUS));
            bodyGroup.Children.Add(
                new RectangleGeometry(new Rect(new Point(BORDER,BORDER), new Size(WIDTH - 2*BORDER, HEIGHT - 2*BORDER)),
                RADIUS - BORDER, 
                RADIUS - BORDER));
            bodyPath.Data = bodyGroup;
            // добавление
            this.Children.Add(bodyPath);
        }
    }
}
