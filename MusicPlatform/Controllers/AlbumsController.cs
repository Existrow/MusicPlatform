using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Models.Api.Requests;
using MusicPlatform.Models.Api.Responses;
using MusicPlatform.Models.DataBaseEntities;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly PlatformContext _context;
        private readonly IImageFileConverter _imageConverter;
        private readonly IReleaseToApiConverter _releaseConverter;
        private readonly IFileSaver _fileSaver;

        public AlbumsController(PlatformContext context, IImageFileConverter imageConverter, IReleaseToApiConverter releaseConverter, IFileSaver saver)
            => (_context, _imageConverter, _releaseConverter, _fileSaver) = (context, imageConverter, releaseConverter, saver);

        // GET: api/Albums
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AlbumApi>>> GetAlbums()
        {
            var user = GetUser();
            return await _context.Albums
                .Include(a => a.Tracks)
                .Include(a => a.Artists)
                .Include(a => a.Genre)
                .Select(a => _releaseConverter.Convert(a, user.Id))
                .ToListAsync();
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AlbumApi>> GetAlbum(int id)
        {
            var user = GetUser();
            var album = await _context.Albums
                .Include(a => a.Tracks)
                .Include(a => a.Artists)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
            {
                return NotFound();
            }

            return _releaseConverter.Convert(album, user.Id);
        }

        // PUT: api/Albums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAlbum(int id, [FromForm]AlbumAdd albumRequest)
        {
            var trr = albumRequest.Tracks.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e));
            var album = _context.Albums.Find(id);
            album.Title = albumRequest.Title;
            album.Cover = albumRequest.Cover is not null
                ? _imageConverter.ConvertToByte(albumRequest.Cover)
                : null;
            album.Tracks = _context.Tracks.Where(t => trr.Contains(t.Id)).ToList();
            album.Description = albumRequest.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Albums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AlbumApi>> PostAlbum([FromForm]AlbumAdd albumRequest)
        {
            var trr = albumRequest.Tracks.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e));
            var artist = _context.Artists.Find(albumRequest.ArtistId);
            var album = new Album
            {
                Title = albumRequest.Title,
                Artists = new List<Artist> { artist },
                ReleaseData = DateTime.Now,
                Cover = albumRequest.Cover is not null
                    ? _imageConverter.ConvertToByte(albumRequest.Cover)
                    : null,
                Tracks = _context.Tracks.Where(t => trr.Contains(t.Id)).ToList(),
                Genre = _context.Genres.First(),
                Description = albumRequest.Description
            };

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlbum", new { id = album.Id }, album);
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }

        private User GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var user = _context.Users.Include(u => u.Artist).FirstOrDefault((user) => user.Login == userName);
            return user;
        }
    }
}
