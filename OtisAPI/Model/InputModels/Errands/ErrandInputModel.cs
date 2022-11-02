using OtisAPI.Infrastructure;
using OtisAPI.Model.ViewModels.Elevator;

namespace OtisAPI.Model.InputModels.Errands;

public class ErrandInputModel
{
    public string ErrandNumber { get; set; } = ErrandNumberGenerator.GenerateErrandNumber();
    public string Title { get; set; } = null!;
    public ElevatorViewModel Elevator { get; set; } = null!;
    public List<ErrandUpdateCreationModel> ErrandUpdates { get; set; } = new List<ErrandUpdateCreationModel>();
    public bool IsResolved { get; set; } = false;
}