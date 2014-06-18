using System.Configuration;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;

namespace DalSoft.Configuration
{
    public abstract class JsonConfig<TConfig> where TConfig : JsonConfig<TConfig>, new()
    {
        private static TConfig _cachedConfig;
        
        public static TConfig GetSettings()
        {
            if (_cachedConfig != null) return _cachedConfig;
            
            var name = typeof(TConfig).Name;
            var json = CloudConfigurationManager.GetSetting(name) ?? ConfigurationManager.AppSettings[name];
            
            if (json==null)
                throw new ConfigurationErrorsException(string.Format("{0} setting is missing from your config", name));

            return GetConfig(json);
        }

        private static TConfig GetConfig(string json)
        {
            try
            {
                _cachedConfig = JsonConvert.DeserializeObject<TConfig>(json);
                return _cachedConfig;
            }
            
            catch (JsonSerializationException)
            {
                throw new ConfigurationErrorsException(string.Format("The Json value in your setting is invalid.\n{0}", json));
            }
        }
    }
}
