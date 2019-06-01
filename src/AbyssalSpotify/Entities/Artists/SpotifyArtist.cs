using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents a full artist entity returned by Spotify.
    /// </summary>
    public class SpotifyArtist
    {
        /// <summary>
        ///     A list of all known external URLs for this artist, like Twitter, Facebook, etc.    
        /// </summary>
        /// <example>
        ///     Here is an example of an external URL object.
        ///     <code>
        ///     var spotifyExternalUrl = artist.ExternalUrls["spotify"];
        ///     var facebookExternalUrl = artist.ExternalUrls["facebook"];
        ///     </code>
        /// </example>
        public ImmutableDictionary<string, string> ExternalUrls { get; }

        /// <summary>
        ///     The number of followers that this artist has on Spotify.
        /// </summary>
        public int FollowerCount { get; }

        /// <summary>
        ///     A list of genres this artist is known to be associated with.
        ///     If the artist is not classified, this array will be empty.
        /// </summary>
        public ImmutableList<string> AssociatedGenres { get; }

        /// <summary>
        ///     Images of the artist in various sizes.
        /// </summary>
        public ImmutableList<SpotifyImage> Images { get; }

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

        internal SpotifyArtist(JObject data)
        {
            ExternalUrls = data["external_urls"].ToObject<ImmutableDictionary<string, string>>();
            FollowerCount = data["followers"]["total"].ToObject<int>();
            AssociatedGenres = data["genres"].ToObject<ImmutableList<string>>();
            Images = data["images"].ToObject<ImmutableList<SpotifyImage>>();
            Name = data["name"].ToObject<string>();
            Popularity = data["popularity"].ToObject<int>();
            Id = new SpotifyUri(data["uri"].ToObject<string>());
        }
    }
}
