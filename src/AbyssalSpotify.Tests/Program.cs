﻿using System;
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

            //var artists = await client.GetRelatedArtistsAsync("20cozXBkQdesuJYvtvZ2vH");

            //foreach (var artist in artists)
            //{
            //    PrintArtist(artist);
            //}

            var albums = await client.GetTracksAsync(new[] { "1S30kHvkkdMkcuCTGSgS41", "7l9uOhfVJC8Y0c6PXHrgbs", "5hi8rVr3NZvIEjlk8Ts8Tx" });

            foreach (var track in albums) PrintTrack(track);

            Console.ReadKey();
        }

        private static void PrintTrack(SpotifyTrack track)
        {
            Console.WriteLine("Name: " + track.Name);
            Console.WriteLine("Artists: " + string.Join(", ", track.Artists.Select(a => a.Name)));
            Console.WriteLine("Album: " + track.Album.Name);
            Console.WriteLine("Id: " + track.Id);
        }

        private static void PrintTrackRef(SpotifyTrackReference track)
        {
            Console.WriteLine(track.Name);
            Console.WriteLine(string.Join(", ", track.Artists.Select(a => a.Name)));
        }

        private static void PrintArtist(SpotifyArtist atc)
        {
            Console.WriteLine(atc.Name);
            Console.WriteLine(atc.FollowerCount + " followers");
            Console.WriteLine("Genres: " + string.Join(", ", atc.AssociatedGenres));
            Console.WriteLine(string.Join(", ", atc.ExternalUrls.Select(a => $"{a.Key}: {a.Value}")));
            Console.WriteLine("Images: " + string.Join(", ", atc.Images.Select(a => a.Url)));
            Console.WriteLine("Popularity %: " + atc.Popularity);
        }

        private static void PrintAlbum(SpotifyAlbum album)
        {
            Console.WriteLine(album.Name);
            Console.WriteLine("ID: " + album.Id.Id);
            Console.WriteLine("Popularity %: " + album.Popularity);
            Console.WriteLine("Label: " + album.Label);
            Console.WriteLine("Tracks: " + string.Join(", ", album.Tracks.Items.Select(a => a.Name)));
            Console.WriteLine("Tracks Limit: " + album.Tracks.Limit);
            Console.WriteLine("Tracks Total: " + album.Tracks.Total);
            Console.WriteLine("Artists: " + string.Join(", ", album.Artists.Select(a => a.Name)));
            Console.WriteLine("Release Date: " + album.ReleaseDate.ToString());
            Console.WriteLine("Type: " + album.Type.ToString());
            Console.WriteLine("Copyrights: " + string.Join(", ", album.Copyrights.Select(a => $"{a.CopyrightType.ToString("G")} {a.CopyrightText}")));
            Console.WriteLine("External IDs: " + string.Join(", ", album.ExternalIds.Select(a => $"{a.Key}: {a.Value}")));
            Console.WriteLine("External URLs: " + string.Join(", ", album.ExternalUrls.Select(a => $"{a.Key}: {a.Value}")));
            Console.WriteLine("Images: " + string.Join(", ", album.Images.Select(a => a.Url)));
            Console.WriteLine("Type: " + album.Type.ToString("G"));
        }
    }
}