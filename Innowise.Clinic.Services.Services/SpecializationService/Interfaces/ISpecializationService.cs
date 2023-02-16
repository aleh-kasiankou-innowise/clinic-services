using Innowise.Clinic.Services.Dto;

namespace Innowise.Clinic.Services.Services.SpecializationService.Interfaces;

public interface ISpecializationService
{
    Task<IEnumerable<SpecializationDto>> GetSpecializationsAsync(bool isFilterByActiveStatus);

    Task<SpecializationWithServicesDto> GetSpecializationInfoAsync(Guid id);

    Task<Guid> CreateSpecializationAsync(SpecializationDto newServiceDto);

    Task UpdateSpecializationAsync(Guid id, SpecializationEditStatusDto updatedStatusDto);
}