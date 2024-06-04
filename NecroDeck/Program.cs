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
            Global.DebugOutput = false;
            Global.RunPostNecro = true;

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
            List<int> losses = new List<int>();
            for (int i = 0; i < (Global.DebugOutput ? 10 : 1000); i++)
            {
                if (Global.DebugOutput)
                {
                    if (i == 6)
                    {

                    }
                    Console.Write(i + ": ");
                }
                else
                {
                    if (i % 100 == 0)
                    {
                        Console.WriteLine(i);
                    }
                }
                var seed = Global.DebugOutput ? i : Global.R.Next(0, Int32.MaxValue);
                var rr = runner.Run(deck, seed);
                rr.Index = i;
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
                    losses.Add(i);
                }
            }
            stopWatch.Stop();
            int wins = runResults.Where(p => p.Win).Count();
            int prot = runResults.Where(p => p.Protected).Count();
            int inconclusive = runResults.Where(p => p.Inconclusive).Count();
            int gotNecro = runResults.Where(p => p.State?.TimingState == TimingState.InstantOnly).Count();
            int gotBourne = runResults.Where(p => p.State?.TimingState == TimingState.Borne).Count();
            int gotBorneFizzled = runResults.Where(p => p.BorneLoss).Count();

            Console.WriteLine(stopWatch.ElapsedMilliseconds + " ms");

            Console.WriteLine("wins " + wins);
            Console.WriteLine("Protected wins " + prot);
            Console.WriteLine("Inconclusive " + inconclusive);
            Console.WriteLine("Got necro" + gotNecro);
            Console.WriteLine("Got borne" + gotBourne);
            Console.WriteLine("Borne fizzled" + gotBorneFizzled);

            while (true)
            {
                var inp = Console.ReadLine();
                try
                {
                    if (inp.StartsWith("show "))
                    {
                        var spl = inp.Split(' ');
                        var index = Int32.Parse(spl[1]);
                        Show(runResults[index]);
                    }
                    if (inp.StartsWith("findloss "))
                    {
                        var spl = inp.Split(' ');
                        var r = runResults.Where(q => q.State?.TimingState == TimingState.InstantOnly && q.State.Cards.Any(p => Global.Deck.Cards[p].StartsWith(spl[1]))).ToList();
                        Console.WriteLine($"Found {r.Count()} games");
                        Console.WriteLine(string.Join(", ", r.Select(p => p.Index)));
                    }
                    if (inp.StartsWith("findfizzle"))
                    {
                        var spl = inp.Split(' ');
                        
                        var r = spl.Length > 1 ?
                            runResults.Where(q => q.Win).ToList() :
                            runResults.Where(q => q.State?.TimingState == TimingState.InstantOnly).ToList();
                        Console.WriteLine($"Found {r.Count()} games");
                        Dictionary<string, int> cc = new Dictionary<string, int>();
                        foreach (var x in Global.Deck.Cards)
                        {
                            cc[x] = 0;
                        }

                        foreach (var l in r.SelectMany(p => p.NecroState.Cards))
                        {
                            cc[Global.Deck.Cards[l]] = cc[Global.Deck.Cards[l]] + 1;
                        }

                        foreach (var x in cc.OrderBy(p => p.Value))
                        {
                            Console.WriteLine(x.Key + "\t\t:" + x.Value + "   -   " + ((decimal)x.Value)/(decimal)r.Count);
                        }

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("syntax error!");
                }
            }


            //Console.WriteLine("Lossed " + string.Join(", ", losses));
            return wins;
        }

        private static void Show(RunResult runResult)
        {
            var playOrder = new List<State>();

            var last = runResult.State;
            while(last != null)
            {
                playOrder.Add(last);

                last = last.Parent;
            }
            playOrder.Reverse();
            var mulligans = runResult.Mulligans;
            for (int i = 0; i < playOrder.Count; i++)
            {
                State x = playOrder[i];
                if (i == 0)
                {
                    x.Print();
                }
                
                if (i == playOrder.Count - 1)
                {
                    Console.WriteLine(runResult.Win ? "WIN" : " loss :(");
                }
                else
                {//de är mana förändringarna
                    var next = playOrder[i + 1];
                    var missingCard = x.Cards.Where(p => !next.Cards.Contains(p)).Select(q => Global.Deck.Cards[q]).ToList();
                    var newCards = next.Cards.Where(p => !x.Cards.Contains(p)).Select(q => Global.Deck.Cards[q]).ToList();

                    if (missingCard.Any() || newCards.Any())
                    {
                        Console.Write("action: ");
                    }
                    if (missingCard.Any())
                    {
                        string castOrMull = mulligans > 0 ? "mull " : "cast ";
                        mulligans--;
                        Console.Write(castOrMull + string.Join(", ", missingCard));
                        Console.Write(", MANA: " + next.PrintMana());
                        Console.WriteLine();
                    }
                    
                    if (newCards.Any())
                    {
                        Console.Write("drew: " + string.Join(", ", newCards));
                        Console.Write(", MANA: " + next.PrintMana());
                        Console.WriteLine();
                    }
                }
            }

        }

    }
}
