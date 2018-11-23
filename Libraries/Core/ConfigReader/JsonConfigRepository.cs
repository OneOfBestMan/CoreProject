using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Linq;

namespace Core.ConfigReader
{
    /// <summary>
    /// Json config repository.
    /// </summary>
    public class JsonConfigRepository : IConfig
    {
        /// <summary>
        /// The values.
        /// </summary>
        static Dictionary<string, string> _values;
        /// <summary>
        /// The environment.
        /// </summary>
        readonly string _environment = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.ConfigReader.JsonConfigRepository"/> class.
        /// </summary>
        /// <param name="environment">Environment.</param>
        public JsonConfigRepository(string environment = null)
        {
            if (environment != null)
                _environment = environment;

            _values = GetValues();
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <returns>The values.</returns>
        Dictionary<string, string> GetValues()
        {
            var configFileName = "appsettings.json";
            if (!string.IsNullOrEmpty(_environment))
                configFileName = $"appsettings.{_environment}.json";


            var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, configFileName);

            using (var streamReader = new StreamReader(filePath))
            {
                return JObject.Parse(streamReader.ReadToEnd())["Config"].ToObject<Dictionary<string, string>>();
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="key">Key.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T GetValue<T>(string key)
        {
            if (_values.TryGetValue(key, out string config))
            {
                return (T)Convert.ChangeType(config, typeof(T));
            }
            throw new ArgumentNullException($"{key} not found.");
        }
    }
}