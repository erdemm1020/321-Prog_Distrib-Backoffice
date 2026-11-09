using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Backoffice;

public class Business
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public Product Special { get; set; }

    public Business(string id, string name, string logo, Product special)
    {
        Id = id;
        Name = name;
        Logo = logo;
        Special = special;
    }

    public void ExportJson()
    {
        Directory.CreateDirectory(JsonSer.dataFolder);

        string path = Path.Combine(JsonSer.dataFolder, "business.json");
        File.WriteAllText(path, JsonSerializer.Serialize(this, JsonSer.JsonOptions));

        Console.WriteLine($"Exporté : {path}");
    }

}
