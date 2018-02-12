using AppData.Model;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;

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
            var store = new Store();
            var user = new User(login, password);
            _browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            _browser.Manage().Window.Maximize();
            _browser.Navigate().GoToUrl("https://www.walmart.com/account/login");

            var emailInput = GetElement(By.Name("email"));
            var passwordInput = GetElement(By.Name("password"));
            var rememberMe = GetElement(By.XPath("//div[4]/div/div/label"));
            var submitButton = GetElement(By.XPath("//button[@type='submit']"));

            emailInput.Click();
            emailInput.Clear();
            emailInput.SendKeys(user.Login);
            passwordInput.Clear();
            passwordInput.SendKeys(user.Password);
            rememberMe.Click();
            submitButton.Click();
            Thread.Sleep(1000);

            if (_browser.Url == "https://www.walmart.com/account/login")
            {
                GetElement(By.CssSelector("body > div > div > div > div.login-wrapper-container > div > section > form > " +
                                          "div.form-field-password > div > div.js-password > div > button")).Click();
                ((ITakesScreenshot)_browser).GetScreenshot().SaveAsFile(store.GetFileName("access_denied"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                do
                {
                    Thread.Sleep(1000);
                }
                while (GetElement(By.XPath("(//a[contains(text(),'Your Account')])[2]")) == null);
                _browser.FindElement(By.LinkText("Purchase History")).Click();
                _browser.FindElement(By.CssSelector("div.flyout.flyout-bottom.flyout-align-null.flyout-animate.flyout-fluid.flyout-button.xs-margin-ends > button.dropdown.btn.dropdown")).Click();
                _browser.FindElement(By.XPath("(//button[@type='button'])[42]")).Click();
                Thread.Sleep(500);
                ((ITakesScreenshot)_browser).GetScreenshot().SaveAsFile(store.GetFileName("Walmart_history"), System.Drawing.Imaging.ImageFormat.Jpeg);
                _browser.FindElement(By.LinkText("Payment Methods")).Click();
                Thread.Sleep(500);
                ((ITakesScreenshot)_browser).GetScreenshot().SaveAsFile(store.GetFileName("Walmart_Payment"), System.Drawing.Imaging.ImageFormat.Jpeg);
                _browser.FindElement(By.LinkText("Your Account")).Click();
                _browser.FindElement(By.LinkText("Shipping Addresses")).Click();
                Thread.Sleep(500);
                ((ITakesScreenshot)_browser).GetScreenshot().SaveAsFile(store.GetFileName("Walmart_Address"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            _browser.Quit();
        }
    }
}
