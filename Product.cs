using System;

namespace Backoffice;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public float Price { get; set; }

    public Product(string id, string name, string picture, float price)
    {
        Id = id;
        Name = name;
        Picture = picture;
        Price = price;
    }
}
