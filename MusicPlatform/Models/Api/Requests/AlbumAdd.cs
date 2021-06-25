using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MusicPlatform.Models.Api.Responses;

namespace MusicPlatform.Models.Api.Requests
{
    public class AlbumAdd
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ArtistId { get; set; }
        public string Tracks { get; set; }
        public IFormFile Cover { get; set; }
    }
}
