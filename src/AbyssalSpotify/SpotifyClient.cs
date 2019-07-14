using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents an HTTP client that can be used to interface with Spotify's API.
    /// </summary>
    public class SpotifyClient
    {
        internal HttpClient HttpClient { get; }

        internal ISpotifyAuthorizer Authorizer { get; }

        internal SpotifyClient(ISpotifyAuthorizer authorizer)
        {
            HttpClient = new HttpClient();
            Authorizer = authorizer;
        }

        /// <summary>
        ///     Creates a new <see cref="SpotifyClient"/> using a client ID and a client secret.
        /// </summary>
        /// <param name="clientId">The ID of the client.</param>
        /// <param name="clientSecret">The client's secret key.</param>
        /// <returns>A <see cref="SpotifyClient"/> that uses a client ID and client secret to authorize.</returns>
        public static SpotifyClient FromClientCredentials(string clientId, string clientSecret)
        {
            return FromClientCredentials($"{clientId}:{clientSecret}");
        }

        /// <summary>
        ///     Creates a new <see cref="SpotifyClient"/> using a combined set of client credentials, using format "[client id]:[client secret]".
        /// </summary>
        /// <example>
        ///     This sample shows how to use <see cref="SpotifyClient.FromClientCredentials(string)"/> to create a <see cref="SpotifyClient"/>
        ///     using a combined credential string.
        ///     <code>
        ///     var client = SpotifyClient.FromClientCredentials("MyClientId:MyClientSecret");
        ///     </code>
        /// </example>
        /// <param name="combinedClientCredentials">The combined credential string.</param>
        /// <returns></returns>
        public static SpotifyClient FromClientCredentials(string combinedClientCredentials)
        {
            return new SpotifyClient(new ClientCredentialsAuthorizer(combinedClientCredentials));
        }

        internal Task<bool> EnsureAuthorizedAsync()
        {
            return Authorizer.EnsureAuthorizedAsync(this);
        }

        internal async Task<JObject> RequestAsync(string endpoint, HttpMethod method)
        {
            await EnsureAuthorizedAsync();
            return await InternalRequestAsync(new Uri("https://api.spotify.com/v1/" + endpoint), method).ConfigureAwait(false);
        }

        internal async Task<JObject> InternalRequestAsync(Uri requestUri, HttpMethod method)
        {
            var message = new HttpRequestMessage
            {
                Method = method,
                RequestUri = requestUri
            };

            var response = await HttpClient.SendAsync(message).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await Authorizer.HandleAuthenticationErrorAsync(new AuthorizationError((int) response.StatusCode, JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false)))).ConfigureAwait(false);
                    throw new InvalidOperationException("Cannot continue with request after authorization error.");
                }

                var err = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                var errorMessage = err["error"]["message"].ToObject<string>();

                throw new SpotifyException((int) response.StatusCode,  errorMessage);
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            response.Dispose();
            message.Dispose();

            return JObject.Parse(content);
        }

        /// <summary>
        ///     Gets an artist from the Spotify API with it's unique identifier.
        /// </summary>
        /// <param name="id">The base-62 Spotify ID to use when getting the artist.</param>
        /// <returns>An asynchronous operation that will yield the <see cref="SpotifyArtist"/> that has the provided ID.</returns>
        /// <example>
        ///     Here is an example of getting a Spotify artist.
        ///     <code>
        ///     var artist = await client.GetArtistAsync("6yhD1KjhLxIETFF7vIRf8B");
        ///     Console.WriteLine(artist.Name);
        ///     </code>
        /// </example>
        public async Task<SpotifyArtist> GetArtistAsync(string id)
        {
            var data = await RequestAsync("artists/" + id, HttpMethod.Get).ConfigureAwait(false);

            return new SpotifyArtist(this, data);
        }

        /// <summary>
        ///     Gets a bulk collection of artists from the Spotify API with their unique identifiers.
        /// </summary>
        /// <param name="artistIds">A list of base-62 Spotify IDs representing the artists to find.</param>
        /// <returns>
        ///     An asynchronous operation that will yield an immutable collection of <see cref="SpotifyArtist"/>s representing
        ///     the artists with the provided IDs.
        /// </returns>
        /// <remarks>
        ///     Inserting an invalid or unknown Spotify ID will result in that entry being <c>null</c> in the returning collection.
        ///     Inserting duplicate Spotify IDs will result in duplicate entries of that artist in the returning collection.
        /// </remarks>
        public async Task<ImmutableArray<SpotifyArtist>> GetArtistsAsync(IEnumerable<string> artistIds)
        {
            var l = artistIds.ToList();
            if (l.Count > 50) throw new ArgumentOutOfRangeException(nameof(artistIds), "SpotifyClient#GetArtistsAsync does not allow more than 50 IDs.");
            if (l.Count < 1) throw new ArgumentOutOfRangeException(nameof(artistIds), "SpotifyClient#GetArtistsAsync requires at least 1 ID.");
            var data = (await RequestAsync($"artists?ids={string.Join(",", l)}", HttpMethod.Get).ConfigureAwait(false))["artists"];
            return data.ToObject<IEnumerable<JObject>>().Select(a => new SpotifyArtist(this, a)).ToImmutableArray();
        }

        /// <summary>
        ///     Gets a collection of an artist's Related Artists from the Spotify API using the artist's unique identifier.
        /// </summary>
        /// <param name="artistId">The base-62 Spotify ID to use when finding related artists.</param>
        /// <returns>
        ///     An asynchronous operation that will yield an immutable collection of <see cref="SpotifyArtist"/> entities representing
        ///     the related artists.
        /// </returns>
        public async Task<ImmutableArray<SpotifyArtist>> GetRelatedArtistsAsync(string artistId)
        {
            var data = (await RequestAsync($"artists/{artistId}/related-artists", HttpMethod.Get).ConfigureAwait(false))["artists"];
            return data.ToObject<IEnumerable<JObject>>().Select(a => new SpotifyArtist(this, a)).ToImmutableArray();
        }

        /// <summary>
        ///     Gets an album from Spotify using its unique identifier.
        /// </summary>
        /// <param name="id">The album's unique identifier.</param>
        /// <returns>
        ///     An asynchronous operation that will yield a <see cref="SpotifyAlbum"/> representing the requested album.
        /// </returns>
        public async Task<SpotifyAlbum> GetAlbumAsync(string id)
        {
            var data = await RequestAsync($"albums/{id}", HttpMethod.Get).ConfigureAwait(false);
            return new SpotifyAlbum(this, data);
        }

        /// <summary>
        ///     Gets a bulk collection of albums from Spotify using their unique identifiers.
        /// </summary>
        /// <param name="albumIds">A list of base-62 album IDs representing the albums to find.</param>
        /// <returns>
        ///     An asynchronous operation that will yield an immutable collection of <see cref="SpotifyAlbum"/> entities representing
        ///     the related artists.
        /// </returns>
        public async Task<ImmutableArray<SpotifyAlbum>> GetAlbumsAsync(IEnumerable<string> albumIds)
        {
            var ids = albumIds.ToList();
            if (ids.Count < 1) throw new ArgumentOutOfRangeException(nameof(albumIds), "SpotifyClient#GetAlbumsAsync requires at least 1 album ID.");
            if (ids.Count > 50) throw new ArgumentOutOfRangeException(nameof(albumIds), "SpotifyClient#GetAlbumsAsync does not allow more than 50 album IDs.");
            var data = (await RequestAsync($"albums?ids={string.Join(",", ids)}", HttpMethod.Get).ConfigureAwait(false))["albums"];

            return data.ToObject<IEnumerable<JObject>>().Select(a => new SpotifyAlbum(this, a)).ToImmutableArray();
        }

        /// <summary>
        ///     Gets an album's tracks from Spotify using its unique identifier.
        /// </summary>
        /// <param name="albumId">The album's unique identifier.</param>
        /// <param name="limit">The maximum number of tracks to return. Minimum is 1, maximum is 50.</param>
        /// <param name="offset">The index of the first track to return.</param>
        /// <returns>
        ///     An asynchronous operaiton that will yield a paging container containing the requested tracks.
        /// </returns>
        public async Task<SpotifyPagingResponse<SpotifyTrackReference>> GetAlbumTracksAsync(string albumId, int limit = 20, int offset = 0)
        {
            if (limit > 50) throw new ArgumentOutOfRangeException(nameof(limit), "SpotifyClient#GetAlbumTracksAsync does not allow a limit over 50.");
            if (limit < 1) throw new ArgumentOutOfRangeException(nameof(limit), "SpotifyClient#GetAlbumTracksAsync requires that the limit be over 1.");

            var data = await RequestAsync($"albums/{albumId}/tracks?limit={limit}&offset={offset}", HttpMethod.Get).ConfigureAwait(false);

            return new SpotifyPagingResponse<SpotifyTrackReference>(data, (a, c) => new SpotifyTrackReference(this, a), d => d, this);
        }

        /// <summary>
        ///     Gets a track from the Spotify API with it's unique identifier.
        /// </summary>
        /// <param name="id">The base-62 Spotify ID to use when getting the track.</param>
        /// <returns>An asynchronous operation that will yield the <see cref="SpotifyTrack"/> that has the provided ID.</returns>
        /// <example>
        ///     Here is an example of getting a Spotify track.
        ///     <code>
        ///     var track = await client.GetTrackAsync("1S30kHvkkdMkcuCTGSgS41");
        ///     Console.WriteLine(track.Name);
        ///     </code>
        /// </example>
        public async Task<SpotifyTrack> GetTrackAsync(string id)
        {
            var data = await RequestAsync("tracks/" + id, HttpMethod.Get).ConfigureAwait(false);

            return new SpotifyTrack(data, this);
        }

        /// <summary>
        ///     Gets a bulk collection of tracks from the Spotify API with their unique identifiers.
        /// </summary>
        /// <param name="trackIds">A list of base-62 Spotify IDs representing the tracks to find.</param>
        /// <returns>
        ///     An asynchronous operation that will yield an immutable collection of <see cref="SpotifyTrack"/>s representing
        ///     the tracks with the provided IDs.
        /// </returns>
        /// <remarks>
        ///     Inserting an invalid or unknown Spotify ID will result in that entry being <c>null</c> in the returning collection.
        ///     Inserting duplicate Spotify IDs will result in duplicate entries of that track in the returning collection.
        /// </remarks>
        public async Task<ImmutableArray<SpotifyTrack>> GetTracksAsync(IEnumerable<string> trackIds)
        {
            var l = trackIds.ToList();
            if (l.Count > 50) throw new ArgumentOutOfRangeException(nameof(trackIds), "SpotifyClient#GetTracksAsync does not allow more than 50 IDs.");
            if (l.Count < 1) throw new ArgumentOutOfRangeException(nameof(trackIds), "SpotifyClient#GetTracksAsync requires at least 1 ID.");
            var data = (await RequestAsync($"tracks?ids={string.Join(",", l)}", HttpMethod.Get).ConfigureAwait(false))["tracks"];
            return data.ToObject<IEnumerable<JObject>>().Select(a => new SpotifyTrack(a, this)).ToImmutableArray();
        }

        /// <summary>
        ///     Searches Spotify's database for an entity using query parameters.
        /// </summary>
        /// <param name="query">The query to use. See https://developer.spotify.com/documentation/web-api/reference/search/search/#writing-a-query---guidelines for how to use this.</param>
        /// <param name="searchType">The type of entity to search for. You can combine types using enum flags.</param>
        /// <param name="limit">The maximum number of results to return, per type.</param>
        /// <param name="offset">The index of the first result to return. 0 = the first result.</param>
        /// <returns>An asynchronous operation representing the search response.</returns>
        /// <example>
        ///     Here is an example showing how to use this method to search for 3 albums and 3 tracks with the name "hello".
        ///     <code>
        ///     var client = SpotifyClient.FromClientCredentials("my id", "my secret");
        ///     var response = await SpotifyClient.SearchAsync("hello", SearchType.Track | SearchType.Album, 3);
        ///     var song = response.Tracks.Items.First();
        ///     </code>
        /// </example>
        public async Task<SpotifySearchResponse> SearchAsync(string query, SearchType searchType, int limit = 20, int offset = 0)
        {
            var data = await RequestAsync($"search?q={query.Replace(" ", "%20")}&type={(searchType == SearchType.All ? "album,artist,playlist,track" : searchType.ToString().ToLower())}&limit={limit}&offset={offset}", HttpMethod.Get);
            return new SpotifySearchResponse(data, this);
        }
    }
}