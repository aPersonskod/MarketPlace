using Orchestrator.Api.Apis;
using Orchestrator.Api.Extensions;
using Orchestrator.Application;
using Orchestrator.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();
builder.Services.AddOrchestratorInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddOrchestratorApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultApi();
app.MapBuyOrchestratorEndpoints();
app.Run();