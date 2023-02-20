using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Persistence.Models;

namespace Innowise.Clinic.Services.Services.SpecializationService.Interfaces;

public interface ISpecializationService
{
    Task<IEnumerable<Specialization>> GetSpecializationsAsync(bool isFilterByActiveStatus);

    Task<SpecializationWithServicesDto> GetSpecializationInfoAsync(Guid id);

    Task<Guid> CreateSpecializationAsync(SpecializationDto newServiceDto);

    Task UpdateSpecializationAsync(Guid id, SpecializationEditStatusDto updatedStatusDto);
}