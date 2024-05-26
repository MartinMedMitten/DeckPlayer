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
                ColorDict[i] = Color.None; //cant imprint

                return NoOp;
            }
            if (v == "Valakut")
            {
                ColorDict[i] = Color.RedGreen; //cant imprint

                return NoOp;
            }
            if (v == "SummonersPact")
            {
                ColorDict[i] = Color.RedGreen; //cant imprint

                return SummonersPact;
            }

            throw new NotImplementedException();
        }

        private static IEnumerable<State> SummonersPact(State arg)
        {
            yield return arg.Clone().With(p => p.RedGreenMana++);
            if (arg.RedGreenMana > 0) //TODO inte om man redan har va heter den
            {
                yield return arg.Clone().With(p => { p.RedGreenMana--; p.BlackMana++; });
            }
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
            }
            if (arg.RedGreenMana > 0)
            {
                if (arg.OtherMana > 0)
                {
                    yield return arg.Clone().With(p => { p.BlackMana += 2; p.OtherMana--; p.RedGreenMana--; });
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
            ImmutableList
        }

        private static IEnumerable<State> ChromeMox(State arg)
        {
           
            if (arg.Card1 >= 0)
            {
                var color = ColorDict[arg.Card1];
                if (color == Color.Black) 
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.Card1 = -1; p.CardsInHand--; });
                }
                if (color == Color.RedGreen)
                {
                    yield return arg.Clone().With(p => { p.RedGreenMana++; p.BargainFodder++; p.Card1 = -1; p.CardsInHand--; });
                }
                if (color == Color.Other)
                {
                    yield return arg.Clone().With(p => { p.OtherMana++; p.BargainFodder++; p.Card1 = -1; p.CardsInHand--; });
                }
            }

            if (arg.Card2 >= 0)
            {
                var color = ColorDict[arg.Card2];
                if (color == Color.Black)
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.Card2 = -1; p.CardsInHand--; });
                }
                if (color == Color.RedGreen)
                {
                    yield return arg.Clone().With(p => { p.RedGreenMana++; p.BargainFodder++; p.Card2 = -1; p.CardsInHand--; });
                }
                if (color == Color.Other)
                {
                    yield return arg.Clone().With(p => { p.OtherMana++; p.BargainFodder++; p.Card2 = -1; p.CardsInHand--; });
                }
            }

            if (arg.Card3 >= 0)
            {
                var color = ColorDict[arg.Card3];
                if (color == Color.Black)
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.Card3 = -1; p.CardsInHand--; });
                }
                if (color == Color.RedGreen)
                {
                    yield return arg.Clone().With(p => { p.RedGreenMana++; p.BargainFodder++; p.Card3 = -1; p.CardsInHand--; });
                }
                if (color == Color.Other)
                {
                    yield return arg.Clone().With(p => { p.OtherMana++; p.BargainFodder++; p.Card3 = -1; p.CardsInHand--; });
                }
            }

            if (arg.Card4 >= 0)
            {
                var color = ColorDict[arg.Card4];
                if (color == Color.Black)
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.Card4 = -1; p.CardsInHand--; });
                }
                if (color == Color.RedGreen)
                {
                    yield return arg.Clone().With(p => { p.RedGreenMana++; p.BargainFodder++; p.Card4 = -1; p.CardsInHand--; });
                }
                if (color == Color.Other)
                {
                    yield return arg.Clone().With(p => { p.OtherMana++; p.BargainFodder++; p.Card4 = -1; p.CardsInHand--; });
                }
            }
            if (arg.Card5 >= 0)
            {
                var color = ColorDict[arg.Card5];
                if (color == Color.Black)
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.Card5 = -1; p.CardsInHand--; });
                }
                if (color == Color.RedGreen)
                {
                    yield return arg.Clone().With(p => { p.RedGreenMana++; p.BargainFodder++; p.Card5 = -1; p.CardsInHand--; });
                }
                if (color == Color.Other)
                {
                    yield return arg.Clone().With(p => { p.OtherMana++; p.BargainFodder++; p.Card5 = -1; p.CardsInHand--; });
                }
            }
            if (arg.Card6 >= 0)
            {
                var color = ColorDict[arg.Card6];
                if (color == Color.Black)
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.Card6 = -1; p.CardsInHand--; });
                }
                if (color == Color.RedGreen)
                {
                    yield return arg.Clone().With(p => { p.RedGreenMana++; p.BargainFodder++; p.Card6 = -1; p.CardsInHand--; });
                }
                if (color == Color.Other)
                {
                    yield return arg.Clone().With(p => { p.OtherMana++; p.BargainFodder++; p.Card6 = -1; p.CardsInHand--; });
                }
            }
            if (arg.Card7 >= 0)
            {
                var color = ColorDict[arg.Card7];
                if (color == Color.Black)
                {
                    yield return arg.Clone().With(p => { p.BlackMana++; p.BargainFodder++; p.Card7 = -1; p.CardsInHand--; });
                }
                if (color == Color.RedGreen)
                {
                    yield return arg.Clone().With(p => { p.RedGreenMana++; p.BargainFodder++; p.Card7 = -1; p.CardsInHand--; });
                }
                if (color == Color.Other)
                {
                    yield return arg.Clone().With(p => { p.OtherMana++; p.BargainFodder++; p.Card7 = -1; p.CardsInHand--; });
                }
            }

            yield return arg.Clone().With(p => { p.BargainFodder++; });

            //todo ingen imprint på chromemox! TESTA INFERNAL TUTOR
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
