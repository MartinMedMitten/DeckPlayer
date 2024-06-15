using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class SummonersPact : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Mana.Green);

        public override string Name => "summoner's pact";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.GreenMana++);
            if (Global.ContainsCantor && !arg.RunState.CantorInHand && arg.TimingState != TimingState.InstantOnly) //TODO if wildcantor is in deck and not it hand
            {
                yield return arg.Clone().With(p => { p.AddCardToHand(Global.CantorId); p.ModifyRunState(x => x.CantorInHand = true); });
            }
        }
     
    }     
}
