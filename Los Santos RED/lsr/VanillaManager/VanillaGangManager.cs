using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public class VanillaGangManager
{
    private bool isGangScenarioBlocked = false;
    private bool IsVanillaScenarioGangsActive = true;
    private ISettingsProvideable Settings;
    private List<string> GangScenarios;
    private bool IsVanillaGangPedsSupressed;
    private uint GameTimeLastStoppedGang;

    private IPlacesOfInterest PlacesOfInterest;
    public VanillaGangManager(ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Settings = settings;
        PlacesOfInterest = placesOfInterest;

 
    }
    public void Setup()
    {
        SetGangScenarioBlocking(true);
    }
    public void Dispose()
    {
        if (isGangScenarioBlocked)
        {
            SetGangScenarioBlocking(false);
        }
    }
    public void Tick()
    {

    }
    private void SetGangScenarioBlocking(bool IsBlocked)
    {
        if (IsBlocked)
        {
            foreach(GangDen gangDen in PlacesOfInterest.PossibleLocations.GangDens.Where(x=>x.DisableNearbyScenarios))
            {
                NativeFunction.Natives.ADD_SCENARIO_BLOCKING_AREA<int>(gangDen.EntrancePosition.X - gangDen.DisableScenarioDistance, gangDen.EntrancePosition.Y - gangDen.DisableScenarioDistance, gangDen.EntrancePosition.Z - gangDen.DisableScenarioDistance, gangDen.EntrancePosition.X + gangDen.DisableScenarioDistance, gangDen.EntrancePosition.Y + gangDen.DisableScenarioDistance, gangDen.EntrancePosition.Z + gangDen.DisableScenarioDistance, false, true, true, true);
            }
        }
        else
        {
            NativeFunction.Natives.REMOVE_SCENARIO_BLOCKING_AREAS();
        }
        isGangScenarioBlocked = IsBlocked;
    }
}

