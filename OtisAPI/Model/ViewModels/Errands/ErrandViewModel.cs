using OtisAPI.Model.ViewModels.Elevator;
using OtisAPI.Model.ViewModels.Users;

namespace OtisAPI.Model.ViewModels.Errands;

public class ErrandViewModel
{
    public Guid Id { get; set; }
    public string ErrandNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public ElevatorViewModel Elevator { get; set; } = null!;
    public List<ErrandUpdateViewModel> ErrandUpdates { get; set; } = null!;
    public List<EmployeeViewModel> AssignedTechnicians { get; set; } = null!;
    public bool IsResolved { get; set; }
}