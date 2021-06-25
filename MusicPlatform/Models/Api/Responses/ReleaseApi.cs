using System;
using System.Collections.Generic;
using MusicPlatform.Models.DataBaseEntities;

namespace MusicPlatform.Models.Api.Responses
{
    public class ReleaseApi
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Listnings { get; set; }
        public DateTime ReleaseData { get; set; }
        public string Cover { get; set; }
        public bool IsUserLiked { get; set; }
        public int Likes { get; set; }

        public IEnumerable<object> Authors { get; set; }

        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
