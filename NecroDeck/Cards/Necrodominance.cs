using System.Collections.Generic;
using System.Linq;

namespace NecroDeck.Cards
{
    class Necrodominance : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.Black, true);

        public override string Name => "necrodominance";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState != TimingState.MainPhase)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Black, 3))
            {
                if (Global.RunPostNecro)
                {
                    foreach (var y in arg.WaysToPay(Mana.Black, 3))
                    {
                        yield return y.With(p =>
                        {
                            p.TimingState = TimingState.InstantOnly;
                            p.DrawCards(19);
                        });
                    }
                }
                else 
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }
            }
        }
     
    } 
}
