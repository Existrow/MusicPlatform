using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicPlatform.Models.DataBaseEntities;

namespace MusicPlatform.Services
{
    public class DataCreator
    {
        private readonly PlatformContext _context;

        public DataCreator(PlatformContext context)
            => _context = context;

        public void Create()
        {
            _context.Users.Add(new() { Login = "user", Password = "Password", Role="User" });
            _context.Genres.Add(new() { Name = "Drill" });
            _context.Artists.Add(new() { Name = "SVAGG", Description="SHu" });
            _context.Artists.Add(new() { Name = "GOO", Description = "Buf" });
            _context.SaveChanges();

            _context.Tracks.Add(new()
            {
                GenreId = _context.Genres.FirstOrDefault().Id,
                Title = "RUSDRILL",
                Genre = _context.Genres.FirstOrDefault(),
                Artists = new() { _context.Artists.FirstOrDefault() },
                FilePath = "/static/sk.mp3",
                ReleaseData = DateTime.UtcNow,
                //Featuring = new() { _context.Artists.FirstOrDefault(a => a.Name == "GOO") }
            });

            _context.SaveChanges();
        }
    }
}
