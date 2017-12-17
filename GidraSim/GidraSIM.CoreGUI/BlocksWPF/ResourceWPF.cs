using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public class ResourceWPF : SquareBlockWPF
    {
        private const int IMG_SIZE = 30;
        private const int IMG_LEFT = 3;
        private const int IMG_TOP = 27;
        private const string IMG_SOURCE = "/Image/Resourse.png";

        // Соединения с процедурами
        private List<ResConnectionWPF> resPuts;

        public ResourceWPF(Point position, string resourceName) : base(position, resourceName)
        {
            this.resPuts = new List<ResConnectionWPF>();

            MakeIMG();

            // установить ZIndex
            //SetZIngex();
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

        protected override void UpdateConnectoins()
        {
            if (resPuts != null)
            {
                foreach (ResConnectionWPF connection in resPuts)
                {
                    connection.Refresh();
                }
            }
        }

        /// <summary>
        /// Добавить соединение с процессом
        /// </summary>
        /// <param name="connectoin"></param>
        public void AddResPutConnection(ResConnectionWPF connectoin)
        {
            resPuts.Add(connectoin);
        }

        public override void RemoveConnection(ConnectionWPF connection)
        {
            if (connection is ResConnectionWPF)
            {
                ResConnectionWPF resConnection = connection as ResConnectionWPF;

                resPuts.Remove(resConnection);
            }
        }

        public override void RemoveAllConnections()
        {
            if (resPuts != null)
            {
                while (resPuts.Count != 0)
                {
                    resPuts[0].Remove();
                }
            }
        }
    }
}
