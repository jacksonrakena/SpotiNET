using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;

namespace AbyssalSpotify
{
    internal interface ISpotifyAuthorizer
    {
        Task<bool> EnsureAuthorizedAsync(SpotifyClient client);

        Task HandleAuthenticationErrorAsync(AuthorizationError error);
    }
}
