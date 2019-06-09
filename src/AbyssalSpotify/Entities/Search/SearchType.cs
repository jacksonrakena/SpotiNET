using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    [Flags]
    public enum SearchType
    {
        [JsonProperty("album")]
        Album,

        [JsonProperty("artist")]
        Artist,

        [JsonProperty("playlist")]
        Playlist,

        [JsonProperty("track")]
        Track
    }
}