using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Persistence;
using Innowise.Clinic.Services.Persistence.Models;
using Innowise.Clinic.Services.Services.ServiceService.Interfaces;
using Innowise.Clinic.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Services.Services.ServiceService.Implementations;

public class ServiceService : IServiceService
{
    private readonly ServicesDbContext _dbContext;

    public ServiceService(ServicesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Service>> GetServicesAsync(bool isFilterByActiveStatus)
    {
        var dbSetQuery = _dbContext.Services.AsQueryable<Service>();

        if (isFilterByActiveStatus)
        {
            dbSetQuery = dbSetQuery.Where(s => s.IsActive);
        }

        return await dbSetQuery.ToListAsync();
    }

    public async Task<ServiceDto> GetServiceInfoAsync(Guid id)
    {
        var service = await GetServiceById(id);
        return new ServiceDto(service.Name, service.Price, service.Category, service.SpecializationId,
            service.IsActive);
    }

    public async Task<Guid> CreateServiceAsync(ServiceDto newServiceDto)
    {
        var newService = new Service
        {
            Name = newServiceDto.Name,
            Price = newServiceDto.Price,
            Category = newServiceDto.Category,
            SpecializationId = newServiceDto.SpecializationId,
            IsActive = newServiceDto.IsActive
        };

        _dbContext.Services.Add(newService);
        await _dbContext.SaveChangesAsync();
        return newService.ServiceId;
    }

    public async Task UpdateServiceAsync(Guid id, ServiceEditStatusDto serviceUpdateDto)
    {
        var service = await GetServiceById(id);

        service.IsActive = serviceUpdateDto.IsActive;

        if (serviceUpdateDto is ServiceEditAllFieldsDto updateWithAllFields)
        {
            service.Name = updateWithAllFields.Name;
            service.Price = updateWithAllFields.Price;
            service.Category = updateWithAllFields.Category;
        }

        _dbContext.Update(service);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Service> GetServiceById(Guid id)
    {
        var service = await _dbContext.Services.FirstOrDefaultAsync(x => x.ServiceId == id);
        if (service == null)
        {
            throw new EntityNotFoundException("The service with the specified id doesn't exist.");
        }

        return service;
    }
}