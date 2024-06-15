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
                if (x.Trim().Length == 0)
                {
                    continue;
                }
                var ss = x.Split(' ');
                var num = int.Parse(ss[0][0].ToString());
                var name = string.Join(" ", ss.Skip(1)).Trim().ToLower();
                for (int i = 0; i < num; i++)
                {
                    Cards.Add(name);
                }
            }

            Cards = OrderByPriority(Cards);

            for (int i = 0; i < Cards.Count; i++)
            {
                if (!Global.Dict.ContainsKey(Cards[i]))
                {
                    Global.Dict[Cards[i]] = new List<int>();
                }
                Global.Dict[Cards[i]].Add(i);
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

        private List<string> OrderByPriority(List<string> cards)
        {
            return cards.OrderBy(p => GetPriority(p)).ToList();
        }
        //byt från queue to state when switching
        private int GetPriority(string p)
        {
            if (p == "vault of whispers")
            {
                return 100;
            }
            if (p == "gemstone mine")
            {
                return 95;
            }
            if (p == "dark ritual")
            {
                return 90;
            }
            if (p == "necropotence")
            {
                return 85;
            }
            if (p == "valakut awakening")
            {
               // return 3;
            }
            return 5;
        }
    }
}
