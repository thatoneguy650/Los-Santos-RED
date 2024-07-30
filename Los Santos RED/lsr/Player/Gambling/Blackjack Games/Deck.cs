using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Deck
    {
        private List<Card> cards;

        /// <summary>
        /// Initilize on creation of Deck.
        /// </summary>
        public Deck()
        {
            Initialize();
        }

        /// <returns>
        /// Returns a Cold Deck-- a deck organized by Suit and Face.
        /// </returns>
        public List<Card> GetColdDeck()
        {
            List<Card> coldDeck = new List<Card>();

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    coldDeck.Add(new Card((Suit)j, (Face)i));
                }
            }

            return coldDeck;
        }

        /// <summary>
        /// Remove top 2 cards of Deck and turn it into a list.
        /// </summary>
        /// <returns>List of 2 Cards</returns>
        public List<Card> DealHand()
        {
            // Create a temporary list of cards and give it the top two cards of the deck.
            List<Card> hand = new List<Card>();
            hand.Add(cards[0]);
            hand.Add(cards[1]);

            // Remove the cards added to the hand.
            cards.RemoveRange(0, 2);

            return hand;
        }

        /// <summary>
        /// Pick top card and remove it from the deck
        /// </summary>
        /// <returns>The top card of the deck</returns>
        public Card DrawCard()
        {
            Card card = cards[0];
            cards.Remove(card);

            return card;
        }

        /// <summary>
        /// Randomize the order of the cards in the Deck.
        /// </summary>
        public void Shuffle()
        {
            Random rng = new Random();

            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
            }
        }

        /// <summary>
        /// Replace the deck with a Cold Deck and then Shuffle it.
        /// </summary>
        public void Initialize()
        {
            cards = GetColdDeck();
            Shuffle();
        }
    }
}