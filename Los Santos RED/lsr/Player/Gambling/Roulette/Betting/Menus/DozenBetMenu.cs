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
    public class DozenBetMenu : RouletteBetMenu
    {
        protected UIMenuListScrollerItem<int> BetTypeScroller;
        public override string MainTitle => "Dozen Bets";
        public override string MainDescription => "Make bets on the dozen of the pocket.~n~Winning Pockets: ~n~1st Dozen: 1-12 ~n~2nd Dozen: 13-24 ~n~3rd Dozen: 25-36 ~n~Pays ~y~2 to 1~s~.";
        public override int SortOrder => 21;
        public DozenBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (DozenBet sb in RouletteGame.RouletteRoundBet.DozenBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.DozenBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        protected override void CreateBetTypeScroller()
        {
            BetTypeScroller = new UIMenuListScrollerItem<int>("Dozen", "Selected dozen", new List<int> { 1, 2, 3 }) { Formatter = v => $"{v}{(v == 1 ? "st" : v == 2 ? "nd" : v == 3 ? "rd" :"")} Dozen" };
            MakeNewBetMenu.AddItem(BetTypeScroller);
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            if (RouletteGame.RouletteRoundBet.DozenBets != null && RouletteGame.RouletteRoundBet.DozenBets.Any(x => x.Level == BetTypeScroller.SelectedItem))
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.DozenBets.Add(new DozenBet(BetTypeScroller.SelectedItem, BetAmountScroller.Value));
                Player.BankAccounts.GiveMoney(-1 * BetAmountScroller.Value, false);
                Player.GamblingManager.OnMoneyWon(GameLocation, -1 * BetAmountScroller.Value);
                menu.Visible = false;
                UpdateBetAmount();
                MainBetsMenu.Visible = true;
            }
        }
        public override void UpdateBetAmount()
        {
            int totalBets = RouletteGame.RouletteRoundBet.DozenBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
