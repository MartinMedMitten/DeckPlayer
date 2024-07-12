using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class WildCantor : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.Red | Mana.Green);

        public override string Name => "wild cantor";

        public override IEnumerable<State> FromBoard(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.AnyMana++;
                p.RemoveCardsInPlay(cardId);
            });
        }

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Red | Mana.Green, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Red | Mana.Green, 1))
                {
                    yield return x.With(p => p.AddCardsInPlay(cardId, null, false));
                }
            }

            

        }
      
    }
    class AnOfferYouCantRefuse : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.Red | Mana.Green);

        public override string Name => "an offer you can't refuse";

        public override IEnumerable<State> FromBoard(State arg, int cardId)
        {
            yield return arg.Clone().With(p =>
            {
                p.AnyMana++;
                p.RemoveCardsInPlay(cardId);
            });
        }

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            //Leta reda på alla 0 cost cards för respektive timing state
            //om man har pact + 0 cost card kan man bara exila pact
            //annars får man exilea 0 cost cardet för att få 2 mana of any color
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }

            if (arg.CanPay(Mana.Red | Mana.Green, 1))
            {
                foreach (var x in arg.WaysToPay(Mana.Red | Mana.Green, 1))
                {
                    yield return x.With(p => p.AddCardsInPlay(cardId, null, false));
                }
            }



        }

    }
}
