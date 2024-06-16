using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class ManaMorphose : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Mana.Red | Mana.Green);

        public override string Name => "manamorphose";


        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.CanPay(Mana.Red | Mana.Green, 1, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Red | Mana.Green, 1, 1))
                {
                    if (arg.TimingState != TimingState.MainPhase) //because i don't want to effect mulligan decisions
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
        }
     
    }
}
