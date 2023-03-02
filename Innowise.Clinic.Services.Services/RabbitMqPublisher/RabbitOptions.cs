namespace Innowise.Clinic.Services.Services.RabbitMqPublisher;

public class RabbitOptions
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string SpecializationChangeRoutingKey { get; set; }
    public string ServicesProfilesExchangeName { get; set; }
}