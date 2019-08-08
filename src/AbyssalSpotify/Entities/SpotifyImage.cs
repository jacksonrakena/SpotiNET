using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("height")]
        public int? Height { get; }

        /// <summary>
        ///     The image width in pixels. Can be <c>null</c> if unknown.
        /// </summary>
        [JsonPropertyName("width")]
        public int? Width { get; }

        /// <summary>
        ///     The source URL of the image.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; }

        /// <summary>
        ///     Returns the URL of this image.
        /// </summary>
        /// <returns>The URL of this image.</returns>
        public override string ToString() => Url;
    }
}