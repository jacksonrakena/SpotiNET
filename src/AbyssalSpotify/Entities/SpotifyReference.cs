using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents a Spotify reference, which is a lightweight container that contains
    ///     less data than a complete object, but can be dereferenced into a complete object.
    /// </summary>
    /// <typeparam name="T">The full, detailed object type that this reference represents.</typeparam>
    public abstract class SpotifyReference<T> : SpotifyEntity
    {
        internal SpotifyReference(SpotifyClient client) : base(client)
        {
        }

        /// <summary>
        ///     Dereferences this reference, finding the full, detailed object.
        /// </summary>
        /// <returns>An asynchronous operation that will yield the full, detailed object that this reference represents.</returns>
        public abstract Task<T> GetFullEntityAsync();
    }
}