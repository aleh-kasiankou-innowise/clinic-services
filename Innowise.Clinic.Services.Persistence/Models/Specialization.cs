using System.Text.Json.Serialization;

namespace Innowise.Clinic.Services.Persistence.Models;

public class Specialization
{
    public Guid SpecializationId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    [JsonIgnore]
    public virtual IEnumerable<Service> Services { get; set; }
}