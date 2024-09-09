using Microsoft.AspNetCore.Mvc;
using WebProjectManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Projects/Create
    public IActionResult Create()
    {
        var username = HttpContext.Session.GetString("Username");
        ViewBag.Username = username;

        return View();
    }

    // POST: Projects/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {

        if (true)
        {
            project.CreatedAt = DateTime.Now;
            project.UpdatedAt = DateTime.Now;
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    // GET: Projects/Index
    public async Task<IActionResult> Index(string name, DateTime? startDate, DateTime? endDate, string status)
    {
        var username = HttpContext.Session.GetString("Username");
        ViewBag.Username = username;

        // Проверка дали потребителят е влязъл в системата
        if (string.IsNullOrEmpty(username))
        {
            ViewData["ErrorMessage"] = "You are not logged in. Please log in to view the projects.";
            return View("ProjectsNotLoggedIn");
        }

        // Намери текущия потребител
        var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (currentUser == null)
        {
            return Unauthorized();
        }

        var projects = _context.Projects.AsQueryable();

        // Ако потребителят е 'User', филтрирайте проектите, към които е свързан
        if (currentUser.RoleType == "User")
        {
            projects = from p in _context.Projects
                       join pu in _context.ProjectUsers on p.Id equals pu.ProjectId
                       where pu.UserId == currentUser.Id
                       select p;
        }

        // Добавяне на съществуващите филтри
        if (!string.IsNullOrEmpty(name))
        {
            projects = projects.Where(p => p.Name.Contains(name));
        }

        if (startDate.HasValue)
        {
            projects = projects.Where(p => p.StartDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            projects = projects.Where(p => p.EndDate <= endDate.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            projects = projects.Where(p => p.Status == status);
        }

        return View(await projects.ToListAsync());
    }




    // GET: Projects/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        var username = HttpContext.Session.GetString("Username");
        ViewBag.Username = username;

        if (id == null)
        {
            return NotFound();
        }

        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // POST: Projects/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name, Description, StartDate, EndDate, Status")] Project project)
    {
        if (id != project.Id)
        {
            return NotFound();
        }

        if (true)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    // GET: Projects/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        var username = HttpContext.Session.GetString("Username");
        ViewBag.Username = username;

        if (id == null)
        {
            return NotFound();
        }

        var project = await _context.Projects
            .FirstOrDefaultAsync(m => m.Id == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // POST: Projects/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
 
    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.Id == id);
    }

}
