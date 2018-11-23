using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Model;

namespace Core.Helper
{
    /// <summary>
    /// Http helper.
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Reads the header token.
        /// </summary>
        /// <returns>The header token.</returns>
        /// <param name="context">Context.</param>
        public static Token Token(this HttpContext context)
        {
            context.Request.Headers.TryGetValue("Authorization", out StringValues values);
            return values.Count > 0 ? values[0].DecryptToken() : null;
        }

        /// <summary>
        /// Language the specified context.
        /// </summary>
        /// <returns>The language.</returns>
        /// <param name="context">Context.</param>
        public static string Language(this HttpContext context)
        {
            context.Request.Headers.TryGetValue("CF-IPLanguage", out StringValues values);
            return values.Count > 0 ? values[0] : null;
        }

        /// <summary>
        /// Country the specified context.
        /// </summary>
        /// <returns>The country.</returns>
        /// <param name="context">Context.</param>
        public static string Country(this HttpContext context)
        {
            context.Request.Headers.TryGetValue("CF-IPCountry", out StringValues values);
            return values.Count > 0 ? values[0] : null;
        }

        /// <summary>
        /// Ip the specified context.
        /// </summary>
        /// <returns>The ıp.</returns>
        /// <param name="context">Context.</param>
        public static string Ip(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }
    }
}