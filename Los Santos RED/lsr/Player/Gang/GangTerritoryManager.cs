using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangTerritoryManager
{
    private IGangTerritoryManageable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private IGangTerritories GangTerritories;
    private IPlacesOfInterest PlacesOfInterest;
    private ITimeReportable Time;
    private IZones Zones;
    private List<Zone> ChangedZones = new List<Zone>();
    public List<GangWar> GangWars { get; set; } = new List<GangWar>();
    public bool IsAtWarWith(Gang gang) => GangWars.Any(x=> !x.IsWarEnded && x.TargetGang != null && x.TargetGang.ID.ToLower() == gang.ID.ToLower());
    public bool IsAtWarWithAnyGang() => GangWars.Any(x => !x.IsWarEnded);
    public GangTerritoryManager(IGangTerritoryManageable player, ISettingsProvideable settings, IEntityProvideable world, IGangTerritories gangTerritories, 
        IPlacesOfInterest placesOfInterest, ITimeReportable time, IZones zones)
    {
        Player = player;
        Settings = settings;
        World = world;
        GangTerritories = gangTerritories;
        PlacesOfInterest = placesOfInterest;
        Time = time;
        Zones = zones;
    }

    public void Dispose()
    {
        GangWars.Clear();
        ChangedZones.Clear();
    }

    public void Setup()
    {
        
    }

    public void Update()
    {
        foreach(GangWar w in GangWars)
        {
            if (Player.RecentlyRespawned)
            {
                EndGangWar(w.TargetGang, false);
            }
            else
            {
                w.Update();
            }
        }

    }
    public void Reset()
    {
        foreach(Zone zone in ChangedZones.ToList())
        {
            SetRestoreZone(zone);
        }
        GangWars.Clear();
        ChangedZones.Clear();
    }
    public bool StartGangWar(Gang gangToBattle, Zone zone)
    {
        if(gangToBattle == null)
        {
            return false;
        }
        GangWar existingWar = GangWars.Where(x => x.TargetGang != null && x.TargetGang.ID.ToLower() == gangToBattle.ID.ToLower()).FirstOrDefault();
        if(existingWar == null)
        {
            existingWar = new GangWar(gangToBattle, new List<Zone>() { zone }, gangToBattle.GangWarCasualtyLimit);
            GangWars.Add(existingWar);
            existingWar.Start();
            Game.DisplayHelp($"Gang War Started with {gangToBattle.ShortName} in {zone.DisplayName}");
        }
        
        return true;
    }
    public bool EndGangWar(Gang gangToBattle, bool IsPlayerVictory)
    {
        if (gangToBattle == null)
        {
            return false;
        }
        GangWar existingWar = GangWars.Where(x => x.TargetGang != null && x.TargetGang.ID.ToLower() == gangToBattle.ID.ToLower()).FirstOrDefault();
        if (existingWar == null)
        {
            return false;
        }
        existingWar.SetOutcome(IsPlayerVictory);
        if(IsPlayerVictory)
        {
            foreach(Zone zone in existingWar.ZonesToAttack)
            {
                SetTookOverZone(zone);
            }
        }
        Game.DisplayHelp($"Gang War ENDED with {gangToBattle.ShortName} in {existingWar.ZonesToAttack.FirstOrDefault()} IsPlayerVictory:{IsPlayerVictory}");
        return true;
    }
    public bool SetTookOverZone(Zone zone)
    {
        if(Player.CurrentGang == null)
        {
            return false;
        }
        if(zone == null)
        {
            return false;
        }
        bool updated = GangTerritories.UpdateTerritory(Player.CurrentGang.ID, zone);
        if(updated)
        {
            zone.UpdateGangItems(GangTerritories);
            List<GangDen> densToUpdate = PlacesOfInterest.PossibleLocations.GangDens.Where(x=> x.ZoneID == zone.InternalGameName).ToList();
            foreach(GangDen dens in densToUpdate)
            {
                dens.SetTakeoverGang(Player.CurrentGang);
            }
            ChangedZones.Add(zone);
        }
        return updated;
    }
    public bool SetRestoreZone(Zone zone)
    {
        if (zone == null)
        {
            return false;
        }
        bool restored = GangTerritories.RestoreTerritory(zone);
        if(restored)
        {
            zone.UpdateGangItems(GangTerritories);
            List<GangDen> densToUpdate = PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.ZoneID == zone.InternalGameName).ToList();
            foreach (GangDen dens in densToUpdate)
            {
                dens.ResetGang();
            }
            ChangedZones.Remove(zone);
        }
        return restored;
    }

    public void AddCasuality(GangMember gangMember)
    {
        if(gangMember == null)
        {
            return;
        }
        Zone deathZone = Zones.GetZone(gangMember.Position);
        GangWar existingWar = GangWars.Where(x => x.TargetGang != null 
        && x.TargetGang.ID.ToLower() == gangMember.Gang.ID.ToLower() 
        && x.ZonesToAttack.Any(y=> y.InternalGameName.ToLower() == deathZone.InternalGameName.ToLower())
        ).FirstOrDefault();
        if (existingWar == null)
        {
            EntryPoint.WriteToConsole($"NO MATCHING GANG WAR FOUND FOR {gangMember.Gang.ShortName} IN {deathZone.DisplayName}");
            return;
        }
        existingWar.AddCasuality();
        EntryPoint.WriteToConsole($"ADDED CASUALTY TO GANG WAR {gangMember.Gang.ShortName} IN {deathZone.DisplayName}");
    }
}

