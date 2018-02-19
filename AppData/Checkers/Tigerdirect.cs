using System;
using System.IO;
using AppData.Model;
using OpenQA.Selenium;
using System.Threading;
using System.Windows;

namespace AppData.Checkers
{
    public class Tigerdirect
    {
        public void Check(string login, string password, IWebDriver browser)
        {
            var store = new Store();
            var user = new User(login, password);
            var fs = new FileStream(store.LogFileName("Tigerdirect"), FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            try
            {
                browser.Navigate().GoToUrl("http://www.tigerdirect.com/");
                Thread.Sleep(1000);
                browser.FindElement(By.LinkText("My Account")).Click();
                browser.FindElement(By.XPath("(//input[@name='email'])[3]")).Click();
                browser.FindElement(By.XPath("(//input[@name='email'])[3]")).Clear();
                browser.FindElement(By.XPath("(//input[@name='email'])[3]")).SendKeys(user.Login);
                browser.FindElement(By.XPath("(//input[@name='password'])[2]")).Clear();
                browser.FindElement(By.XPath("(//input[@name='password'])[2]")).SendKeys(user.Password);
                browser.FindElement(By.XPath("(//input[@name='imageField'])[2]")).Click();
                Thread.Sleep(1000);
                if (browser.Url == "https://www.tigerdirect.com/cgisec/modifyaccount.asp")
                {
                    sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|true");
                    browser.Navigate().GoToUrl("https://www.tigerdirect.com/cgisec/orderHistory.asp");
                    ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Tigerdirect", user.Login), System.Drawing.Imaging.ImageFormat.Jpeg);
                    browser.Navigate().GoToUrl("https://www.tigerdirect.com/cgisec/logoff.asp");
                }
                else
                {
                    sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|false");
                }
                sw.Close();
                fs.Close();
            }
            catch (InvalidOperationException)
            {
                Store.ResultMessage("many");
                browser.Quit();
                Environment.Exit(0);
            }
            catch (NoSuchElementException e)
            {
                var result = MessageBox.Show("Something wrong: " + "\r\n" + e.Message,
                    "Error",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK,
                    MessageBoxOptions.ServiceNotification);

                switch (result)
                {
                    case MessageBoxResult.OK:
                        sw.Close();
                        fs.Close();
                        Check(login, password, browser);
                        break;
                    case MessageBoxResult.Cancel:
                        sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login +
                                     "|" + password + "|exception|" + e.Message);
                        break;
                }
            }
            catch (WebDriverException)
            {
                Store.ResultMessage("many");
                browser.Quit();
                Environment.Exit(0);
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }
    }
}
