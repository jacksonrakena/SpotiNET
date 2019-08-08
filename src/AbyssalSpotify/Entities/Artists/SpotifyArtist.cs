using AbyssalSpotify;
using AbyssalSpotify.Entities.Artists;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents a full artist entity returned by Spotify.
    /// </summary>
    public class SpotifyArtist : SpotifyEntity
    {
        /// <summary>
        ///     A list of all known external URLs for this artist, like Twitter, Facebook, etc.
        ///     This dictionary allows custom indexing for unknown properties.
        /// </summary>
        [JsonPropertyName("external_urls")]
        public Dictionary<string, string> ExternalUrls { get; set; }

        /// <summary>
        ///     The follower data for this artist.
        /// </summary>
        [JsonPropertyName("followers")]
        public SpotifyArtistFollowers Followers { get; set; }

        /// <summary>
        ///     A list of genres this artist is known to be associated with.
        ///     If the artist is not classified, this array will be empty.
        /// </summary>
        [JsonPropertyName("genres")]
        public List<string> AssociatedGenres { get; set; }

        /// <summary>
        ///     Images of the artist in various sizes.
        /// </summary>
        [JsonPropertyName("images")]
        public List<SpotifyImage> Images { get; set; }

        /// <summary>
        ///     The name of the artist.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        ///     The popularity of the artist, between 0 and 100, with 100 being the most popular.
        /// </summary>
        [JsonPropertyName("popularity")]
        public int Popularity { get; set; }

        /// <summary>
        ///     The Spotify ID data for this artist.
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        /// <summary>
        ///     Gets this artist's Related Artists.
        /// </summary>
        /// <returns>
        ///     An asynchronous operation representing the Related Artists of this artist.
        /// </returns>
        public Task<ImmutableArray<SpotifyArtist>> GetRelatedArtistsAsync(SpotifyClient client) => client.GetRelatedArtistsAsync(Uri);
    }
}