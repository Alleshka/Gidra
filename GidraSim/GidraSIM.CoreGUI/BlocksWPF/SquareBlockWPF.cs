using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    /// <summary>
    /// Абстрактный класс для квадратных блоков
    /// </summary>
    [DataContract]
    public abstract class SquareBlockWPF : BlockWPF
    {
        // Константные параметры блока
        public const int DEFAULT_HEIGHT = 60;
        public const int DEFAULT_WIDTH = 100;
        public const int RADIUS = 10;

        protected const int TEXT_WIDTH = 90;
        protected const int TEXT_LEFT = 5;
        protected const int TEXT_TOP = 16;

        protected static Brush fill = Brushes.White;
        protected static Brush inPointFill = Brushes.Black;
        protected static Brush outPointFill = Brushes.Green;

        public string BlockName { get; private set; }

        public override Point MidPosition
        {
            get
            {
                return new Point(
                    Position.X + bodyPath.ActualWidth / 2,
                    Position.Y + bodyPath.ActualHeight / 2);
            }
        }

        public override Point LeftPosition
        {
            get
            {
                return new Point(
                    Position.X,
                    Position.Y + bodyPath.ActualHeight / 2);
            }
        }

        public override Point RightPosition
        {
            get
            {
                return new Point(
                    Position.X + bodyPath.ActualWidth,
                    Position.Y + bodyPath.ActualHeight / 2);
            }
        }

        public SquareBlockWPF(Point position, string squareBlockName) : base(position)
        {
            // построить заголовок
            this.MakeTitle(squareBlockName);
            BlockName = squareBlockName;
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

        /// <summary>
        /// path тела блока
        /// </summary>
        protected Path bodyPath;

        /// <summary>
        /// объект, отвечающий за геометрию тела блока
        /// </summary>
        protected RectangleGeometry bodyGeometry;

        protected override void MakeBody()
        {
            bodyPath = new Path();

            bodyPath.Stroke = stroke;
            bodyPath.Fill = fill;
            // содержимое path
            bodyGeometry = new RectangleGeometry(new Rect(new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT)), RADIUS, RADIUS);
            bodyPath.Data = bodyGeometry;
            // добавление
            this.Children.Add(bodyPath);
        }
    }
}
