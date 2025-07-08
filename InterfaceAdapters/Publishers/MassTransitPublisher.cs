using Domain.Models;
using Domain.Messages;
using MassTransit;
using Application.IPublishers;

namespace InterfaceAdapters.Publishers;

public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpoint;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpoint)
    {
        _publishEndpoint = publishEndpoint;
        _sendEndpoint = sendEndpoint;

    }

    public async Task PublishCreatedUserMessageAsync(Guid id, string names, string surnames, string email, PeriodDateTime period)
    {
        await _publishEndpoint.Publish(new UserCreatedMessage(id, names, surnames, email, period));
    }

}
