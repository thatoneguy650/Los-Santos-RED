using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MarkerManager
{
    private IEntityProvideable World;
    private ILocationInteractable Player;
    private ITimeReportable Time;
    private ISettingsProvideable Settings;
    private List<GameLocation> Locations = new List<GameLocation>();
    private uint GameTimeLastUpdatedLocations;
    public bool ShouldUpdateLocations => GameTimeLastUpdatedLocations == 0 || Game.GameTime - GameTimeLastUpdatedLocations >= 3000;
    public MarkerManager(ILocationInteractable player, IEntityProvideable world, ITimeReportable time, ISettingsProvideable settings)
    { 
        Player = player;
        World = world;
        Time = time;
        Settings = settings;
    }

    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void UpdateMarkers()
    {
        if (!Settings.SettingsManager.WorldSettings.ShowMarkersOnLocationEntrances)
        {
            return;
        }
        foreach (GameLocation bl in Locations)
        {        
            if (bl.EntranceGroundZ == 0.0f)
            {
                float entranceZPosition = bl.EntrancePosition.Z;
                NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(bl.EntrancePosition.X, bl.EntrancePosition.Y, bl.EntrancePosition.Z, out entranceZPosition, false);
                bl.EntranceGroundZ = entranceZPosition;
            }
            NativeFunction.Natives.DRAW_MARKER(1, bl.EntrancePosition.X, bl.EntrancePosition.Y, bl.EntranceGroundZ, 0f, 0f, 0f, 0f, 0f, 0f, 1.0f, 1.0f, 1.0f,
                EntryPoint.LSRedColor.R, EntryPoint.LSRedColor.G, EntryPoint.LSRedColor.B, EntryPoint.LSRedColor.A,
                false, false, 2, true, 0, 0, false);//false, true, 2, true, 0, 0, false);
                                                    // }
        }

    }
    public void UpdateLocations()
    {
        if (!Settings.SettingsManager.WorldSettings.ShowMarkersOnLocationEntrances)
        {
            return;
        }
        Locations.Clear();
        int total = 0;
        foreach (GameLocation bl in World.Places.ActiveLocations.OrderBy(x=> x.DistanceToPlayer).ToList())
        {
            if (!bl.IsActivated || bl.DistanceToPlayer > 30f || bl.IsTemporarilyClosed || !bl.ShowsMarker || !bl.IsOpen(Time.CurrentHour))
            {
                continue;
            }
            Locations.Add(bl);
            total++;
            if(total >= Settings.SettingsManager.WorldSettings.ShowMarkersOnLocationEntrancesMaxNumber)
            {
                break;
            }
        }
        GameTimeLastUpdatedLocations = Game.GameTime;
    }
    public void Update()
    {
        if (!Settings.SettingsManager.WorldSettings.ShowMarkersOnLocationEntrances)
        {
            return;
        }
        foreach (GameLocation bl in World.Places.ActiveLocations)
        {
            if(!bl.IsActivated || bl.DistanceToPlayer > 30f || bl.IsTemporarilyClosed || !bl.ShowsMarker || !bl.IsOpen(Time.CurrentHour))
            {
                continue;
            }
            //if(bl.HasVendor)//bl.Vendor != null && bl.Vendor.Pedestrian.Exists())
            //{
            //    //Vector3 PedPos = bl.Vendor.Pedestrian.Position;
            //    //NativeFunction.Natives.DRAW_MARKER(2, PedPos.X, PedPos.Y, PedPos.Z + (bl.Vendor.Pedestrian.Model.Dimensions.Z / 2.0f) + 0.35f, 0f, 0f, 0f, 0f, 180f, 0f, 1.0f, 1.0f, 1.0f, 
            //    //    EntryPoint.LSRedColor.R, EntryPoint.LSRedColor.G, EntryPoint.LSRedColor.B, EntryPoint.LSRedColor.A, 
            //    //    true, true, 2, true, 0, 0, false);
            //}
            //else
            //{  
                if (bl.EntranceGroundZ == 0.0f)
                {
                    float entranceZPosition = bl.EntrancePosition.Z;
                    NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(bl.EntrancePosition.X, bl.EntrancePosition.Y, bl.EntrancePosition.Z, out entranceZPosition, false);
                    bl.EntranceGroundZ = entranceZPosition;
                }
                NativeFunction.Natives.DRAW_MARKER(1, bl.EntrancePosition.X, bl.EntrancePosition.Y, bl.EntranceGroundZ, 0f, 0f, 0f, 0f, 0f, 0f, 1.0f, 1.0f, 1.0f, 
                    EntryPoint.LSRedColor.R, EntryPoint.LSRedColor.G, EntryPoint.LSRedColor.B, EntryPoint.LSRedColor.A,
                    false, false, 2, true, 0, 0, false);//false, true, 2, true, 0, 0, false);
           // }
        }  
    }
}

