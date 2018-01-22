using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    /// <summary>
    /// Обший родительский класс для блоков и их соединений
    /// </summary>
    public abstract class GSFigure : Canvas, ISelectable
    {
        public const int POINT_SIZE = 5; // возможно стоит вывести все константы в одно место

        public GSFigure()
        {
            ShadowColor = Colors.Purple;
            Stroke = Brushes.Black;
        }

        /// <summary>
        /// Координата верхнего левого угла блока
        /// </summary>
        public Point Position { get; protected set; }

        /// <summary>
        /// Координата центра блока
        /// </summary>
        public virtual Point MidPosition
        {
            get
            {
                return new Point(
                    Position.X + this.ActualWidth / 2,
                    Position.Y + this.ActualHeight / 2);
            }
        }

        /// <summary>
        /// Координата центра левой стороны блока
        /// </summary>
        public virtual Point LeftPosition
        {
            get
            {
                return new Point(
                    Position.X,
                    Position.Y + this.ActualHeight / 2);
            }
        }

        /// <summary>
        /// Координата центра правой стороны блока
        /// </summary>
        public virtual Point RightPosition
        {
            get
            {
                return new Point(
                    Position.X + this.ActualWidth,
                    Position.Y + this.ActualHeight / 2);
            }
        }

        public Brush Stroke { get; set; }

        private bool isSelectable;

        // параметры тени выделенной фигуры
        public Color ShadowColor { get; set; }
        private const int SHADOW_BLUR_RADIUS = 10;

        /// <summary>
        /// Можно ли выделять фигуру
        /// </summary>
        public bool IsSelectable
        {
            get
            {
                return isSelectable;
            }

            set
            {
                isSelectable = value;
            }
        }

        /// <summary>
        /// Выделена ли фигура
        /// </summary>
        public bool IsSelected { get; private set; }

        public void Select()
        {
            if (IsSelectable)
            {
                DropShadowEffect shadow = new DropShadowEffect();
                shadow.Color = ShadowColor;
                shadow.BlurRadius = SHADOW_BLUR_RADIUS;

                this.Effect = shadow;

                IsSelected = true;
            }
        }

        public void UnSelect()
        {
            if (IsSelectable)
            {
                this.Effect = null;

                IsSelected = false;
            }
        }

        /// <summary>
        /// удаляет текущий блок из родительского канваса
        /// </summary>
        public abstract void Remove();


        /// <summary>
        /// это вспомогательная функция, ей место в общей библиотеке. Оставлю пока это здесь.
        /// Метод находит родителя по типу в визуальном дереве
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        protected static T FindVisualParent<T>(DependencyObject element) where T : UIElement
        {
            var parent = element;
            while (parent != null)
            {
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }
    }
}
