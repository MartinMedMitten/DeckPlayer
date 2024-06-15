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
        public bool GotNecro => NecroState != null;
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
            start.CardsInPlay = new List<CardInPlay>();
            start.DrawCards(7);
            State postNecroState = null;
            bool gotBorne = false;
            //bool gotNecro = false;

            start.RunState.CantorInHand = start.Cards.Contains(Global.CantorId);

            bool containsPact = start.ContainsCards("pact of negation");

            if (Global.DebugOutput)
            {
                start.Print();
            }

            StructureWrapper<State> open = new QueueWrapper<State>();
            var closed = new HashSet<State>();

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
                        x.ModifyRunState((st) => st.CantorInHand = x.HasCardInHand(Global.CantorId)); //might have mulliganed it away
                    }
                    open.Enqueue(x);
                }
            }


            while (open.Any())
            {
                var s = open.Dequeue();


                if (s.TimingState == TimingState.InstantOnly)
                {
                    if (s.CanPay(Mana.Blue, 1) && Global.Dict["borne upon a wind"].Any(p => s.HasCardInHand(p)))
                        s.Win = true;
                    else if (s.CanPay(Mana.Red, 2) && Global.Dict["valakut awakening"].Any(p => s.HasCardInHand(p)))
                    {
                        s.Win = true;
                    }

                    if (s.Win)
                    {
                        return new RunResult
                        {
                            Win = true,
                            Mulligans = mulligans,
                            Protected = containsPact && s.CardsInHand > 0,
                            State = s,
                            NecroState = postNecroState,
                        };
                    }
                }


                if (stopWatch.ElapsedMilliseconds > 3000)
                {
                    if (Global.DebugOutput)
                    {
                        Console.WriteLine("timeout");
                    }

                    return new RunResult
                    {
                     
                        State = postNecroState,
                        Mulligans = mulligans,
                        Protected = false,
                        Win = false,
                        Inconclusive = true,
                        NecroState = postNecroState
                    };
                }

         
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
                                NecroState = postNecroState,
                            };
                        }
                        if (s.TimingState == TimingState.Borne)
                        {
                            gotBorne = true;
                        }
                        
                        if (s.TimingState == TimingState.MainPhase && x.TimingState == TimingState.InstantOnly)
                        {
                            open = new QueueWrapper<State>();
                            //open.Clear();
                            x.ClearMana();
                            open.Enqueue(x);
                            closed.Clear();
                         
                            postNecroState = x;
                            break;
                        }

                        open.Enqueue(x);
                    }
                }

            }
            if (mulligans < 4 && postNecroState == null) //cant win on 5 mulligans
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
                    NecroState = postNecroState,
                    
                };

            }
            return new RunResult();
        }

        private static IEnumerable<State> GetActions(State s)
        {

            foreach (var card in s.Cards)
            {
                if (!s.HasCardInHand(card))
                {
                    continue;
                }
                foreach (var x in Rules.GetResult(card, s))
                {
                    x.RemoveCard(card);
                    yield return x;
                }
            }
            foreach (var card in s.CardsInPlay)
            {
                if (card.Used)
                {
                    continue;
                }
                foreach (var x in Rules.GetResultFromInPlay(card.Card, s))
                {
                    yield return x;
                }
            }
        }
        private Random r;
        

        private IEnumerable<State> TakeMulligan(int mullC, State start)
        {
            mullC--;
            foreach (var xa in start.Cards.Where(p => start.HasCardInHand(p))) 
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
