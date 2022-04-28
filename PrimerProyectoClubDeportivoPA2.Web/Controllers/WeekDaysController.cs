namespace PrimerProyectoClubDeportivoPA2.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PrimerProyectoClubDeportivoPA2.Web.Data;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = "Admin")]
    public class WeekDaysController : Controller
    {
        private readonly DataContext _context;

        public WeekDaysController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.WeekDays.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await _context.WeekDays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weekDay == null)
            {
                return NotFound();
            }

            return View(weekDay);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] WeekDay weekDay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weekDay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weekDay);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await _context.WeekDays.FindAsync(id);
            if (weekDay == null)
            {
                return NotFound();
            }
            return View(weekDay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] WeekDay weekDay)
        {
            if (id != weekDay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weekDay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeekDayExists(weekDay.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(weekDay);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekDay = await _context.WeekDays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weekDay == null)
            {
                return NotFound();
            }

            return View(weekDay);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weekDay = await _context.WeekDays.FindAsync(id);
            _context.WeekDays.Remove(weekDay);
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "No se pueden eliminar registros");
            }
            return View(weekDay);
        }

        private bool WeekDayExists(int id)
        {
            return _context.WeekDays.Any(e => e.Id == id);
        }
    }
}
