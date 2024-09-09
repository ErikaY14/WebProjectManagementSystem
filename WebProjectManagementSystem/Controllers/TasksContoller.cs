using Microsoft.AspNetCore.Mvc;
using WebProjectManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using System;

public class TasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Tasks/ExportToExcel
    public async Task<IActionResult> ExportToExcel(int projectId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Tasks");
            worksheet.Cells["A1"].Value = "Title";
            worksheet.Cells["B1"].Value = "Description";
            worksheet.Cells["C1"].Value = "Assigned To";
            worksheet.Cells["D1"].Value = "Status";
            worksheet.Cells["E1"].Value = "Created At";
            worksheet.Cells["F1"].Value = "Updated At";

            var rowIndex = 2;
            foreach (var task in tasks)
            {
                worksheet.Cells[rowIndex, 1].Value = task.Title;
                worksheet.Cells[rowIndex, 2].Value = task.Description;
                worksheet.Cells[rowIndex, 3].Value = task.AssignedTo;
                worksheet.Cells[rowIndex, 4].Value = task.Status;
                worksheet.Cells[rowIndex, 5].Value = task.CreatedAt.ToShortDateString();
                worksheet.Cells[rowIndex, 6].Value = task.UpdatedAt.ToShortDateString();
                rowIndex++;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);

            var fileName = $"Tasks_Project_{projectId}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }

    // GET: Tasks/Create
    public IActionResult Create(int projectId)
    {
        var username = HttpContext.Session.GetString("Username");
        ViewBag.Username = username;

        ViewData["ProjectId"] = projectId;
        return View();
    }

    // POST: Tasks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WebProjectManagementSystem.Models.Task task)
    {
        if (true)
        {
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;
            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Tasks", new { projectId = task.ProjectId });
        }
        return View(task);
    }

    public async Task<IActionResult> Index(string titleFilter, string statusFilter, int assignedToFilter, int? projectId)
    {
        var username = HttpContext.Session.GetString("Username");
        ViewBag.Username = username;

        if (!projectId.HasValue)
        {
            return NotFound();
        }

        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
        {
            return NotFound();
        }

        // Зареждаме потребителите, които ще бъдат използвани в падащото меню
        var users = await _context.Users.ToListAsync();
        ViewBag.Users = users;
        ViewData["ProjectName"] = project.Name;

        var tasks = _context.Tasks.AsQueryable();

        tasks = tasks.Where(t => t.ProjectId == projectId.Value);

        if (!string.IsNullOrEmpty(titleFilter))
        {
            tasks = tasks.Where(t => t.Title.Contains(titleFilter));
        }

        if (!string.IsNullOrEmpty(statusFilter))
        {
            tasks = tasks.Where(t => t.Status == statusFilter);
        }

        if (assignedToFilter > 0)
        {
            tasks = tasks.Where(t => t.AssignedTo == assignedToFilter);
        }

        var model = await tasks.ToListAsync();

        ViewData["ProjectId"] = projectId;

        return View(model);
    }

    // GET: Tasks/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        var username = HttpContext.Session.GetString("Username");
        ViewBag.Username = username;

        if (id == null)
        {
            return NotFound();
        }

        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    // POST: Tasks/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] WebProjectManagementSystem.Models.Task task)
    {
        if (id != task.Id)
        {
            return NotFound();
        }

        if (true)
        {
            try
            {
                // Изтегляне на съществуващата задача
                var existingTask = await _context.Tasks.FindAsync(id);

                if (existingTask == null)
                {
                    return NotFound();
                }

                // Актуализиране само на статуса
                existingTask.Status = task.Status;
                existingTask.UpdatedAt = DateTime.Now;

                _context.Update(existingTask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var currentTask = await _context.Tasks.FindAsync(id);
            // Предава ProjectId, ако е наличен в TempData или ViewData
            int? projectId = currentTask.ProjectId;
            return RedirectToAction(nameof(Index), new { projectId });
        }

        // Подаване на ProjectId към изгледа
        ViewData["ProjectId"] = HttpContext.Session.GetInt32("ProjectId");
        return View(task);
    }




    // GET: Tasks/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        var username = HttpContext.Session.GetString("Username");
        ViewBag.Username = username;

        if (id == null)
        {
            return NotFound();
        }

        var task = await _context.Tasks
            .FirstOrDefaultAsync(m => m.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    // POST: Tasks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", new { projectId = task.ProjectId });
    }

    private bool TaskExists(int id)
    {
        return _context.Tasks.Any(e => e.Id == id);
    }

}
