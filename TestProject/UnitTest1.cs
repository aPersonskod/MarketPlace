using System.Text.Json;
using Models.Dtos;

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
        var ids = new string[] {Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
        /*var uids = new string[] {Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),Guid.NewGuid().ToString() };
        var client = new HttpClient();
        var response = await client.GetStringAsync($"https://localhost:7002/ShoppingCart");
        var shoppingCarts = JsonSerializer.Deserialize<CartDto>(response)?.OrderIds;
        Assert.That(shoppingCarts, Is.Not.Null);*/
    }
}

