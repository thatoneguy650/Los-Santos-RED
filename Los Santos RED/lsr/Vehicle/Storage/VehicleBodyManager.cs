using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
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
    public bool LoadBody(PedExt pedExt, VehicleDoorSeatData bone)
    {
        //EntryPoint.WriteToConsoleTestLong($"VehicleBodyManager LoadBody {bone}");
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (pedExt == null || !pedExt.Pedestrian.Exists() || bone == null)
        {
            return false;
        }
        StoredBodies.RemoveAll(x => x.PedExt == null || !x.PedExt.Pedestrian.Exists());
        if (StoredBodies.Any(x => x.VehicleDoorSeatData?.SeatBone == bone.SeatBone))
        {
            Game.DisplayHelp($"{bone} is Full");
            return false;
        }
        StoredBody storedBody = new StoredBody(pedExt, bone, VehicleExt, Settings);
        if (storedBody.Load())
        {
            //EntryPoint.WriteToConsoleTestLong($"VehicleBodyManager LoadBody {bone} FINISHED SUCCESSFULLY");
            StoredBodies.Add(storedBody);
            return true;
        }
        return false;
    }
    public void OnVehicleCrashed()
    {
        foreach (StoredBody storedBody in StoredBodies)
        {
            storedBody.OnVehicleCrashed();
        }
        StoredBodies.RemoveAll(x => x.WasEjected);
    }
  
    //public void UnloadBody(PedExt pedExt)
    //{
    //    if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
    //    {
    //        return;
    //    }
    //    if (pedExt == null || !pedExt.Pedestrian.Exists())
    //    {
    //        return;
    //    }
    //    StoredBodies.RemoveAll(x => x.PedExt == null || !x.PedExt.Pedestrian.Exists());
    //    StoredBody storedBody = StoredBodies.FirstOrDefault(x=> x.PedExt.Handle == pedExt.Handle);
    //    if(storedBody == null)
    //    {
    //        return;
    //    }
    //    if(storedBody.Unload())
    //    {
    //        StoredBodies.Remove(storedBody);
    //    }
    //}

    public void CreateInteractionMenu(MenuPool menuPool, UIMenu VehicleInteractMenu)
    {
        if(VehicleExt.VehicleBodyManager.StoredBodies == null || !VehicleExt.VehicleBodyManager.StoredBodies.Any())
        {
            return;
        }
        UIMenu UnloadBodiesSubMenu = menuPool.AddSubMenu(VehicleInteractMenu, "Unload Bodies");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Unload bodies from the vehicle.";
        UnloadBodiesSubMenu.SetBannerType(EntryPoint.LSRedColor);

        foreach (StoredBody storedBody in VehicleExt.VehicleBodyManager.StoredBodies)
        {
            if (storedBody.PedExt == null || !storedBody.PedExt.Pedestrian.Exists())
            {
                continue;
            }
            UIMenuItem unloadBody = new UIMenuItem($"Unload {storedBody.PedExt.Name}", $"Unload {storedBody.PedExt.Name} from {storedBody.VehicleDoorSeatData?.SeatName}");
            unloadBody.Activated += (menu, item) =>
            {
                VehicleExt.VehicleBodyManager.StoredBodies.Remove(storedBody);
                storedBody.Unload();
                UnloadBodiesSubMenu.Visible = false;
            };
            UnloadBodiesSubMenu.AddItem(unloadBody);
        }
    }
}

