using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify.Authorization
{
    public interface ISpotifyAuthorizer
    {
        Task<AuthorizationSet> AuthorizeAsync(SpotifyClient client);
    }
}
