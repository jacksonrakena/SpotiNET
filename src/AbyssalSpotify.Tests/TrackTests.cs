using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssalSpotify.Tests
{
    [TestClass]
    [TestCategory("Track")]
    public class TrackTests
    {
        private static SpotifyClient _client;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _client = SpotifyClient.FromClientCredentials(Environment.GetEnvironmentVariable("SpotifyCredentials", EnvironmentVariableTarget.Machine));
        }

        [TestMethod]
        public async Task Test_Track_Fetch()
        {
            var track = await _client.GetTrackAsync("2WDffjRjjtxHcHCHkLpB5p");

            Assert.IsNotNull(track);
            Assert.AreEqual("2WDffjRjjtxHcHCHkLpB5p", track.Id.Id);
            Assert.AreEqual("Queen Of The Night", track.Name);

            Assert.AreEqual("Queen Of The Night", track.Album.Name);
            Assert.AreEqual("1BWJHMlxzPea13xJIgVxg0", track.Album.Id.Id);

            Assert.AreEqual("Hey Violet", track.Artists[0].Name);
            Assert.AreEqual("4JNfz6aO9ZFz0gp5GY88am", track.Artists[0].Id.Id);

            Assert.AreEqual(1, track.TrackNumber);
            Assert.AreEqual(TimeSpan.FromSeconds(197.497), track.Duration);

            Assert.IsTrue(track.HasExplicitLyrics);
        }

        [TestMethod]
        public async Task Test_BatchTracks_Fetch()
        {
            var tracks = await _client.GetTracksAsync(new string[] { "2WDffjRjjtxHcHCHkLpB5p", "20PISOo4VbLz4yFH78Zv5R" });

            Assert.IsNotNull(tracks);

            var track0 = tracks[0];

            Assert.IsNotNull(track0);
            Assert.AreEqual("2WDffjRjjtxHcHCHkLpB5p", track0.Id.Id);
            Assert.AreEqual("Queen Of The Night", track0.Name);

            Assert.AreEqual("Queen Of The Night", track0.Album.Name);
            Assert.AreEqual("1BWJHMlxzPea13xJIgVxg0", track0.Album.Id.Id);

            Assert.AreEqual("Hey Violet", track0.Artists[0].Name);
            Assert.AreEqual("4JNfz6aO9ZFz0gp5GY88am", track0.Artists[0].Id.Id);

            Assert.AreEqual(1, track0.TrackNumber);
            Assert.AreEqual(TimeSpan.FromSeconds(197.497), track0.Duration);

            Assert.IsTrue(track0.HasExplicitLyrics);

            var track1 = tracks[1];

            Assert.IsNotNull(track1);
            Assert.AreEqual("20PISOo4VbLz4yFH78Zv5R", track1.Id.Id);
            Assert.AreEqual("Swim", track1.Name);

            Assert.AreEqual("You Are Someone Else", track1.Album.Name);
            Assert.AreEqual("7LdUzhm4SloDil5y0sRen0", track1.Album.Id.Id);

            Assert.AreEqual("Fickle Friends", track1.Artists[0].Name);
            Assert.AreEqual("1nhSLEYdoBHG6cJ8NDwoF1", track1.Artists[0].Id.Id);

            Assert.AreEqual(3, track1.TrackNumber);
            Assert.AreEqual(TimeSpan.FromSeconds(197.506), track1.Duration);

            Assert.IsFalse(track1.HasExplicitLyrics);
        }

        [DataTestMethod]
        [DataRow(20, 0, DisplayName = "Default (Limit=20, Offset=0)")] // default
        [DataRow(40, 0, DisplayName = "Limit=40, Offset=0")]
        [DataRow(12, 0, DisplayName = "Limit=12, Offset=0")]
        public async Task Test_AlbumTracks_Fetch(int limit, int offset)
        {
            var albumTracks = await _client.GetAlbumTracksAsync("6ZZbMKUghvClcRLF5pZT6Y", limit, offset);

            Assert.IsNotNull(albumTracks);
            Assert.IsNotNull(albumTracks.Items);

            var trackNames = new [] 
            {
                "Break My Heart",
                "Brand New Moves",
                "Guys My Age",
                "Hoodie",
                "My Consequence",
                "O.D.D.",
                "All We Ever Wanted",
                "Fuqboi",
                "Unholy",
                "Where Have You Been (All My Night)",
                "Like Lovers Do",
                "This Is Me Breaking Up With You"
            };
            
            for (var i = 0; i < trackNames.Length; i++)
            {
                var trackName = trackNames[i];
                var track = albumTracks.Items[i];
                Assert.AreEqual(trackName, track.Name);
            }
        }

        [TestMethod]
        public Task Test_AlbumTracks_LimitOver50_Throws()
        {
            return Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => _client.GetAlbumTracksAsync("6ZZbMKUghvClcRLF5pZT6Y", 51));
        }

        [TestMethod]
        public Task Test_AlbumTracks_LimitUnder1_Throws()
        {
            return Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => _client.GetAlbumTracksAsync("6ZZbMKUghvClcRLF5pZT6Y", 0));
        }
    }
}
