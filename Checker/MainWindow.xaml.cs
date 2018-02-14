using AppData.Checkers;
using AppData.Model;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using OpenQA.Selenium.Support.UI;
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
            }
            else if (LoginTextBox.Text != "" && PaswordTextBox.Text != "")
            {
                IWebDriver browser = new OpenQA.Selenium.Chrome.ChromeDriver();
                browser.Manage().Window.Maximize();
                var user = new User
                {
                    Login = LoginTextBox.Text,
                    Password = PaswordTextBox.Text
                };
                CheckSelected(user, browser);
                browser.Quit();
            }
            else
            {
                MessageBox.Show("Enter the login and the password, or select source file", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            SourceTextBox.Text = sourceBrowserDlg.SafeFileName;
            Store.SourceFile = sourceBrowserDlg.FileName;
            GetUsers.IsEnabled = true;
        }

        private void GetUsers_Click(object sender, RoutedEventArgs e)
        {
            var lines = File.ReadLines(Store.SourceFile).ToList();
            ProgressBar.Maximum = lines.Count;
            var users = new List<User>();
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
                sears.Check(user.Login, user.Password);
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
                //browser.Navigate().GoToUrl("https://www.bestbuy.com/");
                //browser.FindElement(By.Name("select_locale")).Click();
                //new SelectElement(browser.FindElement(By.Name("select_locale"))).SelectByText("United States - English");
                //browser.FindElement(By.XPath("//option[@value='1']")).Click();
                //browser.FindElement(By.XPath("//input[@type='checkbox']")).Click();
                //browser.FindElement(By.XPath("//img[@alt='Go']")).Click();
                //browser.FindElement(By.XPath("(//button[@type='button'])[3]")).Click();
                bestbuy.Check(user.Login, user.Password, browser);
            }
        }

    }

    //internal class SelectElement
    //{
    //    private IWebElement webElement;

    //    public SelectElement(IWebElement webElement)
    //    {
    //        this.webElement = webElement;
    //    }
    //}
}