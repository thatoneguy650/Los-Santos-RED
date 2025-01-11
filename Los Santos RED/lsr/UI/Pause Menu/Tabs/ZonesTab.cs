using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;

public class ZonesTab
{
    private IGangRelateable Player;
    private IPlacesOfInterest PlacesOfInterest;
    private IShopMenus ShopMenus;
    private IModItems ModItems;
    private IGangTerritories GangTerritories;
    private IZones Zones;
    private ISettingsProvideable Settings;

    private TabView TabView;
    private List<TabItem> items;
    private bool addedItems;
    private IEntityProvideable World;

    public ZonesTab(IGangRelateable player, IPlacesOfInterest placesOfInterest, IShopMenus shopMenus, IModItems modItems, IZones zones, TabView tabView, IGangTerritories gangTerritories, ISettingsProvideable settings, IEntityProvideable world)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        ShopMenus = shopMenus;
        ModItems = modItems;
        Zones = zones;
        TabView = tabView;
        GangTerritories = gangTerritories;
        Settings = settings;
        World = world;
    }
    public void AddItems()
    {
        items = new List<TabItem>();
        addedItems = false;
        string currentState = Player.CurrentLocation.CurrentZone?.GameState?.StateID;
        foreach (Zone zone in Zones.ZoneList.Where(x => x.GameState?.StateID == currentState || (x.GameState != null && Player.CurrentLocation.CurrentZone?.GameState != null && x.GameState.IsSisterState(Player.CurrentLocation.CurrentZone?.GameState))).OrderBy(x => x.DisplayName))
        {
            AddItem(zone);
        }
        if (addedItems)
        {
            TabView.AddTab(new TabSubmenuItem("Zones", items));
        }
    }
    private void AddItem(Zone zone)
    {
        List<MissionInformation> zoneMissionInfos = new List<MissionInformation>();
        // Relationship *planned*
        // infamy etc etc like watch dogs

        // gangs
        List<Tuple<string, string>> gangTuples = AddGangs(zone);
        MissionInformation GangInformation = new MissionInformation("Gangs", "", gangTuples);
        zoneMissionInfos.Add(GangInformation);

        // police+ other agencyies
        zoneMissionInfos.Add(new MissionInformation("Law Enforcement", "", AddAgency(zone, ResponseType.LawEnforcement)));
        zoneMissionInfos.Add(new MissionInformation("Medical Services", "", AddAgency(zone, ResponseType.EMS)));
        zoneMissionInfos.Add(new MissionInformation("Fire-Fighting", "", AddAgency(zone, ResponseType.Fire)));

        // drugs
        List<Tuple<string, string>> drugTuples = AddDrugs(zone);
        MissionInformation DrugInformation = new MissionInformation("Drugs", "", drugTuples);
        zoneMissionInfos.Add(DrugInformation);

        TabMissionSelectItem ZoneList = new TabMissionSelectItem($"{zone.DisplayName}~s~", zoneMissionInfos);
        items.Add(ZoneList);
        addedItems = true;
    }

    private List<Tuple<string, string>> AddGangs(Zone zone)
    {
        List<Tuple<string, string>> toReturn = new List<Tuple<string, string>>();
        foreach (GangReputation gr in Player.RelationshipManager.GangRelationships.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Member).ThenByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
        {
            List<ZoneJurisdiction> gangTerritory = GangTerritories.GetGangTerritory(gr.Gang.ID);

            if (gangTerritory != null && gangTerritory.Any())
            {
                foreach (ZoneJurisdiction zj in gangTerritory)
                {
                    Zone gangZone = Zones.GetZone(zj.ZoneInternalGameName);
                    if (gangZone != null && gangZone == zone)
                    {
                        toReturn.Add(new Tuple<string, string>($"{gr.Gang.ShortName} {gr.ToBlip()}", "")); break;
                    }
                }
            }
        }
        return toReturn;
    }

    private List<Tuple<string, string>> AddAgency(Zone zone, ResponseType responseType)
    {
        List<Tuple<string, string>> toReturn = new List<Tuple<string, string>>();
        if(zone.Agencies == null || !zone.Agencies.Any(x=> x.ResponseType == responseType))
        {
            return toReturn;
        }
        foreach (Agency agency in zone.Agencies.Where(x => x.ResponseType == responseType).OrderBy(x=> x.Classification).ThenBy(x => x.ShortName))
        {
            toReturn.Add(new Tuple<string, string>($"{agency.ShortName} {agency.Classification}", ""));
        }
        return toReturn;
    }

    private List<Tuple<string, string>> AddDrugs(Zone zone)
    {
        List<Tuple<string, string>> toReturn = new List<Tuple<string, string>>();
        List<ModItem> drugsList = ModItems.AllItems().Where(x => x.ItemType == ItemType.Drugs && x.ItemSubType == ItemSubType.Narcotic).ToList();
        List<string> zoneDrugs = new List<string>();

        // Get economy drug menu. Unsure how to do this.

        if (GangTerritories.GetGangs(zone.InternalGameName, 0) != null)
        {
            List<Gang> gangList = GangTerritories.GetGangs(zone.InternalGameName, 0);
            foreach (Gang gang in gangList)
            {
                GangDen myDen = PlacesOfInterest.GetMainDen(gang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
                ShopMenu dealerMenu;
                ShopMenu denMenu;
                dealerMenu = ShopMenus.GetWeightedRandomMenuFromGroup(gang.DealerMenuGroup);
                denMenu = myDen?.Menu;

                if (dealerMenu != null)
                {
                    foreach (MenuItem mi in dealerMenu.Items)
                    {
                        ModItem modItem = ModItems.Get(mi.ModItemName);
                        if (modItem != null)
                        {
                            if (modItem.ItemType == ItemType.Drugs && !zoneDrugs.Contains(modItem.Name))
                            {
                                zoneDrugs.Add(modItem.Name);
                            }
                        }
                    }
                }
                if (denMenu != null)
                {
                    foreach (MenuItem mi in denMenu.Items)
                    {
                        ModItem modItem = ModItems.Get(mi.ModItemName);
                        if (modItem != null)
                        {
                            if (modItem.ItemType == ItemType.Drugs && !zoneDrugs.Contains(modItem.Name))
                            {
                                zoneDrugs.Add(modItem.Name);
                            }
                        }
                    }
                }
            }
        }

        if (zone.CustomerMenus != null)
        {
            foreach (ModItem drug in drugsList)
            {
                if (zone.CustomerMenus.HasItem(drug, ShopMenus) && !zoneDrugs.Contains(drug.Name))
                {
                    zoneDrugs.Add(drug.Name);
                }
            }
        }

        if (zoneDrugs.Any())
        {
            foreach (string drug in zoneDrugs)
            {
                toReturn.Add(new Tuple<string, string>(drug, ""));
            }
        }

        return toReturn;
    }

}