using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Helper;
using Rage;

namespace Roulette
{
    public class EvenOddBetMenu : RouletteBetMenu
    {
        protected UIMenuListScrollerItem<string> BetTypeScroller;
        public override string MainTitle => "Even/Odd Bets";
        public override string MainDescription => "Make bets on the pocket being even or odd. ~n~Pays 1 to 1.";

        public override int SortOrder => 30;
        public EvenOddBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (EvenOddBet sb in RouletteGame.RouletteRoundBet.EvenOddBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.EvenOddBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        protected override void CreateBetTypeScroller()
        {
            BetTypeScroller = new UIMenuListScrollerItem<string>("Type", "Selected type", new List<string> { "Even", "Odd" });
            MakeNewBetMenu.AddItem(BetTypeScroller);
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            bool isOdd = BetTypeScroller.SelectedItem == "Odd";
            if (RouletteGame.RouletteRoundBet.EvenOddBets != null && RouletteGame.RouletteRoundBet.EvenOddBets.Any(x => x.IsOdd == isOdd))
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.EvenOddBets.Add(new EvenOddBet(isOdd, BetAmountScroller.Value));
                Player.BankAccounts.GiveMoney(-1 * BetAmountScroller.Value, false);
                Player.GamblingManager.OnMoneyWon(GameLocation, -1 * BetAmountScroller.Value);
                menu.Visible = false;
                UpdateBetAmount();
                MainBetsMenu.Visible = true;
            }
        }
        public override void UpdateBetAmount()
        {
            int totalBets = RouletteGame.RouletteRoundBet.EvenOddBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
