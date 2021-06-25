using System.Collections.Generic;

namespace MusicPlatform.Models.DataBaseEntities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Release> Releases { get; set; } = new List<Release>();
    }
}
