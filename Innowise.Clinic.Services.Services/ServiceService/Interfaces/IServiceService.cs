using Innowise.Clinic.Services.Dto;

namespace Innowise.Clinic.Services.Services.ServiceService.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<ServiceDto>> GetServicesAsync(bool isFilterByActiveStatus);

    Task<ServiceDto> GetServiceAsyncInfoAsync(Guid id);

    Task<Guid> CreateServiceAsync(ServiceDto newServiceDto);

    Task UpdateServiceAsync(Guid id, ServiceEditStatusDto updatedStatusDto);
}