using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     A light data class containing some artist data, sometimes returned by the Spotify API
    ///     in lieu of a full <see cref="SpotifyArtist"/> object.
    /// </summary>
    public class SpotifyArtistReference : SpotifyReference<SpotifyArtist>
    {
        /// <summary>
        ///     A list of all known external URLs for this artist, like Twitter, Facebook, etc.
        ///     This dictionary allows custom indexing for unknown properties.
        /// </summary>
        [JsonPropertyName("external_urls")]
        public SpotifyExternalUrlsCollection ExternalUrls { get; }

        /// <summary>
        ///     The Spotify ID data for this artist.
        /// </summary>
        [JsonPropertyName("uri")]
        public SpotifyUri Id { get; }

        /// <summary>
        ///     The name of the artist.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; }

        /// <inheritdoc />
        public override Task<SpotifyArtist> GetFullEntityAsync(SpotifyClient client) => client.GetArtistAsync(Id.Id);
    }
}