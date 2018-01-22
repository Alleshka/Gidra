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
        public ResultWindow(ICollection<Token> tokens, double complexity)
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
            double wastedTime = 0;
            double totalTime = 0;
            foreach(var token in tokens)
            {
                wastedTime += token.ProcessStartTime - token.BornTime;
                totalTime += token.ProcessEndTime - token.BornTime;
            }
            this.WastedTime.Text = wastedTime.ToString();
            this.SummaryTime.Text = totalTime.ToString();
            this.EffectiveTime.Text = (totalTime - wastedTime).ToString();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
