using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace Backoffice;

public static class Horaires
{
    // valeurs par défaut, remplacées par Data/horaires.json s'il existe
    public static TimeSpan Ouverture { get; private set; } = new(6, 0, 0);
    public static TimeSpan PauseDebut { get; private set; } = new(11, 30, 0);
    public static TimeSpan PauseFin { get; private set; } = new(13, 0, 0);
    public static TimeSpan Fermeture { get; private set; } = new(14, 30, 0);

    public static readonly string[] Jours =
        { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };

    public static string Texte =>
        $"{Ouverture:h\\hmm} - {PauseDebut:h\\hmm} / {PauseFin:h\\hmm} - {Fermeture:h\\hmm}";

    private static string Chemin => Path.Combine(JsonSer.dataFolder, "horaires.json");

    private record Donnees(string Ouverture, string PauseDebut, string PauseFin, string Fermeture);

    public static int IndexJour(DateTime date) => ((int)date.DayOfWeek + 6) % 7;

    public static bool EstOuvert(DateTime heure)
    {
        TimeSpan t = heure.TimeOfDay;
        bool pause = t >= PauseDebut && t < PauseFin;
        return t >= Ouverture && t < Fermeture && !pause;
    }

    // Change les horaires (format 6:00). Retourne false si les valeurs sont fausses.
    public static bool Modifier(string? ouverture, string? pauseDebut, string? pauseFin, string? fermeture)
    {
        if (!Lire(ouverture, out var o) || !Lire(pauseDebut, out var d) ||
            !Lire(pauseFin, out var f) || !Lire(fermeture, out var fin))
            return false;

        // ordre obligatoire ; pause début = pause fin veut dire pas de pause
        if (!(o <= d && d <= f && f <= fin && o < fin))
            return false;

        (Ouverture, PauseDebut, PauseFin, Fermeture) = (o, d, f, fin);
        return true;
    }

    public static void Enregistrer()
    {
        var donnees = new Donnees(
            Ecrire(Ouverture), Ecrire(PauseDebut), Ecrire(PauseFin), Ecrire(Fermeture));

        Directory.CreateDirectory(JsonSer.dataFolder);
        File.WriteAllText(Chemin, JsonSerializer.Serialize(donnees, JsonSer.JsonOptions));
    }

    public static void Charger()
    {
        try
        {
            if (!File.Exists(Chemin)) return;

            var d = JsonSerializer.Deserialize<Donnees>(File.ReadAllText(Chemin), JsonSer.JsonOptions);
            if (d == null || !Modifier(d.Ouverture, d.PauseDebut, d.PauseFin, d.Fermeture))
                Console.WriteLine("horaires.json incorrect : horaires par défaut");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"horaires.json illisible : {ex.Message}");
        }
    }

    public static string Ecrire(TimeSpan heure) => heure.ToString(@"hh\:mm");

    private static bool Lire(string? texte, out TimeSpan heure) =>
        TimeSpan.TryParseExact(texte?.Trim(), @"h\:mm", CultureInfo.InvariantCulture, out heure);
}
