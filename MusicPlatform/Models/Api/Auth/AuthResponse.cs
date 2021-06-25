using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlatform.Models.Api.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string Id { get; set; }
    }
}
