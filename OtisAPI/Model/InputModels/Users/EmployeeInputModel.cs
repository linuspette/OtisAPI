using System.ComponentModel.DataAnnotations;

namespace OtisAPI.Model.InputModels.Users;

public class EmployeeInputModel
{
    [Range(0, 99999)]
    public int EmployeeNumber { get; set; }
    public string FullName { get; set; } = null!;
}