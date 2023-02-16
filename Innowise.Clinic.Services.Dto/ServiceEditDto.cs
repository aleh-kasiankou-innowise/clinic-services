using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Services.Dto;

public record ServiceEditStatusDto([Required] bool Status);

public record ServiceEditAllFieldsDto([Required] string Name, [Required] decimal Price, [Required] Guid CategoryId,
    [Required] bool Status) : ServiceEditStatusDto(Status);