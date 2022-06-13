using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace ContactBookAndroidTests
{
    public class AndroidTest
    {
        private const string AppiumServerUri = "http://127.0.0.1:4723/wd/hub";
        private const string AppPath = @"D:\SoftUni\QA\Demo\contactbook-androidclient.apk";
        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void Setup()
        {
            this.options=new AppiumOptions() { PlatformName="Android"};
            options.AddAdditionalCapability("app", AppPath);
            driver=new AndroidDriver<AndroidElement>(new Uri(AppiumServerUri),options);  
            driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(5);
        }

       
        [Test]
        public void Test_SearchExistingContact()
        {
            var textApiUrlField = driver.FindElementById("contactbook.androidclient:id/editTextApiUrl");
            textApiUrlField.Clear();
            textApiUrlField.SendKeys("https://contactbook.nakov.repl.co/api");
            var connectButton = driver.FindElementById("contactbook.androidclient:id/buttonConnect");
            connectButton.Click();
            var keyWordTextBox = driver.FindElementById("contactbook.androidclient:id/editTextKeyword");
            keyWordTextBox.SendKeys("steve");
            var searchButton = driver.FindElementById("contactbook.androidclient:id/buttonSearch");
            searchButton.Click();
            var firstName = driver.FindElementByXPath("//android.widget.TableRow[3]/android.widget.TextView[2]").Text;
            var lastName = driver.FindElementByXPath("//android.widget.TableRow[4]/android.widget.TextView[2]").Text;

            Assert.That(firstName, Is.EqualTo("Steve"));
            Assert.That(lastName, Is.EqualTo("Jobs"));
        }
    }
}