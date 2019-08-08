using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AbyssalSpotify.Entities.Artists
{
    /// <summary>
    ///     Represents follower data for a <see cref="SpotifyArtist"/>.
    /// </summary>
    public class SpotifyArtistFollowers
    {
        /// <summary>
        ///     The total amount of followers this artist has.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
