using JWTAuthentication.API.Dtos.Roles;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Service;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleService roleService, ILogger<RolesController> logger) : Controller
    {

        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto role)
        {
            RoleDto roleDto = null;
            try
            {
                var roleCreated = await roleService.Create(new Role()
                {
                    Name = role.Name
                });
                roleDto = role with
                {
                    Id = roleCreated.Id,
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
        public async Task<ActionResult<ICollection<RoleDto>>> Index()
        {
            try
            {

                var roleList = await roleService.GetAll();
                return Ok(roleList.Select(r =>
                new RoleDto(r.Id, r.Name, r.CreateAt, r.UpdateAt)
               ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await roleService.Delete(id);
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
