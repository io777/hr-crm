using System.ComponentModel.DataAnnotations;

namespace HrCrm.Models;

public class Department
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    [Display(Name = "Название")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Описание")]
    public string? Description { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
