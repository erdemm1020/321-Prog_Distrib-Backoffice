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

        string path = Path.Combine(JsonSer.dataFolder, $"{Id}.json");
        File.WriteAllText(path, JsonSerializer.Serialize(this, JsonSer.JsonOptions));

        Console.WriteLine($"Exporté : {path}");
    }
    public static Business? LoadData(string id)
    {
        string path = Path.Combine(JsonSer.dataFolder, $"{id}.json");
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Business>(json, JsonSer.JsonOptions);
    }

}
