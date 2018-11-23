namespace Core.Localization
{
    /// <summary>
    /// Localization.
    /// </summary>
    public interface ILocalization
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="countryCode">Country Code.</param>
        /// <param name="key">Key.</param>
        string Get(string countryCode, string key);

        /// <summary>
        /// Get the specified key.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="key">Key.</param>
        string Get(string key);
    }
}