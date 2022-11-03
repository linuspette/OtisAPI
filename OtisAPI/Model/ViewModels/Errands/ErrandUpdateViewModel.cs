namespace OtisAPI.Model.ViewModels.Errands;

public class ErrandUpdateViewModel
{
    public string Status { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime DateOfUpdate { get; set; }
}