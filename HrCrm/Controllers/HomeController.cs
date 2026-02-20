using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrCrm.Data;

namespace HrCrm.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.EmployeeCount = await _db.Employees.CountAsync();
        ViewBag.ActiveCount = await _db.Employees.CountAsync(e => e.TerminationDate == null);
        ViewBag.DepartmentCount = await _db.Departments.CountAsync();
        return View();
    }
}
