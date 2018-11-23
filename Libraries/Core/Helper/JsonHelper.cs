using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Core.Helper
{
    /// <summary>
    /// Json helper.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// verilen objeyi, JSON'a çevirir.
        /// Varsayılan Ayar: "ReferenceLoopHandling.Ignore","NullValueHandling.Ignore"
        /// Varsayılan Format: "Formatting.Indented" 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return obj.ToJson(null);
        }

        /// <summary>
        /// Tos the json.
        /// </summary>
        /// <returns>The json.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="nullValue">Null value.</param>
        public static string ToJson(this object obj, string nullValue)
        {
            var defaultSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };
            var iso = new Newtonsoft.Json.Converters.IsoDateTimeConverter();

            //iso.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFKzzz";
            defaultSettings.Converters.Add(iso);
            return obj.ToJson(defaultSettings, nullValue);
        }

        /// <summary>
        /// Tos the json.
        /// </summary>
        /// <returns>The json.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="nullValue">Null value.</param>
        public static string ToJson(this object obj, JsonSerializerSettings settings, string nullValue)
        {
            var json = nullValue;
            if (obj != null)
            {
                json = JsonConvert.SerializeObject(obj, Formatting.None, settings);
            }

            return json;
        }

        /// <summary>
        /// verilen json formatını, tanımlanan tipe çevirir.
        /// Varsayılan Ayar: "ReferenceLoopHandling.Ignore"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string jsonString)
        {
            var defaultSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                NullValueHandling = NullValueHandling.Include
            };
            var iso = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            //iso.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFKzzz";

            defaultSettings.Converters.Add(iso);

            return JsonConvert.DeserializeObject<T>(jsonString, defaultSettings);
        }

        const string IndentString = "    ";
        /// <summary>
        /// Formats the json.
        /// </summary>
        /// <returns>The json.</returns>
        /// <param name="str">String.</param>
        public static string FormatJson(string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, ++indent).ForEach(item => sb.Append(IndentString));
                    }
                    break;
                    case '}':
                    case ']':
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, --indent).ForEach(item => sb.Append(IndentString));
                    }
                    sb.Append(ch);
                    break;
                    case '"':
                    sb.Append(ch);
                    var escaped = false;
                    var index = i;
                    while (index > 0 && str[--index] == '\\')
                        escaped = !escaped;
                    if (!escaped)
                        quoted = !quoted;
                    break;
                    case ',':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, indent).ForEach(item => sb.Append(IndentString));
                    }
                    break;
                    case ':':
                    sb.Append(ch);
                    if (!quoted)
                        sb.Append(" ");
                    break;
                    default:
                    sb.Append(ch);
                    break;
                }
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Extensions.
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <param name="ie">İe.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}