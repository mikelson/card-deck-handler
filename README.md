# card-deck-handler
Author: Peter Mikelsons

C# library for sorting and shuffling "standard" playing cards. It provides the following:

1. PlayingCard, a class representing a playing card.

2. void CardDeckSorter.Sort(List<PlayingCard>, Comparison<PlayingCard>, which sorts a standard deck of 52
(or any number actually) playing cards in ascending order. You provide a delegate to determine what
the term “ascending order” means for a deck of cards.

3. List<PlayingCard> CardDeckShuffler.Create(Random rng) that randomly shuffles a standard deck of 52
playing cards.

This code could used in a variety of games that use a standard deck of 52 playing cards (four suits of 
thirteen cards each). A Visual Studio test project provides tests and examples of use for the above
class and functions.
