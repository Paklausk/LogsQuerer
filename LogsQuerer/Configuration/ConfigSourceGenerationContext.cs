using System.Text.Json.Serialization;

namespace LogsQuerer.Configuration
{
    [JsonSourceGenerationOptions(
        WriteIndented = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        GenerationMode = JsonSourceGenerationMode.Default)]
    [JsonSerializable(typeof(Config))]
    internal partial class ConfigSourceGenerationContext : JsonSerializerContext
    {
    }
}
