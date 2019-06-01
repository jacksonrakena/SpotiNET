using System;
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
    }
}
