namespace ShoppingCartsWorkerService.Settings;

public class ShoppingCartKafkaSettings
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    public string Topic { get; set; }
}