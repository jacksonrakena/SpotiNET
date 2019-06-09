using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AbyssalSpotify
{
    internal class SpotifyTrackPagingResponse : ISpotifyPagingResponse<SpotifyTrack>
    {
        public ImmutableArray<SpotifyTrack> Items { get; }

        public int Limit { get; }

        public int Total { get; }

        private readonly SpotifyClient _client;

        internal SpotifyTrackPagingResponse(SpotifyClient client, JToken data)
        {
            _client = client;

            Limit = data["limit"].ToObject<int>();
            Total = data["total"].ToObject<int>();
            Items = data["items"].ToObject<IEnumerable<JObject>>().Select(a => new SpotifyTrack(a, _client)).ToImmutableArray();
        }

        internal SpotifyTrackPagingResponse(SpotifyClient client)
        {
            _client = client;
            Limit = 0;
            Total = 0;
            Items = ImmutableArray<SpotifyTrack>.Empty;
        }
    }
}