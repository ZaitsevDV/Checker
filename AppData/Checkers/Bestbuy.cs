using AppData.Model;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace AppData.Checkers
{
    public class Bestbuy
    {
        public void Check(string login, string password, IWebDriver browser)
        {
            var store = new Store();
            var user = new User(login, password);
            var fs = new FileStream(store.LogFileName("Bestbuy"), FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            try
            {
                browser.Navigate().GoToUrl("https://www.bestbuy.com/identity/global/signin");
                Thread.Sleep(1000);
                //browser.FindElement(By.XPath("//header[@id='header']/div/div[2]/div/a/span")).Click();
                //browser.FindElement(By.Id("profileMenuWrap1")).Click();
                //browser.FindElement(By.XPath("(//a[contains(text(),'Sign In')])[2]")).Click();
                browser.FindElement(By.Id("fld-e")).Click();
                browser.FindElement(By.Id("fld-e")).Clear();
                browser.FindElement(By.Id("fld-e")).SendKeys(user.Login);
                browser.FindElement(By.Id("fld-p1")).Clear();
                browser.FindElement(By.Id("fld-p1")).SendKeys(user.Password);
                browser.FindElement(By.XPath("//button[@type='submit']")).Click();
                Thread.Sleep(10000);
                if (browser.Url == "https://www.bestbuy.com/myaccount")
                {
                    sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|true");
                    browser.Navigate().GoToUrl("https://www.bestbuy.com/myaccount/#/orders");
                    ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(
                        store.ScreenShotFileName("Bestbuy", user.Login), System.Drawing.Imaging.ImageFormat.Jpeg);
                    browser.Navigate().GoToUrl("https://www.bestbuy.com/logout");
                }
                else
                {
                    sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password +
                                 "|false");
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
