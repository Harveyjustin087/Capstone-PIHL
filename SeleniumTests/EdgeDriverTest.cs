using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using System;

namespace SeleniumTests
{
    [TestClass]
    public class EdgeDriverTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from http://go.microsoft.com/fwlink/?LinkId=619687
        // to install Microsoft WebDriver.

        private EdgeDriver _driver;
        WebDriverWait wait;

        [TestInitialize]
        public void EdgeDriverInitialize()
        {
            // Initialize edge driver 
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            _driver = new EdgeDriver(options);
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [TestMethod]
        public void TestAuthorizePagesRedirectToLogin()
        {
            //Setup
            _driver.Url = "http://localhost:5000/Scorekeeper";
            //Act
            string className = _driver.FindElement(By.XPath("/html/body/div[1]/div/main/div/div/div")).GetAttribute("class");
            //Assert
            Assert.AreEqual("card login-logout-tab", className);
        }

        [TestMethod]
        public void TestLogin()
        {
            _driver.Url = "http://localhost:5000/";
            _driver.FindElement(By.LinkText("Scorekeeper Login")).Click();
            _driver.FindElement(By.Id("email")).SendKeys("christopher.thoms@colliers.com");
            _driver.FindElement(By.Id("password")).SendKeys("Batman316");
            _driver.FindElement(By.XPath("//*[@id='account']/div[5]/button")).Click();
            wait.Until(webDriver => webDriver.FindElement(By.LinkText("Hello christopher.thoms@colliers.com! |")).Displayed);
        }

        [TestMethod]
        public void TestScorekeeperCantSeeAdminOptions()
        {
            _driver.Url = "http://localhost:5000/";
            _driver.FindElement(By.LinkText("Scorekeeper Login")).Click();
            _driver.FindElement(By.Id("email")).SendKeys("test@email.com");
            _driver.FindElement(By.Id("password")).SendKeys("Batman316");
            _driver.FindElement(By.XPath("//*[@id='account']/div[5]/button")).Click();
            wait.Until(webDriver => webDriver.FindElement(By.LinkText("Hello test@email.com! |")).Displayed);
            Assert.AreEqual(0, _driver.FindElements(By.XPath("/html/body/div/div/main/a[4]")).Count);
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }
    }
}
