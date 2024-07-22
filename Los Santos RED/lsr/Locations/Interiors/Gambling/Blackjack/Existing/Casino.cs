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
        public Casino(int minimumBet, int maximumBet)
        {
            MinimumBet = minimumBet;
            MaximumBet = maximumBet;
        }
        public int MinimumBet { get; }
        public int MaximumBet { get; set; }
        public bool IsHandBlackjack(List<Card> hand)
        {
            if (hand.Count == 2)
            {
                if (hand[0].Face == Face.Ace && hand[1].Value == 10) return true;
                else if (hand[1].Face == Face.Ace && hand[0].Value == 10) return true;
            }
            return false;
        }
    }
    public class CasinoPlayer
    {
        private ICasinoGamePlayable CasinoGamePlayable;
        public CasinoPlayer(string name,ICasinoGamePlayable casinoGamePlayable)
        {
            Name = name;
            CasinoGamePlayable = casinoGamePlayable;
        }
        public string Name { get; set; }
        public int Bet { get; set; }
        public int Wins { get; set; }
        public int HandsCompleted { get; set; } = 1;
        public List<Card> Hand { get; set; }
        public void AddBet(int bet)
        {
            Bet += bet;
            CasinoGamePlayable.BankAccounts.GiveMoney(-1 * bet, false);
        }
        public void ClearBet()
        {
            Bet = 0;
        }
        public void ReturnBet()
        {
            CasinoGamePlayable.BankAccounts.GiveMoney(Bet, false);
            ClearBet();
        }
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
            ClearBet();
            return chipsWon;
        }
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
        public string WriteHand()
        {
            if(Hand == null)
            {
                return "";
            }
            string toReturn = $"~b~{Name}:~s~ ({GetHandValue()})";
            int i = 0;
            //foreach (Card card in Hand)
            //{
            //    toReturn = toReturn + (i == 0 ? " " : ", ") + card.Description();
            //    i++;
            //}

            for (int i3 = Hand.Count - 1; i3 >= 0; i3--)
            {
                toReturn = toReturn + (i == 0 ? " " : ", ") + Hand[i3].Description();
                i++;
            }


            return toReturn;
        }
        public string PrintCards()
        {
            if (Hand == null)
            {
                return "";
            }
            string toReturn = $"";
            int i = 0;
            //foreach (Card card in Hand)
            //{
            //    toReturn = toReturn + (i == 0 ? " " : "~n~") + card.Description();
            //    i++;
            //}

            for (int i3 = Hand.Count - 1; i3 >= 0; i3--)
            {
                toReturn = toReturn + (i == 0 ? " " : "~n~") + Hand[i3].Description();
                i++;
            }

            return toReturn;
        }
    }
    public class Dealer
    {
        public Dealer(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<Card> TotalCards
        {
            get
            {
                List<Card> totalList = new List<Card>();
                totalList.AddRange(HiddenCards);
                totalList.AddRange(RevealedCards);
                return TotalCards;
            }
        }

        public List<Card> HiddenCards { get; set; } = new List<Card>();
        public List<Card> RevealedCards { get; set; } = new List<Card>();
        public Card RevealCard()
        {
            Card toReturn = HiddenCards[0];
            RevealedCards.Add(toReturn);
            HiddenCards.RemoveAt(0);
            return toReturn;
        }
        public int GetHandValue()
        {
            int value = 0;
            foreach (Card card in RevealedCards)
            {
                value += card.Value;
            }
            return value;
        }
        public string WriteHand()
        {
            string toReturn = $"~o~{Name}:~s~ ({GetHandValue()})";
            int i2 = 0;
            //foreach (Card card in RevealedCards)
            //{
            //    toReturn = toReturn + (i2 == 0 ? " " : ", ") + card.Description();
            //    i2++;
            //}

            for (int i3 = RevealedCards.Count - 1; i3 >= 0; i3--)
            {
                toReturn = toReturn + (i2 == 0 ? " " : ", ") + RevealedCards[i3].Description();
                i2++;
            }


            for (int i = 0; i < HiddenCards.Count; i++)
            {
                toReturn = toReturn + ", ~o~?~s~";
            }
            return toReturn;
        }
    }
}