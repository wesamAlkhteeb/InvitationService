using InvitationQueryService.Application.ServiceBus;
using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Application.QuerySideServiceBus.Send;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryService.Infrastructure.Repository;
using InvitationQueryService.Infrastructure.ServiceBus;
using InvitationQueryService.Presentation.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var azureOptions = builder.Configuration.GetSection("Azure").Get<AzureOptions>();
builder.Services.AddSingleton<AzureOptions>(azureOptions!);

builder.Services.AddDbContext<InvitationDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
    //option=> option.UseInMemoryDatabase(Guid.NewGuid().ToString())

    );
builder.Services.AddScoped<IInvitationEventsRepository, InvitationEventsRepository>();
builder.Services.AddScoped<ISubscriptorRepository, SubscriptorRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();


builder.Services.AddHostedService<InvitationListener>();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(SendInvitationQueryHandler).Assembly));

var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
await services.Database.EnsureCreatedAsync();

app.MapGrpcService<PermissionsService>();
app.MapGrpcService<SubsctiptionsService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }