using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class PriestOfGix : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Mana.Black);

        public override string Name => "priest of gix";


        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Black, 1, 2))
            {
                foreach (var x in arg.WaysToPay(Mana.Black, 1, 2))
                {
                    yield return x.With(p => p.BlackMana+=3);
                }
            }

        }

    }
}
