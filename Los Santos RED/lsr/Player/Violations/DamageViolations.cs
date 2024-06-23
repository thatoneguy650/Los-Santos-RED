using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DamageViolations
{
    private IViolateable Player;
    private Violations Violations;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private ICrimes Crimes;
    private IZones Zones;
    private IGangTerritories GangTerritories;

    private uint GameTimeLastHurtCivilian;
    private uint GameTimeLastKilledCivilian; 
    private uint GameTimeLastHurtCop;
    private uint GameTimeLastKilledCop;
    private List<SecurityGuard> PlayerKilledSecurityGuards = new List<SecurityGuard>();
    private List<PedExt> PlayerKilledCivilians = new List<PedExt>();
    private List<Cop> PlayerKilledCops = new List<Cop>();
    public int CountKilledCopsByAgency(string agencyID) => PlayerKilledCops.Where(x => x.AssignedAgency != null && x.AssignedAgency.ID == agencyID).Count();
    public bool NearPoliceMurderVictim => CountNearPoliceMurderVictim > 0;
    public int CountNearPoliceMurderVictim => PlayerKilledCops.Count(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Player.Character) <= Settings.SettingsManager.ViolationSettings.MurderDistance);
    public bool NearCivilianMurderVictim => CountNearCivilianMurderVictim > 0;
    public int CountNearCivilianMurderVictim => PlayerKilledCivilians.Count(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Player.Character) <= Settings.SettingsManager.ViolationSettings.MurderDistance);
    public int CountRecentCivilianMurderVictim => PlayerKilledCivilians.Count(x => x.Pedestrian.Exists() && Game.GameTime - x.GameTimeKilled <= 15000);

    public int CountRecentCivilianMurderVictimWithoutSecurity => PlayerKilledCivilians.Count(x => x.Pedestrian.Exists() && Game.GameTime - x.GameTimeKilled <= 15000) - PlayerKilledSecurityGuards.Count(x => x.Pedestrian.Exists() && Game.GameTime - x.GameTimeKilled <= 15000);



    public bool RecentlyHurtCivilian => GameTimeLastHurtCivilian != 0 && Game.GameTime - GameTimeLastHurtCivilian <= Settings.SettingsManager.ViolationSettings.RecentlyHurtCivilianTime;
    public bool RecentlyKilledCivilian => GameTimeLastKilledCivilian != 0 && Game.GameTime - GameTimeLastKilledCivilian <= Settings.SettingsManager.ViolationSettings.RecentlyKilledCivilianTime;
    public bool RecentlyHurtCop => GameTimeLastHurtCop != 0 && Game.GameTime - GameTimeLastHurtCop <= Settings.SettingsManager.ViolationSettings.RecentlyHurtPoliceTime;
    public bool RecentlyKilledCop => GameTimeLastKilledCop != 0 && Game.GameTime - GameTimeLastKilledCop <= Settings.SettingsManager.ViolationSettings.RecentlyKilledPoliceTime;
    public DamageViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time, ICrimes crimes, IZones zones, IGangTerritories gangTerritories)
    {
        Player = player;
        Violations = violations;
        Settings = settings;
        Time = time;
        Crimes = crimes;
        Zones = zones;
        GangTerritories = gangTerritories;
    }

    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void Reset()
    {
        GameTimeLastHurtCivilian = 0;
        GameTimeLastKilledCivilian = 0;
        GameTimeLastHurtCop = 0;
        GameTimeLastKilledCop = 0;
        PlayerKilledCops.Clear();
        PlayerKilledCivilians.Clear();
        PlayerKilledSecurityGuards.Clear();
    }
    public void AddKilledCivilian()
    {
        GameTimeLastKilledCivilian = Game.GameTime;
        GameTimeLastHurtCivilian = Game.GameTime;


        Violations.AddViolatingAndObserved(StaticStrings.KillingCiviliansCrimeID);

    }
    public void Update()
    {
        if (RecentlyKilledCop || (NearPoliceMurderVictim && (Violations.IsViolatingSeriousCrime || Player.ActivityManager.IsDraggingBody)))
        {
            Violations.AddViolating(StaticStrings.KillingPoliceCrimeID);
        }
        if (RecentlyHurtCivilian)
        {
            Violations.AddViolating(StaticStrings.HurtingCiviliansCrimeID);
        }
        if (RecentlyKilledCivilian || (NearCivilianMurderVictim && !Player.IsInVehicle && (Violations.IsViolatingSeriousCrime || Player.ActivityManager.IsDraggingBody)))
        {
            Violations.AddViolating(StaticStrings.KillingCiviliansCrimeID);
        }
        if ((CountNearCivilianMurderVictim >= 3 && Violations.IsViolatingSeriousCrime) || CountRecentCivilianMurderVictim >= 4)
        {
            Violations.AddViolating(StaticStrings.TerroristActivityCrimeID);
        }

        //CheckVehicleDamage();
    }



    public void AddInjured(PedExt myPed, bool WasShot, bool WasMeleeAttacked, bool WasHitByVehicle)
    {
        if (myPed.IsCop)
        {
            GameTimeLastHurtCop = Game.GameTime;
            Player.AddCrime(Crimes.GetCrime(StaticStrings.HurtingPoliceCrimeID), true, Player.Position, Player.CurrentSeenVehicle, Player.WeaponEquipment.CurrentSeenWeapon, true, true, true);
        }
        else
        {
            myPed.OnInjuredByPlayer(Player, Zones, GangTerritories);
            if (Violations.CanDamageWantedCivilians && myPed.PedViolations.IsViolatingWanted)
            {
                return;
            }
            GameTimeLastHurtCivilian = Game.GameTime; 
        }
    }
    public void AddKilled(PedExt myPed, bool WasShot, bool WasMeleeAttacked, bool WasHitByVehicle)
    {
        myPed.GameTimeKilled = Game.GameTime;
        if (myPed.IsCop && myPed.GetType() == typeof(Cop))
        {
            Cop myCop = (Cop)myPed;
            PlayerKilledCops.Add(myCop);
            GameTimeLastKilledCop = Game.GameTime;
            GameTimeLastHurtCop = Game.GameTime;
            Player.AddCrime(Crimes.GetCrime(StaticStrings.KillingPoliceCrimeID), true, Player.Position, Player.CurrentSeenVehicle, Player.WeaponEquipment.CurrentSeenWeapon, true, true, true);
            Player.OnKilledCop();
        }
        else
        {
            myPed.OnKilledByPlayer(Player, Zones, GangTerritories);    
            Player.OnKilledCivilian();
            if (Violations.CanDamageWantedCivilians && myPed.PedViolations.IsViolatingWanted)
            {
                return;
            }
            PlayerKilledCivilians.Add(myPed);
            if (myPed.GetType() == typeof(SecurityGuard))
            {
                PlayerKilledSecurityGuards.Add((SecurityGuard)myPed);
            }
            GameTimeLastKilledCivilian = Game.GameTime;
            GameTimeLastHurtCivilian = Game.GameTime;       
        }
    }
    public void AddFakeKilled(PedExt myPed)
    {
        myPed.GameTimeKilled = Game.GameTime;
        if (myPed.IsCop && myPed.GetType() == typeof(Cop))
        {
            Cop myCop = (Cop)myPed;
            PlayerKilledCops.Add(myCop);
        }
        else
        {
            if (Violations.CanDamageWantedCivilians && myPed.PedViolations.IsViolatingWanted)
            {
                return;
            }
            PlayerKilledCivilians.Add(myPed);
        }
    }
}

