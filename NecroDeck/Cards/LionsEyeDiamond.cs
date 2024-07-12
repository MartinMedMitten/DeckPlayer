using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class LionsEyeDiamond : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.None, true, true) { ZeroCost = true };

        public override string Name => "lion's eye diamond";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.LedInPlay++;
                p.BargainFodder++;
            });
        }
     
    }    
}
