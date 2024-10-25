using JWTAuthentication.Domain.Core.Configuration.Serialization;
using JWTAuthentication.Domain.Core.ValueObjects;
using Newtonsoft.Json;

namespace JWTAuthentication.Domain.Core.Extensions
{
    public static class JsonOptionsExtensions
    {
        public static IJsonOptions Configure(this IJsonOptions options, Action<JsonSerializerSettings> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new ChainedJsonOptions(options, action);
        }

        public static ChainedJsonOptions Use(this JsonOptions options, Func<JsonSerializerSettings> settingsFactory)
        {
            return new ChainedJsonOptions(options, target =>
            {
                var source = settingsFactory();
                source.CopyTo(target);
            });
        }

        public static IJsonOptions AddSingleValueObjects(this IJsonOptions options)
        {
            return options.AddConverter<SingleValueObjectConverter>();
        }

        public static IJsonOptions AddConverter<T>(this IJsonOptions options)
            where T : JsonConverter, new()
        {
            return options.Configure(s => s.Converters.Insert(0, new T()));
        }

        private static JsonSerializerSettings Clone(this JsonSerializerSettings settings)
        {
            var result = new JsonSerializerSettings();
            settings.CopyTo(result);
            return result;
        }

        private static void CopyTo(this JsonSerializerSettings settings, JsonSerializerSettings target)
        {
            target.CheckAdditionalContent = settings.CheckAdditionalContent;
            target.ConstructorHandling = settings.ConstructorHandling;
            target.Context = settings.Context;
            target.ContractResolver = settings.ContractResolver;
            target.Culture = settings.Culture;
            target.DateFormatHandling = settings.DateFormatHandling;
            target.DateFormatString = settings.DateFormatString;
            target.DateParseHandling = settings.DateParseHandling;
            target.DateTimeZoneHandling = settings.DateTimeZoneHandling;
            target.DefaultValueHandling = settings.DefaultValueHandling;
            target.EqualityComparer = settings.EqualityComparer;
            target.Error = settings.Error;
            target.FloatFormatHandling = settings.FloatFormatHandling;
            target.FloatParseHandling = settings.FloatParseHandling;
            target.Formatting = settings.Formatting;
            target.MaxDepth = settings.MaxDepth;
            target.MetadataPropertyHandling = settings.MetadataPropertyHandling;
            target.MissingMemberHandling = settings.MissingMemberHandling;
            target.NullValueHandling = settings.NullValueHandling;
            target.ObjectCreationHandling = settings.ObjectCreationHandling;
            target.PreserveReferencesHandling = settings.PreserveReferencesHandling;
            target.ReferenceLoopHandling = settings.ReferenceLoopHandling;
            target.ReferenceResolverProvider = settings.ReferenceResolverProvider;
            target.SerializationBinder = settings.SerializationBinder;
            target.StringEscapeHandling = settings.StringEscapeHandling;
            target.TraceWriter = settings.TraceWriter;
            target.TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
            target.TypeNameHandling = settings.TypeNameHandling;
            target.Converters = settings.Converters.ToList();
        }
    }
}
