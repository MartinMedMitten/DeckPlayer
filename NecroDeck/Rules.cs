using System;
using System.Collections.Generic;
using System.Linq;

namespace NecroDeck
{
    interface StructureWrapper<T>
    {
        void Enqueue(T item);
        T Dequeue();
        bool Any();
    }

    class StackWrapper<T> : StructureWrapper<T>
    {
        private Stack<T> b = new Stack<T>();

        public bool Any()
        {
            return b.Any();
        }

        public T Dequeue()
        {
            return b.Dequeue();
        }

        public void Enqueue(T item)
        {
            b.Enqueue(item);
        }
    }
    class QueueWrapper<T> : StructureWrapper<T>
    {
        private Queue<T> b = new Queue<T>();

        public bool Any()
        {
            return b.Any();
        }

        public T Dequeue()
        {
            return b.Dequeue();
        }

        public void Enqueue(T item)
        {
            b.Enqueue(item);
        }
    }
    static class Utility
    {

        public static int FirstIndexOf<T>(this IList<T> list, Func<T, bool> func)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (func(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static IEnumerable<T> ExceptItem<T>(this IEnumerable<T> e, T item)
        {
            foreach (var x in e)
            {
                if (!x.Equals(item))
                {
                    yield return x;
                }
            }
        }

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

                return Tendrils;
            }
            if (v == "valakut awakening" )
            {
                ColorDict[i] = Mana.Red;

                return Valakut;
            }
                
            if ( v == "fateful showdown")
            {
                ColorDict[i] = Mana.Red; 

                return NoOp;
            }
            if (v == "electrodominance")
            {
                ColorDict[i] = Mana.Red;
                return ElectroDominance;
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
            if (v == "grief")
            {
                ColorDict[i] = Mana.Black;

                return NoOp;
            }
            if (v == "infernal tutor")
            {
                ColorDict[i] = Mana.Black;

                return InfernalTutor;
            }
            if (v == "gaea's will")
            {
                ColorDict[i] = Mana.Green;

                return NoOp;
            }
            if (v == "lion's eye diamond")
            {
                ColorDict[i] = Mana.None;

                return LionsEyeDiamond;
            }

            if (v == "necrologia")
            {
                ColorDict[i] = Mana.Black;

                return Necrologia;
            }
            if (v == "crop rotation")
            {
                ColorDict[i] = Mana.Green;

                return CropRotation;
            }
            if (v == "big score")
            {
                ColorDict[i] = Mana.Red;

                return BigScore;
            }  
            if (v == "onetreasure")
            {
                ColorDict[i] = Mana.Red;

                return OneTreasure;
            }

            throw new NotImplementedException();
        }

        private static IEnumerable<State> Valakut(State arg)
        {
            if (arg.TimingState == TimingState.MainPhase)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Red, 1, 2))
            {
                //om jag tar fram first index of för manamorphose, bourne, och beseech tendrils samt alla spirit guides
                //sen slänger resten och drar så många nya. Då har vi kanske de klart och betart
                var l = arg.Cards.Where(p => !Global.Deck.Cards[p].StartsWith("tend") && !Global.Deck.Cards[p].Contains("spirit")).ToList();
                //sen ta bort alla utom första manamorphose, bourne och beseech
                var borne = l.FirstIndexOf(x => Global.Deck.Cards[x].StartsWith("born"));
                var manamorphose = l.FirstIndexOf(x => Global.Deck.Cards[x].StartsWith("manamorp"));

                var toDiscard = l.ExceptItem(borne).ExceptItem(manamorphose).ToList();
                int toDraw = toDiscard.Count;
                foreach (var x in arg.WaysToPay(Mana.Red, 1, 2))
                {
                    yield return x.With(p =>
                    {
                        p.Cards = p.Cards.Except(toDiscard).ToList();
                        p.DrawCards(toDraw);
                    });
                }
            }
        }

        private static IEnumerable<State> OneTreasure(State arg)
        {
            if (arg.TimingState == TimingState.MainPhase)
            {
                yield break;
            }
            if (arg.CanPay(Mana.Red, 1, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Red, 1, 1))
                {
                    yield return x.With(p =>
                    {
                        p.AnyMana += 1;
                    });
                }
            }
        }

        private static IEnumerable<State> ElectroDominance(State arg)
        {
            if (arg.TimingState != TimingState.InstantOnly)
            {
                yield break;
            }
            if (arg.CanPay(Mana.Red, 2, 2))
            {
                var borne = arg.Cards.FirstIndexOf(q => Global.Deck.Cards[q].StartsWith("bor"));
                if (borne != -1)
                {
                    foreach (var x in arg.WaysToPay(Mana.Red, 2, 2))
                    {
                        yield return x.With(p =>
                        {
                            p.TimingState = TimingState.Borne;
                            p.DrawCards(1);
                            p.RemoveCard(p.Cards[borne]);
                        });
                    }
                }

            }
            if (arg.CanPay(Mana.Red, 2, 4))
            {
                var beseech = arg.Cards.FirstIndexOf(q => Global.Deck.Cards[q].StartsWith("bes"));
                if (beseech != -1)
                {
                    yield return arg.Clone().With(p =>
                    {
                        p.Win = true;
                    });
                }
            }

            //de är nog bara bourne och beseech
        }

        private static IEnumerable<State> BigScore(State arg)
        {
            if (arg.TimingState == TimingState.MainPhase)
            {
                yield break;
            }
            if (arg.CanPay(Mana.Red, 1, 3))
            {
                foreach (var x in arg.WaysToPay(Mana.Red, 1, 3))
                {
                    yield return x.With(p =>
                    {
                        p.AnyMana += 2;
                        p.DrawCards(2);
                    });
                }
            }
        }

        private static IEnumerable<State> Tendrils(State arg)
        {
            if (arg.TimingState != TimingState.Borne)
            {
                yield break;
            }
            if (arg.CanPay(Mana.Black, 2, 2))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }

        }

        private static IEnumerable<State> CropRotation(State arg)
        {
            throw new NotImplementedException();
            yield break;
            if (arg.CanPay(Mana.Green, 1, 1))
            {

            }
        }

        private static IEnumerable<State> LionsEyeDiamond(State arg)
        {
            yield return arg.Clone().With(p =>
            {
                p.LedInPlay++;
                p.BargainFodder++;
            });
        }

        private static IEnumerable<State> InfernalTutor(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Black, 1, 1))
            {
                if (arg.CardsInHand <= 1 && arg.CanPay(Mana.Black, 4)) 
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }
                if (arg.LedInPlay > 0)
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }

            }
        }

        private static IEnumerable<State> Borne(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                if (arg.CanPay(Mana.Blue, 1, 1))
                {
                    //yield return arg.Clone().With(p => p.Win = true);
                    foreach (var x in arg.WaysToPay(Mana.Blue, 1, 1))
                    {
                        yield return x.With(p =>
                        {
                            p.TimingState = TimingState.Borne;
                            p.DrawCards(1);
                        });

                    }
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
            if (Global.ContainsCantor && !arg.RunState.CantorInHand && arg.TimingState != TimingState.InstantOnly) //TODO if wildcantor is in deck and not it hand
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
                    if (false && arg.TimingState != TimingState.MainPhase) //because i don't want to effect mulligan decisions
                    {
                        yield return x.With(p =>
                        {
                            p.AnyMana += 2;
                            p.DrawCards(1);
                        });
                    }
                    else
                    {
                        yield return x.With(p => p.AnyMana += 2);
                    }
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
            if (arg.TimingState == TimingState.InstantOnly) //don't seem necessary to cast necro post necro
            {
                yield break;
            }
            if (arg.TimingState == TimingState.Borne)
            {
                if (arg.CanPay(Mana.Black, 3, 1))
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }
            }
            else
            {
                if (Global.RunPostNecro)
                {
                    if (arg.CanPay(Mana.Black, 6, 1))
                    {
                        foreach (var y in arg.WaysToPay(Mana.Black, 6, 1))
                        {
                            yield return y.With(p =>
                            {
                                p.TimingState = TimingState.InstantOnly;
                                p.DrawCards(19);
                            });
                        }
                    }
                    if (arg.BargainFodder > 0 && arg.CanPay(Mana.Black, 3, 1))
                    {
                        foreach (var y in arg.WaysToPay(Mana.Black, 3, 1))
                        {
                            yield return y.With(p =>
                            {
                                p.TimingState = TimingState.InstantOnly;
                                p.DrawCards(19);
                            });
                        }
                    }
                }
                else
                {
                    if (arg.CanPay(Mana.Black, 6, 1))
                    {
                        yield return arg.Clone().With(p => p.Win = true);
                    }
                    if (arg.BargainFodder > 0 && arg.CanPay(Mana.Black, 3, 1))
                    {
                        yield return arg.Clone().With(p => p.Win = true);
                    }
                }
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
                    foreach (var y in arg.WaysToPay(Mana.Black, 3))
                    {
                        yield return y.With(p =>
                        {
                            p.TimingState = TimingState.InstantOnly;
                            p.DrawCards(19);
                        });
                    }
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

            if (Global.RunPostNecro)
            {
                if (arg.CanPay(Mana.Black, 2, 3))
                {
                    foreach (var y in arg.WaysToPay(Mana.Black, 2, 3))
                    {
                        yield return y.Clone().With(p =>
                        {
                            p.TimingState = TimingState.InstantOnly;
                            p.DrawCards(19);
                        });
                    }
                }
            }
            else
            {
                if (arg.CanPay(Mana.Black, 2, 3))
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }
            }
        }

        private static IEnumerable<State> ChromeMox(State arg)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            for (int i = arg.Cards.Count-1; i >= 0; i--)
            {
                var x = arg.Cards[i];

            
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