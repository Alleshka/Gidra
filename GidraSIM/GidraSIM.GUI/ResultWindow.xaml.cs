using GidraSIM.Core.Model;
using System;
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

            var Token = tokens.ToList()[0];

            var tokens2 = from token in tokens
                          select new
                          {
                               token.ProcessedByBlock,
                               token.BornTime,
                               token.Parent,
                               token.ProcessStartTime,
                               token.ProcessEndTime
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
