using backlog.api;
using backlog.api.Classes;
using backlog.api.DTO;
using backlog.api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            Context context,
            ILogger<AuthController> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult Login(AuthRequest data)
        {
            try
            {
                var hash = Hasher.HashToHex(Hasher.GetHash(data.Password));
                var user = _context.Users.FirstOrDefault(x => x.Login == data.Login && x.Password == hash);
                if (user == null)
                    return StatusCode(StatusCodes.Status403Forbidden, "Неправильный логин или пароль");

                var token = GenerateToken(user.Id, user.Login);
                return Json(new { token });
            }
            catch (Exception ex)
            {
                var msg = "Ошибка при входе в систему";
                _logger.LogError(ex, msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Authorize]
        public IActionResult CurrentUser()
        {
            try
            {
                var userID = this.CurrentUserId();
                var currentUser = _context.Users.First(u => u.Id == userID);

                return Ok(currentUser);
            }
            catch (Exception ex)
            {
                var msg = "Ошибка получения текущего пользователя";
                _logger.LogError(ex, msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        private static string GenerateToken(int id, string login)
        {
            var claims = new List<Claim> {
                    new (ClaimsIdentity.DefaultNameClaimType, login),
                    new ("UserId", id.ToString())
                };

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.Issuer,
                            audience: AuthOptions.Audience,
                            notBefore: now,
                            claims: new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType).Claims,
                            expires: now.Add(TimeSpan.FromHours(AuthOptions.TokenLifeTimeHours)),
                            signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}