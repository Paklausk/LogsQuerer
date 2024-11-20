using LogsQuerer.Common;

namespace LogsQuerer.Logs
{
    public class LogException(string message) : AppException(message)
    {
    }

    public class LogFileException(string message) : LogException(message)
    {
    }
}
