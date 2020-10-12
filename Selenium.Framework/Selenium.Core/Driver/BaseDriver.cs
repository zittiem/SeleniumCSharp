using OpenQA.Selenium;
using Selenium.Core.Manager;
using Selenium.Core.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Core.Driver
{
    public abstract class BaseDriver
    {
        public IWebDriver WebDriver;
        public DriverProperty Property;

        public abstract void Create(string capabilities);
    }
}
