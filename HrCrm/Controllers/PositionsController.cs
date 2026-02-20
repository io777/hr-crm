using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrCrm.Data;
using HrCrm.Models;

namespace HrCrm.Controllers;

public class PositionsController : Controller
{
    private readonly AppDbContext _db;

    public PositionsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var positions = await _db.Positions
            .Include(p => p.Employees)
            .OrderBy(p => p.Title)
            .ToListAsync();
        return View(positions);
    }

    public IActionResult Create() => View(new Position());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Position position)
    {
        if (!ModelState.IsValid) return View(position);

        _db.Positions.Add(position);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var pos = await _db.Positions.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == id);
        if (pos == null) return NotFound();
        if (pos.Employees.Any())
        {
            TempData["Error"] = "Нельзя удалить должность с сотрудниками";
            return RedirectToAction(nameof(Index));
        }

        _db.Positions.Remove(pos);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
