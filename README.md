<h1 align="center">SpotiNET 🎶</h1>
A Spotify API wrapper for .NET Standard 2.1.  
  
Currently, there are no stable releases, although the latest pre-release is fairly stable and should work in your production builds. When updating, always check the commit notes. 

### Usage
Documentation is a work-in-progress. You can see `AbyssalSpotify.Tests\Program.cs` for a brief example, 
but XML documentation is included with all public-facing classes and methods. You can also generate the documentation using DocFX and browse it locally. If you have any issues, feel free to open a GitHub issue.

### Feature Roadmap
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


### Copyright  
Copyright (c) 2020 Abyssal.  
All media and content belongs to their respective owners.  
