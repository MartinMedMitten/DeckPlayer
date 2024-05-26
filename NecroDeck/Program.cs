using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecroDeck
{
    public class Deck
    {
        public static List<string> Cards2;
        public List<string> Cards => Cards2;
        public Deck()
        {
            Cards2 = File.ReadAllLines("decklist.txt").ToList();
        }
    }

    class State
    {
        public int Card1;
        public int Card2;
        public int Card3;
        public int Card4;
        public int Card5;
        public int Card6;
        public int Card7;


        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            if (Card1 >= 0)
            {
                sb.Append(Deck.Cards2[Card1]);
                sb.Append(", ");
            }
            if (Card2 >= 0)
            {
                sb.Append(Deck.Cards2[Card2]);
                sb.Append(", ");
            }
            if (Card3 >= 0)
            {
                sb.Append(Deck.Cards2[Card3]);
                sb.Append(", ");
            }
            if (Card4 >= 0)
            {
                sb.Append(Deck.Cards2[Card4]);
                sb.Append(", ");

            }
            if (Card5 >= 0)
            {
                sb.Append(Deck.Cards2[Card5]);
                sb.Append(", ");
            }
            if (Card6 >= 0)
            {
                sb.Append(Deck.Cards2[Card6]);
                sb.Append(", ");
            }
            if (Card7 >= 0)
            {
                sb.Append(Deck.Cards2[Card7]);
                sb.Append(", ");
            }
            Console.WriteLine(sb.ToString());
        }

        public int CardsInHand = 0;

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
                Card1 = this.Card1,
                Card2 = this.Card2,
                Card3 = this.Card3,
                Card4 = this.Card4,
                Card5 = this.Card5,
                Card6 = this.Card6,
                Card7 = this.Card7,
                CardsInHand = this.CardsInHand,
                BlackMana = this.BlackMana,
                RedGreenMana = this.RedGreenMana,
                OtherMana = this.OtherMana,
                LandDrops = this.LandDrops,
                BargainFodder = this.BargainFodder,
                Win = this.Win
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is State other)
            {
                return this.Card1 == other.Card1 &&
                       this.Card2 == other.Card2 &&
                       this.Card3 == other.Card3 &&
                       this.Card4 == other.Card4 &&
                       this.Card5 == other.Card5 &&
                       this.Card6 == other.Card6 &&
                       this.Card7 == other.Card7 &&
                       this.CardsInHand == other.CardsInHand &&
                       this.BlackMana == other.BlackMana &&
                       this.RedGreenMana == other.RedGreenMana &&
                       this.OtherMana == other.OtherMana &&
                       this.LandDrops == other.LandDrops &&
                       this.BargainFodder == other.BargainFodder &&
                       this.Win == other.Win;
            }
            return false;
        }

        // Implementing the GetHashCode method
        public override int GetHashCode()
        {
            // Use a simple hash code combination formula
            int hash = 17;
            hash = hash * 31 + CardsInHand.GetHashCode();
            hash = hash * 31 + BlackMana.GetHashCode();
            hash = hash * 31 + RedGreenMana.GetHashCode();
            hash = hash * 31 + OtherMana.GetHashCode();
            hash = hash * 31 + LandDrops.GetHashCode();
            hash = hash * 31 + BargainFodder.GetHashCode();
            return hash;
        }

        internal bool ContainsCards(string v)
        {
            var spl = v.Split(',');

            List<string> c = new List<string>();
            if (Card1 >= 0)
            {
                c.Add(Deck.Cards2[Card1]);
            }
            if (Card2 >= 0)
            {
                c.Add(Deck.Cards2[Card2]);
            }
            if (Card3 >= 0)
            {
                c.Add(Deck.Cards2[Card3]);
            }
            if (Card4 >= 0)
            {
                c.Add(Deck.Cards2[Card4]);
            }
            if (Card5 >= 0)
            {
                c.Add(Deck.Cards2[Card5]);
            }
            if (Card6 >= 0)
            {
                c.Add(Deck.Cards2[Card6]);
            } 
            if (Card7 >= 0)
            {
                c.Add(Deck.Cards2[Card7]);
            }

            return c.Count == spl.Length && spl.All(q => c.Contains(q));

            
        }
    }
    class Program
    {
        static Random r = new Random(1);


        static void Main(string[] args)
        {
            var deck = new Deck();
            Rules.InitRuleDict(deck);
            int wins = 0;
            for (int i = 0; i < 10000; i++)
            {
                if (i == 67)
                {//WildCantor, PactofNegation, ChromeMox, GemstoneMine, Necro, VaultOfWhispers, CabalRitual,

                }
              //  Console.Write(i + ": ");
                if (Run(deck))
                {
                //    Console.WriteLine("WIN");
                    wins++;
                }
                else
                {
            //        Console.WriteLine("loss");
                }
            }

            
             Console.WriteLine("wins " + wins);

            Console.ReadKey();

        }
        static bool Run(Deck deck)
        {
         
            //s.Card1
            int mulligans = 0;
            takeMulligan:
            var start = new State();
            start.CardsInHand = 7;
            var hand = Get7Unique(deck);
            start.Card1 = hand[0];
            start.Card2 = hand[1];
            start.Card3 = hand[2];
            start.Card4 = hand[3];
            start.Card5 = hand[4];
            start.Card6 = hand[5];
            start.Card7 = hand[6];

          //  start.Print();
            Queue<State> open = new Queue<State>();

            HashSet<State> closed = new HashSet<State>();
            if (mulligans == 0)
            {
                open.Enqueue(start);
                closed.Add(start);
            }
            else
            {
                foreach (var x in TakeMulligan(mulligans, start))
                {
                    //if (x.Card1 != -1 && x.Card2 != -1 && x.Card4 != -1 && x.Card6 != -1)
                    open.Enqueue(x);
                }
            }
                




            while (open.Any())
            {
                var s = open.Dequeue();
                //if (s.ContainsCards("Necro,CabalRitual,VaultOfWhispers,SpiritGuide"))
                //{

                //}
                if (s.Win)
                {
                    return true;
                }
                var newActions = GetActions(s);

                foreach (var x in newActions)
                {
                    if (closed.Add(x))
                    {
                        if (x.Win)
                        {
                            return true;
                        }
                        open.Enqueue(x);
                    }
                }

            }
            if (mulligans < 5)
            {
                mulligans++;
                goto takeMulligan;
            }
            return false;
        }

        private static IEnumerable<State> TakeMulligan(int mullC, State start)
        {
            mullC--;
            if (start.Card1 >= 0)
            {
                var c = start.Clone().With(p => p.Card1 = -1);
                if (mullC > 0)
                {
                    foreach (var x in TakeMulligan(mullC, c))
                    {
                        yield return x;
                    }
                }
                else
                {
                    yield return c;
                }
            }

            if (start.Card2 >= 0)
            {
                var c = start.Clone().With(p => p.Card2 = -1);
                if (mullC > 0)
                {
                    foreach (var x in TakeMulligan(mullC, c))
                    {
                        yield return x;
                    }
                }
                else
                {
                    yield return c;
                }
            }
            if (start.Card3 >= 0)
            {
                var c = start.Clone().With(p => p.Card3 = -1);
                if (mullC > 0)
                {
                    foreach (var x in TakeMulligan(mullC, c))
                    {
                        yield return x;
                    }
                }
                else
                {
                    yield return c;
                }
            }
            if (start.Card4 >= 0)
            {
                var c = start.Clone().With(p => p.Card4 = -1);
                if (mullC > 0)
                {
                    foreach (var x in TakeMulligan(mullC, c))
                    {
                        yield return x;
                    }
                }
                else
                {
                    yield return c;
                }
            }
            if (start.Card5 >= 0)
            {
                var c = start.Clone().With(p => p.Card5 = -1);
                if (mullC > 0)
                {
                    foreach (var x in TakeMulligan(mullC, c))
                    {
                        yield return x;
                    }
                }
                else
                {
                    yield return c;
                }
            }
            if (start.Card6 >= 0)
            {
                var c = start.Clone().With(p => p.Card6 = -1);
                if (mullC > 0)
                {
                    foreach (var x in TakeMulligan(mullC, c))
                    {
                        yield return x;
                    }
                }
                else
                {
                    yield return c;
                }
            }
            if (start.Card7 >= 0)
            {
                var c = start.Clone().With(p => p.Card7 = -1);
                if (mullC > 0)
                {
                    foreach (var x in TakeMulligan(mullC, c))
                    {
                        yield return x;
                    }
                }
                else
                {
                    yield return c;
                }
            }
        }

        private static IEnumerable<State> GetActions(State s)
        {
            if (s.Card1 >= 0)
            {
                foreach (var x in Rules.GetResult(s.Card1, s))
                {
                    x.Card1 = -1;
                    x.CardsInHand--;
                    yield return x;
                }
            }

            if (s.Card2 >= 0)
            {
                foreach (var x in Rules.GetResult(s.Card2, s))
                {
                    x.Card2 = -1;
                    x.CardsInHand--;

                    yield return x;
                }
            }

            if (s.Card3 >= 0)
            {
                foreach (var x in Rules.GetResult(s.Card3, s))
                {
                    x.Card3 = -1;
                    x.CardsInHand--;

                    yield return x;
                }
            }

            if (s.Card4 >= 0)
            {
                foreach (var x in Rules.GetResult(s.Card4, s))
                {
                    x.Card4 = -1;
                    x.CardsInHand--;

                    yield return x;
                }
            }


            if (s.Card5 >= 0)
            {
                foreach (var x in Rules.GetResult(s.Card5, s))
                {
                    x.Card5 = -1;
                    x.CardsInHand--;

                    yield return x;
                }
            }


            if (s.Card6 >= 0)
            {
                foreach (var x in Rules.GetResult(s.Card6, s))
                {
                    x.Card6 = -1;
                    x.CardsInHand--;

                    yield return x;
                }
            }


            if (s.Card7 >= 0)
            {
                foreach (var x in Rules.GetResult(s.Card7, s))
                {
                    x.Card7 = -1;
                    x.CardsInHand--;

                    yield return x;
                }
            }

        }

        static List<int> Get7Unique(Deck deck)
        {
            int max = deck.Cards.Count;
            HashSet<int> uniqueNumbers = new HashSet<int>();

            while (uniqueNumbers.Count < 7)
            {
                int number = r.Next(0, max);
                uniqueNumbers.Add(number);
            }

            return uniqueNumbers.ToList();

        }
    }
}
