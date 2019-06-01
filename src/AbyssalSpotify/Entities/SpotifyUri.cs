using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     A set of unique identifiers to identify Spotify entities.
    /// </summary>
    public class SpotifyUri
    {
        /// <summary>
        ///     A resource identifier that clearly identifies the type of entity and the ID of the entity.
        /// </summary>
        /// <example>
        ///     Here is an example of a Spotify URI.
        ///     <code>
        ///     spotify:track:6rqhFgbbKwnb9MLmUQDhG6
        ///     </code>
        /// </example>
        public string Uri { get; }

        /// <summary>
        ///     A base-62 identifier that identifies the entity's ID within it's entity type.
        /// </summary>
        /// <example>
        ///     Here is an example of a Spotify ID.
        ///     <code>
        ///     6rqhFgbbKwnb9MLmUQDhG6
        ///     </code>
        /// </example>
        public string Id { get; }

        /// <summary>
        ///     A unique string identifying the Spotify category (also called an entity type).
        /// </summary>
        public string CategoryId { get; }

        /// <summary>
        ///     A link that opens the entity in a Spotify client.
        /// </summary>
        public string Url { get; }

        internal SpotifyUri(string uri)
        {
            Uri = uri;
            var uriParts = Uri.Split(':');
            if (uriParts.Length != 3) throw new ArgumentException("Attempted to parse a bad Spotify URI.", nameof(uri));

            CategoryId = uriParts[1];
            Id = uriParts[2];

            Url = $"http://open.spotify.com/{CategoryId}/{Id}";
        }
    }
}
