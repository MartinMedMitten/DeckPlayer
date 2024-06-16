using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NecroDeck
{
    class RunState
    {
        public bool CantorInHand { get; set; }
        public Random Random { get; internal set; }
        public int SerumPowder { get; internal set; }

        public HashSet<int> DrawnCards = new HashSet<int>();

        public RunState Clone()
        {
            return new RunState
            {
                CantorInHand = CantorInHand,
                DrawnCards = new HashSet<int>(DrawnCards),
                Random = Random,
                SerumPowder = SerumPowder,

            };
        }
    }

    enum TimingState
    {
        MainPhase,
        InstantOnly,
        Borne
    }
    class CardInPlay
    {
        public int Card;
        public bool Used;
        public object AdditionalData;

        public CardInPlay(int card, object additionalData)
        {
            Card = card;
            AdditionalData = additionalData;
        }

        internal CardInPlay MakeUsed()
        {
            return new CardInPlay(Card, AdditionalData)
            {
                Used = true
            };
        }
    }
    class State
    {
        public ulong CardsInHandBitflag;

        public void ListToBitflag()
        {
            CardsInHandBitflag = 0;
            foreach (int num in Cards)
            {
                if (num >= 0 && num <= 60) // Ensure the number is within the valid range
                {
                    CardsInHandBitflag |= (1UL << num);
                }
            }
            //return CardsInHandBitflag; //MARTIN, MEN TÄNK PÅ OM DE ÄR SAMMA TYP, 2 DARK RITUAL, SPELAR INGEN ROLL VILKEN AV DEM MAN HAR I HANDEN.
        }
        public List<int> BitflagToList()
        {
            List<int> intList = new List<int>();
            for (int i = 0; i <= 60; i++)
            {
                if ((CardsInHandBitflag & (1UL << i)) != 0)
                {
                    intList.Add(i);
                }
            }
            return intList;
        }
        public bool HasCardInHand(int num)
        {
            if (num >= 0 && num <= 60) // Ensure the number is within the valid range
            {
                return (CardsInHandBitflag & (1UL << num)) != 0;
            }
            return false; // Return false if the number is out of range
        }
        public void AddToBitflag(int num)
        {
            if (num >= 0 && num <= 60) // Ensure the number is within the valid range
            {
                CardsInHandBitflag |= (1UL << num);
            }
        }
        public void RemoveFromBitflag(int num)
        {
            if (num >= 0 && num <= 60) // Ensure the number is within the valid range
            {
                CardsInHandBitflag &= ~(1UL << num);
            }
            else
            {

            }
        }

        public List<int> Cards;
        public List<CardInPlay> CardsInPlay;
        
      
        public void AddCardsInPlay(int card, object additionalData, bool bargainable)
        {
            CardsInPlay = new List<CardInPlay>(CardsInPlay);
            CardsInPlay.Add(new CardInPlay(card, additionalData));
            if (bargainable)
            {
                BargainFodder++;
            }
        }

        internal void SetUsed(CardInPlay t)
        {
            CardsInPlay = new List<CardInPlay>(CardsInPlay);
            CardsInPlay.Remove(t);
            CardsInPlay.Add(t.MakeUsed());
        }
        internal void SetUsed(int t)
        {
            var tc = CardsInPlay.Single(p => p.Card == t);
            SetUsed(tc);
        }
        public void RemoveCardsInPlay(int card)
        {
            CardsInPlay = new List<CardInPlay>(CardsInPlay);
            CardsInPlay.RemoveAll(p => p.Card == card);
        }
        public RunState RunState { get; set; }

        public void ModifyRunState(Action<RunState> modaction)
        {
            RunState = RunState.Clone();
            modaction(RunState);
        }

        public void RemoveCard(int x)
        {
            //if (!IsFlagSet(x))
            //{

            //}
            RemoveFromBitflag(x);
            //if (IsFlagSet(x))
            //{nånting är buggat, de borde fungera även om jag tar bort den där cards...

            //}
            //Cards = Cards.ExceptItem(x).ToList();
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var x in Cards)
            {
                sb.Append(Global.Deck.Cards[x]);
                sb.Append(", ");
            }
           
            Console.WriteLine(sb.ToString());
        }
        public string PrintMana()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string('B', BlackMana));
            sb.Append(new string('R', RedMana));
            sb.Append(new string('G', GreenMana));
            sb.Append(new string('U', BlueMana));
            sb.Append(new string('*', AnyMana));

            return sb.ToString();
        }

        public int CardsInHandCount => Cards.Where(q => HasCardInHand(q)).Count();
        

        public int TotalMana => BlackMana + BlueMana + AnyMana + RedMana + GreenMana;

        public State Parent { get; private set; }
        public bool Powderable { get; internal set; }
        public string Text { get; internal set; }

        public int LedInPlay = 0;
        public int AnyMana = 0;
        public int BlackMana = 0;
        public int RedMana = 0;
        public int GreenMana = 0;
        public int BlueMana = 0;
        public int LandDrops = 0;
        public int BargainFodder = 0;
        public TimingState TimingState;
        public bool Win;

        public State Clone()
        {
            return new State
            {
                LedInPlay = LedInPlay,
                RunState = RunState,
                Cards = Cards,
                CardsInPlay = CardsInPlay,
                BlackMana = BlackMana,
                BlueMana = BlueMana,
                RedMana = RedMana,
                GreenMana = GreenMana,
                AnyMana = AnyMana,
                LandDrops = LandDrops,
                BargainFodder = BargainFodder,
                Win = Win,
                TimingState = TimingState,
                CardsInHandBitflag = CardsInHandBitflag,
                Parent = this
            };
        }

        internal void ClearMana()
        {
            BlackMana = 0;
            AnyMana = 0;
            RedMana = 0;
            GreenMana = 0;
            BlueMana = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is State other)
            {
                return  
                       BlackMana == other.BlackMana &&
                       BlueMana == other.BlueMana &&
                       AnyMana == other.AnyMana &&
                       RedMana == other.RedMana &&
                       GreenMana == other.GreenMana &&
                       LandDrops == other.LandDrops &&
                       BargainFodder == other.BargainFodder &&
                       TimingState == other.TimingState &&
                       CardsInHandBitflag == other.CardsInHandBitflag &&
                       //CardCompare(other) &&
                       CardInPlayCompare(other) &&
                       Win == other.Win &&
                       LedInPlay == other.LedInPlay;
            }
            return false;
        }

        private bool CardCompare(State other)
        {
            if (Cards.Count != other.Cards.Count)
            {
                return false;
            }

            return Cards.All(p => other.Cards.Contains(p));

        }
        private bool CardInPlayCompare(State other)
        {
            if (CardsInPlay == other.CardsInPlay)
            {
                return true;
            }
            if (CardsInPlay.Count != other.CardsInPlay.Count)
            {
                return false;
            }

            return CardsInPlay.All(p => other.CardsInPlay.Any(q => q.Card == p.Card));

        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + CardsInHandBitflag.GetHashCode();
            hash = hash * 37 + BlackMana.GetHashCode();
            hash = hash * 41 + BlueMana.GetHashCode();
            hash = hash * 7 + AnyMana.GetHashCode();
            hash = hash * 43 + BargainFodder.GetHashCode();
            hash = hash * 31 + CardsInPlay.Count.GetHashCode();
            return hash;
        }

        internal bool ContainsCardsExact(string v)
        {
            var spl = v.Split(',').Select(p => p.Trim()).ToArray();

            List<string> c = new List<string>();
            foreach (var x in Cards)
            {
                c.Add(Global.Deck.Cards[x]);
            }

            return c.Count == spl.Length && spl.All(q => c.Contains(q));

            
        }
        internal bool ContainsCards(string v)
        {
            var spl = v.Split(',').Select(p => p.Trim().ToLower()).ToArray();

            List<string> c = new List<string>();
            foreach (var x in Cards)
            {
                c.Add(Global.Deck.Cards[x].ToLower());
            }

            return spl.All(q => c.Contains(q));


        }

        internal void AddCardToHand(int cardId)
        {
            AddToBitflag(cardId);
            Cards = Cards.ConcatItem(cardId).ToList();
            ModifyRunState(x => x.DrawnCards = new HashSet<int>(x.DrawnCards.ConcatItem(cardId)));
        }
        internal void DrawCards(int v)
        {
            int max = Global.Deck.Cards.Count;

            var newRunState = RunState.Clone();
            var c = newRunState.DrawnCards.Count;
            while (newRunState.DrawnCards.Count - c < v)
            {
                if (newRunState.DrawnCards.Count >= max)
                {
                    break;
                }
                int number = newRunState.Random.Next(0, max);
                newRunState.DrawnCards.Add(number);
            }
            var newCards = newRunState.DrawnCards.Except(RunState.DrawnCards).ToList();

            var totalCards = new List<int>(Cards);
            foreach (var x in newCards.OrderBy(p => p))
            {
                
                totalCards.Add(x);
                AddToBitflag(x);

            }
            Cards = totalCards;
            RunState = newRunState;
        }

        internal IEnumerable<State> Pay(Mana color, int v)
        {
            if (color.HasFlag(Mana.Black))
            {
                var clone = Clone();
                clone.BlackMana -= v;
                if (clone.BlackMana < 0)
                {
                    clone.AnyMana += clone.BlackMana;
                    clone.BlackMana = 0;
                  
                }
                if (clone.AnyMana >= 0) yield return clone;
            }
            if (color.HasFlag(Mana.Blue))
            {
                var clone = Clone();
                clone.BlueMana -= v;
                if (clone.BlueMana < 0)
                {
                    clone.AnyMana += clone.BlueMana;
                    clone.BlueMana = 0;
                   
                }
                if (clone.AnyMana >= 0) yield return clone;
            }
            if (color.HasFlag(Mana.Red))
            {
                var clone = Clone();
                clone.RedMana -= v;
                if (clone.RedMana < 0)
                {
                    clone.AnyMana += clone.RedMana;
                    clone.RedMana = 0;
                 
                }
                if (clone.AnyMana >= 0)
                    yield return clone;
            }
            if (color.HasFlag(Mana.Green))
            {
                var clone = Clone();
                clone.GreenMana -= v;
                if (clone.GreenMana < 0)
                {
                    clone.AnyMana += clone.GreenMana;
                    clone.GreenMana = 0;
                 
                }
                if (clone.AnyMana >= 0)
                    yield return clone;
            }
        }

        internal bool CanPay(Mana color, int v, int generic = 0)
        {
            if (color.HasFlag(Mana.Black))
            {
                if (AnyMana > 0)
                {

                }
                if (BlackMana+ AnyMana >= v && (TotalMana -v) >= generic)
                {
                    return true;
                }
            }
            if (color.HasFlag(Mana.Blue))
            {
                if (BlueMana + AnyMana >= v && (TotalMana - v) >= generic)
                {
                    return true;
                }
            }
            if (color.HasFlag(Mana.Red))
            {
                if (RedMana + AnyMana >= v && (TotalMana - v) >= generic)
                {
                    return true;
                }
            }
            if (color.HasFlag(Mana.Green))
            {
                if (GreenMana + AnyMana >= v && (TotalMana - v) >= generic)
                {
                    return true;
                }
            }
            return false;
        }

        internal IEnumerable<State> WaysToPay(Mana color, int v1, int v2 = 0)
        {
            //de är alltid rätt att tappa för svart
            
            List<State> ways = this.Pay(color, v1).ToList();
            for (int i = 0; i < v2; i++)
            {
                var newList = new List<State>();

                newList.AddRange(ways.SelectMany(p => PayOneColorless(p)));
                ways = newList;
            }
            return ways;
        }
        private IEnumerable<State> PayOneColorless(State clone)
        {
            if (clone.BlueMana > 0)
            {
                yield return clone.Clone().With(p => { p.BlueMana--; });
            }
            if (clone.RedMana > 0)
            {
                yield return clone.Clone().With(p => { p.RedMana--; });

            }
            if (clone.GreenMana > 0)
            {
                yield return clone.Clone().With(p => { p.GreenMana--; });

            }
            if (clone.BlackMana > 0)
            {
                yield return clone.Clone().With(p => { p.BlackMana--; });
            }
            if (clone.AnyMana > 0)
            {
                yield return clone.Clone().With(p => { p.AnyMana--; });
            }



        }

        
    }
}
