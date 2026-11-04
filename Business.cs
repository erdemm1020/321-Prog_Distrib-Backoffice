using System;

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

}
