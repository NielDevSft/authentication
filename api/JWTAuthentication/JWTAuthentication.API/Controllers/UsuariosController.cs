using JWTAuthentication.API.Dtos.Roles;
using JWTAuthentication.API.Dtos.Usuarios;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<UsuarioDto>>> Index()
        {
            try
            {
                var usuarioList = await usuarioService.GetAll();
                return Ok(usuarioList.Select(u =>
                new UsuarioDto(
                u.Email,
                u.Username,
                u.PasswordHash,
                null,
                u.Uuid.ToString(),
                u.CreateAt,
                u.UpdateAt)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{uuid}")]
        [Authorize]
        public async Task<ActionResult<UsuarioDto>> Details(Guid uuid)
        {
            UsuarioDto usuario;
            try
            {

                var usuarioExisting = await usuarioService.GetById(uuid);
                usuario = new UsuarioDto(
                usuarioExisting.Email,
                    usuarioExisting.Username,
                    usuarioExisting.PasswordHash,
                    null!,
                    usuarioExisting.Uuid.ToString(),
                    usuarioExisting.CreateAt,
                    usuarioExisting.UpdateAt
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(usuario);
        }
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> Create([FromBody] UsuarioDto usuario)
        {
            UsuarioDto usuarioDto;
            try
            {
                var usuarioCreated = await usuarioService.Create(new Usuario()
                {
                    Email = usuario.Email,
                    Username = usuario.Username,
                    PasswordHash = usuario.PasswordHash,
                });
                usuarioDto = usuario with
                {
                    Uuid = usuarioCreated.Uuid.ToString(),
                    CreateAt = usuarioCreated.CreateAt,
                    UpdateAt = usuarioCreated.UpdateAt,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Created("success", usuarioDto);
        }

        [HttpPost("{uuid}/Roles")]
        public async Task<ActionResult<UsuarioDto>> SetRoleList(Guid uuid, [FromBody] ICollection<Guid> uuidRoles)
        {
            UsuarioDto usuarioDto;
            try
            {
                var usuarioCreated = await usuarioService.SetRoleList(uuid, uuidRoles);
                usuarioDto = new UsuarioDto(usuarioCreated.Email,
                    usuarioCreated.Username,
                    usuarioCreated.PasswordHash,
                    usuarioCreated
                    .JwtClaims!
                    .RoleJwtClaims.Select(c => c.Role)
                    .Select(r => new RoleDto(r!.Name, r.CreateAt, r.UpdateAt, r.Uuid.ToString()))
                    .ToList(),
                    usuarioCreated.Uuid.ToString(),
                    usuarioCreated.CreateAt,
                    usuarioCreated.UpdateAt);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Created("success", usuarioDto);
        }
    }
}
