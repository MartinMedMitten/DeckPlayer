using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class SerumPowder : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Mana.None);

        public override string Name => "serum powder";


        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (!arg.Powderable)
                yield break;

            yield return arg.Clone().With(p =>
            {
                var c = p.CardsInHand;
                p.CardsInHandBitflag = 0;
                p.ModifyRunState(x => x.SerumPowder++);
                p.DrawCards(c);
            });
        }

    }
}
