using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Models.DataBaseEntities;

namespace MusicPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListningsController : ControllerBase
    {
        private readonly PlatformContext _context;

        public ListningsController(PlatformContext context)
            => _context = context;

        [Authorize]
        [HttpPost("{id}")]
        public async Task<IActionResult> AddListning(int id)
        {
            var release = _context.Releases.Find(id);
            if(release is not null)
            {
                release.Listnings++;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Invalid id");
        }
    }
}
