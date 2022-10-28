using OtisAPI.Model.DataEntities.Errands;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtisAPI.Model.DataEntities.Elevators;

[Table("Elevators")]
public class ElevatorEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string Location { get; set; } = null!;
    [Required] public List<ErrandEntity> Errands { get; set; } = null!;
}