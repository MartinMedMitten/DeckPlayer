using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class LotusPetal : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Mana.None, true, true);

        public override string Name => "lotus petal";

        public override IEnumerable<State> FromBoard(State arg, int card)
        {
            yield return arg.Clone().With(p =>
            {
                p.BargainFodder--;
                p.AnyMana++;
                p.RemoveCardsInPlay(card);
            });
        }

        public override IEnumerable<State> FromHand(State arg, int card)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            yield return arg.Clone().With(p => p.AddCardsInPlay(card, null, true));

        }
    }
}
