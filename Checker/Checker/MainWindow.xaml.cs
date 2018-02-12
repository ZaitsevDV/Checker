using AppData.Checkers;
using AppData.Model;
using System;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;


namespace Checker
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            FolderTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Store.ScreenshotDirectory = FolderTextBox.Text;
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            var user = new User();

            if (LoginTextBox.Text != "" && PaswordTextBox.Text != "")
            {
                user.Login = LoginTextBox.Text;
                user.Password = PaswordTextBox.Text;
            }
            else
            {
                MessageBox.Show("Enter the login and the password", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (WallmartCheckBox.IsChecked == true)
            {
                var wallmart = new Wallmart();
                wallmart.Check(user.Login, user.Password);
            }
            if (SearsCheckBox.IsChecked == true)
            {
                var sears = new Sears();
                sears.Check(user.Login, user.Password);
            }
            if (WallmartCheckBox.IsChecked == false && SearsCheckBox.IsChecked == false)
            {
                MessageBox.Show("Choose what sites you whant to check", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }


        private void GetFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };
            var dlgResult = folderBrowserDlg.ShowDialog();
            if (!dlgResult.Equals(System.Windows.Forms.DialogResult.OK)) return;
            FolderTextBox.Text = folderBrowserDlg.SelectedPath;
            Store.ScreenshotDirectory = folderBrowserDlg.SelectedPath;
        }
    }
}