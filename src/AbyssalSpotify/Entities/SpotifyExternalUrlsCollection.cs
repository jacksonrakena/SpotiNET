using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents a collection of external URLs for a Spotify entity.
    ///     Contains properties representing some known properties, but also allows
    ///     indexing through <see cref="IReadOnlyDictionary{TKey, TValue}"/>'s methods
    ///     for unknown properties.
    /// </summary>
    public class SpotifyExternalUrlsCollection : IReadOnlyDictionary<string, string>
    {
        private readonly IDictionary<string, string> _data;

        /// <inheritdoc />
        public string this[string key] => _data[key];

        /// <inheritdoc />
        public IEnumerable<string> Keys => _data.Keys;

        /// <inheritdoc />
        public IEnumerable<string> Values => _data.Values;

        /// <inheritdoc />
        public int Count => _data.Count;

        /// <inheritdoc />
        public bool ContainsKey(string key) => _data.ContainsKey(key);

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _data.GetEnumerator();

        /// <inheritdoc />
        public bool TryGetValue(string key, out string value) => _data.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();

        internal SpotifyExternalUrlsCollection(IDictionary<string, string> data)
        {
            _data = data;
        }

        /// <summary>
        ///     The Wikipedia link. Will be <c>null</c> if unknown.
        /// </summary>
        public string Wikipedia => TryGetValue("wikipedia", out string d) ? d : null;

        /// <summary>
        ///     The Facebook link. Will be <c>null</c> if unknown.
        /// </summary>
        public string Facebook => TryGetValue("facebook", out string fb) ? fb : null;

        /// <summary>
        ///     The Twitter link. Will be <c>null</c> if unknown.
        /// </summary>
        public string Twitter => TryGetValue("twitter", out string tw) ? tw : null;

        /// <summary>
        ///     The Instagram link. Will be <c>null</c> if unknown.
        /// </summary>
        public string Instagram => TryGetValue("instagram", out string ig) ? ig : null;
    }
}