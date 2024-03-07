using InvitationQueryService.Application.Abstraction;
using InvitationQueryService.Application.CommandHandler.Send;
using InvitationQueryService.Application.ServiceBus;
using InvitationQueryService.Database;
using InvitationQueryService.Infrastructure.Repository;
using InvitationQueryService.Infrastructure.ServiceBus;
using InvitationQueryService.Presentation.Interceptors;
using InvitationQueryService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(option =>
{
    option.Interceptors.Add<HandleErrorInterceptor>();
});
var azureOptions = builder.Configuration.GetSection("Azure").Get<AzureOptions>();

builder.Services.AddSingleton<AzureOptions>(azureOptions!);

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(SendInvitationCommandHandler).Assembly));

builder.Services.AddDbContext<InvitationDbContext>(
    options => options.UseSqlServer("")
    );
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddSingleton<ServiceBusPublisher>();
builder.Services.AddScoped<IServiceBusRepository, ServiceBusRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<InvitationService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();


public partial class Program { }