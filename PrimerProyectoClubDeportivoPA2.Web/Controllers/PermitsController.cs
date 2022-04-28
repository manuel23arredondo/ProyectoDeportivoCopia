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

    public class PermitsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly ICombosHelper combosHelper;

        public PermitsController(DataContext dataContext,
            ICombosHelper combosHelper)
        {
            this.dataContext = dataContext;
            this.combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.dataContext.Permits
                .Include(m => m.MembershipType)
                .Include(s => s.Sport)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new PermitViewModel
            {
                MembershipTypes = this.combosHelper.GetComboMembershipTypes(),
                Sports = this.combosHelper.GetComboSports()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PermitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var permit = new Permit
                {
                    Name = model.Name,
                    MembershipType = await this.dataContext.MembershipTypes.FindAsync(model.MembershipTypeId),
                    Sport = await this.dataContext.Sports.FindAsync(model.SportId)
                };
                this.dataContext.Add(permit);
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

            var permit = await this.dataContext.Permits
                .Include(m => m.MembershipType)
                .Include(s => s.Sport)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (permit == null)
            {
                return NotFound();
            }

            var model = new PermitViewModel
            {
                Name = permit.Name,
                MembershipType = permit.MembershipType,
                MembershipTypeId = permit.MembershipType.Id,
                MembershipTypes = this.combosHelper.GetComboMembershipTypes(),
                Sport = permit.Sport,
                SportId = permit.Sport.Id,
                Sports = this.combosHelper.GetComboSports(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PermitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var permit = new Permit
                {
                    Id = model.Id,
                    Name = model.Name,
                    MembershipType = await this.dataContext.MembershipTypes.FindAsync(model.MembershipTypeId),
                    Sport = await this.dataContext.Sports.FindAsync(model.SportId)
                };

                this.dataContext.Update(permit);
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

            var permit = await dataContext.Permits
                .Include(m => m.MembershipType)
                .Include(s => s.Sport)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (permit == null)
            {
                return NotFound();
            }

            return View(permit);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permit = await dataContext.Permits
                .Include(m => m.MembershipType)
                .Include(s => s.Sport)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (permit == null)
            {
                return NotFound();
            }

            return View(permit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permit = await dataContext.Permits.FindAsync(id);
            dataContext.Permits.Remove(permit);
            await dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}