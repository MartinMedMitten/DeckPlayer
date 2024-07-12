using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class Necrologia : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Instant, Mana.Black);

        public override string Name => "necrologia";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState != TimingState.MainPhase)
            {
                yield break;
            }

            if (Global.RunPostNecro)
            {
                if (arg.CanPay(Mana.Black, 2, 3))
                {
                    foreach (var y in arg.WaysToPay(Mana.Black, 2, 3))
                    {
                        yield return y.Clone().With(p =>
                        {
                            p.TimingState = TimingState.InstantOnly;
                            p.DrawCards(19);
                        });
                    }
                }
            }
            else 
            {
                if (arg.CanPay(Mana.Black, 2, 3))
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }
            }
        }
     
    }   
}
