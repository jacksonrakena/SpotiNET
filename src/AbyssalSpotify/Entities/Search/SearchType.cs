using System;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents different entites to query.
    /// </summary>
    [Flags]
    public enum SearchType
    {
        /// <summary>
        ///     Queries for albums.
        /// </summary>
        Album,

        /// <summary>
        ///     Queries for artists.
        /// </summary>
        Artist,

        /// <summary>
        ///     Queries for playlists.
        /// </summary>
        Playlist,

        /// <summary>
        ///     Queries for tracks.
        /// </summary>
        Track,

        /// <summary>
        ///     Queries all known types.
        /// </summary>
        All
    }
}