using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IronFit.Models;
using IronFit.Data;

namespace IronFit.Controllers
{
    [Authorize]
    public class AlunoController : Controller
    {
        private readonly Alunos _aluno;

        public AlunoController(Alunos aluno)
        {
            _aluno = aluno;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(Alunos aluno)
        {
       
            if (aluno.PersonalId <= 0)
            {
                return BadRequest("O vínculo com o Personal Trainer é obrigatório.");
            }

            if (ModelState.IsValid)
            {
                await _aluno.Salvar(aluno);
                return RedirectToAction("Index", "Treino");
            }

            return RedirectToAction("Index", "Treino");
        }
    }
}