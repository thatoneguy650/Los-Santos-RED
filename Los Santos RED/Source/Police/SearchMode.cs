using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class SearchMode
{
    private static bool PrevIsInSearchMode;
    private static bool PrevIsInActiveMode;
    private static uint GameTimeStartedSearchMode;
    private static uint GameTimeStartedActiveMode;
    public static bool IsInSearchMode { get; set; }
    public static bool IsInActiveMode { get; set; }
    public static bool IsRunning { get; set; }
    public static uint TimeInSearchMode
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
    public static uint TimeInActiveMode
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
    public static float BlipSize
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
                    return PersonOfInterest.SearchRadius * TimeInSearchMode / CurrentSearchTime;
                }
            }
        }
    }
    public static Color BlipColor
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
    public static uint CurrentSearchTime
    {
        get
        {
            return (uint)PlayerState.WantedLevel * 30000;//30 seconds each
        }
    }
    public static uint CurrentActiveTime
    {
        get
        {
            return (uint)PlayerState.WantedLevel * 30000;//30 seconds each
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            UpdateWanted();
        }
    }
    private static void UpdateWanted()
    {
        DetermineMode();
        ToggleModes();
        HandleFlashing();
    }
    private static void DetermineMode()
    {
        if (PlayerState.IsWanted)
        {
            if (Police.AnyRecentlySeenPlayer)
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
    private static void ToggleModes()
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
    private static void StartSearchMode()
    {
        IsInActiveMode = false;
        IsInSearchMode = true;
        PrevIsInSearchMode = IsInSearchMode;
        PrevIsInActiveMode = IsInActiveMode;
        GameTimeStartedSearchMode = Game.GameTime;
        GameTimeStartedActiveMode = 0;
        Debugging.WriteToLog("SearchMode", "Start Search Mode");
    }
    private static void StartActiveMode()
    {
        IsInActiveMode = true;
        IsInSearchMode = false;
        PrevIsInSearchMode = IsInSearchMode;
        PrevIsInActiveMode = IsInActiveMode;
        GameTimeStartedActiveMode = Game.GameTime;
        GameTimeStartedSearchMode = 0;
        Debugging.WriteToLog("SearchMode", "Start Active Mode");
    }
    private static void EndSearchMode()
    {
        Debugging.WriteToLog("SearchMode", string.Format("End Search Mode Start, IsInSearchMode {0} IsInActiveMode {1}, TimeInSearchMode {2}, TimeInActiveMode {3}", IsInSearchMode, IsInActiveMode, TimeInSearchMode, TimeInActiveMode));
        Debugging.WriteToLog("SearchMode", string.Format("End Search Mode Start, AnyRecentlySeenPlayer {0},CurrentSearchTime {1}, CurrentActiveTime {2}", Police.AnyRecentlySeenPlayer, CurrentSearchTime, CurrentActiveTime));
        IsInActiveMode = false;
        IsInSearchMode = false;
        PrevIsInSearchMode = IsInSearchMode;
        PrevIsInActiveMode = IsInActiveMode;
        GameTimeStartedSearchMode = 0;
        GameTimeStartedActiveMode = 0;
        WantedLevelScript.SetWantedLevel(0, "Search Mode Timeout", true);
        Debugging.WriteToLog("SearchMode", string.Format("End Search Mode End, IsInSearchMode {0} IsInActiveMode {1}, TimeInSearchMode {2}, TimeInActiveMode {3}", IsInSearchMode, IsInActiveMode, TimeInSearchMode, TimeInActiveMode));
        Debugging.WriteToLog("SearchMode", string.Format("End Search Mode End, AnyRecentlySeenPlayer {0},CurrentSearchTime {1}, CurrentActiveTime {2}", Police.AnyRecentlySeenPlayer, CurrentSearchTime, CurrentActiveTime));

    }
    private static void HandleFlashing()
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
