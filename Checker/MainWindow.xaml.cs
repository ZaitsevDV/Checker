using AppData.Checkers;
using AppData.Model;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Store.Directory = FolderTextBox.Text;
            var users = new List<User>();
            Store.Users = users;
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            if (WallmartCheckBox.IsChecked == false &&
                SearsCheckBox.IsChecked == false &&
                TigerdirectCheckBox.IsChecked == false &&
                OverstockCheckBox.IsChecked == false &&
                NeweggCheckBox.IsChecked == false &&
                ToysrusCheckBox.IsChecked == false &&
                BestbuyCheckBox.IsChecked == false)
            {
                MessageBox.Show("Choose what sites you whant to check", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (Store.Users.Count > 0)
            {
                ProgressBar.Value = 0;
                ProgressBar.Maximum = Store.Users.Count;
                IWebDriver browser = new OpenQA.Selenium.Chrome.ChromeDriver();
                browser.Manage().Cookies.DeleteAllCookies();
                browser.Manage().Window.Maximize();
                foreach (var user in Store.Users)
                {
                    if (user.Login != "" && user.Password != "")
                    {
                        CheckSelected(user, browser);
                    }
                    ProgressBar.Value += 1;
                }
                browser.Quit();
                Store.ResultMessage(ProgressBar.Value.ToString());
            }
            else
            {
                MessageBox.Show("You must download users", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            Store.Directory = folderBrowserDlg.SelectedPath;
        }

        private void GetSource_Click(object sender, RoutedEventArgs e)
        {
            var sourceBrowserDlg = new OpenFileDialog
            {
                DefaultExt = "txt",
                AddExtension = true,
                Multiselect = false,
                InitialDirectory = Store.Directory,
            };
            var sourceResult = sourceBrowserDlg.ShowDialog();
            if (!sourceResult.Equals(System.Windows.Forms.DialogResult.OK)) return;
            if (!sourceBrowserDlg.SafeFileName.EndsWith("txt"))
            {
                MessageBox.Show("Only \"txt\" file can be checked", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                GetSource_Click(sender, e);
            }
            SourceTextBox.Text = sourceBrowserDlg.SafeFileName;
            Store.SourceFile = sourceBrowserDlg.FileName;
            GetUsers.IsEnabled = true;
        }

        private void GetUsers_Click(object sender, RoutedEventArgs e)
        {
            var lines = File.ReadLines(Store.SourceFile).ToList();
            ProgressBar.Maximum = lines.Count;
            var users = new List<User>();
            try
            {
                foreach (var item in lines)
                {
                    var yourStringArray = item.Split('\t');
                    var login = yourStringArray[0];
                    var password = yourStringArray[1];
                    var user = new User(login, password);
                    users.Add(user);
                    ProgressBar.Value = users.Count;
                }
                Store.Users = users;
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Check source file", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                GetSource_Click(sender, e);
            }
        }

        private void CheckSelected(User user, IWebDriver browser)
        {

            if (WallmartCheckBox.IsChecked == true)
            {
                var wallmart = new Wallmart();
                wallmart.Check(user.Login, user.Password, browser);
            }

            if (SearsCheckBox.IsChecked == true)
            {
                var sears = new Sears();
                sears.Check(user.Login, user.Password, browser);
            }

            if (TigerdirectCheckBox.IsChecked == true)
            {
                var tigerdirect = new Tigerdirect();
                tigerdirect.Check(user.Login, user.Password, browser);
            }

            if (OverstockCheckBox.IsChecked == true)
            {
                var overstock = new Overstock();
                overstock.Check(user.Login, user.Password, browser);
            }

            if (BestbuyCheckBox.IsChecked == true)
            {
                var bestbuy = new Bestbuy();
                bestbuy.Check(user.Login, user.Password, browser);
            }

            if (NeweggCheckBox.IsChecked == true)
            {
                var newegg = new Newegg();
                newegg.Check(user.Login, user.Password, browser);
            }

            if (ToysrusCheckBox.IsChecked == true)
            {
                var toysrus = new Toysrus();
                toysrus.Check(user.Login, user.Password, browser);
            }
        }
    }
}