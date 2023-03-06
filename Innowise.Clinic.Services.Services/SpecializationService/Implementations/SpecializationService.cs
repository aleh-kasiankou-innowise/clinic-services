using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Exceptions;
using Innowise.Clinic.Services.Persistence;
using Innowise.Clinic.Services.Persistence.Models;
using Innowise.Clinic.Services.Services.SpecializationService.Interfaces;
using Innowise.Clinic.Shared.Enums;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SpecializationDto = Innowise.Clinic.Services.Dto.SpecializationDto;

namespace Innowise.Clinic.Services.Services.SpecializationService.Implementations;

public class SpecializationService : ISpecializationService
{
    private readonly ServicesDbContext _dbContext;
    private readonly IBus _bus;

    public SpecializationService(ServicesDbContext dbContext, IBus bus)
    {
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task<IEnumerable<Specialization>> GetSpecializationsAsync(bool isFilterByActiveStatus)
    {
        var dbSetQuery = _dbContext.Specializations.AsQueryable();

        if (isFilterByActiveStatus)
        {
            dbSetQuery = dbSetQuery
                .Where(s => s.IsActive);
        }

        return await dbSetQuery
            .ToListAsync();
    }

    public async Task<SpecializationWithServicesDto> GetSpecializationInfoAsync(Guid id)
    {
        var specialization = await GetSpecializationByIdAsync(id);
        var serviceCollection = specialization.Services.Select(x =>
            new ServiceDto(x.Name, x.Price, x.Category, x.SpecializationId, x.IsActive));
        return new SpecializationWithServicesDto(specialization.Name, specialization.IsActive, serviceCollection);
    }

    public async Task<Guid> CreateSpecializationAsync(SpecializationDto newServiceDto)
    {
        var specialization = new Specialization
        {
            Name = newServiceDto.Name,
            IsActive = newServiceDto.IsActive,
            Services = new List<Service>()
        };

        _dbContext.Add(specialization);
        await _dbContext.SaveChangesAsync();

        await _bus.Publish(
            new SpecializationUpdatedMessage(SpecializationChange.Add,
                new Shared.Dto.SpecializationDto(specialization.SpecializationId, specialization.Name))
        );
        return specialization.SpecializationId;
    }

    public async Task UpdateSpecializationAsync(Guid id, SpecializationEditStatusDto updatedSpecializationEditStatusDto)
    {
        var specialization = await GetSpecializationByIdAsync(id);
        specialization.IsActive = updatedSpecializationEditStatusDto.IsActive;
        bool specializationNameChanged = false;

        if (updatedSpecializationEditStatusDto is SpecializationEditAllFieldsDto editAllFieldsDto)
        {
            specializationNameChanged = specialization.Name != editAllFieldsDto.Name;
            specialization.Name = editAllFieldsDto.Name;
            specialization.IsActive = editAllFieldsDto.IsActive;
        }

        _dbContext.Update(specialization);
        await _dbContext.SaveChangesAsync();

        if (specializationNameChanged)
        {
            await _bus.Publish(
                new SpecializationUpdatedMessage(SpecializationChange.Update,
                    new Shared.Dto.SpecializationDto(specialization.SpecializationId, specialization.Name))
            );
        }
    }

    private async Task<Specialization> GetSpecializationByIdAsync(Guid id)
    {
        var service = await _dbContext.Specializations.Include(x => x.Services)
            .FirstOrDefaultAsync(x => x.SpecializationId == id);
        if (service == null)
            throw new EntityNotFoundException("The specialization with the specified id doesn't exist.");
        return service;
    }
}