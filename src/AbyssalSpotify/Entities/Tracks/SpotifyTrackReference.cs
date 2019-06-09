using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify
{
    public class SpotifyTrackReference : SpotifyReference<SpotifyTrack>
    {
        /// <summary>
        ///     A list of references to the artists who performed the track.
        /// </summary>
        public ImmutableArray<SpotifyArtistReference> Artists { get; }

        /// <summary>
        ///     A list of ISO 3166-1 alpha-2 country codes, representing markets in which this track can be played.
        /// </summary>
        public ImmutableArray<string> AvailableMarkets { get; }

        /// <summary>
        ///     The disc number, usually <c>1</c> unless the album consists of more than one disc.
        /// </summary>
        public int DiscNumber { get; }

        /// <summary>
        ///     The duration of the track.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        ///     Whether or not this track has explicit lyrics. Note that <c>false</c> means the track doesn't have explicit lyrics OR it is unknown.
        /// </summary>
        public bool HasExplicitLyrics { get; }

        /// <summary>
        ///     A collection of known external URLs for this track.
        /// </summary>
        public SpotifyExternalUrlsCollection ExternalUrls { get; }

        /// <summary>
        ///     The Spotify ID data for this track.
        /// </summary>
        public SpotifyUri Id { get; }

        // TODO: Track Relinking (is_playable, linked_from, restrictions)

        /// <summary>
        ///     The name of this track.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     A URL to a 30-second preview (in MP3 format) of the track.
        /// </summary>
        public string PreviewUrl { get; }

        /// <summary>
        ///     The number of the track on this disc. If the album has several discs,
        ///     the track number is the number on the specified disc.
        /// </summary>
        public int TrackNumber { get; }

        /// <summary>
        ///     Whether or not the track is from a Spotify local file.
        /// </summary>
        public bool IsLocalTrack { get; }

        internal SpotifyTrackReference(SpotifyClient client, JObject data) : base(client)
        {
            Artists = data["artists"].ToObject<IEnumerable<JObject>>().Select(a => new SpotifyArtistReference(client, a)).ToImmutableArray();
            AvailableMarkets = data["available_markets"].ToObject<ImmutableArray<string>>();
            DiscNumber = data["disc_number"].ToObject<int>();
            Duration = TimeSpan.FromMilliseconds(data["duration_ms"].ToObject<int>());
            HasExplicitLyrics = data["explicit"].ToObject<bool>();
            ExternalUrls = new SpotifyExternalUrlsCollection(data["external_urls"].ToObject<IDictionary<string, string>>());
            Id = new SpotifyUri(data["uri"].ToObject<string>());
            // TODO: IS_PLAYABLE, LINKED_FROM, RESTRICTIONS
            Name = data["name"].ToObject<string>();
            PreviewUrl = data["preview_url"].ToObject<string>();
            TrackNumber = data["track_number"].ToObject<int>();
            IsLocalTrack = data["is_local"].ToObject<bool>();
        }

        /// <summary>
        ///     Downloads the full <see cref="SpotifyTrack"/> that this <see cref="SpotifyTrackReference"/> represents.
        /// </summary>
        /// <returns>
        ///     An asynchronous operation representing the <see cref="SpotifyTrack"/> that this <see cref="SpotifyTrackReference"/> represents.
        /// </returns>
        public override Task<SpotifyTrack> GetFullEntityAsync() => Client.GetTrackAsync(Id.Id);
    }
}