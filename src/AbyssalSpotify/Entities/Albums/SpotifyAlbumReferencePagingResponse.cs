using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AbyssalSpotify
{
    internal class SpotifyAlbumReferencePagingResponse : ISpotifyPagingResponse<SpotifyAlbumReference>
    {
        public ImmutableList<SpotifyAlbumReference> Items { get; }

        public int Limit { get; }

        public int Total { get; }

        private readonly SpotifyClient _client;

        internal SpotifyAlbumReferencePagingResponse(SpotifyClient client, JToken data)
        {
            _client = client;

            Limit = data["limit"].ToObject<int>();
            Total = data["total"].ToObject<int>();
            Items = data["items"].ToObject<IEnumerable<JObject>>().Select(a => new SpotifyAlbumReference(a, _client)).ToImmutableList();
        }

        internal SpotifyAlbumReferencePagingResponse(SpotifyClient client)
        {
            _client = client;
            Limit = 0;
            Total = 0;
            Items = ImmutableList<SpotifyAlbumReference>.Empty;
        }
    }
}