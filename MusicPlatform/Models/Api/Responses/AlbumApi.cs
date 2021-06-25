using System.Collections.Generic;

namespace MusicPlatform.Models.Api.Responses
{
    public class AlbumApi : ReleaseApi
    {
        public string Description { get; set; }

        public IEnumerable<TrackApi> Tracks { get; set; }
    }
}
