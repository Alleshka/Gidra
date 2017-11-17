using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Абстрактный класс для блоков
    /// </summary>
    public abstract class BlockWPF : GSFigure
    {
        // Константные параметры блока
        public const int HEIGHT = 60;
        public const int WIDTH = 100;
        public const int RADIUS = 10;

        public const int POINT_SIZE = 4;

        protected const int ZINDEX = 3;

        protected const int TEXT_WIDTH = 90;
        protected const int TEXT_LEFT = 5;
        protected const int TEXT_TOP = 16;

        /// <summary>
        /// Координата верхнего левого угла блока
        /// </summary>
        public Point Position { get; protected set; }

        public bool IsMovable { get; private set; }

        /// <summary>
        /// Координата центра блока
        /// </summary>
        public virtual Point MidPosition
        {
            get
            {
                return new Point(
                    Position.X + WIDTH / 2,
                    Position.Y + HEIGHT / 2 );
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
                    Position.X ,
                    Position.Y + HEIGHT / 2);
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
                    Position.X + WIDTH,
                    Position.Y + HEIGHT / 2);
            }
        }

        public void Move()
        {
            Canvas.SetTop(this, Position.Y);
            Canvas.SetLeft(this, Position.X);

            this.UpdateConnectoins();
        }

        protected static Brush fill = Brushes.White;
        protected static Brush inPointFill = Brushes.Black;
        protected static Brush outPointFill = Brushes.Green;

        public BlockWPF(Point position, string processName)
        {
            this.Position = position;
            this.Move();
            this.Freeze();

            // построить блок
            this.MakeBody();

            // построить заголовок
            this.MakeTitle(processName);

            SetZIndex(this, 10);
        }

        protected virtual void MakeBody()
        {
            // path тела процесса
            Path bodyPath = new Path();
            bodyPath.Stroke = stroke;
            bodyPath.Fill = fill;
            // содержимое path
            bodyPath.Data = new RectangleGeometry(new Rect(new Size(WIDTH, HEIGHT)), RADIUS, RADIUS);
            // добавление
            this.Children.Add(bodyPath);
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

        protected virtual void SetZIngex()
        {
            foreach (UIElement child in this.Children)
            {
                Canvas.SetZIndex(child, ZINDEX);
            }
        }

        protected abstract void UpdateConnectoins();

        //
        //      Drag&Drop
        //
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
