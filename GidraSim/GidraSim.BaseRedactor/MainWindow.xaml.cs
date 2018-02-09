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
using System.Windows.Navigation;
using System.Windows.Shapes;

using GidraSIM.Core.Model.Resources;
using GidraSIM.DataLayer.MSSQL;

namespace GidraSim.BaseRedactor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String _connectionString = "Data Source=DESKTOP-H4JQP0V;Initial Catalog=SimSapr;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public MainWindow()
        {
            InitializeComponent();

            _connectionString = System.Configuration.ConfigurationManager.
                ConnectionStrings["connectionString"].ConnectionString;
        }

        private void CpuAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var redactor = new CPURedactor();

                if (redactor.ShowDialog() == true)
                {
                    CpuRepository repository = new CpuRepository(_connectionString);
                    repository.Create(redactor.curCPU);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GpuAddItem_Click(object sender, RoutedEventArgs e)
        {
            var redactor = new GPURedactor();
            if (redactor.ShowDialog() == true)
            {
                var repository = new GpuRepository(_connectionString);
                repository.Create(redactor.curGPU);
            }
        }

        private void InfSupAddItem_Click(object sender, RoutedEventArgs e)
        {
            var redactor = new InformationSupportRedactor();
            if (redactor.ShowDialog() == true)
            {
                var repository = new InformationSupportRepository(_connectionString);
                repository.Create(redactor.curINF);
            }
        }
    }
}
