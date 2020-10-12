using Newtonsoft.Json.Linq;
using Selenium.Core.Driver;
using Selenium.Core.Helper;
using System;
using System.Reflection;

namespace Selenium.Core.Manager
{
    public class DriverFactory
    {
        public static BaseDriver Generate(string settings)
        {
            var caps = JsonHelper.Default.Deserialize<JObject>(settings);
            string provider = caps.SelectToken("provider")?.ToString() ?? "Selenium";
            string driverType = caps.SelectToken("driverType")?.ToString() ?? "Chrome";
            string capabilities = caps.SelectToken("capabilities")?.ToString();
            Type type = Type.GetType($"Selenium.Core.Driver.{provider}.{driverType}Driver");
            object obj = Activator.CreateInstance(type);
            return (BaseDriver)type.InvokeMember("Create", BindingFlags.InvokeMethod, null, obj, new object[] { capabilities });
        }   
    }
}
