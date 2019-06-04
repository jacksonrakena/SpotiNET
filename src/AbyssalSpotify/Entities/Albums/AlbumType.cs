using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents the type of an album.
    /// </summary>
    public enum AlbumType
    {
        /// <summary>
        ///     A traditional album.
        /// </summary>
        [EnumMember(Value = "album")]
        Album,

        /// <summary>
        ///     A single, usually containing less than five tracks.
        /// </summary>
        [EnumMember(Value = "single")]
        Single,

        /// <summary>
        ///     A compilation of tracks.
        /// </summary>
        [EnumMember(Value = "compilation")]
        Compilation
    }
}