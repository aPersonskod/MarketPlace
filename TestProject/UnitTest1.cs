using System.Text.Json;
using Models;
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
        var id = Guid.Parse("b8a0ff81-f0f6-405a-bfc5-ebb0e09fac8b");
        Assert.That(id.ToString(), Is.EqualTo("b8a0ff81-f0f6-405a-bfc5-ebb0e09fac8b"));
        /*var uids = new string[] {Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),Guid.NewGuid().ToString() };
        var client = new HttpClient();
        var response = await client.GetStringAsync($"https://localhost:7002/ShoppingCart");
        var shoppingCarts = JsonSerializer.Deserialize<CartDto>(response)?.OrderIds;
        Assert.That(shoppingCarts, Is.Not.Null);*/
    }
}