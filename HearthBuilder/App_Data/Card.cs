using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Card
{
    public string CardId { get; set; }
    public string Name { get; set; }
    public string CardSet { get; set; }
    public string Type { get; set; }
    public string Faction { get; set; }
    public string Rarity { get; set; }
    public int Cost { get; set; }
    public int Attack { get; set; }
    public int Health { get; set; }
    public int Durability { get; set; }
    public string Text { get; set; }
    public string InPlayText { get; set; }
    public string Flavor { get; set; }
    public string Artist { get; set; }
    public bool Collectable { get; set; }
    public bool Elite { get; set; }
    public string Race { get; set; }
    public string PlayerClass { get; set; }
    public string HowToGet { get; set; }
    public string Img { get; set; }
    public string ImgGold { get; set; }
    public string Locale { get; set; }
    public List<string> Mechanics { get; set; }

    public Card ()
    {

    }
    /*
	public Card (
        string dardId,
        string name,
        string cardSet,
        string type,
        string faction,
        string rarity,
        int cost,
        int attack,
        int health,
        int durability,
        string text,
        string inPlayText,
        string flavor,
        string artist,
        bool collectable
        )
	{
     



	}*/   
}