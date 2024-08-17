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
    public class SingleBetMenu : RouletteBetMenu
    {
        private UIMenuListScrollerItem<RoulettePocket> SingleBetPocketScroller;
        public override string MainTitle => "Single Bets";
        public override string MainDescription => "Make bets on a single pocket. ~n~Pays ~r~35 to 1~s~.";
        public override int SortOrder => 0;
        public SingleBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (SingleBet sb in RouletteGame.RouletteRoundBet.SingleBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.SingleBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        protected override void CreateBetTypeScroller()
        {
            SingleBetPocketScroller = new UIMenuListScrollerItem<RoulettePocket>("Pocket", "Selected pocket", RouletteGame.RouletteWheel.PocketsList) { Formatter = v => v.PocketDisplay };
            MakeNewBetMenu.AddItem(SingleBetPocketScroller);
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            if (RouletteGame.RouletteRoundBet.SingleBets != null && RouletteGame.RouletteRoundBet.SingleBets.Any(x => x.RoulettePocket.PocketID == SingleBetPocketScroller.SelectedItem.PocketID))
            {
                Game.DisplaySubtitle("You already Have a bet for this pocket");
            }
            else
            {
                RouletteGame.RouletteRoundBet.SingleBets.Add(new SingleBet(SingleBetPocketScroller.SelectedItem, BetAmountScroller.Value));
                Player.BankAccounts.GiveMoney(-1 * BetAmountScroller.Value, false);
                Player.GamblingManager.OnMoneyWon(GameLocation, -1 * BetAmountScroller.Value);
                menu.Visible = false;
                MainBetsMenu.Visible = true;
                UpdateBetAmount();
            }
        }
        public override void UpdateBetAmount()
        {
            int totalBets = RouletteGame.RouletteRoundBet.SingleBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}