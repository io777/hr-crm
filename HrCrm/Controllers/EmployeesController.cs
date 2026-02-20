using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HrCrm.Data;
using HrCrm.Models;

namespace HrCrm.Controllers;

public class EmployeesController : Controller
{
    private readonly AppDbContext _db;

    public EmployeesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search, int? departmentId)
    {
        var query = _db.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.LastName.Contains(search) ||
                e.FirstName.Contains(search) ||
                (e.Email != null && e.Email.Contains(search)));
            ViewBag.Search = search;
        }

        if (departmentId.HasValue)
        {
            query = query.Where(e => e.DepartmentId == departmentId);
            ViewBag.DepartmentId = departmentId;
        }

        ViewBag.Departments = new SelectList(
            await _db.Departments.OrderBy(d => d.Name).ToListAsync(),
            "Id", "Name", departmentId);

        return View(await query.OrderBy(e => e.LastName).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return View(new Employee());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns();
            return View(employee);
        }

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        await PopulateDropdowns();
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            await PopulateDropdowns();
            return View(employee);
        }

        _db.Update(employee);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee != null)
        {
            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns()
    {
        ViewBag.Departments = new SelectList(
            await _db.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
        ViewBag.Positions = new SelectList(
            await _db.Positions.OrderBy(p => p.Title).ToListAsync(), "Id", "Title");
    }
}
