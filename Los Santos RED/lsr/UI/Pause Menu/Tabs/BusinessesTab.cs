using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Windows.Media;
using static DispatchScannerFiles;


public class BusinessesTab : ITabbableMenu
{
    private ILocationInteractable Player;
    private TabSubmenuItem TabItem;
    private TabView TabView;
    private List<TabItem> items;
    //public BusinessesTab(ILocationInteractable player, TabView tabView)
    //{
    //    Player = player;
    //    TabView = tabView;
    //}
    public void AddItems()
    {

    }
    //public void AddItems()
    //{
    //    items = new List<TabItem>();
    //    bool canFocusTabItem = false;
    //    if (Player.Properties.Businesses.Count == 0 && Player.Properties.PayoutProperties.Count == 0)
    //    {
    //        TabTextItem ttx = new TabTextItem("No businesses", "", "You do not own any businesses.");
    //        ttx.CanBeFocused = false;
    //        items.Add(ttx);
    //    }
    //    else
    //    {
    //        foreach (Business business in Player.Properties.Businesses)
    //        {
    //            MissionLogo missionLogo = null;
    //            if (business.HasBannerImage)
    //            {
    //                missionLogo = new MissionLogo(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{business.BannerImagePath}"));
    //            }
    //            List<MissionInformation> propertyInfos = new List<MissionInformation>();
    //            List<Tuple<string, string>> financialTuples = AddFinancials(business);
    //            financialTuples.Add(Tuple.Create<string, string>("Mode of Payment", business.IsPayoutInModItems ? business.ModItemToPayout : "Cash"));
    //            MissionInformation financialInformation = new MissionInformation("Financials", "", financialTuples);
    //            financialInformation.Logo = missionLogo;
    //            propertyInfos.Add(financialInformation);
    //            List<Tuple<string, string>> storageTuples = new List<Tuple<string, string>>();
    //            foreach (InventoryItem item in business.SimpleInventory.ItemsList)
    //            {
    //                storageTuples.Add(Tuple.Create<string, string>(item.ModItem.Name, item.Amount.ToString()));
    //            }
    //            storageTuples.Add(Tuple.Create<string, string>("Cash Storage", $"${business.CashStorage.StoredCash}"));
    //            MissionInformation storageInformation = new MissionInformation("Storage", "", storageTuples);
    //            storageInformation.Logo = missionLogo;
    //            propertyInfos.Add(storageInformation);
    //            List<Tuple<string, string>> gpsTuple = AddGPS(business);
    //            MissionInformation gpsInformation = new MissionInformation("GPS", "", gpsTuple);
    //            gpsInformation.Logo = missionLogo;
    //            propertyInfos.Add(gpsInformation);
    //            TabMissionSelectItem BusinessItem = new TabMissionSelectItem($"{business.Name} - {business.ZoneName}", propertyInfos);
    //            BusinessItem.OnItemSelect += (selectedItem) =>
    //            {
    //                if (selectedItem != null && selectedItem.Name == "GPS")
    //                {
    //                    Player.GPSManager.AddGPSRoute(business.Name, business.EntrancePosition, true);
    //                }
    //            };
    //            items.Add(BusinessItem);
    //        }
    //        foreach (GameLocation business in Player.Properties.PayoutProperties)
    //        {
    //            MissionLogo missionLogo = null;
    //            if (business.HasBannerImage)
    //            {
    //                missionLogo = new MissionLogo(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{business.BannerImagePath}"));
    //            }
    //            List<MissionInformation> propertyInfos = new List<MissionInformation>();
    //            List<Tuple<string, string>> financialTuples = AddFinancials(business);
    //            MissionInformation financialInformation = new MissionInformation("Financials", "", financialTuples);
    //            financialInformation.Logo = missionLogo;
    //            propertyInfos.Add(financialInformation);
    //            List<Tuple<string, string>> gpsTuple = AddGPS(business);
    //            MissionInformation gpsInformation = new MissionInformation("GPS", "", gpsTuple);
    //            gpsInformation.Logo = missionLogo;
    //            propertyInfos.Add(gpsInformation);
    //            TabMissionSelectItem BusinessItem = new TabMissionSelectItem($"{business.Name} - {business.ZoneName}", propertyInfos);
    //            BusinessItem.OnItemSelect += (selectedItem) =>
    //            {
    //                if (selectedItem != null && selectedItem.Name == "GPS")
    //                {
    //                    Player.GPSManager.AddGPSRoute(business.Name, business.EntrancePosition, true);
    //                }
    //            };
    //            items.Add(BusinessItem);
    //        }
    //        canFocusTabItem = true;
    //    }
    //    TabItem = new TabSubmenuItem("Businesses", items);
    //    TabItem.CanBeFocused = canFocusTabItem;
    //    TabView.AddTab(TabItem);
    //}
    //private List<Tuple<string, string>> AddFinancials(GameLocation gameLocation)
    //{
    //    List<Tuple<string,string>> toAdd = new List<Tuple<string,string>>();
    //    toAdd.Add(Tuple.Create<string, string>("Sell Price", $"${gameLocation.CurrentSalesPrice}"));
    //    toAdd.Add(Tuple.Create<string, string>("Payment Due", gameLocation.DatePayoutDue.ToString("dd-MMM-yyyy")));
    //    return toAdd;
    //}
    //private List<Tuple<string, string>> AddGPS(GameLocation gameLocation)
    //{
    //    List<Tuple<string, string>> toAdd = new List<Tuple<string, string>>();
    //    toAdd.Add(Tuple.Create<string, string>("GPS", gameLocation.StreetAddress));
    //    return toAdd;
    //}

}
