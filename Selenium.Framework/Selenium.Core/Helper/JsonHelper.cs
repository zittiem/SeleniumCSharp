using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Selenium.Core.Helper
{
    public class JsonHelper
    {
        private JsonSerializerSettings jsonSerializerSettings;

        public JsonHelper(JsonSerializerSettings serializerSettings)
        {
            jsonSerializerSettings = serializerSettings;
        }

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings);
        }

        public string RemoveProperty(string value, params string[] propertyNames)
        {
            if (propertyNames != null)
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(value, jsonSerializerSettings);
                if (jObject != null)
                {
                    foreach (var name in propertyNames)
                    {
                        jObject.SelectTokens($"$..{name}").ToList().ForEach(x => x.Parent.Remove());
                    }

                    return jObject.ToString();
                }
            }
            return value;
        }

        public string GetPropertyContentFromFile(string filePath, string propertyName)
        {
            string propertyContent = string.Empty;
            string fileContent = FileHelper.ReadAllTextFromFile(filePath);
            var jObject = JsonConvert.DeserializeObject<JObject>(fileContent, jsonSerializerSettings);

            if (jObject != null && jObject.SelectToken($"$..['{propertyName}']") != null)
                propertyContent = jObject.SelectToken($"$..['{propertyName}']").ToString();

            return propertyContent;
        }

        public T GetPropertyObjectFromFile<T>(string filePath, string propertyName)
        {
            return Deserialize<T>(GetPropertyContentFromFile(filePath, propertyName));
        }

        public static JsonHelper Default
        {
            get
            {
                return new JsonHelper(new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DateParseHandling = DateParseHandling.None,
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore
                });
            }
        }

    }
}
