using Newtonsoft.Json;
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
        ///     The image height in pixels. Can be <c>null</c> if unknown.
        /// </summary>
        public int? Height { get; }

        /// <summary>
        ///     The image width in pixels. Can be <c>null</c> if unknown.
        /// </summary>
        public int? Width { get; }

        /// <summary>
        ///     The source URL of the image.
        /// </summary>
        public string Url { get; }

        /// <summary>
        ///     Creates a new <see cref="SpotifyImage"/>. This constructor should not be used
        ///     by consumers of this API.
        /// </summary>
        /// <param name="height">The height of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="url">The URL of the image.</param>
        public SpotifyImage(int? height, int? width, string url)
        {
            Height = height;
            Width = width;
            Url = url;
        }

        /// <summary>
        ///     Returns the URL of this image.
        /// </summary>
        /// <returns>The URL of this image.</returns>
        public override string ToString() => Url;
    }
}