using System;
using System.Collections.Generic;

namespace ShiftWiseCardDeckHandler
{
    public class CardDeckSorter
    {
        /// <summary>
        /// Sorts a standard deck of 52 (or any number, really) playing cards in ascending order.
        /// </summary>
        /// <param name="deck">Collection to be sorted</param>
        /// <param name="comparison">Definition of ascending order, which varies by game and geography</param>
        public static void Sort(List<PlayingCard> deck, Comparison<PlayingCard> comparison)
        {
            deck?.Sort(comparison);
        }
    }
}
