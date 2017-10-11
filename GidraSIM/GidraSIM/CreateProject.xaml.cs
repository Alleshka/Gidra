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
using System.Windows.Forms;

namespace GidraSIM
{
    public partial class CreateProject : Window
    {
        public string NamePr;
        public string WayFile;
        WorksystemWithFiles FilesWs;

        public CreateProject(ref WorksystemWithFiles FilesWorksystem)
        {
            FilesWs = FilesWorksystem;
            InitializeComponent();
        }

        private void button_Create_Click(object sender, RoutedEventArgs e)
        {
            NamePr = textBox_NameProject.Text;
            // WayFile = textBox_WayProject.Text;

            if (FilesWs.CheckWay(WayFile, NamePr))
                this.Close();
            else
                System.Windows.MessageBox.Show("Каталог с именем " + NamePr + " уже существует.");
        }

        private void button_openFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog myDialog = new FolderBrowserDialog();
            myDialog.ShowDialog();
            WayFile = myDialog.SelectedPath;
            label_WayProject.Content = WayFile;


        }

    }
}
