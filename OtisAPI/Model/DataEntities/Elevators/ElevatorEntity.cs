using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtisAPI.Model.DataEntities.Elevators;

[Table("Elevators")]
public class ElevatorEntity
{
    [Key] public Guid Id { get; set; }
}