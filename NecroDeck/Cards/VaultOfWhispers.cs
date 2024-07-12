using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class VaultOfWhispers : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.None, true, true);

        public override string Name => "vault of whispers";

        public override IEnumerable<State> FromBoard(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.SetUsed(cardId);
                p.BlackMana += 1;
            });
        }

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.LandDrops == 0)
                yield return arg.Clone().With(p => { p.AddCardsInPlay(cardId, null, true); p.LandDrops++; });

        }
     
    } 

}
