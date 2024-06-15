using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class CabalRitual : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Mana.Black);

        public override string Name => "cabal ritual";


        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.CanPay(Mana.Black, 1, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Black, 1, 1))
                {
                    yield return x.With(p => p.BlackMana += 3);
                }
            }
        }
     
    } 

}
