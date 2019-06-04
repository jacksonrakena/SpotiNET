using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    public abstract class SpotifyEntity
    {
        internal SpotifyClient Client;

        internal SpotifyEntity(SpotifyClient client)
        {
            Client = client;
        }
    }
}