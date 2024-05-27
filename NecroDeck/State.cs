using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NecroDeck
{
    class RunState
    {
        public bool CantorInHand { get; set; }
    }
    class State
    {
        public List<int> Cards;
      
        public RunState RunState { get; set; }
        public void RemoveCard(int x)
        {
            Cards.Remove(x);
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var x in Cards)
            {
                sb.Append(Global.Deck.Cards[x]);
                sb.Append(", ");
            }
           
            Console.WriteLine(sb.ToString());
        }

        public int CardsInHand => Cards.Count;

        public int BlackMana = 0;
        public int RedGreenMana = 0;
        public int OtherMana = 0;
        public int LandDrops = 0;
        public int BargainFodder = 0;
        public bool Win;

        public State Clone()
        {
            return new State
            {
                RunState = RunState,
                Cards = new List<int>(Cards),
                BlackMana = BlackMana,
                RedGreenMana = RedGreenMana,
                OtherMana = OtherMana,
                LandDrops = LandDrops,
                BargainFodder = BargainFodder,
                Win = Win
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is State other)
            {
                return  
                       BlackMana == other.BlackMana &&
                       RedGreenMana == other.RedGreenMana &&
                       OtherMana == other.OtherMana &&
                       LandDrops == other.LandDrops &&
                       BargainFodder == other.BargainFodder &&
                       CardCompare(other) &&
                       Win == other.Win;
            }
            return false;
        }

        private bool CardCompare(State other)
        {
            if (Cards.Count != other.Cards.Count)
            {
                return false;
            }

            return Cards.All(p => other.Cards.Contains(p));

        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + CardsInHand.GetHashCode();
            hash = hash * 31 + BlackMana.GetHashCode();
            hash = hash * 31 + RedGreenMana.GetHashCode();
            hash = hash * 31 + OtherMana.GetHashCode();
            hash = hash * 31 + BargainFodder.GetHashCode();
            return hash;
        }

        internal bool ContainsCardsExact(string v)
        {
            var spl = v.Split(',').Select(p => p.Trim()).ToArray();

            List<string> c = new List<string>();
            foreach (var x in Cards)
            {
                c.Add(Global.Deck.Cards[x]);
            }

            return c.Count == spl.Length && spl.All(q => c.Contains(q));

            
        }
        internal bool ContainsCards(string v)
        {
            var spl = v.Split(',').Select(p => p.Trim().ToLower()).ToArray();

            List<string> c = new List<string>();
            foreach (var x in Cards)
            {
                c.Add(Global.Deck.Cards[x].ToLower());
            }

            return spl.All(q => c.Contains(q));


        }
    }
}
