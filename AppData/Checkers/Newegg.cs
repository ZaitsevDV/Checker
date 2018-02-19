using AppData.Model;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;
using System.Windows;

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
            try
            {
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
                    browser.Navigate().GoToUrl("https://secure.newegg.com/NewMyAccount/DashBoard.aspx");
                    ((ITakesScreenshot) browser).GetScreenshot().SaveAsFile(
                        store.ScreenShotFileName("Newegg Dash Board", user.Login),
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                    browser.Navigate().GoToUrl("https://secure.newegg.com/NewMyAccount/OrderHistory.aspx");
                    ((ITakesScreenshot) browser).GetScreenshot().SaveAsFile(
                        store.ScreenShotFileName("Newegg Order History", user.Login),
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                    browser.Navigate().GoToUrl("https://secure.newegg.com/NewMyAccount/AccountLogout.aspx");
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
