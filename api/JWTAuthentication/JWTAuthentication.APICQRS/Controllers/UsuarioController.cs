using JWTAuthentication.Domain.Core;
using JWTAuthentication.Domain.Usuarios.Aggregates;
using JWTAuthentication.Domain.Usuarios.Aggregates.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.APICQRS.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ICommandBus _commandBus;

        public UsuarioController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        // GET: UsuarioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
