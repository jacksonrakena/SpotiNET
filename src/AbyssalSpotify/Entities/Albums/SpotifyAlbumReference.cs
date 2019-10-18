using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    /// <summary>
    ///     A light data class containing some album data, sometimes returned by the Spotify API
    ///     in lieu of a full <see cref="SpotifyAlbum"/> object.
    /// </summary>
    public class SpotifyAlbumReference : SpotifyReference<SpotifyAlbum>
    {
        // TODO: Album group

        /// <summary>
        ///     The type of this album.
        /// </summary>
        public AlbumType Type { get; }

        /// <summary>
        ///     A list of references to the artists who contributed to this album.
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
        ///     The name of the album. In case of an album takedown, the value may be an
        ///     empty string.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The date the album was first released. Depending on the precision available from Spotify,
        ///     this could be year only (i.e. 1981-01-01), year and month (i.e. 1981-06-01), or year/month/date (i.e. 1981-06-24).
        /// </summary>
        public DateTimeOffset ReleaseDate { get; }

        internal SpotifyAlbumReference(JObject data, SpotifyClient client) : base(client)
        {
            Type = data["album_type"].ToObject<AlbumType>();
            Artists = data["artists"].ToObject<IEnumerable<JObject>>().Select(a => new SpotifyArtistReference(client, a)).ToImmutableArray();
            AvailableMarkets = data["available_markets"].ToObject<ImmutableArray<string>>();
            ExternalUrls = new SpotifyExternalUrlsCollection(data["external_urls"].ToObject<IDictionary<string, string>>());
            Id = new SpotifyUri(data["uri"].ToObject<string>());
            Images = data["images"].ToObject<ImmutableArray<SpotifyImage>>();
            Name = data["name"].ToObject<string>();
            var rdp = data["release_date_precision"].ToObject<string>();
            var rd = data["release_date"].ToObject<string>().Split('-');
            ReleaseDate = rdp switch
            {
                "year" => new DateTimeOffset(int.Parse(rd[0]), 1, 1, 0, 0, 0, TimeSpan.Zero),

                "month" => new DateTimeOffset(int.Parse(rd[0]), int.Parse(rd[1]), 1, 0, 0, 0, TimeSpan.Zero),

                "day" => new DateTimeOffset(int.Parse(rd[0]), int.Parse(rd[1]), int.Parse(rd[2]), 0, 0, 0, TimeSpan.Zero),

                _ => throw new InvalidOperationException("The specified release date precision was not year, month, or day."),
            };
        }

        /// <inheritdoc />
        public override Task<SpotifyAlbum> GetFullEntityAsync() => Client.GetAlbumAsync(Id.Id);
    }
}