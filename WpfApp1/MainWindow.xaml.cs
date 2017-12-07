using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonInstall_Click(object sender, RoutedEventArgs e)
        {
            var path = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var exePath = new FileInfo(System.IO.Path.Combine(path.DirectoryName, "TestForTopShelfAndInstaller.exe"));
            TextBox1.Text = exePath.FullName;

            if (exePath.Exists)
            {
                System.Diagnostics.Process.Start(exePath.FullName, "install");
            }
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = "cmd";
            psi.Arguments = "/c net start TopShelfSample_Service";
            psi.Verb = "runas";
            var p = System.Diagnostics.Process.Start(psi);
        }
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = "cmd";
            psi.Arguments = "/c net stop TopShelfSample_Service";
            psi.Verb = "runas";
            var p = System.Diagnostics.Process.Start(psi);
        }
        private void ButtonUninstall_Click(object sender, RoutedEventArgs e)
        {
            var path = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var exePath = new FileInfo(System.IO.Path.Combine(path.DirectoryName, "TestForTopShelfAndInstaller.exe"));
            TextBox1.Text = exePath.FullName;

            if (exePath.Exists)
            {
                System.Diagnostics.Process.Start(exePath.FullName, "uninstall");
            }

        }
    }
}
