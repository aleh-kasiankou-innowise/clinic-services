using Innowise.Clinic.Services.Dto;

namespace Innowise.Clinic.Services.Services.SpecializationService.Interfaces;

public class SpecializationService : ISpecializationService
{
    public Task<IEnumerable<SpecializationDto>> GetSpecializationsAsync(bool isFilterByActiveStatus)
    {
        throw new NotImplementedException();
    }

    public Task<SpecializationWithServicesDto> GetSpecializationInfoAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateSpecializationAsync(SpecializationDto newServiceDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSpecializationAsync(Guid id, SpecializationEditStatusDto updatedStatusDto)
    {
        throw new NotImplementedException();
    }
}