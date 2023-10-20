using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VanillaCopManager
{

    private bool IsVanillaDispatchActive = true;
    private bool IsVanillaScenarioCopsActive = true;

    private uint GameTimeLastTerminatedVanillaDispatch;
    private uint GameTimeLastTerminatedScenarioCops;
    private ISettingsProvideable Settings;
    private bool IsTimeToTerminatedVanillaDispatch => GameTimeLastTerminatedVanillaDispatch == 0 || Game.GameTime - GameTimeLastTerminatedVanillaDispatch >= 5000;
    private bool IsTimeToTerminateScenarioCops => GameTimeLastTerminatedScenarioCops == 0 || Game.GameTime - GameTimeLastTerminatedScenarioCops >= 5000;
    public VanillaCopManager(ISettingsProvideable settings)
    {
        Settings = settings;
    }

    public void Setup()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (EntryPoint.ModController.IsRunning)
                {
                    if (Settings.SettingsManager.VanillaSettings.SupressRandomPoliceEvents)
                    {
                        SuppressRandomEvents();
                    }
                    if(Settings.SettingsManager.VanillaSettings.SupressVanillaCopCrimes)
                    {
                        SupressCopCrimes();
                    }
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error: " + e.Message + " : " + e.StackTrace, 0);
            }
        }, $"Supress Logic Ran");
    }

    private void SupressCopCrimes()
    {
        List<int> crimes = new List<int>() { 12,14,16,24,27,32,33,34,41, 40,13, };
        foreach(int crimeId in crimes)
        {
            NativeFunction.Natives.SUPPRESS_CRIME_THIS_FRAME(Game.LocalPlayer, crimeId);
        }
    }
    public enum CRIME_TYPE
    {
        CRIME_NONE = 0,
        CRIME_POSSESSION_GUN,
        CRIME_RUN_REDLIGHT,
        CRIME_RECKLESS_DRIVING,
        CRIME_SPEEDING,
        CRIME_DRIVE_AGAINST_TRAFFIC,
        CRIME_RIDING_BIKE_WITHOUT_HELMET,
        CRIME_LAST_MINOR_CRIME = CRIME_RIDING_BIKE_WITHOUT_HELMET,      // Minor crimes will not automatically make the wanted level go up but it will start a parole instead. If the player does anything wrong doing the parole period he will get a wanted level.

        CRIME_STEAL_VEHICLE,
        CRIME_STEAL_CAR,
        CRIME_LAST_ONE_NO_REFOCUS = CRIME_STEAL_CAR,                    // Some crimes will not refocus the search area.

        CRIME_BLOCK_POLICE_CAR,
        CRIME_STAND_ON_POLICE_CAR,
        CRIME_HIT_PED,
        CRIME_HIT_COP,
        CRIME_SHOOT_PED,
        CRIME_SHOOT_COP,
        CRIME_RUNOVER_PED,
        CRIME_RUNOVER_COP,
        CRIME_DESTROY_HELI,
        CRIME_PED_SET_ON_FIRE,
        CRIME_COP_SET_ON_FIRE,
        CRIME_CAR_SET_ON_FIRE,
        CRIME_DESTROY_PLANE,
        CRIME_CAUSE_EXPLOSION,
        CRIME_STAB_PED,
        CRIME_STAB_COP,
        CRIME_DESTROY_VEHICLE,
        CRIME_DAMAGE_TO_PROPERTY,
        CRIME_TARGET_COP,
        CRIME_FIREARM_DISCHARGE,
        CRIME_RESIST_ARREST,
        CRIME_MOLOTOV,
        CRIME_SHOOT_NONLETHAL_PED,
        CRIME_SHOOT_NONLETHAL_COP,
        CRIME_KILL_COP,
        CRIME_SHOOT_AT_COP,
        CRIME_SHOOT_VEHICLE,
        CRIME_TERRORIST_ACTIVITY,
        CRIME_HASSLE,
        CRIME_THROW_GRENADE,
        CRIME_VEHICLE_EXPLOSION,
        CRIME_KILL_PED,
        CRIME_STEALTH_KILL_COP,
        CRIME_SUICIDE,
        CRIME_DISTURBANCE,
        CRIME_CIVILIAN_NEEDS_ASSISTANCE,
        CRIME_STEALTH_KILL_PED,
        CRIME_SHOOT_PED_SUPPRESSED,
        CRIME_JACK_DEAD_PED,
        CRIME_CHAIN_EXPLOSION,
        MAX_CRIMES
    }
    public void Dispose()
    {
        ActivateDispatch();
        ActivateScenarioCops();
    }
    public void Tick()
    {
        if (Settings.SettingsManager.VanillaSettings.TerminateDispatch)
        {
            if (IsVanillaDispatchActive || IsTimeToTerminatedVanillaDispatch)
            {
                TerminateDispatch();
            }
        }
        else if (!Settings.SettingsManager.VanillaSettings.TerminateDispatch)
        {
            if (!IsVanillaDispatchActive)
            {
                ActivateDispatch();
            }
        }
        if (Settings.SettingsManager.VanillaSettings.TerminateScenarioCops)
        {
            if (IsVanillaScenarioCopsActive || IsTimeToTerminateScenarioCops)
            {
                TerminateScenarioCops();
             }
        }
        else if (!Settings.SettingsManager.VanillaSettings.TerminateScenarioCops)
        {
            if (!IsVanillaScenarioCopsActive)
            {
                ActivateScenarioCops();
            }
        }
    }
    private void SuppressRandomEvents()
    {
        NativeFunction.Natives.SUPRESS_RANDOM_EVENT_THIS_FRAME((int)eRandomEventType.CopChase, true);
        NativeFunction.Natives.SUPRESS_RANDOM_EVENT_THIS_FRAME((int)eRandomEventType.CopChaseFlee, true);
        NativeFunction.Natives.SUPRESS_RANDOM_EVENT_THIS_FRAME((int)eRandomEventType.CopFast, true);
    }
    private void TerminateDispatch()
    {
        SetDispatch(false);
        IsVanillaDispatchActive = false;
        GameTimeLastTerminatedVanillaDispatch = Game.GameTime;
    }
    private void TerminateScenarioCops()
    {
        IsVanillaScenarioCopsActive = false;
        SetScenarioCops(false);
        GameTimeLastTerminatedScenarioCops = Game.GameTime;
    }
    private void ActivateDispatch()
    {
        SetDispatch(true);
        IsVanillaDispatchActive = true;
    }
    private void ActivateScenarioCops()
    {
        IsVanillaScenarioCopsActive = true;
        SetScenarioCops(true);
    }
    private void SetDispatch(bool Enabled)
    {
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobile, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceHelicopter, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceVehicleRequest, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.SwatAutomobile, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.SwatHelicopter, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceRiders, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceRoadBlock, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobileWaitCruising, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobileWaitPulledOver, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.AmbulanceDepartment, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.FireDepartment, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.Gangs, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.BikerBackup, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceBoat, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.ArmyVehicle, Enabled);
        NativeFunction.Natives.SET_DISPATCH_COPS_FOR_PLAYER(Enabled);
    }
    private void SetScenarioCops(bool Enabled)
    {
        NativeFunction.Natives.SET_CREATE_RANDOM_COPS(Enabled);
        NativeFunction.Natives.SET_CREATE_RANDOM_COPS_ON_SCENARIOS(Enabled);
        NativeFunction.Natives.SET_CREATE_RANDOM_COPS_NOT_ON_SCENARIOS(Enabled);
        NativeFunction.Natives.SET_DISPATCH_COPS_FOR_PLAYER(Enabled);
    }
}

