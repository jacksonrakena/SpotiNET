using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    public class SpotifyException : Exception
    {
        public SpotifyException(int statusCode, string message) : base ($"Spotify returned {statusCode} error: {message}")
        {
        }
    }
}
