using appClinica.Helpers;
using appClinica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appVeterinaria.Controllers
{
    [Authorize]
    public class TutorController : Controller
    {
        private readonly DbClinicaContext _context;
        public TutorController(DbClinicaContext context) { _context = context; }

        public async Task<IActionResult> Index(string pesquisa)
        {
            var query = _context.Tutor.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                pesquisa = pesquisa.Trim();
                var hashPesquisa = Criptografia.HashSha256(pesquisa);
                query = query.Where(t =>
                    t.Nome.Contains(pesquisa) ||
                    t.Telefone.Contains(pesquisa) ||
                    t.Cpf == hashPesquisa);
            }
            return View(await query.OrderBy(t => t.Nome).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var tutor = await _context.Tutor.Include(t => t.Pets)
                .FirstOrDefaultAsync(t => t.Codigo == id);
            if (tutor == null) return NotFound();
            return View(tutor);
        }

        [AllowAnonymous]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Nome,Cpf,Telefone,DataNascimento")] Tutor tutor)
        {
            if (string.IsNullOrWhiteSpace(tutor.Cpf))
                ModelState.AddModelError("Cpf", "O CPF é obrigatório.");

            if (ModelState.IsValid)
            {
                tutor.Cpf = Criptografia.HashSha256(tutor.Cpf.Trim()); // criptografa antes de salvar
                if (await _context.Tutor.AnyAsync(t => t.Cpf == tutor.Cpf))
                {
                    ModelState.AddModelError("Cpf", "CPF já cadastrado.");
                    tutor.Cpf = string.Empty;
                    return View(tutor);
                }
                _context.Add(tutor);
                await _context.SaveChangesAsync();
                return User.Identity != null && User.Identity.IsAuthenticated
                    ? RedirectToAction(nameof(Index))
                    : RedirectToAction("Login", "Account");
            }
            tutor.Cpf = string.Empty;
            return View(tutor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null) return NotFound();
            return View(tutor);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Codigo,Nome,Telefone,DataNascimento")] Tutor dados, string? novoCpf)
        {
            if (id != dados.Codigo) return NotFound();
            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null) return NotFound();

            if (ModelState.IsValid)
            {
                tutor.Nome = dados.Nome;
                tutor.Telefone = dados.Telefone;
                tutor.DataNascimento = dados.DataNascimento;

                if (!string.IsNullOrWhiteSpace(novoCpf))
                {
                    var hash = Criptografia.HashSha256(novoCpf.Trim());
                    if (await _context.Tutor.AnyAsync(t => t.Cpf == hash && t.Codigo != id))
                    {
                        ModelState.AddModelError("", "CPF já cadastrado para outro tutor.");
                        return View(dados);
                    }
                    tutor.Cpf = hash;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dados);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var tutor = await _context.Tutor.FirstOrDefaultAsync(t => t.Codigo == id);
            if (tutor == null) return NotFound();
            return View(tutor);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _context.Pet.AnyAsync(p => p.TutorId == id) ||
                await _context.Agendamento.AnyAsync(a => a.TutorId == id))
            {
                TempData["Erro"] = "Não é possível excluir: tutor possui pets ou agendamentos vinculados.";
                return RedirectToAction(nameof(Index));
            }
            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor != null) _context.Tutor.Remove(tutor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}