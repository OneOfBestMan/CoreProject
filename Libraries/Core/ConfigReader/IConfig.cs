namespace Core.ConfigReader
{
    /// <summary>
    /// Config.
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="key">Key.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        T GetValue<T>(string key);
    }
}