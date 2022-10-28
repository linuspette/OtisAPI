using OtisAPI.Model.DataEntities.Elevators;
using OtisAPI.Model.DataEntities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtisAPI.Model.DataEntities.Errands;

[Table("Errands")]
public class ErrandEntity
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    [Required, StringLength(150)] public string Title { get; set; } = null!;
    [Required] public ElevatorEntity Elevator { get; set; } = null!;
    [Required] public List<ErrandUpdateEntity> ErrandUpdates { get; set; } = null!;
    [Required] public List<EmployeeEntity> AssignedTechnicians { get; set; } = null!;
    [Required] public bool IsResolved { get; set; } = false;
}