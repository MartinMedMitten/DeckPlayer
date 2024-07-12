using System;
using System.Collections.Generic;
using System.Linq;

namespace NecroDeck.Cards
{
    class InfernalTutor : CardMetaData
    {
        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.Black);

        public override string Name => "infernal tutor";

        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly)
            {
                yield break;
            }
            throw new ApplicationException("Verify that setting bitflag to 0 is equal to discard hand");
            if (arg.CanPay(Mana.Black, 1, 1))
            {
                if (Global.RunPostNecro)
                {
                    if (arg.CardsInHandBitflag == 0)
                    {
                        yield return arg.Clone().With(p =>
                        {
                            var necro = Global.Dict["necrodominance"].First(); //TODO not complete
                            p.AddCardToHand(necro);
                        });
                    }
                    if (arg.LedInPlay > 0)
                    {
                        yield return arg.Clone().With(p =>
                        {
                            var necro = Global.Dict["necrodominance"].First(); //TODO not complete
                            p.CardsInHandBitflag = 0;
                            p.AddCardToHand(necro);
                            p.BlackMana += 3;

                        });
                    }
                }
                else
                {
                    if (arg.CardsInHandBitflag == 0 && arg.CanPay(Mana.Black, 4, 1))
                    {
                        yield return arg.Clone().With(p => p.Win = true);
                    }
                    if (arg.LedInPlay > 0)
                    {
                        yield return arg.Clone().With(p => p.Win = true);
                    }
                }

            }
        }
     
    }   
}
