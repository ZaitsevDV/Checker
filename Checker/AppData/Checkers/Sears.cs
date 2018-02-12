using AppData.Model;
using OpenQA.Selenium;
using System.Threading;

namespace AppData.Checkers
{
    public class Sears
    {
        private IWebDriver _browser;

        public void Check(string login, string password)
        {
            var store = new Store();
            var user = new User(login, password);
            _browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            _browser.Manage().Window.Maximize();
            _browser.Navigate().GoToUrl("http://www.sears.com/");
            _browser.Navigate().GoToUrl("https://www.sears.com/universalprofile/userLogonForm?upid=3&formName=LOGIN&URL=http%3A%2F%2Fwww.sears.com%2F");
            _browser.FindElement(By.XPath("//*[@id=\"email\"]")).Click();
            _browser.FindElement(By.XPath("//*[@id=\"email\"]")).Clear();
            _browser.FindElement(By.XPath("//*[@id=\"email\"]")).SendKeys(user.Login);
            _browser.FindElement(By.XPath("//*[@id=\"password\"]")).Click();
            _browser.FindElement(By.XPath("//*[@id=\"password\"]")).Clear();
            _browser.FindElement(By.XPath("//*[@id=\"password\"]")).SendKeys(user.Password);
            _browser.FindElement(By.CssSelector("button.shcBtn.shcBtnCTA.signIn")).Click();

            //_browser.Navigate().GoToUrl("https://www.sears.com/universalprofile/userLogonForm");
            //_browser.SwitchTo().Frame("index=3");
            //_browser.FindElement(By.Id("email")).Click();
            //_browser.FindElement(By.Id("email")).Clear();
            //_browser.FindElement(By.Id("email")).SendKeys(user.Login);
            //_browser.FindElement(By.Id("password")).Click();
            //_browser.FindElement(By.Id("password")).Clear();
            //_browser.FindElement(By.Id("password")).SendKeys(user.Password);
            //_browser.FindElement(By.CssSelector("button.shcBtn.shcBtnCTA.signIn")).Click();

            Thread.Sleep(1000);
            ((ITakesScreenshot)_browser).GetScreenshot().SaveAsFile(store.GetFileName("sears"), System.Drawing.Imaging.ImageFormat.Jpeg);
            _browser.Quit();
        }
    }
}