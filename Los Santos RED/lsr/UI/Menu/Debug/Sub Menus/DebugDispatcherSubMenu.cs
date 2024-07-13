using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


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


        CreateAgencySubMenu();
        CreateGangSubMenu();
        CreateRoadblockSubMenu();



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


        DispatcherMenu.AddItem(PlayScanner);
        DispatcherMenu.AddItem(RemoveCops);
        DispatcherMenu.AddItem(ClearSpawned);
    }

    private void CreateRoadblockSubMenu()
    {
        UIMenu RoadblockNewMenu = MenuPool.AddSubMenu(DispatcherMenu, "Roadblock Spawn Menu");
        RoadblockNewMenu.SetBannerType(EntryPoint.LSRedColor);
        RoadblockNewMenu.Width = 0.6f;


        UIMenuCheckboxItem EnableCars = new UIMenuCheckboxItem("Enable Cars", true);
        UIMenuCheckboxItem EnableSpike = new UIMenuCheckboxItem("Enable Spikes", true);
        UIMenuCheckboxItem EnableProps = new UIMenuCheckboxItem("Enable Props", true);
        RoadblockNewMenu.AddItem(EnableCars);
        RoadblockNewMenu.AddItem(EnableSpike);
        RoadblockNewMenu.AddItem(EnableProps);
        UIMenuNumericScrollerItem<float> SpawnRockblock = new UIMenuNumericScrollerItem<float>("Spawn Roadblock", "Spawn roadblock", 10f, 300f, 10f) { Value = 150f };
        SpawnRockblock.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnRoadblock(EnableCars.Checked, EnableSpike.Checked, EnableProps.Checked, SpawnRockblock.Value);
            menu.Visible = false;
        };
        RoadblockNewMenu.AddItem(SpawnRockblock);
        UIMenuItem DespawnRockblock = new UIMenuItem("Despawn Roadblock", "Despawn roadblock");
        DespawnRockblock.Activated += (menu, item) =>
        {
            Dispatcher.DebugRemoveRoadblock();
            menu.Visible = false;
        };
        RoadblockNewMenu.AddItem(DespawnRockblock);

    }

    private void CreateAgencySubMenu()
    {
        UIMenu NewSubDispatcherMenu = MenuPool.AddSubMenu(DispatcherMenu, "Agency Spawn Menu");
        NewSubDispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
        NewSubDispatcherMenu.Width = 0.6f;

        List<Agency> AllAgencies = Agencies.GetAgencies().Where(x=> x.Personnel != null && x.Vehicles != null).OrderBy(x => x.Classification).ThenBy(x => x.FullName).ToList();
        List<VehicleNameSelect> vehicleNameList = new List<VehicleNameSelect>();
        vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });

        if(AllAgencies == null || !AllAgencies.Any())
        {
            return;
        }
        Agency first = AllAgencies.FirstOrDefault();
        if(first== null || first.Vehicles == null || !first.Vehicles.Any() || first.Personnel == null || !first.Personnel.Any())
        {
            return;
        }

        foreach (DispatchableVehicle dv in first.Vehicles.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
        {
            VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName) { DispatchableVehicle = dv };
            vns.UpdateItems();
            vehicleNameList.Add(vns);
        }
        UIMenuListScrollerItem<VehicleNameSelect> SpawnVehicleScroller = new UIMenuListScrollerItem<VehicleNameSelect>("Vehicle", $"Choose a vehicle to spawn.", vehicleNameList);

        List<DispatchablePerson> pedNameList = new List<DispatchablePerson>();
        pedNameList.Add(new DispatchablePerson() { DebugName = "Random" });
        foreach (DispatchablePerson dv in first.Personnel)
        {
            pedNameList.Add(dv);
        }
        UIMenuListScrollerItem<DispatchablePerson> SpawnPedScroller = new UIMenuListScrollerItem<DispatchablePerson>("Ped", $"Choose a ped to spawn.", pedNameList);

        UIMenuCheckboxItem SpawnVehicleCheckboxItem = new UIMenuCheckboxItem("Include Vehicle",true, "If checked a vehicle will be spawned");
        UIMenuCheckboxItem SpawnPedCheckboxItem = new UIMenuCheckboxItem("Include Ped", true, "If checked a ped will be spawned");

        UIMenuListScrollerItem<Agency> SpawnAgencyScroller = new UIMenuListScrollerItem<Agency>("Agency", $"Choose an agency", AllAgencies);
        SpawnAgencyScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SpawnVehicleScroller.Items.Clear();
            List<VehicleNameSelect> vehicleNameList2 = new List<VehicleNameSelect>();
            vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });

            Agency selectedAgency = SpawnAgencyScroller.SelectedItem;
            EntryPoint.WriteToConsole($"selectedAgency?{selectedAgency}");

            if (selectedAgency != null && selectedAgency.Vehicles != null && selectedAgency.Vehicles.Any())
            {
                EntryPoint.WriteToConsole($"I AM AGENCY {selectedAgency.FullName} VehiclesNulls?{selectedAgency.Vehicles == null}");
                EntryPoint.WriteToConsole($"VehiclesNulls?{selectedAgency.Vehicles == null}");
                EntryPoint.WriteToConsole($"VehiclesEMPTY?{selectedAgency.Vehicles.Any()}");
                foreach (DispatchableVehicle dv in selectedAgency.Vehicles.ToList())//.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
                {
                    if(dv == null)
                    { 
                        continue;
                    }
                    VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName) { DispatchableVehicle = dv };
                    vns.UpdateItems();
                    vehicleNameList2.Add(vns);
                }
            }
            SpawnVehicleScroller.Items = vehicleNameList2.ToList();

            SpawnPedScroller.Items.Clear();
            List<DispatchablePerson> pedNameList2 = new List<DispatchablePerson>();
            pedNameList2.Add(new DispatchablePerson() { DebugName = "Random" });
            if (SpawnAgencyScroller.SelectedItem != null && SpawnAgencyScroller.SelectedItem.Personnel != null && SpawnAgencyScroller.SelectedItem.Personnel.Any())
            {
                foreach (DispatchablePerson dv in SpawnAgencyScroller.SelectedItem.Personnel)
                {
                    pedNameList2.Add(dv);
                }
            }
            SpawnPedScroller.Items = pedNameList2.ToList();
        };


        SpawnPedScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            if (SpawnPedScroller.SelectedItem == null)
            {
                return;
            }
            SpawnPedScroller.Description = SpawnPedScroller.SelectedItem.GetDescription();
        };

        SpawnVehicleScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            if(SpawnVehicleScroller.SelectedItem == null || SpawnVehicleScroller.SelectedItem.DispatchableVehicle == null)
            {
                return;
            }
            SpawnVehicleScroller.Description = SpawnVehicleScroller.SelectedItem.DispatchableVehicle.GetDescription();

        };


        UIMenuItem SpawnItem = new UIMenuItem("Spawn", $"Spawn the item");
        SpawnItem.Activated += (sender, selectedItem) =>
        {
            if(SpawnAgencyScroller.SelectedItem == null)
            {
                return;
            }
            bool spawnPed = SpawnPedCheckboxItem.Checked;
            bool spawnVehicle = SpawnVehicleCheckboxItem.Checked;
            string selectedAgencyID = SpawnAgencyScroller.SelectedItem.ID;
            ResponseType selectAgencyResponseType = SpawnAgencyScroller.SelectedItem.ResponseType;
            DispatchableVehicle selectedDispatchableVehicle = null;
            if(spawnVehicle && SpawnVehicleScroller.Items != null && SpawnVehicleScroller.Items.Any() && SpawnVehicleScroller.SelectedItem != null && SpawnVehicleScroller.SelectedItem.DispatchableVehicle != null && SpawnVehicleScroller.SelectedItem.DispatchableVehicle.ModelName != "Random")
            {
                selectedDispatchableVehicle = SpawnVehicleScroller.SelectedItem.DispatchableVehicle;
            }
            DispatchablePerson selectedDispatchablePerson = null;
            if(spawnPed && SpawnPedScroller.Items != null && SpawnPedScroller.Items.Any() && SpawnPedScroller.SelectedItem != null && SpawnPedScroller.SelectedItem.ModelName != "Random")
            {
                selectedDispatchablePerson = SpawnPedScroller.SelectedItem;
            }
            EntryPoint.WriteToConsole($"selectedAgencyID {selectedAgencyID} spawnPed {spawnPed} spawnVehicle {spawnVehicle} onFoot: {!spawnVehicle} isEmpty: {!spawnPed} DV: {(selectedDispatchableVehicle != null ? selectedDispatchableVehicle.ModelName : "")} DP: {(selectedDispatchablePerson != null ? selectedDispatchablePerson.ModelName : "")}");
            if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.EMS)
            {
                Dispatcher.EMSDispatcher.DebugSpawnEMT(selectedAgencyID, !spawnVehicle, !spawnPed, selectedDispatchableVehicle, selectedDispatchablePerson);
            }
            else if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.Fire)
            {
                Dispatcher.FireDispatcher.DebugSpawnFire(selectedAgencyID, !spawnVehicle, !spawnPed, selectedDispatchableVehicle, selectedDispatchablePerson);
            }
            else if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.Security)
            {
                Dispatcher.SecurityDispatcher.DebugSpawnSecurity(selectedAgencyID, !spawnVehicle, !spawnPed, selectedDispatchableVehicle, selectedDispatchablePerson);
            }
            else
            {
                Dispatcher.LEDispatcher.DebugSpawnCop(SpawnAgencyScroller.SelectedItem.ID, !spawnVehicle, !spawnPed, selectedDispatchableVehicle, selectedDispatchablePerson, false);
            }
            sender.Visible = false;
        };
        NewSubDispatcherMenu.AddItem(SpawnAgencyScroller);
        NewSubDispatcherMenu.AddItem(SpawnVehicleCheckboxItem);
        NewSubDispatcherMenu.AddItem(SpawnVehicleScroller);
        NewSubDispatcherMenu.AddItem(SpawnPedCheckboxItem);
        NewSubDispatcherMenu.AddItem(SpawnPedScroller);
        NewSubDispatcherMenu.AddItem(SpawnItem);
    }




    private void CreateGangSubMenu()
    {
        UIMenu NewSubDispatcherMenu = MenuPool.AddSubMenu(DispatcherMenu, "Gang Spawn Menu");
        NewSubDispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
        NewSubDispatcherMenu.Width = 0.6f;

        List<Gang> AllGangs = Gangs.GetAllGangs().Where(x=> x.Personnel != null && x.Vehicles != null).ToList();;// Agencies.GetAgencies();
        List<VehicleNameSelect> vehicleNameList = new List<VehicleNameSelect>();
        vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });




        if (AllGangs == null || !AllGangs.Any())
        {
            return;
        }
        Gang first = AllGangs.FirstOrDefault();
        if (first == null || first.Vehicles == null || !first.Vehicles.Any() || first.Personnel == null || !first.Personnel.Any())
        {
            return;
        }

        foreach (DispatchableVehicle dv in first.Vehicles.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
        {
            VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName) { DispatchableVehicle = dv };
            vns.UpdateItems();
            vehicleNameList.Add(vns);
        }
        UIMenuListScrollerItem<VehicleNameSelect> SpawnVehicleScroller = new UIMenuListScrollerItem<VehicleNameSelect>("Vehicle", $"Choose a vehicle to spawn.", vehicleNameList);

        List<DispatchablePerson> pedNameList = new List<DispatchablePerson>();
        pedNameList.Add(new DispatchablePerson() { DebugName = "Random" });
        foreach (DispatchablePerson dv in first.Personnel)
        {
            pedNameList.Add(dv);
        }
        UIMenuListScrollerItem<DispatchablePerson> SpawnPedScroller = new UIMenuListScrollerItem<DispatchablePerson>("Ped", $"Choose a ped to spawn.", pedNameList);

        UIMenuCheckboxItem SpawnVehicleCheckboxItem = new UIMenuCheckboxItem("Include Vehicle", true, "If checked a vehicle will be spawned");
        UIMenuCheckboxItem SpawnPedCheckboxItem = new UIMenuCheckboxItem("Include Ped", true, "If checked a ped will be spawned");

        UIMenuListScrollerItem<Gang> SpawnGangScroller = new UIMenuListScrollerItem<Gang>("Gang", $"Choose an gang", AllGangs);
        SpawnGangScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SpawnVehicleScroller.Items.Clear();
            List<VehicleNameSelect> vehicleNameList2 = new List<VehicleNameSelect>();
            vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });
            if (SpawnGangScroller.SelectedItem != null && SpawnGangScroller.SelectedItem.Vehicles != null && SpawnGangScroller.SelectedItem.Vehicles.Any())
            {
                foreach (DispatchableVehicle dv in SpawnGangScroller.SelectedItem.Vehicles.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
                {
                    VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName) { DispatchableVehicle = dv };
                    vns.UpdateItems();
                    vehicleNameList2.Add(vns);
                }
            }
            SpawnVehicleScroller.Items = vehicleNameList2.ToList();

            SpawnPedScroller.Items.Clear();
            List<DispatchablePerson> pedNameList2 = new List<DispatchablePerson>();
            pedNameList.Add(new DispatchablePerson() { DebugName = "Random" });
            if (SpawnGangScroller.SelectedItem != null && SpawnGangScroller.SelectedItem.Personnel != null && SpawnGangScroller.SelectedItem.Personnel.Any())
            {
                foreach (DispatchablePerson dv in SpawnGangScroller.SelectedItem.Personnel)
                {
                    pedNameList.Add(dv);
                }
            }
            SpawnPedScroller.Items = pedNameList2.ToList();
        };


        SpawnPedScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            if (SpawnPedScroller.SelectedItem == null)
            {
                return;
            }
            SpawnPedScroller.Description = SpawnPedScroller.SelectedItem.GetDescription();
        };

        SpawnVehicleScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            if (SpawnVehicleScroller.SelectedItem == null || SpawnVehicleScroller.SelectedItem.DispatchableVehicle == null)
            {
                return;
            }
            SpawnVehicleScroller.Description = SpawnVehicleScroller.SelectedItem.DispatchableVehicle.GetDescription();

        };


        UIMenuItem SpawnItem = new UIMenuItem("Spawn", $"Spawn the item");
        SpawnItem.Activated += (sender, selectedItem) =>
        {
            if (SpawnGangScroller.SelectedItem == null)
            {
                return;
            }
            bool spawnPed = SpawnPedCheckboxItem.Checked;
            bool spawnVehicle = SpawnVehicleCheckboxItem.Checked;
            string selectedGangID = SpawnGangScroller.SelectedItem.ID;
            DispatchableVehicle selectedDispatchableVehicle = null;
            if(spawnVehicle && SpawnVehicleScroller.Items != null && SpawnVehicleScroller.Items.Any() && SpawnVehicleScroller.SelectedItem != null && SpawnVehicleScroller.SelectedItem.DispatchableVehicle != null && SpawnVehicleScroller.SelectedItem.DispatchableVehicle.ModelName != "Random")
            {
                selectedDispatchableVehicle = SpawnVehicleScroller.SelectedItem.DispatchableVehicle;
            }
            DispatchablePerson selectedDispatchablePerson = null;
            if(spawnPed && SpawnPedScroller.Items != null && SpawnPedScroller.Items.Any() && SpawnPedScroller.SelectedItem != null && SpawnPedScroller.SelectedItem.ModelName != "Random")
            {
                selectedDispatchablePerson = SpawnPedScroller.SelectedItem;
            }
            EntryPoint.WriteToConsole($"selectedGangID {selectedGangID} spawnPed {spawnPed} spawnVehicle {spawnVehicle} onFoot: {!spawnVehicle} isEmpty: {!spawnPed} DV: {(selectedDispatchableVehicle != null ? selectedDispatchableVehicle.ModelName : "")} DP: {(selectedDispatchablePerson != null ? selectedDispatchablePerson.ModelName : "")}");
            
            Dispatcher.GangDispatcher.DebugSpawnGangMember(selectedGangID, !spawnVehicle, !spawnPed,selectedDispatchableVehicle,selectedDispatchablePerson);
            sender.Visible = false;
        };
        NewSubDispatcherMenu.AddItem(SpawnGangScroller);
        NewSubDispatcherMenu.AddItem(SpawnVehicleCheckboxItem);
        NewSubDispatcherMenu.AddItem(SpawnVehicleScroller);
        NewSubDispatcherMenu.AddItem(SpawnPedCheckboxItem);
        NewSubDispatcherMenu.AddItem(SpawnPedScroller);
        NewSubDispatcherMenu.AddItem(SpawnItem);
    }

    //private void CreateOrganizationSubMenu()
    //{
    //    UIMenu NewSubDispatcherMenu = MenuPool.AddSubMenu(DispatcherMenu, "Organization Spawn Menu");
    //    NewSubDispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
    //    NewSubDispatcherMenu.Width = 0.6f;

    //    List<Organization> AllAgencies = Organizations.GetOrganizations().Where(x => x.Personnel != null && x.Vehicles != null).ToList();
    //    List<VehicleNameSelect> vehicleNameList = new List<VehicleNameSelect>();
    //    vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });

    //    if (AllAgencies == null || !AllAgencies.Any())
    //    {
    //        return;
    //    }
    //    Organization first = AllAgencies.FirstOrDefault();
    //    if (first == null || first.Vehicles == null || !first.Vehicles.Any() || first.Personnel == null || !first.Personnel.Any())
    //    {
    //        return;
    //    }

    //    foreach (DispatchableVehicle dv in first.Vehicles.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
    //    {
    //        VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName) { DispatchableVehicle = dv };
    //        vns.UpdateItems();
    //        vehicleNameList.Add(vns);
    //    }
    //    UIMenuListScrollerItem<VehicleNameSelect> SpawnVehicleScroller = new UIMenuListScrollerItem<VehicleNameSelect>("Vehicle", $"Choose a vehicle to spawn.", vehicleNameList);

    //    List<DispatchablePerson> pedNameList = new List<DispatchablePerson>();
    //    pedNameList.Add(new DispatchablePerson() { DebugName = "Random" });
    //    foreach (DispatchablePerson dv in first.Personnel)
    //    {
    //        pedNameList.Add(dv);
    //    }
    //    UIMenuListScrollerItem<DispatchablePerson> SpawnPedScroller = new UIMenuListScrollerItem<DispatchablePerson>("Ped", $"Choose a ped to spawn.", pedNameList);

    //    UIMenuCheckboxItem SpawnVehicleCheckboxItem = new UIMenuCheckboxItem("Include Vehicle", true, "If checked a vehicle will be spawned");
    //    UIMenuCheckboxItem SpawnPedCheckboxItem = new UIMenuCheckboxItem("Include Ped", true, "If checked a ped will be spawned");

    //    UIMenuListScrollerItem<Organization> SpawnAgencyScroller = new UIMenuListScrollerItem<Organization>("Organization", $"Choose an org", AllAgencies);
    //    SpawnAgencyScroller.IndexChanged += (sender, oldIndex, newIndex) =>
    //    {
    //        SpawnVehicleScroller.Items.Clear();
    //        List<VehicleNameSelect> vehicleNameList2 = new List<VehicleNameSelect>();
    //        vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });

    //        Organization selectedAgency = SpawnAgencyScroller.SelectedItem;
    //        EntryPoint.WriteToConsole($"selectedOrganization?{selectedAgency}");

    //        if (selectedAgency != null && selectedAgency.Vehicles != null && selectedAgency.Vehicles.Any())
    //        {
    //            EntryPoint.WriteToConsole($"I AM Organization {selectedAgency.FullName} VehiclesNulls?{selectedAgency.Vehicles == null}");
    //            EntryPoint.WriteToConsole($"VehiclesNulls?{selectedAgency.Vehicles == null}");
    //            EntryPoint.WriteToConsole($"VehiclesEMPTY?{selectedAgency.Vehicles.Any()}");
    //            foreach (DispatchableVehicle dv in selectedAgency.Vehicles.ToList())//.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
    //            {
    //                if (dv == null)
    //                {
    //                    continue;
    //                }
    //                VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName) { DispatchableVehicle = dv };
    //                vns.UpdateItems();
    //                vehicleNameList2.Add(vns);
    //            }
    //        }
    //        SpawnVehicleScroller.Items = vehicleNameList2.ToList();

    //        SpawnPedScroller.Items.Clear();
    //        List<DispatchablePerson> pedNameList2 = new List<DispatchablePerson>();
    //        pedNameList2.Add(new DispatchablePerson() { DebugName = "Random" });
    //        if (SpawnAgencyScroller.SelectedItem != null && SpawnAgencyScroller.SelectedItem.Personnel != null && SpawnAgencyScroller.SelectedItem.Personnel.Any())
    //        {
    //            foreach (DispatchablePerson dv in SpawnAgencyScroller.SelectedItem.Personnel)
    //            {
    //                pedNameList2.Add(dv);
    //            }
    //        }
    //        SpawnPedScroller.Items = pedNameList2.ToList();
    //    };


    //    SpawnPedScroller.IndexChanged += (sender, oldIndex, newIndex) =>
    //    {
    //        if (SpawnPedScroller.SelectedItem == null)
    //        {
    //            return;
    //        }
    //        SpawnPedScroller.Description = SpawnPedScroller.SelectedItem.GetDescription();
    //    };

    //    SpawnVehicleScroller.IndexChanged += (sender, oldIndex, newIndex) =>
    //    {
    //        if (SpawnVehicleScroller.SelectedItem == null || SpawnVehicleScroller.SelectedItem.DispatchableVehicle == null)
    //        {
    //            return;
    //        }
    //        SpawnVehicleScroller.Description = SpawnVehicleScroller.SelectedItem.DispatchableVehicle.GetDescription();

    //    };


    //    UIMenuItem SpawnItem = new UIMenuItem("Spawn", $"Spawn the item");
    //    SpawnItem.Activated += (sender, selectedItem) =>
    //    {
    //        if (SpawnAgencyScroller.SelectedItem == null)
    //        {
    //            return;
    //        }
    //        bool spawnPed = SpawnPedCheckboxItem.Checked;
    //        bool spawnVehicle = SpawnVehicleCheckboxItem.Checked;
    //        string selectedAgencyID = SpawnAgencyScroller.SelectedItem.ID;
    //        ResponseType selectAgencyResponseType = SpawnAgencyScroller.SelectedItem.ResponseType;
    //        DispatchableVehicle selectedDispatchableVehicle = null;
    //        if (spawnVehicle && SpawnVehicleScroller.Items != null && SpawnVehicleScroller.Items.Any() && SpawnVehicleScroller.SelectedItem != null && SpawnVehicleScroller.SelectedItem.DispatchableVehicle != null && SpawnVehicleScroller.SelectedItem.DispatchableVehicle.ModelName != "Random")
    //        {
    //            selectedDispatchableVehicle = SpawnVehicleScroller.SelectedItem.DispatchableVehicle;
    //        }
    //        DispatchablePerson selectedDispatchablePerson = null;
    //        if (spawnPed && SpawnPedScroller.Items != null && SpawnPedScroller.Items.Any() && SpawnPedScroller.SelectedItem != null && SpawnPedScroller.SelectedItem.ModelName != "Random")
    //        {
    //            selectedDispatchablePerson = SpawnPedScroller.SelectedItem;
    //        }
    //        EntryPoint.WriteToConsole($"selectedAgencyID {selectedAgencyID} spawnPed {spawnPed} spawnVehicle {spawnVehicle} onFoot: {!spawnVehicle} isEmpty: {!spawnPed} DV: {(selectedDispatchableVehicle != null ? selectedDispatchableVehicle.ModelName : "")} DP: {(selectedDispatchablePerson != null ? selectedDispatchablePerson.ModelName : "")}");
    //        if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.EMS)
    //        {
    //            Dispatcher.EMSDispatcher.DebugSpawnEMT(selectedAgencyID, !spawnVehicle, !spawnPed, selectedDispatchableVehicle, selectedDispatchablePerson);
    //        }
    //        else if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.Fire)
    //        {
    //            Dispatcher.FireDispatcher.DebugSpawnFire(selectedAgencyID, !spawnVehicle, !spawnPed, selectedDispatchableVehicle, selectedDispatchablePerson);
    //        }
    //        else if (SpawnAgencyScroller.SelectedItem.ResponseType == ResponseType.Security)
    //        {
    //            Dispatcher.SecurityDispatcher.DebugSpawnSecurity(selectedAgencyID, !spawnVehicle, !spawnPed, selectedDispatchableVehicle, selectedDispatchablePerson);
    //        }
    //        else
    //        {
    //            Dispatcher.LEDispatcher.DebugSpawnCop(SpawnAgencyScroller.SelectedItem.ID, !spawnVehicle, !spawnPed, selectedDispatchableVehicle, selectedDispatchablePerson, false);
    //        }


    //        Dispae


    //        sender.Visible = false;
    //    };
    //    NewSubDispatcherMenu.AddItem(SpawnAgencyScroller);
    //    NewSubDispatcherMenu.AddItem(SpawnVehicleCheckboxItem);
    //    NewSubDispatcherMenu.AddItem(SpawnVehicleScroller);
    //    NewSubDispatcherMenu.AddItem(SpawnPedCheckboxItem);
    //    NewSubDispatcherMenu.AddItem(SpawnPedScroller);
    //    NewSubDispatcherMenu.AddItem(SpawnItem);
    //}

}

