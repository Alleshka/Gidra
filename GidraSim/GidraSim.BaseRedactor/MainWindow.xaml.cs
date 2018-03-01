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
using GidraSIM.GUI.AdmSet;

namespace GidraSim.BaseRedactor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private String _connectionString = "Data Source=DESKTOP-H4JQP0V;Initial Catalog=SimSapr;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private String _connectionString;

        CpuRepository cpuRepository;
        GpuRepository gpuRepository;
        InformationSupportRepository informationSupportRepository;
        MonitorRepository monitorRepository;
        SoftwareRepository softwareRepository;
        StorageDeviceRepository storageRepository;

        Settings settings;

        private string[] types = new string[]
        {
            "GPU",
            "CPU",
            "Information support",
            "Monitor",
            "Soft",
            "Storage"
        };


        public MainWindow()
        {
            InitializeComponent();
            listBox1.ItemsSource = types;

            //попытка чтения без рекурсии
            if (SettingsReader.TryRead(out settings))
            {
            }
            else
            {
                SettingsView settings = new SettingsView();
                settings.ShowDialog();
                if (settings.DialogResult != true)
                {
                    MessageBox.Show("Нет строки подключения, завершение программы");
                    System.Windows.Application.Current.Shutdown();
                }
            }

            //_connectionString = System.Configuration.ConfigurationManager.
            //    ConnectionStrings["connectionString"].ConnectionString;
            _connectionString = SettingsReader.ResourcesConnectionString;

            LoadRep();
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
            try
            {
                String type = listBox1.SelectedItem.ToString();

                if (type == types[0])
                {
                    dataGrid1.ItemsSource = gpuRepository.GetAll().ToList();
                }
                else if (type == types[1])
                {
                    dataGrid1.ItemsSource = cpuRepository.GetAll().ToList();
                }
                else if (type == types[2])
                {
                    dataGrid1.ItemsSource = informationSupportRepository.GetAll().ToList();
                }
                else if (type == types[3])
                {
                    dataGrid1.ItemsSource = monitorRepository.GetAll().ToList();
                }
                else if (type == types[4])
                {
                    dataGrid1.ItemsSource = softwareRepository.GetAll().ToList();
                }
                else if (type == types[5])
                {
                    dataGrid1.ItemsSource = storageRepository.GetAll().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MonitorItem_Click(object sender, RoutedEventArgs e)
        {
            var redactor = new MonitorRedactor();
            if (redactor.ShowDialog()==true)
            {
                monitorRepository.Create(redactor.curMonitor);
            }
        }

        private void SoftItemAdd_Click(object sender, RoutedEventArgs e)
        {
            var red = new SoftRedactor();
            if (red.ShowDialog() == true)
            {
                softwareRepository.Create(red.curSoft);
            }
        }

        private void StorageItemAdd_Click(object sender, RoutedEventArgs e)
        {
            var red = new StorageRedactor();
            if (red.ShowDialog() == true)
            {
                storageRepository.Create(red._curStorage);
            }
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsView settings = new SettingsView();
            settings.ShowDialog();
            //перезагрузка рпеозиториев
            if(settings.DialogResult == true)
            {
                _connectionString = SettingsReader.ResourcesConnectionString;
                LoadRep();
            }
        }

        private void dataGrid1_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if( e.GetType() == typeof(GPU))
            {
                gpuRepository.Create(e.NewItem as GPU);
            }
        }

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        }

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.dataGrid1.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= dataGrid1_RowEditEnding;
                (sender as DataGrid).CommitEdit();
                if(dataGrid1.SelectedItem.GetType() == typeof(GPU))
                    gpuRepository.Update((this.dataGrid1.SelectedItem as GPU));
                if (dataGrid1.SelectedItem.GetType() == typeof(CPU))
                    cpuRepository.Update((this.dataGrid1.SelectedItem as CPU));
                if (dataGrid1.SelectedItem.GetType() == typeof(InformationSupport))
                    informationSupportRepository.Update((this.dataGrid1.SelectedItem as InformationSupport));
                if (dataGrid1.SelectedItem.GetType() == typeof(Monitor))
                    monitorRepository.Update((this.dataGrid1.SelectedItem as Monitor));
                if (dataGrid1.SelectedItem.GetType() == typeof(Software))
                    softwareRepository.Update((this.dataGrid1.SelectedItem as Software));
                if (dataGrid1.SelectedItem.GetType() == typeof(StorageDevice))
                    storageRepository.Update((this.dataGrid1.SelectedItem as StorageDevice));
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).RowEditEnding += dataGrid1_RowEditEnding;
            }
        }



        private void LoadRep()
        {
            cpuRepository = new CpuRepository(_connectionString);
            gpuRepository = new GpuRepository(_connectionString);
            informationSupportRepository = new InformationSupportRepository(_connectionString);
            monitorRepository = new MonitorRepository(_connectionString);
            softwareRepository = new SoftwareRepository(_connectionString);
            storageRepository = new StorageDeviceRepository(_connectionString);
        }
    }
}
