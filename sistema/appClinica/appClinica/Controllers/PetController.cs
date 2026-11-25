using appClinica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace appVeterinaria.Controllers
{
    [Authorize]
    public class PetController : Controller
    {
        private readonly DbClinicaContext _context;
        public PetController(DbClinicaContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var pets = await _context.Pet.Include(p => p.Tutor).OrderBy(p => p.Nome).ToListAsync();
            return View(pets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var pet = await _context.Pet.Include(p => p.Tutor)
                .FirstOrDefaultAsync(p => p.Codigo == id);
            if (pet == null) return NotFound();
            return View(pet);
        }

        public IActionResult Create()
        {
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Codigo", "Nome");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Especie,Raca,TutorId")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Codigo", "Nome", pet.TutorId);
            return View(pet);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var pet = await _context.Pet.FindAsync(id);
            if (pet == null) return NotFound();
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Codigo", "Nome", pet.TutorId);
            return View(pet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Especie,Raca,TutorId")] Pet pet)
        {
            if (id != pet.Codigo) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Pet.Any(e => e.Codigo == pet.Codigo)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TutorId"] = new SelectList(_context.Tutor, "Codigo", "Nome", pet.TutorId);
            return View(pet);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var pet = await _context.Pet.Include(p => p.Tutor)
                .FirstOrDefaultAsync(p => p.Codigo == id);
            if (pet == null) return NotFound();
            return View(pet);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _context.Agendamento.AnyAsync(a => a.PetId == id))
            {
                TempData["Erro"] = "Não é possível excluir: pet possui agendamentos vinculados.";
                return RedirectToAction(nameof(Index));
            }
            var pet = await _context.Pet.FindAsync(id);
            if (pet != null) _context.Pet.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}