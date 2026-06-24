using BuyReport.Api.Apis;
using BuyReport.Api.Extensions;
using BuyReport.Application;
using BuyReport.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();
builder.Services.AddBuyReportInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddBuyReportApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultApi();
app.MapBuyReportEndpoints();
app.Run();