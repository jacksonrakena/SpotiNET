using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify.Authorization
{
    public enum AuthorizerType
    {
        /// <summary>
        ///     Allows the client to access user resources, and allows the client to refresh their access token.
        /// </summary>
        /// <remarks>
        ///     Suitable for long-running applications in which the user grants permission only once.
        ///     It provides an access token that can be refreshed.
        /// </remarks>
        AuthorizationCode,
        /// <summary>
        ///     Allows the client to access Spotify resources like tracks, artists, and albums.
        ///     Does not allow the client to access and user information.
        /// </summary>
        /// <remarks>
        ///     Suitable for applications that only wish to access Spotify data.
        /// </remarks>
        ClientCredentials
    }
}
