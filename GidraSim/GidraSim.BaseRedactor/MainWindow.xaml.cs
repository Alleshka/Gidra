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

        CpuRepository cpuRepository;
        GpuRepository gpuRepository;
        InformationSupportRepository informationSupportRepository;

        private  string[] types = new string[]
        {
            "GPU",
            "CPU",
            "Information support"
        };

        public MainWindow()
        {
            InitializeComponent();



            listBox1.ItemsSource = types;

            _connectionString = System.Configuration.ConfigurationManager.
                ConnectionStrings["connectionString"].ConnectionString;


            cpuRepository = new CpuRepository(_connectionString);
            gpuRepository = new GpuRepository(_connectionString);
            informationSupportRepository = new InformationSupportRepository(_connectionString);
        }

        private void CpuAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var redactor = new CPURedactor();

                if (redactor.ShowDialog() == true)
                {
                    //CpuRepository repository = new CpuRepository(_connectionString);
                    cpuRepository.Create(redactor.curCPU);
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
                //var repository = new GpuRepository(_connectionString);
                gpuRepository.Create(redactor.curGPU);
            }
        }

        private void InfSupAddItem_Click(object sender, RoutedEventArgs e)
        {
            var redactor = new InformationSupportRedactor();
            if (redactor.ShowDialog() == true)
            {
                //var repository = new InformationSupportRepository(_connectionString);
                informationSupportRepository.Create(redactor.curINF);
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((string)listBox1.SelectedItem == types[0])
            {
                dataGrid1.ItemsSource = gpuRepository.GetAll();
            }
            else if ((string)listBox1.SelectedItem == types[1])
            {
                dataGrid1.ItemsSource = cpuRepository.GetAll();
            }
            else if ((string)listBox1.SelectedItem == types[2])
            {
                dataGrid1.ItemsSource = informationSupportRepository.GetAll();
            }
        }
    }
}
