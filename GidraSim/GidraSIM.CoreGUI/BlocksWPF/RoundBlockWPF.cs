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

        }

        protected void MakeBody(string IMG_SOURCE)
        {
            // изображение
            Image img = new Image();
            BitmapImage bm = new BitmapImage();
            bm.BeginInit();
            bm.UriSource = new Uri(IMG_SOURCE, UriKind.Relative);
            bm.EndInit();
            img.Source = bm;
            // размеры
            img.Height = IMG_SIZE;
            img.Width = IMG_SIZE;
            // добавление
            this.Children.Add(img);
        }

        protected override void MakeBody()
        {
            
        }
    }
}
