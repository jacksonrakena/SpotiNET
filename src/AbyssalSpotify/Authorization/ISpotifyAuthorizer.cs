using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    public interface ISpotifyAuthorizer
    {
        Task<AuthorizationSet> AuthorizeAsync(SpotifyClient client);
        AuthenticationHeaderValue GetAuthenticationHeaderValue(AuthorizationSet set);
    }
}
