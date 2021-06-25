namespace MusicPlatform.Models.DataBaseEntities
{
    public class Reference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }

        public int ArtistId;
        public Artist Artist;
    }
}
