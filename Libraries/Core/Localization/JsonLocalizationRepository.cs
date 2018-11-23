using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Core.ConfigReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.PlatformAbstractions;

namespace Core.Localization
{
    /// <summary>
    /// Json localization repository.
    /// </summary>
    public class JsonLocalizationRepository : ILocalization
    {
        /// <summary>
        /// The values.
        /// </summary>
        static Dictionary<string, string> _values;
        /// <summary>
        /// The config.
        /// </summary>
        readonly IConfig _config;
        /// <summary>
        /// The http context accessor.
        /// </summary>
        readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.Localization.JsonLocalizationRepository"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="httpContextAccessor">Http context accessor.</param>
        public JsonLocalizationRepository(IConfig config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _values = GetValues();
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <returns>The values.</returns>
        Dictionary<string, string> GetValues()
        {
            var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "localization.json");

            using (var streamReader = new StreamReader(filePath))
            {
                return JObject.Parse(streamReader.ReadToEnd())["Localization"].ToObject<Dictionary<string, string>>();
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="countryCode">Country Code.</param>
        /// <param name="key">Key.</param>
        public string Get(string countryCode, string key)
        {
            if (_values.TryGetValue($"{countryCode.ToLower()}_{key}", out string config))
            {
                return config.Replace($"{countryCode.ToLower()}_", "");
            }
            return key.Replace($"{countryCode.ToLower()}_", "");
        }

        /// <summary>
        /// Get the specified key.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="key">Key.</param>
        public string Get(string key)
        {
            var defaultLanguage = _config.GetValue<string>("DefaultLanguage");
            string userLanguage = null;


            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("CF-IPLanguage", out StringValues values);
            if (values.Count > 0)
            {
                userLanguage = values[0].ToLower();
            }

            if (userLanguage == null)
            {
                if (_values.TryGetValue($"{defaultLanguage.ToLower()}_{key}", out string config2))
                {
                    return config2.Replace($"{defaultLanguage.ToLower()}_", "");
                }
                return key.Replace($"{defaultLanguage.ToLower()}_", "");
            }

            _values.TryGetValue($"{userLanguage.ToLower()}_{key}", out string userLangValue);
            if (!string.IsNullOrEmpty(userLangValue))
            {
                return userLangValue.Replace($"{userLanguage.ToLower()}_", "");
            }
            if (_values.TryGetValue($"{defaultLanguage.ToLower()}_{key}", out string config))
            {
                return config.Replace($"{defaultLanguage.ToLower()}_", "");
            }
            return key.Replace($"{defaultLanguage.ToLower()}_", "");

        }
    }
}