using ShoppingCartsWorkerService;
using ShoppingCartsWorkerService.Settings;

var builder = Host.CreateApplicationBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<ShoppingCartKafkaSettings>(builder.Configuration.GetSection("Kafka:ShoppingCartDev"));
    builder.Services.Configure<BuyActionsSettings>(builder.Configuration.GetSection("Grpc:BuyActionsDev"));
}
else
{
    builder.Services.Configure<ShoppingCartKafkaSettings>(builder.Configuration.GetSection("Kafka:ShoppingCart"));
    builder.Services.Configure<BuyActionsSettings>(builder.Configuration.GetSection("Grpc:BuyActions"));
}

builder.Services.AddTransient<IBuyCartService, BuyCartService>();
builder.Services.AddHostedService<ShoppingCartConsumerService>();

var host = builder.Build();
host.Run();