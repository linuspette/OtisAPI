using Microsoft.EntityFrameworkCore;
using OtisAPI.Model.DataEntities.Errands;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtisAPI.Model.DataEntities.Users;

[Table("Employees")]
[Index(nameof(EmployeeNumber), IsUnique = true)]
public class EmployeeEntity
{
    [Key, Required] public Guid Id { get; set; }
    [Required] public int EmployeeNumber { get; set; }
    [Required, Column(TypeName = "nvarchar(50)")] public string FullName { get; set; } = null!;
    [Required] public List<ErrandEntity> AssignedErrands { get; set; } = null!;
}