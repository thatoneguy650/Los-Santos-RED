using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class EMT : PedExt
{
    private uint GameTimeSpawned;
    private ISettingsProvideable Settings;
    public EMT(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, IEntityProvideable world) : base(pedestrian,settings,crimes,weapons,name, "EMT", world)
    {
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        Settings = settings;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
    }
    public Agency AssignedAgency { get; set; } = new Agency();
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
    public override void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if (Pedestrian.Exists())
        {
            if (Pedestrian.IsAlive)
            {
                if (NeedsFullUpdate)
                {
                    IsInWrithe = Pedestrian.IsInWrithe;
                    UpdatePositionData();
                    PlayerPerception.Update(perceptable, placeLastSeen);
                    if (Settings.SettingsManager.PerformanceSettings.EMSUpdatePerformanceMode1 && !PlayerPerception.RanSightThisUpdate)
                    {
                        GameFiber.Yield();//TR TEST 30
                    }
                    if (Settings.SettingsManager.PerformanceSettings.IsEMSYield1Active)
                    {
                        GameFiber.Yield();//TR TEST 30
                    }
                    UpdateVehicleState();
                    if(Settings.SettingsManager.PerformanceSettings.IsEMSYield2Active)
                    {
                        GameFiber.Yield();
                    }
                    if (Settings.SettingsManager.PerformanceSettings.EMSUpdatePerformanceMode2 && !PlayerPerception.RanSightThisUpdate)
                    {
                        GameFiber.Yield();//TR TEST 30
                    }
                    if (Pedestrian.Exists() && Settings.SettingsManager.EMSSettings.AllowEMTsToCallEMTsOnBodies && !IsUnconscious && !HasSeenDistressedPed && PlayerPerception.DistanceToTarget <= 150f)//only care in a bubble around the player, nothing to do with the player tho
                    {
                        LookForDistressedPeds(world);
                    }
                    GameTimeLastUpdated = Game.GameTime;
                }
            }
            CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok
        }
    }
}