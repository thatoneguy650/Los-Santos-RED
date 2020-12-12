using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SearchModeManager
{
    private bool PrevIsInSearchMode;
    private bool PrevIsInActiveMode;
    private uint GameTimeStartedSearchMode;
    private uint GameTimeStartedActiveMode;
    public bool IsInSearchMode { get; private set; }
    public bool IsInActiveMode { get; private set; }
    public uint TimeInSearchMode
    {
        get
        {
            if(IsInSearchMode)
            {
                if (GameTimeStartedSearchMode == 0)
                    return 0;
                else
                    return (Game.GameTime - GameTimeStartedSearchMode);
            }
            else
            {
                return 0;
            }
            
        }
    }
    public uint TimeInActiveMode
    {
        get
        {
            if (IsInActiveMode)
            {
                return (Game.GameTime - GameTimeStartedActiveMode);
            }
            else
            {
                return 0;
            }

        }
    }
    public float BlipSize
    {
        get
        {
            if(IsInActiveMode)
            {
                return 100f;
            }
            else
            {
                if (CurrentSearchTime == 0)
                {
                    return 100f;
                }
                else
                {
                    return Mod.PersonOfInterestManager.SearchRadius * TimeInSearchMode / CurrentSearchTime;
                }
            }
        }
    }
    public Color BlipColor
    {
        get
        {
            if (IsInActiveMode)
            {
                return Color.Red;
            }
            else
            {
                return Color.Orange;
            }
        }
    }
    public uint CurrentSearchTime
    {
        get
        {
            return (uint)Mod.Player.WantedLevel * 30000;//30 seconds each
        }
    }
    public uint CurrentActiveTime
    {
        get
        {
            return (uint)Mod.Player.WantedLevel * 30000;//30 seconds each
        }
    }
    public void Tick()
    {
        UpdateWanted();
    }
    private void UpdateWanted()
    {
        DetermineMode();
        ToggleModes();
        HandleFlashing();
    }
    private void DetermineMode()
    {
        if (Mod.Player.IsWanted)
        {
            if (Mod.PolicePerception.AnyRecentlySeenPlayer)
            {
                IsInActiveMode = true;
                IsInSearchMode = false;
            }
            else
            {
                IsInActiveMode = false;
                IsInSearchMode = true;
            }


            if (IsInSearchMode && TimeInSearchMode >= CurrentSearchTime)
            {
                IsInActiveMode = false;
                IsInSearchMode = false;
            }
        }
        else
        {
            IsInActiveMode = false;
            IsInSearchMode = false;
        }
    }
    private void ToggleModes()
    {
        if(PrevIsInActiveMode != IsInActiveMode)
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
        Debugging.WriteToLog("SearchMode", "Start Search Mode");
    }
    private void StartActiveMode()
    {
        IsInActiveMode = true;
        IsInSearchMode = false;
        PrevIsInSearchMode = IsInSearchMode;
        PrevIsInActiveMode = IsInActiveMode;
        GameTimeStartedActiveMode = Game.GameTime;
        GameTimeStartedSearchMode = 0;
        Debugging.WriteToLog("SearchMode", "Start Active Mode");
    }
    private void EndSearchMode()
    {
        IsInActiveMode = false;
        IsInSearchMode = false;
        PrevIsInSearchMode = IsInSearchMode;
        PrevIsInActiveMode = IsInActiveMode;
        GameTimeStartedSearchMode = 0;
        GameTimeStartedActiveMode = 0;
        Mod.Player.CurrentPoliceResponse.SetWantedLevel(0, "Search Mode Timeout", true);
        Debugging.WriteToLog("SearchMode", "Stop Search Mode");

    }
    private void HandleFlashing()
    {
        if (IsInActiveMode)
        {
            NativeFunction.CallByName<bool>("FLASH_WANTED_DISPLAY", true);
        }
        else
        {
            NativeFunction.CallByName<bool>("FLASH_WANTED_DISPLAY", false);
        }
    }
}
