using LosSantosRED.lsr.Interface;
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
    public void Update()
    {
        if (Settings.SettingsManager.WorldSettings.ShowMarkersOnLocationEntrances)
        {
            foreach (InteractableLocation bl in World.Places.ActiveInteractableLocations)
            {
                if (bl.IsActivated && bl.DistanceToPlayer <= 100f && bl.IsOpen(Time.CurrentHour))
                {
                    float entranceZPosition = bl.EntrancePosition.Z;
                    NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(bl.EntrancePosition.X, bl.EntrancePosition.Y, bl.EntrancePosition.Z, out entranceZPosition, false);
                    NativeFunction.Natives.DRAW_MARKER(1, bl.EntrancePosition.X, bl.EntrancePosition.Y, entranceZPosition, 0f, 0f, 0f, 0f, 0f, 0f, 1.0f, 1.0f, 1.0f, EntryPoint.LSRedColor.R, EntryPoint.LSRedColor.G, EntryPoint.LSRedColor.B, EntryPoint.LSRedColor.A, false, true, 2, true, 0, 0, false);
                }
            }
        }
    }
}

