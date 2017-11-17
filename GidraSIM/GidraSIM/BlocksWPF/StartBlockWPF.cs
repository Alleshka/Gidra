using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace GidraSIM.BlocksWPF
{
    public class StartBlockWPF : BlockWPF
    {
        private const string IMG_SOURCE = "/Image/Begin.png";


        // Выходы
        private List<ProcConnectionWPF> outPuts;

        /// <summary>
        /// Координата центра блока
        /// </summary>
        public override Point MidPosition
        {
            get
            {
                return new Point(
                    Position.X + HEIGHT / 2,
                    Position.Y + HEIGHT / 2);
            }
        }

        /// <summary>
        /// Координата центра левой стороны блока
        /// </summary>
        public override Point LeftPosition
        {
            get
            {
                return new Point(
                    Position.X + 3,
                    Position.Y + HEIGHT / 2);
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
                    Position.X + HEIGHT - 3,
                    Position.Y + HEIGHT / 2);
            }
        }

        public StartBlockWPF(Point position) : base (position, "start")
        {
            this.outPuts = new List<ProcConnectionWPF>();

            // установить ZIndex
            //SetZIngex();
        }

        protected override void MakeBody()
        {
            // изображение
            Image img = new Image();
            BitmapImage bm = new BitmapImage();
            bm.BeginInit();
            bm.UriSource = new Uri(IMG_SOURCE, UriKind.Relative);
            bm.EndInit();
            img.Source = bm;
            // размеры
            img.Height = HEIGHT;
            img.Width = HEIGHT;
            // добавление
            this.Children.Add(img);
        }

        protected override void MakeTitle(string processName)
        {
            
        }

        protected override void UpdateConnectoins()
        {
            if(outPuts != null)
            {
                foreach (ProcConnectionWPF connection in outPuts)
                {
                    connection.Refresh();
                }
            }
        }

        /// <summary>
        /// Добавить соединение на выход
        /// </summary>
        /// <param name="connectoin"></param>
        public void AddOutPutConnection(ProcConnectionWPF connectoin)
        {
            outPuts.Add(connectoin);
        }
    }
}
