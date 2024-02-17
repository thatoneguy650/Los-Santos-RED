using LosSantosRED.lsr;
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
    private uint GameTimeLastEjectedBody;
    public List<StoredBody> StoredBodies { get; private set; } = new List<StoredBody>();

    public bool RecentlyEjectedBody => GameTimeLastEjectedBody != 0 && Game.GameTime - GameTimeLastEjectedBody <= 5000;
    public bool IsTrunkOpen { get; private set; } = false;
    public VehicleBodyManager(VehicleExt vehicleExt, ISettingsProvideable settings)
    {
        VehicleExt = vehicleExt;
        Settings = settings;
    }
    public void Update(IVehicleSeatAndDoorLookup vehicleSeatDoorData, IEntityProvideable world)
    {

        StoredBodies.RemoveAll(x => !x.IsValid());

        if(VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }

        for(int seatindex = -1;seatindex <= 5;seatindex++) 
        {
            if (StoredBodies.Any(x => x.VehicleDoorSeatData?.SeatID == seatindex))
            {
                continue;
            }
            Ped pedOnSeat = VehicleExt.Vehicle.GetPedOnSeat(seatindex);
            if(!pedOnSeat.Exists())
            {
                continue;
            }

            PedExt pedExtOnSeat = world.Pedestrians.GetPedExt(pedOnSeat.Handle);
            if(vehicleSeatDoorData == null || vehicleSeatDoorData.VehicleDoorSeatDataList == null)
            {
                continue;
            }
            VehicleDoorSeatData vdsd = vehicleSeatDoorData?.VehicleDoorSeatDataList?.Where(x => x.SeatID == seatindex).FirstOrDefault();
            if(vdsd == null)
            {
                continue;
            }
            if (pedExtOnSeat.IsDead || pedExtOnSeat.IsUnconscious || pedExtOnSeat.IsBusted)
            {
                StoredBody storedBody = new StoredBody(pedExtOnSeat, vdsd, VehicleExt, Settings, world);
                EntryPoint.WriteToConsole("Added Existng Stored Body to the data set");
                StoredBodies.Add(storedBody);
            }
        }

    }
    public void UpdateData()
    {
        IsTrunkOpen = CheckIsTrunkOpen();
        if (IsTrunkOpen)
        {
            EntryPoint.WriteToConsole("TRUNK IS OPEN, SUSPICION IS ON!");
        }
    }
    public bool LoadBody(PedExt pedExt, VehicleDoorSeatData bone, bool withFade, IEntityProvideable world)
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
        StoredBody storedBody = new StoredBody(pedExt, bone, VehicleExt, Settings, world);
        if (storedBody.Load(withFade))
        {
            //EntryPoint.WriteToConsoleTestLong($"VehicleBodyManager LoadBody {bone} FINISHED SUCCESSFULLY");
            if (storedBody.IsAttachedToVehicle)
            {
                storedBody.PedExt.IsLoadedInTrunk = bone.SeatID == -2;
                StoredBodies.Add(storedBody);
            }
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
  
    public void CreateInteractionMenu(MenuPool menuPool, UIMenu VehicleInteractMenu, IVehicleSeatAndDoorLookup vehicleSeatDoorData, IEntityProvideable world)
    {
        Update(vehicleSeatDoorData, world);

        if(VehicleExt.VehicleBodyManager.StoredBodies == null || !VehicleExt.VehicleBodyManager.StoredBodies.Any())
        {
            return;
        }

        UIMenu UnloadBodiesSubMenu = menuPool.AddSubMenu(VehicleInteractMenu, "Unload Peds");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Unload peds from the vehicle.";
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

    public void OnEjectedBody()
    {
        GameTimeLastEjectedBody = Game.GameTime;
    }

    public bool CheckSuspicious()
    {
        if (RecentlyEjectedBody)
        {
            return true;
        }
        if (!StoredBodies.Any())
        {
            return false;
        }
        if (StoredBodies.Any(x => x.VehicleDoorSeatData.SeatID != -2))
        {
            return true;
        }
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (!StoredBodies.Any(x => x.VehicleDoorSeatData.SeatID == -2))
        {
            return false;
        }
        if (IsTrunkOpen)
        {
            return true;
        }
        return false;
    }

    private bool CheckIsTrunkOpen()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (!VehicleExt.Vehicle.Doors[5].IsValid())
        {
            return false;
        }
        if (VehicleExt.Vehicle.Doors[5].AngleRatio >= 0.1f || VehicleExt.Vehicle.Doors[5].IsDamaged)
        {
            return true;
        }
        return false;
    }
}

