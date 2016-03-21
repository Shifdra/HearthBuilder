using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Parser
/// </summary>
public static class CardFactory
{
	public static List<Card> BuildCards()
	{
        List<Card> cards = new List<Card>();

        /*JsonTextReader reader = new JsonTextReader(new StringReader(json));
while (reader.Read())
{
    if (reader.Value != null)
    {
        Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
    }
    else
    {
        Console.WriteLine("Token: {0}", reader.TokenType);
    }
}*/

        return cards;

	}
}