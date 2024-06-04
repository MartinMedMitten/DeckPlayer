using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NecroDeck
{
    public class RunResult
    {
        public bool Win { get; set; }
        public int Mulligans { get; set; }
        public bool Protected { get; set; }
        public bool Inconclusive { get; internal set; }
        public bool BorneLoss { get; internal set; }
        public int Index { get; internal set; }
        internal State State { get; set; }
        internal State NecroState { get; set; }
    }
    public class DeckPlayer
    {
        public RunResult Run(Deck deck, int seed)
        {
            r = new Random(seed);
            int mulligans = 0;
        takeMulligan:
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var start = new State()
            {
                TimingState = TimingState.MainPhase
            };
            start.RunState = new RunState
            {
                Random = r,
            };
            start.Cards = new List<int>();
            start.DrawCards(7);
            bool canMulligan = true;
            State postNecroState = null;
            bool gotBorne = false;

            start.RunState.CantorInHand = start.Cards.Contains(Global.CantorId);

            //var hand = Get7Unique(deck);
            //start.Cards = hand;
           

            bool containsPact = start.ContainsCards("pact of negation");

            if (Global.DebugOutput)
            {
                start.Print();
            }

            StructureWrapper<State> open = new QueueWrapper<State>();
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
              
                if (stopWatch.ElapsedMilliseconds > 5000)
                {
                    if (Global.DebugOutput)
                    {
                        Console.WriteLine("timeout");
                    }

                    return new RunResult
                    {
                        
                        Mulligans = mulligans,
                        Protected = false,
                        Win = false,
                        Inconclusive = true,
                    };
                }

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
                                Protected = containsPact && x.CardsInHand > 0,
                                State = x,
                                NecroState = postNecroState
                            };
                        }
                        if (s.TimingState == TimingState.Borne)
                        {
                            gotBorne = true;
                        }
                        if (s.TimingState == TimingState.MainPhase && x.TimingState == TimingState.InstantOnly)
                        {
                            open = new StackWrapper<State>();
                            //open.Clear();
                            x.BlackMana = 0;
                            open.Enqueue(x);
                         
                            postNecroState = x;
                            canMulligan = false;
                            break;
                        }

                        open.Enqueue(x);
                    }
                }

            }
            if (mulligans < 4 && canMulligan) //cant win on 5 mulligans
            {
                mulligans++;
                goto takeMulligan; //sue me
            }
            if (postNecroState != null)
            {
                return new RunResult
                {
                    State = postNecroState,
                    Win = false,
                    BorneLoss = gotBorne,
                    Mulligans = mulligans,
                    NecroState = postNecroState

                };

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
