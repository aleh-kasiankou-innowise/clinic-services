using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Services.Dto;

public record SpecializationDto([Required] string Name, [Required] bool IsActive);

public record SpecializationWithServicesDto([Required] string Name, [Required] bool IsActive,
    [Required] IEnumerable<ServiceDto> relatedServices) : SpecializationDto(Name,
    IsActive);