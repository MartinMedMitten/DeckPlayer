using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class BorneUponAWind : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Instant, Mana.Blue);

        public override string Name => "borne upon a wind";

        public override IEnumerable<State> FromHand(State arg, int cardId)
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
     
    }     
}
