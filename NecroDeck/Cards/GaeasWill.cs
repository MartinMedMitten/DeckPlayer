using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class GaeasWill : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.Green);

        public override string Name => "gaea's will";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield break;
        }
     
    }     
}
