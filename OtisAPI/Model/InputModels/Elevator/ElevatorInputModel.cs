using System.ComponentModel.DataAnnotations;

namespace OtisAPI.Model.InputModels.Elevator;

public class ElevatorInputModel
{
    [Required] public Guid Id { get; set; }
    [Required] public string Location { get; set; } = null!;
}