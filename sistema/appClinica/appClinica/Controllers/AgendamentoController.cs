using appClinica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace appClinica.Controllers
{
    [Authorize]
    public class AgendamentoController : Controller
    {
        private readonly DbClinicaContext _context;
        public AgendamentoController(DbClinicaContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var agendamentos = await _context.Agendamento
                .Include(a => a.Tutor)
                .Include(a => a.Pet)
                .Include(a => a.Medico)
                .OrderBy(a => a.DataAgendamento)
                .ToListAsync();
            return View(agendamentos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var agendamento = await _context.Agendamento
                .Include(a => a.Tutor).Include(a => a.Pet).Include(a => a.Medico)
                .FirstOrDefaultAsync(a => a.Codigo == id);
            if (agendamento == null) return NotFound();
            return View(agendamento);
        }

        public IActionResult Create()
        {
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Codigo", "Nome");
            ViewData["PetId"] = new SelectList(_context.Pet, "Codigo", "Nome");
            ViewData["MedicoId"] = new SelectList(_context.Medico, "Codigo", "Nome");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("DataAgendamento,TutorId,PetId,MedicoId")] Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Codigo", "Nome", agendamento.TutorId);
            ViewData["PetId"] = new SelectList(_context.Pet, "Codigo", "Nome", agendamento.PetId);
            ViewData["MedicoId"] = new SelectList(_context.Medico, "Codigo", "Nome", agendamento.MedicoId);
            return View(agendamento);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var agendamento = await _context.Agendamento.FindAsync(id);
            if (agendamento == null) return NotFound();
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Codigo", "Nome", agendamento.TutorId);
            ViewData["PetId"] = new SelectList(_context.Pet, "Codigo", "Nome", agendamento.PetId);
            ViewData["MedicoId"] = new SelectList(_context.Medico, "Codigo", "Nome", agendamento.MedicoId);
            return View(agendamento);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Codigo,DataAgendamento,TutorId,PetId,MedicoId")] Agendamento agendamento)
        {
            if (id != agendamento.Codigo) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agendamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Agendamento.Any(e => e.Codigo == agendamento.Codigo)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Codigo", "Nome", agendamento.TutorId);
            ViewData["PetId"] = new SelectList(_context.Pet, "Codigo", "Nome", agendamento.PetId);
            ViewData["MedicoId"] = new SelectList(_context.Medico, "Codigo", "Nome", agendamento.MedicoId);
            return View(agendamento);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var agendamento = await _context.Agendamento
                .Include(a => a.Tutor).Include(a => a.Pet).Include(a => a.Medico)
                .FirstOrDefaultAsync(a => a.Codigo == id);
            if (agendamento == null) return NotFound();
            return View(agendamento);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agendamento = await _context.Agendamento.FindAsync(id);
            if (agendamento != null) _context.Agendamento.Remove(agendamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}