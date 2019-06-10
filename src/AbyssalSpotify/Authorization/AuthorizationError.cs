using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssalSpotify
{
    /// <summary>
    ///     Represents an error that occurred during the authorization process.
    /// </summary>
    public sealed class AuthorizationError
    {
        /// <summary>
        ///     A high level description of the error as specified in RFC 6749 Section 5.2.
        /// </summary>
        public string Error { get; }

        /// <summary>
        ///     A more detailed description of the error as specified in RFC 6749 Section 4.1.2.1.
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     The HTTP status code returned.
        /// </summary>
        public int StatusCode { get; }

        internal AuthorizationError(int statusCode, JObject data)
        {
            StatusCode = statusCode;

            Error = data["error"].ToObject<string>();

            Description = data["error_description"].ToObject<string>();
        }
    }
}
