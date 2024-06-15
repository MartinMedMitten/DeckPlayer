using System.Collections.Generic;
using System.Linq;

namespace NecroDeck.Cards
{
    class ChromeMox : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Mana.None, true, true);

        public override string Name => "chrome mox";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            for (int i = arg.Cards.Count - 1; i >= 0; i--)
            {
                var x = arg.Cards[i];


                var color = Rules.MetadataDictionary[x].Color;
                if (color.HasFlag(Mana.Black))
                {
                    yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, Mana.Black, true); p.BargainFodder++; p.RemoveCard(x); });
                }
                if (color.HasFlag(Mana.Red))
                {
                    yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, Mana.Red, true); p.BargainFodder++; p.RemoveCard(x); });
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
        public override IEnumerable<State> FromBoard(State arg, int cardId)
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

    } 

}
