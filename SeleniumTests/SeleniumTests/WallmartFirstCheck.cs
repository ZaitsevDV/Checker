using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class WallmartFirstCheck
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "https://www.katalon.com/";
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheWallmartFirstCheckTest()
        {
            driver.Navigate().GoToUrl("https://www.walmart.com/");
            driver.Navigate().GoToUrl("https://www.walmart.com/account/login?tid=0&returnUrl=%2F");
            driver.FindElement(By.Name("email")).Click();
            driver.FindElement(By.Name("email")).Clear();
            driver.FindElement(By.Name("email")).SendKeys("zdv3223666@gmail.com");
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys("2danger2");
            driver.FindElement(By.XPath("//div[4]/div/div/label")).Click();
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            driver.Navigate().GoToUrl("https://www.walmart.com/account");
            driver.Navigate().GoToUrl("https://www.walmart.com/account/orders");
            driver.FindElement(By.XPath("(//button[@type='button'])[35]")).Click();
            driver.FindElement(By.LinkText("2014")).Click();
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
