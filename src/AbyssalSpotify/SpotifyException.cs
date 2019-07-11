using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents an error returned by the Spotify API.
    /// </summary>
    public class SpotifyException : Exception
    {
        internal SpotifyException(int statusCode, string message) : base ($"Spotify returned {statusCode} error: {message}")
        {
        }
    }
}
