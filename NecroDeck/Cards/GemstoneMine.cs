using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class GemstoneMine : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Mana.None);

        public override string Name => "gemstone mine";

        public override IEnumerable<State> FromBoard(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.SetUsed(cardId);
                p.AnyMana++;
            });
        }

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState != TimingState.MainPhase)
            {
                yield break;
            }
            if (arg.LandDrops == 0)
            {
                yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, null, false); p.LandDrops++; });
            }

        }
    }  
}
