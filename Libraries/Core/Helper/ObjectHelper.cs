using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Helper
{
    /// <summary>
    /// Object helper.
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Tos the byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        /// <param name="obj">Object.</param>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;

            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// T to the object.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="arrBytes">Arr bytes.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T ToObject<T>(this byte[] arrBytes) where T : class, new()
        {
            if (arrBytes == null)
            {
                return null;
            }
            var memStream = new MemoryStream();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return (T)new BinaryFormatter().Deserialize(memStream);
        }

        /// <summary>
        /// Nots the null.
        /// </summary>
        /// <returns><c>true</c>, if null was noted, <c>false</c> otherwise.</returns>
        /// <param name="obj">Object.</param>
        public static bool IsNull(this object obj) => obj == null;

        /// <summary>
        /// Tos the date time.
        /// </summary>
        /// <returns>The date time.</returns>
        /// <param name="seconds">Seconds.</param>
        public static DateTime ToDateTime(this long? seconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)seconds);
        }

        /// <summary>
        /// Tos the date time.
        /// </summary>
        /// <returns>The date time.</returns>
        /// <param name="seconds">Seconds.</param>
        public static DateTime ToDateTime(this string seconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToInt64(seconds));
        }
    }
}