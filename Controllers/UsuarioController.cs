using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using IronFit.Models;
using IronFit.Data;

namespace IronFit.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Usuarios _usuario;

        public UsuariosController(Usuarios usuario)
        {
            _usuario = usuario;
        }

        [HttpGet]
        public IActionResult Login(string? erro)
        {
            ViewBag.Erro = erro;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                ViewBag.Erro = "Preencha todos os campos!";
                return View();
            }

            var usuario = await _usuario.BuscarPorEmail(email);

         
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
            {
                ViewBag.Erro = "Usuário ou senha inválidos!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Nome, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });

            return RedirectToAction("Index", "Dashboard");
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}