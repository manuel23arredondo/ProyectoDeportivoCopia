namespace PrimerProyectoClubDeportivoPA2.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PrimerProyectoClubDeportivoPA2.Web.Data;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using PrimerProyectoClubDeportivoPA2.Web.Helpers;
    using PrimerProyectoClubDeportivoPA2.Web.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = "Admin")]
    public class CoachesController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;
        private readonly IImageHelper imageHelper;

        public CoachesController(DataContext dataContext, IImageHelper imageHelper, IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.imageHelper = imageHelper;
            this.userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await dataContext.Coaches
                .Include(t => t.User)
                .ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CoachViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userHelper.GetUserByIdAsync(model.User.Id);
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.User.FirstName,
                        LastName = model.User.LastName,
                        PhoneNumber = model.User.PhoneNumber,
                        BirhtDate = model.User.BirhtDate,
                        EnrollmentNumber = model.User.EnrollmentNumber,
                        Email = model.User.Email,
                        UserName = model.User.Email
                    };
                    var result = await userHelper.AddUserAsync(user, "123456");
                    await userHelper.AddUserToRoleAsync(user, "Coach");
                    if (result == IdentityResult.Success)
                    {
                        var coach = new Coach
                        {
                            Id = model.Id,
                            Salary = model.Salary,
                            Expertise = model.Expertise,
                            ImageUrl = (model.ImageFile != null ? await imageHelper.UploadImageAsync(
                        model.ImageFile,
                        model.User.FullName,
                        "coaches") : string.Empty),
                            User = await this.dataContext.Users.FindAsync(user.Id)
                        };
                        dataContext.Add(coach);
                        await dataContext.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError(string.Empty, "El email ingresado no está disponible");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await this.dataContext.Coaches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await this.dataContext.Coaches
                .Include(u => u.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (coach == null)
            {
                return NotFound();
            }

            var model = new CoachViewModel
            {
                Id = coach.Id,
                Salary = coach.Salary,
                Expertise = coach.Expertise,
                ImageUrl = coach.ImageUrl,
                User = coach.User
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CoachViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this.dataContext.Users.FindAsync(model.User.Id);
                user.FirstName = model.User.FirstName;
                user.LastName = model.User.LastName;
                user.PhoneNumber = model.User.PhoneNumber;
                user.BirhtDate = model.User.BirhtDate;
                user.EnrollmentNumber = model.User.EnrollmentNumber;
                user.Email = model.User.Email;
                user.UserName = model.User.Email;

                this.dataContext.Update(user);
                var result = await userHelper.UpdateUserAsync(user);
                await dataContext.SaveChangesAsync();

                if (result == IdentityResult.Success)
                {
                    var coach = new Coach
                    {
                        Id = model.Id,
                        Salary = model.Salary,
                        Expertise = model.Expertise,
                        ImageUrl = (model.ImageFile != null ? await imageHelper.UploadImageAsync(
                                            model.ImageFile, "coach", "coaches") : model.ImageUrl),
                        User = await this.dataContext.Users.FindAsync(model.User.Id)
                    };
                    this.dataContext.Update(coach);
                    await dataContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "El email ingresado no está disponible");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await this.dataContext.Coaches
                .Include(u => u.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coach = await this.dataContext.Coaches
                .Include(u => u.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            this.dataContext.Coaches.Remove(coach);
            var user = await dataContext.Users.FindAsync(coach.User.Id);
            dataContext.Users.Remove(user);

            try
            {
                await dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "No se pueden eliminar registros");
            }
            return View(coach);
        }

        private bool CoachExists(int id)
        {
            return this.dataContext.Coaches.Any(e => e.Id == id);
        }
    }
}
