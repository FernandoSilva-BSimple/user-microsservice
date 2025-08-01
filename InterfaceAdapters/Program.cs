using Application.DTO;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using InterfaceAdapters.Publishers;
using Application.IPublishers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AbsanteeContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<IMessagePublisher, MassTransitPublisher>();

//Repositories
builder.Services.AddTransient<IUserRepository, UserRepositoryEF>();

//Factories
builder.Services.AddTransient<IUserFactory, UserFactory>();

//Mappers
builder.Services.AddTransient<UserDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<User, UserDTO>();
});

// MassTransit
var instanceId = InstanceInfo.InstanceId;

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<CollaboratorWithoutUserCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", 5674, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint($"users-cmd-{instanceId}", e =>
{
    e.ConfigureConsumer<UserCreatedConsumer>(context);
});

        cfg.ReceiveEndpoint("users-cmd-saga", e =>
                {
                    e.ConfigureConsumer<CollaboratorWithoutUserCreatedConsumer>(context);
                });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

// read env variables for connection string
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();



app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();
    dbContext.Database.Migrate();
}

app.Run();

public partial class Program { }
