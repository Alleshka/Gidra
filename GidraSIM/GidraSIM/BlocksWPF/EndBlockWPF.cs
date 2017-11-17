using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace GidraSIM.BlocksWPF
{
    public class EndBlockWPF : BlockWPF
    {
        private const string IMG_SOURCE = "/Image/End.png";


        //Входы
        private List<ProcConnectionWPF> inPuts;

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


        public EndBlockWPF(Point position) : base(position, "end")
        {
            this.inPuts = new List<ProcConnectionWPF>();

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
            if (inPuts != null)
            {
                foreach (ProcConnectionWPF connection in inPuts)
                {
                    connection.Refresh();
                }
            }
        }

        /// <summary>
        /// Добавить соединение на вход
        /// </summary>
        /// <param name="connectoin"></param>
        public void AddInPutConnection(ProcConnectionWPF connectoin)
        {
            inPuts.Add(connectoin);
        }
    }
}
