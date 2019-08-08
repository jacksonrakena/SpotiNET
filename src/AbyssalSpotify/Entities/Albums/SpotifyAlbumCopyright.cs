using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents Spotify copyright data for an album.
    /// </summary>
    public class SpotifyAlbumCopyright
    {
        /// <summary>
        ///     The type of this copyright.
        /// </summary>
        [JsonPropertyName("type")]
        public AlbumCopyrightType CopyrightType { get; }

        /// <summary>
        ///     The text of the copyright.
        /// </summary>
        [JsonPropertyName("text")]
        public string CopyrightText { get; }

        internal SpotifyAlbumCopyright(string text, AlbumCopyrightType act)
        {
            CopyrightText = text;
            CopyrightType = act;
        }
    }

    /// <summary>
    ///     Represents the type of copyright that this piece of data holds.
    /// </summary>
    public enum AlbumCopyrightType
    {
        /// <summary>
        ///     The standard (release) copyright.
        /// </summary>
        [EnumMember(Value = "C")]
        Copyright,

        /// <summary>
        ///     The performance (sound recording) copyright.
        /// </summary>
        [EnumMember(Value = "P")]
        PerformanceCopyright
    }
}