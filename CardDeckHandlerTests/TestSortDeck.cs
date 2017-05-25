using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ShiftWiseCardDeckHandler;

namespace ShiftwiseCardDeckHandlerTests
{
    [TestClass]
    public class TestSortDeck
    {
        [TestMethod]
        public void NullDeckDoesNothing()
        {
            CardDeckSorter.Sort(null, CompareAlphabeticSuitAcesLow);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullComparisonThrowsException()
        {
            CardDeckSorter.Sort(new List<PlayingCard>(), null);
        }

        [TestMethod]
        public void EmptyDeckIsStillEmpty()
        {
            var deck = new List<PlayingCard>();

            CardDeckSorter.Sort(deck, CompareAlphabeticSuitAcesLow);

            Assert.AreEqual(0, deck.Count);
        }

        [TestMethod]
        public void OneCardRemainsUnchanged()
        {
            var card = PlayingCard.GetAllPossibleCards().First();
            var deck = new List<PlayingCard>{ card };

            CardDeckSorter.Sort(deck, CompareAlphabeticSuitAcesLow);

            Assert.AreEqual(1, deck.Count);
            Assert.AreEqual(card, deck[0]);
        }

        [TestMethod]
        public void TestAlphabeticSuitAcesLow()
        {
            TestComparison(CompareAlphabeticSuitAcesLow);
        }

        [TestMethod]
        public void TestAlphabeticSuitAcesHigh()
        {
            TestComparison(CompareAlphabeticSuitAcesHigh);
        }

        private void TestComparison(Comparison<PlayingCard> comparison)
        {
            var deck = PlayingCard.GetAllPossibleCards().ToList();

            CardDeckSorter.Sort(deck, comparison);

            // Make sure every pair of cards increases
            var last = deck.First();
            foreach (var card in deck.Skip(1))
            {
                var comparisonResult = comparison(last, card);
                Assert.IsTrue(comparisonResult < 0, $"{last} came before {card}, comparison={comparisonResult}");
                last = card;
            }
        }

        internal static int CompareAlphabeticSuitAcesLow(PlayingCard x, PlayingCard y)
        {
            var rankDiff = x.CompareRankKingsHigh(y);
            if (rankDiff != 0)
            {
                return rankDiff;
            }
            return x.suit.ToString().CompareTo(y.suit.ToString());
        }

        private static int CompareAlphabeticSuitAcesHigh(PlayingCard x, PlayingCard y)
        {
            var rankDiff = x.CompareRankAcesHigh(y);
            if (rankDiff != 0)
            {
                return rankDiff;
            }
            return x.suit.ToString().CompareTo(y.suit.ToString());
        }
    }
}
