using System;

namespace NecroDeck
{
    public class Global
    {
        public static Deck Deck { get; set; }
        public static bool DebugOutput { get; set; }
        public static bool ContainsCantor { get; internal set; }
        public static int CantorId { get; internal set; }

        public static Random R = new Random(1);

    }
}
