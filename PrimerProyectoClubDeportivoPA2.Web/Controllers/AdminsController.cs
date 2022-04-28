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
    
    [Authorize(Roles="Admin")]
    public class AdminsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;

        public AdminsController(DataContext dataContext,
            IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await dataContext.Admins
                .Include(t => t.User)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminViewModel model)
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
                    await userHelper.AddUserToRoleAsync(user, "Admin");
                    if (result == IdentityResult.Success)
                    {
                        var admin = new Admin
                        {
                            Id = model.Id,
                            Salary = model.Salary,
                            User = await this.dataContext.Users.FindAsync(user.Id)
                        };
                        dataContext.Add(admin);
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

            var admin = await this.dataContext.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await dataContext.Admins
                .Include(u => u.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
            {
                return NotFound();
            }

            var model = new AdminViewModel
            {
                Id = admin.Id,
                Salary = admin.Salary,
                User = admin.User
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminViewModel model)
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
                    var admin = new Admin
                    {
                        Id = model.Id,
                        Salary = model.Salary,
                        User = await this.dataContext.Users.FindAsync(model.User.Id)
                    };

                    this.dataContext.Update(admin);
                    await dataContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "El email ingresado no está disponible");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await this.dataContext.Admins
                .Include(u => u.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin = await this.dataContext.Admins
                .Include(u => u.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            this.dataContext.Admins.Remove(admin);
            var user = await dataContext.Users.FindAsync(admin.User.Id);
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
            return View(admin);
        }

        private bool AdminExists(int id)
        {
            return this.dataContext.Admins.Any(e => e.Id == id);
        }
    }
}
