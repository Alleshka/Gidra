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
using System.Windows.Shapes;

using GidraSIM.GUI.Core.BlocksWPF;
using GidraSIM.GUI.Utility;
using GidraSIM.Core.Model;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        //переменная для именования процессов
        int processNamesCounter = 1;
        Process mainProcess = new Process(new TokensCollector()) { Description = "Процесс 1" };
        List<Process> processes = new List<Process>();
        List<DrawArea> drawAreas = new List<DrawArea>();

        public TestWindow()
        {
            InitializeComponent();

            //добавление первого процесса
            //добавляем область рисования
            drawAreas.Add(new DrawArea());
            //запихиваем область рисования во вкладку
            (this.testTabControl.Items[0] as TabItem).Content = drawAreas[0];
            //переименовываем вкладку
            (this.testTabControl.Items[0] as TabItem).Header = mainProcess;
            //доабавляем процесс в список
            processes.Add(mainProcess);


            //
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed));
        }

        private void Delete_Executed(object sender, RoutedEventArgs e)//выбрали пункт меню Удалить
        {
            //берём от текущей вкладки
            var drawArea = (testTabControl.SelectedItem as TabItem).Content as DrawArea;
            drawArea.DeleteSelected();
        }

        /// <summary>
        /// Событие нажатия клавиши в режиме указатель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cursor_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        /*
        /// <summary>
        /// Событие нажатия клавиши в режиме процедура
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Procedure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point cursorPoint = e.GetPosition(workCanvas);
            Point procedurePosition = (Point)(cursorPoint - new Point(ProcedureWPF.WIDTH / 2, ProcedureWPF.HEIGHT / 2));
            ProcedureWPF proc = new ProcedureWPF(procedurePosition, "Процедура");
            workCanvas.Children.Add(proc);
            MoveBlock(proc);
        }
        */
        private void MoveBlock(BlockWPF block)
        {
            Canvas.SetTop(block, block.Position.Y);
            Canvas.SetLeft(block, block.Position.X);
        }
        
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //меняем режим для всехна процедуры
            drawAreas.ForEach(area => area.SelectProcedureMode());
            //testTabControl.MouseLeftButtonDown -= Cursor_MouseDown;
            //testTabControl.MouseLeftButtonDown += Procedure_MouseDown;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //меняем режим для всех на выделение
            drawAreas.ForEach(area => area.SelectArrowMode());
            //testTabControl.MouseLeftButtonDown -= Procedure_MouseDown;
            //testTabControl.MouseLeftButtonDown += Cursor_MouseDown;
        }
        
        private void Pr(object sender, MouseButtonEventArgs e)
        {

        }
        /// <summary>
        /// ресурс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //меняем режим для всех на ресурсы
            drawAreas.ForEach(area => area.SelectResourseMode());
        }
        /// <summary>
        /// связи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //меняем для всех на связи
            drawAreas.ForEach(area => area.SelectConnectMode());
        }

        /// <summary>
        /// подпроцесс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            //меняем для всех на подпроцессы
            drawAreas.ForEach(area => area.SelectSubProcessMode());
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModelConverter converter = new ViewModelConverter();

            //запихиваем содержимое области рисования в процесс
            foreach (var item in testTabControl.Items)
            {
                var tab = item as TabItem;
                var drawArea = tab.Content as DrawArea;
                converter.Map( drawArea.Children, tab.Header as Process);
            }

            ////запихиваем содержимое главной области рисования в процесс
            //converter.Map(drawAreas[0].Children, mainProcess);

            //добавляем на стартовый блок токен
            mainProcess.AddToken(new Token(0, 10), 0);
            //double i = 0;
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for(modelingTime.Now=0;modelingTime.Now<1000 ;modelingTime.Now+=modelingTime.Delta)
            {
                mainProcess.Update(modelingTime);
                //на конечном блоке на выходе появился токен
                if(mainProcess.EndBlockHasOutputToken)
                {
                    break;
                }
            }

            //TODO сделать DataBinding
            listBox1.Items.Clear();
            mainProcess.Collector.GetHistory().ForEach(item => listBox1.Items.Add(item));

            //выводим число токенов и время затраченное (в заголовке)
            MessageBox.Show(modelingTime.Now.ToString());
            mainProcess.Collector.GetHistory().Clear();
            mainProcess = new Process(new TokensCollector());
        }

        private void CreateProcessButton_Click(object sender, RoutedEventArgs e)
        {
            //создаём новый процесс
            var process = new Process(mainProcess.Collector) { Description = "Процесс "+ (++this.processNamesCounter)};
            //добавляем в список всех процессов
            processes.Add(process);
            //надеюсь, что заголовок будет содержать название
            var tabItem = new TabItem() { Header = process};
            //переключаемся на новую вкладку, чтобы не было проблем с добавлением
            testTabControl.SelectedItem = tabItem;
            //теперь создаём область рисования
            var drawArea = new DrawArea();
            //добавляем в список
            drawAreas.Add(drawArea);
            //добавляем на вкладку
            tabItem.Content = drawArea;
            //и добавляем вкладку
            testTabControl.Items.Add(tabItem);
        }

        private void testTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    MessageBox.Show("Selected tab " + (testTabControl.SelectedItem as TabItem).Header.ToString());
            //}
            //catch( InvalidOperationException )
            //{
            //    //
            //}
            var drawArea = (testTabControl.SelectedItem as TabItem).Content as DrawArea;
            

            drawAreas.ForEach(area => area.Unsellect());
            //drawAreas.ForEach(area => area.HideElements());

            //drawArea.ShowElements();

            //drawAreas.ForEach(area => area.UpdateLayout());
        }
    }
}
