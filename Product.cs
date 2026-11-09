using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;


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

    public void ExportJson()
    {
        Directory.CreateDirectory(JsonSer.dataFolder);

        string path = Path.Combine(JsonSer.dataFolder, $"{Id}.json");
        File.WriteAllText(path, JsonSerializer.Serialize(this, JsonSer.JsonOptions));

        Console.WriteLine($"Exporté : {path}");
    }
}