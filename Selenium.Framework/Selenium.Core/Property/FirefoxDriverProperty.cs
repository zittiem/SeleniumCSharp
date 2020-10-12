using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Selenium.Core.Helper;

namespace Selenium.Core.Property
{
    public class FirefoxDriverProperty : DriverProperty
    {
        public string Arguments { get; set; }
        public string Preference { get; set; }

        public FirefoxDriverProperty(string capabilities) : base(capabilities)
        {
            DriverType = "Firefox";
            Provider = "Selenium";
            var prop = JsonHelper.Default.Deserialize<FirefoxDriverProperty>(Capabilities);
            Arguments = prop.Arguments;
            Preference = prop.Preference;
        }

        protected override void Load(DriverOptions options)
        {
            var opts = options as FirefoxOptions;

            if (DownloadLocation != null)
            {
                opts.SetPreference("browser.download.folderList", 2);
                opts.SetPreference("browser.download.dir", DownloadLocation);
            }

            if (Arguments != null)
            {
                opts.AddArguments(JsonHelper.Default.Deserialize<string[]>(Arguments));
            }

            if (Preference != null)
            {
                var pref = JsonHelper.Default.Deserialize<JObject>(Preference);
                foreach (var item in pref)
                {
                    opts.SetPreference(item.Key, item.Value.ToObject<bool>());
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
