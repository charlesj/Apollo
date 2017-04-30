using System.IO;
using Apollo.Utilities;

namespace Apollo.Console
{
    public class Configuration
    {
        public string Endpoint { get; set; }
        public string LoginToken { get; set; }

        public static Configuration GetDefaults()
        {
            return new Configuration
            {
                Endpoint = "http://192.168.142.10/api",
                LoginToken = string.Empty
            };
        }
    }

    public class ConfigurationReader
    {
        private EnvironmentReader environmentReader;
        private ApolloJsonSerializer jsonSerializer;

        public ConfigurationReader()
        {
            this.environmentReader = new EnvironmentReader();
            this.jsonSerializer = new ApolloJsonSerializer();
        }

        public Configuration GetConfiguration()
        {
            var configFilePath = GetConfigFilePath();

            if (!File.Exists(configFilePath))
            {
                var defaults = Configuration.GetDefaults();
                var serialized = this.jsonSerializer.Serialize(defaults);
                File.WriteAllLines(configFilePath, new[] {serialized});
                return defaults;
            }

            var contents = File.ReadAllText(configFilePath);
            var configuration = this.jsonSerializer.Deserialize<Configuration>(contents);

            return configuration;
        }

        public void UpdateConfiguration(Configuration config)
        {
            var configFilePath = GetConfigFilePath();
            var serialized = this.jsonSerializer.Serialize(config);
            File.WriteAllLines(configFilePath, new[] {serialized});
        }

        private string GetConfigFilePath()
        {
            var configFilePath = this.environmentReader.Read("APOLLO_CONFIGFILE");
            if (configFilePath == null)
                configFilePath = "config.json";
            return configFilePath;
        }
    }
}