using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IronFit.Models;
using IronFit.Data;

namespace IronFit.Controllers
{
    [Authorize]
    public class TreinoController : Controller
    {
        private readonly Treinos _treino;
        private readonly Personal _personal;
        private readonly Alunos _aluno;

        public TreinoController(
            Treinos treino,
            Personal personal,
            Alunos aluno)
        {
            _treino= treino;
            _personal= personal;
            _aluno = aluno;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var treinos = await _treino.ListarPorData();
            ViewBag.Personals = await _personal.Listar();
            ViewBag.Alunos = await _aluno.Listar();

            return View(treinos);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(int alunoId, DateTime data)
        {
            var aluno = await _aluno.BuscarPorId(alunoId);

            if (aluno != null)
            {
                var treino = new Treinos
                {
                    AlunoId = aluno.Id,
                    PersonalId = aluno.PersonalId, 
                    Data = data,
                    Status = "Agendado"
                };

                await _treino.Salvar(treino);
            }

            return RedirectToAction(nameof(Index));
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarData(int id, DateTime novaData)
        {
            await _treino.AtualizarData(id, novaData);
            return RedirectToAction(nameof(Index));
        }
    }
}
