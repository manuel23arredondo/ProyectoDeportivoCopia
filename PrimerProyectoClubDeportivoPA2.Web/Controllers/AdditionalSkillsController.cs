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
    
    //[Authorize(Roles = "Admin,Coach,Member")]

    public class AdditionalSkillsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly ICombosHelper combosHelper;

        public AdditionalSkillsController(DataContext dataContext,
            ICombosHelper combosHelper)
        {
            this.dataContext = dataContext;
            this.combosHelper = combosHelper;
        }

        //[Authorize(Roles = "Admin,Coach,Member")]
        public async Task<IActionResult> Index()
        {
            return View(await this.dataContext.AdditionalSkills
                .Include(c => c.Coach)
                .ThenInclude(u => u.User)
                .Include(s => s.Sport)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin,Coach")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new AdditionalSkillViewModel
            {
                Coaches = this.combosHelper.GetComboCoaches(),
                Sports = this.combosHelper.GetComboSports()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdditionalSkillViewModel model)
        {
            if (ModelState.IsValid)
            {
                var additionalSkill = new AdditionalSkill
                {
                    Name = model.Name,
                    Coach = await this.dataContext.Coaches.FindAsync(model.CoachId),
                    Sport = await this.dataContext.Sports.FindAsync(model.SportId)
                };
                this.dataContext.Add(additionalSkill);
                await this.dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Coach")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalSkill = await this.dataContext.AdditionalSkills
                .Include(c => c.Coach)
                .ThenInclude(u => u.User)
                .Include(s => s.Sport)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (additionalSkill == null)
            {
                return NotFound();
            }
            var model = new AdditionalSkillViewModel
            {
                Name = additionalSkill.Name,
                Coach = additionalSkill.Coach,
                CoachId = additionalSkill.Coach.Id,
                Coaches = this.combosHelper.GetComboCoaches(),
                Sport = additionalSkill.Sport,
                SportId = additionalSkill.Sport.Id,
                Sports = this.combosHelper.GetComboSports()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdditionalSkillViewModel model)
        {
            if (ModelState.IsValid)
            {
                var additionalSkill = new AdditionalSkill
                {
                    Id = model.Id,
                    Name = model.Name,
                    Coach = await this.dataContext.Coaches.FindAsync(model.CoachId),
                    Sport = await this.dataContext.Sports.FindAsync(model.SportId)
                };

                this.dataContext.Update(additionalSkill);
                await this.dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Coach,Member")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalSkill = await dataContext.AdditionalSkills
                .Include(c => c.Coach)
                .ThenInclude(u => u.User)
                .Include(sp => sp.Sport)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (additionalSkill == null)
            {
                return NotFound();
            }

            return View(additionalSkill);
        }

        [Authorize(Roles = "Admin,Coach")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalSkill = await dataContext.AdditionalSkills
                .Include(c => c.Coach)
                .ThenInclude(u => u.User)
                .Include(sp => sp.Sport)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (additionalSkill == null)
            {
                return NotFound();
            }

            return View(additionalSkill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var additionalSkill = await dataContext.AdditionalSkills.FindAsync(id);
            dataContext.AdditionalSkills.Remove(additionalSkill);
            await dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
