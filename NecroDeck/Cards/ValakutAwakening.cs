using System.Collections.Generic;
using System.Linq;

namespace NecroDeck.Cards
{
    class ValakutAwakening : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.Red);

        public override string Name => "valakut awakening";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.MainPhase)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Red, 1, 2))
            {
                //om jag tar fram first index of för manamorphose, bourne, och beseech tendrils samt alla spirit guides
                //sen slänger resten och drar så många nya. Då har vi kanske de klart och betart
                var l = arg.Cards.Where(p => !Global.Deck.Cards[p].StartsWith("dark r") && !Global.Deck.Cards[p].StartsWith("lotu") && !Global.Deck.Cards[p].StartsWith("tend") && !Global.Deck.Cards[p].Contains("spirit")).ToList();
                //sen ta bort alla utom första manamorphose, bourne och beseech
                var borne = l.FirstIndexOf(x => Global.Deck.Cards[x].StartsWith("born"));
                var manamorphose = l.FirstIndexOf(x => Global.Deck.Cards[x].StartsWith("manamorp"));
                var bes = l.FirstIndexOf(x => Global.Deck.Cards[x].StartsWith("beseech"));

                var toDiscard = l.ExceptItem(borne).ExceptItem(manamorphose).ToList();
                int toDraw = toDiscard.Count;
                foreach (var x in arg.WaysToPay(Mana.Red, 1, 2))
                {
                    yield return x.With(p =>
                    {
                        p.Cards = p.Cards.Except(toDiscard).ToList();
                        p.DrawCards(toDraw);
                    });
                }
            }
        }
     
    }     
}
