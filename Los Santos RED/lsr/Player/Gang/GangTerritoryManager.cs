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
    public List<GangRetaliation> Retaliations { get; set; } = new List<GangRetaliation>();
    public bool IsAtWarWith(Gang gang) => GangWars.Any(x=> !x.IsWarEnded && x.TargetGang != null && x.TargetGang.ID.ToLower() == gang.ID.ToLower());
    public bool IsDoingRetaliation(Gang gang) => Retaliations.Any(x => !x.IsEnded && x.TargetGang != null && x.TargetGang.ID.ToLower() == gang.ID.ToLower());
    public bool IsAtWarWithAnyGang() => GangWars.Any(x => !x.IsWarEnded);
    public bool IsAnyGangRetaliating() => Retaliations.Any(x => !x.IsEnded);
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
        foreach(GangWar w in GangWars.ToList())
        {
            if (Player.RecentlyRespawned)
            {
                EndGangWar(w.TargetGang, false);
            }
            else
            {
                w.Update(Player);
            }
        }
        foreach(GangRetaliation ended in Retaliations.ToList())
        {
            ended.Update();
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
        Retaliations.Clear();
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
            int CasualityLimit = RandomItems.GetRandomNumberInt(gangToBattle.TakeoverTerritoryCasualtyLimitMin, gangToBattle.TakeoverTerritoryCasualtyLimitMax);


            existingWar = new GangWar(Player,gangToBattle, new List<Zone>() { zone }, CasualityLimit, this);// gangToBattle.GangWarCasualtyLimit);
            GangWars.Add(existingWar);
            existingWar.Start();
            EntryPoint.WriteToConsole($"Gang War Started with {gangToBattle.ShortName} in {zone.DisplayName}");
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
        //existingWar.SetOutcome(IsPlayerVictory);
        if(IsPlayerVictory)
        {
            foreach(Zone zone in existingWar.ZonesToAttack)
            {
                SetTookOverZone(zone);
            }
            GangRetaliation gr = new GangRetaliation(Player, this, Game.GameTime, existingWar.TargetGang, existingWar.ZonesToAttack, Settings);
            gr.Setup();
            Retaliations.Add(gr);

        }
        EntryPoint.WriteToConsole($"Gang War ENDED with {gangToBattle.ShortName} in {existingWar.ZonesToAttack.FirstOrDefault().DisplayName} IsPlayerVictory:{IsPlayerVictory}");

        GangWars.Remove(existingWar);
        return true;
    }
    public void EndRetaliation(GangRetaliation gr, bool isPlayerWinner)
    {
        if (!isPlayerWinner)
        {
            SetRestoreZone(gr.ZonesToAttack.FirstOrDefault());
        }
        Retaliations.Remove(gr);
        EntryPoint.WriteToConsole("END RETALIATION RAN REMOVING ");
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
        EntryPoint.WriteToConsole($"ADDED CASUALTY TO GANG WAR {gangMember.Gang.ShortName} IN {deathZone.DisplayName} Casualites:{existingWar.Casualites} CasualityLimit:{existingWar.CasualityLimit} GangWarCasualtyLimit:{gangMember.Gang.GangWarCasualtyLimit}");
    }




    public void LoadWar(Gang targetGang, List<Zone> zonesToAttack, int casualityLimit)
    {
        GangWar existingWar = new GangWar(Player, targetGang, zonesToAttack, casualityLimit, this);// gangToBattle.GangWarCasualtyLimit);
        GangWars.Add(existingWar);
    }

    public void LoadRetaliation(Gang targetGang, List<Zone> zonesToAttack)
    {
        GangRetaliation gr = new GangRetaliation(Player, this, Game.GameTime, targetGang, zonesToAttack, Settings);
        gr.Setup();  
        Retaliations.Add(gr);
    }
}

