using System.Collections.Generic;
using MusicPlatform.Models.DataBaseEntities;

namespace MusicPlatform.Models.Api.Responses
{
    public class ArtistResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }

        public IEnumerable<TrackApi> Tracks { get; set; }
        public IEnumerable<AlbumApi> Albums { get; set; }
        public IEnumerable<Reference> References { get; set; }
    }
}
