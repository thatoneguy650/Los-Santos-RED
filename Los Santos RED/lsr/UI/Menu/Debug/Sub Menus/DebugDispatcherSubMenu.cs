using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugDispatcherSubMenu : DebugSubMenu
{
    private IAgencies Agencies;
    private Dispatcher Dispatcher;
    private IEntityProvideable World;
    private IGangs Gangs;
    private IOrganizations Organizations;
    private UIMenu DispatcherMenu;
    private ISettingsProvideable Settings;

    public DebugDispatcherSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IAgencies agencies, Dispatcher dispatcher, IEntityProvideable world, IGangs gangs, IOrganizations organizations, ISettingsProvideable settings) : base(debug, menuPool, player)
    {
        Agencies = agencies;
        Dispatcher = dispatcher;
        World = world;
        Gangs = gangs;
        Organizations = organizations;
        Settings = settings;
    }
    public override void AddItems()
    {
        DispatcherMenu = MenuPool.AddSubMenu(Debug, "Dispatcher");
        DispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
        DispatcherMenu.Width = 0.4f;


        CreateNewSubMenu();





        UIMenuListScrollerItem<Agency> SpawnAgencyFoot = new UIMenuListScrollerItem<Agency>("Agency Random On-Foot Spawn", "Spawn a random agency ped on foot", Agencies.GetAgencies());
        SpawnAgencyFoot.Activated += (menu, item) =>
        {
            if (SpawnAgencyFoot.SelectedItem.ResponseType == ResponseType.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnAgencyFoot.SelectedItem.ID, true, false);
            }
            else if (SpawnAgencyFoot.SelectedItem.ResponseType == ResponseType.Fire)
            {
                Dispatcher.DebugSpawnFire(SpawnAgencyFoot.SelectedItem.ID, true, false);
            }
            else if (SpawnAgencyFoot.SelectedItem.ResponseType == ResponseType.Security)
            {
                Dispatcher.DebugSpawnSecurityGuard(SpawnAgencyFoot.SelectedItem.ID, true, false);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnAgencyFoot.SelectedItem.ID, true, false);
            }
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Agency> SpawnAgencyVehicle = new UIMenuListScrollerItem<Agency>("Agency Random Vehicle Spawn", "Spawn a random agency ped with a vehicle", Agencies.GetAgencies());
        SpawnAgencyVehicle.Activated += (menu, item) =>
        {
            if (SpawnAgencyVehicle.SelectedItem.ResponseType == ResponseType.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnAgencyVehicle.SelectedItem.ID, false, false);
            }
            else if (SpawnAgencyVehicle.SelectedItem.ResponseType == ResponseType.Fire)
            {
                Dispatcher.DebugSpawnFire(SpawnAgencyVehicle.SelectedItem.ID, false, false);
            }
            else if (SpawnAgencyVehicle.SelectedItem.ResponseType == ResponseType.Security)
            {
                Dispatcher.DebugSpawnSecurityGuard(SpawnAgencyVehicle.SelectedItem.ID, false, false);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnAgencyVehicle.SelectedItem.ID, false, false);
            }
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Agency> SpawnEmptyAgencyVehicle = new UIMenuListScrollerItem<Agency>("Agency Random Empty Vehicle Spawn", "Spawn a random agency empty vehicle", Agencies.GetAgencies());
        SpawnEmptyAgencyVehicle.Activated += (menu, item) =>
        {
            if (SpawnEmptyAgencyVehicle.SelectedItem.ResponseType == ResponseType.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnEmptyAgencyVehicle.SelectedItem.ID, false, true);
            }
            else if (SpawnEmptyAgencyVehicle.SelectedItem.ResponseType == ResponseType.Fire)
            {
                Dispatcher.DebugSpawnFire(SpawnEmptyAgencyVehicle.SelectedItem.ID, false, true);
            }
            else if (SpawnAgencyVehicle.SelectedItem.ResponseType == ResponseType.Security)
            {
                Dispatcher.DebugSpawnSecurityGuard(SpawnEmptyAgencyVehicle.SelectedItem.ID, false, true);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnEmptyAgencyVehicle.SelectedItem.ID, false, true);
            }
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Agency> SpawnAgencyK9Vehicle = new UIMenuListScrollerItem<Agency>("Agency Random K9 Vehicle Spawn", "Spawn a random agency ped & k9 with a vehicle", Agencies.GetAgenciesByResponse(ResponseType.LawEnforcement));
        SpawnAgencyK9Vehicle.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnK9Cop(SpawnAgencyK9Vehicle.SelectedItem.ID);
            menu.Visible = false;
        };

        UIMenuListScrollerItem<Gang> SpawnGangFoot = new UIMenuListScrollerItem<Gang>("Gang Random On-Foot Spawn", "Spawn a random gang ped on foot", Gangs.GetAllGangs());
        SpawnGangFoot.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnGang(SpawnGangFoot.SelectedItem.ID, true, false);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SpawnGangVehicle = new UIMenuListScrollerItem<Gang>("Gang Random Vehicle Spawn", "Spawn a random gang ped with a vehicle", Gangs.GetAllGangs());
        SpawnGangVehicle.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnGang(SpawnGangVehicle.SelectedItem.ID, false, false);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SpawnEmptyGangVehicle = new UIMenuListScrollerItem<Gang>("Gang Random Empty Vehicle Spawn", "Spawn a random empty gang vehicle", Gangs.GetAllGangs());
        SpawnEmptyGangVehicle.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnGang(SpawnEmptyGangVehicle.SelectedItem.ID, false, true);
            menu.Visible = false;
        };


        UIMenuListScrollerItem<TaxiFirm> SpawnTaxi = new UIMenuListScrollerItem<TaxiFirm>("Taxi Random Vehicle Spawn", "Spawn a random taxi ped with a vehicle", Organizations.PossibleOrganizations.TaxiFirms);
        SpawnTaxi.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnTaxi(SpawnTaxi.SelectedItem, false, false);
            menu.Visible = false;
        };





        UIMenuNumericScrollerItem<float> SpawnRockblock = new UIMenuNumericScrollerItem<float>("Spawn Roadblock", "Spawn roadblock", 10f, 200f, 10f);
        SpawnRockblock.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnRoadblock(SpawnRockblock.Value);
            menu.Visible = false;
        };

        UIMenuItem DespawnRockblock = new UIMenuItem("Despawn Roadblock", "Despawn roadblock");
        DespawnRockblock.Activated += (menu, item) =>
        {
            Dispatcher.DebugRemoveRoadblock();
            menu.Visible = false;
        };

        UIMenuItem PlayScanner = new UIMenuItem("Play Scanner", "Play some random scanner audio");
        PlayScanner.Activated += (menu, item) =>
        {
            Player.Scanner.ForceRandomDispatch();
            menu.Visible = false;
        };

        UIMenuItem RemoveCops = new UIMenuItem("Remove Cops", "Removes all the police");
        RemoveCops.Activated += (menu, item) =>
        {
            World.Pedestrians.ClearPolice();
            World.Vehicles.ClearPolice();
            menu.Visible = false;
        };

        UIMenuItem ClearSpawned = new UIMenuItem("Clear Spawned", "Removes all spawned items");
        ClearSpawned.Activated += (menu, item) =>
        {
            World.Pedestrians.ClearSpawned();
            World.Vehicles.ClearSpawned(true);
            menu.Visible = false;
        };


        DispatcherMenu.AddItem(SpawnAgencyFoot);
        DispatcherMenu.AddItem(SpawnAgencyVehicle);
        DispatcherMenu.AddItem(SpawnEmptyAgencyVehicle);
        DispatcherMenu.AddItem(SpawnAgencyK9Vehicle);
        DispatcherMenu.AddItem(SpawnGangFoot);
        DispatcherMenu.AddItem(SpawnGangVehicle);
        DispatcherMenu.AddItem(SpawnEmptyGangVehicle);
        DispatcherMenu.AddItem(SpawnTaxi);
        DispatcherMenu.AddItem(SpawnRockblock);
        DispatcherMenu.AddItem(DespawnRockblock);
        DispatcherMenu.AddItem(PlayScanner);
        DispatcherMenu.AddItem(RemoveCops);
        DispatcherMenu.AddItem(ClearSpawned);
    }

    private void CreateNewSubMenu()
    {
        UIMenu NewSubDispatcherMenu = MenuPool.AddSubMenu(DispatcherMenu, "Agency Spawn Menu");
        NewSubDispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
        NewSubDispatcherMenu.Width = 0.4f;








        List<Agency> AllAgencies = Agencies.GetAgencies();
        List<VehicleNameSelect> vehicleNameList = new List<VehicleNameSelect>();
        vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });


        foreach (DispatchableVehicle dv in AllAgencies.FirstOrDefault().Vehicles.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
        {
            VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName);
            vns.UpdateItems();
            vehicleNameList.Add(vns);
        }
        UIMenuListScrollerItem<VehicleNameSelect> SpawnVehicleScroller = new UIMenuListScrollerItem<VehicleNameSelect>("Vehicle", $"Choose a vehicle to spawn.", vehicleNameList);
        UIMenuListScrollerItem<Agency> SpawnAgencyScroller = new UIMenuListScrollerItem<Agency>("Agency", $"Choose an agency", AllAgencies);
        SpawnAgencyScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SpawnVehicleScroller.Items.Clear();
            List<VehicleNameSelect> vehicleNameList2 = new List<VehicleNameSelect>();
            vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });

            if (SpawnAgencyScroller.SelectedItem != null)
            {


                foreach (DispatchableVehicle dv in SpawnAgencyScroller.SelectedItem.Vehicles.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
                {
                    VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName);
                    vns.UpdateItems();
                    vehicleNameList2.Add(vns);
                }
            }
            SpawnVehicleScroller.Items = vehicleNameList2.ToList();
        };
        UIMenuItem SpawnItem = new UIMenuItem("Spawn", $"Spawn the item");// { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.TheftPaymentMin:C0}-{ActiveGang.TheftPaymentMax:C0}~s~" };
        SpawnItem.Activated += (sender, selectedItem) =>
        {
            if(SpawnAgencyScroller.SelectedItem == null)
            {
                return;
            }
            //Player.PlayerTasks.GangTasks.StartGangVehicleTheft(ActiveGang, GangContact, GangTheftTargets.SelectedItem, GangTheftVehicles.SelectedItem.ModelName, GangTheftVehicles.SelectedItem.ToString());
            if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.EMS)
            {
                if (SpawnVehicleScroller.SelectedItem.ModelName == "Random")
                {
                    Dispatcher.DebugSpawnEMT(SpawnAgencyScroller.SelectedItem.ID, false, true);
                }
            }
            else if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.Fire)
            {
                if (SpawnVehicleScroller.SelectedItem.ModelName == "Random")
                {
                    Dispatcher.DebugSpawnFire(SpawnAgencyScroller.SelectedItem.ID, false, true);
                }
            }
            else if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.Security)
            {
                if (SpawnVehicleScroller.SelectedItem.ModelName == "Random")
                {
                    Dispatcher.DebugSpawnSecurityGuard(SpawnAgencyScroller.SelectedItem.ID, false, true);
                }
            }
            else
            {
                if (SpawnVehicleScroller.SelectedItem.ModelName == "Random")
                {
                    Dispatcher.DebugSpawnCop(SpawnAgencyScroller.SelectedItem.ID, false, true);
                }
                else
                {
                    
                    Dispatcher.DebugSpawnCop(SpawnAgencyScroller.SelectedItem.ID, false, true, SpawnAgencyScroller.SelectedItem.Vehicles.FirstOrDefault(x => x.ModelName == SpawnVehicleScroller.SelectedItem.ModelName));
                }
            }
            sender.Visible = false;
        };
        NewSubDispatcherMenu.AddItem(SpawnAgencyScroller);
        NewSubDispatcherMenu.AddItem(SpawnVehicleScroller);
        NewSubDispatcherMenu.AddItem(SpawnItem);













        //UIMenuListScrollerItem<Agency> chooseAgencyScrollerItem = new UIMenuListScrollerItem<Agency>("Agency To Spawn", "Select an agency", Agencies.GetAgencies());
        //chooseAgencyScrollerItem.Activated += (menu, item) =>
        //{
        //    if (chooseAgencyScrollerItem.SelectedItem.ResponseType == ResponseType.EMS)
        //    {
        //        Dispatcher.DebugSpawnEMT(chooseAgencyScrollerItem.SelectedItem.ID, false, true);
        //    }
        //    else if (chooseAgencyScrollerItem.SelectedItem.ResponseType == ResponseType.Fire)
        //    {
        //        Dispatcher.DebugSpawnFire(chooseAgencyScrollerItem.SelectedItem.ID, false, true);
        //    }
        //    else if (SpawnAgencyVehicle.SelectedItem.ResponseType == ResponseType.Security)
        //    {
        //        Dispatcher.DebugSpawnSecurityGuard(chooseAgencyScrollerItem.SelectedItem.ID, false, true);
        //    }
        //    else
        //    {
        //        Dispatcher.DebugSpawnCop(chooseAgencyScrollerItem.SelectedItem.ID, false, true);
        //    }
        //    menu.Visible = false;
        //};
        //NewSubDispatcherMenu.AddItem(SpawnAgencyFoot);
    }
}

