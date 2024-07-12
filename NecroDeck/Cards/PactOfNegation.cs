using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class PactOfNegation : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Instant, Mana.Blue) { ZeroCost = true };

        public override string Name => "pact of negation";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield break;
        }
     
    }  
}
