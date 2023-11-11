using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SonarBlip
{
    private VehicleExt VehicleExt;
    private ISettingsProvideable Settings;
    private uint GameTimeLastAddedSonarBlip;
    private bool isDisposed = false;
    public Blip Blip { get; private set; }
    public uint GameTimeBetweenBlips => Settings.SettingsManager.ScannerSettings.PoliceBlipUpdateTime;
    public float PercentElapsed => ((float)(Game.GameTime - GameTimeLastAddedSonarBlip) / (float)GameTimeBetweenBlips);

    public SonarBlip(VehicleExt vehicleExt, ISettingsProvideable settings)
    {
        VehicleExt = vehicleExt;
        Settings = settings;
    }
    public void Update(IEntityProvideable world)
    {
        UpdateBlip(world); 
    }
    public void Dispose()
    {
        isDisposed = true;
        RemoveExistingBlip();
    }
    private void UpdateBlip(IEntityProvideable world)
    {
        if (isDisposed || VehicleExt.AttachedBlip.Exists() || VehicleExt == null || !VehicleExt.Vehicle.Exists() || !VehicleExt.HasSonarBlip)
        {
            return;
        }
        if (Game.GameTime - GameTimeLastAddedSonarBlip >= GameTimeBetweenBlips)
        {
            AddNewSonarBlip(world);
        }
        else
        {
            UpdateAlpha();
        }
    }
    private void RemoveExistingBlip()
    {
        if (Blip.Exists())
        {
            Blip.Delete();
        }
    }
    public void AddNewSonarBlip(IEntityProvideable world)
    {   
        if (VehicleExt.AttachedBlip.Exists() || VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }
        RemoveExistingBlip();
        Blip = new Blip(VehicleExt.Vehicle.Position) { Name = "Police" };
        EntryPoint.WriteToConsole($"SONAR BLIP CREATED");
        if (!Blip.Exists())
        {
            return;
        }
        //Blip.Sprite = (BlipSprite)VehicleExt.PoliceBlipID;// 672;
        //Blip.Angle = (int)VehicleExt.Vehicle.Heading;

        Blip.Scale = VehicleExt.BlipSize;
        Blip.Color = VehicleExt.BlipColor;
        Blip.Alpha = 255;

        world.AddBlip(Blip);
        GameTimeLastAddedSonarBlip = Game.GameTime;
    }
    private void UpdateAlpha()
    {
        if (VehicleExt == null || VehicleExt.AttachedBlip.Exists() || !VehicleExt.Vehicle.Exists() || !Blip.Exists())
        {
            return;
        }
        Blip.Alpha = 1.0f - PercentElapsed;
    }
    //private void UpdateSonarBlip






}
