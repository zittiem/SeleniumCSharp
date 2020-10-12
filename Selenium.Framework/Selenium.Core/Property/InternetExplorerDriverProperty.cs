using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using Selenium.Core.Helper;

namespace Selenium.Core.Property
{
    public class InternetExplorerDriverProperty : DriverProperty
    {
        public InternetExplorerDriverProperty(string capabilities) : base(capabilities)
        {
            DriverType = "InternetExplorer";
            Provider = "Selenium";
        }

        protected override void Load(DriverOptions options)
        {
            var opts = options as InternetExplorerOptions;

            if (DownloadLocation != null)
            {
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main", true);
                if (myKey != null)
                {
                    if (myKey.GetValue("Default Download Directory") == null || myKey.GetValue("Default Download Directory").ToString() != DownloadLocation)
                    {
                        myKey.SetValue("Default Download Directory", DownloadLocation);
                    }
                    myKey.Close();
                }

                myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\3", true);
                if (myKey != null)
                {
                    if (myKey.GetValue("1803") == null || myKey.GetValue("1803").ToString() != "0")
                    {
                        myKey.SetValue("1803", 0);
                    }
                    myKey.Close();
                }

            }

            if (AdditionalCapabilities != null)
            {
                var capabilities = JsonHelper.Default.Deserialize<JObject>(AdditionalCapabilities);
                foreach (var item in capabilities)
                {
                    opts.AddAdditionalCapability(item.Key, item.Value);
                }
            }
        }
    }
}
