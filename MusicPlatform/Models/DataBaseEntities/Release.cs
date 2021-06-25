using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicPlatform.Models.DataBaseEntities
{
    public class Release
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int Listnings { get; set; }
        public DateTime ReleaseData { get; set; }
        public byte[] Cover { get; set; }

        public virtual List<Artist> Artists { get; set; } = new List<Artist>();
        public virtual List<User> UsersLikes { get; set; } = new List<User>();

        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
