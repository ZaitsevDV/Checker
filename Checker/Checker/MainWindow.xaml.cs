using System;
using System.Linq;
using System.Windows;
using OpenQA.Selenium;


namespace Checker
{
    public partial class MainWindow
    {
        private IWebDriver _browser;

        public MainWindow()
        {
            InitializeComponent();
        }


        private IWebElement GetElement(By locator)
        {
            var elements = _browser.FindElements(locator).ToList();
            return elements.Count > 0 ? elements[0] : null;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var login = "";
            var password = "";
            if (LoginTextBox != null && PaswordTextBox != null)
            {
                login    = LoginTextBox.Text;
                password = PaswordTextBox.Text;
            }
            else
            {
                login = "zdv3223666@gmail.com";
                password = "mogilev_01";
            }

            _browser = new OpenQA.Selenium.Chrome.ChromeDriver(/*co*/);
            _browser.Manage().Window.Maximize();
            _browser.Navigate().GoToUrl("https://www.walmart.com/account/login?tid=0&returnUrl=%2F");

            var emailInput = GetElement(By.Name("email"));
            var passwordInput = GetElement(By.Name("password"));
            var rememberMe = GetElement(By.XPath("//div[4]/div/div/label"));
            var submitButton = GetElement(By.XPath("//button[@type='submit']"));
            var fileName = "Screenshot " + DateTime.Now.ToString("yyyyMMdd_HHmmss tt") + ".jpeg";

            emailInput.Click();
            emailInput.Clear();
            emailInput.SendKeys(login);
            passwordInput.Clear();
            passwordInput.SendKeys(password);
            rememberMe.Click();
            submitButton.Click();

            _browser.Navigate().GoToUrl("https://www.walmart.com/account");
            _browser.Navigate().GoToUrl("https://www.walmart.com/account/orders");

            if (GetElement(By.XPath("(//button[@type='button'])[35]")) != null)
            {
                GetElement(By.XPath("(//button[@type='button'])[35]")).Click();
                GetElement(By.LinkText("2014")).Click();
                var ss = ((ITakesScreenshot) _browser).GetScreenshot();
                ss.SaveAsFile(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            _browser.Quit();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}