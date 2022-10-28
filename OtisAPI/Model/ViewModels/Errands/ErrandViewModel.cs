using OtisAPI.Model.ViewModels.Users;

namespace OtisAPI.Model.ViewModels.Errands;

public class ErrandViewModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public ErrandViewModel Elevator { get; set; } = null!;
    public List<ErrandUpdateViewModel> ErrandUpdates { get; set; } = null!;
    public List<EmployeeViewModel> AssignedTechnicians { get; set; } = null!;
    public bool IsResolved { get; set; } = false;
}