using System.ComponentModel.DataAnnotations;

namespace HrCrm.Models;

public class Position
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    [Display(Name = "Должность")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Оклад")]
    public decimal? BaseSalary { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
