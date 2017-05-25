using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ShiftWiseCardDeckHandler;

namespace ShiftwiseCardDeckHandlerTests
{
    [TestClass]
    public class TestShuffledDeck
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullRandomThrowsException()
        {
            CardDeckShuffler.Create(null);
        }

        [TestMethod]
        public void HasOneOfEachCard()
        {
            // I suppose you might argue that this should be tested for number states of Random,
            // but by inspection of Create code that seems like overkill...

            var deck = CardDeckShuffler.Create(new Random(0));

            Assert.AreEqual(PlayingCard.PossibleDifferentCount, deck.Count);

            foreach (var card in PlayingCard.GetAllPossibleCards())
            {
                Assert.AreEqual(1, deck.Count(c => c == card), $"card={card}");
            }
        }

        [TestMethod]
        public void CardPositionsUniform()
        {
            // Testing whether the card shuffles randomly is basically the same as testing
            // whether it is a good random number generator, within the constraint of a
            // finite set of cards. Testing RNG's is a whole branch of comp sci/math,
            // so for brevity I just rely on a good system RNG and test the shuffle itself.

            Assert.IsTrue(IsDeckUniform(GetShuffledDeck));
        }

        [TestMethod]
        public void SortedCardsNotUniform()
        {
            // This is a test of IsDeckUniform test utility
            Assert.IsFalse(IsDeckUniform(GetSortedDeck));
        }

        private IList<PlayingCard> GetShuffledDeck(Random rng)
        {
            return CardDeckShuffler.Create(rng);
        }

        private IList<PlayingCard> GetSortedDeck(Random rng)
        {
            var deck = PlayingCard.GetAllPossibleCards().ToList();
            CardDeckSorter.Sort(deck, TestSortDeck.CompareAlphabeticSuitAcesLow);
            return deck;
        }

        private delegate IList<PlayingCard> DeckGetter(Random rng);

        private bool IsDeckUniform(DeckGetter deckGetter)
        {
            // Test whether the distributions of card positions are all uniform using a
            // Pearson's Chi-Squared test at 95% significance level.
            // https://en.wikipedia.org/wiki/Pearson%27s_chi-squared_test

            // Use a fixed seed for a repeatable test
            var rng = new Random(0);
            // Generate sample frequencies for this deck getter
            var dists = new Dictionary<PlayingCard, int[]>(PlayingCard.PossibleDifferentCount);
            // Factor of 5 is because, per Wikipedia: "The approximation to the chi-squared distribution 
            // breaks down if expected 
            // frequencies are too low. It will normally be acceptable so long as no more than 20% of 
            // the events have expected frequencies below 5."
            var expected = 60;
            for (var i = 0; i < expected * PlayingCard.PossibleDifferentCount; i++)
            {
                var deck = deckGetter(rng);
                for (var j = 0; j < deck.Count(); j++)
                {
                    var card = deck[j];
                    int[] dist;
                    if (!dists.TryGetValue(card, out dist))
                    {
                        dist = new int[PlayingCard.PossibleDifferentCount];
                        dists[card] = dist;
                    }
                    dist[j]++;
                }
            }
            Assert.AreEqual(PlayingCard.PossibleDifferentCount, dists.Count, "there should be one distribution per card");

            // Force floating point calculations just in case of integer overflows
            var expectedD = (double)expected;
            var chi2s = new Dictionary<PlayingCard, double>(PlayingCard.PossibleDifferentCount);
            foreach (var pair in dists)
            {
                var card = pair.Key;
                // Sanity check for distribution computation
                Assert.AreEqual(expected * PlayingCard.PossibleDifferentCount, pair.Value.Sum());

                chi2s[card] = pair.Value.Sum(observed =>
                {
                    var value = observed - expectedD;
                    return value * value / expectedD;
                });
                // print out the distributions, for manual inspection
                //Console.Write($"{card}\t{chi2s[card]}");
                //for (var j = 0; j < pair.Value.Length; j++)
                //{
                //    Console.Write($"\t{pair.Value[j]}");
                //}
                //Console.WriteLine();
            }
            // For 51 (52-1) degrees of freedom and 95% probability
            // https://www.fourmilab.ch/rpkp/experiments/analysis/chiCalc.html
            // "chi-square value indicating a probability Q=0.05 of non-chance occurrence for an experiment with d=51 degrees of freedom"
            var chi2q5d51 = 68.6692;
            var highChi2 = chi2s.Count(x => x.Value > chi2q5d51);
            Console.WriteLine($"{highChi2} cards' distributions have Chi2 greater than {chi2q5d51}");
            var maxHigh = 0.05 * PlayingCard.PossibleDifferentCount; // 2.6
            return highChi2 < maxHigh;
        }
    }
}
