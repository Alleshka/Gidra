using GidraSIM.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GidraSIM.GUI
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(ICollection<Token> tokens, ICollection<Accident> accidents, double complexity)
        {
            InitializeComponent();
            this.Complexity.Text = complexity.ToString();

            var tokens2 = from token in tokens
                          select new
                          {
                               Описание = token.ProcessedByBlock.Description,
                               Создан = token.BornTime,
                               Из = token.Parent == null ? " " : token.Parent.Description,
                               Начало = token.ProcessStartTime,
                               Конец = token.ProcessEndTime
                          };

            this.Tokens.ItemsSource = tokens2;

            var accidents2 = from accident in accidents
                          select new
                          {
                              Описание = accident.Description,
                              Источник = accident.Source == null ? " " : accident.Source.Description,
                              Начался = accident.StartTime,
                              Закончился = accident.EndTime
                          };
            this.Accidents.ItemsSource = accidents2;

            double wastedTime = 0;
            double totalTime = 0;
            foreach(var token in tokens)
            {
                //wastedTime += token.ProcessStartTime - token.BornTime;
                totalTime += token.ProcessEndTime - token.BornTime;
            }

            foreach (var accident in accidents)
            {
                wastedTime += accident.EndTime - accident.StartTime;
            }
            this.WastedTime.Text = wastedTime.ToString();
            this.SummaryTime.Text = totalTime.ToString();
            this.EffectiveTime.Text = (totalTime - wastedTime).ToString();
            this.button1.Focus();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
