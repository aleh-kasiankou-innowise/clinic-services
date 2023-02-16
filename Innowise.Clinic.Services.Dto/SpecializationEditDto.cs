using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Services.Dto;

public record SpecializationEditStatusDto([Required] bool IsActive);

public record SpecializationEditAllFieldsDto([Required] string Name, [Required] bool IsActive) : SpecializationEditStatusDto(IsActive);