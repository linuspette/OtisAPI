using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtisAPI.Model.DataEntities.Errands;

[Table("ErrandUpdates")]
public class ErrandUpdateEntity
{
    [Key] public Guid Id { get; set; }
    [Required, Column(TypeName = "nvarchar(50)")] public string Status { get; set; } = null!;
    [Required, Column(TypeName = "nvarchar(2000)")] public string Message { get; set; } = null!;
    public DateTime DateOfUpdate { get; set; } = DateTime.UtcNow;

}