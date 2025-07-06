using Application.DTO;
using Application.IPublishers;
using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;

public class CollaboratorWithoutUserCreatedConsumer : IConsumer<CollaboratorWithoutUserCreatedMessage>
{
    private readonly IUserService _userService;
    private readonly IMessagePublisher _publisher;

    public CollaboratorWithoutUserCreatedConsumer(IUserService userService, IMessagePublisher publisher)
    {
        _userService = userService;
        _publisher = publisher;
    }

    public async Task Consume(ConsumeContext<CollaboratorWithoutUserCreatedMessage> context)
    {
        var msg = context.Message;

        var userDTO = new UserDTO
        {
            Names = msg.Names,
            Surnames = msg.Surnames,
            Email = msg.Email,
            FinalDate = msg.FinalDate
        };

        await _userService.Add(userDTO);
    }
}