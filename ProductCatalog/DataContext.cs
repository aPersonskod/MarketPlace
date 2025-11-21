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
        new Product { Id = Guid.NewGuid(), Name = "Футболка" },
        new Product { Id = new Guid("6BF3A1CE-1239-4528-8924-A56FF6527596"), Name = "Шорты" },
        new Product { Id = new Guid("6BF3A1CE-1239-4528-8924-A56FF6527597"), Name = "Носки" },
        new Product { Id = Guid.NewGuid(), Name = "Трусы" },
    ];
}