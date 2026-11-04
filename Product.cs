using System;

namespace Backoffice;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public decimal Price { get; set; }

    public Product(int id, string name, string picture, decimal price)
    {
        Id = id;
        Name = name;
        Picture = picture;
        Price = price;
    }
}
