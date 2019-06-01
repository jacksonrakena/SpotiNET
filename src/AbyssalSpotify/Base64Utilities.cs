using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    public static class Base64Utilities
    {
        /// <summary>
        ///     Encodes a UTF8 string to base64.
        /// </summary>
        /// <param name="text">A UTF8 string.</param>
        /// <returns>The base64 equivalent of <paramref name="text"/>.</returns>
        public static string EncodeBase64(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        ///     Decodes a base64 string into UTF8.
        /// </summary>
        /// <param name="data">A base64 string.</param>
        /// <returns>The UTF8 equivalent of <paramref name="data"/>.</returns>
        public static string DecodeBase64(string data)
        {
            var bytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
