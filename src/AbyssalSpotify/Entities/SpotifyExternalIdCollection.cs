using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents a collection of external IDs for a Spotify entity.
    ///     Contains properties representing some known properties, but also allows
    ///     indexing through <see cref="IReadOnlyDictionary{TKey, TValue}"/>'s methods
    ///     for unknown properties.
    /// </summary>
    public class SpotifyExternalIdsCollection : IReadOnlyDictionary<string, string>
    {
        private readonly IDictionary<string, string> _data;

        public string this[string key] => _data[key];

        public IEnumerable<string> Keys => _data.Keys;

        public IEnumerable<string> Values => _data.Values;

        public int Count => _data.Count;

        public bool ContainsKey(string key) => _data.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _data.GetEnumerator();

        public bool TryGetValue(string key, out string value) => _data.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();

        internal SpotifyExternalIdsCollection(IDictionary<string, string> data)
        {
            _data = data;
        }

        /// <summary>
        ///     The International Standard Recording Code for this entity.
        /// </summary>
        public string InternationalStandardRecordingCode => TryGetValue("isrc", out var isrc) ? isrc : null;

        /// <summary>
        ///     The International Article Number for this entity.
        /// </summary>
        public string InternationalArticleNumber => TryGetValue("ean", out var ean) ? ean : null;

        /// <summary>
        ///     The Universal Product code for this entity.
        /// </summary>
        public string UniversalProductCode => TryGetValue("upc", out var upc) ? upc : null;
    }
}