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
                u.Id,
                u.CreateAt,
                u.UpdateAt)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> Details(int id)
        {
            UsuarioDto usuario = null;
            try
            {

                var usuarioExisting = await usuarioService.GetById(id);
                usuario = new UsuarioDto(
                usuarioExisting.Email,
                    usuarioExisting.Username,
                    usuarioExisting.PasswordHash,
                    null,
                    usuarioExisting.Id,
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
            UsuarioDto usuarioDto = null;
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
                    Id = usuarioCreated.Id,
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

        [HttpPost("set-role-list")]
        public async Task<ActionResult<UsuarioDto>> SetRoleList([FromBody] SetRoleListDto setRoleListDto)
        {
            UsuarioDto usuarioDto = null;
            try
            {
                var usuarioCreated = await usuarioService.SetRoleList(setRoleListDto.id, setRoleListDto.roles.ToList());
                usuarioDto = new UsuarioDto(usuarioCreated.Email,
                    usuarioCreated.Username,
                    usuarioCreated.PasswordHash,
                    usuarioCreated
                    .JwtClaims
                    .RoleJwtClaims.Select(c => c.Role)
                    .Select(r => new RoleDto(r!.Id, r.Name, r.CreateAt, r.UpdateAt))
                    .ToList(),
                    usuarioCreated.Id,
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
