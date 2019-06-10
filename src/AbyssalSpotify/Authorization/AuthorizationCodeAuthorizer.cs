using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents the client authentication flow using an authorization code.
    ///     Not currently supported.
    /// </summary>
    public class AuthorizationCodeAuthorizer : ISpotifyAuthorizer
    {
        public Task<AuthorizationSet> AuthorizeAsync(SpotifyClient client)
        {
            throw new NotImplementedException();
        }

        public AuthenticationHeaderValue GetAuthenticationHeaderValue(AuthorizationSet set)
        {
            throw new NotImplementedException();
        }

        public Task HandleAuthenticationErrorAsync(AuthorizationError error)
        {
            throw new NotImplementedException();
        }
    }
}
