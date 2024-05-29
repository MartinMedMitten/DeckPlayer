using System;
using System.Collections.Generic;
using System.Linq;

namespace NecroDeck
{
    static class Utility
    {

        public static T With<T>(this T t, Action<T> a)
        {
            a(t);
            return t;
        }

        public static void Enqueue<T>(this Stack<T> s, T item)
        {
            s.Push(item);
        }
        public static T Dequeue<T>(this Stack<T> s)
        {
            return s.Pop();
        }

    }
    //enum Color
    //{
    //    Black,
    //    RedGreen,
    //    Red,
    //    Green,
    //    Other,
    //    None
    //}
    class Rules
    {
        public static Dictionary<int, Func<State, IEnumerable<State>>> RuleDict = new Dictionary<int, Func<State, IEnumerable<State>>>();
        public static Dictionary<int, Mana> ColorDict = new Dictionary<int, Mana>();

        public static void InitRuleDict(Deck deck)
        {
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                RuleDict[i] = GetFunc(deck.Cards[i], i);

            }
        }

        private static Func<State, IEnumerable<State>> GetFunc(string v, int i)
        {
            if (v == "dark ritual")
            {
                ColorDict[i] = Mana.Black;
                return DarkRitual;
            }
            if (v == "cabal ritual")
            {
                ColorDict[i] = Mana.Black;
                return CabalRitual;
            }
            if (v == "elvish spirit guide")
            {
                ColorDict[i] = Mana.Green;

                return ElvishSpiritGuide;
            }
            if (v == "simian spirit guide")
            {
                ColorDict[i] = Mana.Red;

                return SimianSpiritGuide;
            }
            if (v == "vault of whispers")
            {
                ColorDict[i] = Mana.None;

                return VaultOfWhispers;
            }
            if (v == "gemstone mine")
            {
                ColorDict[i] = Mana.None;

                return GemstoneMine;
            }
            if (v == "wild cantor")
            {
                ColorDict[i] = Mana.Red | Mana.Green;// | Mana.Green; TODO FUCK, how do i represent a red green chromemox

                return WildCantor;
            }
            if (v == "lotus petal")
            {
                ColorDict[i] = Mana.None;

                return LotusPetal;
            }
            if (v == "chrome mox")
            {
                ColorDict[i] = Mana.None;

                return ChromeMox;
            }
            if (v == "necrodominance" || v == "necropotence")
            {
                ColorDict[i] = Mana.Black;

                return Necro;
            }
            if (v == "beseech the mirror")
            {
                ColorDict[i] = Mana.Black;

                return Beseech;
            }
            if (v == "manamorphose")
            {
                ColorDict[i] = Mana.Red | Mana.Green;

                return Manamorphose;
            }    
            if (v == "borne upon a wind")
            {
                ColorDict[i] = Mana.Blue;

                return Borne;
            }
            if (v == "pact of negation")
            {
                ColorDict[i] = Mana.Blue;

                return NoOp;
            }
            if (v == "tendrils of agony")
            {
                ColorDict[i] = Mana.None; //can't imprint tendrils so i fake it.

                return NoOp;
            }
            if (v == "valakut awakening" || v == "fateful showdown")
            {
                ColorDict[i] = Mana.Red; 

                return NoOp;
            }
            if (v == "summoner's pact")
            {
                ColorDict[i] = Mana.Green; 

                return SummonersPact;
            }
            if (v == "brainspoil")
            {
                ColorDict[i] = Mana.Black; 

                return BrainSpoil;
            }

            if (v == "necrologia")
            {
                ColorDict[i] = Mana.Black;

                return Necrologia;
            }


            throw new NotImplementedException();
        }

        private static IEnumerable<State> Borne(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                if (arg.CanPay(Mana.Blue, 1, 1))
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }

            }
        }

        private static IEnumerable<State> BrainSpoil(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Black, 5, 1))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
        }

        private static IEnumerable<State> SummonersPact(State arg)
        {
            yield return arg.Clone().With(p => p.GreenMana++);
            if (Global.ContainsCantor && !arg.RunState.CantorInHand) //TODO if wildcantor is in deck and not it hand
            {
                yield return arg.Clone().With(p => { p.Cards.Add(Global.CantorId); p.ModifyRunState(x => x.CantorInHand = true); });
            }

            //yield return arg.Clone().With(p => { p.BargainFodder++; }); //but no such card exist
        }

        private static IEnumerable<State> NoOp(State arg)
        {
            yield break;
        }

        private static IEnumerable<State> Manamorphose(State arg)
        {
            if (arg.CanPay(Mana.Red | Mana.Green, 1, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Red | Mana.Green, 1, 1))
                {
                    yield return x.With(p => p.AnyMana += 2);
                }
            }

            //if (arg.RedGreenMana > 1)
            //{
            //    yield return arg.Clone().With(p => { p.RedGreenMana -= 2; p.BlackMana += 2; });
            //    yield return arg.Clone().With(p => { p.RedGreenMana -= 1; p.BlackMana += 1; });
            //}
            //if (arg.RedGreenMana > 0)
            //{
            //    if (arg.BlueMana > 0)
            //    {
            //        yield return arg.Clone().With(p => { p.BlackMana += 2; p.BlueMana--; p.RedGreenMana--; });
            //        yield return arg.Clone().With(p => { p.BlackMana += 1; p.BlueMana--; });
            //    }
               
            //    if (arg.BlackMana > 0)
            //    {
            //        yield return arg.Clone().With(p => { p.BlackMana += 1; p.RedGreenMana--; });

            //    }

            //}
        }

        private static IEnumerable<State> Beseech(State arg)
        {
            if (arg.TimingState != TimingState.MainPhase) //don't seem necessary to cast necro post necro
            {
                yield break;
            }

            if (arg.CanPay(Mana.Black, 6, 1))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
            if (arg.BargainFodder > 0 && arg.CanPay(Mana.Black, 3, 1))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
        }

        private static IEnumerable<State> Necro(State arg)
        {
            if (arg.TimingState != TimingState.MainPhase)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Black, 3))
            {
                if (Global.RunPostNecro)
                {
                    yield return arg.Clone().With(p =>
                    {
                        p.TimingState = TimingState.InstantOnly;
                        p.DrawCards(19);
                    });
                }
                else
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }
            }
        }

        private static IEnumerable<State> Necrologia(State arg)
        {
            if (arg.TimingState != TimingState.MainPhase)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Black, 2, 3))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
        }

        private static IEnumerable<State> ChromeMox(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            foreach (var x in arg.Cards)
            {
                var color = ColorDict[x];
                if (color.HasFlag(Mana.Black))
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.RemoveCard(x); });
                }
                if (color.HasFlag(Mana.Red))
                {
                    yield return arg.Clone().With(p => { p.RedMana++; p.BargainFodder++; p.RemoveCard(x);  });
                }
                if (color.HasFlag(Mana.Green))
                {
                    yield return arg.Clone().With(p => { p.GreenMana++; p.BargainFodder++; p.RemoveCard(x); });
                }
                if (color.HasFlag(Mana.Blue))
                {
                    yield return arg.Clone().With(p => { p.BlueMana++; p.BargainFodder++; p.RemoveCard(x); });
                }
            }


            yield return arg.Clone().With(p => { p.BargainFodder++; }); //no imprint

        }

        private static IEnumerable<State> LotusPetal(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            yield return arg.Clone().With(p => p.AnyMana++);
            yield return arg.Clone().With(p => p.BargainFodder++);

        }

        private static IEnumerable<State> WildCantor(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Red |Mana.Green, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Red | Mana.Green, 1))
                {
                    yield return x.With(p => p.AnyMana += 1);
                }
            }
        }

        private static IEnumerable<State> GemstoneMine(State arg)
        {
            if (arg.TimingState != TimingState.MainPhase)
            {
                yield break;
            }
            if (arg.LandDrops == 0)
            {
                yield return arg.Clone().With(p => { p.AnyMana += 1; p.LandDrops++; });
                //yield return arg.Clone().With(p => { p.RedGreenMana += 1; p.LandDrops++; });
            }
        }
        private static IEnumerable<State> FlipLand(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.LandDrops == 0)
            {
                yield return arg.Clone().With(p => { p.BlackMana += 1; p.LandDrops++; });
            }
        }

        private static IEnumerable<State> VaultOfWhispers(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.LandDrops == 0)
                yield return arg.Clone().With(p => { p.BlackMana += 1; p.LandDrops++; p.BargainFodder++; });

        }

        private static IEnumerable<State> ElvishSpiritGuide(State arg)
        {
            yield return arg.Clone().With(p => p.GreenMana++);
        }
        private static IEnumerable<State> SimianSpiritGuide(State arg)
        {
            yield return arg.Clone().With(p => p.RedMana++);
        }

        private static IEnumerable<State> CabalRitual(State arg)
        {
            if (arg.CanPay(Mana.Black, 1, 1))
            {

                foreach (var x in arg.WaysToPay(Mana.Black, 1, 1))
                {
                    yield return x.With(p => p.BlackMana += 3);
                }

                //var clone = arg.Clone().With(p => p.Pay(Mana.Black, 1));
                //if (clone.BlueMana > 0)
                //{
                //    yield return arg.Clone().With(p => { p.BlackMana += 2; p.BlueMana--; });
                //}
                //if (clone.RedMana > 0)
                //{
                //    yield return arg.Clone().With(p => { p.BlackMana += 2; p.RedGreenMana--; });
                //}
                //if (clone.GreenMana > 0)
                //{
                //    yield return arg.Clone().With(p => { p.BlackMana += 2; p.RedGreenMana--; });
                //}
                //if (clone.BlackMana > 1)
                //{
                //    yield return arg.Clone().With(p => p.BlackMana += 1);
                //}

            }
        }

        private static IEnumerable<State> DarkRitual(State arg)
        {
            if (arg.CanPay(Mana.Black, 1))
            {
                return arg.Pay(Mana.Black, 1).Select(q => q.With(p => p.BlackMana += 3));
            }
            return Enumerable.Empty<State>();
        }

        internal static IEnumerable<State> GetResult(int card1, State state)
        {
            return RuleDict[card1](state);
        }
    }
}
[Flags]
enum Mana
{
    Black = 1,
    Red = 2,
    Green = 4,
    Blue = 8,
    None = 16

}