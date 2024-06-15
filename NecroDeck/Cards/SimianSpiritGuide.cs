using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class SimianSpiritGuide : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Mana.Red);

        public override string Name => "simian spirit guide";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.RedMana++);
        }

    }
}
