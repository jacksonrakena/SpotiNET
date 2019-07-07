# AbyssalSpotify
A Spotify API wrapper for .NET Standard 2.0.  
  
## Build Status
| Board status | Pipeline status | Latest stable | Latest pre-release |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------|-----------------------------------------------------------------------------------|
| [![Board Status](https://dev.azure.com/abyssal512/5752da90-1d92-4bc4-affc-9f915c72dd99/6116e7f2-5e4a-438b-b62a-5c91b324fd49/_apis/work/boardbadge/b68e9b92-384a-4282-9484-284b2b2b198b)](https://dev.azure.com/abyssal512/5752da90-1d92-4bc4-affc-9f915c72dd99/_boards/board/t/6116e7f2-5e4a-438b-b62a-5c91b324fd49/Microsoft.RequirementCategory) | [![Build Status](https://dev.azure.com/abyssal512/AbyssalSpotify/_apis/build/status/abyssal512.AbyssalSpotify?branchName=master)](https://dev.azure.com/abyssal512/AbyssalSpotify/_build?definitionId=1) | [![Nuget](https://img.shields.io/nuget/v/AbyssalSpotify.svg)](https://www.nuget.org/packages/AbyssalSpotify/) | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/AbyssalSpotify.svg)](https://www.nuget.org/packages/AbyssalSpotify/) |

## Usage
Documentation is a work-in-progress. You can see `AbyssalSpotify.Tests\Program.cs` for a brief example, 
but XML documentation is included with all public-facing classes and methods. You can also generate the documentation using DocFX and browse it locally.

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
