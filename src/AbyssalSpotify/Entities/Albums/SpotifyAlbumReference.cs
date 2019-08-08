using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     A light data class containing some album data, sometimes returned by the Spotify API
    ///     in lieu of a full <see cref="SpotifyAlbum"/> object.
    /// </summary>
    public class SpotifyAlbumReference : SpotifyReference<SpotifyAlbum>
    {
        // TODO: Album group

        /// <summary>
        ///     The type of this album.
        /// </summary>
        [JsonPropertyName("album_type")]
        public AlbumType Type { get; }

        /// <summary>
        ///     A list of references to the artists who contributed to this album.
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
        ///     The name of the album. In case of an album takedown, the value may be an
        ///     empty string.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; }

        /// <summary>
        ///     The date the album was first released. Depending on the precision available from Spotify,
        ///     this could be year only (i.e. 1981-01-01), year and month (i.e. 1981-06-01), or year/month/date (i.e. 1981-06-24).
        /// </summary>
        [JsonPropertyName("release_date")]
        public DateTimeOffset ReleaseDate { get; }

        /// <inheritdoc />
        public override Task<SpotifyAlbum> GetFullEntityAsync(SpotifyClient client) => client.GetAlbumAsync(Id.Id);
    }
}