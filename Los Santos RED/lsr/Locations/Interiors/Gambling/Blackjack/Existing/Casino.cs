using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Casino
    {
        private string versionCode = "1.0";

        public int MinimumBet { get; } = 10;
        public int MaximumBet { get; set; } = 5000;

        public string GetVersionCode() { return versionCode; }

        /// <param name="hand">The hand to check</param>
        /// <returns>Returns true if the hand is blackjack</returns>
        public bool IsHandBlackjack(List<Card> hand)
        {
            if (hand.Count == 2)
            {
                if (hand[0].Face == Face.Ace && hand[1].Value == 10) return true;
                else if (hand[1].Face == Face.Ace && hand[0].Value == 10) return true;
            }
            return false;
        }

        /// <summary>
        /// Reset Console Colors to DarkGray on Black
        /// </summary>
        public void ResetColor()
        {
            //Console.ForegroundColor = ConsoleColor.DarkGray;
            //Console.BackgroundColor = ConsoleColor.Black;
        }
    }

    public class CasinoPlayer
    {
        private ICasinoGamePlayable CasinoGamePlayable;
        public CasinoPlayer(ICasinoGamePlayable casinoGamePlayable)
        {
            CasinoGamePlayable = casinoGamePlayable;
        }

        //public int Chips { get; set; } = 500;
        public int Bet { get; set; }
        public int Wins { get; set; }
        public int HandsCompleted { get; set; } = 1;

        public List<Card> Hand { get; set; }

        /// <summary>
        /// Add Player's chips to their bet.
        /// </summary>
        /// <param name="bet">The number of Chips to bet</param>
        public void AddBet(int bet)
        {
            Bet += bet;

            CasinoGamePlayable.BankAccounts.GiveMoney(-1 * bet, false);
           // Chips -= bet;
        }

        /// <summary>
        /// Set Bet to 0
        /// </summary>
        public void ClearBet()
        {
            Bet = 0;
        }

        /// <summary>
        /// Cancel player's bet. They will neither lose nor gain any chips.
        /// </summary>
        public void ReturnBet()
        {
            // Chips += Bet;

            CasinoGamePlayable.BankAccounts.GiveMoney(Bet, false);

            ClearBet();
        }

        /// <summary>
        /// Give player chips that they won from their bet.
        /// </summary>
        /// <param name="blackjack">If player won with blackjack, player wins 1.5 times their bet</param>
        public int WinBet(bool blackjack)
        {
            int chipsWon;
            if (blackjack)
            {
                chipsWon = (int)Math.Floor(Bet * 1.5);
            }
            else
            {
                chipsWon = Bet * 2;
            }

            CasinoGamePlayable.BankAccounts.GiveMoney(chipsWon,false);

            //Chips += chipsWon;
            ClearBet();
            return chipsWon;
        }

        /// <returns>
        /// Value of all cards in Hand
        /// </returns>
        public int GetHandValue()
        {
            int value = 0;
            if(Hand == null)
            {
                return value;
            }
            foreach (Card card in Hand)
            {
                value += card.Value;
            }
            return value;
        }

        /// <summary>
        /// Write player's hand to console.
        /// </summary>
        public string WriteHand()
        {
            // Write Bet, Chip, Win, Amount with color, and write Round #
            string toReturn = "";
            //toReturn = $"Bet: {Bet} Chips: {Chips} Wins: {Wins} Round # {HandsCompleted} Your Hand ({GetHandValue()}):";

            //Console.Write(Bet + "  ");
            //Casino.ResetColor();
            //Console.Write("Chips: ");
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.Write(Chips + "  ");
            //Casino.ResetColor();
            //Console.Write("Wins: ");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine(Wins);
            //Casino.ResetColor();
            //Console.WriteLine("Round #" + HandsCompleted);

            //Console.WriteLine();
            //Console.WriteLine("Your Hand (" + GetHandValue() + "):");

            if(Hand == null)
            {
                return "";
            }

            toReturn = $"Hand: ({GetHandValue()})";


            foreach (Card card in Hand)
            {
                toReturn = toReturn + ", " + card.Description();
            }
            return toReturn;
            //Console.WriteLine();
        }
    }

    public class Dealer
    {
        public List<Card> HiddenCards { get; set; } = new List<Card>();
        public List<Card> RevealedCards { get; set; } = new List<Card>();

        /// <summary>
        /// Take the top card from HiddenCards, remove it, and add it to RevealedCards.
        /// </summary> 
        public void RevealCard()
        {
            RevealedCards.Add(HiddenCards[0]);
            HiddenCards.RemoveAt(0);
        }

        /// <returns>
        /// Value of all cards in RevealedCards
        /// </returns>
        public int GetHandValue()
        {
            int value = 0;
            foreach (Card card in RevealedCards)
            {
                value += card.Value;
            }
            return value;
        }

        /// <summary>
        /// Write Dealer's RevealedCards to Console.
        /// </summary>
        public string WriteHand()
        {
            string toReturn = $"Dealer: ({GetHandValue()})";
            foreach (Card card in RevealedCards)
            {
                toReturn = toReturn + ", " + card.Description();
            }
            for (int i = 0; i < HiddenCards.Count; i++)
            {
                toReturn = toReturn + ", ~o~?~c~";
            }
            return toReturn;
        }
    }
}