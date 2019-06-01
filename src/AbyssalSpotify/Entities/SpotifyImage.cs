using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents an image returned by Spotify's API.
    /// </summary>
    public class SpotifyImage
    {
        /// <summary>
        ///     The image height in pixels. Can be <code>null</code> if unknown.
        /// </summary>
        public int? Height { get; }

        /// <summary>
        ///     The image width in pixels. Can be <code>null</code> if unknown.
        /// </summary>
        public int? Width { get; }

        /// <summary>
        ///     The source URL of the image.
        /// </summary>
        public string Url { get; }

        internal SpotifyImage(int? height, int? width, string url)
        {
            Height = height;
            Width = width;
            Url = url;
        }
    }
}
