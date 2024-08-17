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
    public class HalfBetMenu : RouletteBetMenu
    {
        protected UIMenuListScrollerItem<string> BetTypeScroller;
        public override string MainTitle => "1 to 18 19 to 36 Bets";
        public override string MainDescription => "Make bets on the pocket being in the first or second half. ~n~Pays 1 to 1.";
        public override int SortOrder => 32;
        public HalfBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (HalfBet sb in RouletteGame.RouletteRoundBet.HalfBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.HalfBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        protected override void CreateBetTypeScroller()
        {
            BetTypeScroller = new UIMenuListScrollerItem<string>("Type", "Selected type", new List<string> { "1 to 18", "19 to 36" });
            MakeNewBetMenu.AddItem(BetTypeScroller);
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            bool isFirst = BetTypeScroller.SelectedItem == "1 to 18";
            if (RouletteGame.RouletteRoundBet.HalfBets != null && RouletteGame.RouletteRoundBet.HalfBets.Any(x => x.IsFirst == isFirst))
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.HalfBets.Add(new HalfBet(isFirst, BetAmountScroller.Value));
                Player.BankAccounts.GiveMoney(-1 * BetAmountScroller.Value, false);
                Player.GamblingManager.OnMoneyWon(GameLocation, -1 * BetAmountScroller.Value);
                menu.Visible = false;
                UpdateBetAmount();
                MainBetsMenu.Visible = true;
            }
        }
        public override void UpdateBetAmount()
        {
            int totalBets = RouletteGame.RouletteRoundBet.HalfBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
