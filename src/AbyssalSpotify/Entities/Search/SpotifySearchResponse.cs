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
        public ISpotifyPagingResponse<SpotifyAlbumReference> Albums { get; }

        /// <summary>
        ///     A list of tracks returned by this query.
        /// </summary>
        public ISpotifyPagingResponse<SpotifyTrack> Tracks { get; }

        /// <summary>
        ///     A list of artists returned by this query.
        /// </summary>
        public ISpotifyPagingResponse<SpotifyArtist> Artists { get; }

        public SpotifySearchResponse(JObject data, SpotifyClient client) : base(client)
        {
            var artists = data["artists"];
            if (artists != null)
            {
                Artists = new SpotifyArtistPagingResponse(client, artists);
            }
            else
            {
                Artists = new SpotifyArtistPagingResponse(client);
            }

            var albums = data["albums"];
            if (albums != null)
            {
                Albums = new SpotifyAlbumReferencePagingResponse(client, albums);
            }
            else
            {
                Albums = new SpotifyAlbumReferencePagingResponse(client);
            }

            var tracks = data["tracks"];
            if (tracks != null)
            {
                Tracks = new SpotifyTrackPagingResponse(client, tracks);
            }
            else
            {
                Tracks = new SpotifyTrackPagingResponse(client);
            }
        }
    }
}