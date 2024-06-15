namespace NecroDeck
{
    class MetaData
    {
        public MetaData(Mana color, bool bargainable=false, bool artifact = false)
        {
            Color = color;
            Bargainable = bargainable;
            Artifact = artifact;
        }

        public Mana Color { get; set; } 
        public bool Bargainable { get; set; }

        public bool Artifact { get; set; }
    }
}
