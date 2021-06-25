using System.Collections.Generic;

namespace MusicPlatform.Models.Api.Responses
{
    public class ChartApi
    {
        public IEnumerable<TrackApi> Tracks { get; set; }
        public IEnumerable<AlbumApi> Albums { get; set; }
    }
}
