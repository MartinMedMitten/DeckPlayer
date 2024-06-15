using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class TendrilsOfAgony : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Mana.None); //TODO ugly, but setting color to none makes it not imprintable.

        public override string Name => "tendrils of agony";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState != TimingState.Borne)
            {
                yield break;
            }
            if (arg.CanPay(Mana.Black, 2, 2))
            {
                yield return arg.Clone().With(p => p.Win = true);
            }
        }
     
    }    
}
