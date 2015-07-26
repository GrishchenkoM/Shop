using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;

namespace IntegrationTests
{
    [TestFixture]
    public class MyPageTests
    {
        public static string BaseUrl = "http://localhost:16690/";
        [Test(Description = "Log On")]
        public void UserLogOn()
        {
            //Failed to start up socket within 45000 ms. Attempted to connect to the following addresses: 127.0.0.1:7055
            //var profile = new FirefoxProfile {Port = 16690};
           
            //var driver = new FirefoxDriver();

            //var fireBin = new FirefoxBinary("C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe");
            //fireBin.TimeoutInMilliseconds = 130000;
            //var fireProfile = new FirefoxProfile {Port = 9966};
            FirefoxDriver driver = null;
            try
            {
                driver = new FirefoxDriver(DesiredCapabilities.Firefox());
                driver.Navigate().GoToUrl(BaseUrl);
                driver.FindElement(By.Name("LogIn")).Click();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally {if (driver != null) driver.Quit();}



        }
    }
}
