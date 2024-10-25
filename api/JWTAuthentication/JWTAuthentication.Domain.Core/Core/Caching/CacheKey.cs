
using JWTAuthentication.Domain.Core.Extensions;
using JWTAuthentication.Domain.Core.ValueObjects;
using Newtonsoft.Json;

namespace JWTAuthentication.Domain.Core.Core.Caching
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class CacheKey : SingleValueObject<string>
    {
        public const int MaxLength = 256;

        public static CacheKey With(params string[] keys)
        {
            return new CacheKey(string.Join("-", keys));
        }

        public static CacheKey With(Type ownerType, params string[] keys)
        {
            return With($"{ownerType.GetCacheKey()}:{string.Join("-", keys)}");
        }

        public CacheKey(string value) : base(value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            if (value.Length > MaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Cache keys can maximum be '{MaxLength}' in length");
        }
    }
}