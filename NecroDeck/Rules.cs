using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace NecroDeck
{

    class Rules
    {
        public static Dictionary<int, Func<State, int, IEnumerable<State>>> RuleDictFromHand = new Dictionary<int, Func<State, int, IEnumerable<State>>>();
        public static Dictionary<int, Func<State, int, IEnumerable<State>>> RuleDictFromPlay = new Dictionary<int, Func<State, int, IEnumerable<State>>>();
        public static Dictionary<int, MetaData> MetadataDictionary = new Dictionary<int, MetaData>();

        public static void InitRuleDict(Deck deck)
        {
            MetadataDictionary.Clear();
            RuleDictFromHand.Clear();
            RuleDictFromPlay.Clear();
            var cardTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CardMetaData)))
            .ToList();

            var allCards = cardTypes.Select(p => Activator.CreateInstance(p) as CardMetaData).ToDictionary(q => q.Name, q => q);

            for (int i = 0; i < deck.Cards.Count; i++)
            {
                var t = allCards[deck.Cards[i]];

                MetadataDictionary[i] = t.MetaData;
                RuleDictFromHand[i] = t.FromHand;
                RuleDictFromPlay[i] = t.FromBoard;
            }

            
        }

        internal static IEnumerable<State> GetResult(int card1, State state)
        {
            return RuleDictFromHand[card1](state, card1);
        }
        internal static IEnumerable<State> GetResultFromInPlay(int card1, State state)
        {
            return RuleDictFromPlay[card1](state, card1);
        }
    }
}
