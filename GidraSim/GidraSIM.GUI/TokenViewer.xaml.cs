﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.GUI.Core.BlocksWPF;
using GidraSIM.Core.Model.Resources;

/// <summary>
/// TODO: ООООочень кривой способ, рисовался на коленке, надо переделать
/// </summary>

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для TokenViewer.xaml
    /// </summary>
    public partial class TokenViewer : Window
    {
        private TokensCollector _collector; // Принимаемый коллектор

        private double MinDuration; // Длительность минимального процесса

        // Начальные координаты
        private double baseX = 20;
        private double baseY = 20;

        private void StartView()
        {
            // Ставим, что метки проставлены
            MainWindow.IsHaveStartEnd = true;
            MainWindow.Children.Clear(); // Убираем начальный и конечный блоки*

            AnalizeToken(); // Смотрим полученный токен
        }

        public TokenViewer()
        {
            // Создаём форму
            InitializeComponent();

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "TokenCollectorlog",
                DefaultExt = ".xml",
                Filter = "(XML documents .xml)|*.xml"
            };

            if (dlg.ShowDialog() == true)
            {
                using (System.IO.FileStream stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Open))
                {
                    Type[] types = new Type[] {
                        typeof(CadResource),
                        typeof(WorkerResource),
                        typeof(TechincalSupportResource),
                        typeof(MethodolgicalSupportResource),
                        typeof(TokensCollector),
                        typeof(ConnectionManager),
                        typeof(ArrangementProcedure),
                        typeof(ClientCoordinationPrrocedure),
                        typeof(DocumentationCoordinationProcedure),
                        typeof(ElectricalSchemeSimulation),
                        typeof(FixedTimeBlock),
                        typeof(FormingDocumentationProcedure),
                        typeof(PaperworkProcedure),
                        typeof(QualityCheckProcedure),
                        typeof(SampleTestingProcedure),
                        typeof(SchemaCreationProcedure),
                        typeof(TracingProcedure),
                        typeof(Process)};

                    System.Runtime.Serialization.DataContractSerializer ser = new System.Runtime.Serialization.DataContractSerializer(typeof(TokensCollector), types);
                    _collector = (TokensCollector) ser.ReadObject(stream);
                    StartView();
                }
            }
            else
            {
                throw new ArgumentNullException("Файл не выбран");
            }

        }

        public TokenViewer(TokensCollector collector)
        {
            // Создаём форму
            InitializeComponent();
            _collector = collector; // Сохраняем коллектор

            StartView(); // Запускаем работу
        }

        private void AnalizeToken()
        {
            var list = _collector.GetHistory(); // Получаем список токенов
            list = list.OrderBy(x => x.ProcessStartTime).ToList(); // Выстраиваем по времени начала

            MinDuration = list.Min(x => x.ProcessEndTime - x.ProcessStartTime); // Находим минимальную продолжительность

            // Проставляем блоки
            foreach (var l in list)
            {
                AddBlock(l);
            }

            // Проставляем связи (так как пока нет параллельности, идут друг за другом)
            int count = MainWindow.Children.Count; // Смотрим количество блоков из которых пойдёт связь

            this.Width = (count+1) * ProcedureWPF.DEFAULT_WIDTH;

            for (int i = 0; i < count; i++)
            {
                if (i < count - 1)
                {
                    // Берём начальный и конечный блоки
                    ProcedureWPF Start = MainWindow.Children[i] as ProcedureWPF;
                    ProcedureWPF End = MainWindow.Children[i+1] as ProcedureWPF;

                    // Создаём связь
                    ProcConnectionWPF connectionWPF = new ProcConnectionWPF(Start, End, new Point(ProcedureWPF.DEFAULT_WIDTH, ProcedureWPF.DEFAULT_HEIGHT/2), new Point(ProcedureWPF.DEFAULT_WIDTH, ProcedureWPF.DEFAULT_HEIGHT / 2));

                    // Добавляем связь к ресурсам
                    Start.AddOutPutConnection(connectionWPF);
                    End.AddInPutConnection(connectionWPF);

                    // Добавляем связь на область
                    MainWindow.Children.Add(connectionWPF);
                }
            }


        }

        private void AddBlock(Token block)
        {
            // Смотрим длительность
            double duration = (block.ProcessEndTime - block.ProcessStartTime);
            if (duration == 0) duration = 1;

            if (MinDuration == 0) MinDuration = 1;
            int count = Convert.ToInt32(duration / MinDuration); // Количество блоков, которые создадим (чтобы отобразить длительность)

            if (count > 5) count = 5; // Чтобы не выводить миллиард блоков

            for (int i = 0; i < count; i++)
            {
                //Создаём блок
                ProcedureWPF wpf = new ProcedureWPF(new Point(this.baseX+i*ProcedureWPF.DEFAULT_WIDTH, this.baseY), block.ProcessedByBlock);
                // Добавляем на рабочую область
                MainWindow.Children.Add(wpf);
            }

            // Передвигаем следующий
            baseX += count*ProcedureWPF.DEFAULT_WIDTH + 15;
            baseY += ProcedureWPF.DEFAULT_HEIGHT + 15;
        }
    }
}
