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
    public class SplitBetMenu : RouletteBetMenu
    {
        protected UIMenuListScrollerItem<RoulettePocket> BetTypeScroller1;
        protected UIMenuListScrollerItem<RoulettePocket> BetTypeScroller2;
        public override string MainTitle => "Split Bets";
        public override string MainDescription => "Make bets on the pocket being in any two adjoining numbers, vertical or horizontal. ~n~Pays ~o~17 to 1~s~.";
        public override int SortOrder => 2;
        public SplitBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (SplitBet sb in RouletteGame.RouletteRoundBet.SplitBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.SplitBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        protected override void CreateBetTypeScroller()
        {
            BetTypeScroller1 = new UIMenuListScrollerItem<RoulettePocket>("First Pocket", "Select the first pocket.", RouletteGame.RouletteWheel.PocketsList) { Formatter = v => v.PocketDisplay };
            MakeNewBetMenu.AddItem(BetTypeScroller1);

            BetTypeScroller1.IndexChanged += (sender,oldIndex,newIndex) =>
            {
                BetTypeScroller2.Items = RouletteGame.GetSplitBetAdjoiningPockets(BetTypeScroller1?.SelectedItem);
            };

            BetTypeScroller2 = new UIMenuListScrollerItem<RoulettePocket>("Second Pocket", "Select an adjoining second pocket.", RouletteGame.GetSplitBetAdjoiningPockets(BetTypeScroller1?.SelectedItem)) { Formatter = v => v.PocketDisplay };
            MakeNewBetMenu.AddItem(BetTypeScroller2);
        }



        protected override void MakeBetActivated(UIMenu menu)
        {
            int firstNumber = BetTypeScroller1.SelectedItem.PocketID;
            int secondNumber = BetTypeScroller2.SelectedItem.PocketID;
            if (RouletteGame.RouletteRoundBet.SplitBets != null && RouletteGame.RouletteRoundBet.SplitBets.Any(x => x.PrimaryPocketID.PocketID == firstNumber && x.SecondaryPocketID.PocketID == secondNumber))
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.SplitBets.Add(new SplitBet(BetTypeScroller1.SelectedItem, BetTypeScroller2.SelectedItem, BetAmountScroller.Value));
                Player.BankAccounts.GiveMoney(-1 * BetAmountScroller.Value, false);
                Player.GamblingManager.OnMoneyWon(GameLocation, -1 * BetAmountScroller.Value);
                menu.Visible = false;
                UpdateBetAmount();
                MainBetsMenu.Visible = true;
            }
        }
        public override void UpdateBetAmount()
        {
            int totalBets = RouletteGame.RouletteRoundBet.SplitBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
