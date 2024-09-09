using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebProjectManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebProjectManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Действие за показване на страницата за вход
        public IActionResult Login()
        {
            var username = HttpContext.Session.GetString("Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model)
        {
            // Търсене на потребителя в базата данни
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // Проверка на хешираната парола
            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(model, user.PasswordHash, model.PasswordHash);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // Съхраняване на информацията за потребителя в сесията
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("RoleType", user.RoleType);

            return RedirectToAction("Index", "Projects");
        }



        // Действие за показване на страницата за регистрация
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (true)
            {
                var existingUser = await _context.Users.AnyAsync(u => u.Username == model.Username);
                if (!existingUser)
                {
                    var passwordHasher = new PasswordHasher<User>();
                    model.PasswordHash = passwordHasher.HashPassword(model, model.PasswordHash);
                    model.CreatedAt = DateTime.Now;
                    model.UpdatedAt = DateTime.Now;

                    _context.Users.Add(model);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("Username", model.Username);
                    HttpContext.Session.SetString("RoleType", model.RoleType);

                    return RedirectToAction("Index", "Projects");
                }
                ModelState.AddModelError("", "Username already exists.");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;

            if (string.IsNullOrEmpty(username))
            {
                ViewData["ErrorMessage"] = "You are not logged in. Please log in to view your profile.";
                return View("ProfileNotLoggedIn");
            }

            // Намери текущия потребител
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Прехвърляме потребителя към изгледа
            return View(user);
        }
    }

}
