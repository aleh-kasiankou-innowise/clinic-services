using System.Text.Json.Serialization;

namespace Innowise.Clinic.Services.Persistence.Models;

public class Service
{
    public Guid ServiceId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ServiceCategory Category { get; set; }
    public bool IsActive { get; set; }
    public Guid SpecializationId { get; set; }
    [JsonIgnore] public virtual Specialization Specialization { get; set; }
}