using System.Collections.Generic;
using MusicPlatform.Models.DataBaseEntities;

namespace MusicPlatform.Models.Api.Responses
{
    public class TrackApi : ReleaseApi
    {
        public string FilePath { get; set; }
        public string ClipUrl { get; set; }

        public IEnumerable<object> Featuring { get; set; } = new List<Artist>();
    }
}
