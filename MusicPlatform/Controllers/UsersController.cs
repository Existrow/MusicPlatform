using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Models.Api;
using MusicPlatform.Models.Api.Auth;
using MusicPlatform.Models.DataBaseEntities;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PlatformContext _context;
        private readonly IReleaseToApiConverter _apiConverter;

        public UsersController(PlatformContext context, IReleaseToApiConverter converter)
        {
            _context = context;
            _apiConverter = converter;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponse>> GetUser(string id)
        {
            var user = await _context.Users.Include(u => u.Artist).FirstOrDefaultAsync(user => user.Login == id);
            if (user is not null)
            {
                return new AccountResponse
                {
                    UserName = user.Login,
                    ArtistId = user.Artist.Id
                };
            }
            return BadRequest($"{id}: Invalid user id");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
