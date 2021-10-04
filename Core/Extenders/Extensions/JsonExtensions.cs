using Newtonsoft.Json;

namespace Core
{
    public static class JsonExtensions
    {
        public static T JsonToObject<T>(this string source)
            => JsonConvert.DeserializeObject<T>(source);

        public static string ToJsonString<T>(this T source) where T : class
            => JsonConvert.SerializeObject(source);
    }
}
