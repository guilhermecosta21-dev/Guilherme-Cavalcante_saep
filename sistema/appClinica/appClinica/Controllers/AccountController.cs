using appClinica.Helpers;
using appClinica.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace appClinica.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbClinicaContext _context;
        public AccountController(DbClinicaContext context) { _context = context; }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var hashCpf = Criptografia.HashSha256(model.Cpf.Trim());
            var tutor = await _context.Tutor.FirstOrDefaultAsync(t => t.Cpf == hashCpf);

            if (tutor == null)
            {
                ModelState.AddModelError("", "CPF não encontrado ou inválido. Verifique ou faça seu cadastro.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, tutor.Codigo.ToString()),
                new Claim(ClaimTypes.Name, tutor.Nome)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            HttpContext.Session.SetInt32("TutorId", tutor.Codigo);
            HttpContext.Session.SetString("TutorNome", tutor.Nome);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}