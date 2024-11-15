using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LosSantosRED.lsr
{
    public class SearchMode
    {
        private IPoliceRespondable Player;
        private bool PrevIsInSearchMode;
        private bool PrevIsInActiveMode;
        private uint GameTimeStartedSearchMode;
        private uint GameTimeStartedActiveMode;
        private ISettingsProvideable Settings;
        private bool IsPlayerOutsidePoliceRadius;
        private uint LastUpdateGameTime;
        private uint TotalTimeOutsidePoliceRadius;

        public bool IsActive { get; private set; } = true;
        public SearchMode(IPoliceRespondable currentPlayer, ISettingsProvideable settings)
        {
            Player = currentPlayer;
            Settings = settings;
        }
        public float SearchModePercentage => IsInSearchMode ? 1.0f - ((float)TimeInSearchMode / (float)CurrentSearchTime) : 0;
        public bool IsInStartOfSearchMode => IsInSearchMode && SearchModePercentage >= Settings.SettingsManager.PoliceTaskSettings.SixthSenseSearchModeLimitPercentage;
        public bool IsInSearchMode { get; private set; }
        public bool IsInActiveMode { get; private set; }
        public uint TimeInSearchMode => IsInSearchMode && GameTimeStartedSearchMode != 0 ? (Game.GameTime - GameTimeStartedSearchMode) - (TotalTimeOutsidePoliceRadius * Settings.SettingsManager.PoliceSettings.OutsidePoliceResponseSearchScalar) : 0;
      //  private uint TimeInActiveMode => IsInActiveMode ? Game.GameTime - GameTimeStartedActiveMode : 0;
        private uint CurrentSearchTime => (uint)Player.WantedLevel * Settings.SettingsManager.PoliceSettings.SearchTimeMultiplier;//30000;//30 seconds each
        //private uint CurrentActiveTime => (uint)Player.WantedLevel * 30000;//30 seconds each
       // private string DebugString { get; set; }
        public void Update()
        {
            if (IsActive)
            {
                DetermineMode();
                ToggleModes();
                Player.IsInSearchMode = IsInSearchMode;
                LastUpdateGameTime = Game.GameTime;
            }
            //DebugString = IsInSearchMode ? $"TimeInSearchMode: {TimeInSearchMode}, CurrentSearchTime: {CurrentSearchTime}" + $" SearchModePercentage: {SearchModePercentage}" : $"TimeInActiveMode: {TimeInActiveMode}, CurrentActiveTime: {CurrentActiveTime}";
        }
        public void Dispose()
        {
            IsActive = false;
        }
        private void DetermineMode()
        {
            if (Player.IsWanted)// && Player.HasBeenWantedFor >= 5000)
            {
                if (Player.AnyPoliceRecentlySeenPlayer)
                {
                    IsInActiveMode = true;
                    IsInSearchMode = false;
                }
                else if (Player.AnyPoliceKnowInteriorLocation)
                {
                    IsInActiveMode = true;
                    IsInSearchMode = false;
                }
                else
                {
                    if(IsInSearchMode && Player.WantedLevel == 1 && Settings.SettingsManager.PoliceSettings.DisableSearchModeAtOneStart)
                    {
                        IsInActiveMode = false;
                        IsInSearchMode = false;
                        Player.OnSuspectEluded();
                    }
                    else if (IsInSearchMode && TimeInSearchMode >= CurrentSearchTime)
                    {
                        IsInActiveMode = false;
                        IsInSearchMode = false;
                        Player.OnSuspectEluded();
                    }
                    else
                    {
                        IsInActiveMode = false;
                        IsInSearchMode = true;
                    }
                }
            }
            else
            {
                IsInActiveMode = false;
                IsInSearchMode = false;
            }

            if(IsInSearchMode && !Player.PoliceResponse.IsWithinPoliceRadius)
            {
                TotalTimeOutsidePoliceRadius += LastUpdateGameTime - Game.GameTime;
            }




        }
        //private void OnPlayerWentOutsidePoliceRadius()
        //{
        //    IsPlayerOutsidePoliceRadius = true;
        //}
        //private void OnPlayerWentInsidePoliceRadius()
        //{
        //    IsPlayerOutsidePoliceRadius = false;
        //}
        private void ToggleModes()
        {
            if (PrevIsInActiveMode != IsInActiveMode)
            {
                if (IsInActiveMode)
                {
                    StartActiveMode();
                }
            }

            if (PrevIsInSearchMode != IsInSearchMode)
            {
                if (IsInSearchMode)
                {
                    StartSearchMode();
                }
                else
                {
                    EndSearchMode();
                }
            }
        }
        private void StartSearchMode()
        {
            IsInActiveMode = false;
            IsInSearchMode = true;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedSearchMode = Game.GameTime;
            GameTimeStartedActiveMode = 0;
            TotalTimeOutsidePoliceRadius = 0;
            LastUpdateGameTime = Game.GameTime;
            Player.OnWantedSearchMode();
            //EntryPoint.WriteToConsole("SearchMode Start Search Mode");
        }
        private void StartActiveMode()
        {
            IsInActiveMode = true;
            IsInSearchMode = false;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedActiveMode = Game.GameTime;
            GameTimeStartedSearchMode = 0;
            TotalTimeOutsidePoliceRadius = 0;
            Player.OnWantedActiveMode();
            //EntryPoint.WriteToConsole("SEARCH MODE: Start Active Mode");
        }
        private void EndSearchMode()
        {
            IsInActiveMode = false;
            IsInSearchMode = false;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedSearchMode = 0;
            GameTimeStartedActiveMode = 0;
            TotalTimeOutsidePoliceRadius = 0;
            Player.SetWantedLevel(0, "Search Mode Timeout", true);
            //EntryPoint.WriteToConsole("SEARCH MODE: End Search Mode");
        }
    }
}