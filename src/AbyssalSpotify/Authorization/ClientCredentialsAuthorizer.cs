using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents the client credentials authorization flow.
    ///     Allows the client to access Spotify resources like tracks, artists, and albums.
    ///     Does not allow the client to access and user information.
    /// </summary>
    public class ClientCredentialsAuthorizer : ISpotifyAuthorizer
    {
        public static Uri ClientCredentialsAuthorizationEndpoint => new Uri("https://accounts.spotify.com/api/token");
        public static FormUrlEncodedContent ClientCredentialsContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "client_credentials" }
        });

        /// <summary>
        ///     Represents the combined client credentials to use when authorizing. The format is "[client id]:[client secret]".
        /// </summary>
        public string CombinedClientCredentials { get; }

        /// <summary>
        ///     Creates a new <see cref="ClientCredentialsAuthorizer"/> using a client ID and a client secret.
        /// </summary>
        /// <param name="clientId">The client ID to use.</param>
        /// <param name="clientSecret">The client secret to use.</param>
        public ClientCredentialsAuthorizer(string clientId, string clientSecret)
        {
            CombinedClientCredentials = $"{clientId}:{clientSecret}";
        }

        /// <summary>
        ///     Creates a new <see cref="ClientCredentialsAuthorizer"/> using a combined client ID and client secret in format "[client id]:[client secret]".
        /// </summary>
        /// <param name="combinedClientCredentials"></param>
        public ClientCredentialsAuthorizer(string combinedClientCredentials)
        {
            CombinedClientCredentials = combinedClientCredentials;
        }

        /// <summary>
        ///     Attempts to authorize with Spotify using the stored client credentials.
        /// </summary>
        /// <param name="client">The <see cref="SpotifyClient"/> that holds the <see cref="HttpClient"/> to use.</param>
        /// <returns>An asynchronous operation that will yield an <see cref="AuthorizationSet"/> to use for future requests.</returns>
        public async Task<AuthorizationSet> AuthorizeAsync(SpotifyClient client)
        {
            var http = client.HttpClient;

            var request = new HttpRequestMessage();

            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Base64Utilities.EncodeBase64(CombinedClientCredentials));

            request.RequestUri = ClientCredentialsAuthorizationEndpoint;
            request.Content = ClientCredentialsContent;
            request.Method = HttpMethod.Post;

            var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var authorizationResult = new AuthorizationSet();
            var jsonData = JObject.Parse(responseString);

            authorizationResult.AccessToken = jsonData["access_token"].ToObject<string>();
            authorizationResult.AuthorizerType = AuthorizerType.ClientCredentials;
            authorizationResult.ExpirationTime = DateTimeOffset.Now.AddSeconds(jsonData["expires_in"].ToObject<int>());
            authorizationResult.Authorizer = this;

            return authorizationResult;
        }

        /// <summary>
        ///     Builds an <see cref="AuthenticationHeaderValue"/> from an <see cref="AuthorizationSet"/> that has been created
        ///     with a <see cref="ClientCredentialsAuthorizer"/>.
        /// </summary>
        /// <param name="set"></param>
        /// <returns>An <see cref="AuthenticationHeaderValue"/> that can be used with Spotify requests.</returns>
        public AuthenticationHeaderValue GetAuthenticationHeaderValue(AuthorizationSet set)
        {
            return new AuthenticationHeaderValue("Bearer", set.AccessToken);
        }
    }
}
