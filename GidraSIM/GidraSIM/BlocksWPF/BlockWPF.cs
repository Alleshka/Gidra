using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Абстрактный класс для блоков
    /// </summary>
    public abstract class BlockWPF : GSFigure
    {
        // Константные параметры блока
        protected const int HEIGHT = 60;
        protected const int WIDTH = 100;
        protected const int RADIUS = 10;

        protected const int POINT_SIZE = 4;

        protected const int ZINDEX = 1;

        protected const int TEXT_WIDTH = 90;
        protected const int TEXT_LEFT = 5;
        protected const int TEXT_TOP = 16;

        /// <summary>
        /// Координата верхнего левого угла блока
        /// </summary>
        public Point Position { get; protected set; }

        protected static Brush fill = Brushes.White;
        protected static Brush inPointFill = Brushes.Black;
        protected static Brush outPointFill = Brushes.Black;

        public BlockWPF(Point position, string processName)
        {
            this.Position = position;

            // построить блок
            MakeBody();

            // построить заголовок
            MakeTitle(processName);
        }

        protected virtual void MakeBody()
        {
            // path тела процесса
            Path bodyPath = new Path();
            bodyPath.Stroke = stroke;
            bodyPath.Fill = fill;
            // содержимое path
            bodyPath.Data = new RectangleGeometry(new Rect(new Size(WIDTH, HEIGHT)), RADIUS, RADIUS);
            // добавление
            this.Children.Add(bodyPath);
        }

        protected virtual void MakeTitle(string processName)
        {
            // подпись
            TextBlock processNameLabel = new TextBlock();
            processNameLabel.Text = processName;
            processNameLabel.TextWrapping = TextWrapping.Wrap;
            processNameLabel.Width = TEXT_WIDTH;
            // позиция
            Canvas.SetTop(processNameLabel, TEXT_TOP);
            Canvas.SetLeft(processNameLabel, TEXT_LEFT);
            // добавление
            this.Children.Add(processNameLabel);
        }

        protected virtual void SetZIngex()
        {
            foreach (UIElement child in this.Children)
            {
                Canvas.SetZIndex(child, ZINDEX);
            }
        }
    }
}
