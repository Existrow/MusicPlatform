using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Models.Api.Responses;
using MusicPlatform.Models.DataBaseEntities;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IChartBuilder _chartBuilder;
        private readonly PlatformContext _context;
        private readonly IReleaseToApiConverter _apiConverter;

        public ChartController(PlatformContext context, IChartBuilder builder, IReleaseToApiConverter converter)
            => (_chartBuilder, _context, _apiConverter) = (builder, context, converter);

        [Authorize]
        public async Task<ActionResult<ChartApi>> GetChart()
        {
            var user = GetUser();
            if(user is not null)
            {
                var chart = await _chartBuilder.GetChart();

                return new ChartApi
                {
                    Tracks = chart.Tracks.Select(t => _apiConverter.Convert(t, user.Id)),
                    Albums = chart.Albums.Select(a => _apiConverter.Convert(a, user.Id))
                };
            }
            return BadRequest("data base error");
        }

        private User GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            return _context.Users.FirstOrDefault((user) => user.Login == userName);
        }
    }
}
