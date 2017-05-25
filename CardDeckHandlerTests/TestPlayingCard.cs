using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftWiseCardDeckHandler;

namespace ShiftwiseCardDeckHandlerTests
{
    [TestClass]
    public class TestPlayingCard
    {
        [TestMethod]
        public void FiftyTwoStandardCards()
        {
            Assert.AreEqual(52, PlayingCard.PossibleDifferentCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NoneRankThrowsException()
        {
            new PlayingCard(PlayingCard.Rank.None, PlayingCard.Suit.Clubs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidRankThrowsException()
        {
            new PlayingCard(PlayingCard.Rank.King + 1, PlayingCard.Suit.Clubs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NoneSuitThrowsException()
        {
            new PlayingCard(PlayingCard.Rank.Ace, PlayingCard.Suit.None);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidSuitThrowsException()
        {
            new PlayingCard(PlayingCard.Rank.Ace, PlayingCard.Suit.Spades + 1);
        }

        [TestMethod]
        public void AllValidInputsAreAllowed()
        {
            try
            {
                // Need ToList() to cause iteration to actually happen.
                PlayingCard.GetAllPossibleCards().ToList();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void AcesHighBeatsAllRanks()
        {
            const PlayingCard.Suit arbitrarySuit = PlayingCard.Suit.Clubs;
            foreach (var xRank in PlayingCard.GetAllValidRanks())
            {
                var x = new PlayingCard(xRank, arbitrarySuit);
                foreach (var yRank in PlayingCard.GetAllValidRanks())
                {
                    var y = new PlayingCard(yRank, arbitrarySuit);
                    var result = x.CompareRankAcesHigh(y);
                    if (xRank == yRank)
                    {
                        Assert.AreEqual(0, result);
                    }
                    else if (xRank == PlayingCard.Rank.Ace)
                    {
                        Assert.IsTrue(result > 0, $"xRank={xRank}, yRank={yRank}, result={result}");
                    }
                    else if (yRank == PlayingCard.Rank.Ace)
                    {
                        Assert.IsTrue(result < 0, $"xRank={xRank}, yRank={yRank}, result={result}");
                    }
                    else
                    {
                        Assert.IsTrue(result != 0, $"xRank={xRank}, yRank={yRank}, result={result}");
                        Assert.AreEqual(xRank - yRank < 0, result < 0, $"xRank={xRank}, yRank={yRank}, result={result}");
                    }
                }
            }
        }
    }
}
