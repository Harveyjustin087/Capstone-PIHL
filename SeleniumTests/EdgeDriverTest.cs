using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace SeleniumTests
{
    [TestClass]
    public class EdgeDriverTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from http://go.microsoft.com/fwlink/?LinkId=619687
        // to install Microsoft WebDriver.

        IWebDriver driver;
        WebDriverWait wait;
        private Dictionary<string, string> testUserFixture = new Dictionary<string, string>();

        [OneTimeSetUp]
        public void SetUpTheFixture()
        {
            testUserFixture.Add("UserName", "testUser");
            testUserFixture.Add("FirstName", "John");
            testUserFixture.Add("LastName", "Doe");
            testUserFixture.Add("Email", "j_doe@test.com");
            testUserFixture.Add("Password", "aA1234*");
            testUserFixture.Add("ConfirmPassword", "aA1234*");
        }

        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("http://localhost:5000");
        }

        [Test, Order(1)]
        public void TestDoesNotShowPagesContentToUnauthorizedUsers()
        {
            driver.FindElement(By.LinkText("Profile")).Click();
            wait.Until(webDriver => webDriver.FindElement(By.Id("Unauthorized_View")).Displayed);
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            driver.Quit();
        }
    }
}
