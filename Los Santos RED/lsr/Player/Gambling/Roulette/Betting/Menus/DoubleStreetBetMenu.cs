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
    public class DoubleStreetBetMenu : RouletteBetMenu
    {
        protected UIMenuListScrollerItem<DoubleStreetBet> BetTypeScroller;
        public override string MainTitle => "Double Street Bets";
        public override string MainDescription => "Make bets on six numbers for two horizontal rows. ~n~Pays ~o~5 to 1~s~.";

        public override int SortOrder => 11;
        public DoubleStreetBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (DoubleStreetBet sb in RouletteGame.RouletteRoundBet.DoubleStreetBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.DoubleStreetBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        private DoubleStreetBet CreateStreet(int one, int two, int three, int four, int five, int six)
        {
            return new DoubleStreetBet(RouletteGame.RouletteWheel.GetPocket(one), RouletteGame.RouletteWheel.GetPocket(two), RouletteGame.RouletteWheel.GetPocket(three),

                RouletteGame.RouletteWheel.GetPocket(four), RouletteGame.RouletteWheel.GetPocket(five), RouletteGame.RouletteWheel.GetPocket(six),

                0);
        }
        protected override void CreateBetTypeScroller()
        {
            List<DoubleStreetBet> possibleCorners = new List<DoubleStreetBet>()
            {
                CreateStreet(1,2,3, 4,5,6),
                CreateStreet(4,5,6,7,8,9),
                CreateStreet(7,8,9,10,11,12),
                CreateStreet(10,11,12,13,14,15),
                CreateStreet(13,14,15,16,17,18),
                CreateStreet(16,17,18,19,20,21),
                CreateStreet(19,20,21,22,23,24),
                CreateStreet(22,23,24,25,26,27),
                CreateStreet(25,26,27,28,29,30),
                CreateStreet(28,29,30,31,32,33),
                CreateStreet(31,32,33,34,35,36),
            };
            BetTypeScroller = new UIMenuListScrollerItem<DoubleStreetBet>("Double Street", "Selected double street", possibleCorners) { Formatter = v => $"{v.BetName}" };
            MakeNewBetMenu.AddItem(BetTypeScroller);
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            if (RouletteGame.RouletteRoundBet.DoubleStreetBets != null && RouletteGame.RouletteRoundBet.DoubleStreetBets.Any(x =>
            x.PrimaryPocketID.PocketID == BetTypeScroller.SelectedItem.PrimaryPocketID.PocketID &&
            x.SecondaryPocketID.PocketID == BetTypeScroller.SelectedItem.SecondaryPocketID.PocketID &&
            x.TeritaryPocketID.PocketID == BetTypeScroller.SelectedItem.TeritaryPocketID.PocketID &&

            x.QuaternaryPocketID.PocketID == BetTypeScroller.SelectedItem.QuaternaryPocketID.PocketID &&
            x.QuinaryPocketID.PocketID == BetTypeScroller.SelectedItem.QuinaryPocketID.PocketID &&
            x.SenaryPocketID.PocketID == BetTypeScroller.SelectedItem.SenaryPocketID.PocketID
            )
                )
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.DoubleStreetBets.Add(new DoubleStreetBet(
                    BetTypeScroller.SelectedItem.PrimaryPocketID,
                    BetTypeScroller.SelectedItem.SecondaryPocketID,
                    BetTypeScroller.SelectedItem.TeritaryPocketID,

                    BetTypeScroller.SelectedItem.QuaternaryPocketID,
                    BetTypeScroller.SelectedItem.QuinaryPocketID,
                    BetTypeScroller.SelectedItem.SenaryPocketID,


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
            int totalBets = RouletteGame.RouletteRoundBet.DoubleStreetBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
