using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class BigScore : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Instant, Mana.Red);

        public override string Name => "big score";

        public override IEnumerable<State> FromHand(State arg, int cardId)
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
                        p.AnyMana += 2; //TODO make treasures bargainable?
                        p.DrawCards(2);
                    });
                }
            }
        }
     
    }    
}
