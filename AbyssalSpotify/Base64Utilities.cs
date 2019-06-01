using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    public static class Base64Utilities
    {
        public static string EncodeBase64(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeBase64(string data)
        {
            var bytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
