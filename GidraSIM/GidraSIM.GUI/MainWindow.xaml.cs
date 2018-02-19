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
using System.Windows.Media;
using System.Windows.Media.Effects;

using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model.Procedures;

namespace GidraSIM.GUI
{
    public enum Theme { Classic, Dark }

    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Theme theme;
        private string savePath = "";

        //переменная для именования процессов
        int processNamesCounter = 1;
        Process mainProcess = new Process() { Description = "Процесс 1" };
        int mainProcessNumber;

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

        Brush imageBackground;

        public MainWindow()
        {
            InitializeComponent();

            // Стандартные команды
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, NewProjectItemMenu_Click));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Click));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Click));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed));

            // Кастомные команды
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Arrow, Arrow_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Procedure, Procedure_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Resourse, Resourse_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.Connect, Connect_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.SubProcess, SubProcess_Executed));
            //this.CommandBindings.Add(new CommandBinding(MainWindowCommands.StartCheck, StartCheck_Executed));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.StartModeling, StartModeling_Executed));


            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.BlackTheme, DarkThemeMenuItem_Click));
            this.CommandBindings.Add(new CommandBinding(MainWindowCommands.WhiteTheme, ClassicThemeMenuItem_Click));

            imageBackground = DockPanel1.Background;


            theme = Theme.Classic;

            // Добавление первого процесса
            CreateNewProject();


            PushButton(button_arrow);
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

        private void Procedure_Executed(object sender, RoutedEventArgs e)
        {
            if (drawAreas.First().Mode == Mode.Procedure)
            {
                SwitchToArrowMode();
            }
            else
            {
                SwitchTProcedureMode();
            }
        }

        private void SwitchTProcedureMode()
        {
            //меняем режим для всех на процедуры
            drawAreas.ForEach(area => area.SelectProcedureMode());

            PushButton(button_procedure);
        }

        private void Arrow_Executed(object sender, RoutedEventArgs e)
        {
            SwitchToArrowMode();
        }

        /// <summary>
        /// Реализация эффекта нажатой кнопки
        /// </summary>
        /// <param name="pushedButton">Кнопка, которую нужно нажать</param>
        private void PushButton(Button pushedButton)
        {
            // снятие выделения со всех кнопок
            foreach (var buttonObj in buttonPanel.Children)
            {
                if(buttonObj is Button)
                {
                    (buttonObj as Button).Effect = null;
                    (buttonObj as Button).Opacity = 1;
                }
            }

            // добавление выделения на pushedButton
            pushedButton.Effect = new BlurEffect() { Radius = 4 };
            pushedButton.Opacity = 0.8;
        }

        private void SwitchToArrowMode()
        {
            //меняем режим для всех на выделение
            drawAreas.ForEach(area => area.SelectArrowMode());

            PushButton(button_arrow);
        }

        private void Pr(object sender, MouseButtonEventArgs e)
        {

        }
        /// <summary>
        /// ресурс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Resourse_Executed(object sender, RoutedEventArgs e)
        {
            if (drawAreas.First().Mode == Mode.Resourse)
            {
                SwitchToArrowMode();
            }
            else
            {
                SwitchToResourseMode();
            }
        }

        private void SwitchToResourseMode()
        {
            //меняем режим для всех на ресурсы
            drawAreas.ForEach(area => area.SelectResourseMode());

            PushButton(button_resourse);
        }


        /// <summary>
        /// связи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Executed(object sender, RoutedEventArgs e)
        {
            if (drawAreas.First().Mode == Mode.Connect)
            {
                SwitchToArrowMode();
            }
            else
            {
                SwitchToConnectMode();
            }
        }

        private void SwitchToConnectMode()
        {
            //меняем для всех на связи
            drawAreas.ForEach(area => area.SelectConnectMode());

            PushButton(button_connect);
        }

        /// <summary>
        /// подпроцесс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubProcess_Executed(object sender, RoutedEventArgs e)
        {
            if (drawAreas.First().Mode == Mode.SubProcess)
            {
                SwitchToArrowMode();
            }
            else
            {
                SwitchToSubProcessMode();
            }
        }

        private void SwitchToSubProcessMode()
        {
            //меняем для всех на подпроцессы
            drawAreas.ForEach(area => area.SelectSubProcessMode());

            PushButton(button_SubProcess);
        }

        private void StartModeling_Executed(object sender, RoutedEventArgs e)
        {

            try
            {
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

                //TokenViewer show = new TokenViewer(mainProcess.TokenCollector as TokensCollector);
                //show.Show();

                //Statictics();

                //TODO сделать DataBinding
                listBox1.Items.Clear();

                //добавляем ещё инцидентры в историю
                AccidentsCollector collector = AccidentsCollector.GetInstance();
                collector.GetHistory().ForEach(item => listBox1.Items.Add(item));

                ResultWindow resultWindow = new ResultWindow(mainProcess.Collector.GetHistory(), collector.GetHistory(),  this.complexity);
                resultWindow.ShowDialog();
                mainProcess.Collector.GetHistory().ForEach(item => listBox1.Items.Add(item));



                mainProcess.Collector.GetHistory().Clear();
                collector.GetHistory().Clear();

                //выводим число токенов и время затраченное(в заголовке)
                //MessageBox.Show("Время, затраченное на имитацию " + modelingTime.Now.ToString(), "Имитация закончена");
            }
            catch (NotImplementedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                foreach (var process in processes)
                {
                    process.ClearProcess();
                }
            }
        }


        private void CreateProcessButton_Click(object sender, RoutedEventArgs e)
        {
            //создаём новый процесс
            var process = new Process() { Description = "Процесс " + (++this.processNamesCounter) };
            //добавляем в список всех процессов
            processes.Add(process);
            //надеюсь, что заголовок будет содержать название
            var tabItem = new TabItem() { Header = process };
            //переключаемся на новую вкладку, чтобы не было проблем с добавлением
            testTabControl.SelectedItem = tabItem;
            //теперь создаём область рисования
            var drawArea = new DrawArea();
            //добавляем ссылку на все ресурсы
            drawArea.Processes = processes;
            drawArea.Mode = drawAreas.First().Mode;
            //добавляем в список
            drawAreas.Add(drawArea);
            //добавляем на вкладку
            tabItem.Content = drawArea;
            //и добавляем вкладку
            testTabControl.Items.Add(tabItem);

            //применение темы для новой вкладки
            SetTheme();
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
            ComplexitySelectionDialog dialog = new ComplexitySelectionDialog();
            if (dialog.ShowDialog() == true)
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
                    var saver = new ProjectSaver();
                    saver.SaveProjectExecute(testTabControl, savePath, mainProcessNumber);
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
            try
            {

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Project",
                    DefaultExt = ".xml",
                    Filter = "(XMl documents .xml)|*.xml"
                };

                if ((bool)dlg.ShowDialog())
                {
                    var saver = new ProjectSaver();
                    saver.SaveProjectExecute(testTabControl, dlg.FileName, mainProcessNumber);
                    savePath = dlg.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Open()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
                {
                    FileName = "Project",
                    DefaultExt = ".xml",
                    Filter = "(XML documents .xml)|*.xml"
                };

                if (dlg.ShowDialog() == true)
                {
                    this.savePath = dlg.FileName;

                    ProjectSaver saver = new ProjectSaver();
                    mainProcessNumber = saver.LoadProjectExecute(dlg.FileName, testTabControl, drawAreas, processes, out mainProcess);
                    processNamesCounter = testTabControl.Items.Count;
                    SetTheme();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClassicThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //ClassicThemeMenuItem.IsChecked = true;
            //DarkThemeMenuItem.IsChecked = false;

            SetClassicTheme();
        }

        private void DarkThemeMenuItem_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ClassicThemeMenuItem_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void DarkThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //background FF2D2D30
            //2D2D30

            //ClassicThemeMenuItem.IsChecked = false;
            //DarkThemeMenuItem.IsChecked = true;

            SetDarkTheme();
        }

        private void SetClassicTheme()
        {

            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("classic.xaml", UriKind.Relative);

            // Динамически меняем коллекцию MergedDictionaries
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictionary);


            //Background="#BBFFFFFF
            foreach (var tab in testTabControl.Items)
            {
                ((tab as TabItem).Content as DrawArea).workArea.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBFFFFFF"));
            }
            //testTabControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBFFFFFF"));
            //DockPanel1.Background = imageBackground;
            //listBox1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBFFFFFF"));
        }

        private void SetDarkTheme()
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/Selen.Wpf.SystemStyles;component/Styles.xaml", UriKind.RelativeOrAbsolute);
            ResourceDictionary myDictionary = new ResourceDictionary();
            myDictionary.Source = new Uri("dark.xaml", UriKind.Relative);
            //DockPanel1.Background = ;

            // Динамически меняем коллекцию MergedDictionaries
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
            Application.Current.Resources.MergedDictionaries.Add(myDictionary);

            foreach (var tab in testTabControl.Items)
            {
                ((tab as TabItem).Content as DrawArea).workArea.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526"));
            }

            //DockPanel1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2D2D30"));
            //listBox1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526"));
        }

        private void SaveStatistic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                throw new NotImplementedException("В разработке");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Statictics()
        {
            using (FileStream stream = new FileStream($"tokens-log-{System.DateTime.Now.ToString("dd.MM.yyyy hh-mm-ss")}.xml", FileMode.Create))
            {
                Type[] types = new Type[]
                {
                typeof(AndBlock),
                typeof(DuplicateOutputsBlock),
                typeof(CadResource),
                typeof(WorkerResource),
                typeof(TechincalSupportResource),
                typeof(MethodolgicalSupportResource),
                typeof(TokensCollector),
                typeof(ConnectionManager),
                typeof(ArrangementProcedure),
                typeof(Assembling),
                typeof(ClientCoordinationPrrocedure),
                typeof(DocumentationCoordinationProcedure),
                typeof(ElectricalSchemeSimulation),
                typeof(FixedTimeBlock),
                typeof(FormingDocumentationProcedure),
                typeof(Geometry2D),
                typeof(KDT),
                typeof(KinematicСalculations),
                typeof(PaperworkProcedure),
                typeof(QualityCheckProcedure),
                typeof(SampleTestingProcedure),
                typeof(SchemaCreationProcedure),
                typeof(StrengthСalculations),
                typeof(TracingProcedure),
                typeof(Process)
                };

                System.Runtime.Serialization.DataContractSerializer ser = new System.Runtime.Serialization.DataContractSerializer(typeof(TokensCollector), types);
                ser.WriteObject(stream, TokensCollector.GetInstance());
            }
        }

        private void NewProjectItemMenu_Click(object sender, RoutedEventArgs e)
        {
            CreateNewProject();
        }

        private void CreateNewProject()
        {
            // Устанавливаем дефолтные значения
            testTabControl.Items.Clear();
            drawAreas.Clear();
            processes.Clear();

            mainProcess = new Process() { Description = "Процесс 1" };
            mainProcessNumber = 0;
            processNamesCounter = 1;

            // Создаём процесс
            //Process process = new Process() { Description = mainProcess.Description };
            processes.Add(mainProcess); // Добавляем в список процессов

            var tabItem = new TabItem() { Header = mainProcess };
            testTabControl.SelectedItem = tabItem;

            // Создаём область рисования
            var drawArea = new DrawArea()
            {
                Processes = processes
            };

            drawAreas.Add(drawArea);
            tabItem.Content = drawArea;

            testTabControl.Items.Add(tabItem);

            SetTheme();
        }

        private void SetTheme()
        {
            switch (theme)
            {
                case Theme.Classic:
                    {
                        SetClassicTheme();
                        break;
                    }
                case Theme.Dark:
                    {
                        SetDarkTheme();
                        break;
                    }
            }
        }

        private void TokenViewerItemMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TokenViewer viewer = new TokenViewer();
                viewer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AdmSet.SettingsView settingsWindow = new AdmSet.SettingsView();
            if(settingsWindow.ShowDialog() == true)
            {

            }

        }
    }
}
