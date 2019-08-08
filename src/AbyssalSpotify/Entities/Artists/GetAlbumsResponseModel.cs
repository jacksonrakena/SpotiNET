using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AbyssalSpotify.Entities.Artists
{
    internal class GetAlbumsResponseModel
    {
        [JsonPropertyName("albums")]
        public IEnumerable<SpotifyAlbum> Albums { get; set; }
    }
}
