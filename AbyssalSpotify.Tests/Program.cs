using System;
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
            await client.EnsureAuthorizedAsync();

            Console.WriteLine(client.HttpClient.DefaultRequestHeaders.Authorization.ToString());

            var f = await client.EnsureAuthorizedAsync();
            Console.WriteLine(f);

            Console.ReadLine();
        }
    }
}
