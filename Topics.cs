using System;
using System.IO;
using System.Text.Json;

namespace Backoffice;

public static class Topics
{
    // valeurs par défaut, remplacées par Data/topics.json s'il existe
    public static string Heure { get; private set; } = "umddcmid3b/timebackup";
    public static string Log { get; private set; } = "umddcmid3b/log";

    private static string Chemin => Path.Combine(JsonSer.dataFolder, "topics.json");

    private record Donnees(string Heure, string Log);

    // Change les topics. Retourne false si un topic est vide, contient + ou #, ou si les deux sont pareils.
    public static bool Modifier(string? heure, string? log)
    {
        heure = heure?.Trim();
        log = log?.Trim();

        if (!EstValide(heure) || !EstValide(log) || heure == log)
            return false;

        (Heure, Log) = (heure!, log!);
        return true;
    }

    public static void Enregistrer()
    {
        Directory.CreateDirectory(JsonSer.dataFolder);
        File.WriteAllText(Chemin, JsonSerializer.Serialize(new Donnees(Heure, Log), JsonSer.JsonOptions));
    }

    public static void Charger()
    {
        try
        {
            if (!File.Exists(Chemin)) return;

            var d = JsonSerializer.Deserialize<Donnees>(File.ReadAllText(Chemin), JsonSer.JsonOptions);
            if (d == null || !Modifier(d.Heure, d.Log))
                Console.WriteLine("topics.json incorrect : topics par défaut");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"topics.json illisible : {ex.Message}");
        }
    }

    private static bool EstValide(string? topic) =>
        !string.IsNullOrEmpty(topic) && topic.IndexOfAny(new[] { '+', '#' }) < 0;
}
