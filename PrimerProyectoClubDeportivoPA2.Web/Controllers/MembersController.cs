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
    public class MembersController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;
        private readonly IImageHelper imageHelper;
        private readonly ICombosHelper combosHelper;

        public MembersController(DataContext dataContext, IImageHelper imageHelper, IUserHelper userHelper, ICombosHelper combosHelper)
        {
            this.dataContext = dataContext;
            this.imageHelper = imageHelper;
            this.userHelper = userHelper;
            this.combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await dataContext.Members
                .Include(u => u.User)
                .Include(m => m.MembershipType)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await this.dataContext.Members
                .Include(u => u.User)
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new MemberViewModel
            {
                MembershipTypes = this.combosHelper.GetComboMembershipTypes()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberViewModel model)
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
                    await userHelper.AddUserToRoleAsync(user, "Member");
                    if (result == IdentityResult.Success)
                    {
                        var member = new Member
                        {
                            Id = model.Id,
                            MembershipType = await this.dataContext.MembershipTypes.FindAsync(model.MembershipTypeId),
                            ImageUrl = (model.ImageFile != null ? await imageHelper.UploadImageAsync(
                         model.ImageFile,
                         model.User.FullName,
                         "members") : string.Empty),
                            User = await this.dataContext.Users.FindAsync(user.Id)
                        };
                        dataContext.Add(member);
                        await dataContext.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError(string.Empty, "El email ingresado no está disponible");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await this.dataContext.Members
                .Include(u => u.User)
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            var model = new MemberViewModel
            {
                Id = member.Id,
                MembershipType = member.MembershipType,
                MembershipTypeId = member.MembershipType.Id,
                MembershipTypes = this.combosHelper.GetComboMembershipTypes(),
                ImageUrl = member.ImageUrl,
                User = member.User
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MemberViewModel model)
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
                    var member = new Member
                    {
                        Id = model.Id,
                        MembershipType = await this.dataContext.MembershipTypes.FindAsync(model.MembershipTypeId),
                        ImageUrl = (model.ImageFile != null ? await imageHelper.UploadImageAsync(
                        model.ImageFile, "member","members") : model.ImageUrl),
                        User = await this.dataContext.Users.FindAsync(model.User.Id)
                    };

                    this.dataContext.Update(member);
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

            var member = await this.dataContext.Members
                .Include(u => u.User)
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await this.dataContext.Members
                .Include(u => u.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            this.dataContext.Members.Remove(member);
            var user = await dataContext.Users.FindAsync(member.User.Id);
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
            return View(member);
        }

        private bool MemberExists(int id)
        {
            return dataContext.Members.Any(e => e.Id == id);
        }
    }
}
