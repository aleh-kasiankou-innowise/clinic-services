using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Persistence.Models;

namespace Innowise.Clinic.Services.Services.ServiceService.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<Service>> GetServicesAsync(bool isFilterByActiveStatus);

    Task<ServiceDto> GetServiceAsyncInfoAsync(Guid id);

    Task<Guid> CreateServiceAsync(ServiceDto newServiceDto);

    Task UpdateServiceAsync(Guid id, ServiceEditStatusDto updatedStatusDto);
}