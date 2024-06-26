﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NecroDeck
{
    public class Deck
    {
        public List<string> Cards { get; set; }
        public List<int> CardNumbers { get; set; } = new List<int>(); //a bit silly, this is just 1-60
        public List<int> PriorityList { get; set; } = new List<int>(); 
        public Deck(int? cut = null)
        {
            Global.Dict.Clear();
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
                    if (Cards.Count == cut)
                    {
                        cut = null;
                        continue;
                    }
                    CardNumbers.Add(Cards.Count);
                    Cards.Add(name);
                }
            }

            Cards = OrderByPriority(Cards);
            Global.Dict.Add("serum powder", new List<int>());
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


            AddPriority("necrodominance");
            AddPriority("dark ritual");
            AddPriority("lotus petal");
            Rules.InitRuleDict(this);
        }

        private void AddPriority(string v)
        {
            PriorityList.AddRange(Global.Dict[v]);
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
