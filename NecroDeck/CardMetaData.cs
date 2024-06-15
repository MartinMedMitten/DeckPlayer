using System.Collections.Generic;

namespace NecroDeck
{
    //enum Color
    //{
    //    Black,
    //    RedGreen,
    //    Red,
    //    Green,
    //    Other,
    //    None
    //}
    abstract class CardMetaData
    {
        public abstract string Name { get; }
        public abstract MetaData MetaData { get; } 

        public virtual IEnumerable<State> FromBoard(State arg, int card)
        {
            yield break;
        }

        public abstract IEnumerable<State> FromHand(State arg, int card);
    }
}
