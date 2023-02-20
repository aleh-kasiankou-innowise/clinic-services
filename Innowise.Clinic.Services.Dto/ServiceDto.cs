using System.ComponentModel.DataAnnotations;
using Innowise.Clinic.Services.Persistence.Models;

namespace Innowise.Clinic.Services.Dto;

public record ServiceDto([Required] string Name, [Required] decimal Price, [Required] ServiceCategory Category,
    [Required] Guid SpecializationId, [Required] bool IsActive);
    