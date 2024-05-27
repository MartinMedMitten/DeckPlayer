using System;
using System.Collections.Generic;

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
    enum Color
    {
        Black,
        RedGreen,
        Other,
        None
    }
    class Rules
    {
        public static Dictionary<int, Func<State, IEnumerable<State>>> RuleDict = new Dictionary<int, Func<State, IEnumerable<State>>>();
        public static Dictionary<int, Color> ColorDict = new Dictionary<int, Color>();

        public static void InitRuleDict(Deck deck)
        {
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                RuleDict[i] = GetFunc(deck.Cards[i], i);

            }
        }

        private static Func<State, IEnumerable<State>> GetFunc(string v, int i)
        {
            if (v == "DarkRitual")
            {
                ColorDict[i] = Color.Black;
                return DarkRitual;
            }
            if (v == "CabalRitual")
            {
                ColorDict[i] = Color.Black;
                return CabalRitual;
            }
            if (v == "SpiritGuide")
            {
                ColorDict[i] = Color.RedGreen;

                return SpiritGuide;
            }
            if (v == "VaultOfWhispers")
            {
                ColorDict[i] = Color.None;

                return VaultOfWhispers;
            }
            if (v == "GemstoneMine")
            {
                ColorDict[i] = Color.None;

                return GemstoneMine;
            }
            if (v == "WildCantor")
            {
                ColorDict[i] = Color.RedGreen;

                return WildCantor;
            }
            if (v == "LotusPetal")
            {
                ColorDict[i] = Color.None;

                return LotusPetal;
            }
            if (v == "ChromeMox")
            {
                ColorDict[i] = Color.None;

                return ChromeMox;
            }
            if (v == "Necro")
            {
                ColorDict[i] = Color.Black;

                return Necro;
            }
            if (v == "Beseech")
            {
                ColorDict[i] = Color.Black;

                return Beseech;
            }
            if (v == "Manamorphose")
            {
                ColorDict[i] = Color.RedGreen;

                return Manamorphose;
            }    
            if (v == "Borne")
            {
                ColorDict[i] = Color.Other;

                return NoOp;
            }
            if (v == "PactofNegation")
            {
                ColorDict[i] = Color.Other;

                return NoOp;
            }
            if (v == "Tendrils")
            {
                ColorDict[i] = Color.None; //can't imprint tendrils so i fake it.

                return NoOp;
            }
            if (v == "Valakut")
            {
                ColorDict[i] = Color.RedGreen; 

                return NoOp;
            }
            if (v == "SummonersPact")
            {
                ColorDict[i] = Color.RedGreen; 

                return SummonersPact;
            }
            if (v == "BrainSpoil")
            {
                ColorDict[i] = Color.Black; 

                return BrainSpoil;
            }

            throw new NotImplementedException();
        }

        private static IEnumerable<State> BrainSpoil(State arg)
        {
            if (arg.BlackMana > 4 && (arg.BlackMana > 5 || arg.RedGreenMana > 0 || arg.OtherMana > 0))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
        }

        private static IEnumerable<State> SummonersPact(State arg)
        {
            yield return arg.Clone().With(p => p.RedGreenMana++);
            if (arg.RedGreenMana > 0 && Global.ContainsCantor && !arg.RunState.CantorInHand) //TODO if wildcantor is in deck and not it hand
            {
                yield return arg.Clone().With(p => { p.RedGreenMana--; p.BlackMana++; });
            }

            //yield return arg.Clone().With(p => { p.BargainFodder++; }); //but no such card exist
        }

        private static IEnumerable<State> NoOp(State arg)
        {
            yield break;
        }

        private static IEnumerable<State> Manamorphose(State arg)
        {
            if (arg.RedGreenMana > 1)
            {
                yield return arg.Clone().With(p => { p.RedGreenMana -= 2; p.BlackMana += 2; });
                yield return arg.Clone().With(p => { p.RedGreenMana -= 1; p.BlackMana += 1; });
            }
            if (arg.RedGreenMana > 0)
            {
                if (arg.OtherMana > 0)
                {
                    yield return arg.Clone().With(p => { p.BlackMana += 2; p.OtherMana--; p.RedGreenMana--; });
                    yield return arg.Clone().With(p => { p.BlackMana += 1; p.OtherMana--; });
                }
               
                if (arg.BlackMana > 0)
                {
                    yield return arg.Clone().With(p => { p.BlackMana += 1; p.RedGreenMana--; });

                }

            }
        }

        private static IEnumerable<State> Beseech(State arg)
        {
            if (arg.BlackMana > 5 && (arg.BlackMana > 6 ||arg.OtherMana > 0 || arg.RedGreenMana > 0))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
            if (arg.BargainFodder > 0 && arg.BlackMana > 2 && (arg.BlackMana > 3 || arg.OtherMana > 0 || arg.RedGreenMana > 0))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
        }

        private static IEnumerable<State> Necro(State arg)
        {
            if (arg.BlackMana > 2)
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
        }

        private static IEnumerable<State> ChromeMox(State arg)
        {
            foreach (var x in arg.Cards)
            {
                var color = ColorDict[x];
                if (color == Color.Black)
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.RemoveCard(x); });
                }
                if (color == Color.RedGreen)
                {
                    yield return arg.Clone().With(p => { p.RedGreenMana++; p.BargainFodder++; p.RemoveCard(x);  });
                }
                if (color == Color.Other)
                {
                    yield return arg.Clone().With(p => { p.OtherMana++; p.BargainFodder++; p.RemoveCard(x); });
                }
            }


            yield return arg.Clone().With(p => { p.BargainFodder++; }); //no imprint

        }

        private static IEnumerable<State> LotusPetal(State arg)
        {
            yield return arg.Clone().With(p => p.BlackMana++);
            yield return arg.Clone().With(p => p.BargainFodder++);
            yield return arg.Clone().With(p => p.RedGreenMana++);

        }

        private static IEnumerable<State> WildCantor(State arg)
        {
            if (arg.RedGreenMana > 0)
                yield return arg.Clone().With(p => { p.BlackMana += 1; p.RedGreenMana--; });
        }

        private static IEnumerable<State> GemstoneMine(State arg)
        {
            if (arg.LandDrops == 0)
            {
                yield return arg.Clone().With(p => { p.BlackMana += 1; p.LandDrops++; });
                yield return arg.Clone().With(p => { p.RedGreenMana += 1; p.LandDrops++; });
            }
        }
        private static IEnumerable<State> FlipLand(State arg)
        {
            if (arg.LandDrops == 0)
            {
                yield return arg.Clone().With(p => { p.BlackMana += 1; p.LandDrops++; });
            }
        }

        private static IEnumerable<State> VaultOfWhispers(State arg)
        {
            if (arg.LandDrops == 0)
                yield return arg.Clone().With(p => { p.BlackMana += 1; p.LandDrops++; p.BargainFodder++; });

        }

        private static IEnumerable<State> SpiritGuide(State arg)
        {
            yield return arg.Clone().With(p => p.RedGreenMana++);
        }

        private static IEnumerable<State> CabalRitual(State arg)
        {
            if (arg.BlackMana > 0)
            {
                if (arg.OtherMana > 0)
                {
                    yield return arg.Clone().With(p => { p.BlackMana += 2; p.OtherMana--; });
                }
                if (arg.RedGreenMana > 0)
                {
                    yield return arg.Clone().With(p => { p.BlackMana += 2; p.RedGreenMana--; });
                }
                if (arg.BlackMana > 1)
                {
                    yield return arg.Clone().With(p => p.BlackMana += 1);
                }

            }
        }

        private static IEnumerable<State> DarkRitual(State arg)
        {
            if (arg.BlackMana > 0)
            {
                yield return arg.Clone().With(p => p.BlackMana += 2);
            }
        }

        internal static IEnumerable<State> GetResult(int card1, State state)
        {
            return RuleDict[card1](state);
        }
    }
}
