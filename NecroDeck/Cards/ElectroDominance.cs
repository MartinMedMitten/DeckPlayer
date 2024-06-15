using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class ElectroDominance : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Mana.Red);

        public override string Name => "electrodominance";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState != TimingState.InstantOnly)
            {
                yield break;
            }
            if (arg.CanPay(Mana.Red, 2, 2))
            {
                int borne = Global.Dict["borne upon a wind"].FirstOrDefault(p => arg.HasCardInHand(p), -1);
                if (borne != -1)
                {
                    foreach (var x in arg.WaysToPay(Mana.Red, 2, 2))
                    {
                        yield return x.With(p =>
                        {
                            p.TimingState = TimingState.Borne;
                            p.DrawCards(1);
                            p.RemoveCard(borne);
                        });
                    }
                }

            }
            if (arg.CanPay(Mana.Red, 2, 4))
            {
                int beseech = Global.Dict["beseech the mirror"].FirstOrDefault(p => arg.HasCardInHand(p), -1);

                if (beseech != -1)
                {
                    yield return arg.Clone().With(p =>
                    {
                        p.Win = true;
                    });
                }
            }
        }

    }    
}
