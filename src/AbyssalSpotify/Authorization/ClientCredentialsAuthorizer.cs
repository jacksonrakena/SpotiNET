using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents the client credentials authorization flow.
    ///     Allows the client to access Spotify resources like tracks, artists, and albums.
    ///     Does not allow the client to access and user information.
    /// </summary>
    internal class ClientCredentialsAuthorizer : ISpotifyAuthorizer
    {
        private static Uri ClientCredentialsAuthorizationEndpoint => new Uri("https://accounts.spotify.com/api/token");

        private readonly static FormUrlEncodedContent ClientCredentialsContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "client_credentials" }
        });

        public string CombinedClientCredentials { get; }

        private AuthorizationSet Authorization { get; set; }

        internal ClientCredentialsAuthorizer(string clientId, string clientSecret)
        {
            CombinedClientCredentials = $"{clientId}:{clientSecret}";
        }

        internal ClientCredentialsAuthorizer(string combinedClientCredentials)
        {
            CombinedClientCredentials = combinedClientCredentials;
        }

        public async Task<bool> EnsureAuthorizedAsync(SpotifyClient client)
        {
            if (Authorization == null || string.IsNullOrEmpty(Authorization.AccessToken)
                || Authorization.ExpirationTime.ToUnixTimeSeconds() < DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                var http = client.HttpClient;

                var request = new HttpRequestMessage
                {
                    RequestUri = ClientCredentialsAuthorizationEndpoint,
                    Content = ClientCredentialsContent,
                    Method = HttpMethod.Post
                };

                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Basic", EncodeBase64(CombinedClientCredentials));

                var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var jsonData = JObject.Parse(responseString);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await HandleAuthenticationErrorAsync(new AuthorizationError((int) response.StatusCode, jsonData)).ConfigureAwait(false);
                    throw new InvalidOperationException("Cannot continue with request after authorization error.");
                }

                response.EnsureSuccessStatusCode();

                Authorization = new AuthorizationSet
                {
                    AccessToken = jsonData["access_token"].ToObject<string>(),
                    ExpirationTime = DateTimeOffset.Now.AddSeconds(jsonData["expires_in"].ToObject<int>())
                };

                UpdateClientAuthorization(client);

                return true;
            }

            return false;
        }

        private void UpdateClientAuthorization(SpotifyClient client)
        {
            client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorization.AccessToken);
        }

        private string EncodeBase64(string enc)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(enc));
        }

        /// <summary>
        ///     Handles an error that occurred while authorizing with this authorizer.
        /// </summary>
        /// <param name="error">The error that occurred while authorizing with this authorizer.</param>
        /// <returns>An asynchronous operation representing the completion of this method.</returns>
        public Task HandleAuthenticationErrorAsync(AuthorizationError error)
        {
            throw new AuthenticationException("Error occurred during authorization: " + error.Error + ": " + error.Description);
        }

        private class AuthorizationSet
        {
            public string AccessToken { get; set; }

            public DateTimeOffset ExpirationTime { get; set; }
        }
    }
}