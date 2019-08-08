using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AbyssalSpotify.Tests
{
    [TestClass]
    [TestCategory("Artist")]
    public class ArtistTests
    {
        private static SpotifyClient _client;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _client = SpotifyClient.FromClientCredentials(Environment.GetEnvironmentVariable("SpotifyCredentials", EnvironmentVariableTarget.Machine));
        }

        [TestMethod]
        public async Task Test_Artist_Fetch()
        {
            var artist = await _client.GetArtistAsync("4JNfz6aO9ZFz0gp5GY88am");

            Assert.IsNotNull(artist);
            Assert.AreEqual("Hey Violet", artist.Name);
            Assert.AreEqual("spotify:artist:4JNfz6aO9ZFz0gp5GY88am", artist.Uri);
            Assert.IsNotNull(artist.Images);
            Assert.IsNotNull(artist.AssociatedGenres);
        }

        [TestMethod]
        public async Task Test_RelatedArtists_Fetch()
        {
            var relatedArtists = await _client.GetRelatedArtistsAsync("4JNfz6aO9ZFz0gp5GY88am");

            Assert.IsNotNull(relatedArtists);
        }

        [TestMethod]
        public async Task Test_BatchArtists_Fetch()
        {
            var bulkArtists = await _client.GetArtistsAsync(new string[]
                {
                    "1nhSLEYdoBHG6cJ8NDwoF1",
                    "4JNfz6aO9ZFz0gp5GY88am"
                });

            Assert.IsNotNull(bulkArtists);

            var artist0 = bulkArtists.FirstOrDefault();
            Assert.IsNotNull(artist0);
            Assert.AreEqual(artist0.Name, "Fickle Friends");

            var artist1 = bulkArtists.ElementAt(1);
            Assert.IsNotNull(artist1);
            Assert.AreEqual(artist1.Name, "Hey Violet");
        }
    }
}
