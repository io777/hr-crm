using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrCrm.Data;
using HrCrm.Models;

namespace HrCrm.Controllers;

public class DepartmentsController : Controller
{
    private readonly AppDbContext _db;

    public DepartmentsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var departments = await _db.Departments
            .Include(d => d.Employees)
            .OrderBy(d => d.Name)
            .ToListAsync();
        return View(departments);
    }

    public IActionResult Create() => View(new Department());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department)
    {
        if (!ModelState.IsValid) return View(department);

        _db.Departments.Add(department);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var dept = await _db.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);
        if (dept == null) return NotFound();
        if (dept.Employees.Any())
        {
            TempData["Error"] = "Нельзя удалить отдел с сотрудниками";
            return RedirectToAction(nameof(Index));
        }

        _db.Departments.Remove(dept);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
