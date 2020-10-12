using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Selenium.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Core.Property
{
    public abstract class DriverProperty
    {
        // Base properties
        public string DriverType { get; set; }
        public string Provider { get; set; }
        public string Capabilities { get; set; }

        // Capabilities
        [JsonProperty("browserName")]
        public string Browser { get; set; }
        [JsonProperty("platformName")]
        public string Platform { get; set; }
        public string RemoteUrl { get; set; }
        public string DownloadLocation { get; set; }
        public string DriverVersion { get; set; }
        public string AdditionalCapabilities { get; set; }

        public DriverProperty(string capabilities)
        {
            Capabilities = capabilities;
            var prop = JsonHelper.Default.Deserialize<DriverProperty>(Capabilities);
            Browser = prop.Browser;
            Platform = Platform ?? JsonHelper.Default.Deserialize<JObject>(Capabilities).SelectToken("platform")?.ToString() ?? "ANY";
            RemoteUrl = prop.RemoteUrl;
            DownloadLocation = prop.DownloadLocation;
            DriverVersion = prop.DriverVersion;
            AdditionalCapabilities = prop.AdditionalCapabilities;
        }

        public T GetOptions<T>() where T : DriverOptions
        {
            var options = JsonHelper.Default.Deserialize<T>(Capabilities);
            Load(options);
            return options;
        }

        protected abstract void Load(DriverOptions options);
    }
}
