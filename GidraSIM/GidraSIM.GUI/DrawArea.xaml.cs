using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GidraSIM.Core.Model;
using GidraSIM.GUI.Core.BlocksWPF;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для DrawArea.xaml
    /// </summary>
    public partial class DrawArea : UserControl
    {

        enum Mode { Arrow, Procedure, Resourse , Connect , SubProcess }

        Mode mode = (Mode)10;

        private bool isHaveStartAndEnd;
        public bool IsHaveStartEnd
        {
            get => isHaveStartAndEnd;
            set => isHaveStartAndEnd = value;
        }

        public List<Process> Processes { get; set; }

        private bool AllChildrenIsSelectable
        {
            set
            {
                foreach(GSFigure figure in workArea.Children)
                {
                    figure.IsSelectable = value;
                }
            }
        }

        GSFigure selectedFigure;
        ConnectPointWPF selectedPoint;

        public DrawArea()
        {
            InitializeComponent();

            isHaveStartAndEnd = false;
        }

        /// <summary>
        /// добавление процедур
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Procedure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Вычисление координат
            Point cursorPoint = e.GetPosition(workArea);
            Point procedurePosition = (Point)(cursorPoint - new Point(ProcedureWPF.DEFAULT_WIDTH / 2, ProcedureWPF.DEFAULT_HEIGHT / 2));

            // Добавление
            // TODO: Ввод имени процедуры и связь с моделью
            //workArea.Children.Add(new ProcedureWPF(procedurePosition, "Процедура", rand.Next(1, 11), rand.Next(1, 11)));

            TestProcedureSelectionDialog dialog = new TestProcedureSelectionDialog();

            if(dialog.ShowDialog() == true)
            {
                var procedure = dialog.SelectedBlock;

                workArea.Children.Add(new ProcedureWPF(procedurePosition,procedure));
            }

            //workArea.Children.Add(new ProcedureWPF(procedurePosition, "Фикс. процедура (10)", 1, 1));
        }


        private static Random rand = new Random();

        /// <summary>
        /// добавление подпроцессов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_SubProcess_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Вычисление координат
            Point cursorPoint = e.GetPosition(workArea);
            Point subProcessPosition = (Point)(cursorPoint - new Point(SubProcessWPF.DEFAULT_WIDTH / 2, SubProcessWPF.DEFAULT_HEIGHT / 2));

            // Добавление
            // TODO: Ввод имени подпроцесса и связь с моделью
            TestSubProcessSelectionDialog dialog = new TestSubProcessSelectionDialog(subProcessPosition, Processes);
            if (dialog.ShowDialog() == true)
            {
                var process = dialog.SelectedProcess;
                workArea.Children.Add(process);
            }
            //workArea.Children.Add(new SubProcessWPF(subProcessPosition, "Подпроцесс", rand.Next(1, 11), rand.Next(1, 11)));
        }

        /// <summary>
        /// добавление ресурсов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Resource_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Вычисление координат
            Point cursorPoint = e.GetPosition(workArea);
            Point resourcePosition = (Point)(cursorPoint - new Point(ResourceWPF.DEFAULT_WIDTH / 2, ResourceWPF.DEFAULT_HEIGHT / 2));

            // Добавление
            // TODO: Ввод имени процедуры и связь с моделью
            //workArea.Children.Add(new ResourceWPF(resourcePosition, "Ресурс"));
            TestResourceSelectionDialog dialog = new TestResourceSelectionDialog();

            if (dialog.ShowDialog() == true)
            {
                var resource = dialog.SelectedResource;
                workArea.Children.Add(new ResourceWPF(resourcePosition,resource));
            }
        }

        /// <summary>
        /// Выделение 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Arrow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool IsSomeFigureSelected = false;

            foreach(GSFigure figure in workArea.Children)
            {
                if (figure.IsMouseOver)
                {
                    if(selectedFigure != null) selectedFigure.UnSelect();
                    figure.Select();
                    selectedFigure = figure;
                    IsSomeFigureSelected = true;
                }
            }

            if (!IsSomeFigureSelected)
            {
                // Небыло выделено ниодного элемента
                // следовательно необходимо снять выделение
                Unsellect();
            }
        }

        /// <summary>
        /// Соединение 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Connect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool IsSomeFigureSelected = false;

            foreach (GSFigure figure in workArea.Children)
            {
                if (figure is BlockWPF)
                {
                    BlockWPF block = figure as BlockWPF;
                    if (block.IsMouseOver)
                    {

                        if (selectedFigure != null)
                        {
                            // Соединение
                            if (Connect(selectedFigure as BlockWPF, block))
                            {
                                // соединение прощло успешно
                                Unsellect();
                                return;
                            }
                            else
                            {
                                // соединения не прошло
                                return;
                            }
                        }

                        // Выделение первого блока
                        block.Select();
                        selectedFigure = block;

                        // при выделении процедуры/подпроцесса поверяем выделена ли точка выхода
                        if (selectedFigure is ProcedureWPF)
                        {
                            foreach(UIElement child in selectedFigure.Children)
                            {
                                if (child.IsMouseOver)
                                {
                                    if (child is ConnectPointWPF)
                                    {
                                        ConnectPointWPF point = child as ConnectPointWPF;
                                        if (point.ConnectType == ConnectPointWPF_Type.outPut)
                                        {
                                            selectedPoint = point;
                                            selectedPoint.Select();
                                        }
                                    }
                                }
                            }
                        }

                        IsSomeFigureSelected = true;
                    }
                }
            }

            if (!IsSomeFigureSelected)
            {
                // Небыло выделено ни одного элемента
                // следовательно необходимо снять выделение
                Unsellect();
            }
        }

        /// <summary>
        /// снять выделение
        /// </summary>
        public void Unsellect()
        {
            if (selectedFigure != null) selectedFigure.UnSelect();
            selectedFigure = null;
            if (selectedPoint != null) selectedPoint.UnSelect();
            selectedPoint = null;
            switch(mode)
            {
                case Mode.Arrow:
                    this.Cursor = Cursors.Arrow;
                    break;
                case Mode.Connect:
                    this.Cursor = Cursors.Cross;
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;
            }
        }

        /// <summary>
        /// Переход в режим выделения элементов
        /// </summary>
        public void SelectArrowMode()
        {
            if (mode != Mode.Arrow)
            {
                //TODO:
                Unsellect();
                AllChildrenIsSelectable = true;
                workArea.MouseLeftButtonDown += Canvas_Arrow_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown -= Canvas_Connect_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown -= Canvas_Procedure_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown -= Canvas_SubProcess_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown -= Canvas_Resource_MouseLeftButtonDown;
                foreach (GSFigure block in workArea.Children)
                {
                    (block as BlockWPF)?.Unfreeze();
                }

                mode = Mode.Arrow;

                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Переход в режим добавления процедур
        /// </summary>
        public void SelectProcedureMode()
        {
            if (mode != Mode.Procedure)
            {
                //TODO:
                Unsellect();
                AllChildrenIsSelectable = false;
                workArea.MouseLeftButtonDown -= Canvas_Arrow_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown -= Canvas_Connect_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown += Canvas_Procedure_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown -= Canvas_SubProcess_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown -= Canvas_Resource_MouseLeftButtonDown;
                foreach (GSFigure block in workArea.Children)
                {
                    (block as BlockWPF)?.Freeze();
                }

                mode = Mode.Procedure;
            }
        }

        /// <summary>
        /// Переход в режим добавления подпроцесса
        /// </summary>
        public void SelectSubProcessMode()
        {
            if (mode != Mode.SubProcess)
            {
                //TODO:
                Unsellect();
                AllChildrenIsSelectable = false;
                workArea.MouseLeftButtonDown -= Canvas_Arrow_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown -= Canvas_Connect_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown -= Canvas_Procedure_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown += Canvas_SubProcess_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown -= Canvas_Resource_MouseLeftButtonDown;
                foreach (GSFigure block in workArea.Children)
                {
                    (block as BlockWPF)?.Freeze();
                }

                mode = Mode.SubProcess;
            }
        }

        /// <summary>
        /// Переход в режим добавления ресурсов
        /// </summary>
        public void SelectResourseMode()
        {
            if (mode != Mode.Resourse)
            {
                //TODO:
                Unsellect();
                AllChildrenIsSelectable = false;
                workArea.MouseLeftButtonDown -= Canvas_Arrow_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown -= Canvas_Connect_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown -= Canvas_Procedure_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown -= Canvas_SubProcess_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown += Canvas_Resource_MouseLeftButtonDown;
                foreach (GSFigure block in workArea.Children)
                {
                    (block as BlockWPF)?.Freeze();
                }

                mode = Mode.Resourse;
            }
        }

        /// <summary>
        /// Переход в режим добавления связей
        /// </summary>
        public void SelectConnectMode()
        {
            if (mode != Mode.Connect)
            {
                //TODO:
                Unsellect();
                AllChildrenIsSelectable = true;
                workArea.MouseLeftButtonDown -= Canvas_Arrow_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown += Canvas_Connect_MouseLeftButtonDown;

                workArea.MouseLeftButtonDown -= Canvas_Procedure_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown -= Canvas_SubProcess_MouseLeftButtonDown;
                workArea.MouseLeftButtonDown -= Canvas_Resource_MouseLeftButtonDown;
                foreach (GSFigure block in workArea.Children)
                {
                    (block as BlockWPF)?.Freeze();
                }

                mode = Mode.Connect;
            }
            this.Cursor = Cursors.Cross;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            MakeStartAndEnd();
            SelectArrowMode();

            // Для обновления связей ресурсов под актуальные размеры
            foreach (var ch in workArea.Children)
            {
                if (ch is ResConnectionWPF)
                {
                    (ch as ResConnectionWPF).Refresh();
                }
            }
        }

        public void MakeStartAndEnd()
        {
            if (isHaveStartAndEnd == false)
            {
                double bord = 5;
                double d = SquareBlockWPF.DEFAULT_HEIGHT / 2.0;
                double x = workArea.ActualWidth;
                double y = workArea.ActualHeight / 2.0;

                workArea.Children.Add(new StartBlockWPF(new Point(bord, y - d)));
                workArea.Children.Add(new EndBlockWPF(new Point(x - bord - 2 * d, y - d)));

                isHaveStartAndEnd = true;
            }
        }

        /// <summary>
        /// Соединение 2х блоков
        /// </summary>
        /// <param name="block1"></param>
        /// <param name="block2"></param>
        /// <returns></returns>
        private bool Connect(BlockWPF block1, BlockWPF block2)
        {
            if( (block1 != null) && (block2 != null) )
            {
                if ((block1 is ResourceWPF) && (block2 is ResourceWPF))
                {
                    return false;
                }
                else
                {
                    if (((block1 is ResourceWPF) && (block2 is ProcedureWPF)) || ((block1 is ProcedureWPF) && (block2 is ResourceWPF)))
                    {
                        // Соединение процедуры и ресурса
                        return ResConnect(block1, block2);
                    }
                    else
                    {
                        // Соединение процедур
                        return ProcConnect(block1, block2);
                    }
                }
            }

            return false;
        }

        private bool ResConnect(BlockWPF block1, BlockWPF block2)
        {
            ResConnectionWPF connection = new ResConnectionWPF(block1, block2);

            if(block1 is ResourceWPF)
            {
                (block1 as ResourceWPF).AddResPutConnection(connection);
            }
            else
            {
                (block1 as ProcedureWPF).AddResPutConnection(connection);
            }
            if (block2 is ResourceWPF)
            {
                (block2 as ResourceWPF).AddResPutConnection(connection);
            }
            else
            {
                (block2 as ProcedureWPF).AddResPutConnection(connection);
            }

            workArea.Children.Add(connection);
            return true;
        }

        private bool ProcConnect(BlockWPF block1, BlockWPF block2)
        {
            int aPort = 0, bPort = 0;

            if ((block1 is ResourceWPF) || 
                (block1 is EndBlockWPF) || 
                (block2 is ResourceWPF) ||
                (block2 is StartBlockWPF)) return false;

            // TODO: Добавить обработку и вывод ошибок


            // расчёт локальных смещений
            Point a = block1.RightPosition - (Vector)block1.Position;
            Point b = block2.LeftPosition - (Vector)block2.Position;
            
            if((selectedPoint == null) && !(block1 is StartBlockWPF)) return false; // случай, когда выбран блок, но не выбрана точка из которой будет выходить соединение

            if (selectedPoint != null)
            {
                a = selectedPoint.Position;
                aPort = selectedPoint.Port;
            }

            // если второй блок ProcedureWPF, то для соединения, необходимо, 
            // чтобы была выбрана одна из точек входа этого блока
            if (block2 is ProcedureWPF)
            {
                bool someSelect = false;

                foreach (UIElement child in block2.Children)
                {
                    if (child.IsMouseOver)
                    {
                        if (child is ConnectPointWPF)
                        {
                            
                            ConnectPointWPF point = child as ConnectPointWPF;
                            if (point.ConnectType == ConnectPointWPF_Type.inPut)
                            {
                                //selectedPoint = point;
                                //selectedPoint.Select();
                                b = point.Position;
                                bPort = point.Port;
                                someSelect = true;
                            }
                        }
                    }
                }

                if (!someSelect) return false;
            }

            ProcConnectionWPF connection = new ProcConnectionWPF(block1, block2, a, b, aPort, bPort);

            if (block1 is StartBlockWPF)
            {
                (block1 as StartBlockWPF).AddOutPutConnection(connection);
            }
            else
            {
                (block1 as ProcedureWPF).AddOutPutConnection(connection);
            }
            if (block2 is EndBlockWPF)
            {
                (block2 as EndBlockWPF).AddInPutConnection(connection);
            }
            else
            {
                (block2 as ProcedureWPF).AddInPutConnection(connection);
            }

            workArea.Children.Add(connection);
            return true;
        }

        //TODO это явно неправильно, нужно что-то поумнее
        public UIElementCollection Children
        {
            get => workArea.Children;
        }

        /// <summary>
        /// Удаляет вылеленный элемент (работает в режиме указателя)
        /// </summary>
        public void DeleteSelected()
        {
            // работает в режиме указателя
            if (mode == Mode.Arrow)
            {
                selectedFigure?.Remove();
            }
        }
        
    }
}
