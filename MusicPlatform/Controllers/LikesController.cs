using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Models.Api.Auth;
using MusicPlatform.Models.Api.Responses;
using MusicPlatform.Models.DataBaseEntities;
using MusicPlatform.Models.Implementations;
using MusicPlatform.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly PlatformContext _context;
        private readonly IReleaseToApiConverter _apiConverter;

        public LikesController(PlatformContext context, IReleaseToApiConverter converter)
            => (_context, _apiConverter) = (context, converter);

        [HttpGet]
        [Authorize]
        public ActionResult<ChartApi> Get()
        {
            var user = GetUser();
            return new ChartApi()
            {
                Tracks = _context.Tracks
                    .Include(t => t.UsersLikes)
                    .Include(t => t.Artists)
                    .Include(t => t.Featuring)
                    .Include(t => t.Albums)
                    .Where(track => track.UsersLikes.Contains(user)).ToList()
                    .Select(t => _apiConverter.Convert(t, user.Id)),
                Albums = _context.Albums
                    .Include(a => a.UsersLikes)
                    .Include(a => a.Artists)
                    .Include(a => a.Tracks)
                    .Where(album => album.UsersLikes.Contains(user)).ToList()
                    .Select(a => _apiConverter.Convert(a, user.Id))
            };
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult> Like(int id)
        {
            var user = GetUser();
            var release = _context.Releases.Find(id);
            if (release is not null)
            {
                user.LikedReleases.Add(release);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Invalid Release Id");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> UnLike(int id)
        {
            var user = GetUser();
            var release = _context.Releases.Find(id);
            if (release is not null)
            {
                user.LikedReleases.Remove(release);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Invalid Release Id");
        }

        private User GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var user = _context.Users.Include(u => u.LikedReleases).FirstOrDefault((user) => user.Login == userName);
            return user;
        }
    }
}
