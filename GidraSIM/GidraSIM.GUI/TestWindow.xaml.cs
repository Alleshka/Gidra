using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GidraSIM.GUI.Core.BlocksWPF;
using GidraSIM.GUI.Utility;
using GidraSIM.Core.Model;
using System.IO;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private string savePath = "";

        //переменная для именования процессов
        int processNamesCounter = 1;
        Process mainProcess = new Process(new TokensCollector()) { Description = "Процесс 1" };
        List<Process> processes = new List<Process>();
        List<DrawArea> drawAreas = new List<DrawArea>();

        /// <summary>
        /// сложность начальной задачи
        /// </summary>
        double complexity = 10;

        /// <summary>
        /// шаг моделирования
        /// </summary>
        double dt = 1;

        /// <summary>
        /// максимальное время моделирования
        /// </summary>
        double maxTime = 1000;

        public TestWindow()
        {
            InitializeComponent();

            //добавление первого процесса

            var drawArea = new DrawArea();
            drawArea.Processes = processes;
            //добавляем область рисования
            drawAreas.Add(drawArea);
            //запихиваем область рисования во вкладку
            (this.testTabControl.Items[0] as TabItem).Content = drawArea;
            //переименовываем вкладку
            (this.testTabControl.Items[0] as TabItem).Header = mainProcess;
            //доабавляем процесс в список
            processes.Add(mainProcess);

            // Стандартные команды
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, Create_Project_Executed));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Project_Executed));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Project_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed));

            // Кастомные команды
            //this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Arrow, Arrow_Executed));
            //this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Procedure, Procedure_Executed));
            //this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Resourse, Resourse_Executed));
            //this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Connect, Connect_Executed));
            //this.CommandBindings.Add(new CommandBinding(MainWindowCommands.SubProcess, SubProcess_Executed));
            //this.CommandBindings.Add(new CommandBinding(MainWindowCommands.StartCheck, StartCheck_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.StartModeling, StartModeling_Executed));
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

        private void StartModeling_Executed(object sender, RoutedEventArgs e)
        {
            //try
            //{
                ViewModelConverter converter = new ViewModelConverter();

                //запихиваем содержимое области рисования в процесс
                foreach (var item in testTabControl.Items)
                {
                    var tab = item as TabItem;
                    var drawArea = tab.Content as DrawArea;
                    converter.Map(drawArea.Children, tab.Header as Process);
                }

                ////запихиваем содержимое главной области рисования в процесс
                //converter.Map(drawAreas[0].Children, mainProcess);

                //добавляем на стартовый блок токен
                mainProcess.AddToken(new Token(0, complexity), 0);
                //double i = 0;
                ModelingTime modelingTime = new ModelingTime() { Delta = this.dt, Now = 0 };
                for (modelingTime.Now = 0; modelingTime.Now < maxTime; modelingTime.Now += modelingTime.Delta)
                {
                    mainProcess.Update(modelingTime);
                    //на конечном блоке на выходе появился токен
                    if (mainProcess.EndBlockHasOutputToken)
                    {
                        break;
                    }
                }

                //TODO сделать DataBinding
                listBox1.Items.Clear();
                mainProcess.Collector.GetHistory().ForEach(item => listBox1.Items.Add(item));
                mainProcess.Collector.GetHistory().Clear();

                //выводим число токенов и время затраченное (в заголовке)
                MessageBox.Show("Время, затраченное на имитацию " + modelingTime.Now.ToString(), "Имитация закончена");
                foreach (var process in processes)
                {
                    process.ClearProcess();
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
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
            //добавляем ссылку на все ресурсы
            drawArea.Processes = processes;
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
            if (testTabControl.Items.Count > 0)
            {
                var drawArea = (testTabControl.SelectedItem as TabItem).Content as DrawArea;
                drawAreas.ForEach(area => area.Unsellect());
            }
            //drawAreas.ForEach(area => area.HideElements());

            //drawArea.ShowElements();

            //drawAreas.ForEach(area => area.UpdateLayout());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }

        private void ModelingParametersButton_Click(object sender, RoutedEventArgs e)
        {
            TestComplexitySelectionDialog dialog = new TestComplexitySelectionDialog();
            if(dialog.ShowDialog() == true)
            {
                complexity = dialog.Complexity;
                dt = dialog.Step;
                maxTime = dialog.MaxTime;
            }
        }
        private void SaveAsItemMenu_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void Save()
        {
            try
            {
                if (savePath.Count() != 0)
                {
                    ProjectSaver saver = new ProjectSaver();
                    saver.SaveProjectExecute(testTabControl, savePath);
                }
                else SaveAs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveAs()
        {
            //try
            //{
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Project",
                    DefaultExt = ".json",
                    Filter = "JSON documents .json)|*.json"
                };

                if ((bool)dlg.ShowDialog())
                {
                    ProjectSaver saver = new ProjectSaver();
                    saver.SaveProjectExecute(testTabControl, dlg.FileName);
                    savePath = dlg.FileName;
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void Open()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Project",
                DefaultExt = ".json",
                Filter = "JSON documents .json)|*.json"
            };

            if (dlg.ShowDialog() == true)
            {
                savePath = dlg.FileName;

                // Удаляем все элементы
                testTabControl.Items.Clear();
                processes.Clear();
                drawAreas.Clear();

                // Выгружаем проект
                ProjectSaver saver = new ProjectSaver();
                var temp = saver.LoadProjecExecute(savePath); // Считываем сохранение


                //Проходим по считанным процессам
                foreach (var proc in temp._processes)
                {
                    // Создаём новый процесс
                    var process = new Process(mainProcess.Collector) { Description = proc.ProcessName };
                    processes.Add(process); // Добавляем в список

                    var tabItem = new TabItem() { Header = process };
                    testTabControl.SelectedItem = tabItem;

                    // Создаём область рисование
                    var drawArea = new DrawArea
                    {
                        Processes = processes,
                    };

                    //добавляем в список
                    drawAreas.Add(drawArea);
                    //добавляем на вкладку
                    tabItem.Content = drawArea;

                    tabItem.Header = mainProcess;
                    //доабавляем процесс в список
                    processes.Add(mainProcess);

                    //и добавляем вкладку
                    testTabControl.Items.Add(tabItem);
                    // Выгружаем элементы
                    saver.LoadProcessExecute(proc, drawArea);

                    this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed));
                    this.CommandBindings.Add(new CommandBinding(MainWindowCommands.StartModeling, StartModeling_Executed));
                }
            }
        }
    }
}
