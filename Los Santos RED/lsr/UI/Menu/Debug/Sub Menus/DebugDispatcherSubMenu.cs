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
    public DebugDispatcherSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IAgencies agencies, Dispatcher dispatcher, IEntityProvideable world, IGangs gangs, IOrganizations organizations) : base(debug, menuPool, player)
    {
        Agencies = agencies;
        Dispatcher = dispatcher;
        World = world;
        Gangs = gangs;
        Organizations = organizations;
    }
    public override void AddItems()
    {
        UIMenu DispatcherMenu = MenuPool.AddSubMenu(Debug, "Dispatcher");
        DispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
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


        UIMenuListScrollerItem<TaxiFirm> SpawnTaxi = new UIMenuListScrollerItem<TaxiFirm>("Taxu Random Vehicle Spawn", "Spawn a random taxi ped with a vehicle", Organizations.PossibleOrganizations.TaxiFirms);
        SpawnTaxi.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnTaxi(SpawnTaxi.SelectedItem.ID, false, false);
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
}

