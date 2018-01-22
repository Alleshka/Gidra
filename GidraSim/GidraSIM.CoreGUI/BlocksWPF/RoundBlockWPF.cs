using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public abstract class RoundBlockWPF : BlockWPF
    {
        private const int BORDER = 3;
        private const int IMG_SIZE = 60;

        public Brush Foreground { get; set; }
        public Brush Fill { get; set; }

        /// <summary>
        /// Координата центра левой стороны блока
        /// </summary>
        public override Point LeftPosition
        {
            get
            {
                return new Point(
                    Position.X + BORDER,
                    Position.Y + IMG_SIZE / 2);
            }
        }

        /// <summary>
        /// Координата центра правой стороны блока
        /// </summary>
        public override Point RightPosition
        {
            get
            {
                return new Point(
                    Position.X + IMG_SIZE - BORDER,
                    Position.Y + IMG_SIZE / 2);
            }
        }

        public RoundBlockWPF(Point position) : base(position)
        {
            this.Width = IMG_SIZE;
            this.Height = IMG_SIZE;
            Foreground = Brushes.White;
            Fill = Brushes.Black;
        }

        protected void MakeBody(string unicodeIcon)
        {
            //круг для фона
            Ellipse ellipse = new Ellipse();
            ellipse.Width = IMG_SIZE;
            ellipse.Height = IMG_SIZE;
            ellipse.Stroke = Stroke;
            ellipse.Fill = Fill;
            
            this.Children.Add(ellipse);

            // иконка
            TextBlock icon = new TextBlock();
            icon.Text = unicodeIcon;
            icon.TextWrapping = TextWrapping.Wrap;
            icon.Foreground = Foreground;
            icon.FontSize = IMG_SIZE*2/3;
            icon.HorizontalAlignment = HorizontalAlignment.Center;
            icon.VerticalAlignment = VerticalAlignment.Center;

            Grid iconGrid = new Grid();
            iconGrid.Height = IMG_SIZE;
            iconGrid.Width = IMG_SIZE;
            iconGrid.Children.Add(icon);
            this.Children.Add(iconGrid);
        }

        protected override void MakeBody()
        {
            
        }
    }
}
