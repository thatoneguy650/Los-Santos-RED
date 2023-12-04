using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugTrunkSubMenu : DebugSubMenu
{
    private ModDataFileManager ModDataFileManager;
    private IEntityProvideable World;
    private UIMenuItem MainMenuItem;
    private PedExt LastSpawnedPed;

    public DebugTrunkSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ModDataFileManager modDataFileManager, IEntityProvideable world) : base(debug, menuPool, player)
    {
        ModDataFileManager = modDataFileManager;
        World = world;
    }

    public override void AddItems()
    {
        SubMenu = MenuPool.AddSubMenu(Debug, "Trunk Menu");
        MainMenuItem = Debug.MenuItems[Debug.MenuItems.Count() - 1];


        MainMenuItem.Description = "Change trunk attach items.";
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        UpdateMenu();
    }
    public override void Update()
    {
        UpdateMenu();
    }
    private void UpdateMenu()
    {
        SubMenu.Clear();


        if (Player.InterestedVehicle == null || !Player.InterestedVehicle.Vehicle.Exists())
        {
            //Game.DisplaySubtitle("No Vehicle");
            return;
        }
        VehicleItem vehicleItem = ModDataFileManager.ModItems.GetVehicle(Player.InterestedVehicle.Vehicle.Model.Hash);


        if (vehicleItem != null)
        {
            SubMenu.SubtitleText = vehicleItem.Name;
            UIMenuCheckboxItem overrideLoadInBedMenu = new UIMenuCheckboxItem("OverrideLoadBodiesInBed",vehicleItem.OverrideLoadBodiesInBed) { Checked = vehicleItem.OverrideLoadBodiesInBed };
            overrideLoadInBedMenu.Activated += (sender, e) =>
            {
                if (vehicleItem == null)
                {
                    return;
                }

                vehicleItem.OverrideLoadBodiesInBed = !vehicleItem.OverrideLoadBodiesInBed;
                overrideLoadInBedMenu.Checked = vehicleItem.OverrideLoadBodiesInBed;
                Game.DisplaySubtitle($"OverrideLoadBodiesInBed: {vehicleItem.OverrideLoadBodiesInBed}");

            };
            SubMenu.AddItem(overrideLoadInBedMenu);


            UIMenuCheckboxItem overrideTrunkAttachMenu = new UIMenuCheckboxItem("OverrideTrunkAttachment", vehicleItem.OverrideTrunkAttachment) { Checked = vehicleItem.OverrideTrunkAttachment };
            overrideTrunkAttachMenu.Activated += (sender, e) =>
            {
                if(vehicleItem == null)
                {
                    return;
                }
                vehicleItem.OverrideTrunkAttachment = !vehicleItem.OverrideTrunkAttachment;
                overrideTrunkAttachMenu.Checked = vehicleItem.OverrideTrunkAttachment;
                Game.DisplaySubtitle($"OverrideTrunkAttachment: {vehicleItem.OverrideTrunkAttachment}");
            };
            SubMenu.AddItem(overrideTrunkAttachMenu);


            //Trunk
            UIMenuItem TrunkAttachOffsetOverrideMenuItemX = new UIMenuItem("TrunkAttachOffsetOverride X") { RightLabel = vehicleItem.TrunkAttachOffsetOverride.X.ToString() };
            TrunkAttachOffsetOverrideMenuItemX.Activated += (sender, e) =>
            {
                if (vehicleItem == null)
                {
                    return;
                }
                if (!float.TryParse(NativeHelper.GetKeyboardInput($""), out float newAttachID))
                {
                    return;
                }
                vehicleItem.TrunkAttachOffsetOverride = new Vector3(newAttachID, vehicleItem.TrunkAttachOffsetOverride.Y, vehicleItem.TrunkAttachOffsetOverride.Z);
                TrunkAttachOffsetOverrideMenuItemX.RightLabel = newAttachID.ToString();
            };
            SubMenu.AddItem(TrunkAttachOffsetOverrideMenuItemX);

            UIMenuItem TrunkAttachOffsetOverrideMenuItemY = new UIMenuItem("TrunkAttachOffsetOverride Y") { RightLabel = vehicleItem.TrunkAttachOffsetOverride.Y.ToString() };
            TrunkAttachOffsetOverrideMenuItemY.Activated += (sender, e) =>
            {
                if (vehicleItem == null)
                {
                    return;
                }
                if (!float.TryParse(NativeHelper.GetKeyboardInput($""), out float newAttachID))
                {
                    return;
                }
                vehicleItem.TrunkAttachOffsetOverride = new Vector3(vehicleItem.TrunkAttachOffsetOverride.X, newAttachID, vehicleItem.TrunkAttachOffsetOverride.Z);
                TrunkAttachOffsetOverrideMenuItemY.RightLabel = newAttachID.ToString();
            };
            SubMenu.AddItem(TrunkAttachOffsetOverrideMenuItemY);

            UIMenuItem TrunkAttachOffsetOverrideMenuItemZ = new UIMenuItem("TrunkAttachOffsetOverride Z") { RightLabel = vehicleItem.TrunkAttachOffsetOverride.Z.ToString() };
            TrunkAttachOffsetOverrideMenuItemZ.Activated += (sender, e) =>
            {
                if (vehicleItem == null)
                {
                    return;
                }
                if (!float.TryParse(NativeHelper.GetKeyboardInput($""), out float newAttachID))
                {
                    return;
                }
                vehicleItem.TrunkAttachOffsetOverride = new Vector3(vehicleItem.TrunkAttachOffsetOverride.X, vehicleItem.TrunkAttachOffsetOverride.Y, newAttachID);
                TrunkAttachOffsetOverrideMenuItemZ.RightLabel = newAttachID.ToString();
            };
            SubMenu.AddItem(TrunkAttachOffsetOverrideMenuItemZ);

            //Bed
            UIMenuItem BedLoadOffsetOverrideMenuItemX = new UIMenuItem("BedLoadOffsetOverride X") { RightLabel = vehicleItem.BedLoadOffsetOverride.X.ToString() };
            BedLoadOffsetOverrideMenuItemX.Activated += (sender, e) =>
            {
                if (vehicleItem == null)
                {
                    return;
                }
                if (!float.TryParse(NativeHelper.GetKeyboardInput($""), out float newAttachID))
                {
                    return;
                }
                vehicleItem.BedLoadOffsetOverride = new Vector3(newAttachID, vehicleItem.BedLoadOffsetOverride.Y, vehicleItem.BedLoadOffsetOverride.Z);
                BedLoadOffsetOverrideMenuItemX.RightLabel = newAttachID.ToString();
            };
            SubMenu.AddItem(BedLoadOffsetOverrideMenuItemX);

            UIMenuItem BedLoadOffsetOverrideMenuItemY = new UIMenuItem("BedLoadOffsetOverride Y") { RightLabel = vehicleItem.BedLoadOffsetOverride.Y.ToString() };
            BedLoadOffsetOverrideMenuItemY.Activated += (sender, e) =>
            {
                if (vehicleItem == null)
                {
                    return;
                }
                if (!float.TryParse(NativeHelper.GetKeyboardInput($""), out float newAttachID))
                {
                    return;
                }
                vehicleItem.BedLoadOffsetOverride = new Vector3(vehicleItem.BedLoadOffsetOverride.X, newAttachID, vehicleItem.BedLoadOffsetOverride.Z);
                BedLoadOffsetOverrideMenuItemY.RightLabel = newAttachID.ToString();
            };
            SubMenu.AddItem(BedLoadOffsetOverrideMenuItemY);

            UIMenuItem BedLoadOffsetOverrideMenuItemZ = new UIMenuItem("BedLoadOffsetOverride Z") { RightLabel = vehicleItem.BedLoadOffsetOverride.Z.ToString() };
            BedLoadOffsetOverrideMenuItemZ.Activated += (sender, e) =>
            {
                if (vehicleItem == null)
                {
                    return;
                }
                if (!float.TryParse(NativeHelper.GetKeyboardInput($""), out float newAttachID))
                {
                    return;
                }
                vehicleItem.BedLoadOffsetOverride = new Vector3(vehicleItem.BedLoadOffsetOverride.X, vehicleItem.BedLoadOffsetOverride.Y, newAttachID);
                BedLoadOffsetOverrideMenuItemZ.RightLabel = newAttachID.ToString();
            };
            SubMenu.AddItem(BedLoadOffsetOverrideMenuItemZ);

        }



        UIMenuItem reattachMenuItem = new UIMenuItem("Reattach Ped");
        reattachMenuItem.Activated += (sender, e) =>
        {
            if (Player.InterestedVehicle == null || !Player.InterestedVehicle.Vehicle.Exists())
            {
                Game.DisplaySubtitle("No Vehicle");
                return;
            }
            StoredBody storedBody = Player.InterestedVehicle.VehicleBodyManager.StoredBodies.FirstOrDefault(x => x.VehicleDoorSeatData != null && x.VehicleDoorSeatData.SeatID == -2);
            if (storedBody == null)
            {
                Game.DisplaySubtitle("No Trunk Ped");
                return;
            }
            storedBody.ReAttach();
        };
        SubMenu.AddItem(reattachMenuItem);


        UIMenuItem attachNewMenu = new UIMenuItem("Attach New Ped");
        attachNewMenu.Activated += (sender, e) =>
        {
            if (Player.InterestedVehicle == null || !Player.InterestedVehicle.Vehicle.Exists())
            {
                Game.DisplaySubtitle("No Vehicle");
                return;
            }
            if (LastSpawnedPed != null && LastSpawnedPed.Pedestrian.Exists())
            {
                LastSpawnedPed.FullyDelete();
            }
            Ped toStore = new Ped(Game.LocalPlayer.Character.Position.Around2D(5f));
            if(!toStore.Exists())
            {
                Game.DisplaySubtitle("No Ped");
                return;
            }
            GameFiber.Sleep(1500);
            if (!toStore.Exists())
            {
                Game.DisplaySubtitle("No Ped");
                return;
            }
          
            LastSpawnedPed = World.Pedestrians.GetPedExt(toStore.Handle);
            if (LastSpawnedPed == null || !LastSpawnedPed.Pedestrian.Exists())
            {
                Game.DisplaySubtitle("No Ped");
                return;
            }
            if (Player.InterestedVehicle == null || !Player.InterestedVehicle.Vehicle.Exists())
            {
                Game.DisplaySubtitle("No Vehicle");
                return;
            }

            LastSpawnedPed.Pedestrian.Kill();


            Player.InterestedVehicle.VehicleBodyManager.LoadBody(LastSpawnedPed, new VehicleDoorSeatData("Trunk", "boot", 5, -2), false, World);


            if (Player.InterestedVehicle == null || !Player.InterestedVehicle.Vehicle.Exists())
            {

                return;
            }
            Player.InterestedVehicle.OpenDoor(5, false);
        };
        SubMenu.AddItem(attachNewMenu);


    }
}

