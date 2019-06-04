using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents an HTTP client that can be used to interface with Spotify's API.
    /// </summary>
    public class SpotifyClient
    {
        /// <summary>
        ///     The internal HTTP client to make requests with.
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        ///     The authorizer that handles authorizing and maintaining authorizations with Spotify's accounts service.
        /// </summary>
        public ISpotifyAuthorizer Authorizer { get; }

        /// <summary>
        ///     The current authorization data, as supplied by <see cref="Authorizer"/>. Will be <c>null</c> if no authorization has occurred yet.
        /// </summary>
        public AuthorizationSet AuthorizationSet { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="SpotifyClient"/> using the provided authorizer.
        /// </summary>
        /// <param name="authorizer"></param>
        public SpotifyClient(ISpotifyAuthorizer authorizer)
        {
            if (authorizer is AuthorizationCodeAuthorizer) throw new NotImplementedException("AuthorizationCodeAuthorizer is not currently implemented.");
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

        /// <summary>
        ///     Forces an authorization or reauthorization, regardless of the current client's authorization state.
        ///     It is recommended to use <see cref="EnsureAuthorizedAsync"/> instead.
        /// </summary>
        /// <returns>An asynchronous operation that will yield the authorization result.</returns>
        public async Task<AuthorizationSet> AuthorizeAsync()
        {
            AuthorizationSet = await Authorizer.AuthorizeAsync(this);
            HttpClient.DefaultRequestHeaders.Authorization = Authorizer.GetAuthenticationHeaderValue(AuthorizationSet);

            return AuthorizationSet;
        }

        /// <summary>
        ///     Ensures that the Spotify client is properly authorized.
        ///     If the client is already authorized, nothing will happen and <c>false</c> will be returned.
        ///     If the client is not authorized, the authorization flow will be completed and <c>true</c> will be returned.
        /// </summary>
        /// <remarks>
        ///     Consumers of <see cref="SpotifyClient"/> should not need to run this method, as it is run as part of all authorized
        ///     requests.
        /// </remarks>
        /// <returns>An asynchronous operation that will yield a boolean indicating whether the client attempted to authorize.</returns>
        public async Task<bool> EnsureAuthorizedAsync()
        {
            if (AuthorizationSet == null || AuthorizationSet.AccessToken == null || AuthorizationSet.AccessToken == string.Empty
                || AuthorizationSet.ExpirationTime.ToUnixTimeSeconds() < DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                await AuthorizeAsync();
                return true;
            }
            return false;
        }

        internal async Task<JObject> RequestAsync(string endpoint, HttpMethod method)
        {
            await EnsureAuthorizedAsync();
            var message = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri("https://api.spotify.com/v1/" + endpoint)
            };

            var response = await HttpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();

            return JObject.Parse(await response.Content.ReadAsStringAsync());
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
            var data = await RequestAsync("artists/" + id, HttpMethod.Get);

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
        public async Task<ImmutableList<SpotifyArtist>> GetArtistsAsync(IEnumerable<string> artistIds)
        {
            var l = artistIds.ToList();
            if (l.Count > 50) throw new ArgumentOutOfRangeException(nameof(artistIds), "SpotifyClient#GetArtistsAsync does not allow more than 50 IDs.");
            if (l.Count < 1) throw new ArgumentOutOfRangeException(nameof(artistIds), "SpotifyClient#GetArtistsAsync requires at least 1 ID.");
            var data = (await RequestAsync($"artists?ids={string.Join(",", l)}", HttpMethod.Get))["artists"];
            return data.ToObject<IEnumerable<JObject>>().Select(a => new SpotifyArtist(this, a)).ToImmutableList();
        }

        /// <summary>
        ///     Gets a collection of an artist's Related Artists from the Spotify API using the artist's unique identifier.
        /// </summary>
        /// <param name="artistId">The base-62 Spotify ID to use when finding related artists.</param>
        /// <returns>
        ///     An asynchronous operation that will yield an immutable collection of <see cref="SpotifyArtist"/> entities representing
        ///     the related artists.
        /// </returns>
        public async Task<ImmutableList<SpotifyArtist>> GetRelatedArtistsAsync(string artistId)
        {
            var data = (await RequestAsync($"artists/{artistId}/related-artists", HttpMethod.Get))["artists"];
            return data.ToObject<IEnumerable<JObject>>().Select(a => new SpotifyArtist(this, a)).ToImmutableList();
        }
    }
}