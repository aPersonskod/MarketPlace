using HostedService.Api;
using HostedService.Application;
using HostedService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedServiceInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddHostedServiceApplication();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();