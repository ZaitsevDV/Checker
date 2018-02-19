using System;
using System.IO;
using AppData.Model;
using OpenQA.Selenium;
using System.Threading;
using System.Windows;

namespace AppData.Checkers
{
    public class Sears
    {
        public void Check(string login, string password, IWebDriver browser)
        {
            var store = new Store();
            var user = new User(login, password);

            var fs = new FileStream(store.LogFileName("Sears"), FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|false");
            sw.Close();
            fs.Close();

            try
            {
                browser = new OpenQA.Selenium.Chrome.ChromeDriver();
                browser.Manage().Window.Maximize();
                browser.Navigate().GoToUrl("http://www.sears.com/");
                browser.Navigate()
                    .GoToUrl(
                        "https://www.sears.com/universalprofile/userLogonForm?upid=3&formName=LOGIN&URL=http%3A%2F%2Fwww.sears.com%2F");
                Thread.Sleep(1000);
                browser.FindElement(By.XPath("//*[@id=\"email\"]")).Click();
                browser.FindElement(By.XPath("//*[@id=\"email\"]")).Clear();
                browser.FindElement(By.XPath("//*[@id=\"email\"]")).SendKeys(user.Login);
                browser.FindElement(By.XPath("//*[@id=\"password\"]")).Click();
                browser.FindElement(By.XPath("//*[@id=\"password\"]")).Clear();
                browser.FindElement(By.XPath("//*[@id=\"password\"]")).SendKeys(user.Password);
                browser.FindElement(By.CssSelector("button.shcBtn.shcBtnCTA.signIn")).Click();
                Thread.Sleep(1000);
                ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Sears", user.Login),
                    System.Drawing.Imaging.ImageFormat.Jpeg);
                browser.Manage().Cookies.DeleteAllCookies();
                browser.Quit();
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
