using OtisAPI.Model.ViewModels.Errands;

namespace OtisAPI.Model.ViewModels.Users;

public class EmployeeViewModel
{
    public int EmployeeNumber { get; set; }
    public string FullName { get; set; } = null!;
    public List<ErrandViewModel>? ErrandViewModels { get; set; }
}