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
    public class SchedulesController : Controller
    {
        private readonly DataContext dataContext;
        private readonly ICombosHelper combosHelper;

        public SchedulesController(DataContext dataContext,
            ICombosHelper combosHelper)
        {
            this.dataContext = dataContext;
            this.combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.dataContext.Schedules
                .Include(f => f.Facility)
                .Include(w => w.WeekDay)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ScheduleViewModel
            {
                Facilities = this.combosHelper.GetComboFacilities(),
                WeekDays = this.combosHelper.GetComboWeekdays()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = new Schedule
                {
                    StartingHour = model.StartingHour,
                    FinishingHour = model.FinishingHour,
                    Facility = await this.dataContext.Facilities.FindAsync(model.FacilityId),
                    WeekDay = await this.dataContext.WeekDays.FindAsync(model.WeekDayId)
                };
                this.dataContext.Add(schedule);
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

            var schedule = await this.dataContext.Schedules
                .Include(f => f.Facility)
                .Include(w => w.WeekDay)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            var model = new ScheduleViewModel
            {
                StartingHour = schedule.StartingHour,
                FinishingHour = schedule.FinishingHour,
                Facility = schedule.Facility,
                FacilityId = schedule.Facility.Id,
                Facilities = this.combosHelper.GetComboFacilities(),
                WeekDay = schedule.WeekDay,
                WeekDayId = schedule.WeekDay.Id,
                WeekDays = this.combosHelper.GetComboWeekdays(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = new Schedule
                {
                    Id = model.Id,
                    StartingHour = model.StartingHour,
                    FinishingHour = model.FinishingHour,
                    Facility = await this.dataContext.Facilities.FindAsync(model.FacilityId),
                    WeekDay = await this.dataContext.WeekDays.FindAsync(model.WeekDayId)
                };

                this.dataContext.Update(schedule);
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

            var schedule = await dataContext.Schedules
                .Include(f => f.Facility)
                .Include(w => w.WeekDay)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await dataContext.Schedules
                .Include(f => f.Facility)
                .Include(w => w.WeekDay)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await dataContext.Schedules.FindAsync(id);
            dataContext.Schedules.Remove(schedule);
            try
            {
                await dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "No se pueden eliminar registros");
            }
            return View(schedule);
        }
    }
}
