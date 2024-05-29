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
            var tmp = File.ReadAllLines("decklist.txt").ToList();
            Cards = new List<string>();
            foreach (var x in tmp)
            {
                var ss = x.Split(' ');
                var num = int.Parse(ss[0][0].ToString());
                var name = string.Join(" ", ss.Skip(1)).Trim().ToLower();
                for (int i = 0; i < num; i++)
                {
                    Cards.Add(name);
                }
            }


            if (Cards.Contains("wild cantor"))
            {
                Global.ContainsCantor = true;
                Global.CantorId = Cards.IndexOf("wild cantor");
            }
            else
            {
                Global.ContainsCantor = false;
            }
        }
    }
}
