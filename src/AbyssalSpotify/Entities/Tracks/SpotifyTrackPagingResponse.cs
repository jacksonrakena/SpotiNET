using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AbyssalSpotify
{
    public class SpotifyTrackReferencePagingResponse : ISpotifyPagingResponse<SpotifyTrackReference>
    {
        public ImmutableList<SpotifyTrackReference> Items { get; }

        public int Limit { get; }

        public int Total { get; }

        private readonly SpotifyClient _client;

        internal SpotifyTrackReferencePagingResponse(SpotifyClient client, JToken data)
        {
            _client = client;

            Limit = data["limit"].ToObject<int>();
            Total = data["total"].ToObject<int>();
            Items = data["items"].ToObject<IEnumerable<JObject>>().Select(a => new SpotifyTrackReference(_client, a)).ToImmutableList();
        }
    }
}