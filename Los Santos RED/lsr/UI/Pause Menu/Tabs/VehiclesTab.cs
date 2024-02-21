using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehiclesTab
{
    private IGangRelateable Player;
    private IStreets Streets;
    private IZones Zones;
    private IInteriors Interiors;
    private TabView TabView;
    private ISettingsProvideable Settings;

    public VehiclesTab(IGangRelateable player,IStreets streets, IZones zones, IInteriors interiors, TabView tabView, ISettingsProvideable settings)
    {
        Player = player;
        Streets = streets;
        Zones = zones;
        Interiors = interiors;
        TabView = tabView;
        Settings = settings;
    }
    public void AddItems()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;
        foreach (VehicleExt car in Player.VehicleOwnership.OwnedVehicles)
        {
            Color carColor = car.VehicleColor();//should match with the others....
            string Make = car.MakeName();
            string Model = car.ModelName();
            string PlateText = car.CarPlate?.PlateNumber;
            string VehicleName = "";

            string hexColor = ColorTranslator.ToHtml(Color.FromArgb(carColor.ToArgb()));
            string ColorizedColorName = carColor.Name;
            if (carColor.ToString() != "")
            {
                ColorizedColorName = $"<FONT color='{hexColor}'>" + carColor.Name + "~s~";
                VehicleName += ColorizedColorName;
            }






            string rightText = "";
            if (car.CarPlate != null && car.CarPlate.IsWanted)
            {
                rightText = " ~r~(Wanted)~s~";
            }

            string DescriptionText = $"~n~Color: {ColorizedColorName}";
            DescriptionText += $"~n~Make: {Make}";
            DescriptionText += $"~n~Model: {Model}";
            DescriptionText += $"~n~Plate: {PlateText} {rightText}";

            string ListEntryText = $"{ColorizedColorName} {Make} {Model} ({PlateText})";
            string DescriptionHeaderText = $"{Model}";
            if (car.Vehicle.Exists())
            {
                LocationData myData = new LocationData(car.Vehicle, Streets, Zones, Interiors, Settings);
                myData.Update(car.Vehicle, false);

                string StreetText = "";
                if (myData.CurrentStreet != null)
                {
                    StreetText += $"~y~{myData.CurrentStreet.ProperStreetName}~s~";
                    if (myData.CurrentCrossStreet != null)
                    {
                        StreetText += $" at ~y~{myData.CurrentCrossStreet.ProperStreetName}~s~";
                    }
                }
                string ZoneText = "";
                if (myData.CurrentZone != null)
                {
                    ZoneText = $" {(myData.CurrentZone.IsSpecificLocation ? "near" : "in")} ~p~{myData.CurrentZone.FullZoneName(Settings)}~s~";
                }
                string LocationText = $"{StreetText} {ZoneText}".Trim();
                LocationText = LocationText.Trim();

                DescriptionText += $"~n~Location: {LocationText}";
            }

            DescriptionText += $"~n~Select to ~r~Clear Ownership~s~";

            TabItem tItem = new TabTextItem(ListEntryText, DescriptionHeaderText, DescriptionText);
            tItem.Activated += (s, e) =>
            {
                TabView.Visible = false;
                Game.IsPaused = false;
                Player.VehicleOwnership.RemoveOwnershipOfVehicle(car);
            };
            items.Add(tItem);
            addedItems = true;
        }
        if (addedItems)
        {
            TabView.AddTab(new TabSubmenuItem("Vehicles", items));
        }
        else
        {
            TabView.AddTab(new TabItem("Vehicles"));
        }
    }
}

