using System.ComponentModel.DataAnnotations;
using Innowise.Clinic.Services.Persistence.Models;

namespace Innowise.Clinic.Services.Dto;

public record ServiceEditStatusDto([Required] bool IsActive);

public record ServiceEditAllFieldsDto([Required] string Name, [Required] decimal Price, [Required] ServiceCategory Category,
    [Required] bool IsActive) : ServiceEditStatusDto(IsActive);