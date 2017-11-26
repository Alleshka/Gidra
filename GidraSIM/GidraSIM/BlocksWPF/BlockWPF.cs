using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Абстрактный класс для блоков
    /// </summary>
    public abstract class BlockWPF : GSFigure
    {
        protected const int ZINDEX = 10;

        public bool IsMovable { get; private set; }

        public void Move()
        {
            Canvas.SetTop(this, Position.Y);
            Canvas.SetLeft(this, Position.X);
            this.UpdateConnectoins();
        }

        public BlockWPF(Point position)
        {
            this.Position = position;
            this.Move();
            this.Freeze();

            // построить блок
            this.MakeBody();

            SetZIndex(this, ZINDEX);
        }

        protected abstract void MakeBody();

        protected abstract void UpdateConnectoins();

        /////////////////////////////////////////////////////////////////////////
        //                            Drag&Drop                                //
        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Запрешает перемещение блока
        /// </summary>
        public void Freeze()
        {
            this.MouseLeftButtonDown -= OnMouseDown;
            this.MouseLeftButtonUp -= OnMouseUp;
            this.IsMovable = false;
        }

        /// <summary>
        /// Разрешает перемещение блока
        /// </summary>
        public void Unfreeze()
        {
            this.MouseLeftButtonDown += OnMouseDown;
            this.MouseLeftButtonUp += OnMouseUp;
            this.IsMovable = true;
        }

        private Vector relativeMousePos; // смещение мыши от левого верхнего угла блока
        Canvas container;        // канвас-контейнер

        /// <summary>
        /// по нажатию на левую клавишу начинаем следить за мышью
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            container = FindVisualParent<Canvas>(this.Parent);
            relativeMousePos = e.GetPosition(this) - new Point();
            MouseMove += OnDragMove;
            LostMouseCapture += OnLostCapture;
            Mouse.Capture(this);
        }

        /// <summary>
        /// клавиша отпущена - завершаем процесс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);
        }
        
        /// <summary>
        /// потеряли фокус (например, юзер переключился в другое окно) - завершаем тоже
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLostCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }

        void OnDragMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        void FinishDrag(object sender, MouseEventArgs e)
        {
            MouseMove -= OnDragMove;
            LostMouseCapture -= OnLostCapture;
            UpdatePosition(e);
        }

        /// <summary>
        /// обновление позиции
        /// </summary>
        /// <param name="e"></param>
        void UpdatePosition(MouseEventArgs e)
        {
            var point = e.GetPosition(container);
            this.Position = (point - relativeMousePos);
            Move();
        }
        
        /// <summary>
        /// это вспомогательная функция, ей место в общей библиотеке. Оставлю пока это здесь.
        /// Метод находит родителя по типу в визуальном дереве
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        private static T FindVisualParent<T>(DependencyObject element) where T : UIElement
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
