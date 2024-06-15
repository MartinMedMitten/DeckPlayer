using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class TreeOfTales : VaultOfWhispers
    {
        public override string Name => "tree of tales";
        public override IEnumerable<State> FromBoard(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.SetUsed(cardId);
                p.GreenMana += 1;
            });
        }
    }
}
