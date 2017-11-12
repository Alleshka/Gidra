using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GidraSIM.BlocksWPF
{
    public class ResourceWPF : BlockWPF
    {
        private const int IMG_SIZE = 30;
        private const int IMG_LEFT = 3;
        private const int IMG_TOP = 27;
        private const string IMG_SOURCE = "Image/Resourse.png";

        public ResourceWPF(Point position, string processName) : base(position, processName)
        {
            MakeIMG();

            // установить ZIndex
            SetZIngex();
        }

        private void MakeIMG()
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
            // позиция
            Canvas.SetTop(img, IMG_TOP);
            Canvas.SetLeft(img, IMG_LEFT);
            // добавление
            this.Children.Add(img);
        }
    }
}
