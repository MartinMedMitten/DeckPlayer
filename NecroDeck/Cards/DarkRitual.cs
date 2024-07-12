using System.Collections.Generic;
using System.Linq;

namespace NecroDeck.Cards
{
    class DarkRitual : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Speed.Instant, Mana.Black);

        public override string Name => "dark ritual";


        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.CanPay(Mana.Black, 1))
            {
                return arg.Pay(Mana.Black, 1).Select(q => q.With(p => p.BlackMana += 3));
            }
            return Enumerable.Empty<State>();
        }
     
    }    
}
