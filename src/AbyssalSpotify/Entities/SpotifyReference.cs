using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    public abstract class SpotifyReference<T> : SpotifyEntity
    {
        public SpotifyReference(SpotifyClient client) : base(client)
        {
        }

        public abstract Task<T> GetFullEntityAsync();
    }
}