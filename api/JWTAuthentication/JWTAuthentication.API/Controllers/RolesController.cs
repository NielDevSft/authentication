using JWTAuthentication.API.Dtos.Roles;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleService roleService, ILogger<RolesController> logger) : Controller
    {

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto role)
        {
            RoleDto roleDto;
            try
            {
                var roleCreated = await roleService.Create(new Role()
                {
                    Name = role.Name
                });
                roleDto = role with
                {
                    Uuid = roleCreated.Uuid.ToString(),
                    CreateAt = roleCreated.CreateAt,
                    UpdateAt = roleCreated.UpdateAt,
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Created("success", roleDto);
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<RoleDto>>> Index()
        {
            try
            {

                var roleList = await roleService.GetAll();
                return Ok(roleList.Select(r =>
                new RoleDto(r.Name, r.CreateAt, r.UpdateAt, r.Uuid.ToString())
               ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{uuid}")]
        [Authorize]
        public async Task<ActionResult> Delete(Guid uuid)
        {
            try
            {
                await roleService.Delete(uuid);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
