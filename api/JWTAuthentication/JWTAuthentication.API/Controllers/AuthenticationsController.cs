using JWTAuthentication.API.Dtos.Logins;
using JWTAuthentication.Domain.Authentications;
using JWTAuthentication.Domain.Authentications.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController(IAuthenticationJwtService service, ILogger<AuthenticationsController> logger) : Controller
    {
        [HttpPost("login")]
        public async Task<ActionResult<JwtAuthResultDto>> Login([FromBody] LoginDto loginRequest)
        {
            try
            {
                var token = await service.Login(new Authentication()
                {
                    Email = loginRequest.Email,
                    Password = loginRequest.Password
                });

                return Ok(new JwtAuthResultDto(token.AccessToken, token.RefreshToken));
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }
        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            var userName = User.Identity?.Name!;
            service.Logout(userName);
            logger.LogInformation("User [{userName}] logged out the system.", userName);
            return Ok();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult<JwtAuthResultDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                var userName = User.Identity?.Name!;
                logger.LogInformation("User [{userName}] is trying to refresh JWT token.", userName);

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }
                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");

                var jwtResult = await service.RefreshToken(request.RefreshToken, accessToken ?? string.Empty);

                return Ok(new JwtAuthResultDto(jwtResult.AccessToken, jwtResult.RefreshToken));
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}
