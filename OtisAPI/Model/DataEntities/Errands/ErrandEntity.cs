using Microsoft.EntityFrameworkCore;
using OtisAPI.Model.DataEntities.Elevators;
using OtisAPI.Model.DataEntities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtisAPI.Model.DataEntities.Errands;

[Table("Errands")]
[Index(nameof(ErrandNumber), IsUnique = true)]
public class ErrandEntity
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [Required, Column(TypeName = "nvarchar(18)")] public string ErrandNumber { get; set; } = null!;
    [Required, Column(TypeName = "nvarchar(99)")] public string Title { get; set; } = null!;
    [Required] public ElevatorEntity Elevator { get; set; } = null!;
    [Required] public List<ErrandUpdateEntity> ErrandUpdates { get; set; } = null!;
    [Required] public List<EmployeeEntity> AssignedTechnicians { get; set; } = null!;
    [Required] public bool IsResolved { get; set; } = false;
}