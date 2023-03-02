using Innowise.Clinic.Services.Dto.RabbitMq;

namespace Innowise.Clinic.Services.Services.RabbitMqPublisher;

public interface IRabbitMqPublisher
{
    void NotifyAboutSpecializationChange(SpecializationChangeTaskDto specializationChangeTask);
}