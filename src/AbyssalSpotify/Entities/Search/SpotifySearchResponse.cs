using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents a search response from a Spotify query.
    /// </summary>
    public class SpotifySearchResponse : SpotifyEntity
    {
        /// <summary>
        ///     A list of references to albums returned by this query.
        /// </summary>
        public SpotifyPagingResponse<SpotifyAlbumReference> Albums { get; }

        /// <summary>
        ///     A list of tracks returned by this query.
        /// </summary>
        public SpotifyPagingResponse<SpotifyTrack> Tracks { get; }

        /// <summary>
        ///     A list of artists returned by this query.
        /// </summary>
        public SpotifyPagingResponse<SpotifyArtist> Artists { get; }

        internal SpotifySearchResponse(JObject data, SpotifyClient client) : base(client)
        {
            var artists = data["artists"];
            if (artists != null)
            {
                Artists = new SpotifyPagingResponse<SpotifyArtist>((JObject) artists, (o, c) => new SpotifyArtist(c, o), j => (JObject) j["artists"], client);
            }
            else
            {
                Artists = new SpotifyPagingResponse<SpotifyArtist>();
            }

            var albums = data["albums"];
            if (albums != null)
            {
                Albums = new SpotifyPagingResponse<SpotifyAlbumReference>((JObject) albums, (o, c) => new SpotifyAlbumReference(o, c), j => (JObject) j["albums"], client);
            }
            else
            {
                Albums = new SpotifyPagingResponse<SpotifyAlbumReference>();
            }

            var tracks = data["tracks"];
            if (tracks != null)
            {
                Tracks = new SpotifyPagingResponse<SpotifyTrack>((JObject) tracks, (o, c) => new SpotifyTrack(o, c), j => (JObject) j["tracks"], client);
            }
            else
            {
                Tracks = new SpotifyPagingResponse<SpotifyTrack>();
            }
        }
    }
}