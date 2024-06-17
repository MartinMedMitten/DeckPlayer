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
        public int SerumPowder { get; internal set; }
        internal State State { get; set; }
        internal State NecroState { get; set; }
    }
    public class DeckPlayer
    {
        public RunResult Run(int seed)
        {
            r = new Random(seed);
            int mulligans = 0;
            ulong exiledToPowder = 0;

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

            ulong exiledToPowderThisMull = 0;

            foreach (var x in Utility.BitFlagToList(exiledToPowder))
            {
                start.RunState.DrawnCards.Add(x);
            }

            start.Cards = new List<int>();
            start.CardsInPlay = new List<CardInPlay>();
            start.DrawCards(7);
            State postNecroState = null;
            bool gotBorne = false;

            start.RunState.CantorInHand = start.Cards.Contains(Global.CantorId);

            bool containsPact = start.ContainsCards("pact of negation"); //TODO does not work with serum powder currently, so protected % will be to high

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

                start.Powderable = !Global.Dict["tendrils of agony"].All(q => start.HasCardInHand(q)) && Global.Dict["serum powder"].Any(q => start.HasCardInHand(q));
            }
            else
            {
                foreach (var x in TakeMulligan(mulligans, start))
                {
                    if (x.RunState.CantorInHand)
                    {
                        x.ModifyRunState((st) => st.CantorInHand = x.HasCardInHand(Global.CantorId)); //might have mulliganed it away
                    }
                    x.Powderable = !Global.Dict["tendrils of agony"].All(q => x.HasCardInHand(q)) && Global.Dict["serum powder"].Any(q => x.HasCardInHand(q));

                    open.Enqueue(x);
                }
            }


            RunResult Win(State s) => new RunResult
            {
                Win = true,
                Mulligans = mulligans,
                Protected = containsPact && s.CardsInHandCount > 0,
                State = s,
                NecroState = postNecroState,
                SerumPowder = s.RunState.SerumPowder,
            };


            while (open.Any())
            {
                var s = open.Dequeue();

                if (s.TimingState == TimingState.InstantOnly)
                {
                    //heuristics for post necro win.
                    if (s.CanPay(Mana.Blue, 1, 1) && Global.Dict["borne upon a wind"].Any(p => s.HasCardInHand(p)))
                        s.Win = true;
                    else if (s.CanPay(Mana.Red, 1, 2) && Global.Dict["valakut awakening"].Any(p => s.HasCardInHand(p)))
                    {
                        s.Win = true;
                    }

                    if (s.Win)
                    {
                        return Win(s);
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

                foreach (var x in newActions.OrderByDescending(p => p.LandDrops))
                {
                    if (x.RunState.ExiledToPowder != 0)
                    {
                        exiledToPowderThisMull |= x.RunState.ExiledToPowder;
                    }
                    if (closed.Add(x))
                    {
                        
                        if (x.Win)
                        {
                            return Win(x);
                        }
                        if (s.TimingState == TimingState.Borne)
                        {
                            gotBorne = true;
                        }
                        
                        if (s.TimingState == TimingState.MainPhase && x.TimingState == TimingState.InstantOnly)
                        {
                            
                            open = new QueueWrapper<State>();
                            x.ClearMana();
                            var qqq = Global.Dict["lotus petal"].Where(q => s.HasCardInHand(q)).ToList();
                            if (qqq.Any())
                            {
                                foreach (var asdf in qqq)
                                {
                                    x.RemoveCard(asdf);
                                    x.AnyMana++;
                                }
                            }
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
                if (exiledToPowderThisMull != 0)
                {
                    var l = Utility.BitFlagToList(exiledToPowderThisMull);

                    l.RemoveRange(0, mulligans);
                    

                    exiledToPowder |= Utility.ListToBitflag(l);
                }

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
                foreach (var x in Rules.GetResult(card, s)) //om jag här kollar att den overridear, så ska jag prioritera det!
                {
                    x.RemoveCard(card);
                    x.Text = Global.Deck.Cards[card];
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
