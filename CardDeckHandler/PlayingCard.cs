using System;
using System.Collections.Generic;
using System.Linq;

namespace ShiftWiseCardDeckHandler
{
    /// <summary>
    /// A card used in playing common card games in the US and Great Britain.
    /// Does not support Jokers.
    /// (Technically a French playing card https://en.wikipedia.org/wiki/French_playing_cards).
    /// </summary>
    public class PlayingCard : IEquatable<PlayingCard>
    {
        public enum Suit
        {
            None,
            Clubs,
            Diamonds,
            Hearts,
            Spades,
        }

        public enum Rank
        {
            None,
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King
        }

        /// <summary>
        /// How many unique, valid PlayingCards are allowed? (Answer: 52)
        /// </summary>
        public static int PossibleDifferentCount
        {
            get
            {
                // -1's are for None values
                return (Enum.GetValues(typeof(Suit)).Length - 1) * (Enum.GetValues(typeof(Rank)).Length - 1);
            }
        }

        /// <summary>
        /// Enumerates all the Rank type values that are valid.
        /// </summary>
        public static IEnumerable<PlayingCard.Rank> GetAllValidRanks()
        {
            return EnumValues<PlayingCard.Rank>().Where(r => r != PlayingCard.Rank.None);
        }

        /// <summary>
        /// Iterate this to create instances of all valid card possibilities.
        /// </summary>
        public static IEnumerable<PlayingCard> GetAllPossibleCards()
        {
            foreach (var suit in EnumValues<PlayingCard.Suit>().Where(s => s != PlayingCard.Suit.None))
            {
                foreach (var rank in GetAllValidRanks())
                {
                    yield return new PlayingCard(rank, suit);
                }
            }
        }

        /// <summary>
        /// Which of the four suit types does this card belong to?
        /// </summary>
        public Suit suit { get; }

        /// <summary>
        /// Which of the thirteen ranks does this card belong to?
        /// </summary>
        public Rank rank { get; }

        /// <summary>
        /// Instantiate a new card of the specified rank and suit.
        /// If either rank or suit is None (value at initialization time) or invalid (via a cast),
        /// throws an ArgumentOutOfRangeException.
        /// </summary>
        public PlayingCard(Rank rank, Suit suit)
        {
            if (rank == Rank.None || !Enum.IsDefined(typeof(Rank), rank))
            {
                throw new ArgumentOutOfRangeException(nameof(rank), rank, "should be Ace, Two, ...King");
            }
            if (suit == Suit.None || !Enum.IsDefined(typeof(Suit), suit))
            {
                throw new ArgumentOutOfRangeException(nameof(suit), suit, "should be clubs, spades, etc.");
            }
            this.rank = rank;
            this.suit = suit;
        }

        /// <summary>
        /// Compare this card's rank to y's rank.
        /// Return less than zero if this card's rank is below y's, 0 for the same rank,
        /// and greater than zero if this card's rank is above y's, where King is the highest rank.
        /// </summary>
        public int CompareRankKingsHigh(PlayingCard y)
        {
            return rank - y.rank;
        }

        /// <summary>
        /// Compare this card's rank to y's rank.
        /// Return less than zero if this card's rank is below y's, 0 for the same rank,
        /// and greater than zero if this card's rank is above y's, where Ace is the highest rank.
        /// </summary>
        public int CompareRankAcesHigh(PlayingCard y)
        {
            if (rank == Rank.Ace)
            {
                return y.rank == Rank.Ace ? 0 : 1;
            }
            else if (y.rank == Rank.Ace)
            {
                return -1;
            }
            return rank - y.rank;
        }

        public bool Equals(PlayingCard other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return suit == other.suit &&
                rank == other.rank;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PlayingCard;
            if (other != null)
            {
                return Equals(other);
            }
            return base.Equals(obj);
        }

        public static bool operator ==(PlayingCard x, PlayingCard y)
        {
            if (object.ReferenceEquals(x, null))
            {
                return object.ReferenceEquals(y, null);
            }
            return x.Equals(y);
        }

        public static bool operator !=(PlayingCard x, PlayingCard y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Enum.GetValues(typeof(Rank)).Length * (int) suit + (int) rank;
        }

        public override string ToString()
        {
            return $"{rank} of {suit}";
        }

        /// <summary>
        /// Utility function to enumerate the type members of an enum.
        /// Why doesn't System.Enum include a method like this?
        /// </summary>
        /// <typeparam name="T">An enum type</typeparam>
        /// <returns>All members of the</returns>
        private static IEnumerable<T> EnumValues<T>()
        {
            var type = typeof(T);
            foreach (var valueAsObject in Enum.GetValues(type))
            {
                yield return (T)Enum.ToObject(type, valueAsObject);
            }
        }
    }
}
