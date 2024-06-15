using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class ElvishSpiritGuide : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Mana.Green);

        public override string Name => "elvish spirit guide";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.GreenMana++);
        }

    }
}
