using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WantedLevelStats
{
    public int MaxWantedLevel = 0;
    private int CopsKilledByPlayer = 0;
    private int CiviliansKilledByPlayer = 0;
    private bool PlayerHurtPolice = false;
    private bool PlayerKilledPolice = false;
    private bool PlayerKilledCivilians = false;
    private bool PlayerAimedAtPolice = false;
    private bool PlayerFiredWeaponNearPolice = false;
    private Police.PoliceState MaxPoliceState = Police.PoliceState.Normal;

    private bool DispatchReportedOfficerDown = false;
    private bool DispatchReportedLethalForceAuthorized = false;
    private bool DispatchReportedAssaultOnOfficer = false;
    private bool DispatchReportedShotsFired = false;
    public WantedLevelStats()
    {
        StoreValues();
    }
    public void StoreValues()
    {
        MaxWantedLevel = InstantAction.MaxWantedLastLife;
        CopsKilledByPlayer = Police.CopsKilledByPlayer;
        CiviliansKilledByPlayer = Police.CiviliansKilledByPlayer;
        PlayerHurtPolice = Police.PlayerHurtPolice;
        PlayerKilledPolice = Police.PlayerKilledPolice;
        PlayerKilledCivilians = Police.PlayerKilledCivilians;
        PlayerAimedAtPolice = Police.PlayerAimedAtPolice;
        PlayerFiredWeaponNearPolice = Police.PlayerFiredWeaponNearPolice;
        MaxPoliceState = Police.CurrentPoliceState;

        DispatchReportedOfficerDown = DispatchAudio.ReportedOfficerDown;
        DispatchReportedLethalForceAuthorized = DispatchAudio.ReportedLethalForceAuthorized;
        DispatchReportedAssaultOnOfficer = DispatchAudio.ReportedAssaultOnOfficer;
        DispatchReportedShotsFired = DispatchAudio.ReportedShotsFired;

        Debugging.WriteToLog("WantedLevelStats Store", string.Format("CopsKilledByPlayer: {0},CiviliansKilledByPlayer: {1},PlayerHurtPolice: {2},PlayerKilledPolice {3},PlayerKilledCivilians {4},PlayerAimedAtPolice: {5},PlayerFiredWeaponNearPolice: {6},MaxPoliceState: {7},MaxWantedLevel {8}",
            CopsKilledByPlayer, CiviliansKilledByPlayer, PlayerHurtPolice, PlayerKilledPolice, PlayerKilledCivilians, PlayerAimedAtPolice, PlayerFiredWeaponNearPolice, MaxPoliceState, MaxWantedLevel));
    }
    public void ReplaceValues()
    {
        if (Game.LocalPlayer.WantedLevel < MaxWantedLevel)
            Game.LocalPlayer.WantedLevel = MaxWantedLevel;

        Police.CopsKilledByPlayer = CopsKilledByPlayer;
        Police.CiviliansKilledByPlayer = CiviliansKilledByPlayer;
        Police.PlayerHurtPolice = PlayerHurtPolice;
        Police.PlayerKilledPolice = PlayerKilledPolice;
        Police.PlayerKilledCivilians = PlayerKilledCivilians;
        Police.PlayerAimedAtPolice = PlayerAimedAtPolice;
        Police.PlayerFiredWeaponNearPolice = PlayerFiredWeaponNearPolice;
        Police.CurrentPoliceState = MaxPoliceState;

        DispatchAudio.ReportedOfficerDown = DispatchReportedOfficerDown;
        DispatchAudio.ReportedLethalForceAuthorized = DispatchReportedLethalForceAuthorized;
        DispatchAudio.ReportedAssaultOnOfficer = DispatchReportedAssaultOnOfficer;
        DispatchAudio.ReportedShotsFired = DispatchReportedShotsFired;


        Debugging.WriteToLog("WantedLevelStats Replace", string.Format("CopsKilledByPlayer: {0},CiviliansKilledByPlayer: {1},PlayerHurtPolice: {2},PlayerKilledPolice {3},PlayerKilledCivilians {4},PlayerAimedAtPolice: {5},PlayerFiredWeaponNearPolice: {6},CurrentPoliceState: {7},Game.LocalPlayer.WantedLevel: {8}",
         Police.CopsKilledByPlayer, Police.CiviliansKilledByPlayer, Police.PlayerHurtPolice, Police.PlayerKilledPolice, Police.PlayerKilledCivilians, Police.PlayerAimedAtPolice, Police.PlayerFiredWeaponNearPolice, Police.CurrentPoliceState, Game.LocalPlayer.WantedLevel));
    }
    public void ClearValues()
    {
        if(MaxWantedLevel != 0 || CopsKilledByPlayer != 0 || CiviliansKilledByPlayer != 0 || PlayerHurtPolice || PlayerKilledPolice || PlayerKilledCivilians || PlayerAimedAtPolice || PlayerFiredWeaponNearPolice || MaxPoliceState != Police.PoliceState.Normal)
        {
            Debugging.WriteToLog("WantedLevelStats ClearValues", "Needed to clear values");
            MaxWantedLevel = 0;
            CopsKilledByPlayer = 0;
            CiviliansKilledByPlayer = 0;
            PlayerHurtPolice = false;
            PlayerKilledPolice = false;
            PlayerKilledCivilians = false;
            PlayerAimedAtPolice = false;
            PlayerFiredWeaponNearPolice = false;
            MaxPoliceState = Police.PoliceState.Normal;
        }

    }
}

