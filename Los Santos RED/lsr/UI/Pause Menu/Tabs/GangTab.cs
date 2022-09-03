using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangTab
{
    private IGangRelateable Player;
    private IPlacesOfInterest PlacesOfInterest;
    private IShopMenus ShopMenus;
    private IModItems ModItems;
    private IWeapons Weapons;
    private IGangTerritories GangTerritories;
    private IZones Zones;

    private string DescriptionText;

    private TabView TabView;
    private List<TabItem> items;
    private bool addedItems;

    public GangTab(IGangRelateable player, IPlacesOfInterest placesOfInterest, IShopMenus shopMenus, IModItems modItems, IWeapons weapons, IGangTerritories gangTerritories, IZones zones, TabView tabView)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        ShopMenus = shopMenus;
        ModItems = modItems;
        Weapons = weapons;
        GangTerritories = gangTerritories;
        Zones = zones;
        TabView = tabView;
    }
    public void AddItems()
    {
        items = new List<TabItem>();
        addedItems = false;
        foreach (GangReputation gr in Player.RelationshipManager.GangRelationships.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
        {
            AddItem(gr);
        }
        if (addedItems)
        {
            TabView.AddTab(new TabSubmenuItem("Gangs", items));
        }
    }
    private void AddItem(GangReputation gr)
    {
        DescriptionText = "";
        AddRelationship(gr);
        AddDen(gr);
        Addterritory(gr);
        AddPlayerInteraction(gr);
        TabItem tabItem = new TabTextItem($"{gr.Gang.ShortName} {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        items.Add(tabItem);
        addedItems = true;
    }
    private void AddRelationship(GangReputation gr)
    {
        DescriptionText += "Relationship: " + gr.ToStringBare();
    }
    private void AddDen(GangReputation gr)
    {
        ShopMenu dealerMenu;
        ShopMenu denMenu;
        GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == gr.Gang.ID);
        if (myDen != null && myDen.IsEnabled)
        {
            DescriptionText += $"~n~{gr.Gang.DenName}: {myDen.FullStreetAddress}"; //+ gr.ToStringBare();
        }
        dealerMenu = ShopMenus.GetRandomMenu(gr.Gang.DealerMenuGroup);
        denMenu = myDen?.Menu;
        List<string> Drugs = new List<string>();
        List<string> Guns = new List<string>();
        if (dealerMenu != null)
        {
            foreach (MenuItem mi in dealerMenu.Items)
            {
                ModItem modItem = ModItems.Get(mi.ModItemName);
                if (modItem != null)
                {
                    if (modItem.ItemType == ItemType.Drugs)
                    {
                        if (!Drugs.Contains(modItem.Name))
                        {
                            Drugs.Add(modItem.Name);
                        }
                    }
                    else if (modItem.ItemType == ItemType.Weapons)
                    {
                        WeaponInformation wi = Weapons.GetWeapon(modItem.ModelItem?.ModelName);
                        if (wi != null)
                        {
                            if (!Guns.Contains(wi.Category.ToString()))
                            {
                                Guns.Add(wi.Category.ToString());
                            }
                        }
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
                    if (modItem.ItemType == ItemType.Drugs)
                    {
                        if (!Drugs.Contains(modItem.Name))
                        {
                            Drugs.Add(modItem.Name);
                        }
                    }
                    else if (modItem.ItemType == ItemType.Weapons)
                    {
                        WeaponInformation wi = Weapons.GetWeapon(modItem.ModelItem?.ModelName);
                        if (wi != null)
                        {
                            if (!Guns.Contains(wi.Category.ToString()))
                            {
                                Guns.Add(wi.Category.ToString());
                            }
                        }
                    }
                }
            }
        }
        if (Drugs.Any())
        {
            DescriptionText += $"~n~Drugs: {string.Join(", ", Drugs.OrderBy(x => x))}"; //+ gr.ToStringBare();
        }
        //if (Guns.Any())
        //{
        //    DescriptionText += $"~n~Guns: {string.Join(", ", Guns.OrderBy(x => x))}"; //+ gr.ToStringBare();
        //}
    }
    private void Addterritory(GangReputation gr)
    {
        string TerritoryText = "None";
        List<ZoneJurisdiction> gangTerritory = GangTerritories.GetGangTerritory(gr.Gang.ID);
        if (gangTerritory.Any())
        {
            TerritoryText = "";
            foreach (ZoneJurisdiction zj in gangTerritory)
            {
                Zone myZone = Zones.GetZone(zj.ZoneInternalGameName);
                if (myZone != null)
                {
                    TerritoryText += "~p~" + myZone.DisplayName + "~s~, ";
                }
            }
        }
        DescriptionText += $"~n~Territory: {TerritoryText.TrimEnd(' ', ',')}";
    }
    private void AddPlayerInteraction(GangReputation gr)
    {
        if (Player.CellPhone.IsContactEnabled(gr.Gang.ContactName))
        {
            string ContactText = gr.Gang.ContactName;
            DescriptionText += $"~n~Contacts: {ContactText}";
        }
        PlayerTask gangTask = Player.PlayerTasks.GetTask(gr.Gang.ContactName);
        if (gangTask != null)
        {
            DescriptionText += $"~n~~g~Has Task~s~";
            if (gangTask.CanExpire)
            {
                DescriptionText += $" Complete Before: ~r~{gangTask.ExpireTime:d} {gangTask.ExpireTime:t}~s~";
            }
        }

        if (gr.MembersKilled > 0)
        {
            DescriptionText += $"~n~~r~Members Killed~s~: {gr.MembersKilled}~s~ ({gr.MembersKilledInTerritory})";
        }
        if (gr.MembersHurt > 0)
        {
            DescriptionText += $"~n~~o~Members Hurt~s~: {gr.MembersHurt}~s~ ({gr.MembersHurtInTerritory})";
        }
        if (gr.MembersCarJacked > 0)
        {
            DescriptionText += $"~n~~o~Members CarJacked~s~: {gr.MembersCarJacked}~s~ ({gr.MembersCarJackedInTerritory})";
        }

        if (gr.PlayerDebt > 0)
        {
            string debtstring = gr.PlayerDebt.ToString("C0");
            DescriptionText += $"~n~~r~Debt~s~: ~r~{debtstring}~s~";
        }

        if (gr.IsMember && Player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
        {
            DescriptionText += $"~n~{Player.RelationshipManager.GangRelationships.CurrentGangKickUp}";
        }
    }
}

//using LosSantosRED.lsr.Interface;
//using Rage;
//using RAGENativeUI.PauseMenu;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class GangTab
//{
//    private IGangRelateable Player;
//    private IPlacesOfInterest PlacesOfInterest;
//    private IShopMenus ShopMenus;
//    private IModItems ModItems;
//    private IWeapons Weapons;
//    private IGangTerritories GangTerritories;
//    private IZones Zones;

//    private string DescriptionText;

//    private TabView TabView;
//    private List<TabItem> items;
//    private bool addedItems;

//    public GangTab(IGangRelateable player, IPlacesOfInterest placesOfInterest, IShopMenus shopMenus, IModItems modItems, IWeapons weapons,IGangTerritories gangTerritories,IZones zones, TabView tabView)
//    {
//        Player = player;
//        PlacesOfInterest = placesOfInterest;
//        ShopMenus = shopMenus;
//        ModItems = modItems;
//        Weapons = weapons;
//        GangTerritories = gangTerritories;
//        Zones = zones;
//        TabView = tabView;
//    }
//    public void AddItems()
//    {
//        items = new List<TabItem>();
//        addedItems = false;
//        //foreach (GangReputation gr in Player.RelationshipManager.GangRelationships.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
//        //{
//        //    AddItem(gr);
//        //}


//        GetDirectoryLocations();

//        if (addedItems)
//        {
//            TabView.AddTab(new TabSubmenuItem("Gangs", items));
//        }
//    }


//    private List<TabItem> GetDirectoryLocations()
//    {
//        List<TabItem> items = new List<TabItem>();
//        foreach (GangReputation gr in Player.RelationshipManager.GangRelationships.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
//        {
//            List<MissionInformation> missionInfoList = new List<MissionInformation>();
//            MissionInformation locationInfo = new MissionInformation("Test", "", DirectoryInfo(gr));
//            //if (bl.HasBannerImage)
//            //{
//            //    locationInfo.Logo = new MissionLogo(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{bl.BannerImagePath}"));
//            //}
//            missionInfoList.Add(locationInfo);
//            TabMissionSelectItem basicLocationTypeList = new TabMissionSelectItem(gr.Gang.ShortName, missionInfoList);
//            items.Add(basicLocationTypeList);
//            addedItems = true;
//        }
//        return items;
//    }


//    public virtual List<Tuple<string, string>> DirectoryInfo(GangReputation gr)
//    {
//        List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
//        toreturn.Add(GetRelationship(gr));
//        toreturn.AddRange(GetDen(gr));
//        toreturn.Add(GetTerritory(gr));
//        toreturn.AddRange(GetPlayerInteraction(gr));
//        return toreturn;

//    }


//    private Tuple<string, string> GetRelationship(GangReputation gr)
//    {
//        return Tuple.Create("Relationship: ", gr.ToStringBare());
//    }
//    private List<Tuple<string, string>> GetDen(GangReputation gr)
//    {
//        List<Tuple<string, string>> denItems = new List<Tuple<string, string>>();


//        ShopMenu dealerMenu;
//        ShopMenu denMenu;
//        GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == gr.Gang.ID);
//        if (myDen != null && myDen.IsEnabled)
//        {
//            DescriptionText += $"~n~{gr.Gang.DenName}: {myDen.FullStreetAddress}"; //+ gr.ToStringBare();
//            denItems.Add(Tuple.Create("Den: ", $"{gr.Gang.DenName}"));
//            denItems.Add(Tuple.Create("Address: ", $"{myDen.FullStreetAddress}"));
//        }
//        dealerMenu = ShopMenus.GetRandomMenu(gr.Gang.DealerMenuGroup);
//        denMenu = myDen?.Menu;
//        List<string> Drugs = new List<string>();
//        List<string> Guns = new List<string>();
//        if (dealerMenu != null)
//        {
//            foreach (MenuItem mi in dealerMenu.Items)
//            {
//                ModItem modItem = ModItems.Get(mi.ModItemName);
//                if (modItem != null)
//                {
//                    if (modItem.ItemType == ItemType.Drugs)
//                    {
//                        if (!Drugs.Contains(modItem.Name))
//                        {
//                            Drugs.Add(modItem.Name);
//                        }
//                    }
//                    else if (modItem.ItemType == ItemType.Weapons)
//                    {
//                        WeaponInformation wi = Weapons.GetWeapon(modItem.ModelItem?.ModelName);
//                        if (wi != null)
//                        {
//                            if (!Guns.Contains(wi.Category.ToString()))
//                            {
//                                Guns.Add(wi.Category.ToString());
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        if (denMenu != null)
//        {
//            foreach (MenuItem mi in denMenu.Items)
//            {
//                ModItem modItem = ModItems.Get(mi.ModItemName);
//                if (modItem != null)
//                {
//                    if (modItem.ItemType == ItemType.Drugs)
//                    {
//                        if (!Drugs.Contains(modItem.Name))
//                        {
//                            Drugs.Add(modItem.Name);
//                        }
//                    }
//                    else if (modItem.ItemType == ItemType.Weapons)
//                    {
//                        WeaponInformation wi = Weapons.GetWeapon(modItem.ModelItem?.ModelName);
//                        if (wi != null)
//                        {
//                            if (!Guns.Contains(wi.Category.ToString()))
//                            {
//                                Guns.Add(wi.Category.ToString());
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        if (Drugs.Any())
//        {
//            denItems.Add(Tuple.Create("Drugs: ", $"{string.Join(", ", Drugs.OrderBy(x => x))}"));
//        }
//        if (Guns.Any())
//        {
//            denItems.Add(Tuple.Create("Guns: ", $"{string.Join(", ", Guns.OrderBy(x => x))}"));
//        }


//        return denItems;
//    }
//    private Tuple<string, string> GetTerritory(GangReputation gr)
//    {
//        string TerritoryText = "None";
//        List<ZoneJurisdiction> gangTerritory = GangTerritories.GetGangTerritory(gr.Gang.ID);
//        if (gangTerritory.Any())
//        {
//            TerritoryText = "";
//            foreach (ZoneJurisdiction zj in gangTerritory)
//            {
//                Zone myZone = Zones.GetZone(zj.ZoneInternalGameName);
//                if (myZone != null)
//                {
//                    TerritoryText += "~p~" + myZone.DisplayName + "~s~, ";
//                }
//            }
//        }
//        return Tuple.Create("Territory: ", TerritoryText.TrimEnd(' ', ','));
//        //DescriptionText += $"~n~Territory: {TerritoryText.TrimEnd(' ', ',')}";
//    }
//    private List<Tuple<string, string>> GetPlayerInteraction(GangReputation gr)
//    {
//        List<Tuple<string, string>> interactionItems = new List<Tuple<string, string>>();
//        if (Player.CellPhone.IsContactEnabled(gr.Gang.ContactName))
//        {
//            string ContactText = gr.Gang.ContactName;
//            interactionItems.Add(Tuple.Create("Contacts: ", $"{ContactText}"));
//        }
//        PlayerTask gangTask = Player.PlayerTasks.GetTask(gr.Gang.ContactName);
//        if (gangTask != null)
//        {
//            DescriptionText = $"~n~~g~Has Task~s~";
//            interactionItems.Add(Tuple.Create("Task: ", $"Active"));
//            if (gangTask.CanExpire)
//            {
//                DescriptionText += $" Complete Before: ~r~{gangTask.ExpireTime:d} {gangTask.ExpireTime:t}~s~";
//                interactionItems.Add(Tuple.Create("Date: ", $"{gangTask.ExpireTime:d} {gangTask.ExpireTime:t}"));
//            }    
//        }
//        else
//        {
//            interactionItems.Add(Tuple.Create("Task: ", $"Inactive"));
//        }
//        if (gr.MembersKilled > 0)
//        {
//            interactionItems.Add(Tuple.Create("Members Killed: ", $"{gr.MembersKilled}~s~ ({gr.MembersKilledInTerritory})"));
//        }
//        if (gr.MembersHurt > 0)
//        {
//            DescriptionText += $"~n~~o~Members Hurt~s~: {gr.MembersHurt}~s~ ({gr.MembersHurtInTerritory})";
//            interactionItems.Add(Tuple.Create("Members Hurt: ", $"{gr.MembersHurt}~s~ ({gr.MembersHurtInTerritory})"));
//        }
//        if (gr.MembersCarJacked > 0)
//        {
//            DescriptionText += $"~n~~o~Members CarJacked~s~: {gr.MembersCarJacked}~s~ ({gr.MembersCarJackedInTerritory})";
//            interactionItems.Add(Tuple.Create("Members CarJacked: ", $"{gr.MembersCarJacked}~s~ ({gr.MembersCarJackedInTerritory})"));
//        }

//        if (gr.PlayerDebt > 0)
//        {
//            string debtstring = gr.PlayerDebt.ToString("C0");
//            DescriptionText += $"~n~~r~Debt~s~: ~r~{debtstring}~s~";
//            interactionItems.Add(Tuple.Create("Debt: ", $"{debtstring}"));
//        }

//        if (gr.IsMember && Player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
//        {
//            DescriptionText += $"~n~{Player.RelationshipManager.GangRelationships.CurrentGangKickUp}";
//            interactionItems.Add(Tuple.Create("Dues: ", $"{Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount}"));
//            interactionItems.Add(Tuple.Create("Due Date: ", $"{Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueDate:g}"));
//        }
//        return interactionItems;
//    }
//}

