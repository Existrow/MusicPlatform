using System.Collections.Generic;
using MusicPlatform.Models.DataBaseEntities;

namespace MusicPlatform.Models.Implementations
{
    public class Chart
    {
        public List<Track> Tracks { get; set; }
        public List<Album> Albums { get; set; }
    }
}
