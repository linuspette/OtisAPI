using System.ComponentModel.DataAnnotations;

namespace OtisAPI.Model.DataEntities;

public class ErrandEntity
{
    [Key]
    public Guid Id { get; set; }
}