using System;
using System.Collections.Generic;
using System.Linq;

namespace ShiftWiseCardDeckHandler
{
    public class CardDeckShuffler
    {
        /// <summary>
        /// Randomly shuffle a standard deck of 52 playing cards
        /// (four suits of thirteen cards each).
        /// </summary>
        /// <param name="rng">Pseudo random number generator to use for this shuffle</param>
        /// <returns>New deck of shuffled cards</returns>
        public static List<PlayingCard> Create(Random rng)
        {
            if (rng == null)
            {
                throw new ArgumentNullException(nameof(rng));
            }
            var deck = PlayingCard.GetAllPossibleCards().ToList();
            for (var i = 0; i < deck.Count; i++)
            {
                // Pick a random card that hasn't been shuffled yet
                var j = rng.Next(i, deck.Count);
                if (i == j)
                {
                    // Randomly chose to swap with itself, do nothing.
                    continue;
                }
                // Swap with the random card
                var temp = deck[j];
                deck[j] = deck[i];
                deck[i] = temp;
            }
            return deck;
        }
    }
}
