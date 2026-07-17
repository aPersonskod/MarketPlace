using User.Api.Apis;
using User.Api.Extensions;
using User.Application;
using User.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDefaultServices();
builder.Services.AddUserInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddUserApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultApi();
app.MapUserEndpoints();
app.MapAuthEndpoints();
app.Run();