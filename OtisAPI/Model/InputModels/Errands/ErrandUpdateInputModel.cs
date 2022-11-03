using OtisAPI.Model.ViewModels.Users;

namespace OtisAPI.Model.InputModels.Errands;

public class ErrandUpdateInputModel
{
    public string ErrandNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Message { get; set; } = null!;
    public bool IsResolved { get; set; }
    public List<EmployeeViewModel>? Employees { get; set; }
}