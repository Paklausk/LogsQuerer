namespace LogsQuerer.Configuration
{
    public interface IConfigFileLoader
    {
        /// <summary>
        /// Load the configuration from the given path
        /// </summary>
        Config Load(string configPath);
    }
}
