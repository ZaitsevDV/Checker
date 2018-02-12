using AppData.Model;
using OpenQA.Selenium;
using System.Threading;

namespace AppData.Checkers
{
    public class Tigerdirect
    {
        private IWebDriver _browser;

        public void Check(string login, string password)
        {
            var store = new Store();
            var user = new User(login, password);
            _browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            _browser.Manage().Window.Maximize();
            _browser.Navigate().GoToUrl("http://www.tigerdirect.com/");
            Thread.Sleep(1000);
            _browser.FindElement(By.LinkText("My Account")).Click();
            _browser.FindElement(By.XPath("(//input[@name='email'])[3]")).Click();
            _browser.FindElement(By.XPath("(//input[@name='email'])[3]")).Clear();
            _browser.FindElement(By.XPath("(//input[@name='email'])[3]")).SendKeys(user.Login);
            _browser.FindElement(By.XPath("(//input[@name='password'])[2]")).Clear();
            _browser.FindElement(By.XPath("(//input[@name='password'])[2]")).SendKeys(user.Password);
            _browser.FindElement(By.XPath("(//input[@name='imageField'])[2]")).Click();
            Thread.Sleep(1000);
            ((ITakesScreenshot)_browser).GetScreenshot().SaveAsFile(store.GetFileName("Tigerdirect", user.Login), System.Drawing.Imaging.ImageFormat.Jpeg);
            _browser.Quit();
        }
    }
}
