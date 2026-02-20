using Microsoft.EntityFrameworkCore;
using HrCrm.Models;

namespace HrCrm.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Position> Positions => Set<Position>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Администрация" },
            new Department { Id = 2, Name = "Производство" },
            new Department { Id = 3, Name = "Продажи" }
        );

        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Title = "Директор", BaseSalary = 150000 },
            new Position { Id = 2, Title = "Менеджер", BaseSalary = 80000 },
            new Position { Id = 3, Title = "Инженер", BaseSalary = 100000 }
        );
    }
}
