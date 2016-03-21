using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// Summary description for Parser
/// </summary>
public static class CardFactory
{
	public static List<Card> BuildCards()
	{
        List<Card> cards = new List<Card>();

        cards = JsonConvert.DeserializeObject<List<Card>>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "\\hearthstone.json")));

        

        return cards;

	}
}