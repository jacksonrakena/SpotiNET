using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents an album entity returned by Spotify.
    /// </summary>
    public class SpotifyAlbum : SpotifyEntity
    {
        /// <summary>
        ///     The type of this album.
        /// </summary>
        [JsonPropertyName("album_type")]
        public AlbumType Type { get; }

        /// <summary>
        ///     The artists that contributed to this album.
        /// </summary>
        [JsonPropertyName("artists")]
        public ImmutableArray<SpotifyArtistReference> Artists { get; }

        /// <summary>
        ///     A list of ISO 3166-1 alpha-2 country codes, representing markets in which this album is available.
        /// </summary>
        /// <remarks>
        ///     An "available market" is a market where at least one of the tracks on this album is available.
        /// </remarks>
        [JsonPropertyName("available_markets")]
        public ImmutableArray<string> AvailableMarkets { get; }

        /// <summary>
        ///     A list of copyright statements held for this album.
        /// </summary>
        [JsonPropertyName("copyrights")]
        public ImmutableArray<SpotifyAlbumCopyright> Copyrights { get; }

        /// <summary>
        ///     A collection of known external IDs for this album.
        /// </summary>
        [JsonPropertyName("external_ids")]
        public SpotifyExternalIdsCollection ExternalIds { get; }

        /// <summary>
        ///     A collection of known external URLs for this album.
        /// </summary>
        [JsonPropertyName("external_urls")]
        public SpotifyExternalUrlsCollection ExternalUrls { get; }

        /// <summary>
        ///     The Spotify ID data for this album.
        /// </summary>
        [JsonPropertyName("uri")]
        public SpotifyUri Id { get; }

        /// <summary>
        ///     Cover art for this album, in various sizes.
        /// </summary>
        [JsonPropertyName("images")]
        public ImmutableArray<SpotifyImage> Images { get; }

        /// <summary>
        ///     The recording/production label for this album.
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; }

        /// <summary>
        ///     The name of the album. In case of an album takedown, the value may be an
        ///     empty string.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; }

        /// <summary>
        ///     The popularity of the album, between 0 and 100, with 100 being the most popular.
        /// </summary>
        [JsonPropertyName("popularity")]
        public int Popularity { get; }

        /// <summary>
        ///     The date the album was first released. Depending on the precision available from Spotify,
        ///     this could be year only (i.e. 1981-01-01), year and month (i.e. 1981-06-01), or year/month/date (i.e. 1981-06-24).
        /// </summary>
        [JsonPropertyName("release_date")]
        public DateTimeOffset ReleaseDate { get; }

        // TODO: Track Relinking

        /// <summary>
        ///     A paging object that contains references to the tracks in this album.
        /// </summary>
        [JsonPropertyName("tracks")]
        public SpotifyPagingResponse<SpotifyTrackReference> Tracks { get; }
    }
}