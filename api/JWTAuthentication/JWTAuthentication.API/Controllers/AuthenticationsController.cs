using JWTAuthentication.API.Dtos.Logins;
using JWTAuthentication.Domain.Authentications;
using JWTAuthentication.Domain.Authentications.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController(IAuthenticationService service) : Controller
    {
        [HttpPost(Name ="login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
        {
            try
            {
                var token = await service.Login(new Authentication()
                {
                    Email = loginRequest.Email,
                    Password = loginRequest.Password
                });

                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }
    }
}
