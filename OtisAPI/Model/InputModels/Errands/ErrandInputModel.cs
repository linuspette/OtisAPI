using OtisAPI.Infrastructure;

namespace OtisAPI.Model.InputModels.Errands;

public class ErrandInputModel
{
    public string ErrandNumber { get; set; } = ErrandNumberGenerator.GenerateErrandNumber();
    public string Title { get; set; } = null!;
    public Guid ElevatorId { get; set; }
    public ErrandUpdateCreationModel ErrandUpdates { get; set; } = null!;
    public bool IsResolved { get; set; } = false;
}