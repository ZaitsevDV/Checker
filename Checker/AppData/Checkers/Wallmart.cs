using AppData.Model;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace AppData.Checkers
{
    public class Wallmart
    {
        private IWebDriver _browser;

        private IWebElement GetElement(By locator)
        {
            var elements = _browser.FindElements(locator).ToList();
            return elements.Count > 0 ? elements[0] : null;
        }

        public void Check(string login, string password)
        {
            User user = new User("zdv3223666@gmail.com", "mogilev_01");

            if (login != null && password != null)
            {
                user.Login = login;
                user.Password = password;
            }

            _browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            _browser.Manage().Window.Maximize();
            _browser.Navigate().GoToUrl("https://www.walmart.com/account/login?tid=0&returnUrl=%2F");

            var emailInput = GetElement(By.Name("email"));
            var passwordInput = GetElement(By.Name("password"));
            var rememberMe = GetElement(By.XPath("//div[4]/div/div/label"));
            var submitButton = GetElement(By.XPath("//button[@type='submit']"));
            var fileName = "Screenshot " + DateTime.Now.ToString("yyyyMMdd_HHmmss tt") + ".jpeg";

            emailInput.Click();
            emailInput.Clear();
            emailInput.SendKeys(user.Login);
            passwordInput.Clear();
            passwordInput.SendKeys(user.Password);
            rememberMe.Click();
            submitButton.Click();

            _browser.Navigate().GoToUrl("https://www.walmart.com/account");
            _browser.Navigate().GoToUrl("https://www.walmart.com/account/orders");

            if (GetElement(By.XPath("(//button[@type='button'])[35]")) != null)
            {
                GetElement(By.XPath("(//button[@type='button'])[35]")).Click();
                GetElement(By.LinkText("2014")).Click();
                var ss = ((ITakesScreenshot) _browser).GetScreenshot();
                ss.SaveAsFile(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            _browser.Quit();
        }
    }
}
