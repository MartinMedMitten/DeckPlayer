using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class PactOfNegation : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Mana.Blue);

        public override string Name => "pact of negation";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield break;
        }
     
    }  
}
