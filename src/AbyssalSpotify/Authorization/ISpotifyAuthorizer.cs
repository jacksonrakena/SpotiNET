using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents the interface for a class that can negotiate the authentication flow with Spotify and authorize
    ///     the client to access data.
    /// </summary>
    public interface ISpotifyAuthorizer
    {
        /// <summary>
        ///     Initiates and completes the Spotify authentication flow, using the <see cref="System.Net.Http.HttpClient"/>
        ///     from the provided <see cref="SpotifyClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="SpotifyClient"/> which contains the <see cref="System.Net.Http.HttpClient"/> to use.</param>
        /// <returns>An asynchronous operation that will yield authentication data for the client to use.</returns>
        Task<AuthorizationSet> AuthorizeAsync(SpotifyClient client);

        /// <summary>
        ///     Converts a provided <see cref="AuthorizationSet"/> into an <see cref="AuthenticationHeaderValue"/> for use
        ///     when authenticating requests.
        /// </summary>
        /// <param name="set">The <see cref="AuthorizationSet"/> to convert.</param>
        /// <returns>An <see cref="AuthenticationHeaderValue"/> to use when authenticating requests.</returns>
        AuthenticationHeaderValue GetAuthenticationHeaderValue(AuthorizationSet set);

        /// <summary>
        ///     Handles an error that occurred while authorizing with this authorizer.
        /// </summary>
        /// <param name="error">The error that occurred while authorizing with this authorizer.</param>
        /// <returns>An asynchronous operation representing the completion of this method.</returns>
        Task HandleAuthenticationErrorAsync(AuthorizationError error);
    }
}
