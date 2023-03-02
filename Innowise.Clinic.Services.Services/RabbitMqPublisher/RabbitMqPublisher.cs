using System.Text.Json;
using Innowise.Clinic.Services.Dto.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Innowise.Clinic.Services.Services.RabbitMqPublisher;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IModel _channel;
    private readonly RabbitOptions _rabbitOptions;

    public RabbitMqPublisher(IOptions<RabbitOptions> rabbitConfig)
    {
        _rabbitOptions = rabbitConfig.Value;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitOptions.HostName, UserName = _rabbitOptions.UserName, Password = _rabbitOptions.Password
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        DeclareServiceProfilesExchange();
    }

    public void NotifyAboutSpecializationChange(SpecializationChangeTaskDto specializationChangeTask)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(specializationChangeTask);
        _channel.BasicPublish(exchange: _rabbitOptions.ServicesProfilesExchangeName,
            routingKey: _rabbitOptions.SpecializationChangeRoutingKey,
            basicProperties: null,
            body: body);
    }

    private void DeclareServiceProfilesExchange()
    {
        _channel.ExchangeDeclare(exchange: _rabbitOptions.ServicesProfilesExchangeName,
            type: ExchangeType.Topic);
    }
}