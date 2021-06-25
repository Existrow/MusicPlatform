using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlatform.Models.DataBaseEntities
{
    [Table("Albums")]
    public class Album : Release
    {
        public string Description { get; set; }

        public virtual List<Track> Tracks { get; set; } = new List<Track>();
    }
}
