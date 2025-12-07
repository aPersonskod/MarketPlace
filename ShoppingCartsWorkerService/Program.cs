using Models.Interfaces;
using ShoppingCartsWorkerService;
using ShoppingCartsWorkerService.Settings;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<ShoppingCartKafkaSettings>(builder.Configuration.GetSection("Kafka:ShoppingCart"));
builder.Services.Configure<BuyActionsSettings>(builder.Configuration.GetSection("Grpc:BuyActions"));

builder.Services.AddTransient<IBuyService, BuyServiceClient>();
builder.Services.AddHostedService<ShoppingCartConsumerService>();

var host = builder.Build();
host.Run();