namespace Innowise.Clinic.Services.Dto.RabbitMq;

public record SpecializationChangeTaskDto(SpecializationChange TaskType, SpecializationDto SpecializationDto);

public record SpecializationDto(Guid SpecializationId, string SpecializationName);