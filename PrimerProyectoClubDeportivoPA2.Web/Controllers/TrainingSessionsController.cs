namespace PrimerProyectoClubDeportivoPA2.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PrimerProyectoClubDeportivoPA2.Web.Data;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using PrimerProyectoClubDeportivoPA2.Web.Helpers;
    using PrimerProyectoClubDeportivoPA2.Web.Models;
    using System;
    using System.Threading.Tasks;
    [Authorize(Roles = "Admin,Coach,Member")]
    public class TrainingSessionsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly ICombosHelper combosHelper;

        public TrainingSessionsController(DataContext dataContext,
            ICombosHelper combosHelper)
        {
            this.dataContext = dataContext;
            this.combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.dataContext.TrainingSessions
                .Include(c => c.Coach)
                .ThenInclude(u => u.User)
                .Include(s => s.Schedule)
                .ThenInclude(w => w.WeekDay)
                .Include(s => s.Schedule)
                .ThenInclude(f => f.Facility)
                .Include(sp => sp.Sport)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin,Coach")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new TrainingSessionViewModel
            {
                Coaches = this.combosHelper.GetComboCoaches(),
                Schedules = this.combosHelper.GetComboSchedules(),
                Sports = this.combosHelper.GetComboSports()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrainingSessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trainingSession = new TrainingSession
                {
                    Name = model.Name,
                    Capacity = model.Capacity,
                    Coach = await this.dataContext.Coaches.FindAsync(model.CoachId),
                    Schedule = await this.dataContext.Schedules.FindAsync(model.ScheduleId),
                    Sport = await this.dataContext.Sports.FindAsync(model.SportId)
                };
                this.dataContext.TrainingSessions.Add(trainingSession);
                await this.dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingSession = await this.dataContext.TrainingSessions
                .Include(c => c.Coach)
                .ThenInclude(u => u.User)
                .Include(s => s.Schedule)
                .Include(sp => sp.Sport)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainingSession == null)
            {
                return NotFound();
            }

            var model = new TrainingSessionViewModel
            {
                Name = trainingSession.Name,
                Capacity = trainingSession.Capacity,
                Coaches = this.combosHelper.GetComboCoaches(),
                Schedules = this.combosHelper.GetComboSchedules(),
                Sports = this.combosHelper.GetComboSports(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrainingSessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trainingSession = new TrainingSession
                {
                    Id = model.Id,
                    Name = model.Name,
                    Capacity = model.Capacity,
                    Coach = await this.dataContext.Coaches.FindAsync(model.CoachId),
                    Schedule = await this.dataContext.Schedules.FindAsync(model.ScheduleId),
                    Sport = await this.dataContext.Sports.FindAsync(model.SportId)
                };

                this.dataContext.Update(trainingSession);
                await this.dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingSession = await dataContext.TrainingSessions
                .Include(c => c.Coach)
                .ThenInclude(u => u.User)
                .Include(s => s.Schedule)
                .ThenInclude(w => w.WeekDay)
                .Include(s => s.Schedule)
                .ThenInclude(f => f.Facility)
                .Include(sp => sp.Sport)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainingSession == null)
            {
                return NotFound();
            }

            return View(trainingSession);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingSession = await dataContext.TrainingSessions
                .Include(c => c.Coach)
                .ThenInclude(u => u.User)
                .Include(s => s.Schedule)
                .ThenInclude(w => w.WeekDay)
                .Include(s => s.Schedule)
                .ThenInclude(f => f.Facility)
                .Include(sp => sp.Sport)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainingSession == null)
            {
                return NotFound();
            }

            return View(trainingSession);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingSession = await dataContext.TrainingSessions.FindAsync(id);
            dataContext.TrainingSessions.Remove(trainingSession);
            try
            {
                await dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "No se pueden eliminar registros");
            }
            return View(trainingSession);
        }
    }
}
