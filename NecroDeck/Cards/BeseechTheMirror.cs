using System.Collections.Generic;

namespace NecroDeck.Cards
{
    class BeseechTheMirror : CardMetaData
    {

        public override MetaData MetaData { get; } = new MetaData(Speed.Sorcery, Mana.Black);

        public override string Name => "beseech the mirror";


        public override IEnumerable<State> FromHand(State arg, int cardId)
        {
            if (arg.TimingState == TimingState.InstantOnly) //don't seem necessary to cast necro post necro
            {
                yield break;
            }
            if (arg.TimingState == TimingState.Borne)
            {
                if (arg.CanPay(Mana.Black, 3, 1))
                {
                    yield return arg.Clone().With(p => p.Win = true);
                }
            }
            else
            {
                if (Global.RunPostNecro)
                {
                    if (arg.CanPay(Mana.Black, 6, 1))
                    {
                        foreach (var y in arg.WaysToPay(Mana.Black, 6, 1))
                        {
                            yield return y.With(p =>
                            {
                                p.TimingState = TimingState.InstantOnly;
                                p.DrawCards(19);
                            });
                        }
                    }
                    if (arg.BargainFodder > 0 && arg.CanPay(Mana.Black, 3, 1))
                    {
                        foreach (var y in arg.WaysToPay(Mana.Black, 3, 1))
                        {
                            yield return y.With(p =>
                            {
                                p.TimingState = TimingState.InstantOnly;
                                p.DrawCards(19);
                            });
                        }
                    }
                }
                else
                {
                    if (arg.CanPay(Mana.Black, 6, 1))
                    {
                        yield return arg.Clone().With(p => p.Win = true);
                    }
                    if (arg.BargainFodder > 0 && arg.CanPay(Mana.Black, 3, 1))
                    {
                        yield return arg.Clone().With(p => p.Win = true);
                    }
                }
            }
        }
     
    }    
}
