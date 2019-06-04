using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     An offset-based paging object, used as a container for a set of Spotify objects.
    /// </summary>
    /// <typeparam name="T">The type of the objects inside this container.</typeparam>
    public interface ISpotifyPagingResponse<T>
    {
        /// <summary>
        ///     The requested items.
        /// </summary>
        ImmutableList<T> Items { get; }

        /// <summary>
        ///     The maximum number of items in the response (<see cref="Items"/>).
        /// </summary>
        int Limit { get; }

        /// <summary>
        ///     The maximum number of items available to return.
        /// </summary>
        int Total { get; }
    }
}