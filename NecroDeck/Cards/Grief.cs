using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class Grief : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.Black);

        public override string Name => "grief";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield break;
        }
     
    }     
}
