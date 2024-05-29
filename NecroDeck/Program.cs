using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NecroDeck
{
    class CutStats
    {
        public string Name { get; set; }
        public int Wins { get; set; }
    }

    class Program
    {

        static void Main(string[] args)
        {
            Global.DebugOutput = true;

            RegularRun();

        }

        private static void RegularRun()
        {
            Global.Deck = new Deck();

            Run();

            Console.ReadKey();
        }
        private static void CutRun()
        {
            Global.Deck = new Deck();
            List<CutStats> cutStats = new List<CutStats>();
            var cuttables = Global.Deck.Cards.Distinct().Except(new[] { "Tendrils", "PactofNegation" }).ToList();
            foreach (var x in cuttables)
            {
                var firstIndexOf = Global.Deck.Cards.IndexOf(x);
                Global.Deck.Cards.RemoveAt(firstIndexOf);
                if (x == "WildCantor")
                {
                    Global.ContainsCantor = false;
                }
                cutStats.Add(new CutStats
                {
                    Name = x,
                    Wins = Run()
                });

                Global.Deck = new Deck();
            }

            foreach (var x in cutStats.OrderByDescending(p => p.Wins))
            {
                Console.WriteLine(x.Name + ": " + x.Wins);
            }

            Console.ReadKey();
        }

        private static int Run()
        {
            
            var deck = Global.Deck;
            Console.WriteLine(Global.Deck.Cards.Count + " card deck");
            Rules.InitRuleDict(deck);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var runner = new DeckPlayer();
            var runResults = new List<RunResult>();

            for (int i = 0; i < (Global.DebugOutput ? 100 : 10000); i++)
            {
                if (Global.DebugOutput)
                {
                    if (i == 67)
                    {

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

            Console.WriteLine(stopWatch.ElapsedMilliseconds + " ms");

            Console.WriteLine("wins " + wins);
            Console.WriteLine("Protected wins " + prot);
            return wins;
        }


    }
}
