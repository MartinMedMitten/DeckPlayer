using System;
using System.Collections.Generic;

namespace NecroDeck
{
    public class Global
    {
        public static Deck Deck { get; set; }
        public static bool DebugOutput { get; set; }
        public static int Runs => DebugOutput ? DebugRunCount : RegularRunCount;

        public static int DebugRunCount { get; set; }
        public static int RegularRunCount { get; set; }

        public static int CounterInterval
        {
            get
            {
                var c = RegularRunCount / 100;
                return c < 1 ? 1 : c;
            }
        }

        public static bool ContainsCantor { get; internal set; } //TODO this is probably not needed any more
        public static int CantorId { get; internal set; }

        public static Random R = new Random(1);

        public static bool RunPostNecro { get; set; } = false;
        public static Dictionary<string, List<int>> Dict { get; internal set; } = new Dictionary<string, List<int>>();
    }
}
