using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NecroDeck
{
    public class Deck
    {
        public List<string> Cards { get; set; }
        public Deck()
        {
            Cards = File.ReadAllLines("decklist.txt").ToList();
            if (Cards.Contains("WildCantor"))
            {
                Global.ContainsCantor = true;
                Global.CantorId = Cards.IndexOf("WildCantor");
            }
        }
    }
}
