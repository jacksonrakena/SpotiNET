using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify.Authorization
{
    /// <summary>
    ///     Represents the result of a Spotify authorization flow, using a <see cref="ISpotifyAuthorizer"/>.
    /// </summary>
    public class AuthorizationSet
    {
        /// <summary>
        ///     Represents the Spotify refresh token, used to extend the validity of the <see cref="AccessToken."/>
        ///     This will be <code>null</code> for every authorizer except <see cref="AuthorizationCodeAuthorizer"/>.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        ///     Represents the token that will be used to authorize Spotify requests.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        ///     Represents the type of <see cref="ISpotifyAuthorizer"/> that <see cref="Authorizer"/> is.
        /// </summary>
        public AuthorizerType AuthorizerType { get; set; }

        /// <summary>
        ///     Represents the <see cref="ISpotifyAuthorizer"/> that authorized this result.
        /// </summary>
        public ISpotifyAuthorizer Authorizer { get; set; }

        /// <summary>
        ///     Represents the <see cref="DateTimeOffset"/> at which the <see cref="AccessToken"/> will no longer be valid.
        /// </summary>
        public DateTimeOffset ExpirationTime { get; set; }
    }
}
