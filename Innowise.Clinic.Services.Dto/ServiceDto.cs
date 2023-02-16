using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Services.Dto;

public record ServiceDto([Required] string Name, [Required] decimal Price, [Required] Guid CategoryId,
    [Required] Guid SpecializationId, [Required] bool Status);
    