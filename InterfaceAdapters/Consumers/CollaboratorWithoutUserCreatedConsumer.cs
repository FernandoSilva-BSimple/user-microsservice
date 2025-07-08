using Application.DTO;
using Application.IPublishers;
using Domain.Commands;
using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;

public class CollaboratorWithoutUserCreatedConsumer : IConsumer<CreateUserFromCollaboratorCommand>
{
    private readonly IUserService _userService;
    private readonly IMessagePublisher _publisher;

    public CollaboratorWithoutUserCreatedConsumer(IUserService userService, IMessagePublisher publisher)
    {
        _userService = userService;
        _publisher = publisher;
    }

    public async Task Consume(ConsumeContext<CreateUserFromCollaboratorCommand> context)
    {
        var msg = context.Message;

        var userDTO = new UserDTO
        {
            Names = msg.Names,
            Surnames = msg.Surnames,
            Email = msg.Email,
            FinalDate = msg.DeactivationDate
        };

        await _userService.AddUserFromSagaAsync(userDTO, msg.InstanceId);
    }
}