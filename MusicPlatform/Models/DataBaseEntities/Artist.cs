using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicPlatform.Models.DataBaseEntities
{
    public class Artist
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Cover { get; set; }

        public virtual List<Release> Releases { get; set; } = new List<Release>();
        public virtual List<Track> FeaturingTracks { get; set; } = new List<Track>();
        public virtual List<Reference> References { get; set; } = new List<Reference>();
    }
}
