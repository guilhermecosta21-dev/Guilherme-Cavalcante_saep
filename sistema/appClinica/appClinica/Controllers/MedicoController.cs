using appClinica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appClinica.Controllers
{
    [Authorize]
    public class MedicoController : Controller
    {
        private readonly DbClinicaContext _context;
        public MedicoController(DbClinicaContext context) { _context = context; }

        public async Task<IActionResult> Index() =>
            View(await _context.Medico.OrderBy(m => m.Nome).ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var medico = await _context.Medico.FirstOrDefaultAsync(m => m.Codigo == id);
            if (medico == null) return NotFound();
            return View(medico);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Crmv,Especialidade")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Medico.AnyAsync(m => m.Crmv == medico.Crmv))
                {
                    ModelState.AddModelError("Crmv", "CRMV já cadastrado.");
                    return View(medico);
                }
                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var medico = await _context.Medico.FindAsync(id);
            if (medico == null) return NotFound();
            return View(medico);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Crmv,Especialidade")] Medico medico)
        {
            if (id != medico.Codigo) return NotFound();
            if (ModelState.IsValid)
            {
                if (await _context.Medico.AnyAsync(m => m.Crmv == medico.Crmv && m.Codigo != id))
                {
                    ModelState.AddModelError("Crmv", "CRMV já cadastrado para outro médico.");
                    return View(medico);
                }
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Medico.Any(e => e.Codigo == medico.Codigo)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var medico = await _context.Medico.FirstOrDefaultAsync(m => m.Codigo == id);
            if (medico == null) return NotFound();
            return View(medico);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _context.Agendamento.AnyAsync(a => a.MedicoId == id))
            {
                TempData["Erro"] = "Não é possível excluir: médico possui agendamentos vinculados.";
                return RedirectToAction(nameof(Index));
            }
            var medico = await _context.Medico.FindAsync(id);
            if (medico != null) _context.Medico.Remove(medico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}