using AppData.Checkers;
using AppData.Model;
using System;
using System.Windows;
using MessageBox = System.Windows.MessageBox;


namespace Checker
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            else
            {
                MessageBox.Show("Choose what sites you whant to check", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}