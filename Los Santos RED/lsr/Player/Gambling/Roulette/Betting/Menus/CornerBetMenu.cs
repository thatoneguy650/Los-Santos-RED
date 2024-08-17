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
    public class CornerBetMenu : RouletteBetMenu
    {
        protected UIMenuListScrollerItem<CornerBet> BetTypeScroller;
        public override string MainTitle => "Corner Bets";
        public override string MainDescription => "Make bets on four numbers that meet at one corner. ~n~Pays ~o~8 to 1~s~.";

        public override int SortOrder => 5;
        public CornerBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (CornerBet sb in RouletteGame.RouletteRoundBet.CornerBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.CornerBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        private CornerBet CreateCorner(int one,int two, int three, int four)
        {
            return new CornerBet(RouletteGame.RouletteWheel.GetPocket(one), RouletteGame.RouletteWheel.GetPocket(two), RouletteGame.RouletteWheel.GetPocket(three), RouletteGame.RouletteWheel.GetPocket(four), 0);
        }
        protected override void CreateBetTypeScroller()
        {
            List<CornerBet> possibleCorners = new List<CornerBet>()
            {
                CreateCorner(1,2,4,5),
                CreateCorner(4,5,7,8),
                CreateCorner(7,8,10,11),
                CreateCorner(10,11,13,14),
                CreateCorner(13,14,16,17),
                CreateCorner(16,17,19,20),
                CreateCorner(19,20,22,23),
                CreateCorner(22,23,25,26),
                CreateCorner(25,26,28,29),
                CreateCorner(28,29,31,32),
                CreateCorner(31,32,34,35),
                CreateCorner(2,3,5,6),
                CreateCorner(5,6,8,9),
                CreateCorner(8,9,11,12),
                CreateCorner(11,12,14,15),
                CreateCorner(14,15,17,18),
                CreateCorner(17,18,20,21),
                CreateCorner(20,21,23,24),
                CreateCorner(23,24,26,27),
                CreateCorner(26,27,29,30),
                CreateCorner(29,30,32,33),
                CreateCorner(32,33,35,36),
            };
            BetTypeScroller = new UIMenuListScrollerItem<CornerBet>("Corner", "Selected corner", possibleCorners) { Formatter = v => $"{v.BetName}" };
            MakeNewBetMenu.AddItem(BetTypeScroller);
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            if (RouletteGame.RouletteRoundBet.CornerBets != null && RouletteGame.RouletteRoundBet.CornerBets.Any(x => 
            x.PrimaryPocketID.PocketID == BetTypeScroller.SelectedItem.PrimaryPocketID.PocketID &&
            x.SecondaryPocketID.PocketID == BetTypeScroller.SelectedItem.SecondaryPocketID.PocketID &&
            x.TeritaryPocketID.PocketID == BetTypeScroller.SelectedItem.TeritaryPocketID.PocketID &&
            x.QuaternaryPocketID.PocketID == BetTypeScroller.SelectedItem.QuaternaryPocketID.PocketID
            )
                )
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.CornerBets.Add(new CornerBet(
                    BetTypeScroller.SelectedItem.PrimaryPocketID,
                    BetTypeScroller.SelectedItem.SecondaryPocketID,
                    BetTypeScroller.SelectedItem.TeritaryPocketID,
                    BetTypeScroller.SelectedItem.QuaternaryPocketID,
                    BetAmountScroller.Value));
                Player.BankAccounts.GiveMoney(-1 * BetAmountScroller.Value, false);
                Player.GamblingManager.OnMoneyWon(GameLocation, -1 * BetAmountScroller.Value);
                menu.Visible = false;
                UpdateBetAmount();
                MainBetsMenu.Visible = true;
            }
        }
        public override void UpdateBetAmount()
        {
            int totalBets = RouletteGame.RouletteRoundBet.CornerBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
