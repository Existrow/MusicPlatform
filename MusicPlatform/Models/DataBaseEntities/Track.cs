using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlatform.Models.DataBaseEntities
{
    [Table("Tracks")]
    public class Track : Release
    {
        public string FilePath { get; set; }
        public string ClipUrl { get; set; }

        public virtual List<Artist> Featuring { get; set; } = new List<Artist>();
        public virtual List<Album> Albums { get; set; } = new List<Album>();
    }
}
