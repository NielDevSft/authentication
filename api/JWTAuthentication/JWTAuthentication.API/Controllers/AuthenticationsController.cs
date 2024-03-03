using JWTAuthentication.API.Dtos.Login;
using JWTAuthentication.Domain.Authentications;
using JWTAuthentication.Domain.Authentications.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{

    public class AuthenticationsController(IAuthenticationService service) : Controller
    {
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
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
