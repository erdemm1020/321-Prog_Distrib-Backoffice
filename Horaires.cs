using System;

namespace Backoffice;

public static class Horaires
{
    private static readonly TimeSpan Ouverture = new(6, 0, 0);
    private static readonly TimeSpan Fermeture = new(14, 30, 0);
    private static readonly TimeSpan PauseDebut = new(11, 30, 0);
    private static readonly TimeSpan PauseFin = new(13, 0, 0);

    public static bool EstOuvert(DateTime heure)
    {
        TimeSpan t = heure.TimeOfDay;
        bool pause = t >= PauseDebut && t < PauseFin;
        return t >= Ouverture && t < Fermeture && !pause;
    }
}
