using JWTAuthentication.Domain.Core;
using JWTAuthentication.Domain.Usuarios.Aggregates;
using JWTAuthentication.Domain.Usuarios.Aggregates.Commands;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.APICQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly ICommandBus _commandBus;

        public UsuarioController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        // POST: UsuarioController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUsuarioRequest request)
        {
            try
            {
                UsuarioId usuarioId = UsuarioId.New;
                await _commandBus.PublishAsync(
                    new CreateUsuarioCommand(
                        usuarioId, request.Email,
                        request.PasswordHash,
                        request.Username),
                    CancellationToken.None)
                    .ConfigureAwait(false);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


}
