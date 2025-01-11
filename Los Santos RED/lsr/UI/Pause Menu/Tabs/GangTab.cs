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
    private ITimeReportable Time;
    private ISettingsProvideable Settings;

    private string DescriptionText;

    private TabView TabView;
    private List<TabItem> items;
    private bool addedItems;
    private IEntityProvideable World;
    public GangTab(IGangRelateable player, IPlacesOfInterest placesOfInterest, IShopMenus shopMenus, IModItems modItems, IWeapons weapons, IGangTerritories gangTerritories, IZones zones, TabView tabView, ITimeReportable time, ISettingsProvideable settings, IEntityProvideable world)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        ShopMenus = shopMenus;
        ModItems = modItems;
        Weapons = weapons;
        GangTerritories = gangTerritories;
        Zones = zones;
        TabView = tabView;
        Time = time;
        Settings = settings;
        World = world;
    }
    public void AddItems()
    {
        items = new List<TabItem>();
        addedItems = false;
        foreach (GangReputation gr in Player.RelationshipManager.GangRelationships.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Member).ThenByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
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


        GangDen myDen = PlacesOfInterest.GetMainDen(gr.Gang.ID, World.IsMPMapLoaded, Player.CurrentLocation);//PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == gr.Gang.ID);

        if(myDen == null)
        {
            return;
        }

        MissionLogo missionLogo = null;
        if (myDen.HasBannerImage)
        {
            missionLogo = new MissionLogo(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{myDen.BannerImagePath}"));
        }



        List<MissionInformation> gangMissionInfos = new List<MissionInformation>();

        List<Tuple<string, string>> relationshipTuples = AddRelationship(gr);
        MissionInformation RelationshipInformation = new MissionInformation("Relationship", "", relationshipTuples);
        RelationshipInformation.Logo = missionLogo;
        gangMissionInfos.Add(RelationshipInformation);


        if (myDen != null && myDen.IsEnabled)
        {
            List<Tuple<string, string>> denTuples = AddDen(gr, myDen);
            MissionInformation DenInformation = new MissionInformation(gr.Gang.DenName, "", denTuples);
            DenInformation.Logo = missionLogo;
            gangMissionInfos.Add(DenInformation);
        }

        List<Tuple<string, string>> shopTuples = AddShop(gr, myDen);
        MissionInformation shopInformation = new MissionInformation("Items", "", shopTuples);
        shopInformation.Logo = missionLogo;
        gangMissionInfos.Add(shopInformation);



        List<Tuple<string, string>> territoryTuples = Addterritory(gr);
        MissionInformation TerritoryInformation = new MissionInformation("Territory", "", territoryTuples);
        TerritoryInformation.Logo = missionLogo;
        gangMissionInfos.Add(TerritoryInformation);


        List<Tuple<string, string>> playerTuples = AddPlayerInteraction(gr);
        MissionInformation PlayerGangInformation = new MissionInformation("Interaction", "", playerTuples);
        PlayerGangInformation.Logo = missionLogo;
        gangMissionInfos.Add(PlayerGangInformation);

        TabMissionSelectItem GangList = new TabMissionSelectItem($"{gr.Gang.ShortName} {gr.ToBlip()}~s~", gangMissionInfos);
        items.Add(GangList);
        addedItems = true;
    }






    private List<Tuple<string, string>> AddRelationship(GangReputation gr)
    {
        DescriptionText += "Relationship: " + gr.ToStringBare();
        List<Tuple<string, string>> toReturn = new List<Tuple<string, string>>();
        toReturn.Add(new Tuple<string, string>("Relationship:", gr.ToStringSimple()));
        toReturn.Add(new Tuple<string, string>("Rep Level:", gr.ReputationLevel.ToString()));

        if (gr.GangLoan != null && gr.GangLoan.DueAmount > 0)
        {
            toReturn.Add(new Tuple<string, string>("Loan:", gr.GangLoan.ToString()));
        }
        return toReturn;
    }
    private List<Tuple<string, string>> AddDen(GangReputation gr, GangDen myDen)
    {
        List<Tuple<string, string>> toReturn = new List<Tuple<string, string>>();
        if (myDen != null && myDen.IsEnabled)
        {
            toReturn.AddRange(myDen.DirectoryInfo(Time.CurrentHour, Player.Character.DistanceTo2D(myDen.EntrancePosition)));
        } 
        return toReturn;
    }


    private List<Tuple<string, string>> AddShop(GangReputation gr, GangDen myDen)
    {
        List<Tuple<string, string>> toReturn = new List<Tuple<string, string>>();
        if (myDen != null)
        {
            ShopMenu dealerMenu;
            ShopMenu denMenu;
            dealerMenu = ShopMenus.GetWeightedRandomMenuFromGroup(gr.Gang.DealerMenuGroup);
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
                                if (!Guns.Contains(wi.Category.ToString()) && wi.Category != WeaponCategory.Melee && wi.Category != WeaponCategory.Throwable && wi.Category != WeaponCategory.Misc)
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
                string startString = "Drugs:";

                string drugString = string.Join(", ", Drugs.Take(5).OrderBy(x => x));

                if(Drugs.Count > 5)
                {
                    drugString += " ...";
                }

                toReturn.Add(new Tuple<string, string>(startString, ""));
                foreach (string drug in Drugs)
                {
                    toReturn.Add(new Tuple<string, string>("~o~" + drug,""));
                    //startString = "";
                }


            }
            if (Guns.Any())
            {
                string startString = "Guns:";
                string gunString = string.Join(", ", Guns.Take(5).OrderBy(x => x));


                if (Guns.Count > 5)
                {
                    gunString += " ...";
                }

                toReturn.Add(new Tuple<string, string>(startString,""));
                foreach (string gun in Guns)
                {
                    toReturn.Add(new Tuple<string, string>("~y~" + gun,""));
                    //startString = "";
                }
            }
        }
        return toReturn;
    }


    private List<Tuple<string, string>> Addterritory(GangReputation gr)
    {
        List<Tuple<string, string>> toReturn = new List<Tuple<string, string>>();
        List<ZoneJurisdiction> gangTerritory = GangTerritories.GetGangTerritory(gr.Gang.ID);
        string startString = "Zones:";

        toReturn.Add(new Tuple<string, string>(startString,""));
        startString = "";

        if (gangTerritory != null && gangTerritory.Any())
        {
            foreach (ZoneJurisdiction zj in gangTerritory)
            {
                Zone myZone = Zones.GetZone(zj.ZoneInternalGameName);
                if (myZone != null)
                {
                    toReturn.Add(new Tuple<string, string>("~p~" + myZone.DisplayName,""));
                    //startString = "";
                }
            }
        }
        return toReturn;
    }
    private List<Tuple<string, string>> AddPlayerInteraction(GangReputation gr)
    {
        List<Tuple<string, string>> toReturn = new List<Tuple<string, string>>();
        if (Player.CellPhone.IsContactEnabled(gr.Gang.ContactName))
        {
            toReturn.Add(new Tuple<string, string>("Contact:", gr.Gang.ContactName));
        }
        else
        {
            toReturn.Add(new Tuple<string, string>("Contact:", "Unknown"));
        }
        PlayerTask gangTask = Player.PlayerTasks.GetTask(gr.Gang.ContactName);
        if (gangTask != null)
        {
            toReturn.Add(new Tuple<string, string>("Task:", "~g~Active~s~"));
            if (gangTask.CanExpire)
            {
                toReturn.Add(new Tuple<string, string>("Due:", $"{gangTask.ExpireTime:g}"));
            }      
        }
        else
        {
            toReturn.Add(new Tuple<string, string>("Task:", "~y~Inactive~s~"));

        }
        toReturn.Add(new Tuple<string, string>("Members Killed:", $"{gr.MembersKilled} ({gr.MembersKilledInTerritory})"));
        toReturn.Add(new Tuple<string, string>("Members Hurt:", $"{gr.MembersHurt} ({gr.MembersHurtInTerritory})"));   
        toReturn.Add(new Tuple<string, string>("Members CarJacked:", $"{gr.MembersCarJacked} ({gr.MembersCarJackedInTerritory})"));
    
        if (gr.PlayerDebt > 0)
        {
            string debtstring = gr.PlayerDebt.ToString("C0");
            toReturn.Add(new Tuple<string, string>("Debt:", $"~r~{debtstring}~s~"));
        }

        if (gr.IsMember && Player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
        {
            toReturn.Add(new Tuple<string, string>("Kick Up:", $"${Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount}"));
            toReturn.Add(new Tuple<string, string>("Due:", $"{Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueDate:g}"));
            //toReturn.Add(new Tuple<string, string>("Currently:", $"{Time.CurrentDateTime:g}"));
        }

        return toReturn;
    }
}