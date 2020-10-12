using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Selenium.Core.Property;
using System;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Selenium.Core.Driver.Selenium
{
    public class ChromeDriver : BaseDriver
    {
        public override void Create(string capabilities)
        {
            Property = new ChromeDriverProperty(capabilities);

            if (Property.DriverVersion != null)
            {
                new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), Property.DriverVersion);
            }
            else
            {
                new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            }

            var options = Property.GetOptions<ChromeOptions>();

            if (Property.RemoteUrl != null)
            {
                WebDriver = new RemoteWebDriver(new Uri(Property.RemoteUrl), options);
            }
            else
            {
                WebDriver = new OpenQA.Selenium.Chrome.ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(180));
            }
        }
    }
}
