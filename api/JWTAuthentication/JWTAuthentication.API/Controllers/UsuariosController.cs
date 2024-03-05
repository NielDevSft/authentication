using JWTAuthentication.API.Dtos.Usuarios;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Service;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController(IUsuarioService usuarioService) : Controller
    {
        // GET: UsuariosController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> Index()
        {
            try
            {

                var usuarioList = await usuarioService.GetAll();
                return Ok(usuarioList.Select(u =>
                new UsuarioDto(
                u.Email,
                u.Username,
                u.PasswordHash,
                u.Id,
                u.CreateAt,
                u.UpdateAt)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: UsuariosController/Details/5
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
                    usuarioExisting.Id,
                    usuarioExisting.CreateAt,
                    usuarioExisting.UpdateAt
                );
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
            return Ok(usuario);
        }

        // GET: UsuariosController/Create
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
                BadRequest(ex.Message);
            }
            return Created("success", usuarioDto);
        }

        //// POST: UsuariosController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UsuariosController/Edit/5
        //public async Task<ActionResult> Edit(int id)
        //{
        //    return View();
        //}

        //// POST: UsuariosController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UsuariosController/Delete/5
        //public async Task<ActionResult> Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UsuariosController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
