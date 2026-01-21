using AdZZiM.Data;
using AdZZiM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdZZiM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .GroupJoin(
                    _context.AccDates,
                    user => user.Id,
                    profile => profile.UserId,
                    (user, profiles) => new { User = user, Profile = profiles.FirstOrDefault() }
                )
                .Select(x => new UserList
                {
                    Id = x.User.Id,
                    Email = x.User.Email,
                    UserName = x.User.UserName,
                    RegistrationDate = x.Profile != null ? x.Profile.RegistrationDate : DateTime.MinValue
                })
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();
            var dateEntry = await _context.AccDates.FirstOrDefaultAsync(d => d.UserId == id);
            var model = new UserDetails
            {
                User = user,
                RegistrationDate = dateEntry != null ? dateEntry.RegistrationDate : DateTime.MinValue
            };

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var profile = new AccDate
                    {
                        UserId = user.Id,
                        RegistrationDate = DateTime.Now
                    };
                    _context.AccDates.Add(profile);
                    await _context.SaveChangesAsync();
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}