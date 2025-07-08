using Domain.Models;

namespace Application.IPublishers;

public interface IMessagePublisher
{
    Task PublishCreatedUserMessageAsync(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime);
    Task SendCreatedUserFromCollabMessageAsync(string InstanceId, Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime);
}
