using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Blackjack.Suit;
using static Blackjack.Face;

namespace Blackjack
{
    public enum Suit
    {
        Clubs,
        Spades,
        Diamonds,
        Hearts
    }
    public enum Face
    {
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
    public class Card
    {
        public Suit Suit { get; }
        public Face Face { get; }
        public int Value { get; set; }
        public Card(Suit suit, Face face)
        {
            Suit = suit;
            Face = face;
            switch (Face)
            {
                case Ten:
                case Jack:
                case Queen:
                case King:
                    Value = 10;
                    break;
                case Ace:
                    Value = 11;
                    break;
                default:
                    Value = (int)Face + 1;
                    break;
            }
        }
        public string Description()
        {
            if (Face == Ace)
            {
                if (Value == 11)
                {
                    return " Soft " + Face + " of " + Suit;
                }
                else
                {
                    return " Hard " + Face + " of " + Suit;
                }
            }
            else
            {
                 return Face + " of " + Suit;
            }
        }
    }
}