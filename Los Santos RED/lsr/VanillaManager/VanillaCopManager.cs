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
    private ISettingsProvideable Settings;
    private List<string> CopScenarios;
    private bool IsTimeToTerminatedVanillaDispatch => GameTimeLastTerminatedVanillaDispatch == 0 || Game.GameTime - GameTimeLastTerminatedVanillaDispatch >= 5000;
    public VanillaCopManager(ISettingsProvideable settings)
    {
        Settings = settings;
        CopScenarios = new List<string>
        {
            "WORLD_VEHICLE_POLICE_BIKE",
            "WORLD_VEHICLE_POLICE_CAR",
            "WORLD_VEHICLE_POLICE_NEXT_TO_CAR",
            "CODE_HUMAN_POLICE_CROWD_CONTROL",
            "CODE_HUMAN_POLICE_INVESTIGATE",
        };
    }

    public void Setup()
    {

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
            //if (IsVanillaDispatchActive || IsTimeToTerminatedVanillaDispatch)
            //{
            TerminateDispatch();
            //}
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
            //if (IsVanillaScenarioCopsActive)
            //{
            TerminateScenarioCops();
            // }
        }
        else if (!Settings.SettingsManager.VanillaSettings.TerminateScenarioCops)
        {
            if (!IsVanillaScenarioCopsActive)
            {
                ActivateScenarioCops();
            }
        }
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
        foreach (string scenario in CopScenarios)
        {
            NativeFunction.Natives.SET_SCENARIO_TYPE_ENABLED(scenario, Enabled);
        }

    }
}

