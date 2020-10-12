using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Core.Helper;

namespace Selenium.Core.Property
{
    public class ChromeDriverProperty : DriverProperty
    {
        public string Extensions { get; set; }
        public string Arguments { get; set; }
        public string UserProfilePreference { get; set; }
        public string EnableMobileEmulation { get; set; }

        public ChromeDriverProperty(string capabilities) : base(capabilities)
        {
            DriverType = "Chrome";
            Provider = "Selenium";
            var prop = JsonHelper.Default.Deserialize<ChromeDriverProperty>(Capabilities);
            Extensions = prop.Extensions;
            Arguments = prop.Arguments;
            UserProfilePreference = prop.UserProfilePreference;
            EnableMobileEmulation = prop.EnableMobileEmulation;
        }

        protected override void Load(DriverOptions options)
        {
            var opts = options as ChromeOptions;

            if (DownloadLocation != null)
            {
                opts.AddUserProfilePreference("download.default_directory", DownloadLocation);
            }

            if (Arguments != null)
            {
                opts.AddArguments(JsonHelper.Default.Deserialize<string[]>(Arguments));
            }

            if (UserProfilePreference != null)
            {
                var pref = JsonHelper.Default.Deserialize<JObject>(UserProfilePreference);
                foreach (var item in pref)
                {
                    opts.AddUserProfilePreference(item.Key, item.Value);
                }
            }

            if (EnableMobileEmulation != null)
            {
                opts.EnableMobileEmulation(EnableMobileEmulation);
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
