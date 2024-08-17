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
    public class StreetBetMenu : RouletteBetMenu
    {
        protected UIMenuListScrollerItem<StreetBet> BetTypeScroller;
        public override string MainTitle => "Street Bets";
        public override string MainDescription => "Make bets on three numbers horizontally. ~n~Pays ~o~11 to 1~s~.";

        public override int SortOrder => 3;
        public StreetBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (StreetBet sb in RouletteGame.RouletteRoundBet.StreetBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.StreetBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        private StreetBet CreateStreet(int one, int two, int three)
        {
            return new StreetBet(RouletteGame.RouletteWheel.GetPocket(one), RouletteGame.RouletteWheel.GetPocket(two), RouletteGame.RouletteWheel.GetPocket(three), 0);
        }
        protected override void CreateBetTypeScroller()
        {
            List<StreetBet> possibleCorners = new List<StreetBet>()
            {
                CreateStreet(1,2,3),
                CreateStreet(4,5,6),
                CreateStreet(7,8,9),
                CreateStreet(10,11,12),
                CreateStreet(13,14,15),
                CreateStreet(16,17,18),
                CreateStreet(19,20,21),
                CreateStreet(22,23,24),
                CreateStreet(25,26,27),
                CreateStreet(28,29,30),
                CreateStreet(31,32,33),
                CreateStreet(34,35,36),
            };
            BetTypeScroller = new UIMenuListScrollerItem<StreetBet>("Street", "Selected street", possibleCorners) { Formatter = v => $"{v.BetName}" };
            MakeNewBetMenu.AddItem(BetTypeScroller);
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            if (RouletteGame.RouletteRoundBet.StreetBets != null && RouletteGame.RouletteRoundBet.StreetBets.Any(x =>
            x.PrimaryPocketID.PocketID == BetTypeScroller.SelectedItem.PrimaryPocketID.PocketID &&
            x.SecondaryPocketID.PocketID == BetTypeScroller.SelectedItem.SecondaryPocketID.PocketID &&
            x.TeritaryPocketID.PocketID == BetTypeScroller.SelectedItem.TeritaryPocketID.PocketID
            )
                )
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.StreetBets.Add(new StreetBet(
                    BetTypeScroller.SelectedItem.PrimaryPocketID,
                    BetTypeScroller.SelectedItem.SecondaryPocketID,
                    BetTypeScroller.SelectedItem.TeritaryPocketID,
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
            int totalBets = RouletteGame.RouletteRoundBet.StreetBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
