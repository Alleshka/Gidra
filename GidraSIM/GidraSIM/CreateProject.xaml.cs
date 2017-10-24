using System.Windows;
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
            {
                this.DialogResult = true;
                this.Close();
            }
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
