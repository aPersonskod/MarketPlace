using Models;

namespace ProductCatalog;

public class DataContext
{
    public List<Product> Products { get => StaticData.Products; set => StaticData.Products = value; }
}

internal class StaticData
{
    public static List<Product> Products { get; set; } =
    [
        new Product { Id = Guid.NewGuid(), Name = "Футболка", Cost = 100},
        new Product { Id = new Guid("6BF3A1CE-1239-4528-8924-A56FF6527596"), Name = "Шорты", Cost = 200 },
        new Product { Id = new Guid("6BF3A1CE-1239-4528-8924-A56FF6527597"), Name = "Носки", Cost = 50 },
        new Product { Id = Guid.NewGuid(), Name = "Трусы", Cost = 70 },
    ];
}