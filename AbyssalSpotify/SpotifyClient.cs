using AbyssalSpotify.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    public class SpotifyClient
    {
        public HttpClient HttpClient { get; }
        public ISpotifyAuthorizer Authorizer { get; }
        public AuthorizationSet AuthorizationSet { get; private set; }

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

        public static SpotifyClient FromClientCredentials(string combinedClientCredentials)
        {
            return new SpotifyClient(new ClientCredentialsAuthorizer(combinedClientCredentials));
        }

        /// <summary>
        ///     Forces an authorization or reauthorization. It is recommended to use <see cref="EnsureAuthorizedAsync"/> instead.
        /// </summary>
        /// <returns>An asynchronous operation that will yield the authorization result.</returns>
        public async Task<AuthorizationSet> AuthorizeAsync()
        {
            AuthorizationSet = await Authorizer.AuthorizeAsync(this);
            // TODO: Replace AuthorizationSet.AccessToken with a way to get the refresh token for AuthorizationCodeAuthorizer
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizationSet.AccessToken);

            return AuthorizationSet;
        }

        /// <summary>
        ///     Ensures that the Spotify client is properly authorized.
        ///     If the client is already authorized, nothing will happen and <code>false</code> will be returned.
        ///     If the client is not authorized, the authorization flow will be completed and <code>true</code> will be returned.
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
