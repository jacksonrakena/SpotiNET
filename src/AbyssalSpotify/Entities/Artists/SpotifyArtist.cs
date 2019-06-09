using AbyssalSpotify;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
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
        public SpotifyExternalUrlsCollection ExternalUrls { get; }

        /// <summary>
        ///     The number of followers that this artist has on Spotify.
        /// </summary>
        public int FollowerCount { get; }

        /// <summary>
        ///     A list of genres this artist is known to be associated with.
        ///     If the artist is not classified, this array will be empty.
        /// </summary>
        public ImmutableArray<string> AssociatedGenres { get; }

        /// <summary>
        ///     Images of the artist in various sizes.
        /// </summary>
        public ImmutableArray<SpotifyImage> Images { get; }

        /// <summary>
        ///     The name of the artist.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The popularity of the artist, between 0 and 100, with 100 being the most popular.
        /// </summary>
        public int Popularity { get; }

        /// <summary>
        ///     The Spotify ID data for this artist.
        /// </summary>
        public SpotifyUri Id { get; }

        /// <summary>
        ///     Gets this artist's Related Artists.
        /// </summary>
        /// <returns>
        ///     An asynchronous operation representing the Related Artists of this artist.
        /// </returns>
        public Task<ImmutableArray<SpotifyArtist>> GetRelatedArtistsAsync() => Client.GetRelatedArtistsAsync(Id.Id);

        internal SpotifyArtist(SpotifyClient client, JObject data) : base(client)
        {
            ExternalUrls = new SpotifyExternalUrlsCollection(data["external_urls"].ToObject<IDictionary<string, string>>());
            FollowerCount = data["followers"]["total"].ToObject<int>();
            AssociatedGenres = data["genres"].ToObject<ImmutableArray<string>>();
            Images = data["images"].ToObject<ImmutableArray<SpotifyImage>>();
            Name = data["name"].ToObject<string>();
            Popularity = data["popularity"].ToObject<int>();
            Id = new SpotifyUri(data["uri"].ToObject<string>());
        }
    }
}