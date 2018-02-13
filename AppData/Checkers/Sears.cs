using System;
using System.IO;
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

            var fs = new FileStream(store.LogFileName("Sears"), FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|false");
            sw.Close();
            fs.Close();


            _browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            _browser.Manage().Window.Maximize();
            _browser.Navigate().GoToUrl("http://www.sears.com/");
            _browser.Navigate().GoToUrl("https://www.sears.com/universalprofile/userLogonForm?upid=3&formName=LOGIN&URL=http%3A%2F%2Fwww.sears.com%2F");
            Thread.Sleep(1000);
            _browser.FindElement(By.XPath("//*[@id=\"email\"]")).Click();
            _browser.FindElement(By.XPath("//*[@id=\"email\"]")).Clear();
            _browser.FindElement(By.XPath("//*[@id=\"email\"]")).SendKeys(user.Login);
            _browser.FindElement(By.XPath("//*[@id=\"password\"]")).Click();
            _browser.FindElement(By.XPath("//*[@id=\"password\"]")).Clear();
            _browser.FindElement(By.XPath("//*[@id=\"password\"]")).SendKeys(user.Password);
            _browser.FindElement(By.CssSelector("button.shcBtn.shcBtnCTA.signIn")).Click();
            Thread.Sleep(1000);
            ((ITakesScreenshot)_browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Sears", user.Login), System.Drawing.Imaging.ImageFormat.Jpeg);
            _browser.Manage().Cookies.DeleteAllCookies();
            _browser.Quit();
        }
    }
}