[![Discord](https://img.shields.io/discord/598437365891203072.svg?style=plastic)](https://discord.gg/RsRps9M)
# AbyssalSpotify
A Spotify API wrapper for .NET Standard 2.0, originally developed as a replacement for SpotifyAPI-NET for usage in [Abyss](http://github.com/abyssal512/Abyss). 
  
## Build Status
| Pipeline status | Latest stable | Latest pre-release |
|---------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------|-----------------------------------------------------------------------------------|
| [![Build Status](https://dev.azure.com/abyssal512/AbyssalSpotify/_apis/build/status/abyssal512.AbyssalSpotify?branchName=master)](https://dev.azure.com/abyssal512/AbyssalSpotify/_build?definitionId=1) | [![Nuget](https://img.shields.io/nuget/v/AbyssalSpotify.svg)](https://www.nuget.org/packages/AbyssalSpotify/) | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/AbyssalSpotify.svg)](https://www.nuget.org/packages/AbyssalSpotify/) |

## Usage
Documentation is a work-in-progress. You can see `AbyssalSpotify.Tests\Program.cs` for a brief example, 
but XML documentation is included with all public-facing classes and methods. You can also generate the documentation using DocFX and browse it locally. If you have any issues, feel free to open a GitHub issue, or hop in my Discord guild: [https://discord.gg/RsRps9M](https://discord.gg/RsRps9M).

## Feature Roadmap
* [x] Authentication
  * [x] Client Credentials authentication flow
  * [ ] Implicit Grant
  * [ ] Authorization Code
* [x] Tracks
  * [x] Track references
  * [x] Dereferencing
* [x] Albums
  * [x] Album references
  * [x] Dereferencing
* [x] Querying
  * [x] Mixed querying
  * [x] Selective querying
  * [x] Pagination querying
    * [ ] Use `IAsyncEnumerable<T>`

* [ ] Ratelimits (429 Too Many Requests and `Retry-After`)
* [ ] Playlists
* [ ] Player
* [ ] Personalization
* [ ] Categories
* [ ] Users
* [ ] Conditional requests (Cache-Control)

## Examples
AbyssalSpotify is used extensively in some of my other projects, namely [Abyss](https://github.com/abyssal512/Abyss), a Discord chatbot, under [SpotifyModule](https://github.com/abyssal512/Abyss/blob/master/Modules/SpotifyModule.cs), and [Program](https://github.com/abyssal512/Abyss/blob/master/Program.cs).
  
## Copyright  
Copyright (c) 2019 abyssal512.  
Spotify application and API is Copyright (c) 2019 Spotify Technology S.A.   
All media and content belongs to their respective owners.
