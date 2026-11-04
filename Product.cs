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

    private static string _dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        // indente le josn sur plusieurs lignes pour qu'il soit lisible
        WriteIndented = true,
        // encode correctment avec les accents
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    public Product(string id, string name, string picture, float price)
    {
        Id = id;
        Name = name;
        Picture = picture;
        Price = price;
    }

    public void ExportJson()
    {
        Directory.CreateDirectory(_dataFolder);

        string path = Path.Combine(_dataFolder, "product.json");
        File.WriteAllText(path, JsonSerializer.Serialize(this, JsonOptions));

        Console.WriteLine($"Exporté : {path}");
    }
}