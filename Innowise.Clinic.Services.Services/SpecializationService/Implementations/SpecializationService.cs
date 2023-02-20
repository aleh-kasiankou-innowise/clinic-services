using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Exceptions;
using Innowise.Clinic.Services.Persistence;
using Innowise.Clinic.Services.Persistence.Models;
using Innowise.Clinic.Services.Services.SpecializationService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Services.Services.SpecializationService.Implementations;

public class SpecializationService : ISpecializationService
{
    private readonly ServicesDbContext _dbContext;

    public SpecializationService(ServicesDbContext dbContext)
    {
        _dbContext = dbContext;
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

        return specialization.SpecializationId;
    }

    public async Task UpdateSpecializationAsync(Guid id, SpecializationEditStatusDto updatedSpecializationEditStatusDto)
    {
        var specialization = await GetSpecializationByIdAsync(id);
        specialization.IsActive = updatedSpecializationEditStatusDto.IsActive;

        if (updatedSpecializationEditStatusDto is SpecializationEditAllFieldsDto editAllFieldsDto)
        {
            specialization.Name = editAllFieldsDto.Name;
            specialization.IsActive = editAllFieldsDto.IsActive;
        }

        _dbContext.Update(specialization);
        await _dbContext.SaveChangesAsync();
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