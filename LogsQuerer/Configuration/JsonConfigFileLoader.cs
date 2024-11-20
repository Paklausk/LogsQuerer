using System.Text.Json;

namespace LogsQuerer.Configuration
{
    public class JsonConfigFileLoader : IConfigFileLoader
    {
        public Config Load(string configPath)
        {
            if (File.Exists(configPath) == false)
            {
                throw new ConfigFileException($"Config file '{configPath}' not found");
            }

            var jsonText = File.ReadAllText(configPath);

            if (string.IsNullOrWhiteSpace(jsonText))
            {
                throw new ConfigFileException($"Config file '{configPath}' is empty");
            }

            try
            {
                var config = JsonSerializer.Deserialize(jsonText, typeof(Config), ConfigSourceGenerationContext.Default) as Config;

                if (config is null)
                {
                    throw new ConfigFileException($"Config file '{configPath}' is empty");
                }

                return config;
            }
            catch (JsonException ex)
            {
                throw new ConfigFileException($"Error deserializing json config file '{configPath}': {ex.Message}");
            }
        }
    }
}
