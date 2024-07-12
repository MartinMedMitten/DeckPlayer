using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class SummonersPact : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Instant, Mana.Green) { ZeroCost = true };

        public override string Name => "summoner's pact";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            yield return arg.Clone().With(p => p.GreenMana++); //TODO implement this as a tutor for esg instead
            if (Global.ContainsCantor && !arg.RunState.CantorInHand && arg.TimingState != TimingState.InstantOnly) 
            {
                yield return arg.Clone().With(p => { p.AddCardToHand(Global.CantorId); p.ModifyRunState(x => x.CantorInHand = true); });
            }
        }
     
    }     
}
