using System.Collections.Generic;
using System.Linq;

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
                var c = p.CardsInHandCount;
                var exiledCards = p.CardsInHandBitflag;
                p.CardsInHandBitflag = 0;
                p.ModifyRunState(x =>
                {
                    x.SerumPowder++;
                });
                p.DrawCards(c);
                p.Powderable = !p.HasCardInHand(Global.Dict["tendrils of agony"].First()) && Global.Dict["serum powder"].Any(q => p.HasCardInHand(q));
            });
        }

    }
}
