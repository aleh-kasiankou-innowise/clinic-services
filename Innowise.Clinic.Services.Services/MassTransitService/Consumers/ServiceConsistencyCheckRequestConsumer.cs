using Innowise.Clinic.Services.Persistence;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;

namespace Innowise.Clinic.Services.Services.MassTransitService.Consumers;

public class ServiceConsistencyCheckRequestConsumer : IConsumer<ServiceExistsAndBelongsToSpecializationRequest>
{
    private readonly ServicesDbContext _dbContext;

    public ServiceConsistencyCheckRequestConsumer(ServicesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ServiceExistsAndBelongsToSpecializationRequest> context)
    {
        var service = await _dbContext.Services.FindAsync(context.Message.ServiceId.ToString());

        if (service is not null)
        {
            if (service.SpecializationId != context.Message.SpecializationId)
            {
                await context.RespondAsync(
                    new ServiceExistsAndBelongsToSpecializationResponse(false,
                        "The service exists but belongs to a different specialization."));
            }

            else
            {
                await context.RespondAsync(
                    new ServiceExistsAndBelongsToSpecializationResponse(true, null));
            }
        }

        else
        {
            await context.RespondAsync(
                new ServiceExistsAndBelongsToSpecializationResponse(false, "The requested service does not exist."));
        }
    }
}