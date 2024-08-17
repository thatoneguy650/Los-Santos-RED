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
    public class RowBetMenu : RouletteBetMenu
    {
        public override string MainTitle => "Row Bet";
        public override string MainDescription => "Make bet on the row of the pocket. ~n~Winning Pockets: 0, 00 ~n~Pays ~o~17 to 1~s~.";
        public override int SortOrder => 1;
        public RowBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (RowBet sb in RouletteGame.RouletteRoundBet.RowBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.RowBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            if (RouletteGame.RouletteRoundBet.RowBets != null && RouletteGame.RouletteRoundBet.RowBets.Any())
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.RowBets.Add(new RowBet(BetAmountScroller.Value));
                Player.BankAccounts.GiveMoney(-1 * BetAmountScroller.Value, false);
                Player.GamblingManager.OnMoneyWon(GameLocation, -1 * BetAmountScroller.Value);
                menu.Visible = false;
                UpdateBetAmount();
                MainBetsMenu.Visible = true;
            }
        }
        public override void UpdateBetAmount()
        {
            int totalBets = RouletteGame.RouletteRoundBet.RowBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
