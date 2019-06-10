using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     An offset-based paging object, used as a container for a set of Spotify objects.
    /// </summary>
    /// <typeparam name="T">The type of the objects inside this container.</typeparam>
    public class SpotifyPagingResponse<T>
    {
        /// <summary>
        ///     The requested items.
        /// </summary>
        public ImmutableArray<T> Items { get; private set; }

        private string nextUrl;
        private string previousUrl;
        private readonly Func<JObject, SpotifyClient, T> _objectBuilder;
        private readonly Func<JObject, JObject> _dataAccessor;
        private readonly SpotifyClient _client;
        private readonly bool _isEmpty;

        private void UpdateData(JObject data)
        {
            nextUrl = data["next"].ToObject<string>();
            previousUrl = data["previous"].ToObject<string>();

            Items = data["items"].ToObject<IEnumerable<JObject>>().Select(a => _objectBuilder(a, _client)).ToImmutableArray();
        }

        internal SpotifyPagingResponse(JObject baseData, Func<JObject, SpotifyClient, T> objectBuilder, Func<JObject, JObject> dataAccessor, SpotifyClient client)
        {
            _objectBuilder = objectBuilder;
            _dataAccessor = dataAccessor;
            _client = client;
            _isEmpty = false;

            UpdateData(baseData);
        }

        internal SpotifyPagingResponse()
        {
            _isEmpty = true;
        }

        // TODO: int limit
        /// <summary>
        ///     Advances to the next page in the query or request, and clears the current page.
        /// </summary>
        /// <returns>
        ///     An asynchronous operation representing whether the next page was actually advanced to. This can sometimes
        ///     be false if there is no next page, or this pager is empty and represents no request.
        /// </returns>
        public async Task<bool> GetNextAsync()
        {
            if (_isEmpty || nextUrl == null) return false;
            var response = await _client.InternalRequestAsync(new Uri(nextUrl), HttpMethod.Get).ConfigureAwait(false);
            if (response == null) return false;
            var data = _dataAccessor(response);
            if (data == null) return false;
            UpdateData(data);
            return true;
        }

        /// <summary>
        ///     Goes back to the previous page in the query or request, and clears the current page.
        /// </summary>
        /// <returns>
        ///     An asynchronous operation representing whether the preivous page was actually went back to. This can sometimes
        ///     be false if there is no previous page, or this pager is empty and represents no request.
        /// </returns>
        public async Task<bool> GetPreviousAsync()
        {
            if (_isEmpty || previousUrl == null) return false;
            var response = await _client.InternalRequestAsync(new Uri(previousUrl), HttpMethod.Get).ConfigureAwait(false);
            if (response == null) return false;
            var data = _dataAccessor(response);
            if (data == null) return false;
            UpdateData(data);
            return true;
        }
    }
}