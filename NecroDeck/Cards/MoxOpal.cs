using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class MoxOpal : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.None, true, true) { ZeroCost = true };

        public override string Name => "mox opal";

        public override IEnumerable<State> FromBoard(State arg, int cardId)
        {
            var metalCraft = 0;
            for (int i = 0; i < arg.CardsInPlay.Count; i++)
            {
                if (Rules.MetadataDictionary[arg.CardsInPlay[i].Card].Artifact)
                {
                    metalCraft++;
                }
                if (metalCraft >= 3)
                {
                    break;
                }
            }
            if (metalCraft < 3)
            {
                yield break;
            }
            yield return arg.Clone().With(p =>
            {
                p.SetUsed(cardId);
                p.AnyMana++;
            });
        }

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            yield return arg.Clone().With(p => p.AddCardsInPlay(cardId, null, true));

        }

    }

}
