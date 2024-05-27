using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NecroDeck
{
    public class Global
    {
        public static Deck Deck { get; set; }
        public static bool DebugOutput { get; set; }
        public static Random R = new Random(1);

    }
    public class Deck
    {
        public List<string> Cards { get; set; }
        public Deck()
        {
            Cards = File.ReadAllLines("decklist.txt").ToList();
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            Global.DebugOutput = false;
            Global.Deck = new Deck();
            var deck = Global.Deck;
            Console.WriteLine(Global.Deck.Cards.Count + " card deck");
            Rules.InitRuleDict(deck);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var runner = new DeckPlayer();
            List<RunResult> runResults = new List<RunResult>();

            for (int i = 0; i < (Global.DebugOutput ? 100 : 10000); i++)
            {
                if (Global.DebugOutput)
                {
                    if (i == 67)
                    {//WildCantor, PactofNegation, ChromeMox, GemstoneMine, Necro, VaultOfWhispers, CabalRitual,

                    }
                    Console.Write(i + ": ");
                }
                var seed = Global.DebugOutput ? i : Global.R.Next(0, Int32.MaxValue);
                var rr = runner.Run(deck, seed);
                runResults.Add(rr);


                if (rr.Win)
                {
                    if (Global.DebugOutput)
                        Console.WriteLine("WIN");
                    
            
                }
                else
                {
                    if (Global.DebugOutput)
                        Console.WriteLine("loss");
                }
            }
            stopWatch.Stop();
            int wins = runResults.Where(p => p.Win).Count();
            int prot = runResults.Where(p => p.Protected).Count();

            Console.WriteLine(stopWatch.ElapsedMilliseconds + " ms"); //9309 ms med card1/2 etc
            
             Console.WriteLine("wins " + wins);
             Console.WriteLine("Protected wins " + prot);

            Console.ReadKey();

        }
        


    }
}
