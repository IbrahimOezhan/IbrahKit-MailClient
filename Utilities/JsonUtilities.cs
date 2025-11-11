using System.Text.Json;

namespace Utilities
{
    internal class JsonUtilities
    {
        private static readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow
        };

        public static bool TryDeserialize<T>(string json, out T t, out Exception ex) where T : new()
        {
            try
            {
                t = JsonSerializer.Deserialize<T>(json, options) ?? throw new NullReferenceException();
                ex = default;
                return true;
            }
            catch(Exception exc)
            {
                t = default;
                ex = exc;
                return false;
            }
        }
    }
}
