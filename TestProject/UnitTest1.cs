using Models;
using System.Text.Json;

namespace TestProject;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestApi()
    {
        var client = new HttpClient();
        var response = await client.GetStringAsync($"https://localhost:7002/ShoppingCart");
        var shoppingCarts = JsonSerializer.Deserialize<Cart>(response)?.Orders;
        Assert.That(shoppingCarts, Is.Not.Null);
    }
}

