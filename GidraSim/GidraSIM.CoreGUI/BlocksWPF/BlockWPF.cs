using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.Serialization;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    /// <summary>
    /// Абстрактный класс для блоков
    /// </summary>
    [DataContract]
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
            this.Cursor = Cursors.SizeAll;
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
            this.Cursor = Cursors.Arrow;
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




        public override void Remove()
        {
            Canvas parrent = FindVisualParent<Canvas>(this.Parent);

            // удалить соединения этого блока с остальными
            RemoveAllConnections();

            // удалить блок
            parrent.Children.Remove(this);
        }


        /// <summary>
        /// Уделяет ифнормацию о текущим соединении из блока
        /// </summary>
        /// <param name="connection"></param>
        public abstract void RemoveConnection(ConnectionWPF connection);

        /// <summary>
        /// Уделяет всю ифнормацию о соединенях из блока
        /// </summary>
        /// <param name="connection"></param>
        public abstract void RemoveAllConnections();
    }
}
