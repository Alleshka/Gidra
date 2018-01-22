using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using GidraSIM.Core.Model.Resources;
using System.Windows.Media;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public class ResourceWPF : SquareBlockWPF
    {
        private const int IMG_SIZE = 30;
        private const int IMG_LEFT = 3;
        private const int IMG_TOP = 27;
        private const string IMG_SOURCE = "pack://application:,,,//Image//Resourse.png";

        // Соединения с процедурами
        private List<ResConnectionWPF> resPuts;

        public AbstractResource ResourceModel { get; private set; }

        public Brush Foreground { get; set; }

        public ResourceWPF(Point position, AbstractResource resource) : base(position, resource.Description)
        {
            this.resPuts = new List<ResConnectionWPF>();
            this.ResourceModel = resource;
            Foreground = Brushes.Black;

            MakeIMG();

            // установить ZIndex
            //SetZIngex();
        }

        private void MakeIMG()
        {
            //FIXME тут что-то всё сломалось, так что добавлю лучше юникод

            //// изображение
            //Image img = new Image();
            //Image image = new Image();
            //BitmapImage bm = new BitmapImage();
            //bm.BeginInit();
            //bm.UriSource = new Uri(IMG_SOURCE);
            //bm.EndInit();
            //img.Source = bm;
            //// размеры
            //img.Height = IMG_SIZE;
            //img.Width = IMG_SIZE;
            //// позиция
            //Canvas.SetTop(img, IMG_TOP);
            //Canvas.SetLeft(img, IMG_LEFT);
            // добавление
            //this.Children.Add(img);

            //"⚙"
            // иконка
            TextBlock icon = new TextBlock();
            icon.Text = "⚙";
            icon.TextWrapping = TextWrapping.Wrap;
            icon.Foreground = Foreground;
            icon.FontSize = IMG_SIZE * 2 / 3;
            icon.Width = IMG_SIZE;
            icon.Height = IMG_SIZE;
            icon.HorizontalAlignment = HorizontalAlignment.Center;
            icon.VerticalAlignment = VerticalAlignment.Center;
            Canvas.SetTop(icon, IMG_TOP);
            Canvas.SetLeft(icon, IMG_LEFT);
            this.Children.Add(icon);
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
