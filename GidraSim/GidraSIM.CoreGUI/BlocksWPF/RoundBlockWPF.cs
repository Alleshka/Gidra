using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;
using System;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public abstract class RoundBlockWPF : BlockWPF
    {
        private const int BORDER = 3;
        private const int IMG_SIZE = 60;

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
        }

        protected void MakeBody(string unicodeIcon)
        {
            Brush fill = Brushes.Black;
            //круг для фона
            Ellipse ellipse = new Ellipse();
            ellipse.Width = IMG_SIZE;
            ellipse.Height = IMG_SIZE;
            ellipse.Stroke = stroke;
            ellipse.Fill = fill;
            
            this.Children.Add(ellipse);

            // иконка
            TextBlock icon = new TextBlock();
            icon.Text = unicodeIcon;
            icon.TextWrapping = TextWrapping.Wrap;
            icon.Foreground = Brushes.White;
            icon.FontSize = IMG_SIZE*2/3;
            icon.Width = ellipse.Width;
            icon.Height = ellipse.Height;
            icon.HorizontalAlignment = HorizontalAlignment.Center;
            icon.VerticalAlignment = VerticalAlignment.Center;
            this.Children.Add(icon);

        }

        protected override void MakeBody()
        {
            
        }
    }
}
