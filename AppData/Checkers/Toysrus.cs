using AppData.Model;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace AppData.Checkers
{
    public class Toysrus
    {
        public void Check(string login, string password, IWebDriver browser)
        {
            var store = new Store();
            var user = new User(login, password);
            var fs = new FileStream(store.LogFileName("Toysrus"), FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            try
            {
                browser.Navigate().GoToUrl("https://www.toysrus.com/");
                Thread.Sleep(1000);
                browser.FindElement(By.XPath("//div[@id='root']/div/div[2]/div/header/div[2]/a/span")).Click();
                browser.FindElement(By.LinkText("my account")).Click();
                browser.FindElement(By.XPath("//a[contains(text(),'sign in')]")).Click();
                browser.FindElement(By.Id("email")).Click();
                browser.FindElement(By.Id("email")).Clear();
                browser.FindElement(By.Id("email")).SendKeys(user.Login);
                browser.FindElement(By.Id("password")).Click();
                browser.FindElement(By.Id("password")).Clear();
                browser.FindElement(By.Id("password")).SendKeys(user.Password);
                browser.FindElement(By.Id("loginSubmitBtn")).Click();
                Thread.Sleep(1000);
                if (browser.Url != "https://www.toysrus.com/myaccount/myAccount.jsp")
                {
                    sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|true");
                    browser.Navigate().GoToUrl("https://www.toysrus.com/myaccount/#/orders");
                    ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Toysrus", user.Login), System.Drawing.Imaging.ImageFormat.Jpeg);
                    browser.Navigate().GoToUrl("https://www.toysrus.com/logout");
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
