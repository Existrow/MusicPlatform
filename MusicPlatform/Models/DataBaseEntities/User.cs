using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicPlatform.Models.DataBaseEntities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public string Role { get; set; }

        public virtual List<Release> LikedReleases { get; set; } = new List<Release>();

        public int? ArtistId;
        public virtual Artist Artist { get; set; }
    }
}
