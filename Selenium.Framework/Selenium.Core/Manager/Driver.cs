using OpenQA.Selenium;
using Selenium.Core.Driver;
using Selenium.Core.Property;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.Core.Manager
{
    public static class Driver
    {
        [ThreadStatic]
        private static Dictionary<string, BaseDriver> driverList;
        [ThreadStatic]
        private static string defaultKey;
        [ThreadStatic]
        private static string currentKey;

        public static IWebDriver WebDriver
        {
            get { return driverList[currentKey].WebDriver; }
        }

        public static DriverProperty Property
        {
            get { return driverList[currentKey].Property; }
        }

        /// <summary>
        /// Create new driver from settings and return it's generated key
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>Generated key</returns>
        public static string Add(string settings, string customKey = null)
        {
            if (driverList == null)
            {
                driverList = new Dictionary<string, BaseDriver>();
            }

            var driver = DriverFactory.Generate(settings);

            if (customKey != null)
                currentKey = customKey;
            else
                currentKey = GetKey(driver.Property.DriverType);
            
            if (defaultKey == null)
                defaultKey = currentKey;

            driverList.Add(currentKey, DriverFactory.Generate(settings));
            return currentKey;
        }

        public static void Close()
        {
            if (driverList != null)
            {
                if (driverList[currentKey].WebDriver.WindowHandles.Count > 1)
                    driverList[currentKey].WebDriver.Close();
                else
                    Quit();
            }
        }

        public static void Quit()
        {
            if (driverList != null)
            {
                driverList[currentKey].WebDriver.Quit();
                if (driverList.Keys.Count > 0)
                {
                    if (currentKey == defaultKey)
                        defaultKey = driverList.Keys.First();
                    currentKey = driverList.Keys.Last();
                }
                else
                {
                    QuitAll();
                }
            }
        }

        public static void QuitAll()
        {
            if (driverList != null)
            {
                foreach (var item in driverList.Values)
                {
                    item.WebDriver.Quit();
                }
                driverList = null;
                defaultKey = null;
                currentKey = null;
            }
        }

        private static string GetKey(string prefix)
        {
            string key;
            int number = 1;
            while (true)
            {
                key = prefix + "-" + number;
                if (!driverList.ContainsKey(key))
                    return key;
                number++;
            }
        }
    }
}
