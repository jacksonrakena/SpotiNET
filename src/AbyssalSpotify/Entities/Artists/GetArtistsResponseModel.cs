using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AbyssalSpotify.Entities.Artists
{
    internal class GetArtistsResponseModel
    {
        [JsonPropertyName("artists")]
        public IEnumerable<SpotifyArtist> Artists { get; set; }
    }
}
