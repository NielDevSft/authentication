using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Configuration.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace JWTAuthentication.Domain.Core.Core
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settingsNotIndented;
        private readonly JsonSerializerSettings _settingsIndented;

        public JsonSerializer(IJsonOptions options = null)
        {
            _settingsNotIndented = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CustomContractResolver(),
                Formatting = Formatting.None
            };

            _settingsIndented = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CustomContractResolver(),
                Formatting = Formatting.Indented
            };

            options?.Apply(_settingsIndented);
            options?.Apply(_settingsNotIndented);
        }

        public string Serialize(object obj, bool indented = false)
        {
            var settings = indented ? _settingsIndented : _settingsNotIndented;

            try
            {
                if(obj is IAggregateEvent aggregateEvent) {
                    return "{}";
                }
                return JsonConvert.SerializeObject(obj, settings);
            }
            catch (JsonSerializationException ex)
            {
                throw new InvalidOperationException("Erro durante a serialização: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro inesperado durante a serialização: " + ex.Message, ex);
            }
        }

        public object Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, _settingsNotIndented);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settingsNotIndented);
        }

        // Custom ContractResolver to ignore System.Reflection properties
        private class CustomContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var properties = base.CreateProperties(type, memberSerialization);
                return properties
                    .Where(p => !IsSystemReflectionProperty(p))
                    .ToList();
            }

            private bool IsSystemReflectionProperty(JsonProperty property)
            {
                return property.PropertyType.Assembly == typeof(Assembly).Assembly;
            }
        }
    }
}
