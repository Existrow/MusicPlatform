using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlatform.Models.Api.Auth
{
    public class AuthRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
