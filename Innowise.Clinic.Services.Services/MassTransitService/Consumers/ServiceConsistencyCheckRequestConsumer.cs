using Innowise.Clinic.Services.Persistence;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Innowise.Clinic.Services.Services.MassTransitService.Consumers;

public class ServiceConsistencyCheckRequestConsumer : IConsumer<ServiceExistsAndBelongsToSpecializationRequest>
{
    private readonly ServicesDbContext _dbContext;
    private readonly ILogger<ServiceConsistencyCheckRequestConsumer> _logger;

    public ServiceConsistencyCheckRequestConsumer(ServicesDbContext dbContext,
        ILogger<ServiceConsistencyCheckRequestConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ServiceExistsAndBelongsToSpecializationRequest> context)
    {
        var service = await _dbContext.Services.FindAsync(context.Message.ServiceId);

        try
        {
            if (service is not null)
            {
                if (service.SpecializationId != context.Message.SpecializationId)
                {
                    var message = "The service exists but belongs to a different specialization.";
                    _logger.LogInformation(
                        "{baseMessage} Expected Specialization: {ExpectedSpecialization}, Actual Specialization: {ActualSpecialization}",
                        message, context.Message.SpecializationId, service.SpecializationId);
                    await context.RespondAsync<ServiceExistsAndBelongsToSpecializationResponse>(
                        new(false,
                            message));
                }

                else
                {
                    _logger.LogInformation("The service belongs to the expected specialization");
                    await context.RespondAsync<ServiceExistsAndBelongsToSpecializationResponse>(
                        new(true, null));
                }
            }

            else
            {
                await context.RespondAsync(
                    new ServiceExistsAndBelongsToSpecializationResponse(false,
                        "The requested service does not exist."));
            }
        }
        finally
        {
            _logger.LogInformation(
                "Consistency check for Service with id: {ServiceId} and Specialization with id {SpecializationId} finished",
                context.Message.ServiceId, context.Message.SpecializationId);
        }
    }
}