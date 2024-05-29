using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NecroDeck
{
    class RunState
    {
        public bool CantorInHand { get; set; }
        public Random Random { get; internal set; }

        public HashSet<int> DrawnCards = new HashSet<int>();

        public RunState Clone()
        {
            return new RunState
            {
                CantorInHand = CantorInHand,
                DrawnCards = new HashSet<int>(DrawnCards),
                Random = Random,

            };
        }
    }

    enum TimingState
    {
        MainPhase,
        InstantOnly,
        Borne
    }

    class State
    {
        public List<int> Cards;
      
        public RunState RunState { get; set; }

        public void ModifyRunState(Action<RunState> modaction)
        {
            RunState = RunState.Clone();
            modaction(RunState);
        }

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

        public int TotalMana => BlackMana + BlueMana + AnyMana + RedMana + GreenMana;

        public int AnyMana = 0;
        public int BlackMana = 0;
        public int RedMana = 0;
        public int GreenMana = 0;
        public int BlueMana = 0;
        public int LandDrops = 0;
        public int BargainFodder = 0;
        public TimingState TimingState;
        public bool Win;

        public State Clone()
        {
            return new State
            {
                RunState = RunState,
                Cards = new List<int>(Cards),
                BlackMana = BlackMana,
                BlueMana = BlueMana,
                RedMana = RedMana,
                GreenMana = GreenMana,
                AnyMana = AnyMana,
                LandDrops = LandDrops,
                BargainFodder = BargainFodder,
                Win = Win,
                TimingState = TimingState
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is State other)
            {
                return  
                       BlackMana == other.BlackMana &&
                       BlueMana == other.BlueMana &&
                       AnyMana == other.AnyMana &&
                       RedMana == other.RedMana &&
                       GreenMana == other.GreenMana &&
                       LandDrops == other.LandDrops &&
                       BargainFodder == other.BargainFodder &&
                       TimingState == other.TimingState &&
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
            hash = hash * 31 + BlueMana.GetHashCode();
            hash = hash * 31 + AnyMana.GetHashCode();
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

        internal void DrawCards(int v)
        {
            int max = Global.Deck.Cards.Count;

            var newRunState = RunState.Clone();
            var c = newRunState.DrawnCards.Count;

            while (newRunState.DrawnCards.Count - c < v)
            {
                int number = newRunState.Random.Next(0, max);
                newRunState.DrawnCards.Add(number);
            }

            Cards.AddRange(newRunState.DrawnCards.Except(RunState.DrawnCards).OrderBy(p => p));
            RunState = newRunState;
        }

        internal IEnumerable<State> Pay(Mana color, int v)
        {
            if (color.HasFlag(Mana.Black))
            {
                var clone = Clone();
                clone.BlackMana -= v;
                if (clone.BlackMana < 0)
                {
                    clone.AnyMana += clone.BlackMana;
                    clone.BlackMana = 0;
                }
                yield return clone;
            }
            if (color.HasFlag(Mana.Blue))
            {
                var clone = Clone();
                clone.BlueMana -= v;
                if (clone.BlueMana < 0)
                {
                    clone.AnyMana += clone.BlueMana;
                    clone.BlueMana = 0;
                }
                yield return clone;
            }
            if (color.HasFlag(Mana.Red))
            {
                var clone = Clone();
                clone.RedMana -= v;
                if (clone.RedMana < 0)
                {
                    clone.AnyMana += clone.RedMana;
                    clone.RedMana = 0;
                }
                yield return clone;
            }
            if (color.HasFlag(Mana.Green))
            {
                var clone = Clone();
                clone.GreenMana -= v;
                if (clone.GreenMana < 0)
                {
                    clone.AnyMana += clone.GreenMana;
                    clone.GreenMana = 0;
                }
                yield return clone;
            }
        }

        internal bool CanPay(Mana color, int v, int generic = 0)
        {
            if (color.HasFlag(Mana.Black))
            {
                if (AnyMana > 0)
                {

                }
                if (BlackMana+ AnyMana >= v && (TotalMana -v) >= generic)
                {
                    return true;
                }
            }
            if (color.HasFlag(Mana.Blue))
            {
                if (BlueMana + AnyMana >= v && (TotalMana - v) >= generic)
                {
                    return true;
                }
            }
            if (color.HasFlag(Mana.Red))
            {
                if (RedMana + AnyMana >= v && (TotalMana - v) >= generic)
                {
                    return true;
                }
            }
            if (color.HasFlag(Mana.Green))
            {
                if (GreenMana + AnyMana >= v && (TotalMana - v) >= generic)
                {
                    return true;
                }
            }
            return false;
        }

        internal IEnumerable<State> WaysToPay(Mana color, int v1, int v2 = 0)
        {
            //de är alltid rätt att tappa för svart
            
            List<State> ways = this.Pay(color, v1).ToList();
            for (int i = 0; i < v2; i++)
            {
                var newList = new List<State>();

                newList.AddRange(ways.SelectMany(p => PayOneColorless(p)));
                ways = newList;
            }
            return ways;
        }
        private IEnumerable<State> PayOneColorless(State clone)
        {
            if (clone.BlueMana > 0)
            {
                yield return clone.Clone().With(p => { p.BlueMana--; });
            }
            if (clone.RedMana > 0)
            {
                yield return clone.Clone().With(p => { p.RedMana--; });

            }
            if (clone.GreenMana > 0)
            {
                yield return clone.Clone().With(p => { p.GreenMana--; });

            }
            if (clone.BlackMana > 0)
            {
                yield return clone.Clone().With(p => { p.BlackMana--; });
            }
            if (clone.AnyMana > 0)
            {
                yield return clone.Clone().With(p => { p.AnyMana--; });
            }



        }
    }
}
