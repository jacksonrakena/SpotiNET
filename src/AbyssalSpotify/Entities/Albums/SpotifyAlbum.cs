using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents an album entity returned by Spotify.
    /// </summary>
    public class SpotifyAlbum : SpotifyEntity
    {
        /// <summary>
        ///     The type of this album.
        /// </summary>
        public AlbumType Type { get; }

        /// <summary>
        ///     The artists that contributed to this album.
        /// </summary>
        public ImmutableArray<SpotifyArtistReference> Artists { get; }

        /// <summary>
        ///     A list of ISO 3166-1 alpha-2 country codes, representing markets in which this album is available.
        /// </summary>
        /// <remarks>
        ///     An "available market" is a market where at least one of the tracks on this album is available.
        /// </remarks>
        public ImmutableArray<string> AvailableMarkets { get; }

        /// <summary>
        ///     A list of copyright statements held for this album.
        /// </summary>
        public ImmutableArray<SpotifyAlbumCopyright> Copyrights { get; }

        /// <summary>
        ///     A collection of known external IDs for this album.
        /// </summary>
        public SpotifyExternalIdsCollection ExternalIds { get; }

        /// <summary>
        ///     A collection of known external URLs for this album.
        /// </summary>
        public SpotifyExternalUrlsCollection ExternalUrls { get; }

        /// <summary>
        ///     The Spotify ID data for this album.
        /// </summary>
        public SpotifyUri Id { get; }

        /// <summary>
        ///     Cover art for this album, in various sizes.
        /// </summary>
        public ImmutableArray<SpotifyImage> Images { get; }

        /// <summary>
        ///     The recording/production label for this album.
        /// </summary>
        public string Label { get; }

        /// <summary>
        ///     The name of the album. In case of an album takedown, the value may be an
        ///     empty string.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The popularity of the album, between 0 and 100, with 100 being the most popular.
        /// </summary>
        public int Popularity { get; }

        /// <summary>
        ///     The date the album was first released. Depending on the precision available from Spotify,
        ///     this could be year only (i.e. 1981-01-01), year and month (i.e. 1981-06-01), or year/month/date (i.e. 1981-06-24).
        /// </summary>
        public DateTimeOffset ReleaseDate { get; }

        // TODO: Track Relinking

        /// <summary>
        ///     A paging object that contains references to the tracks in this album.
        /// </summary>
        public SpotifyPagingResponse<SpotifyTrackReference> Tracks { get; }

        internal SpotifyAlbum(SpotifyClient client, JObject data) : base(client)
        {
            Type = data["album_type"].ToObject<AlbumType>();
            Artists = data["artists"].ToObject<IEnumerable<JObject>>().Select(a => new SpotifyArtistReference(client, a)).ToImmutableArray();
            AvailableMarkets = data["available_markets"].ToObject<ImmutableArray<string>>();
            Copyrights = data["copyrights"]
                .ToObject<IEnumerable<JObject>>()
                .Select(a =>
                new SpotifyAlbumCopyright(a["text"].ToObject<string>(), a["type"].ToObject<string>() == "C" ? AlbumCopyrightType.Copyright : AlbumCopyrightType.PerformanceCopyright))
                .ToImmutableArray();
            ExternalIds = new SpotifyExternalIdsCollection(data["external_ids"].ToObject<IDictionary<string, string>>());
            ExternalUrls = new SpotifyExternalUrlsCollection(data["external_urls"].ToObject<IDictionary<string, string>>());
            Id = new SpotifyUri(data["uri"].ToObject<string>());
            Images = data["images"].ToObject<ImmutableArray<SpotifyImage>>();
            Label = data["label"].ToObject<string>();
            Name = data["name"].ToObject<string>();
            Popularity = data["popularity"].ToObject<int>();

            var rdp = data["release_date_precision"].ToObject<string>();
            var rd = data["release_date"].ToObject<string>().Split('-');
            switch (rdp)
            {
                case "year":
                    ReleaseDate = new DateTimeOffset(int.Parse(rd[0]), 1, 1, 0, 0, 0, TimeSpan.Zero);
                    break;

                case "month":
                    ReleaseDate = new DateTimeOffset(int.Parse(rd[0]), int.Parse(rd[1]), 1, 0, 0, 0, TimeSpan.Zero);
                    break;

                case "day":
                    ReleaseDate = new DateTimeOffset(int.Parse(rd[0]), int.Parse(rd[1]), int.Parse(rd[2]), 0, 0, 0, TimeSpan.Zero);
                    break;

                default:
                    throw new InvalidOperationException("The specified release date precision was not year, month, or day.");
            }

            // TODO: Track Relinking

            Tracks = new SpotifyPagingResponse<SpotifyTrackReference>((JObject) data["tracks"], (o, c) => new SpotifyTrackReference(c, o), a => (JObject) a["tracks"], client);
        }
    }
}