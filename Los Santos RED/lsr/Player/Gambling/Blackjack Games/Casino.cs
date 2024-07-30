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
    }

    public class Hand
    {
        public bool IsActive { get; set; }
        public string WriteHeader(string PlayerName)
        {
            string toReturn = $"{(IsActive ? "->" : "")}~b~{GetHandName(PlayerName)}:~s~ ({GetHandValue()})";
            return toReturn;
        }
        public int HandBet { get; set; }
        public bool HasTakenAction { get; set; }
        public int Order { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public bool WasSplit { get; internal set; }
        public bool IsBust => GetHandValue() > 21;
        public bool IsSurrendered { get; set; }
        public string GetHandName(string PlayerName)
        {
            string toRetun = "";
            if(Order == 0)
            {
                toRetun = "Primary Hand";
            }
            else
            {
                toRetun = $"Split Hand #{Order}";
            }
            return toRetun;
        }
        public int GetHandValue()
        {
            int value = 0;
            if (Cards == null)
            {
                return value;
            }
            foreach (Card card in Cards)
            {
                value += card.Value;
            }
            return value;
        }
        public bool CanSplit() => Cards != null && GetSplitCard() != null;
        public Card GetSplitCard()
        {
            if (Cards == null)
            {
                return null;
            }

            foreach(Card card in Cards)
            {
                foreach(Card card1 in Cards)
                {
                    if(card.Face == card1.Face && card != card1)
                    {
                        return card;
                    }
                }
            }
            return null;

            var something = Cards.GroupBy(x => x.Face).Where(x => x.Count() > 1);//.Select(g => g.Key);
            if(something == null)
            {
                return null;
            }
            EntryPoint.WriteToConsole($"GetSplitCard {something.Select(x=> x.Key).ToString()}");
            Face selectedFace = something.Select(x=> x.Key).FirstOrDefault();
            Card selectedCard = Cards.Where(x => x.Face == selectedFace).FirstOrDefault();
            return selectedCard;
        }
        public bool IsHandBlackjack()
        {
            if (Cards.Count == 2)
            {
                if (Cards[0].Face == Face.Ace && Cards[1].Value == 10) return true;
                else if (Cards[1].Face == Face.Ace && Cards[0].Value == 10) return true;
            }
            return false;
        }
        public string PrintCards()
        {
            string toReturn = $"";
            int i = 0;
            for (int i3 = Cards.Count - 1; i3 >= 0; i3--)
            {
                toReturn = toReturn + (i == 0 ? " " : "~n~") + Cards[i3].Description();
                i++;
            }
            return toReturn;
        }

        public void OnSurrendered()
        {
            IsSurrendered = true;
        }
    }

    public class CasinoPlayer
    {
        private ICasinoGamePlayable CasinoGamePlayable;
        private GamblingDen GameLocation;
        public CasinoPlayer(string name, ICasinoGamePlayable casinoGamePlayable, GamblingDen gameLocation)
        {
            Name = name;
            CasinoGamePlayable = casinoGamePlayable;
            GameLocation = gameLocation;
        }
        public string Name { get; set; }
        public int Bet { get; set; }
        public int Wins { get; set; }
        public int HandsCompleted { get; set; } = 1;
        public int TotalMoneyWon { get; set; }
        public int TotalMoneyBet { get; set; }
        public Hand PrimaryHand { get; set; }
        public List<Hand> SplitHands { get; set; }
        public Hand ActiveHand { get; private set; }
        public void AddBet(int bet)
        {
            Bet += bet;
            CasinoGamePlayable.BankAccounts.GiveMoney(-1 * bet, false);
            CasinoGamePlayable.GamblingManager.OnMoneyWon(GameLocation, -1 * bet);
            TotalMoneyBet += bet;
        }
        public void ClearBet()
        {
            Bet = 0;
        }
        public void ReturnBet()
        {
            CasinoGamePlayable.BankAccounts.GiveMoney(Bet, false);
            CasinoGamePlayable.GamblingManager.OnMoneyWon(GameLocation, Bet);
            TotalMoneyWon += Bet;
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
            CasinoGamePlayable.GamblingManager.OnMoneyWon(GameLocation, chipsWon);
            TotalMoneyWon += chipsWon;
            return chipsWon;
        }
        public string GetGameStatus()
        {
            string toReturn = "Session: ";
            if(Bet > 0)
            {
                toReturn = $"Current Bet: ${Bet}";
            }
            if (HandsCompleted > 0)
            {
                toReturn += $" Wins: ~g~{Wins}~s~ Hands: {HandsCompleted}";
            }
            if(TotalMoneyBet > 0)
            {
                toReturn += $"  Total Bets: ~r~${TotalMoneyBet}~s~ Total Won: ~g~${TotalMoneyWon}~s~";
            }
            return toReturn;
        }
        public bool OnSplitHands(Hand hand)
        {
            if(hand == null)
            {
                return false;
            }
            if(SplitHands == null)
            {
                SplitHands = new List<Hand>();
            }
            else if(SplitHands.Count >= 4)
            {
                return false;
            }
            Card selectedCard = hand.GetSplitCard();
            if (selectedCard!= null)
            {
                hand.Cards.Remove(selectedCard);
                int currentOrder = 1;
                if (SplitHands != null && SplitHands.Any())
                {
                    currentOrder = SplitHands.Max(x => x.Order) + 1;
                }
                SplitHands.Add(new Hand() { HandBet = Bet, Cards = new List<Card>() { selectedCard }, Order = currentOrder });
                CasinoGamePlayable.BankAccounts.GiveMoney(-1 * Bet, false);
                CasinoGamePlayable.GamblingManager.OnMoneyWon(GameLocation, -1 * Bet);
                TotalMoneyBet += Bet;
                return true;
            }
            return false;
        }
        public void OnSurrendered(Hand hand)
        {
            if(hand == null)
            {
                return;
            }
            CasinoGamePlayable.BankAccounts.GiveMoney((int)Math.Floor(Bet * 0.5), false);
            CasinoGamePlayable.GamblingManager.OnMoneyWon(GameLocation, (int)Math.Floor(Bet * 0.5));
            TotalMoneyWon += (int)Math.Floor(Bet * 0.5);
        }
        public void OnHit()
        {

        }
        public void ClearAllHands()
        {
            PrimaryHand = new Hand();
            PrimaryHand.Order = 0;
            SplitHands?.Clear();
        }
        public void SetActiveHand(Hand hand)
        {
            if(ActiveHand != null)
            {
                ActiveHand.IsActive = false;
            }
            EntryPoint.WriteToConsole("SetActiveHand RAN");
            ActiveHand = hand;
            ActiveHand.IsActive = true;
        }
    }
    public class Dealer
    {
        public Dealer(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
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
        public string WriteHeader()
        {
            string toReturn = $"~o~{Name}:~s~ ({GetHandValue()})";
            return toReturn;
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