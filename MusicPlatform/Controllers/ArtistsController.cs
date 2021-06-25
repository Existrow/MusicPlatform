using System;
using System.Collections.Generic;
using System.IO;
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
    public class ArtistsController : ControllerBase
    {
        private readonly PlatformContext _context;
        private readonly IImageFileConverter _imageConverter;
        private readonly IReleaseToApiConverter _releaseConverter;

        public ArtistsController(PlatformContext context, IImageFileConverter imageConverter, IReleaseToApiConverter releaseConverter)
            => (_context, _imageConverter, _releaseConverter) = (context, imageConverter, releaseConverter);

        // GET: api/Artists
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            return await _context.Artists.ToListAsync();
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ArtistResponse>> GetArtist(int id)
        {
            var user = GetUser();
            var artist = await _context.Artists
                .Include(a => a.Releases).FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
            {
                return NotFound();
            }

            var albums = _context.Albums
                .Include(p => p.Artists)
                .Include(p => p.Tracks)
                .Include(p => p.UsersLikes)
                .ToList();

            var tracks = _context.Tracks
                .Include(p => p.Artists)
                .Include(p => p.Featuring)
                .Include(p => p.UsersLikes)
                .ToList();

            return new ArtistResponse
            {
                Id = artist.Id,
                Cover = _imageConverter.ConvertToString(artist.Cover),
                Description = artist.Description,
                Name = artist.Name,
                Albums = albums.Where(a => a.Artists.Contains(artist))
                    .Select(a => _releaseConverter.Convert(a, user.Id)),
                Tracks = tracks.Where(a => a.Artists.Contains(artist))
                    .Select(a => _releaseConverter.Convert(a, user.Id)),
            };
        }

        // PUT: api/Artists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutArtist(int id, Artist artist)
        {
            if (id != artist.Id)
            {
                return BadRequest();
            }

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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

        // POST: api/Artists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Artist>> PostArtist([FromForm]ArtistAdd artistRequest)
        {
            var user = GetUser();
            var artist = new Artist()
            {
                Name = artistRequest.Name,
                Description = artistRequest.Description,
                Cover = artistRequest.Cover is not null
                    ? _imageConverter.ConvertToByte(artistRequest.Cover)
                    : null,
            };

            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            user.Artist = artist;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtist", new { id = artist.Id }, artist);
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
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
