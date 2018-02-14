using AppData.Model;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;

namespace AppData.Checkers
{
    public class Newegg
    {
        public void Check(string login, string password, IWebDriver browser)
        {
            var store = new Store();
            var user = new User(login, password);
            var fs = new FileStream(store.LogFileName("Newegg"), FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);

            browser.Navigate().GoToUrl("https://www.newegg.com/");
            Thread.Sleep(1000);
            browser.FindElement(By.LinkText("Log in or Register")).Click();
            browser.FindElement(By.Id("UserName")).Click();
            browser.FindElement(By.Id("UserName")).Clear();
            browser.FindElement(By.Id("UserName")).SendKeys(user.Login);
            browser.FindElement(By.Id("UserPwd")).Clear();
            browser.FindElement(By.Id("UserPwd")).SendKeys(user.Password);
            browser.FindElement(By.Id("submit")).Click();
            Thread.Sleep(1000);
            if (browser.Url != "https://secure.newegg.com/NewMyAccount/AccountLogin.aspx")
            {
                sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|true");
                browser.Navigate().GoToUrl("https://www.newegg.com/myaccount/#/orders");
                ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Newegg", user.Login), System.Drawing.Imaging.ImageFormat.Jpeg);
                browser.Navigate().GoToUrl("https://www.newegg.com/logout");
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
