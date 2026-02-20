using System.ComponentModel.DataAnnotations;

namespace HrCrm.Models;

public class Employee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна")]
    [Display(Name = "Фамилия")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имя обязательно")]
    [Display(Name = "Имя")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Отчество")]
    [StringLength(100)]
    public string? MiddleName { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string? Email { get; set; }

    [Display(Name = "Телефон")]
    [Phone(ErrorMessage = "Некорректный телефон")]
    public string? Phone { get; set; }

    [Display(Name = "Дата рождения")]
    [DataType(DataType.Date)]
    public DateOnly? BirthDate { get; set; }

    [Display(Name = "Дата приёма")]
    [DataType(DataType.Date)]
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Дата увольнения")]
    [DataType(DataType.Date)]
    public DateOnly? TerminationDate { get; set; }

    [Display(Name = "Отдел")]
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    [Display(Name = "Должность")]
    public int? PositionId { get; set; }
    public Position? Position { get; set; }

    [Display(Name = "ФИО")]
    public string FullName => $"{LastName} {FirstName} {MiddleName}".TrimEnd();

    [Display(Name = "Активен")]
    public bool IsActive => TerminationDate == null;
}
