using System;

namespace NecroDeck
{
    class MetaData
    {
        public MetaData(Speed canBeCast, Mana color, bool bargainable=false, bool artifact = false)
        {
            
            Color = color;
            Bargainable = bargainable;
            Artifact = artifact;
            Speed = canBeCast;
        }

        public Mana Color { get; set; } 
        public bool Bargainable { get; set; }

        public bool Artifact { get; set; }
        public Speed Speed { get; }

        public bool ZeroCost { get; set; }
    }
}
