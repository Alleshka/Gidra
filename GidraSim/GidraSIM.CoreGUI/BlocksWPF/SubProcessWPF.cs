using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using GidraSIM.Core.Model;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public class SubProcessWPF : ProcedureWPF
    {
        private const int BORDER = 3;

        public SubProcessWPF(Point position, Process process) : base(position, process)
        {

        }

        /// <summary>
        /// второй объект, отвечающий за геометрию тела блока подпроцесса
        /// </summary>
        protected RectangleGeometry bodyGeometry2;

        protected override void MakeBody()
        {
            InPointFill = Brushes.Black;
            OutPointFill = Brushes.Green;
            Fill = Brushes.White;

            // path тела процесса
            bodyPath = new Path();
            bodyPath.Stroke = Stroke;
            bodyPath.Fill = Fill;
            // содержимое path
            GeometryGroup bodyGroup = new GeometryGroup();
            bodyGroup.FillRule = FillRule.Nonzero;

            bodyGeometry = new RectangleGeometry(new Rect(
                new Size(
                    DEFAULT_WIDTH, 
                    DEFAULT_HEIGHT)), 
                RADIUS, 
                RADIUS);
            bodyGeometry2 = new RectangleGeometry(
                new Rect(
                    new Point(BORDER, BORDER), 
                    new Size(DEFAULT_WIDTH - 2 * BORDER, DEFAULT_HEIGHT - 2 * BORDER)),
                RADIUS - BORDER,
                RADIUS - BORDER);
            bodyGroup.Children.Add(bodyGeometry);
            bodyGroup.Children.Add(bodyGeometry2);
            bodyPath.Data = bodyGroup;
            // добавление
            this.Children.Add(bodyPath);
        }

        protected override void SetHeight(double height)
        {
            bodyGeometry2.Rect = new Rect(
               new Point(BORDER, BORDER),
               new Size(
                   DEFAULT_WIDTH - 2 * BORDER,
                   height - 2 * BORDER));
            
            base.SetHeight(height);
        }
    }
}
