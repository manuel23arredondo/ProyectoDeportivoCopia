namespace PrimerProyectoClubDeportivoPA2.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PrimerProyectoClubDeportivoPA2.Web.Data;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using PrimerProyectoClubDeportivoPA2.Web.Helpers;
    using PrimerProyectoClubDeportivoPA2.Web.Models;
    using System.Threading.Tasks;

    [Authorize(Roles ="Admin,Coach")]
    public class AgendaController : Controller
    {
        private readonly DataContext dataContext;
        private readonly ICombosHelper combosHelper;

        public AgendaController(DataContext dataContext,
            ICombosHelper combosHelper)
        {
            this.dataContext = dataContext;
            this.combosHelper = combosHelper;
        }
        
        [Authorize(Roles = "Admin,Coach")]
        public async Task<IActionResult> Index()
        {
            return View(await this.dataContext.Agendas
                .Include(m => m.Member)
                .ThenInclude(u => u.User)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule.WeekDay)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule.Facility)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Coach.User )
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Sport)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new AgendaViewModel
            {
                TrainingSessions = this.combosHelper.GetComboTrainingSessions(),
                Members = this.combosHelper.GetComboMembers()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AgendaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var agenda = new Agenda
                {
                    TrainingSession = await this.dataContext.TrainingSessions.FindAsync(model.TrainingSessionId),
                    Member = await this.dataContext.Members.FindAsync(model.MemberId)
                };
                this.dataContext.Add(agenda);
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

            var agenda = await this.dataContext.Agendas
                .Include(m => m.Member)
                .ThenInclude(u => u.User)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule.WeekDay)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule.Facility)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Coach.User)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Sport)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (agenda == null)
            {
                return NotFound();
            }

            var model = new AgendaViewModel
            {
                TrainingSession = agenda.TrainingSession,
                TrainingSessionId = agenda.TrainingSession.Id,
                TrainingSessions = this.combosHelper.GetComboTrainingSessions(),
                Member = agenda.Member,
                MemberId = agenda.Member.Id,
                Members = this.combosHelper.GetComboMembers(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AgendaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var agenda = new Agenda
                {
                    Id = model.Id,
                    TrainingSession = await this.dataContext.TrainingSessions.FindAsync(model.TrainingSessionId),
                    Member = await this.dataContext.Members.FindAsync(model.MemberId)
                };

                this.dataContext.Update(agenda);
                await this.dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Coach")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await dataContext.Agendas
                .Include(m => m.Member)
                .ThenInclude(u => u.User)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule.WeekDay)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule.Facility)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Coach.User)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Sport)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await dataContext.Agendas
                .Include(m => m.Member)
                .ThenInclude(u => u.User)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule.WeekDay)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Schedule.Facility)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Coach.User)
                .Include(t => t.TrainingSession)
                .ThenInclude(s => s.Sport)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agenda = await dataContext.Agendas.FindAsync(id);
            dataContext.Agendas.Remove(agenda);
            await dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
