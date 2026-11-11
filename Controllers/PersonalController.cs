using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IronFit.Models;
using IronFit.Data;

namespace IronFit.Controllers
{
    [Authorize]
    public class PersonalController : Controller
    {
        private readonly Personal _personal;

        public PersonalController(Personal personal)
        {
            _personal = personal;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index(string busca = "")
        {
            var personals = await _personal.Listar(busca);
            ViewBag.Busca = busca;
            return View(personals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(Personal personal)
        {
            if (ModelState.IsValid)
            {
                await _personal.Salvar(personal);
                return RedirectToAction(nameof(Index));
            }

            return View("Index", await _personal.Listar());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, string nome, string telefone, string especialidade)
        {
            var personal = await _personal.BuscarPorId(id);
            if (personal == null)
            {
                return NotFound();
            }

            personal.Nome = nome;
            personal.Telefone = telefone;
            personal.Especialidade = especialidade;

            await _personal.Atualizar(personal);
            return RedirectToAction(nameof(Index));
        }
    }
}
