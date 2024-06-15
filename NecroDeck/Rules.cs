using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NecroDeck.Cards;
namespace NecroDeck
{
    class MetaData
    {
        

        public MetaData(Mana color, bool bargainable=false, bool artifact = false)
        {
            Color = color;
            Bargainable = bargainable;
            Artifact = artifact;
        }

        public Mana Color { get; set; } 
        public bool Bargainable { get; set; }

        public bool Artifact { get; set; }
    }

    class Rules
    {
        public static Dictionary<int, Func<State, int, IEnumerable<State>>> RuleDictFromHand = new Dictionary<int, Func<State, int, IEnumerable<State>>>();
        public static Dictionary<int, Func<State, int, IEnumerable<State>>> RuleDictFromPlay = new Dictionary<int, Func<State, int, IEnumerable<State>>>();
        public static Dictionary<int, MetaData> MetadataDictionary = new Dictionary<int, MetaData>();

        public static void InitRuleDict(Deck deck)
        {
            var cardTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CardMetaData)))
            .ToList();

            var allCards = cardTypes.Select(p => Activator.CreateInstance(p) as CardMetaData).ToDictionary(q => q.Name, q => q);

            for (int i = 0; i < deck.Cards.Count; i++)
            {

                var t = allCards[deck.Cards[i]];

                RuleDictFromHand[i] = t.FromHand;
                RuleDictFromPlay[i] = GetFuncFromPlay(deck.Cards[i], i);
            }
        }
        private static Func<State, int, IEnumerable<State>> GetFuncFromPlay(string v, int i)
        {
            if (v == "lotus petal")
            {
                return LotusPetalFromBoard;
            }
            if (v == "wild cantor")
            {
                return WildCantorFromBoard;
            }
            if (v == "gemstone mine")
            {
                return GemstoneMineFromBoard;
            }
            if (v == "vault of whispers")
            {
                return VaultOfWhispersFromBoard;
            }
            if (v == "tree of tales")
            {
                return TreeOfTalesFromBoard;
            }
            if (v == "chrome mox")
            {
                return ChromeMoxFromBoard;
            }
            if (v == "mox opal")
            {
                return MoxOpalFromBoard;
            }

            return NoOp;
            throw new ApplicationException("Missing card from play: " + Global.Deck.Cards[i]);
        }
        private static Func<State, int, IEnumerable<State>> GetFunc(string v, int i)
        {
            if (v == "tree of tales")
            {
                MetadataDictionary[i] = new MetaData(Mana.None, true, true);

                return ArtifactLand;
            }
            if (v == "mox opal")
            {
                MetadataDictionary[i] = new MetaData(Mana.None, true, true);

                return MoxOpal;
            }
            if (v == "dark ritual")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black);
                return DarkRitual;
            }
            if (v == "cabal ritual")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black);
                return CabalRitual;
            }
            if (v == "elvish spirit guide")
            {
                MetadataDictionary[i] = new MetaData(Mana.Green);

                return ElvishSpiritGuide;
            }
            if (v == "blue spirit guide")
            {
                MetadataDictionary[i] = new MetaData(Mana.Blue);

                return BlueSpiritGuide;
            }
            if (v == "black spirit guide")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black);

                return BlackSpiritGuide;
            }
            if (v == "simian spirit guide")
            {
                MetadataDictionary[i] = new MetaData(Mana.Red);

                return SimianSpiritGuide;
            }
            if (v == "vault of whispers")
            {
                MetadataDictionary[i] = new MetaData(Mana.None, true, true);

                return ArtifactLand;
            }
            if (v == "gemstone mine")
            {
                MetadataDictionary[i] = new MetaData(Mana.None);

                return GemstoneMine;
            }
            if (v == "wild cantor")
            {
                MetadataDictionary[i] = new MetaData(Mana.Red | Mana.Green);// | Mana.Green; TODO FUCK, how do i represent a red green chromemox

                return WildCantor;
            }
            if (v == "lotus petal")
            {
                MetadataDictionary[i] = new MetaData(Mana.None, true, true);

                return LotusPetal;
            }
            if (v == "chrome mox")
            {
                MetadataDictionary[i] = new MetaData(Mana.None, true, true);

                return ChromeMox;
            }
            if (v == "necrodominance" || v == "necropotence")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black, true);

                return Necro;
            }
            if (v == "beseech the mirror")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black);

                return Beseech;
            }
            if (v == "manamorphose")
            {
                MetadataDictionary[i] = new MetaData(Mana.Red | Mana.Green);

                return Manamorphose;
            }    
            if (v == "borne upon a wind")
            {
                MetadataDictionary[i] = new MetaData(Mana.Blue);

                return Borne;
            }
            if (v == "pact of negation")
            {
                MetadataDictionary[i] = new MetaData(Mana.Blue);

                return NoOp;
            }
            if (v == "tendrils of agony")
            {
                MetadataDictionary[i] = new MetaData(Mana.None); //can't imprint tendrils so i fake it.

                return Tendrils;
            }
            if (v == "valakut awakening" )
            {
                MetadataDictionary[i] = new MetaData(Mana.Red);

                return Valakut;
            }
                
            if ( v == "fateful showdown")
            {
                MetadataDictionary[i] = new MetaData(Mana.Red); 

                return NoOp;
            }
            if (v == "electrodominance")
            {
                MetadataDictionary[i] = new MetaData(Mana.Red);
                return ElectroDominance;
            }
            if (v == "summoner's pact")
            {
                MetadataDictionary[i] = new MetaData(Mana.Green); 

                return SummonersPact;
            }
            if (v == "brainspoil")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black); 

                return BrainSpoil;
            }
            if (v == "grief")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black);

                return NoOp;
            }
            if (v == "infernal tutor")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black);

                return InfernalTutor;
            }
            if (v == "gaea's will")
            {
                MetadataDictionary[i] = new MetaData(Mana.Green);

                return NoOp;
            }
            if (v == "lion's eye diamond")
            {
                MetadataDictionary[i] = new MetaData(Mana.None, true, true);

                return LionsEyeDiamond;
            }

            if (v == "necrologia")
            {
                MetadataDictionary[i] = new MetaData(Mana.Black);

                return Necrologia;
            }
            if (v == "crop rotation")
            {
                MetadataDictionary[i] = new MetaData(Mana.Green);

                return CropRotation;
            }
            if (v == "big score")
            {
                MetadataDictionary[i] = new MetaData(Mana.Red);

                return BigScore; //TODO tror inte jag har gjort treasures bargainable
            }  
            if (v == "onetreasure")
            {
                MetadataDictionary[i] = new MetaData(Mana.Red);

                return OneTreasure;
            }

            throw new NotImplementedException();
        }

        private static IEnumerable<State> Valakut(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.MainPhase)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Red, 1, 2))
            {
                //om jag tar fram first index of för manamorphose, bourne, och beseech tendrils samt alla spirit guides
                //sen slänger resten och drar så många nya. Då har vi kanske de klart och betart
                var l = arg.Cards.Where(p => !Global.Deck.Cards[p].StartsWith("dark r") && !Global.Deck.Cards[p].StartsWith("lotus") && !Global.Deck.Cards[p].StartsWith("tend") && !Global.Deck.Cards[p].Contains("spirit")).ToList();
                //sen ta bort alla utom första manamorphose, bourne och beseech
                var borne = l.FirstIndexOf(x => Global.Deck.Cards[x].StartsWith("born"));
                var manamorphose = l.FirstIndexOf(x => Global.Deck.Cards[x].StartsWith("manamorp"));
                var bes = l.FirstIndexOf(x => Global.Deck.Cards[x].StartsWith("beseech"));

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

        private static IEnumerable<State> OneTreasure(State arg, int cardId)
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

        private static IEnumerable<State> ElectroDominance(State arg, int cardId)
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

        private static IEnumerable<State> BigScore(State arg, int cardId)
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

        private static IEnumerable<State> Tendrils(State arg, int cardId)
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

        private static IEnumerable<State> CropRotation(State arg, int cardId)
        {
            throw new NotImplementedException();
            yield break;
            if (arg.CanPay(Mana.Green, 1, 1))
            {

            }
        }

        private static IEnumerable<State> LionsEyeDiamond(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.LedInPlay++;
                p.BargainFodder++;
            });
        }

        private static IEnumerable<State> InfernalTutor(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Black, 1, 1))
            {
                if (arg.CardsInHand <= 1 && arg.CanPay(Mana.Black, 4)) //TODO funkar inte om jag kör med bitflag!
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }
                if (arg.LedInPlay > 0)
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }

            }
        }

        private static IEnumerable<State> Borne(State arg, int cardId)
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

        private static IEnumerable<State> BrainSpoil(State arg, int cardId)
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

        private static IEnumerable<State> SummonersPact(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.GreenMana++);
            if (Global.ContainsCantor && !arg.RunState.CantorInHand && arg.TimingState != TimingState.InstantOnly) //TODO if wildcantor is in deck and not it hand
            {
                yield return arg.Clone().With(p => { p.AddCardToHand(Global.CantorId); p.ModifyRunState(x => x.CantorInHand = true); });
            }

            //yield return arg.Clone().With(p => { p.BargainFodder++; }); //but no such card exist
        }

        private static IEnumerable<State> NoOp(State arg, int cardId)
        {
            yield break;
        }

        private static IEnumerable<State> Manamorphose(State arg, int cardId)
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

        private static IEnumerable<State> Beseech(State arg, int cardId)
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

        private static IEnumerable<State> Necro(State arg, int cardId)
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

        private static IEnumerable<State> Necrologia(State arg, int cardId)
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

        private static IEnumerable<State> ChromeMox(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            for (int i = arg.Cards.Count-1; i >= 0; i--)
            {
                var x = arg.Cards[i];

            
                var color = MetadataDictionary[x].Color;
                if (color.HasFlag(Mana.Black))
                {
                    yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, Mana.Black, true); p.BargainFodder++; p.RemoveCard(x); });
                }
                if (color.HasFlag(Mana.Red))
                {
                    yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, Mana.Red, true); p.BargainFodder++; p.RemoveCard(x);  });
                }
                if (color.HasFlag(Mana.Green))
                {
                    yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, Mana.Green, true); p.BargainFodder++; p.RemoveCard(x); });
                }
                if (color.HasFlag(Mana.Blue))
                {
                    yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, Mana.Blue, true); p.BargainFodder++; p.RemoveCard(x); });
                }
            }


            yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, null, true); }); //no imprint

        }
        private static IEnumerable<State> ChromeMoxFromBoard(State arg, int cardId)
        {
            var t = arg.CardsInPlay.Single(p => p.Card == cardId);
            if (t.Used)
            {
                yield break;
            }
            if (t.AdditionalData == null)
            {
                yield break;
            }
            var c = (Mana)t.AdditionalData;
            if (c == Mana.Black)
            {
                yield return arg.Clone().With(p =>
                {
                    p.BlackMana++;
                    p.SetUsed(t);
                });
            }
            if (c == Mana.Blue)
            {
                yield return arg.Clone().With(p =>
                {
                    p.SetUsed(t);
                    p.BlueMana++;
                });
            }
            if (c == Mana.Red)
            {
                yield return arg.Clone().With(p =>
                {
                    p.SetUsed(t);
                    p.RedMana++;
                });
            }
            if (c == Mana.Green)
            {
                yield return arg.Clone().With(p =>
                {
                    p.SetUsed(t);
                    p.GreenMana++;
                });
            }

        }
        private static IEnumerable<State> MoxOpalFromBoard(State arg, int cardId)
        {
            var metalCraft = 0;
            for (int i = 0; i < arg.CardsInPlay.Count; i++)
            {
                if (MetadataDictionary[arg.CardsInPlay[i].Card].Artifact)
                {
                    metalCraft++;
                }
                if (metalCraft >= 3)
                {
                    break;
                }
            }
            if (metalCraft < 3)
            {
                yield break;
            }
            yield return arg.Clone().With(p =>
            {
                p.SetUsed(cardId);
                p.AnyMana++;
            });
        }
        private static IEnumerable<State> MoxOpal(State arg, int card)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            yield return arg.Clone().With(p => p.AddCardsInPlay(card, null, true));

        }
       
      
        private static IEnumerable<State> WildCantorFromBoard(State arg, int card)
        {
            yield return arg.Clone().With(p =>
            {
                p.AnyMana++;
                p.RemoveCardsInPlay(card);
            });
        }

        private static IEnumerable<State> WildCantor(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Red |Mana.Green, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Red | Mana.Green, 1))
                {
                    yield return x.With(p => p.AddCardsInPlay(cardId, null, false));
                }
            }
        }

        private static IEnumerable<State> GemstoneMine(State arg, int cardId)
        {
            if (arg.TimingState != TimingState.MainPhase)
            {
                yield break;
            }
            if (arg.LandDrops == 0)
            {
                yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, null, false); p.LandDrops++; });
            }
        }

        private static IEnumerable<State> GemstoneMineFromBoard(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.SetUsed(cardId);
                p.AnyMana++;
            });
        }

        private static IEnumerable<State> FlipLand(State arg, int cardId)
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

        private static IEnumerable<State> ArtifactLand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.LandDrops == 0)
                yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, null, true); p.LandDrops++; });

        }
        private static IEnumerable<State> VaultOfWhispersFromBoard(State arg, int cardId)
        {
                yield return arg.Clone().With(p =>
                {
                    p.SetUsed(cardId);
                    p.BlackMana += 1;
                });

        }
        private static IEnumerable<State> TreeOfTalesFromBoard(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.SetUsed(cardId);
                p.GreenMana += 1;
            });

        }
        private static IEnumerable<State> ElvishSpiritGuide(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.GreenMana++);
        }
        private static IEnumerable<State> BlueSpiritGuide(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.BlueMana++);
        }
        private static IEnumerable<State> BlackSpiritGuide(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.BlackMana++);
        }
        private static IEnumerable<State> SimianSpiritGuide(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.RedMana++);
        }

        private static IEnumerable<State> CabalRitual(State arg, int cardId)
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

        private static IEnumerable<State> DarkRitual(State arg, int cardId)
        {
            if (arg.CanPay(Mana.Black, 1))
            {
                return arg.Pay(Mana.Black, 1).Select(q => q.With(p => p.BlackMana += 3));
            }
            return Enumerable.Empty<State>();
        }

        internal static IEnumerable<State> GetResult(int card1, State state)
        {
            return RuleDictFromHand[card1](state, card1);
        }
        internal static IEnumerable<State> GetResultFromInPlay(int card1, State state)
        {
            return RuleDictFromPlay[card1](state, card1);
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