using System;
using System.Collections.Generic;
using System.Linq;

namespace NecroDeck
{
    public class RunResult
    {
        public bool Win { get; set; }
        public int Mulligans { get; set; }
        public bool Protected { get; set; }
    }
    public class DeckPlayer
    {
        public RunResult Run(Deck deck, int seed)
        {
            r = new Random(seed);
            int mulligans = 0;
        takeMulligan:
            var start = new State();
            start.RunState = new RunState
            {
                Random = r,
            };
            start.Cards = new List<int>();
            start.DrawCards(7);

            start.RunState.CantorInHand = start.Cards.Contains(Global.CantorId);

            //var hand = Get7Unique(deck);
            //start.Cards = hand;
           

            bool containsPact = start.ContainsCards("pact of negation");

            if (Global.DebugOutput)
            {
                start.Print();
            }
            
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
                    if (x.RunState.CantorInHand)
                    {
                        x.ModifyRunState((st) => st.CantorInHand = x.Cards.Contains(Global.CantorId));//might have mulliganed it away
                    }
                    open.Enqueue(x);
                }
            }


            while (open.Any())
            {
                var s = open.Dequeue();
                

                //if (s.ContainsCards("Necro,CabalRitual,VaultOfWhispers,SpiritGuide"))
                //{

                //}
                var newActions = GetActions(s);

                foreach (var x in newActions)
                {
                    if (closed.Add(x))
                    {
                        if (x.Win)
                        {
                            return new RunResult
                            {
                                Win = true,
                                Mulligans = mulligans,
                                Protected = containsPact && x.CardsInHand > 0
                            };
                        }
                        open.Enqueue(x);
                    }
                }

            }
            if (mulligans < 4) //cant win on 5 mulligans
            {
                mulligans++;
                goto takeMulligan; //sue me
            }
            return new RunResult();
        }

        private static IEnumerable<State> GetActions(State s)
        {
            foreach (var card in s.Cards)
            {
                foreach (var x in Rules.GetResult(card, s))
                {
                    x.RemoveCard(card);
                    yield return x;
                }
            }
        }
        private Random r;
        
        List<int> Get7Unique(Deck deck)
        {
            int max = deck.Cards.Count;
            HashSet<int> uniqueNumbers = new HashSet<int>();

            while (uniqueNumbers.Count < 7)
            {
                int number = r.Next(0, max);
                uniqueNumbers.Add(number);
            }

            return uniqueNumbers.OrderBy(p => p).ToList();

        }

        private IEnumerable<State> TakeMulligan(int mullC, State start)
        {
            mullC--;
            foreach (var xa in start.Cards) 
            {
                var c = start.Clone().With(p => p.RemoveCard(xa));
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
    }
}
