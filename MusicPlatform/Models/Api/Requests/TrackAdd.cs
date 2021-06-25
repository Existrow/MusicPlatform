using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MusicPlatform.Models.Api.Requests
{
    public class TrackAdd
    {
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public IFormFile Cover { get; set; }
        public IFormFile File { get; set; }
    }
}
