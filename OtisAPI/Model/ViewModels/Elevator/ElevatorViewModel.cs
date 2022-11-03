using OtisAPI.Model.ViewModels.Errands;

namespace OtisAPI.Model.ViewModels.Elevator;

public class ElevatorViewModel
{
    public Guid Id { get; set; }
    public string Location { get; set; } = null!;
    public List<ErrandViewModel> Errands { get; set; } = null!;
}