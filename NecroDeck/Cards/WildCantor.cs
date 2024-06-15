using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class WildCantor : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Mana.Red | Mana.Green);

        public override string Name => "wild cantor";

        public override IEnumerable<State> FromBoard(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.AnyMana++;
                p.RemoveCardsInPlay(cardId);
            });
        }

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Red | Mana.Green, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Red | Mana.Green, 1))
                {
                    yield return x.With(p => p.AddCardsInPlay(cardId, null, false));
                }
            }

        }
      
    }
}
