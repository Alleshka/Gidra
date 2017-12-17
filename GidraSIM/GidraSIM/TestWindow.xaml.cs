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
using GidraSIM.Utility;
using GidraSIM.Core.Model;

namespace GidraSIM
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        Process process = new Process(new TokensCollector());

        public TestWindow()
        {
            InitializeComponent();


            //
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed));
        }

        private void Delete_Executed(object sender, RoutedEventArgs e)//выбрали пункт меню Удалить
        {
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
            drawArea.SelectProcedureMode();
            //testTabControl.MouseLeftButtonDown -= Cursor_MouseDown;
            //testTabControl.MouseLeftButtonDown += Procedure_MouseDown;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            drawArea.SelectArrowMode();
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
            drawArea.SelectResourseMode();
        }
        /// <summary>
        /// связи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            drawArea.SelectConnectMode();
        }

        /// <summary>
        /// подпроцесс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            drawArea.SelectSubProcessMode();
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModelConverter converter = new ViewModelConverter();
            //drawArea.Children;
            converter.Map(drawArea.Children, process);
            //добавляем на стартовый блок токен
            process.AddToken(new Token(0, 100), 0);
            //double i = 0;
            ModelingTime modelingTime = new ModelingTime() { Delta = 1, Now = 0 };
            for(modelingTime.Now=0;modelingTime.Now<1000 ;modelingTime.Now+=modelingTime.Delta)
            {
                process.Update(modelingTime);
                //на конечном блоке на выходе появился токен
                if(process.EndBlockHasOutputToken)
                {
                    break;
                }
            }

            //выводим число токенов и время затраченное (в заголовке)
            MessageBox.Show(process.Collector.GetHistory().Count.ToString(), modelingTime.Now.ToString());
            process.Collector.GetHistory().Clear();
        }
    }
}
