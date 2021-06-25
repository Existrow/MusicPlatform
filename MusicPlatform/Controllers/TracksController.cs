using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Models.Api.Requests;
using MusicPlatform.Models.DataBaseEntities;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly PlatformContext _context;
        private readonly IImageFileConverter _imageConverter;
        private readonly IReleaseToApiConverter _releaseConverter;
        private readonly IFileSaver _fileSaver;

        public TracksController(PlatformContext context, IImageFileConverter imageConverter, IReleaseToApiConverter releaseConverter, IFileSaver saver)
            => (_context, _imageConverter, _releaseConverter, _fileSaver) = (context, imageConverter, releaseConverter, saver);

        // GET: api/Tracks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
            => await _context.Tracks.ToListAsync();

        // GET: api/Tracks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
            {
                return NotFound();
            }

            return track;
        }

        // PUT: api/Tracks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTrack(int id, [FromForm]TrackAdd trackRequest)
        {
            var track = _context.Tracks.Find(id);
            track.Title = trackRequest.Title;
            track.Cover = trackRequest.Cover is not null
                    ? _imageConverter.ConvertToByte(trackRequest.Cover)
                    : null;
            track.FilePath = trackRequest.File is not null
                    ? _fileSaver.Save(trackRequest.File)
                    : null;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackExists(id))
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

        // POST: api/Tracks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Track>> PostTrack([FromForm]TrackAdd trackRequest)
        {
            var artist = _context.Artists.Find(trackRequest.ArtistId);
            var track = new Track
            {
                Title = trackRequest.Title,
                Artists = new List<Artist> { artist },
                ReleaseData = DateTime.Now,
                Cover = trackRequest.Cover is not null
                    ? _imageConverter.ConvertToByte(trackRequest.Cover)
                    : null,
                FilePath = trackRequest.File is not null
                    ? _fileSaver.Save(trackRequest.File)
                    : null,
                Genre = _context.Genres.First()
            };
            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrack", new { id = track.Id }, track);
        }

        // DELETE: api/Tracks/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrackExists(int id)
        {
            return _context.Tracks.Any(e => e.Id == id);
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
