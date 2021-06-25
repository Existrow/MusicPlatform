using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MusicPlatform.Models.Api.Auth;
using MusicPlatform.Models.DataBaseEntities;
using MusicPlatform.Services;
using MusicPlatform.Settings.Auth;

namespace MusicPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PlatformContext _context;

        public AuthController(PlatformContext context, DataCreator creator)
            => (_context) = (context);

        [HttpPost]
        [Route("token")]
        public ActionResult<AuthResponse> Token([FromBody]AuthRequest request)
        {
            var identity = GetIdentity(request.Username, request.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResponse()
            {
                AccessToken = encodedJwt,
                Username = identity.Name
            };
        }

        [HttpPost]
        [Route("checkAuth")]
        [Authorize]
        public ActionResult<string> Check()
        {
            return GetUser() is not null
                ? User.Identity.Name
                : BadRequest("Identiti timeout");
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody]AuthRequest request)
        {
            var login = request.Username.ToLower();
            if(!_context.Users.Where(user => user.Login == login).Any())
            {
                var user = new User()
                {
                    Login = login,
                    Password = request.Password,
                    Role = "user"
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Token(new() { Username = user.Login, Password = user.Password });
            }
            return BadRequest($"Login \"{login}\" is busy");
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            if(username is null || password is null)
            {
                return null;
            }
            var person = _context.Users.FirstOrDefault(x => (x.Login == username.ToLower()) && (x.Password == password));
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                return new (claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }
            return null;
        }

        private User GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var user = _context.Users.FirstOrDefault((user) => user.Login == userName);
            return user;
        }
    }
}
