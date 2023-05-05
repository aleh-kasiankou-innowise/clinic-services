using Innowise.Clinic.Services.Services.ServiceService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;

namespace Innowise.Clinic.Services.Services.MassTransitService.Consumers;

public class ServiceNameRequestConsumer : IConsumer<ServiceNameRequest>
{
    private readonly IServiceService _serviceService;

    public ServiceNameRequestConsumer(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task Consume(ConsumeContext<ServiceNameRequest> context)
    {
        var serviceInfo = await _serviceService.GetServiceInfoAsync(context.Message.ServiceId);
        await context.RespondAsync<ServiceNameResponse>(new(serviceInfo.Name));
    }
}