using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleBodyManager
{
    private VehicleExt VehicleExt;
    private ISettingsProvideable Settings;
    public List<StoredBody> StoredBodies { get; private set; } = new List<StoredBody>();

    public VehicleBodyManager(VehicleExt vehicleExt, ISettingsProvideable settings)
    {
        VehicleExt = vehicleExt;
        Settings = settings;
    }
    public void Update()
    {
        
    }
    public void LoadBody(PedExt pedExt, string bone)
    {
        EntryPoint.WriteToConsole($"VehicleBodyManager LoadBody {bone}");
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (pedExt == null || !pedExt.Pedestrian.Exists())
        {
            return;
        }
        StoredBodies.RemoveAll(x => x.PedExt == null || !x.PedExt.Pedestrian.Exists());
        if (StoredBodies.Any(x => x.StoredBone == bone))
        {
            Game.DisplayHelp($"{bone} is Full");
            return;
        }
        StoredBody storedBody = new StoredBody(pedExt, bone, VehicleExt, Settings);
        if (storedBody.Load())
        {
            EntryPoint.WriteToConsole($"VehicleBodyManager LoadBody {bone} FINISHED SUCCESSFULLY");
            StoredBodies.Add(storedBody);
        }
    }
  
    public void UnloadBody(PedExt pedExt)
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (pedExt == null || !pedExt.Pedestrian.Exists())
        {
            return;
        }
        StoredBodies.RemoveAll(x => x.PedExt == null || !x.PedExt.Pedestrian.Exists());
        StoredBody storedBody = StoredBodies.FirstOrDefault(x=> x.PedExt.Handle == pedExt.Handle);
        if(storedBody == null)
        {
            return;
        }
        if(storedBody.Unload())
        {
            StoredBodies.Remove(storedBody);
        }
    }



}

