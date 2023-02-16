using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Services.ServiceService.Interfaces;

namespace Innowise.Clinic.Services.Services.ServiceService.Implementations;

public class ServiceService : IServiceService
{
    public Task<IEnumerable<ServiceDto>> GetServicesAsync(bool isFilterByActiveStatus)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceDto> GetServiceAsyncInfoAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateServiceAsync(ServiceDto newServiceDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateServiceAsync(Guid id, ServiceEditStatusDto updatedStatusDto)
    {
        throw new NotImplementedException();
    }
}