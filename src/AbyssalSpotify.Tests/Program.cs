using System;
using System.Linq;
using System.Threading.Tasks;
using AbyssalSpotify;

namespace AbyssalSpotify.Tests
{
    public class Program
    {
        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            var client = SpotifyClient.FromClientCredentials(Environment.GetEnvironmentVariable("SpotifyCredentials", EnvironmentVariableTarget.Machine));

            var atc = await client.GetArtistAsync("6yhD1KjhLxIETFF7vIRf8B");

            Console.WriteLine(atc.Name);
            Console.WriteLine(atc.FollowerCount + " followers");
            Console.WriteLine("Genres: " + string.Join(", ", atc.AssociatedGenres));
            Console.WriteLine(string.Join(", ", atc.ExternalUrls.Select(a => $"{a.Key}: {a.Value}")));
            Console.WriteLine("Images: " + string.Join(", ", atc.Images.Select(a => a.Url)));
            Console.WriteLine("Popularity %: " + atc.Popularity);

            Console.ReadKey();
        }
    }
}
