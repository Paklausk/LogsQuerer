using LogsQuerer.Common;

namespace LogsQuerer.Configuration
{
    public class ConfigException(string message) : AppException(message)
    {
    }

    public class ConfigFileException(string message) : ConfigException(message)
    {
    }
}
