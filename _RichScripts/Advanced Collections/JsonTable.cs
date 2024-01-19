using Newtonsoft.Json;
using System.Collections.Generic;

namespace ScaryRobot.LostInSpace
{
    [System.Serializable]
    public class JsonTable : Dictionary<string, string>
    {
        public void Set<T>(string key, T value)
        {
            Add(key, JsonConvert.SerializeObject(value));
        }
        
        public T Get<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(key);
        }
        
        public T GetOrDefault<T>(string key, T @default = default)
        {
            return TryGetValue(key, out string value)
                ? JsonConvert.DeserializeObject<T>(value)
                : default;
        }
    }
}
