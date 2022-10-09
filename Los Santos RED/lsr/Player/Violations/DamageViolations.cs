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

    private List<PedExt> PlayerKilledCivilians = new List<PedExt>();
    private List<PedExt> PlayerKilledCops = new List<PedExt>();
    public bool NearCivilianMurderVictim => PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Player.Character) <= Settings.SettingsManager.ViolationSettings.MurderDistance);
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
    }
    public void Update()
    {
        if (RecentlyKilledCivilian || NearCivilianMurderVictim)
        {
            Violations.AddViolating("KillingCivilians");
        }

        if (RecentlyHurtCivilian)
        {
            Violations.AddViolating("HurtingCivilians");
        }
    }

    public void AddInjured(PedExt myPed, bool WasShot, bool WasMeleeAttacked, bool WasHitByVehicle)
    {
        if (myPed.IsCop)
        {
            GameTimeLastHurtCop = Game.GameTime;
            Player.AddCrime(Crimes.GetCrime("HurtingPolice"), true, Player.Position, Player.CurrentSeenVehicle, Player.WeaponEquipment.CurrentSeenWeapon, true, true, true);
            EntryPoint.WriteToConsole($"VIOLATIONS: Hurting Police Added WasShot {WasShot} WasMeleeAttacked {WasMeleeAttacked} WasHitByVehicle {WasHitByVehicle}", 5);
        }
        else
        {
            if (myPed.GetType() == typeof(GangMember))
            {
                GangMember gm = (GangMember)myPed;
                AddAttackedGang(gm, false);
            }
            GameTimeLastHurtCivilian = Game.GameTime;
        }
        EntryPoint.WriteToConsole($"VIOLATIONS: Hurting WasShot {WasShot} WasMeleeAttacked {WasMeleeAttacked} WasHitByVehicle {WasHitByVehicle}", 5);
    }
    public void AddKilled(PedExt myPed, bool WasShot, bool WasMeleeAttacked, bool WasHitByVehicle)
    {
        if (myPed.IsCop)
        {
            PlayerKilledCops.Add(myPed);
            GameTimeLastKilledCop = Game.GameTime;
            GameTimeLastHurtCop = Game.GameTime;
            Player.AddCrime(Crimes.GetCrime("KillingPolice"), true, Player.Position, Player.CurrentSeenVehicle, Player.WeaponEquipment.CurrentSeenWeapon, true, true, true);
            Player.OnKilledCop();
            EntryPoint.WriteToConsole($"VIOLATIONS: Killing Police Added WasShot {WasShot} WasMeleeAttacked {WasMeleeAttacked} WasHitByVehicle {WasHitByVehicle}", 5);
        }
        else
        {
            if (myPed.IsGangMember)
            {
                if (myPed.GetType() == typeof(GangMember))
                {
                    GangMember gm = (GangMember)myPed;
                    AddAttackedGang(gm, true);
                }
            }
            PlayerKilledCivilians.Add(myPed);
            Player.OnKilledCivilian();
            GameTimeLastKilledCivilian = Game.GameTime;
            GameTimeLastHurtCivilian = Game.GameTime;
        }
        EntryPoint.WriteToConsole($"VIOLATIONS: Killing WasShot {WasShot} WasMeleeAttacked {WasMeleeAttacked} WasHitByVehicle {WasHitByVehicle}", 5);
    }

    private void AddAttackedGang(GangMember gm, bool isKilled)
    {
        int RepToRemove = -500;
        if (isKilled)
        {
            RepToRemove = -1000;
        }
        GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(gm.Gang);//.MembersKilled++;
        if (gr != null)
        {
            if (isKilled)
            {
                gr.MembersKilled++;
                EntryPoint.WriteToConsole($"VIOLATIONS: Killing GangMemeber {gm.Gang.ShortName} {gr.MembersKilled}", 5);
            }
            else
            {
                gr.MembersHurt++;
                EntryPoint.WriteToConsole($"VIOLATIONS: Hurting GangMemeber {gm.Gang.ShortName} {gr.MembersHurt}", 5);
            }
            if (gm.Pedestrian.Exists())
            {
                Zone KillingZone = Zones.GetZone(gm.Pedestrian.Position);
                if (KillingZone != null)
                {
                    EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone {KillingZone.InternalGameName}", 5);
                    List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(gm.Gang.ID);
                    if (totalTerritories.Any(x => x.ZoneInternalGameName.ToLower() == KillingZone.InternalGameName.ToLower()))
                    {
                        EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone {KillingZone.InternalGameName} IS GANG TERRITORY!", 5);
                        if (isKilled)
                        {
                            RepToRemove -= 4000;// 1000;
                            gr.MembersKilledInTerritory++;
                            EntryPoint.WriteToConsole($"VIOLATIONS: Killing GangMemeber {gm.Gang.ShortName} On Own Turf {gr.MembersKilledInTerritory}", 5);
                        }
                        else
                        {
                            RepToRemove -= 2500;// 500;
                            gr.MembersHurtInTerritory++;
                            EntryPoint.WriteToConsole($"VIOLATIONS: Hurting GangMemeber {gm.Gang.ShortName} On Own Turf {gr.MembersHurtInTerritory}", 5);
                        }

                    }
                }
                else
                {
                    EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone fail", 5);
                }
            }
        }
        Player.RelationshipManager.GangRelationships.ChangeReputation(gm.Gang, RepToRemove, true);
        Player.RelationshipManager.GangRelationships.AddAttacked(gm.Gang);
    }

}

