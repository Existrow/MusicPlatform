using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Services;

namespace MusicPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly DataCreator _creator;

        public DebugController(DataCreator creator)
            => _creator = creator;

        [HttpGet]
        public IActionResult Get()
        {
            _creator.Create();
            return Ok();
        }
    }
}
