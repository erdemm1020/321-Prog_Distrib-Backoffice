using System;

namespace Backoffice;

public class Business
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Special { get; set; }

    public Business(int id, string name, string logo, string special)
    {
        Id = id;
        Name = name;
        Logo = logo;
        Special = special;
    }

}
