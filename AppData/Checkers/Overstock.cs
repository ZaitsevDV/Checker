using AppData.Model;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;

namespace AppData.Checkers
{
    public class Overstock
    {
        public void Check(string login, string password, IWebDriver browser)
        {
            var store = new Store();
            var user = new User(login, password);
            var fs = new FileStream(store.LogFileName("Overstock"), FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);

            browser.Navigate().GoToUrl("https://www.overstock.com/");
            browser.Navigate().GoToUrl("https://www.overstock.com/myaccount");
            Thread.Sleep(1000);
            browser.FindElement(By.Id("loginEmailInput")).Clear();
            browser.FindElement(By.Id("loginEmailInput")).SendKeys(user.Login);
            browser.FindElement(By.Id("loginPasswordInput")).Clear();
            browser.FindElement(By.Id("loginPasswordInput")).SendKeys(user.Password);
            browser.FindElement(By.XPath("(//button[@name='submit'])[2]")).Click();
            Thread.Sleep(1000);
            if (browser.Url != "https://www.overstock.com/myaccount")
            {
                sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|true");
                browser.Navigate().GoToUrl("https://www.overstock.com/myaccount/#/orders");
                ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Overstock", user.Login), System.Drawing.Imaging.ImageFormat.Jpeg);
                browser.Navigate().GoToUrl("https://www.overstock.com/logout");
            }
            else
            {
                sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|false");
            }
            sw.Close();
            fs.Close();
        }
    }
}

