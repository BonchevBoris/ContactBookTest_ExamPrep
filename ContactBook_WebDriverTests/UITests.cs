using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace ContactBook_WebDriverTests
{
    public class UITests
    {
        private WebDriver driver;
        private const string url = "https://contactbook.nakov.repl.co/";

        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(5);

        }

        [TearDown]  
        public void CloseBrowser()
        {
            driver.Close();
        }

        [Test]
        public void Test_ListContacts_CheckFirstContact()
        {
            var contactButton = driver.FindElement(By.LinkText("Contacts"));
            contactButton.Click();
            var firstContactFirstName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td")).Text;
            var firstContactLastNAme = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td")).Text;
            Assert.That(firstContactFirstName, Is.EqualTo("Steve"));
            Assert.That(firstContactLastNAme, Is.EqualTo("Jobs"));
        }

        [Test]
        public void Test_SearchWithExistingKeyword()
        {
            driver.FindElement(By.LinkText("Search")).Click();
            var keyWordField = driver.FindElement(By.Id("keyword"));
            keyWordField.Clear();
            keyWordField.SendKeys("albert");
            driver.FindElement(By.Id("search")).Click();
            var firstContactFirstName = driver.FindElement(By.CssSelector("#contact3 > tbody > tr.fname > td")).Text;
            var firstContactLastNAme = driver.FindElement(By.CssSelector("#contact3 > tbody > tr.lname > td")).Text;
            Assert.That(firstContactFirstName, Is.EqualTo("Albert"));
            Assert.That(firstContactLastNAme, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_SearchWithInvalidKeyword()
        {
            driver.FindElement(By.LinkText("Search")).Click();
            var keyWordField = driver.FindElement(By.Id("keyword"));
            keyWordField.Clear();
            keyWordField.SendKeys("invalid2635");
            driver.FindElement(By.Id("search")).Click();
            var result = driver.FindElement(By.Id("searchResult")).Text;
            Assert.That(result, Is.EqualTo("No contacts found."));
           
        }

        [Test]
        public void Test_CreateContactWithInvalidData()
        {
            var createButton = driver.FindElement(By.LinkText("Create"));
            createButton.Click();
            driver.FindElement(By.Id("firstName")).SendKeys("Testme");
            driver.FindElement(By.Id("email")).SendKeys("Testme@test.ts");
            driver.FindElement(By.Id("phone")).SendKeys("00025555");
            driver.FindElement(By.Id("create")).Click();
            var message=driver.FindElement(By.CssSelector("body > main > div")).Text;
            Assert.That(message, Is.EqualTo("Error: Last name cannot be empty!"));

        }

        [Test]
        public void Test_CreateContactWithValidDataCkeckItIsOnLastPossition()
        {
            var contactCount =int.Parse(driver.FindElement(By.CssSelector("body > main > section > b")).Text);
            var createButton = driver.FindElement(By.LinkText("Create"));
            createButton.Click();
            driver.FindElement(By.Id("firstName")).SendKeys("Testme");
            driver.FindElement(By.Id("lastName")).SendKeys("TestmeLastname");
            driver.FindElement(By.Id("email")).SendKeys("Testme@test.ts");
            driver.FindElement(By.Id("phone")).SendKeys("00025555");
            driver.FindElement(By.Id("create")).Click();
            var lastContactFirstName = driver.FindElement(By.CssSelector($"#contact{contactCount+1} > tbody > tr.fname > td")).Text;
            var lastContactLastName = driver.FindElement(By.CssSelector($"#contact{contactCount+1} > tbody > tr.lname > td")).Text;
            var lastContactEmail = driver.FindElement(By.CssSelector($"#contact{contactCount+1} > tbody > tr.email > td")).Text;
            var lastContactPhone = driver.FindElement(By.CssSelector($"#contact{contactCount+1} > tbody > tr.phone > td")).Text;
      
            Assert.That(lastContactFirstName, Is.EqualTo("Testme"));
            Assert.That(lastContactLastName, Is.EqualTo("TestmeLastname"));
            Assert.That(lastContactEmail, Is.EqualTo("Testme@test.ts"));
            Assert.That(lastContactPhone, Is.EqualTo("00025555"));

        }
    }
}