using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using static DispatchScannerFiles;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;

public class ModItems : IModItems
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\ModItems.xml";

    private readonly float TinyHungerRecover = 5f;
    private readonly float SmallHungerRecover = 15f;
    private readonly float MediumHungerRecover = 35f;
    private readonly float LargeHungerRecover = 55f;
    private readonly float HugeHungerRecover = 75f;
    private readonly float FullHungerRecover = 100f;

    private readonly float TinyThirstRecover = 5f;
    private readonly float SmallThirstRecover = 15f;
    private readonly float MediumThirstRecover = 35f;
    private readonly float LargeThirstRecover = 55f;
    private readonly float HugeThirstRecover = 75f;
    private readonly float FullThirstRecover = 100f;

    private readonly float TinySleepRecover = 5f;
    private readonly float SmallSleepRecover = 15f;
    private readonly float MediumSleepRecover = 35f;
    private readonly float LargeSleepRecover = 55f;
    private readonly float HugeSleepRecover = 75f;
    private readonly float FullSleepRecover = 100f;

    private readonly int TinyHealthRecover = 5;
    private readonly int SmallHealthRecover = 15;
    private readonly int MediumHealthRecover = 35;
    private readonly int LargeHealthRecover = 55;
    private readonly int HugeHealthRecover = 75;
    private readonly int FullHealthRecover = 100;

    public ModItems()
    {
        PossibleItems = new PossibleItems();
    }
    public PossibleItems PossibleItems { get; private set; }
    public ModItem Get(string name)
    {
        return AllItems().FirstOrDefault(x => x.Name == name);
    }
    public WeaponItem GetWeapon(uint modelHash)
    {
        return PossibleItems.WeaponItems.FirstOrDefault(x => x.ModelHash == modelHash);
    }
    public WeaponItem GetWeapon(string modelName)
    {
        return PossibleItems.WeaponItems.FirstOrDefault(x => x.ModelName.ToLower() == modelName.ToLower());
    }
    public VehicleItem GetVehicle(uint modelHash)
    {
        return PossibleItems.VehicleItems.FirstOrDefault(x => x.ModelHash == modelHash);
    }
    public VehicleItem GetVehicle(string modelName)
    {
        return PossibleItems.VehicleItems.FirstOrDefault(x => x.ModelName.ToLower() == modelName.ToLower());
    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("ModItems*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Mod Items config: {ConfigFile.FullName}",0);
            PossibleItems = Serialization.DeserializeParam<PossibleItems>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Mod Items config  {ConfigFileName}",0);
            PossibleItems = Serialization.DeserializeParam<PossibleItems>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Mod Items config found, creating default", 0);
            DefaultConfig();
            DefaultConfig_FullModernTraffic();
            DefaultConfig_FullExpandedJurisdiction();
            DefaultConfig_FullExpandedExperience();
            DefaultConfig_LosSantos2008();
        }
    }
    private void DefaultConfig_FullExpandedJurisdiction()
    {
        PossibleItems newPossibleItems = PossibleItems.Copy();

        //Taxi Service 
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Interceptor Service", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "servinterceptor" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Stanier Service", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "servstanier2" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Karin Vivanite Taxi", true, ItemType.Vehicles) { OverrideMakeName = "Karin", ModelName = "taxvivaniteliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Karin Dilettante Service", true, ItemType.Vehicles) { OverrideMakeName = "Karin", ModelName = "servdilettante" });

        //Fire/EMS
        newPossibleItems.VehicleItems.Add(new VehicleItem("MTL Fire Truck (Livery)", ItemType.Vehicles) { ModelName = "firetrukliv", OverrideMakeName = "MTL", OverrideClassName = "Utility", });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Brute Ambulance (Livery)", ItemType.Vehicles) { ModelName = "ambulanceliv", OverrideMakeName = "Brute", OverrideClassName = "Van", });

        //Mil
        newPossibleItems.VehicleItems.RemoveAll(x => x.ModelName == "insurgent3");
        newPossibleItems.VehicleItems.Add(new VehicleItem("Mammoth Squaddie Armed", true, ItemType.Vehicles) { ModelName = "insurgent3", Description = "Long gone are the days of playing army men with other kids. Now you play army men with grownups. And what's more grownup than giving each other a high and tight before piling into an armored truck, stripping to the waist and making revving noises? It's just like the old days, but your mom's not there to make you snacks.", });


        //Heli
        newPossibleItems.VehicleItems.Add(new VehicleItem("Maibatsu Frogger Police", true, ItemType.Vehicles) { OverrideMakeName = "Maibatsu", ModelName = "polfroggerliv", Description = "Stylish, roomy, easy to handle with a cruise speed of 130 knots, this 4-seat single-engine light helicopter is popular with both private pilots and charter companies." });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Buckingham Maverick Police (Livery)", ItemType.Vehicles) { ModelName = "polmavliv", OverrideMakeName = "Buckingham", OverrideClassName = "Helicopter", });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Western Annihilator", ItemType.Vehicles) { ModelName = "annihilatorliv", OverrideMakeName = "Western", OverrideClassName = "Helicopter", });


        //Police
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Aleutian Police", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "polaleutianliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Bravado Bison Police", true, ItemType.Vehicles) { OverrideMakeName = "Bravado", ModelName = "polbisonliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Bravado Buffalo S Police", true, ItemType.Vehicles) { OverrideMakeName = "Bravado", ModelName = "polbuffalosliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Bravado Buffalo STX Police", true, ItemType.Vehicles) { OverrideMakeName = "Bravado", ModelName = "polbuffalostxliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Caracara Police", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "polcaracaraliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Cheval Fugitive Police", true, ItemType.Vehicles) { OverrideMakeName = "Cheval", ModelName = "polfugitiveliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Declasse Granger 3600 Police", true, ItemType.Vehicles) { OverrideMakeName = "Declasse", ModelName = "polgranger3600liv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Bravado Gresley Police", true, ItemType.Vehicles) { OverrideMakeName = "Bravado", ModelName = "polgresleyliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Maibatsu Kuruma Police", true, ItemType.Vehicles) { OverrideMakeName = "Maibatsu", ModelName = "polkurumaunmarked" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Dundreary Landstalker XL Police", true, ItemType.Vehicles) { OverrideMakeName = "Dundreary", ModelName = "pollandstalkerxlliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Ubermacht Oracle Police", true, ItemType.Vehicles) { OverrideMakeName = "Ubermacht", ModelName = "poloracleliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Radius Police", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "polradiusliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Ubermacht Rebla GTS Police", true, ItemType.Vehicles) { OverrideMakeName = "Ubermacht", ModelName = "polreblagtsliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Riata Police", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "polriataliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Maibatsu Sanchez Police", true, ItemType.Vehicles) { OverrideMakeName = "Maibatsu", ModelName = "polsanchezliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Canis Terminus Police", true, ItemType.Vehicles) { OverrideMakeName = "Canis", ModelName = "polterminusliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Dinka Thrust Police", true, ItemType.Vehicles) { OverrideMakeName = "Dinka", ModelName = "polthrustliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Dinka Verus Police", true, ItemType.Vehicles) { OverrideMakeName = "Dinka", ModelName = "polverusliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Dinka Vindicator Police", true, ItemType.Vehicles) { OverrideMakeName = "Dinka", ModelName = "polvindicatorliv" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Albany STR Police", true, ItemType.Vehicles) { OverrideMakeName = "Albany", ModelName = "polvstrliv" });
        
        //Serialization.SerializeParam(newPossibleItems, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\ModItems_FullExpandedJurisdiction.xml");
    }
    private void DefaultConfig_FullModernTraffic()
    {
        PossibleItems newPossibleItems = PossibleItems.Copy();

        //Sedans
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Interceptor", ItemType.Vehicles) { OverrideMakeName = "Vapid", OverrideClassName = "Sedan", ModelName = "civinterceptor", Description = "The civilian version of the police classic. So what if they couldn't sell it to law enforcement? It still can get you to Burger Shot without breaking down. Often.", });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Stanier 2nd Gen", ItemType.Vehicles) { OverrideMakeName = "Vapid",OverrideClassName = "Sedan", ModelName = "civstanier2", Description = "The remix of a classic. As heavy and slow as before, now with worse quality control. We'll make up for it in fleet sales.", }); newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Stanier 2nd Gen", ItemType.Vehicles) { ModelName = "tornado3", Description = "The remix of a classic. As heavy and slow as before, now with worse quality control. We'll make up for it in fleet sales.", });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Albany Esperanto", true, ItemType.Vehicles) { OverrideMakeName = "Albany", OverrideClassName = "Sedan", ModelName = "civesperanto", Description = "Heavy, slow, and full of chrome. See why this was one of the the top police vehicles.... 40 years ago." });
        newPossibleItems.VehicleItems.RemoveAll(x => x.ModelName == "kuruma");
        newPossibleItems.VehicleItems.Add(new VehicleItem("Maibatsu Kuruma", ItemType.Vehicles) { OverrideMakeName = "Maibatsu", OverrideClassName = "Sedan", ModelName = "kuruma", Description = "The perfect car to go with your flesh tunnel earrings, frosted spikes, and oversize jeans. Buy this and you'll never fail to be mistaken for a small town drug dealer again.", });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Declasse Merit", ItemType.Vehicles) { OverrideMakeName = "Declasse", OverrideClassName = "Sedan", ModelName = "civmerit" });


        newPossibleItems.VehicleItems.Add(new VehicleItem("Schyster PMP 600", ItemType.Vehicles) { OverrideMakeName = "Schyster", OverrideClassName = "Sedan", ModelName = "civpmp600" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Albany Presidente", ItemType.Vehicles) { OverrideMakeName = "Albany", OverrideClassName = "Sedan", ModelName = "civpresidente" });

        //Trucks
        newPossibleItems.VehicleItems.RemoveAll(x => x.ModelName == "caracara");
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Caracara Service", ItemType.Vehicles) { ModelName = "caracara" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Caracara 4x2", ItemType.Vehicles) { ModelName = "civcaracarawork" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Bobcat 4x4", ItemType.Vehicles) { OverrideMakeName = "Vapid", OverrideClassName = "Pickup", ModelName = "civbobcatoffroad" });
        newPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Bobcat Regular Bed", ItemType.Vehicles) { OverrideMakeName = "Vapid", OverrideClassName = "Pickup", ModelName = "civbobcatwork" });
        newPossibleItems.VehicleItems.RemoveAll(x => x.ModelName == "contender");
        newPossibleItems.VehicleItems.Add(new VehicleItem("Karin Everon V8", true, ItemType.Vehicles) { OverrideMakeName = "Karin", ModelName = "contender" });//swapped from vaid to KARIN and renamed

        newPossibleItems.VehicleItems.Add(new VehicleItem("Canis Bodhi Mod", ItemType.Vehicles) { OverrideMakeName = "Canis", ModelName = "bodhi", Description = "The Canis Bodhi has traveled the well-trodden path from military to redneck to hipster." });

        //HELIS
        newPossibleItems.VehicleItems.Add(new VehicleItem("Buckingham Maverick 2nd Gen", true, ItemType.Vehicles) { OverrideMakeName = "Buckingham", ModelName = "civmaverick2" });//civ 2nd gen mav

        Serialization.SerializeParam(newPossibleItems, "Plugins\\LosSantosRED\\AlternateConfigs\\FullModernTraffic\\Variations\\Full Modern Traffic\\ModItems_FullModernTraffic.xml");
    }
    private void DefaultConfig_LosSantos2008()
    {
        PossibleItems oldPossibleItems = PossibleItems.Copy();

        //Civilian
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Albany Esperanto", true, ItemType.Vehicles) { OverrideMakeName = "Albany", OverrideClassName = "Sedan", ModelName = "civesperanto", Description = "Heavy, slow, and full of chrome. See why this was one of the the top police vehicles.... 40 years ago." });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Declasse Merit", ItemType.Vehicles) { OverrideMakeName = "Declasse", OverrideClassName = "Sedan", ModelName = "civmerit" });

        //Service Taxi
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Minivan Taxi", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "servminivan" });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Stanier 1st Gen Service", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "servstanierold" });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Karin Dilettante Service", true, ItemType.Vehicles) { OverrideMakeName = "Karin", ModelName = "servdilettante" });

        //Police
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Western Police Bike (Livery)", ItemType.Vehicles) { ModelName = "policebikeold", OverrideMakeName = "Western", OverrideClassName = "Motorcycle", });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Albany Esperanto Police", true, ItemType.Vehicles) { OverrideMakeName = "Albany", OverrideClassName = "Sedan", ModelName = "polesperantoliv" });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Buckingham Maverick Classic Police", true, ItemType.Vehicles) { OverrideMakeName = "Buckingham", ModelName = "polmaverickoldliv" });//police 1st gen mav
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Declasse Merit Police", true, ItemType.Vehicles) { OverrideMakeName = "Declasse", ModelName = "polmeritliv" });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Albany Washington Police", true, ItemType.Vehicles) { OverrideMakeName = "Albany", ModelName = "polwashingtonunmarked" });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Vapid Stanier 1st Gen Police", true, ItemType.Vehicles) { OverrideMakeName = "Vapid", ModelName = "polstanieroldliv" });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Mammoth Patriot P2 Police", true, ItemType.Vehicles) { OverrideMakeName = "Mammoth", ModelName = "polpatriotliv" });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Canis Seminole Police", true, ItemType.Vehicles) { OverrideMakeName = "Canis", ModelName = "polseminoleliv" });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Declasse Police Transporter (Livery)", ItemType.Vehicles) { ModelName = "poltransporterliv", OverrideMakeName = "Declasse", OverrideClassName = "Van", });
        oldPossibleItems.VehicleItems.Add(new VehicleItem("Cheval Fugitive Police", true, ItemType.Vehicles) { OverrideMakeName = "Cheval", ModelName = "polfugitiveliv" });


        Serialization.SerializeParam(oldPossibleItems, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\ModItems_LosSantos2008.xml");
    }
    private void DefaultConfig_FullExpandedExperience()
    {
        PossibleItems newPossibleItems = PossibleItems.Copy();
        DrinkItem ecolaCan = newPossibleItems.DrinkItems.FirstOrDefault(x => x.ModelItemID == "ng_proc_sodacan_01a");
        if (ecolaCan != null)
        {
            ecolaCan.ModelItemID = "prop_ecolacan_01a";
        }
        DrinkItem sprunkCan = newPossibleItems.DrinkItems.FirstOrDefault(x => x.ModelItemID == "ng_proc_sodacan_01b");
        if (sprunkCan != null)
        {
            sprunkCan.ModelItemID = "prop_sprunkcan_01a";
        }
        FoodItem strawRails = newPossibleItems.FoodItems.FirstOrDefault(x => x.Name == "Strawberry Rails Cereal");
        if(strawRails != null)
        {
            strawRails.ModelItemID = "prop_strawberryrailsbox_01a";
        }
        newPossibleItems.DrinkItems.AddRange(new List<DrinkItem>
        {
            new DrinkItem("Can of PiBwasser", "Cheap 11% ABV fighting lager brewed in Germany for export only from rice, barley, hops and the fresh urine of Bavarian virgins", ItemType.Drinks){
                ModelItemID = "prop_pisswassercan_01a",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = 5,
                SleepChangeAmount = -2.0f,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = 5.0f,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5 },
            new DrinkItem("Can of PiBwasser ICE", "Cheap 11% ABV fighting lager brewed in Germany for export only from rice, barley, hops and the fresh urine of Bavarian virgins", ItemType.Drinks){
                ModelItemID = "prop_pisswassercan_01b",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = 5,
                SleepChangeAmount = -2.0f,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = 5.0f,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5 },
        });
        //Serialization.SerializeParam(newPossibleItems, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedExperience\\ModItems_FullExpandedExperience.xml");
    }

    public void WriteToFile()
    {
        Serialization.SerializeParam(PossibleItems, ConfigFileName);
    }
    public List<ModItem> AllItems()
    {
        List<ModItem> AllItems = new List<ModItem>();
        AllItems.AddRange(PossibleItems.FlashlightItems);
        AllItems.AddRange(PossibleItems.CellphoneItems);
        AllItems.AddRange(PossibleItems.ShovelItems);
        AllItems.AddRange(PossibleItems.LicensePlateItems);
        AllItems.AddRange(PossibleItems.UmbrellaItems);
        AllItems.AddRange(PossibleItems.FoodItems);
        AllItems.AddRange(PossibleItems.SmokeItems);
        AllItems.AddRange(PossibleItems.DrinkItems);
        AllItems.AddRange(PossibleItems.PipeSmokeItems);
        AllItems.AddRange(PossibleItems.IngestItems);
        AllItems.AddRange(PossibleItems.InhaleItems);
        AllItems.AddRange(PossibleItems.InjectItems);
        AllItems.AddRange(PossibleItems.VehicleItems);
        AllItems.AddRange(PossibleItems.WeaponItems);
        AllItems.AddRange(PossibleItems.HotelStayItems);
        AllItems.AddRange(PossibleItems.DrillItems);
        AllItems.AddRange(PossibleItems.TapeItems);
        AllItems.AddRange(PossibleItems.ScrewdriverItems);
        AllItems.AddRange(PossibleItems.LighterItems);
        AllItems.AddRange(PossibleItems.PliersItems);
        AllItems.AddRange(PossibleItems.HammerItems);
        AllItems.AddRange(PossibleItems.BongItems);
        AllItems.AddRange(PossibleItems.PipeItems);
        AllItems.AddRange(PossibleItems.RollingPapersItems);
        AllItems.AddRange(PossibleItems.BinocularsItems);
        AllItems.AddRange(PossibleItems.RadioItems);
        AllItems.AddRange(PossibleItems.ValuableItems);
        AllItems.AddRange(PossibleItems.EquipmentItems);
        AllItems.AddRange(PossibleItems.BodyArmorItems);
        AllItems.AddRange(PossibleItems.RadarDetectorItems);
        return AllItems;
    }
    public List<ModItem> PropItems()
    {
        List<ModItem> AllItems = new List<ModItem>();
        AllItems.AddRange(PossibleItems.FlashlightItems);
        AllItems.AddRange(PossibleItems.CellphoneItems);
        AllItems.AddRange(PossibleItems.ShovelItems);
        AllItems.AddRange(PossibleItems.LicensePlateItems);
        AllItems.AddRange(PossibleItems.UmbrellaItems);
        AllItems.AddRange(PossibleItems.FoodItems);
        AllItems.AddRange(PossibleItems.SmokeItems);
        AllItems.AddRange(PossibleItems.DrinkItems);
        AllItems.AddRange(PossibleItems.PipeSmokeItems);
        AllItems.AddRange(PossibleItems.IngestItems);
        AllItems.AddRange(PossibleItems.InhaleItems);
        AllItems.AddRange(PossibleItems.InjectItems);
       // AllItems.AddRange(PossibleItems.HotelStayItems);
        AllItems.AddRange(PossibleItems.DrillItems);
        AllItems.AddRange(PossibleItems.TapeItems);
        AllItems.AddRange(PossibleItems.ScrewdriverItems);
        AllItems.AddRange(PossibleItems.LighterItems);
        AllItems.AddRange(PossibleItems.PliersItems);
        AllItems.AddRange(PossibleItems.HammerItems);
        AllItems.AddRange(PossibleItems.BongItems);
        AllItems.AddRange(PossibleItems.PipeItems);
        AllItems.AddRange(PossibleItems.RollingPapersItems);
        AllItems.AddRange(PossibleItems.BinocularsItems);
        AllItems.AddRange(PossibleItems.RadioItems);
        AllItems.AddRange(PossibleItems.RadarDetectorItems);
        return AllItems;
    }
    public List<ModItem> PossibleFoundItems()
    {
        List<ModItem> AllItems = new List<ModItem>();
        AllItems.AddRange(PossibleItems.FlashlightItems);
        AllItems.AddRange(PossibleItems.CellphoneItems);
        AllItems.AddRange(PossibleItems.ShovelItems);
        AllItems.AddRange(PossibleItems.UmbrellaItems);
        AllItems.AddRange(PossibleItems.FoodItems);
        AllItems.AddRange(PossibleItems.SmokeItems);
        AllItems.AddRange(PossibleItems.DrinkItems);
        AllItems.AddRange(PossibleItems.PipeSmokeItems);
        AllItems.AddRange(PossibleItems.IngestItems);
        AllItems.AddRange(PossibleItems.InhaleItems);
        AllItems.AddRange(PossibleItems.InjectItems);
        AllItems.AddRange(PossibleItems.DrillItems);
        AllItems.AddRange(PossibleItems.TapeItems);
        AllItems.AddRange(PossibleItems.ScrewdriverItems);
        AllItems.AddRange(PossibleItems.LighterItems);
        AllItems.AddRange(PossibleItems.PliersItems);
        AllItems.AddRange(PossibleItems.HammerItems);
        AllItems.AddRange(PossibleItems.BongItems);
        AllItems.AddRange(PossibleItems.PipeItems);
        AllItems.AddRange(PossibleItems.RollingPapersItems);
        AllItems.AddRange(PossibleItems.BinocularsItems);
        AllItems.AddRange(PossibleItems.RadioItems);
        AllItems.AddRange(PossibleItems.RadarDetectorItems);
        return AllItems.Where(x => x.FindPercentage > 0).ToList();
    }
    public List<WeaponItem> PossibleFoundWeapons()
    {
        List<WeaponItem> AllItems = new List<WeaponItem>();
        AllItems.AddRange(PossibleItems.WeaponItems);
        return AllItems.Where(x => x.FindPercentage > 0).ToList();
    }
    public List<ModItem> InventoryItems()
    {
        List<ModItem> AllItems = new List<ModItem>();
        AllItems.AddRange(PossibleItems.FlashlightItems);
        AllItems.AddRange(PossibleItems.CellphoneItems);
        AllItems.AddRange(PossibleItems.ShovelItems);
        AllItems.AddRange(PossibleItems.UmbrellaItems);
        AllItems.AddRange(PossibleItems.FoodItems);
        AllItems.AddRange(PossibleItems.SmokeItems);
        AllItems.AddRange(PossibleItems.DrinkItems);
        AllItems.AddRange(PossibleItems.PipeSmokeItems);
        AllItems.AddRange(PossibleItems.IngestItems);
        AllItems.AddRange(PossibleItems.InhaleItems);
        AllItems.AddRange(PossibleItems.InjectItems);
        AllItems.AddRange(PossibleItems.DrillItems);
        AllItems.AddRange(PossibleItems.TapeItems);
        AllItems.AddRange(PossibleItems.ScrewdriverItems);
        AllItems.AddRange(PossibleItems.LighterItems);
        AllItems.AddRange(PossibleItems.PliersItems);
        AllItems.AddRange(PossibleItems.HammerItems);
        AllItems.AddRange(PossibleItems.BongItems);
        AllItems.AddRange(PossibleItems.PipeItems);
        AllItems.AddRange(PossibleItems.RollingPapersItems);
        AllItems.AddRange(PossibleItems.BinocularsItems);
        AllItems.AddRange(PossibleItems.RadioItems);
        AllItems.AddRange(PossibleItems.ValuableItems);
        AllItems.AddRange(PossibleItems.EquipmentItems);
        AllItems.AddRange(PossibleItems.BodyArmorItems);
        AllItems.AddRange(PossibleItems.RadarDetectorItems);
        return AllItems;
    }
    public void Setup(PhysicalItems physicalItems, IWeapons weapons, IIntoxicants intoxicants, ICellphones cellphones)
    {
        foreach(ModItem modItem in AllItems())
        {
            modItem.Setup(physicalItems, weapons, intoxicants);
        }
        foreach(CellphoneItem cell in PossibleItems.CellphoneItems)
        {
            if(cellphones.GetPhone(cell.Name) == null)
            {
                cell.FindPercentage = 0;
                EntryPoint.WriteToConsole($"REMOVING CELLPHONE {cell.Name} FIND PERCENTAGE NO MATCHING VALUE IN CELLPHONES.XML");
            }
        }
    }
    public ModItem GetRandomItem(bool allowIllegal, bool allowCellphones)// List<string> RequiredModels)
    {
        List<ModItem> ToPickFrom = PossibleFoundItems();
        if(!allowCellphones)
        {
            ToPickFrom.RemoveAll(x => x.ItemType == ItemType.Equipment && x.ItemSubType == ItemSubType.CellPhone);
        }
        if(!allowIllegal)
        {
            ToPickFrom.RemoveAll(x => x.IsPossessionIllicit);
        }
        int Total = ToPickFrom.Sum(x => x.FindPercentage);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (ModItem modItem in ToPickFrom)
        {
            int SpawnChance = modItem.FindPercentage;
            if (RandomPick < SpawnChance)
            {
                return modItem;
            }
            RandomPick -= SpawnChance;       
        }
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom();
        }
        return null;
    }
    public WeaponItem GetRandomWeapon(bool allowIllegal)// List<string> RequiredModels)
    {
        List<WeaponItem> ToPickFrom = PossibleFoundWeapons();
        int Total = ToPickFrom.Sum(x => x.FindPercentage);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (WeaponItem modItem in ToPickFrom)
        {
            int SpawnChance = modItem.FindPercentage;
            if (RandomPick < SpawnChance)
            {
                return modItem;
            }
            RandomPick -= SpawnChance;
        }
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom();
        }
        return null;
    }
    private void DefaultConfig()
    {
        DefaultConfig_Drinks();
        DefaultConfig_Food();
        DefaultConfig_Drugs();
        DefaultConfig_Weapons();
        DefaultConfig_Armor();
        DefaultConfig_Paraphernalia();
        DefaultConfig_Tools();
        DefaultConfig_Vehicles();
        DefaultConfig_Services();
        DefaultConfig_Valuables();
        DefaultConfig_FEE();
        Serialization.SerializeParam(PossibleItems, ConfigFileName);
    }
    private void DefaultConfig_FEE()
    {

    }
    private void DefaultConfig_Armor()
    {
        PossibleItems.BodyArmorItems.AddRange(new List<BodyArmorItem>
        {
            new BodyArmorItem("Light Body Armor","It's like you're wearing nothing at all!", ItemType.Equipment) {
                ItemSubType = ItemSubType.BodyArmor, ArmorChangeAmount = 50,PackageItemID = "prop_armour_pickup" },
            new BodyArmorItem("Medium Body Armor","Actually offers protection instead of just peace of mind..", ItemType.Equipment) {
                ItemSubType = ItemSubType.BodyArmor,ArmorChangeAmount = 100,PackageItemID = "prop_armour_pickup" },
            new BodyArmorItem("Heavy Body Armor","Planning on entering a warzone?", ItemType.Equipment) {
                ItemSubType = ItemSubType.BodyArmor,ArmorChangeAmount = 150,PackageItemID = "prop_armour_pickup" },
            new BodyArmorItem("Full Body Armor","For the bullet hypochondriacs", ItemType.Equipment) {
                ItemSubType = ItemSubType.BodyArmor,ArmorChangeAmount = 400,PackageItemID = "prop_armour_pickup" },
        });


        PossibleItems.EquipmentItems.AddRange(new List<EquipmentItem>
        {
            new EquipmentItem("Health Pack","Is this real life or a video game?", ItemType.Equipment) {
                ItemSubType = ItemSubType.HealthPack, AlwaysChangesHealth = true, HealthChangeAmount = 75,PackageItemID = "prop_ld_health_pack" },
        });
    }
    private void DefaultConfig_Valuables()
    {
        PossibleItems.ValuableItems.AddRange(new List<ValuableItem>
        {
            new ValuableItem("Fake Gold Ring","As real as the stars in Vinewood.", ItemType.Jewelry) {
                FindPercentage = 5,ItemSubType = ItemSubType.Ring },
            new ValuableItem("Gold Ring","No hobbits around.", ItemType.Jewelry) {
                FindPercentage = 1,ItemSubType = ItemSubType.Ring },
            new ValuableItem("Fake Silver Ring","It's fakes all the way down.", ItemType.Jewelry) {
                FindPercentage = 6,ItemSubType = ItemSubType.Ring },
            new ValuableItem("Silver Ring","Whats wrong with second place?", ItemType.Jewelry) {
                FindPercentage = 5,ItemSubType = ItemSubType.Ring },


            new ValuableItem("Cash Bundle","Cash", ItemType.Valuables) { ModelItemID = "prop_cash_pile_02",
                ItemSubType = ItemSubType.Money },



            new ValuableItem("Marked Cash Stack","Stack of marked cash", ItemType.Valuables) { ModelItemID = "prop_cash_pile_02",
                ItemSubType = ItemSubType.Money },


            new ValuableItem("Drivers License","Drivers License", ItemType.Valuables) { 
                ModelItemID = "p_ld_id_card_002",
                ItemSubType = ItemSubType.Identification },

            new ValuableItem("Police ID Card","Police Identification Card", ItemType.Valuables) {
                ModelItemID = "p_ld_id_card_01",
                ItemSubType = ItemSubType.Identification },
            new ValuableItem("Pseudoephedrine", "Pseudoephedrine, usually found in anti-allergy medicines.", ItemType.Valuables) {
                ItemSubType = ItemSubType.Medication}
        });
    }
    private void DefaultConfig_Drinks()
    {
        PossibleItems.DrinkItems.AddRange(new List<DrinkItem> {
            //Drinks
            //Bottles
            new DrinkItem("Bottle of Raine Water", "The water that rich people drink, and the main reason why there are now entire continents of plastic bottles floating in the ocean", ItemType.Drinks) { 
                ModelItemID = "ba_prop_club_water_bottle",
                HealthChangeAmount = MediumHealthRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Water, 
                FindPercentage = 10 },//slight clipping, no issyes
            new DrinkItem("Flow Water ZERO", "Hydrogen & Oxygen Infusion", ItemType.Drinks) {
                ModelItemID = "prop_ld_flow_bottle",
                HealthChangeAmount = MediumHealthRecover,
                ThirstChangeAmount = FullThirstRecover,
                ItemSubType = ItemSubType.Water,
                FindPercentage = 10 },//slight clipping, no issyes

            //prop_ld_flow_bottle.ydr
            new DrinkItem("Bottle of GREY Water", "Expensive water that tastes worse than tap!", ItemType.Drinks){
                ModelItemID = "h4_prop_battle_waterbottle_01a",
                HealthChangeAmount = MediumHealthRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Water, 
                FindPercentage = 10 },//lotsa clipping, does not have gravity
            new DrinkItem("Bottle of JUNK Energy", "The Quick Fix!", ItemType.Drinks){
                ModelItemID = "prop_energy_drink",
                HealthChangeAmount = MediumHealthRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = MediumSleepRecover,
                ItemSubType = ItemSubType.Soda, 
                FindPercentage = 10 },//fine
            //Beer
            new DrinkItem("Bottle of PiBwasser", "Cheap 11% ABV fighting lager brewed in Germany for export only from rice, barley, hops and the fresh urine of Bavarian virgins", ItemType.Drinks){
                ModelItemID = "prop_amb_beer_bottle",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = 5, 
                SleepChangeAmount = -2.0f,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = 5.0f, 
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5 },//is perfecto
            new DrinkItem("Bottle of A.M.", "Mornings Golden Shower", ItemType.Drinks){
                ModelItemID = "prop_beer_am",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover, 
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Barracho", "Es Playtime!", ItemType.Drinks){
                ModelItemID = "prop_beer_bar",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Blarneys", "Making your mouth feel lucky", ItemType.Drinks){
                ModelItemID = "prop_beer_blr", 
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Jakeys", "Drink Outdoors With Jakey's", ItemType.Drinks){
                ModelItemID = "prop_beer_jakey", 
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Logger", "A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned", ItemType.Drinks){
                ModelItemID = "prop_beer_logger",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Patriot", "Never refuse a patriot", ItemType.Drinks){
                ModelItemID = "prop_beer_patriot", 
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Pride", "Swallow Me", ItemType.Drinks){
                ModelItemID = "prop_beer_pride", 
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Stronzo", "Birra forte d'Italia", ItemType.Drinks){
                ModelItemID = "prop_beer_stz",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Dusche", "Das Ist Gut Ja!", ItemType.Drinks){
                ModelItemID = "prop_beerdusche", 
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.1f,
                FindPercentage = 5},//Does not have gravity, attachmentis too far down
            //Liquor
            new DrinkItem("Bottle of 40 oz", "Drink like a true thug!", ItemType.Drinks){
                ModelItemID = "prop_cs_beer_bot_40oz",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Beer,
                IntoxicationPerInterval = 0.2f,
                FindPercentage = 1},
            new DrinkItem("Bottle of Sinsimito Tequila", "Extra Anejo 100% De Agave. 42% Alcohol by volume", ItemType.Drinks){
                PackageItemID = "h4_prop_h4_t_bottle_02a",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},
            new DrinkItem("Bottle of Cazafortuna Tequila", "Tequila Anejo. 100% Blue Agave 40% Alcohol by volume", ItemType.Drinks){
                PackageItemID = "h4_prop_h4_t_bottle_01a", 
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            new DrinkItem("The Mount Bourbon Whisky", "The ulitmate blender. 60% alc/vol. Distilled and bottled by The Mount Distilling Co.", ItemType.Drinks){
                PackageItemID = "prop_whiskey_bottle",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            new DrinkItem("Richard's Whisky Short", "Made with grain produced in the fields of Kentucky.", ItemType.Drinks){
                PackageItemID = "ba_prop_battle_whiskey_bottle_s",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},
            new DrinkItem("Macbeth Single Malt Short", "Cask aged scotch whisky", ItemType.Drinks){
                PackageItemID = "ba_prop_battle_whiskey_bottle_2_s",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            new DrinkItem("Ragga Rum", "Original Dark", ItemType.Drinks){
                PackageItemID = "prop_rum_bottle",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},
            new DrinkItem("This Worm Has Turned Tequilya", "Tequilya distilled the traditional way with a mind blowing tripping worm!", ItemType.Drinks){
                PackageItemID = "prop_tequila_bottle",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},


            new DrinkItem("Cardiaque Brandy", "Fine Napoleon Brandy.", ItemType.Drinks){
                PackageItemID = "prop_bottle_brandy",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            new DrinkItem("Bourgeoix Cognac", "Fit for a prince.", ItemType.Drinks){
                PackageItemID = "prop_bottle_cognac",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},
            new DrinkItem("Macbeth Single Malt", "Cask aged scotch whisky", ItemType.Drinks){
                PackageItemID = "prop_bottle_macbeth",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            new DrinkItem("Richard's Whisky", "Made with grain produced in the fields of Kentucky.", ItemType.Drinks){
                PackageItemID = "prop_bottle_richard",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            new DrinkItem("NOGO Vodka", "Make the night a NOGO.", ItemType.Drinks){
                PackageItemID = "prop_vodka_bottle",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            new DrinkItem("Bleuter'd Champagne", "Vintage 1969. Nice.", ItemType.Drinks){
                PackageItemID = "prop_champ_01a",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            new DrinkItem("Cherenkov Red Label Vodka", "Warms you to the core.", ItemType.Drinks){
                PackageItemID = "prop_cherenkov_01",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},
            new DrinkItem("Cherenkov Blue Label Vodka", "Warms you to the core.", ItemType.Drinks){
                PackageItemID = "prop_cherenkov_02",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},
            new DrinkItem("Cherenkov Green Label Vodka", "Warms you to the core.", ItemType.Drinks){
                PackageItemID = "prop_cherenkov_03",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},
            new DrinkItem("Cherenkov Purple Label Vodka", "Warms you to the core.", ItemType.Drinks){
                PackageItemID = "prop_cherenkov_04",
                IntoxicantName = "High Proof Alcohol",
                HealthChangeAmount = SmallHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType= ItemSubType.Liquor,
                IntoxicationPerInterval = 0.4f,
                FindPercentage = 1},

            //Cups & Cans
            new DrinkItem("Can of eCola", "Deliciously Infectious!", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacan_01a",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType= ItemSubType.Soda, 
                FindPercentage = 10 },
            new DrinkItem("Can of Sprunk", "Slurp Sprunk Mmm! Delicious", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacan_01b",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType= ItemSubType.Soda, 
                FindPercentage = 10},
            new DrinkItem("Can of Orang-O-Tang", "Orange AND Tang! Orang-O-Tang!", ItemType.Drinks){
                ModelItemID = "prop_orang_can_01",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType= ItemSubType.Soda,
                FindPercentage = 10},//needs better attachment
            new DrinkItem("Carton of Milk", "Full Fat. Farmed and produced in U.S.A.", ItemType.Drinks) { 
                HealthChangeAmount = MediumHealthRecover,
                ThirstChangeAmount = FullThirstRecover,
                HungerChangeAmount = MediumHungerRecover,
                ModelItemID = "prop_cs_milk_01",
                ItemSubType= ItemSubType.Milk },
            new DrinkItem("Cup of eCola", "Deliciously Infectious!", ItemType.Drinks){
                ModelItemID = "prop_plastic_cup_02",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType= ItemSubType.Soda},//has no gravity, too far down
            new DrinkItem("Cup of Sprunk", "Slurp Sprunk Mmm! Delicious", ItemType.Drinks){
                ModelItemID = "prop_plastic_cup_02",//Cluckin Bell
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover, 
                ItemSubType= ItemSubType.Soda},//perfecto
            new DrinkItem("Cup of Coffee", "Finally something without sugar! Sugar on Request", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_02",
                HealthChangeAmount = MediumHealthRecover, 
                SleepChangeAmount = MediumSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Can of Hoplivion Double IPA", "So many hops it should be illegal.", ItemType.Drinks){
                ModelItemID = "h4_prop_h4_can_beer_01a",
                IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Beer},//pretty good, maybeslightly off
            new DrinkItem("Can of Blarneys", "Making your mouth feel lucky", ItemType.Drinks) 
                { IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                FindPercentage = 1,
                IntoxicationPerInterval = 0.1f,
                ItemSubType = ItemSubType.Beer},
            new DrinkItem("Can of Logger", "A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned", ItemType.Drinks) 
                { IntoxicantName = "Low Proof Alcohol",
                HealthChangeAmount = TinyHealthRecover,
                SleepChangeAmount = -1.0f * TinySleepRecover,
                HungerChangeAmount = TinyHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                FindPercentage = 1,
                IntoxicationPerInterval = 0.1f,
                ItemSubType = ItemSubType.Beer },
            //Bean Machine
            new DrinkItem("High Noon Coffee", "Drip coffee, carbonated water, fruit syrup and taurine.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover + 10,
                SleepChangeAmount = MediumSleepRecover + 10.0f,
                ThirstChangeAmount = MediumThirstRecover + 10.0f,
                ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("The Eco-ffee", "Decaf light, rain forest rain, saved whale milk, chemically reclaimed freerange organic tofu, and recycled brown sugar", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover + 2,
                SleepChangeAmount = MediumSleepRecover + 2.0f, 
                ThirstChangeAmount = MediumThirstRecover + 2.0f,
                ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Speedball Coffee", "Caffeine tripe-shot, guarana, bat guano, and mate.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover + 5, 
                SleepChangeAmount = MediumSleepRecover + 5.0f, 
                ThirstChangeAmount = MediumThirstRecover + 5.0f,
                ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Gunkacchino Coffee", "Caffeine, refined sugar, trans fat, high-fructose corn syrup, and cheesecake base.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover + 7, 
                SleepChangeAmount = MediumSleepRecover + 7.0f,
                ThirstChangeAmount = MediumThirstRecover + 7.0f,
                ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Bratte Coffee", "Double shot latte, and 100 pumps of caramel.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = MediumSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Flusher Coffee", "Caffeine, organic castor oil, concanetrated OJ, chicken vindaldo, and senna pods.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = MediumSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Caffeagra Coffee", "Caffeine (Straight up), rhino horn, oyster shell, and sildenafil citrate.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover + 7,
                SleepChangeAmount = MediumSleepRecover + 7.0f,
                ThirstChangeAmount = MediumThirstRecover + 7.0f,
                ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Big Fruit Smoothie", "Frothalot, watermel, carbonated water, taurine, and fruit syrup.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover + 5,
                SleepChangeAmount = MediumSleepRecover + 5.0f,
                ThirstChangeAmount = MediumThirstRecover + 5.0f,
                ItemSubType = ItemSubType.Coffee},//perfecto
            //UP N ATOM
            new DrinkItem("Jumbo Shake", "Almost a whole cow full of milk", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01c",
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = MediumThirstRecover, 
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Milk},//no gravity, attached wrong
            new DrinkItem("Large eCola", "Deliciously Infectious!", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01c",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType= ItemSubType.Soda},//has no gravity, too far down
            new DrinkItem("Large Sprunk", "Slurp Sprunk Mmm! Delicious", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01c",//Cluckin Bell
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType= ItemSubType.Soda},//perfecto
            //burger shot
            new DrinkItem("Double Shot Coffee", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = MediumSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Coffee },//n gravity,not attached right
            new DrinkItem("Liter of eCola", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01a",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType = ItemSubType.Soda},//n gravity,not attached right
            new DrinkItem("Liter of Sprunk", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01a",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType = ItemSubType.Soda },//n gravity,not attached right 
            //Cluckin Bell
            new DrinkItem("XXL eCola", "Deliciously Infectious!", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01b",
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType= ItemSubType.Soda},//has no gravity, too far down
            new DrinkItem("XXL Sprunk", "Slurp Sprunk Mmm! Delicious", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01b",//Cluckin Bell
                HealthChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ThirstChangeAmount = MediumThirstRecover,
                HungerChangeAmount = TinyHungerRecover,
                ItemSubType= ItemSubType.Soda},//perfecto
            //Dessert
            new DrinkItem("Chocolate Shake","", ItemType.Drinks) {
                ConsumeOnPurchase = true,
                ModelItemID = "ng_proc_sodacup_01c",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                SleepChangeAmount = MediumSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new DrinkItem("Vanilla Shake","", ItemType.Drinks) {
                ConsumeOnPurchase = true,
                ModelItemID = "ng_proc_sodacup_01c",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},

        });;
    }
    private void DefaultConfig_Drugs()
    {
        PossibleItems.SmokeItems.AddRange(new List<SmokeItem>
        {
            //Cigarettes/Cigars
            new SmokeItem("Redwood Regular", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda", ItemType.Tobacco) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 15 },
            new SmokeItem("Redwood Mild", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda. Milder version", ItemType.Tobacco) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs2",AmountPerPackage = 20, HealthChangeAmount = -5,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 5 },
            new SmokeItem("Debonaire", "Tobacco products marketed at the more sophisticated smoker, whoever that is", ItemType.Tobacco) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs3",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 2 },
            new SmokeItem("Debonaire Menthol", "Tobacco products marketed at the more sophisticated smoker, whoever that is. With Menthol!", ItemType.Tobacco) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs4",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 2 },
            new SmokeItem("Caradique", "Fine Napoleon Cigarettes", ItemType.Tobacco) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs5",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 2 },
            new SmokeItem("69 Brand","Don't let an embargo stop you", ItemType.Tobacco) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs6",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 2 },
            //new Vector3(-0.025f,0.01f,0.004f),new Rotator(0f, 0f, 90f) female mouth attach?
            new SmokeItem("Estancia Cigar","Medium Cut. Hand Rolled.", ItemType.Tobacco) {
                ModelItemID = "prop_cigar_02",
                PackageItemID = "p_cigar_pack_02_s",AmountPerPackage = 20,Duration = 240000, HealthChangeAmount = -5,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigar, FindPercentage = 1 },
            //new ModItem("ElectroToke Vape","The Electrotoke uses highly sophisticated micro-molecule atomization technology to make the ingestion of hard drugs healthy, dscreet, pleasurable and, best of all, completely safe.", ItemType.Drugs) {
            //    ModelItemID = "h4_prop_battle_vape_01"), IntoxicantName = "Marijuana", PercentLostOnUse = 0.05f },


            new SmokeItem("Marijuana","Little Jacob Tested, Truth Approved", ItemType.Drugs) {
                ModelItemID = "p_cs_joint_01"//p_amb_joint_01
                ,PackageItemID = "sf_prop_sf_bag_weed_01a", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", ItemSubType = ItemSubType.Narcotic, IsPossessionIllicit = true, FindPercentage = 2, HungerChangeAmount = -5.0f, ThirstChangeAmount = -2.0f, NeedsRollingPapers = true },
        });
        PossibleItems.IngestItems.AddRange(new List<IngestItem>
        {
            new IngestItem("Bull Shark Testosterone","More bite than bush elephant testosterone. Become more aggressive, hornier, and irresistible to women! The ultimate man!", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Bull Shark Testosterone" , AmountPerPackage = 10, ItemSubType = ItemSubType.Medication, FindPercentage = 1,ThirstChangeAmount = -5, HungerChangeAmount = -5, SleepChangeAmount = 20 },     
            new IngestItem("Alco Patch","It's the same refreshing feeling of your favorite drink, but delivered transdermally and discreetly.", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Alco Patch",AmountPerPackage = 10, ItemSubType = ItemSubType.Medication, FindPercentage = 1,ThirstChangeAmount = -5, SleepChangeAmount = -15 },     
            new IngestItem("Lax to the Max","Lubricated suppositories. Get flowing again!", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Alco Patch",AmountPerPackage = 10, ItemSubType = ItemSubType.Medication, FindPercentage = 1, HungerChangeAmount = -10, ThirstChangeAmount = -10 },    
            new IngestItem("Mollis","For outstanding erections. Get the performance you've always dreamed of", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Mollis",AmountPerPackage = 10, ItemSubType = ItemSubType.Medication, FindPercentage = 1, AlwaysChangesHealth = true, HealthChangeAmount = 2 },     
            new IngestItem("Chesty","Cough suppressant manufactured by Good Aids Pharmacy. Gives 24-hour relief and is available in honey flavour.", ItemType.Drugs) {
                ModelItemID = "prop_cs_script_bottle_01",IntoxicantName = "Chesty", AmountPerPackage = 10, ItemSubType = ItemSubType.Medication, FindPercentage = 1, AlwaysChangesHealth = true, HealthChangeAmount = 10 },    
            new IngestItem("Equanox","Combats dissatisfaction, lethargy, depression, melancholy, sexual dysfunction.", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Equanox", AmountPerPackage = 10, ItemSubType = ItemSubType.Medication, FindPercentage = 1, AlwaysChangesHealth = true, HealthChangeAmount = 5 },        
            new IngestItem("Zombix","Painkiller and antidepressant manufactured by O'Deas Pharmacy. ~n~'Go straight for the head.'", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Zombix", AmountPerPackage = 10, ItemSubType = ItemSubType.Medication, FindPercentage = 1, AlwaysChangesHealth = true, HealthChangeAmount = 50 },
           
            new IngestItem("Wach-Auf Caffeine Pills","When you need to Wach-Auf, but there's no time to delay! Remember, sleep is for the weak!", ItemType.Drugs) { AmountPerPackage = 25,
                ModelItemID = "prop_cs_pills", ItemSubType = ItemSubType.Medication,SleepChangeAmount = 35,ThirstChangeAmount = -5,HungerChangeAmount = -5, FindPercentage = 5 },

            new IngestItem("Hingmyralgan","For Brain-Ache and other pains!", ItemType.Drugs) { AmountPerPackage = 25,
                ModelItemID = "prop_cs_pills", ItemSubType = ItemSubType.Painkiller,HealthChangeAmount = 12, FindPercentage = 15, AlwaysChangesHealth = true },
            new IngestItem("Deludamol","For a Night You'll Never Remember. Extra Strength Painkiller.", ItemType.Drugs) { AmountPerPackage = 25,
                ModelItemID = "prop_cs_pills", ItemSubType = ItemSubType.Painkiller,HealthChangeAmount = 15,ThirstChangeAmount = -1, FindPercentage = 5, AlwaysChangesHealth = true },
            new IngestItem("Delladamol","Extra Strength Painkiller. Extra Legit Packaging.", ItemType.Drugs) { AmountPerPackage = 12,
                ModelItemID = "prop_cs_pills", ItemSubType = ItemSubType.Painkiller,HealthChangeAmount = 10,ThirstChangeAmount = -3, FindPercentage = 5, AlwaysChangesHealth = true },

            new IngestItem("Diazepam","When you REALLY need to line up the shot. Useful for hunting wolves.",ItemType.Drugs) { 
                AmountPerPackage = 12,
                ModelItemID = "prop_cs_pills", 
                ItemSubType = ItemSubType.Medication,
                HealthChangeAmount = 2,
                ThirstChangeAmount = -3, 
                FindPercentage = 5, 
                AlwaysChangesHealth = true,
                IntoxicantName = "Diazepam",
            },


            new IngestItem("SPANK","You looking for some fun? a little.. hmmm? Some SPANK?", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_pills",IntoxicantName = "SPANK", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic, FindPercentage = 1, PoliceFindDuringPlayerSearchPercentage = 20 },
            new IngestItem("Toilet Cleaner","The hot new legal high that takes you to places you never imagined and leaves you forever changed.", ItemType.Drugs) { IsPossessionIllicit = true,IsPublicUseIllegal = true,
                ModelItemID = "prop_cs_pills",IntoxicantName = "Toilet Cleaner", ItemSubType = ItemSubType.Narcotic, FindPercentage = 1, PoliceFindDuringPlayerSearchPercentage = 15 },
        });
        PossibleItems.InhaleItems.AddRange(new List<InhaleItem>
        {
            new InhaleItem("Cocaine","Also known as coke, crack, girl, lady, charlie, caine, tepung, and snow", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "ba_prop_battle_sniffing_pipe"
                ,PackageItemID = "prop_meth_bag_01"
                ,IsPublicUseIllegal = true
                ,IntoxicantName = "Cocaine", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic, FindPercentage = 1, PoliceFindDuringPlayerSearchPercentage = 12 },
        });
        PossibleItems.InjectItems.AddRange(new List<InjectItem>
        {
            new InjectItem("Heroin","Heroin was first made by C. R. Alder Wright in 1874 from morphine, a natural product of the opium poppy. Things have gone downhill since then.", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_syringe_01"
                ,PackageItemID = "prop_meth_bag_01"
                ,IsPublicUseIllegal = true
                ,IntoxicantName = "Heroin", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic, FindPercentage = 1, PoliceFindDuringPlayerSearchPercentage = 25 },
        });
        PossibleItems.PipeSmokeItems.AddRange(new List<PipeSmokeItem>
        {
            new PipeSmokeItem("Methamphetamine","Also referred to as Speed, Sabu, Crystal and Meth", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_meth_bag_01"
                ,PackageItemID = "prop_meth_bag_01"
                ,IsPublicUseIllegal = true
                ,IntoxicantName = "Methamphetamine", PercentLostOnUse = 0.25f, MeasurementName = "Gram",  ItemSubType = ItemSubType.Narcotic, FindPercentage = 1, PoliceFindDuringPlayerSearchPercentage = 15 },
            new PipeSmokeItem("Crack", "Too cheap for cocaine? Find out what it means when they say things are 'like crack'", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_meth_bag_01"
                ,PackageItemID = "prop_meth_bag_01"
                ,IsPublicUseIllegal = true
                ,IntoxicantName = "Crack", PercentLostOnUse = 0.5f, MeasurementName = "Gram",  ItemSubType = ItemSubType.Narcotic, FindPercentage = 1, PoliceFindDuringPlayerSearchPercentage = 15 },
        });

    }
    private void DefaultConfig_Food()
    {
        PossibleItems.FoodItems.AddRange(new List<FoodItem>
        {
            //Generic Food
            new FoodItem("Hot Dog","Your favorite mystery meat sold on street corners everywhere. Niko would be proud", ItemType.Food) {
                ModelItemID = "prop_cs_hotdog_01", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ItemSubType = ItemSubType.Entree },
            new FoodItem("Hot Sausage","Get all your jokes out", ItemType.Food) {
                ModelItemID = "prop_cs_hotdog_01",
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover, 
                ItemSubType = ItemSubType.Entree },
            new FoodItem("Hot Pretzel","You tie me up", ItemType.Food) {
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover,
                ItemSubType = ItemSubType.Entree },
            new FoodItem("3 Mini Pretzels","Like a pretzel, but smaller", ItemType.Food) {
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover, 
                ItemSubType = ItemSubType.Entree },
            new FoodItem("Nuts","You're gonna love my nuts", ItemType.Food) {
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover, 
                ThirstChangeAmount = -1.0f * TinyThirstRecover, 
                ItemSubType = ItemSubType.Snack },
            new FoodItem("Burger","100% Certified Food", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ItemSubType = ItemSubType.Entree },
            new FoodItem("Donut","MMMMMMM Donuts", ItemType.Food) {
                ModelItemID = "prop_donut_01", 
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover, 
                ItemSubType = ItemSubType.Snack } ,
            new FoodItem("Bagel Sandwich","Bagel with extras, what more do you need?", ItemType.Food) {
                ModelItemID = "p_amb_bagel_01", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Freedom Fries","Made from true Cataldo potatoes!", ItemType.Food) { 
                ModelItemID = "prop_food_bs_chips",
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = -1.0f * TinyThirstRecover, 
                ItemSubType = ItemSubType.Side},
            new FoodItem("French Fries","Made from true Cataldo potatoes!", ItemType.Food) { 
                ModelItemID = "prop_food_chips",
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = -1.0f * TinyThirstRecover, 
                ItemSubType = ItemSubType.Side},
            new FoodItem("Banana","An elongated, edible fruit – botanically a berry[1][2] – produced by several kinds of large herbaceous flowering plants in the genus Musa", ItemType.Food) {
                ModelItemID = "ng_proc_food_nana1a", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = SmallThirstRecover,
                ItemSubType = ItemSubType.Fruit },
            new FoodItem("Orange","Not just a color", ItemType.Food) {
                ModelItemID = "ng_proc_food_ornge1a", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = SmallThirstRecover,
                ItemSubType = ItemSubType.Fruit },
            new FoodItem("Apple","Certified sleeping death free", ItemType.Food) {
                ModelItemID = "ng_proc_food_aple1a", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = SmallThirstRecover, 
                ItemSubType = ItemSubType.Fruit },
            new FoodItem("Ham and Cheese Sandwich","Basic and shitty, just like you", ItemType.Food) {
                ModelItemID = "prop_sandwich_01", 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ItemSubType = ItemSubType.Entree },
            new FoodItem("Turkey Sandwich","The most plain sandwich for the most plain person", ItemType.Food) {
                ModelItemID = "prop_sandwich_01",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Tuna Sandwich","Haven't got enough heavy metals in you at your job? Try tuna!", ItemType.Food) {
                ModelItemID = "prop_sandwich_01",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Taco","", ItemType.Food) { 
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree,
                ModelItemID = "prop_taco_01" },
            new FoodItem("Strawberry Rails Cereal","The breakfast food you snort!", ItemType.Food) { 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = -1.0f * MediumThirstRecover,
                ItemSubType = ItemSubType.Cereal} ,
            new FoodItem("Crackles O' Dawn Cereal","Smile at the crack!", ItemType.Food) { 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = -1.0f * MediumThirstRecover,
                ItemSubType = ItemSubType.Cereal} ,
            new FoodItem("White Bread","Extra white, with minimal taste.", ItemType.Food) { 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = -1.0f * MediumThirstRecover, 
                AmountPerPackage = 25, 
                ItemSubType = ItemSubType.Bread} ,
            //Desert
            new FoodItem("Chocolate Cone","", ItemType.Food) { 
                ConsumeOnPurchase = true, 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = SmallThirstRecover, 
                SleepChangeAmount = MediumSleepRecover, 
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Vanilla Cone","", ItemType.Food) { 
                ConsumeOnPurchase = true, 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = SmallThirstRecover, 
                SleepChangeAmount = SmallSleepRecover, 
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Hot Fudge Sundae","", ItemType.Food) { 
                ConsumeOnPurchase = true, 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = SmallThirstRecover, 
                SleepChangeAmount = MediumSleepRecover, 
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Banana Split","", ItemType.Food) { 
                ConsumeOnPurchase = true, 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = SmallThirstRecover, 
                SleepChangeAmount = SmallSleepRecover, 
                ItemSubType = ItemSubType.Dessert},

            //Pizza
            new FoodItem("Slice of Pizza","Caution may be hot", ItemType.Food) {
                ModelItemID = "v_res_tt_pizzaplate", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Small Cheese Pizza","Best when you are home alone.", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover, 
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Small Pepperoni Pizza","Get a load of our pepperonis!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Small Supreme Pizza","Get stuffed", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Medium Cheese Pizza","Best when you are home alone.", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = HugeHealthRecover,
                HungerChangeAmount = HugeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Medium Pepperoni Pizza","Get a load of our pepperonis!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = HugeHealthRecover,
                HungerChangeAmount = HugeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Medium Supreme Pizza","Get stuffed", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = HugeHealthRecover,
                HungerChangeAmount = HugeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Large Cheese Pizza","Best when you are home alone.", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Large Pepperoni Pizza","Get a load of our pepperonis!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Large Supreme Pizza","Get stuffed", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02",
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("10 inch Cheese Pizza","Extra cheesy!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("10 inch Pepperoni Pizza","Mostly Meat!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("10 inch Supreme Pizza","We forgot the kitchen sink!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("12 inch Cheese Pizza","Extra cheesy!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01",
                HealthChangeAmount = HugeHealthRecover,
                HungerChangeAmount = HugeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("12 inch Pepperoni Pizza","Mostly Meat!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01",
                HealthChangeAmount = HugeHealthRecover,
                HungerChangeAmount = HugeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("12 inch Supreme Pizza","We forgot the kitchen sink!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01",
                HealthChangeAmount = HugeHealthRecover,
                HungerChangeAmount = HugeHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("18 inch Cheese Pizza","Extra cheesy! Extra Large!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", 
                HealthChangeAmount = 65, 
                HungerChangeAmount = 100.0f, 
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("18 inch Pepperoni Pizza","Mostly Meat! Extra Large!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01",
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("18 inch Supreme Pizza","We forgot the kitchen sink! Extra Large!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01",
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ItemSubType = ItemSubType.Pizza } ,
            //Chips
            new FoodItem("Sticky Rib Phat Chips","They are extra phat. Sticky Rib Flavor.", ItemType.Food) {
                ModelItemID = "v_ret_ml_chips1",
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover, 
                ThirstChangeAmount = -1.0f * TinyThirstRecover, 
                ItemSubType = ItemSubType.Snack, 
                FindPercentage = 10 },
            new FoodItem("Habanero Phat Chips","They are extra phat. Habanero flavor", ItemType.Food) {
                ModelItemID = "v_ret_ml_chips2", 
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover,
                ThirstChangeAmount = -1.0f * TinyThirstRecover, 
                ItemSubType = ItemSubType.Snack, 
                FindPercentage = 10 },
            new FoodItem("Supersalt Phat Chips","They are extra phat. Supersalt flavor.", ItemType.Food) {
                ModelItemID = "v_ret_ml_chips3", 
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover, 
                ThirstChangeAmount = -1.0f * TinyThirstRecover, 
                ItemSubType = ItemSubType.Snack,
                FindPercentage = 10 },
            new FoodItem("Big Cheese Phat Chips","They are extra phat. Big Cheese flavor.", ItemType.Food) {
                ModelItemID = "v_ret_ml_chips4",
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover,
                ThirstChangeAmount = -1.0f * TinyThirstRecover, 
                ItemSubType = ItemSubType.Snack,
                FindPercentage = 10 },
            //Candy
            new FoodItem("Ego Chaser Energy Bar","Contains 20,000 Calories! ~n~'It's all about you'", ItemType.Food) {
                ModelItemID = "prop_choc_ego",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Snack,
                FindPercentage = 10 },
            new FoodItem("King Size P's & Q's","The candy bar that kids and stoners love. EXTRA Large", ItemType.Food) {
                ModelItemID = "prop_candy_pqs", 
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHealthRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Snack,
                FindPercentage = 10 },
            new FoodItem("P's & Q's","The candy bar that kids and stoners love", ItemType.Food) {
                ModelItemID = "prop_choc_pq", 
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = SmallHungerRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Snack, 
                FindPercentage = 10 },
            new FoodItem("Meteorite Bar","Dark chocolate with a GOOEY core", ItemType.Food) {
                ModelItemID = "prop_choc_meto",
                HealthChangeAmount = SmallHealthRecover,
                HungerChangeAmount = SmallHungerRecover,
                SleepChangeAmount = SmallSleepRecover, 
                ItemSubType = ItemSubType.Snack, 
                FindPercentage = 10 },
            //Taco Bomb
            new FoodItem("Breakfast Burrito", ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Deep Fried Salad", ItemType.Food) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ConsumeOnPurchase = true,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Beef Bazooka", ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover, 
                ConsumeOnPurchase = true,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Chimichingado Chiquito", ItemType.Food) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Cheesy Meat Flappers", ItemType.Food) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Volcano Mudsplatter Nachos", ItemType.Food) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree},
            //Generic Restaurant
            //FancyDeli
            new FoodItem("Chicken Club Salad", ItemType.Meals) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Spicy Seafood Gumbo", ItemType.Meals) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Muffaletta", ItemType.Meals) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Zucchini Garden Pasta", ItemType.Meals) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Pollo Mexicano", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Italian Cruz Po'boy", ItemType.Meals) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Chipotle Chicken Panini", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true,
                ItemSubType = ItemSubType.Entree } ,
            //FancyFish
            new FoodItem("Coconut Crusted Prawns", ItemType.Meals) {
                PackageItemID = "prop_food_bag1", 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Crab and Shrimp Louie", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Open-Faced Crab Melt", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("King Salmon", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Ahi Tuna", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            //FancyGeneric
            new FoodItem("Smokehouse Burger", ItemType.Meals) {
                ModelItemID = "prop_cs_burger_01",
                 HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree },
            new FoodItem("Chicken Critters Basket", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Prime Rib 16 oz", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Bone-In Ribeye", ItemType.Meals) {
                PackageItemID = "prop_cs_steak", 
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Grilled Pork Chops", ItemType.Meals) {
                PackageItemID = "prop_food_bag1",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Grilled Shrimp", ItemType.Meals) {
                PackageItemID = "prop_food_bag1" ,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ConsumeOnPurchase = true, 
                ItemSubType = ItemSubType.Entree } ,
            //Noodles
            new FoodItem("Juek Suk tong Mandu", ItemType.Meals) {
                PackageItemID = "prop_ff_noodle_01",
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Entree },
            new FoodItem("Hayan Jam Pong", ItemType.Meals) {
                PackageItemID = "prop_ff_noodle_02",
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Sal Gook Su Jam Pong", ItemType.Meals) {
                PackageItemID = "prop_ff_noodle_01",
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Chul Pan Bokkeum Jam Pong", ItemType.Meals) {
                PackageItemID = "prop_ff_noodle_02",
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Deul Gae Udon", ItemType.Meals) {
                PackageItemID = "prop_ff_noodle_02",
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Dakgogo Bokkeum Bap", ItemType.Meals) {
                PackageItemID = "prop_ff_noodle_01",
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                ItemSubType = ItemSubType.Entree } ,


            //Burger Shot
            new FoodItem("The Bleeder Meal","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Torpedo Meal","", ItemType.Combos) { 
                ConsumeOnPurchase = true, 
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover, 
                SleepChangeAmount = TinySleepRecover, 
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Money Shot Meal","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover, 
                HungerChangeAmount = LargeHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Meat Free Meal","", ItemType.Combos) { 
                ConsumeOnPurchase = true, 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ThirstChangeAmount = LargeThirstRecover, 
                SleepChangeAmount = TinySleepRecover, 
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Bleeder Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Torpedo Sandwich","", ItemType.Food) { 
                ModelItemID = "prop_food_burg2",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Money Shot Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},

            //Cluckin
            new FoodItem("Cluckin' Huge Meal","", ItemType.Combos) { 
                PackageItemID = "prop_food_cb_tray_02",
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Cluckin' Big Meal","200% bigger breasts", ItemType.Combos) { 
                PackageItemID = "prop_food_cb_tray_02",
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Cluckin' Little Meal","May contain meat", ItemType.Combos) {
                PackageItemID = "prop_food_cb_tray_03", 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = MediumThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Wing Piece","", ItemType.Food) { 
                PackageItemID = "prop_food_cb_tray_03",
                ConsumeOnPurchase = true, 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Little Peckers","", ItemType.Food) { 
                PackageItemID = "prop_food_cb_tray_03",
                ConsumeOnPurchase = true, 
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Balls & Rings","", ItemType.Food) { 
                PackageItemID = "prop_food_cb_tray_03",
                ConsumeOnPurchase = true,
                HealthChangeAmount = SmallHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ItemSubType = ItemSubType.Side},
            
            //UpNAtom
            new FoodItem("Trio Trio Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Dual Dual Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Solo Solo Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Trio Trio Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Dual Dual Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Solo Solo Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01", 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover, 
                ItemSubType = ItemSubType.Entree},

            //Wigwam
            new FoodItem("Big Wig Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Cheesie Wigwam Combo","", ItemType.Combos) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Wigwam Classic Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
               HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Big Wig Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
               HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Cheesie Wigwam Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
               HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Wigwam Classic Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
               HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree
            },

            //Horny's
            new FoodItem("Big Horny Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Horny Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Randy Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Big Horny Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Horny Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Randy Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
  
            //Snr Buns
            new FoodItem("Snr. Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Soph. Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Jr. Combo","", ItemType.Combos) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Snr. Burger","", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Soph. Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Jr. Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = SmallHealthRecover,
                HungerChangeAmount = SmallHungerRecover,
                ItemSubType = ItemSubType.Entree},

            //Vaca Loca
            new FoodItem("Muy Loca Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Loca Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Locita Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Muy Loca Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Loca Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Locita Burger","", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = SmallHealthRecover,
                HungerChangeAmount = SmallHungerRecover,
                ItemSubType = ItemSubType.Entree},

            //Beefy Bills
            new FoodItem("Kingsize Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Double Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Megacheese Burger","", ItemType.Food) { 
                ModelItemID = "prop_cs_burger_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},

            //Plucker
            new FoodItem("Plucker Combo 1","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Plucker Combo 2","", ItemType.Combos) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Plucker Combo 3","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Plucker Breast","", ItemType.Food) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Plucker Thigh","", ItemType.Food) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Plucker Salad","", ItemType.Food) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},

            //Bishop
            new FoodItem("Pope Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                 HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Cardinal Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Bishop Combo","", ItemType.Combos) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("3 pc Chicken","", ItemType.Food) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover + 10,
                HungerChangeAmount = MediumHungerRecover + 10,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("2 pc Chicken ","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover + 5,
                HungerChangeAmount = MediumHungerRecover + 5,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("1 pc Chicken","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
          
            //Bite
            new FoodItem("Gut Buster Combo","", ItemType.Combos) { 
                ModelItemID = "prop_food_burg2",
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Meat Tube Combo","", ItemType.Combos) { 
                ModelItemID = "prop_food_burg2",
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Iceberg Salad Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                SleepChangeAmount = TinySleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Gut Buster Sandwich","", ItemType.Food) { 
                ModelItemID = "prop_food_burg2",
                HealthChangeAmount = MediumHealthRecover + 10,
                HungerChangeAmount = MediumHungerRecover + 10,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Meat Tube Sandwich","", ItemType.Food) { 
                ModelItemID = "prop_food_burg2",
                HealthChangeAmount = MediumHealthRecover + 5,
                HungerChangeAmount = MediumHungerRecover + 5,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Iceberg Salad","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},

            //Al Dentes
            new FoodItem("Tour of Algonquin","", ItemType.Meals) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = FullHealthRecover,
                HungerChangeAmount = FullHungerRecover,
                ThirstChangeAmount = FullThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Lasagna Cheesico","", ItemType.Meals) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Seafood Ravioli","", ItemType.Meals) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Spaghetti & 'Meat' Balls","", ItemType.Meals) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Meal},

            //TacoFarmer & Generic
            new FoodItem("Asada Plate","", ItemType.Combos) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("2 Tacos Combo","", ItemType.Combos) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("2 Enchiladas Combo","", ItemType.Combos) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("Quesadilla Combo","", ItemType.Combos) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = LargeHealthRecover,
                HungerChangeAmount = LargeHungerRecover,
                ThirstChangeAmount = LargeThirstRecover,
                ItemSubType = ItemSubType.Meal},
            new FoodItem("San Andreas Burrito","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Torta","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},
            new FoodItem("Quesadilla","", ItemType.Food) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ItemSubType = ItemSubType.Entree},

            //Cherry Popper
            new FoodItem("Captain's Log","", ItemType.Food) {
                ConsumeOnPurchase = true, 
                HealthChangeAmount = MediumHealthRecover, 
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover, 
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Uder Milken","", ItemType.Food) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Creamy Chufty","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                 HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Chocolate Chufty","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Zebrabar","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Chilldo X-Treme","", ItemType.Food) {
                ConsumeOnPurchase = true,
                 HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Fruity Streak","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Chocco Streak","", ItemType.Food) { 
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Earthquakes","", ItemType.Food) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Chocolate Starfish","", ItemType.Food) {
                ConsumeOnPurchase = true,
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                ThirstChangeAmount = TinyThirstRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            
            //Rusty Browns
            new FoodItem("Chocolate Donut","", ItemType.Food) {
                ModelItemID = "prop_donut_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Sprinkles Donut","", ItemType.Food) {
                ModelItemID = "prop_donut_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Rusty Ring Donut","", ItemType.Food) {
                ModelItemID = "prop_donut_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
            new FoodItem("Double Choc Whammy Donut","", ItemType.Food) {
                ModelItemID = "prop_donut_01",
                HealthChangeAmount = MediumHealthRecover,
                HungerChangeAmount = MediumHungerRecover,
                SleepChangeAmount = SmallSleepRecover,
                ItemSubType = ItemSubType.Dessert},
        });
    }
    private void DefaultConfig_Services()
    {
        PossibleItems.HotelStayItems.AddRange(new List<HotelStayItem>
        {
            //Generic Hotel
            new HotelStayItem("Room: Single Twin","Cheapest room for the most discerning client", ItemType.Services) { ConsumeOnPurchase = true,MeasurementName = "Night" },
            new HotelStayItem("Room: Single Queen","Clean sheets on request", ItemType.Services) { ConsumeOnPurchase = true, MeasurementName = "Night" },
            new HotelStayItem("Room: Double Queen","Have a little company, but don't want to get too close?", ItemType.Services) { ConsumeOnPurchase = true,MeasurementName = "Night" },
            new HotelStayItem("Room: Single King","Please clean off all mirrors after use", ItemType.Services) { ConsumeOnPurchase = true,MeasurementName = "Night" },
            new HotelStayItem("Room: Delux","Our nicest room, behave yourself", ItemType.Services) { ConsumeOnPurchase = true,MeasurementName = "Night" },

            //Viceroy Hotel
            new HotelStayItem("City View King","Standard room with a view of the city", ItemType.Services) { ConsumeOnPurchase = true,MeasurementName = "Night" },
            new HotelStayItem("City View Deluxe King","Deluxe room with view of the city.", ItemType.Services) { ConsumeOnPurchase = true,MeasurementName = "Night" },
            new HotelStayItem("Ocean View King","Standard room a full view of the ocean. ", ItemType.Services) { ConsumeOnPurchase = true,MeasurementName = "Night" },
            new HotelStayItem("Grande King","XL Deluxe room with plenty of space and amenities.", ItemType.Services) {ConsumeOnPurchase = true,MeasurementName = "Night" },
            new HotelStayItem("Grande Ocean View King","XL Deluxe room with with plenty of space and amenities and a view of the ocean", ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new HotelStayItem("Monarch Suite","Penthouse suite, reserved for the most discerning tastes", ItemType.Services) {ConsumeOnPurchase = true,MeasurementName = "Night" },
            new HotelStayItem("Empire Suite","Deluxe Penthouse suite, by invite only (or lots of money)", ItemType.Services) {ConsumeOnPurchase = true,MeasurementName = "Night" },

            //
        });
    }
    private void DefaultConfig_Paraphernalia()
    {
        PossibleItems.BongItems.AddRange(new List<BongItem>
        {
            new BongItem("Bong","Also known as a water pipe") {
                ModelItemID = "prop_bong_01",ItemSubType = ItemSubType.Paraphernalia, PossibleDrugItems = new List<string>(){ "Marijuana" } } ,
        });
        PossibleItems.PipeItems.AddRange(new List<PipeItem>
        {
            new PipeItem("Crack Pipe","You know what it's for.") {
                ModelItemID = "prop_cs_crackpipe",ItemSubType = ItemSubType.Paraphernalia, PossibleDrugItems = new List<string>(){ "Crack" } } ,
            new PipeItem("Meth Pipe","A required part of every meth bender.") {
                ModelItemID = "prop_cs_meth_pipe",ItemSubType = ItemSubType.Paraphernalia, PossibleDrugItems = new List<string>(){ "Methamphetamine" } } ,
        });
        PossibleItems.RollingPapersItems.AddRange(new List<RollingPapersItem>
        {
            new RollingPapersItem("Smoke Shop Rolling Papers","Need a quick cheap way to smoke your 'tobacco'?") {
                ModelItemID = "p_cs_papers_03",ItemSubType = ItemSubType.Paraphernalia, AmountPerPackage = 20, PossibleDrugItems = new List<string>(){ "Marijuana" } } ,
        });
    }
    private void DefaultConfig_Tools()
    {
        PossibleItems.RadioItems.AddRange(new List<RadioItem>
        {
            new RadioItem("Schmidt & Priss TL6 Scanner","Ever wonder what the LSPD talks about behind your back? Wonder no further.") {
                ModelItemID = "prop_cs_hand_radio", FindPercentage = 10,ItemSubType = ItemSubType.Tool },
        });
        PossibleItems.RadarDetectorItems.AddRange(new List<RadarDetectorItem>
        {
            new RadarDetectorItem("Schmidt & Priss RD4 Radar Detector","Feel the buzz from the fuzz.") {
                ModelItemID = "prop_cs_hand_radio", FindPercentage = 1,ItemSubType = ItemSubType.Tool },
        });
        PossibleItems.ScrewdriverItems.AddRange(new List<ScrewdriverItem>
        {
            //Generic Tools
            new ScrewdriverItem("Flint Phillips Screwdriver","Might get you into some locked things. No relation.") {
                ModelItemID = "prop_tool_screwdvr01", FindPercentage = 10,ItemSubType = ItemSubType.Tool },
            new ScrewdriverItem("Flint Flathead Screwdriver","Might get you into some locked things. With a nice flat head.") {
                ModelItemID = "gr_prop_gr_sdriver_01", FindPercentage = 10,ItemSubType = ItemSubType.Tool },
            new ScrewdriverItem("Flint Multi-Bit Screwdriver","Might get you into some locked things. Now multi-bit!") {
                ModelItemID = "gr_prop_gr_sdriver_02", FindPercentage = 10,ItemSubType = ItemSubType.Tool },
        });
        PossibleItems.DrillItems.AddRange(new List<DrillItem>
        {
            new DrillItem("Power Metal Cordless Drill","Not recommended for dentistry.") {
                ModelItemID = "gr_prop_gr_drill_01a",ItemSubType = ItemSubType.Tool,MinSafeDrillTime = 10000,MaxSafeDrillTime = 19000  },
            new DrillItem("Power Metal Cordless Impact Driver","DRIVE it right in!") {
                ModelItemID = "gr_prop_gr_driver_01a",ItemSubType = ItemSubType.Tool,MinSafeDrillTime = 15000,MaxSafeDrillTime = 20000  },
            new DrillItem("Flint Cordless Drill","2-Speed Battery Drill. Impact-resistant casing. Light, compact and easy to use.") {
                ModelItemID = "prop_tool_drill" ,ItemSubType = ItemSubType.Tool,MinSafeDrillTime = 8000,MaxSafeDrillTime = 17000 },


            new DrillItem("Power Metal Side Drill","Make quick work of a safe deposit box!"){ 
                ModelItemID = "hei_prop_heist_drill",ItemSubType = ItemSubType.Tool,MinSafeDrillTime = 6000,MaxSafeDrillTime = 9000 },

            new DrillItem("Power Metal Custom Side Drill","Make quick work of a safe deposit box! Now with more bells and whistles!"){
                ModelItemID = "ch_prop_ch_heist_drill",ItemSubType = ItemSubType.Tool,MinSafeDrillTime = 6000,MaxSafeDrillTime = 9000 },

            new DrillItem("Power Metal Laser Side Drill","Use space age tech to drill holes in things!"){
                ModelItemID = "ch_prop_laserdrill_01a",ItemSubType = ItemSubType.Tool,MinSafeDrillTime = 4000,MaxSafeDrillTime = 6000 },

        });
        PossibleItems.PliersItems.AddRange(new List<PliersItem>
        {
            new PliersItem("Flint Pliers","For mechanics, pipe bomb makers, and amateur dentists alike. When you really need to grab something.") {
                ModelItemID = "prop_tool_pliers", FindPercentage = 10,ItemSubType = ItemSubType.Tool  },      
        });
        PossibleItems.LighterItems.AddRange(new List<LighterItem>
        {
            new LighterItem("DIC Lighter","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged") {
                ModelItemID = "p_cs_lighter_01", PercentLostOnUse = 0.01f, FindPercentage = 10 ,ItemSubType = ItemSubType.Lighter},
            new LighterItem("DIC Lighter Ultra","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Long burn version.") {
                ModelItemID = "p_cs_lighter_01", PercentLostOnUse = 0.005f, FindPercentage = 2,ItemSubType = ItemSubType.Lighter },
            new LighterItem("Dippo Lighter","Want to have all the hassle of carrying a lighter only for it to be out of fluid when you need it? Dippo is for you!") {
                ModelItemID = "v_res_tt_lighter", PercentLostOnUse = 0.05f, FindPercentage = 2,ItemSubType = ItemSubType.Lighter },
            new LighterItem("DIC Lighter Silver","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Too poor for gold?") {
                ModelItemID = "ex_prop_exec_lighter_01", PercentLostOnUse = 0.02f, FindPercentage = 1,ItemSubType = ItemSubType.Lighter },
            new LighterItem("DIC Lighter Gold","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Golden so it must be good!") {
                ModelItemID = "lux_prop_lighter_luxe",  PercentLostOnUse = 0.02f, FindPercentage = 1,ItemSubType = ItemSubType.Lighter },
        });
        PossibleItems.TapeItems.AddRange(new List<TapeItem>
        {
            new TapeItem("Flint Duct Tape","~r~CURRENTLY UNUSED~s~ Sticks to anything! Ducts, wrists, windows, mouths, and more.") {
                ModelItemID = "gr_prop_gr_tape_01", FindPercentage = 10,ItemSubType = ItemSubType.Tool },
        });
        PossibleItems.HammerItems.AddRange(new List<HammerItem>
        {
            new HammerItem("Flint Rubber Mallet","Give it a whack") {
                ModelItemID = "gr_prop_gr_hammer_01",ItemSubType = ItemSubType.Tool  },
        });


        PossibleItems.CellphoneItems.AddRange(new List<CellphoneItem> {
            new CellphoneItem("iFruit Cellphone","All of the price, none of the features.") {
                ModelItemID = "prop_phone_ing",
                EmissiveDistance = 25.0f,FindPercentage = 10,EmissiveBrightness = 0.5f,EmissiveRadius = 8.0f,UseFakeEmissive = false,AllowPropRotation = false,   CanSearch = false,ItemSubType = ItemSubType.CellPhone
            },
            new CellphoneItem("Facade Cellphone","Operating system dictators, software monopolists and licensing racketeers.") {
                ModelItemID = "prop_phone_ing_02",
                EmissiveDistance = 25.0f,FindPercentage = 10,EmissiveBrightness = 0.5f,EmissiveRadius = 8.0f,UseFakeEmissive = false,AllowPropRotation = false,CanSearch = false,ItemSubType = ItemSubType.CellPhone
            },
            new CellphoneItem("Badger Cellphone","A first-world global communications company with third-world cell phone coverage.") {
                ModelItemID = "prop_phone_ing_03",
                EmissiveDistance = 25.0f,FindPercentage = 10,EmissiveBrightness = 0.5f,EmissiveRadius = 8.0f,UseFakeEmissive = false,AllowPropRotation = false,CanSearch = false,ItemSubType = ItemSubType.CellPhone
            },
            new CellphoneItem("Celltowa Cellphone","Low end feature phone made in China. It makes calls and not much else.") {
                ModelItemID = "prop_prologue_phone",
                HasFlashlight = false,ItemSubType = ItemSubType.CellPhone, 
            },
        });


        PossibleItems.FlashlightItems.AddRange(new List<FlashlightItem> {
            new FlashlightItem("TAG-HARD Flashlight","Need to beat a suspect, but don't have your nightstick? Look no further.") {
                ModelItemID = "prop_cs_police_torch",
                EmissiveRadius = 10f, EmissiveDistance = 75f,EmissiveBrightness = 0.75f, FindPercentage = 1,ItemSubType = ItemSubType.Flashlight },
            new FlashlightItem("Flint Handle Flashlight","Light up the jobsite, or the dead hookers.") {
                ModelItemID = "prop_tool_torch",
                EmissiveRadius = 15f, EmissiveDistance = 100f,EmissiveBrightness = 1.0f,ItemSubType = ItemSubType.Flashlight },

        });
        PossibleItems.ShovelItems.AddRange(new List<ShovelItem> {
            new ShovelItem("Flint Shovel","A lot of holes in the desert, and a lot of problems are buried in those holes. But you gotta do it right. I mean, you gotta have the hole already dug before you show up with a package in the trunk.") {
                ModelItemID = "prop_tool_shovel" ,ItemSubType = ItemSubType.Tool  },
        });
        PossibleItems.UmbrellaItems.AddRange(new List<UmbrellaItem>
        {
            new UmbrellaItem("GASH Blue Umbrella", "Stay out of the acid rain, now in blue."){ ModelItemID = "p_amb_brolly_01",ItemSubType = ItemSubType.Umbrella  },
            new UmbrellaItem("GASH Black Umbrella", "Stay out of the acid rain in fashionable black.") { ModelItemID = "p_amb_brolly_01_s",ItemSubType = ItemSubType.Umbrella  },
        });
        PossibleItems.BinocularsItems.AddRange(new List<BinocularsItem> {
            new BinocularsItem("SCHEISS BS Binoculars","Not just for peeping toms. Basic and Trusted.") {
                ModelItemID = "prop_binoc_01",HasThermalVision = false,HasNightVision = false, MinFOV = 15f,MidFOV = 35f,MaxFOV = 55f, FindPercentage = 1,ItemSubType = ItemSubType.Binoculars  },
            new BinocularsItem("SCHEISS AS Binoculars","Need to spy on a spouse or loved one? Now with more ZOOM!") {
                ModelItemID = "prop_binoc_01",HasThermalVision = false,HasNightVision = false, MinFOV = 12f,MidFOV = 20f,MaxFOV = 50f, FindPercentage = 1 ,ItemSubType = ItemSubType.Binoculars },
            new BinocularsItem("SCHEISS DS Binoculars","Need to spy on spouse or loved one, but in the dark? We have you covered!") {
                ModelItemID = "prop_binoc_01",HasThermalVision = false,HasNightVision = true, MinFOV = 10f,MidFOV = 20f,MaxFOV = 50f, FindPercentage = 1,ItemSubType = ItemSubType.Binoculars  },
            new BinocularsItem("SCHEISS RP Binoculars","All the bells and whistles. They will never see you coming!") {
                ModelItemID = "prop_binoc_01",HasThermalVision = true,HasNightVision = true, MinFOV = 8f,MidFOV = 20f,MaxFOV = 50f, FindPercentage = 1 ,ItemSubType = ItemSubType.Binoculars },
        });
    }
    private void DefaultConfig_Vehicles()
    {
        PossibleItems.VehicleItems.AddRange(new List<VehicleItem> {
            //Cars & Motorcycles
            new VehicleItem("Albany Alpha", "Blending modern performance and design with the classic luxury styling of a stately car, the Alpha is sleek, sexy and handles so well you'll forget you're driving it. Which could be a problem at 150 mph...", true, ItemType.Vehicles) { ModelName = "alpha" },
            new VehicleItem("Albany Roosevelt","Party like it's the Prohibition era in this armored 1920s limousine. Perfect for a gangster and his moll on their first date or their last. Let the Valentine's Day massacres commence.", true, ItemType.Vehicles) { ModelName = "btype", Description = "Party like it's the Prohibition era in this armored 1920s limousine. Perfect for a gangster and his moll on their first date or their last. Let the Valentine's Day massacres commence.", },
            new VehicleItem("Albany Fränken Stange","The unlikely product of Albany's design team leafing through a vintage car magazine while in the depths of a masculine overdose. The Franken Stange will make you the envy of goths, emo hipsters and vampire wannabes everywhere. Don't be fooled by what's left of its old world charm; the steering linkage may be from 1910, but the engine has just enough horsepower to tear itself (and you) to pieces at the first bump in the road.", true, ItemType.Vehicles) { ModelName = "btype2", Description = "The unlikely product of Albany's design team leafing through a vintage car magazine while in the depths of a masculine overdose. The Franken Stange will make you the envy of goths, emo hipsters and vampire wannabes everywhere. Don't be fooled by what's left of its old world charm; the steering linkage may be from 1910, but the engine has just enough horsepower to tear itself (and you) to pieces at the first bump in the road.", },
            new VehicleItem("Albany Roosevelt Valor","They don't make them like they used to, which is a good thing because here at Albany we've completely run out of ideas. Lovingly remodelled, with room for a new suite of personal modifications, the latest edition of our classic Roosevelt represents a new height of criminal refinement, taking you back to the golden age of fraud, racketeering and murder when all you had to worry about were a few charges of tax evasion.", true, ItemType.Vehicles) { ModelName = "btype3", Description = "They don't make them like they used to, which is a good thing because here at Albany we've completely run out of ideas. Lovingly remodelled, with room for a new suite of personal modifications, the latest edition of our classic Roosevelt represents a new height of criminal refinement, taking you back to the golden age of fraud, racketeering and murder when all you had to worry about were a few charges of tax evasion.", },
            new VehicleItem("Albany Buccaneer","With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", ItemType.Vehicles) { ModelName = "buccaneer", Description = "With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Albany Buccaneer Custom","With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", true, ItemType.Vehicles) { ModelName = "buccaneer2", Description = "With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Albany Cavalcade","You could scarcely cross the street without getting mown down by a soccer mom or drug dealer in one of these during the early 2000s. The glory days of the excessively-large, gas-guzzling SUV might be over, but the Cavalcade takes no prisoners.", ItemType.Vehicles) { ModelName = "cavalcade", Description = "The old man luxury automobile, but once you sit inside this comfy car that steers like a boat, you'll know why your old man often fell asleep at the wheel.", },
            new VehicleItem("Albany Cavalcade 2","The old man luxury automobile, but once you sit inside this comfy car that steers like a boat, you'll know why your old man often fell asleep at the wheel.", ItemType.Vehicles) { ModelName = "cavalcade2", Description = "The old man luxury automobile, but once you sit inside this comfy car that steers like a boat, you'll know why your old man often fell asleep at the wheel.", },
            new VehicleItem("Albany Emperor", ItemType.Vehicles) { ModelName = "emperor" },
            new VehicleItem("Albany Emperor 2", ItemType.Vehicles) { ModelName = "emperor2" },
            new VehicleItem("Albany Emperor 3", ItemType.Vehicles) { ModelName = "emperor3" },
            new VehicleItem("Albany Hermes", true, ItemType.Vehicles) { ModelName = "hermes" },
            new VehicleItem("Albany Lurcher", true, ItemType.Vehicles) { ModelName = "lurcher", Description = "Dismissed as 'over the top' for the 90s pro wrestling circuit, the Albany Lurcher was a car ahead of its time. Since then American society has moved on, and this forgotten gem has been taken to heart as the centerpiece of choice for funeral orgies across the nation. Don't take chances when it comes to your legacy: accessorize your demise, and turn your death into a statement that nobody wants to hear.", },
            new VehicleItem("Albany Manana", ItemType.Vehicles) { ModelName = "manana" },
            new VehicleItem("Albany Manana Custom", true, ItemType.Vehicles) { ModelName = "manana2", Description = "The Manana is the kind of history they don't put in the books. It's the story of every other drive-by and drug deal ever to take place in Los Santos, and there's only one man who can write the next chapter. Benny knows this build down to the last millimeter, he knows how to do it justice, and he knows that taking a power hose to the inside of the trunk is always job one. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Albany Primo", ItemType.Vehicles) { ModelName = "primo" },
            new VehicleItem("Albany Primo Custom", true, ItemType.Vehicles) { ModelName = "primo2", Description = "When cheap imports flooded the market through the 80s and 90s, the US car industry did what any proud free marketeer would do: grab a government subsidy, snort an exe bonus and crap out a four-door sedan every bit as drab and forgettable as its Japanese counterparts. It's not often you find a canvas this black, to go wild - it's not like 2 tons of street mods are going to ruin the performance. Eligible to customization at Benny's Original Motor Works.", },
            new VehicleItem("Albany Virgo", true, ItemType.Vehicles) { ModelName = "virgo", Description = "In a postwar car industry awash with fins, scoops and ornamental hubcaps the Virgo stood out as something more composed and thoughtful. Don't let the rhino-horn fittings, onboard minibar and the fact that it handles like a shipping container on shopping cart wheels deceive you: this is as close as the American car industry has ever come to elegance.", },
            new VehicleItem("Albany V-STR", true, ItemType.Vehicles) { ModelName = "vstr", Description = "Looking for something wild? Climb inside the fierce V-STR and listen to it growl as it bares that distinctive grill and charges the competition. With this much untamed energy, get ready to lose control. Figuratively, obviously. This high-performance luxury sedan is the jewel in Albany's crown.", },
            new VehicleItem("Albany Washington", ItemType.Vehicles) { ModelName = "washington", Description = "Is there a more iconic town car than the Washington? Own a piece of livery history! (Just don't loiter for too long at traffic lights or someone will get in the back and ask you to take them to the airport)", },
            new VehicleItem("Annis Elegy Retro Custom", true, ItemType.Vehicles) { ModelName = "elegy" },
            new VehicleItem("Annis Elegy RH8", ItemType.Vehicles) { ModelName = "elegy2" },
            new VehicleItem("Annis Euros", true, ItemType.Vehicles) { ModelName = "euros" },
            new VehicleItem("Annis Hellion", true, ItemType.Vehicles) { ModelName = "hellion" },
            new VehicleItem("Annis RE-7B", true, ItemType.Vehicles) { ModelName = "le7b", Description = "Fresh from baffling onlookers across the GT circuit, the experimental prototype from Annis is now on limited commercial sale. In a revolutionary design process, Japan's finest artisans, engineers, aeronautics experts, martial artists and chefs have come together to product a seamless extension of your delusions of edginess and accomplishment.", },
            new VehicleItem("Annis Remus", true, ItemType.Vehicles) { ModelName = "remus", Description = "Bow down because you're in the presence of JDM nobility. the dynastic prowess of the Annis Remus is a powerful thing. An example? Just kick the back out through the middle of Legion Square, thinly spread a couple dozen onlookers across the tarmac, and listen to nothing but thunderous applause. Long live the emperor!", },
            new VehicleItem("Annis S80RR", true, ItemType.Vehicles) { ModelName = "s80", Description = "The S80RR was designed to do two things. First, to be the predominant endurance racer of its era. Second, to make so few concessions to the physical comfort and psychological wellbeing of the driver that getting as far as the end of your driveway risks multiple organ failure, an irrecoverable nervous breakdown, and absolutely no regrets whatsoever.", },
            new VehicleItem("Annis Savestra", true, ItemType.Vehicles) { ModelName = "savestra", Description = "Back in the 70s, the Savestra was the car your parents banned from the house after it pissed engine oil on the carpet and dry-humped your dad's Schafter. Taking all the power and presence of a big American sports car and boiling them down to a concentrated dose of rage, this little pit bull is the perfect candidate for a whole suite of brutal mod options, including a light machine gun or two.Please note: Weapon modifications can only be applied at a Vehicle Workshop inside an Avenger or Mobile Operations Center.", },
            new VehicleItem("Annis ZR350", true, ItemType.Vehicles) { ModelName = "zr350" },
            new VehicleItem("Annis Apocalypse ZR380", true, ItemType.Vehicles) { ModelName = "zr380" },
            new VehicleItem("Annis Future Shock ZR380", true, ItemType.Vehicles) { ModelName = "zr3802", Description = "This is the future of the sports class: somewhere between designer purse and a hollow-tip bullet, this can reduce a human body to mincemeat more elegantly than anything else on the planet.", },
            new VehicleItem("Annis Nightmare ZR380", true, ItemType.Vehicles) { ModelName = "zr3803", Description = "This edition of the ZR380 has its origins in that thin line between bright, glossy, high-intensity branding and a hallucinatory breakdown. If you ever wanted to drive a weaponized acid trip, this is your chance.", },
            new VehicleItem("Benefactor Apocalypse Bruiser", true, ItemType.Vehicles) { ModelName = "bruiser" },
            new VehicleItem("Benefactor Future Shock Bruiser", true, ItemType.Vehicles) { ModelName = "bruiser2" },
            new VehicleItem("Benefactor Nightmare Bruiser", true, ItemType.Vehicles) { ModelName = "bruiser3" },
            new VehicleItem("Benefactor Dubsta", ItemType.Vehicles) { ModelName = "dubsta" },
            new VehicleItem("Benefactor Dubsta 2", ItemType.Vehicles) { ModelName = "dubsta2" },
            new VehicleItem("Benefactor Dubsta 6x6", true, ItemType.Vehicles) { ModelName = "dubsta3" },
            new VehicleItem("Benefactor Feltzer", ItemType.Vehicles) { ModelName = "feltzer2" },
            new VehicleItem("Benefactor Stirling GT", true, ItemType.Vehicles) { ModelName = "feltzer3", Description = "This Stirling GT is a landmark of Germany's proud history of helping flabby, self-loathing businessmen pretend they're Grand Prix drivers... for two minutes between freeway traffic jams. It's a tradition that's been around for at least sixty years, so while it's still pathetic, at least it got there first.", },
            new VehicleItem("Benefactor Glendale", true, ItemType.Vehicles) { ModelName = "glendale", Description = "What's known in the trade as a 'drug dealers car'. 1990s luxury German four door sedan turned 2010s roving depot for stepped-on cocaine. Expect to get stopped by the cops and stuck up by junkies.", },
            new VehicleItem("Benefactor Glendale Custom", true, ItemType.Vehicles) { ModelName = "glendale2", Description = "Life isn't about being easy. It's about looking easy. That's why you have your jeans lovingly pre-distressed and slow shipping from East Asia for that authentic well-traveled look. Or why you just remortgaged your house for the best bed-hair in town. Or why you never scream your ex's name in bed. And it's why you're going to buy the most iconic mob car in American history, then let Benny show you how he puts the effort in effortlessly cool. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Benefactor Turreted Limo", true, ItemType.Vehicles) { ModelName = "limo2", Description = "Bored during rush hour? Need to make an impression when a dictator is in town? Sick of having to choose between a comfort and status of a limousine and the sheer utility of an Armored Personnel Carrier? German engineering once again provides the kind of efficient, goal-oriented solution the modern businessman needs by sticking a minigun on top of a stretch limo. It's obvious once you see it.", },
            new VehicleItem("Benefactor BR8", true, ItemType.Vehicles) { ModelName = "openwheel1" },
            new VehicleItem("Benefactor Panto", true, ItemType.Vehicles) { ModelName = "panto" },
            new VehicleItem("Benefactor Schafter", "Good-looking yet utilitarian, sexy yet asexual, slender yet terrifyingly powerful, the Schafter is German engineering at its very finest.", ItemType.Vehicles) { ModelName = "schafter2", Description = "Good-looking yet utilitarian, sexy yet asexual, slender yet terrifyingly powerful, the Schafter is German engineering at its very finest.", },
            new VehicleItem("Benefactor Schafter V12", "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has a V12 engine.", true, ItemType.Vehicles) { ModelName = "schafter3", Description = "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. Armored edition available.", },
            new VehicleItem("Benefactor Schafter LWB", "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has an extended wheelbase.", true, ItemType.Vehicles) { ModelName = "schafter4", Description = "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has an extended wheelbase. Armored edition available.", },
            new VehicleItem("Benefactor Schafter V12 (Armored)", true, ItemType.Vehicles) { ModelName = "schafter5", Description = "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. Armored edition available.", },
            new VehicleItem("Benefactor Schafter LWB (Armored)", true, ItemType.Vehicles) { ModelName = "schafter6", Description = "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has an extended wheelbase. Armored edition available.", },
            new VehicleItem("Benefactor Schwartzer", "Say what you will about the Germans - they know luxury. And their economy is the only one worth a crap in Europe. This model has all kinds of extras - too many to list for legal reasons.", ItemType.Vehicles) { ModelName = "schwarzer", Description = "Say what you will about the Germans - they know luxury. And their economy is the only one worth a crap in Europe. This model has all kinds of extras - too many to list for legal reasons.", },
            new VehicleItem("Benefactor Serrano", "Fun fact: what's the fastest growing market in the American auto industry? That's right! Compact SUVs! And do you know why? That's right! Neither do we! And is that a good enough reason to buy one? That's right! It had better be!", ItemType.Vehicles) { ModelName = "serrano" },
            new VehicleItem("Benefactor Surano", "This is luxury reasserted. Right in your neighbour's face. Boom. You like that. That's right, you are better than him, and you could have his wife if you wanted. Try it on with her as soon as she sees this ride. You'll be a double benefactor.", ItemType.Vehicles) { ModelName = "surano", Description = "This is luxury reasserted. Right in your neighbour's face. Boom. You like that. That's right, you are better than him, and you could have his wife if you wanted. Try it on with her as soon as she sees this ride. You'll be a double benefactor.", },
            new VehicleItem("Benefactor XLS", true, ItemType.Vehicles) { ModelName = "xls", Description = "Poised delicately between BAWSAQ chic and bone-crunching utility, the Benefactor XLS is every car to everyone. Whether you're attending a board meeting halfway up a rugged cliff face or ferrying humane remains out of your place of work, it's reassuring to know the Germans have got your back. Armored version available, obviously.", },
            new VehicleItem("Benefactor XLS (Armored)", true, ItemType.Vehicles) { ModelName = "xls2", Description = "Poised delicately between BAWSAQ chic and bone-crunching utility, the Benefactor XLS is every car to everyone. Whether you're attending a board meeting halfway up a rugged cliff face or ferrying humane remains out of your place of work, it's reassuring to know the Germans have got your back. Armored version available, obviously.", },
            new VehicleItem("Benefactor Krieger", true, ItemType.Vehicles) { ModelName = "krieger", Description = "It sounds so simple in theory. Why shouldn't the cutting edge of hypercar design dovetail seamlessly with the bleeding edge of competition-ready, open-wheel racing tech? Well, forget the theory. Sit in the driver's seat. Take a breath, pucker up, and shoot a tentative glance in the direction of the throttle. Did you feel that? Have you noticed that you are suddenly a mile and a half into the next time zone, and your face is inside out? Yeah. That's why.", },
            new VehicleItem("Benefactor Schlagen GT", true, ItemType.Vehicles) { ModelName = "schlagen", Description = "Looking at the low, broad, surly form of the Schlagen for the first time, you'd be forgiven for thinking that it's just waiting for an opportunity to knock you out, steal your keys and organize a gang bang with the Pfisters in your high-end garage. Of course, appearances can be deceptive. Not in this case, admittedly, but it's a good rule of thumb.", },
            new VehicleItem("Benefactor Streiter", true, ItemType.Vehicles) { ModelName = "streiter", Description = "Look in the mirror and what do you see? Is it a flabby, pallid investment manager with a solitary sex life and spiraling personal debts? It's OK, you don't need to answer that - you're only seeing this ad because we've datamined your Lifeinvader profile. And statistically, we know your next step is going to be the impulsive purchase of a 4x4 that you'll never take out of the city - which is where the Streiter comes in.", },
            new VehicleItem("Benefactor Terrorbyte", true, ItemType.Vehicles) { ModelName = "terbyte" },
            new VehicleItem("BF Injection", ItemType.Vehicles) { ModelName = "BfInjection" },
            new VehicleItem("BF Bifta", true, ItemType.Vehicles) { ModelName = "bifta" },
            new VehicleItem("BF Club", true, ItemType.Vehicles) { ModelName = "club" },
            new VehicleItem("BF Dune Buggy", ItemType.Vehicles) { ModelName = "dune" },
            new VehicleItem("BF Dune FAV", true, ItemType.Vehicles) { ModelName = "dune3" },
            new VehicleItem("BF Raptor", true, ItemType.Vehicles) { ModelName = "raptor", Description = "One driver, two seats, three wheels, a straight four under the hood, and five minutes before you're upside down on the freeway wondering how any of this ever made any sense at all. It's the kind of package deal you can only get from BF.", },
            new VehicleItem("BF Surfer", ItemType.Vehicles) { ModelName = "SURFER" },
            new VehicleItem("BF Surfer", ItemType.Vehicles) { ModelName = "Surfer2" },
            new VehicleItem("BF Weevil", true, ItemType.Vehicles) { ModelName = "weevil", Description = "You could pop up in a mood ring, rock a stack perm, roll a doobie and crash the stock market. Or, for a faster 70s vibe fix, you could just purchase the Weevil. Some fashions are timeless.", },
            new VehicleItem("Bollokan Prairie", ItemType.Vehicles) { ModelName = "prairie", Description = "Bollokan's first and only production car was already in showrooms before someone told them that 'Bollokan' didn't sound anything like a fashionable German brand, and the car sucked ass.", },
            new VehicleItem("Bravado Banshee", ItemType.Vehicles) { ModelName = "banshee" },
            new VehicleItem("Bravado Banshee 900R", true, ItemType.Vehicles) { ModelName = "banshee2", Description = "The Banshee defines the modern sports class. Light, low, with sweeping curves and perfect lines, the only thing under its mile-long hood is a feral V8 twin-turbo fighting for space with its driver's colossal manhood. But trust us, the base model is just the start. When we're done, it'll look like your Banshee ate another Banshee at the peak of an all-night steroid binge. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Bravado Bison", ItemType.Vehicles) { ModelName = "bison", OverrideLoadBodiesInBed = true, BedLoadOffsetOverride = new Vector3(0f,-1.5f,1.25f) },
            new VehicleItem("Bravado Bison 2", ItemType.Vehicles) { ModelName = "Bison2", OverrideLoadBodiesInBed = true, BedLoadOffsetOverride = new Vector3(0f,-1.5f,1.25f) },
            new VehicleItem("Bravado Bison 3", ItemType.Vehicles) { ModelName = "Bison3", OverrideLoadBodiesInBed = true, BedLoadOffsetOverride = new Vector3(0f,-1.5f,1.25f) },
            new VehicleItem("Bravado Buffalo", ItemType.Vehicles) { ModelName = "buffalo" },
            new VehicleItem("Bravado Buffalo S", ItemType.Vehicles) { ModelName = "buffalo2" },
            new VehicleItem("Bravado Sprunk Buffalo", ItemType.Vehicles) { ModelName = "buffalo3", Description = "Just like the standard Bravado it's another resurrected 1960s muscle car for the over-muscled EDM generation - but this time featuring exclusive Sprunk Livery! ", },
            new VehicleItem("Bravado Duneloader", ItemType.Vehicles) { ModelName = "dloader" },
            new VehicleItem("Bravado Gauntlet", ItemType.Vehicles) { ModelName = "gauntlet", Description = "An American muscle car in a class by itself. Roll down the windows and scream in testosterone-filled rage as you gun the engine while stuck in traffic. DNA wipes easily off the leather seats.", },
            //new VehicleItem("Bravado Redwood Gauntlet", ItemType.Vehicles) { ModelName = "gauntlet2", Description = "An American muscle car in a class by itself. Roll down the windows and scream in testosterone-filled rage as you gun the engine while stuck in traffic. DNA wipes easily off the leather seats. Now available with exclusive Redwood Livery.", },
            new VehicleItem("Bravado Gauntlet Classic", true, ItemType.Vehicles) { ModelName = "gauntlet3", Description = "In this world, there's muscle, and then there's muscle. Like, you technically have muscles. And then there's that guy you went to school with. The one who posts Snapmatic selfies that look like someone spray-tanned a T-bone steak. Well, the Bravado Gauntlet just ate that guy, washed him down with a supercharged protein shake, and threw its arm round his mom at the drive-in. This is muscle. Anything else is just flab.", },
            new VehicleItem("Bravado Gauntlet Hellfire", true, ItemType.Vehicles) { ModelName = "gauntlet4", Description = "What is a car for, if it's not for getting from A to B? And what's the point of A and B, if they're not a quarter of a mile apart on a flat surface in a straight line? These are the questions Bravado asked themselves, and the Hellfire was the answer. Give it what it needs, and this thing will immolate just about everything else on four wheels. Turn that funny-looking wheel in the middle of the dash even a little bit, and you're as good as dead.", },
            new VehicleItem("Bravado Gauntlet Classic Custom", true, ItemType.Vehicles) { ModelName = "gauntlet5", Description = "The Temptation of Aglaia, Yellow Dog with Cone, the Bravado Gauntlet Classic Custom. Some works of art are too perfect to tamper with. Unless, of course, you're Benny. When a car has room for this many mods, it would be an artistic crime to ignore your reckless creative impulses. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Bravado Gresley", ItemType.Vehicles) { ModelName = "gresley", Description = "The all-new Bravado Gresley promises to be the most fuel-efficient SUV in its class, guaranteed to get you from brunch to yoga to the kids' soccer practice on one tank, or your money back.", },
            new VehicleItem("Bravado Half-track", true, ItemType.Vehicles) { ModelName = "halftrack" },
            new VehicleItem("Bravado Apocalypse Sasquatch", true, ItemType.Vehicles) { ModelName = "monster3" },
            new VehicleItem("Bravado Future Shock Sasquatch", true, ItemType.Vehicles) { ModelName = "monster4" },
            new VehicleItem("Bravado Nightmare Sasquatch", true, ItemType.Vehicles) { ModelName = "monster5" },
            new VehicleItem("Bravado Paradise", true, ItemType.Vehicles) { ModelName = "paradise" },
            new VehicleItem("Bravado Rat-Truck", true, ItemType.Vehicles) { ModelName = "ratloader2", Description = "Mint condition Rat-Loader. This 1930s pickup truck looks like you just drove it out of the Bravado dealership with a quart of moonshine in your pocket and the great depression on your mind.", },
            new VehicleItem("Bravado Rumpo", ItemType.Vehicles) { ModelName = "rumpo" },
            new VehicleItem("Bravado Rumpo 2", ItemType.Vehicles) { ModelName = "rumpo2" },
            new VehicleItem("Bravado Rumpo Custom", true, ItemType.Vehicles) { ModelName = "rumpo3" },
            new VehicleItem("Bravado Verlierer", true, ItemType.Vehicles) { ModelName = "verlierer2", Description = "The Verlierer has all the looks of a 60s roadster, with the added advantage of being able to drive around corners. Combining its sophisticated looks with tight handling and the forward momentum of a hungry leopard, the only thing getting in your way here is your lack of coordination and fear of becoming a road accident statistic.", },
            new VehicleItem("Bravado Youga", ItemType.Vehicles) { ModelName = "youga" },
            new VehicleItem("Bravado Youga Classic", true, ItemType.Vehicles) { ModelName = "youga2" },
            new VehicleItem("Bravado Youga Classic 4x4", true, ItemType.Vehicles) { ModelName = "youga3" },
            new VehicleItem("Brute Boxville", ItemType.Vehicles) { ModelName = "boxville" },
            new VehicleItem("Brute Boxville 3", ItemType.Vehicles) { ModelName = "boxville3" },
            new VehicleItem("Brute Boxville 4", true, ItemType.Vehicles) { ModelName = "boxville4" },
            new VehicleItem("Brute Camper", ItemType.Vehicles) { ModelName = "CAMPER" },
            new VehicleItem("Brute Pony", ItemType.Vehicles) { ModelName = "pony" },
            new VehicleItem("Brute Pony 2", ItemType.Vehicles) { ModelName = "pony2" },
            new VehicleItem("Brute Stockade", ItemType.Vehicles) { ModelName = "stockade" },
            new VehicleItem("Brute Stockade 3", ItemType.Vehicles) { ModelName = "stockade3" },
            new VehicleItem("Brute Tipper", ItemType.Vehicles) { ModelName = "TipTruck" },
            new VehicleItem("Canis Bodhi", ItemType.Vehicles) { ModelName = "Bodhi2", OverrideLoadBodiesInBed = true, BedLoadOffsetOverride = new Vector3(0f,-1.5f,1.25f) },
            new VehicleItem("Canis Crusader", ItemType.Vehicles) { ModelName = "CRUSADER" },
            new VehicleItem("Canis Freecrawler", true, ItemType.Vehicles) { ModelName = "freecrawler" },
            new VehicleItem("Canis Kalahari", true, ItemType.Vehicles) { ModelName = "kalahari" },
            new VehicleItem("Canis Kamacho", true, ItemType.Vehicles) { ModelName = "kamacho" },
            new VehicleItem("Canis Mesa", ItemType.Vehicles) { ModelName = "MESA", OverrideTrunkAttachment = true, TrunkAttachOffsetOverride = new Vector3(0f,-0.75f,0.5f) },
            new VehicleItem("Canis Mesa 2", ItemType.Vehicles) { ModelName = "mesa2", OverrideTrunkAttachment = true, TrunkAttachOffsetOverride = new Vector3(0f,-0.75f,0.5f) },
            new VehicleItem("Canis Mesa 3", ItemType.Vehicles) { ModelName = "MESA3" , OverrideTrunkAttachment = true, TrunkAttachOffsetOverride = new Vector3(0f,-1f,0.6f)},
            new VehicleItem("Canis Seminole", ItemType.Vehicles) { ModelName = "seminole", Description = "A metal cage soldered to a wheel chassis isn't everybody's first choice of car, which is why Canis decided to take their signature off-road car model, encase it in some flimsy bodywork and re-market it as a 'Family SUV.'", },
            new VehicleItem("Canis Seminole Frontier", true, ItemType.Vehicles) { ModelName = "seminole2", Description = "When a responsible adult buys a car, it's a question of making sensible compromises. Your heart wants a feral, mountain-chewing 4x4 with bullbars and more torque than a Panzer division. Your spouse wants a family SUV with childproof locks, powerful aircon, and an airbag so big it'll break your jaw if a fly hits the bumper. But look around you. Do we live in the age of responsible adults or sensible compromises? Enter the Seminole Frontier. Be safe, look reckless, have it all.", },
            new VehicleItem("Chariot Romero Hearse", ItemType.Vehicles) { ModelName = "romero" },
            new VehicleItem("Cheval Fugitive", ItemType.Vehicles) { ModelName = "fugitive", Description = "The Fugitive is the go-to cruiser for law enforcement and those that want to pretend they are law enforcement. The gas mileage isn't so great, but cops mostly sit with the car idling anyway.", },
            //new VehicleItem("Cheval Marshall", ItemType.Vehicles) { ModelName = "marshall" },
            new VehicleItem("Cheval Picador", ItemType.Vehicles) { ModelName = "picador" },
            new VehicleItem("Cheval Surge", ItemType.Vehicles) { ModelName = "surge", Description = "Take shit from your gas guzzling buddies, and spend hours at charging stations just to get mistaken for a rideshare driver. The Cheval Surge turns the zero emissions dream into a dull reality.", },
            new VehicleItem("Cheval Taipan", true, ItemType.Vehicles) { ModelName = "taipan", Description = "Human-led design is a thing of the past. This is what happens when you fire your R&amp;D department and leave a supercomputer alone with a textbook on computational fluid dynamics and some provocative anime. End result: to drive a Taipan is to put yourself at the mercy of a ruthless, inhuman dedication to pure speed and improbable curves. Be afraid.", },
            new VehicleItem("Coil Brawler", true, ItemType.Vehicles) { ModelName = "brawler" },
            new VehicleItem("Coil Cyclone", true, ItemType.Vehicles) { ModelName = "cyclone", Description = "The Coil Cyclone is here to prove one thing: the days of the internal combustion engine are over. Sure, it was fun while it lasted. Just like your psychotic, knife-wielding ex was phenomenal in bed. But that fossil-fuelled comfort zone is about to be nothing more than a distant speck in your rear-view mirror as you surrender to this harbinger of the electric age. True power is here. Drive the lightning.", },
            new VehicleItem("Coil Raiden", true, ItemType.Vehicles) { ModelName = "raiden", Description = "The Raiden is a masterpiece of understatement. If it pulled up next to you while you were slumped over, sobbing at the lights, you wouldn't bother to look up from your ex's Snapmatic profile. But then the lights go green, and you see it put down the kind of noiseless acceleration that internal combustion can only dream of. Your iFruit falls from your snotty grip, and you think: maybe the world's not so bad after all.", },
            new VehicleItem("Coil Voltic", ItemType.Vehicles) { ModelName = "voltic", Description = "The Voltic was the first highway-capable, all-electric sports car on the market in the United States. Boasts a battery life shorter than your iFruit phone so that you can still call a cab home when you grind to a halt in the middle of nowhere.", },
            new VehicleItem("Coil Rocket Voltic", true, ItemType.Vehicles) { ModelName = "voltic2", Description = "There's a very sound reason we don't strap space shuttle parts onto sports cars. But no one in the boardroom at Coil knew what that reason was, so here we are. Once you hit the button you're more likely to get into orbit than stay on the road, and no one has yet survived either outcome to tell us if it was worth it. Probably was though.Note, the production model of this vehicle has a longer recharge time on uses of the rocket burst.", },
            new VehicleItem("Declasse Asea", ItemType.Vehicles) { ModelName = "asea", Description = "An affordable, no frills, fuel-efficient compact sedan. When 'ample headroom' is central to the marketing campaign, what you see is what you get.", },
            new VehicleItem("Declasse Asea2", ItemType.Vehicles) { ModelName = "asea2" },
            new VehicleItem("Declasse Apocalypse Brutus", true, ItemType.Vehicles) { ModelName = "brutus" },
            new VehicleItem("Declasse Future Shock Brutus", true, ItemType.Vehicles) { ModelName = "brutus2" },
            new VehicleItem("Declasse Nightmare Brutus", true, ItemType.Vehicles) { ModelName = "brutus3" },
            new VehicleItem("Declasse Burrito", ItemType.Vehicles) { ModelName = "Burrito" },
            new VehicleItem("Declasse Bugstars Burrito", ItemType.Vehicles) { ModelName = "burrito2" },
            new VehicleItem("Declasse Burrito 3", ItemType.Vehicles) { ModelName = "burrito3" },
            new VehicleItem("Declasse Burrito 4", ItemType.Vehicles) { ModelName = "Burrito4" },
            new VehicleItem("Declasse Burrito 5", ItemType.Vehicles) { ModelName = "burrito5" },
            new VehicleItem("Declasse Gang Burrito", ItemType.Vehicles) { ModelName = "gburrito" },
            new VehicleItem("Declasse Gang Burrito 2", true, ItemType.Vehicles) { ModelName = "gburrito2" },
            new VehicleItem("Declasse Granger", ItemType.Vehicles) { ModelName = "granger", Description = "Marketed heavily on patriotism, a Granger commercial always includes scenes of strong Americans baling hay and winning wars. Strictly speaking, this full-size pickup is still 'made in America'... If you include Central and South America.", },
            new VehicleItem("Declasse Hotring Sabre", true, ItemType.Vehicles) { ModelName = "hotring", Description = "You're either a Hotringer, or you're not, and here's how you tell. If your first instinct isn't to crack a beer on the radiator grille, polish the hood with a mouthful of spit, slap it on the rear fender, call it a 'classy gal' and belch the national anthem, then you're not the target audience. On your way.", },
            new VehicleItem("Declasse Impaler", true, ItemType.Vehicles) { ModelName = "impaler", Description = "Today's muscle cars might look shredded, but we all know the gains aren't real. Deep down they're all juiced-up phoneys with nothing but a short temper and a tiny exhaust. If you want some all-natural brawn, you need go old school. The big block under the Impaler's hood trains by chopping wood, benching peaceniks, and deriding the weak - and there's just no faking that.", },
            new VehicleItem("Declasse Apocalypse Impaler", true, ItemType.Vehicles) { ModelName = "impaler2", Description = "Tetanus shots strongly recommended before purchase.", },
            new VehicleItem("Declasse Future Shock Impaler", true, ItemType.Vehicles) { ModelName = "impaler3", Description = "The most terrifying luxury car of the 2260's.", },
            new VehicleItem("Declasse Nightmare Impaler", true, ItemType.Vehicles) { ModelName = "impaler4", Description = "A fresh hole in the ozone layer with every purchase!", },
            new VehicleItem("Declasse Lifeguard", ItemType.Vehicles) { ModelName = "lguard" },
            new VehicleItem("Declasse Mamba", true, ItemType.Vehicles) { ModelName = "mamba" },
            new VehicleItem("Declasse Moonbeam", true, ItemType.Vehicles) { ModelName = "moonbeam" },
            new VehicleItem("Declasse Moonbeam Custom", true, ItemType.Vehicles) { ModelName = "moonbeam2", Description = "It may look like a sturdy, affordable minivan with high seating capacity and low build quality. Sure. Its manufacturers were happy with a complete lack of features and utter disregard for good looks. But mindless acceptance of obvious facts did not make this country what it is today. Forget about what the Moonbeam is, and start thinking about what a stunted adolescent with thousands of dollars can pay for it to be. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Declasse DR1", true, ItemType.Vehicles) { ModelName = "openwheel2" },
            new VehicleItem("Declasse Premier", ItemType.Vehicles) { ModelName = "premier" },
            new VehicleItem("Declasse Rancher XL", ItemType.Vehicles) { ModelName = "RancherXL" },
            new VehicleItem("Declasse Rancher XL 2", ItemType.Vehicles) { ModelName = "rancherxl2" },
            new VehicleItem("Declasse Rhapsody", true, ItemType.Vehicles) { ModelName = "rhapsody" },
            new VehicleItem("Declasse Sabre Turbo", ItemType.Vehicles) { ModelName = "sabregt", Description = "Tricking out a near-perfect muscle car like the Sabre is a fine art. A mainstream mod shop won't understand that its deafening wheelspin isn't inefficient - it's the foreplay a car like this requires. Its brakes aren't dangerously unresponsive, they're smooth and gradual. The lack of protection isn't careless, it's thrilling. Luckily for you, here at Benny's we can serve a whole lot of style without adding one tiny bit of substance. It's what a car this stupid deserves.", },
            new VehicleItem("Declasse Sabre Turbo Custom", true, ItemType.Vehicles) { ModelName = "sabregt2", Description = "Tricking out a near-perfect muscle car like the Sabre is a fine art. A mainstream mod shop won't understand that its deafening wheelspin isn't inefficient - it's the foreplay a car like this requires. Its brakes aren't dangerously unresponsive, they're smooth and gradual. The lack of protection isn't careless, it's thrilling. Luckily for you, here at Benny's we can serve a whole lot of style without adding one tiny bit of substance. It's what a car this stupid deserves.", },
            new VehicleItem("Declasse Scramjet", true, ItemType.Vehicles) { ModelName = "scramjet", Description = "The Declasse Scramjet is final proof that the greatest inventions are accidents. No one thought we had any need to jolt a hyper-stylized retro speedster thirty feet into the air and turbo boost it into the side of the nearest high-rise. But once it happened, and we saw the kind of potential it unleashed, there was no going back. This one's for the dreamers.", },
            new VehicleItem("Declasse Stallion", ItemType.Vehicles) { ModelName = "stalion", Description = "Get 'em while they're hot. The classic Stallion muscle car's been imported from Liberty City to Los Santos. You might find some baggies or baggy rubbers tucked into the back seat, but that's part of this lady's charm. She's a dime.", },
            //new VehicleItem("Declasse Burger Shot Stallion", ItemType.Vehicles) { ModelName = "stalion2", Description = "Get 'em while they're hot. The classic Stallion muscle car's been imported from Liberty City to Los Santos. You might find some baggies or baggy rubbers tucked into the back seat, but that's part of this lady's charm. She's a dime. Featuring exclusive Burger Shot Livery.", },
            new VehicleItem("Declasse Tampa", true, ItemType.Vehicles) { ModelName = "tampa" },
            new VehicleItem("Declasse Drift Tampa", true, ItemType.Vehicles) { ModelName = "tampa2" },
            new VehicleItem("Declasse Weaponized Tampa", true, ItemType.Vehicles) { ModelName = "tampa3", Description = "Back in the 60's a heavy muscle car with a reinforced frame seemed like a great idea because of all the drunk driving you needed to do. Still, perfect though it seemed, a part of you whispered the car was missing something - and trust us, when you see that sturdy foundation supporting a top-mounted minigun the final piece of the jigsaw is going to slot right into place. Throw in some industrial-grade armor and the Tampa will finally have achieved its full potential.", },
            new VehicleItem("Declasse Tornado", ItemType.Vehicles) { ModelName = "tornado", Description = "You never forget the first time you sit behind the wheel of a mint condition Declasse Tornado: that effortless class, that shameless bulge in your pants or wetspot on the seat, that dawning certainty that you're still going to be crawling up this gentle suburban hill fifteen hours from now. Still, with our help, onlookers will be so floored by your paint job they won't even take a Snapmatic selfie in front of the impressive smoke clouds coming out your hood.", },
            new VehicleItem("Declasse Tornado 2", ItemType.Vehicles) { ModelName = "tornado2" },
            new VehicleItem("Declasse Tornado 3", ItemType.Vehicles) { ModelName = "tornado3" },
            new VehicleItem("Declasse Tornado 4", ItemType.Vehicles) { ModelName = "tornado4" },
            new VehicleItem("Declasse Tornado Custom", true, ItemType.Vehicles) { ModelName = "tornado5", Description = "You never forget the first time you sit behind the wheel of a mint condition Declasse Tornado: that effortless class, that shameless bulge in your pants or wetspot on the seat, that dawning certainty that you're still going to be crawling up this gentle suburban hill fifteen hours from now. Still, with our help, onlookers will be so floored by your paint job they won't even take a Snapmatic selfie in front of the impressive smoke clouds coming out your hood.", },
            new VehicleItem("Declasse Tornado Rat Rod", true, ItemType.Vehicles) { ModelName = "tornado6", Description = "Who can say when the innovative hot rod designs of the 30s and 40s shaded into the grungy rat rod counterculture of subsequent decades? And who can say when that genre was overrun by mediocre welders with endless disposable income mutilating good cars and jacking off to their own edginess in the back seat? What we can say is that this beauty comes with a moist towel as standard.", },
            new VehicleItem("Declasse Tulip", true, ItemType.Vehicles) { ModelName = "tulip", Description = "Can you believe something this beautiful was once built and sold for nothing more than Joe Lunchbucket's paycheck? Sure, the Seventies were a crazy time, but what the hell was that about? Luckily for you, civilization is back on track, and these days Joe's food stamps don't count for much when you and the rest of the one percent are dropping last night's poker chips for a slice of his pawpaw's blue-collar chic.", },
            new VehicleItem("Declasse Vamos", true, ItemType.Vehicles) { ModelName = "vamos", Description = "Believe it or not, the Vamos began life in 1960 as a sensible, affordable compact car. But over the course of the decade, something wonderful happened. It moved out of its parents' house and started hanging around with V8's and fastbacks. Its wheelbase lengthened, its grille expanded, and its hood got so long and flat you could spend a whole summer of love on it. These days, it's exactly the kind of bad influence you were looking for.", },
            new VehicleItem("Declasse Vigero", ItemType.Vehicles) { ModelName = "vigero" },
            new VehicleItem("Declasse Voodoo Custom", true, ItemType.Vehicles) { ModelName = "voodoo", Description = "From the bestseller list of the 60s to the driveway of every self-respecting pimp and gangbanger of the 80s, the Voodoo is your best shot at purchasing the grit and authenticity that died out a decade before you were born. This one may look like it's been left under a bypass and used as a makeshift latrine by a family of hobos, but don't worry - with enough money you can change everything but the stink. This is where a classic starts. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Declasse Voodoo", ItemType.Vehicles) { ModelName = "voodoo2" },
            new VehicleItem("Declasse Yosemite", true, ItemType.Vehicles) { ModelName = "yosemite", Description = "With a vehicle as seminal as the Yosemite, it's hard to know where to start. You could talk about the drop-center ladder frame and the low slung cabin. You could talk about the independent front suspension. Or you could talk about how this thing has been slammed so hard you'll have to be careful driving over fallen leaves. But at the end of the day, you won't be talking at all, because you'll be too busy grinning like an idiot whenever you're behind the wheel.", },
            new VehicleItem("Declasse Drift Yosemite", true, ItemType.Vehicles) { ModelName = "yosemite2", Description = "You've customized your other Yosemite so it's a flame-liveried, chrome-engined, four-wheeled hole in the ozone layer and you don't think it can get any more ridiculous? Well, hold my beer. The Drift Yosemite turns a classic pickup truck into some kind of nightmare beast with an insatiable appetite for asphalt. Arrive on the track and other racers will run in fear. Drive it on the road and watch mouths open in awe. A mad genius redesigned this bad boy from the ground up and all you need to do is hand over your credit card.", },
            new VehicleItem("Declasse Yosemite Rancher", true, ItemType.Vehicles) { ModelName = "yosemite3" },
            new VehicleItem("Dewbauchee Exemplar", ItemType.Vehicles) { ModelName = "exemplar", Description = "The British are well known for their superior auto manufacturing prowess. The company has provided cars for counts, Grand Prix, and doughy dignitaries across the UK.", },
            new VehicleItem("Dewbauchee JB 700", ItemType.Vehicles) { ModelName = "jb700", Description = "Grease up the drive shaft because this hot number has curves in all the right places. A classic luxury grand tourer, the JB 700 has been the car of choice for drunken misogynistic British spies since 1965. Dust off the tuxedo, quip a sexual double-entendre, pour yourself a martini for the road, and stumble out as everyone in the room mutters what a turd you are. For safety reasons this vehicle's concealed weapons have been decommissioned.", },
            new VehicleItem("Dewbauchee JB 700W", true, ItemType.Vehicles) { ModelName = "jb7002", Description = "Grease up the drive shaft because this hot number has curves in all the right places. A classic luxury grand tourer, the JB 700W has been the car of choice for drunken misogynistic British spies since 1965. Dust off the tuxedo, quip a sexual double-entendre, pour yourself a martini for the road, and stumble out as everyone in the room mutters what a turd you are.Note: The vehicle's classic concealed weapons can be recommissioned for recreational use at LS Customs.", },
            new VehicleItem("Dewbauchee Massacro", true, ItemType.Vehicles) { ModelName = "massacro", Description = "Sophisticated, superior, class-obsessed and with more than a little aggression under the hood, this grand tourer from Dewbauchee is as classically British as they come.", },
            new VehicleItem("Dewbauchee Massacro (Racecar)", true, ItemType.Vehicles) { ModelName = "massacro2", Description = "Sophisticated, superior, class-obsessed and with more than a little aggression under the hood, this grand tourer from Dewbauchee is as classically British as they come. Race tuned special edition.", },
            new VehicleItem("Dewbauchee Rapid GT", ItemType.Vehicles) { ModelName = "rapidgt", Description = "The ultimate blend of luxury, refinement and breeding, the Rapid GT is a British supercar that's as superior and classist as its fellow countrymen, but without the need for self-deprecation.", },
            new VehicleItem("Dewbauchee Rapid GT 2", ItemType.Vehicles) { ModelName = "RapidGT2" },
            new VehicleItem("Dewbauchee Rapid GT Classic", true, ItemType.Vehicles) { ModelName = "rapidgt3", Description = "Everything else from the 80s has aged. The Rapid GT Classic has only matured. Like a fine claret or a really good boob job, each passing year adds something mysterious to its allure. Sure, that new sports coupÃ© is half the weight, and it has a cleaner transmission. But trust us: get inside this vintage, experience the kind of technique and self-assurance only time can bestow, and you'll never waste your time on a younger model again.", },
            new VehicleItem("Dewbauchee Seven-70", true, ItemType.Vehicles) { ModelName = "seven70", Description = "Dewbauchee only made a handful of these beauties, and that's not just because of their track record of corporate embezzlement. No, it's a result of a commitment to individual aspiration and strict exclusivity that stems from a deeply held belief that the one percent is the only percent that matters. And if that sounds like an opportunity to prove something, it is.", },
            new VehicleItem("Dewbauchee Specter", true, ItemType.Vehicles) { ModelName = "specter", Description = "Imagine the most exclusive two-door sports car on the planet. Now imagine gutting it, and transforming it into a lightweight, liveried, carbon-fiber racer. Why, you ask? Well, why did we go to the moon? Why do we build to the stars? Why do we watch celebrity sex tapes instead of proper porn? Exactly. Now stop asking perfectly legitimate questions and get out your damn wallet.", },
            new VehicleItem("Dewbauchee Specter Custom", true, ItemType.Vehicles) { ModelName = "specter2", Description = "Imagine the most exclusive two-door sports car on the planet. Now imagine gutting it, and transforming it into a lightweight, liveried, carbon-fiber racer. Why, you ask? Well, why did we go to the moon? Why do we build to the stars? Why do we watch celebrity sex tapes instead of proper porn? Exactly. Now stop asking perfectly legitimate questions and get out your damn wallet.", },
            new VehicleItem("Dewbauchee Vagner", true, ItemType.Vehicles) { ModelName = "vagner", Description = "This is what you get when you start from a truly blank slate. Take every preconception you had about hypercar design: every piece of received wisdom, every rock-solid assumption, every tried and tested formula - take them all and dump a hot, steaming pile of filthy ingenuity all over them. The Vagner is a message from the future: you're late.", },
            new VehicleItem("Dinka Akuma", ItemType.Vehicles) { ModelName = "akuma", Description = "A Japanese crotch rocket sure to please the ladies.", },
            new VehicleItem("Dinka Blista", ItemType.Vehicles) { ModelName = "blista" },
            new VehicleItem("Dinka Blista Compact", ItemType.Vehicles) { ModelName = "blista2" },
           // new VehicleItem("Dinka Go Go Monkey Blista", ItemType.Vehicles) { ModelName = "blista3" },
            new VehicleItem("Dinka Double-T", ItemType.Vehicles) { ModelName = "double" },
            new VehicleItem("Dinka Enduro", true, ItemType.Vehicles) { ModelName = "enduro", Description = "Cross country bike that can literally go across the country. Over-sized gas tank, increased durability, long-travel suspension, blood guards. You'll want one of these in your garage when society inevitably collapses.", },
            new VehicleItem("Dinka Jester", true, ItemType.Vehicles) { ModelName = "jester", Description = "A Japanese hybrid-electric sportscar with a front-end designed to look like an angry grin might be too whimsical for some, but with a 4-liter V6 engine, 420 hp and a top speed of 180mph, the Dinka Jester still packs a serious punchline.", },
            new VehicleItem("Dinka Jester (Racecar)", true, ItemType.Vehicles) { ModelName = "jester2", Description = "A Japanese hybrid-electric sportscar with a front-end designed to look like an angry grin might be too whimsical for some, but with a 4-liter V6 engine and 420hp, the Dinka Jester still packs a serious punchline. Race tuned special edition.", },
            new VehicleItem("Dinka Jester Classic", true, ItemType.Vehicles) { ModelName = "jester3", Description = "Dinka are famous for their hyperbikes and other suicide machines, but their legacy only has one name on it: the Jester Classic. They may have set out to make a sports GT, but they accidentally made the most iconic street racer ever to do a quarter mile with plenty of change out of ten seconds flat. It's still as beautiful as it ever was, it'll still eat you alive if you so much as flinch while you're behind the wheel, and you still wouldn't change a damn thing.", },
            new VehicleItem("Dinka Jester RR", true, ItemType.Vehicles) { ModelName = "jester4", Description = "Experts agree that the world is ending. And other experts agree that the only thing you can do about it is get online and fine tune your consumer choices. Introducing the latest Dinka Jester: the car with the killer smile is back, and this time it's so deadpan we're pretty sure it's not even joking. So stop doomscrolling. You've found it.", },
            new VehicleItem("Dinka Blista Kanjo", true, ItemType.Vehicles) { ModelName = "kanjo", Description = "Cheap ramen noodles, Princess Robot Bubblegum, tentacle porn - if you want to express your teenage love for Japanese culture in a car purchase, look no further. Sure, the hideous straight lines and cafeteria tray bumper might cause your date to turn and run, but under that vibe-destroying exterior you'll find a vehicle ready to prove its worth, even if you and your equally deluded mates are the only ones to see it. The Dinka Blista Kanjo - it's a fapper's delight.", },
            new VehicleItem("Dinka RT3000", true, ItemType.Vehicles) { ModelName = "rt3000", Description = "There's more power behind the Dinka RT3000 than the Palmer-Taylor Power Station when they break out the party dust, start the rave, and let the reactor do its thing. It's not a complicated formula, but it works.", },
            new VehicleItem("Dinka Sugoi", true, ItemType.Vehicles) { ModelName = "sugoi", Description = "How do you let people know you're interesting and exciting without engaging in conversation, or even making eye contact? The Dinka Sugoi has as much finish and flash as the elaborate mating ritual of a bird of paradise. Just paint it bright, beam the lights and powerslide your way round town to lure the admiration and friendship that you alone could not.", },
            new VehicleItem("Dinka Thrust", true, ItemType.Vehicles) { ModelName = "thrust" },
            new VehicleItem("Dinka Verus", true, ItemType.Vehicles) { ModelName = "verus" },
            new VehicleItem("Dinka Veto Classic", true, ItemType.Vehicles) { ModelName = "veto", Description = "Take yourself back to the good old days of birthday party GoKarting: the cigarette smog, the broken shins, the breaks for that luminous fruit drink that turned a bunch of kids blind. Wasn't that heaven? Well, now's your opportunity to relive the memories on the highway with no adult supervision.", },
            new VehicleItem("Dinka Veto Modern", true, ItemType.Vehicles) { ModelName = "veto2", Description = "So you think you're worthy of the Veto Modern? You better be. This vehicle is so sophisticated, so sleek, so moderately powerful, that it's road legal. We think. We haven't verified that yetâ€¦", },
            new VehicleItem("Dinka Vindicator", true, ItemType.Vehicles) { ModelName = "vindicator" },
            new VehicleItem("Dundreary Landstalker", ItemType.Vehicles) { ModelName = "landstalker" },
            new VehicleItem("Dundreary Landstalker XL", true, ItemType.Vehicles) { ModelName = "landstalker2", Description = "The original Landstalker implied one of two things. You were a drug dealer, or you were a trophy wife. Now the massive Landstalker XL implies you're both.", },
            new VehicleItem("Dundreary Regina", ItemType.Vehicles) { ModelName = "regina" },
            new VehicleItem("Dundreary Stretch", ItemType.Vehicles) { ModelName = "stretch" },
            new VehicleItem("Dundreary Virgo Classic Custom", true, ItemType.Vehicles) { ModelName = "virgo2", Description = "After hours of crate digging, you know when they've found the perfect sample to butcher on your latest EDM track. This is that record. Remix culture meets auto culture with the Virgo, a car that's already lower and slower than any road-certified vehicle can afford to be - and trust us, you ain't seen nothing yet. Your old man thought this was stately and composed back when he was curb crawling his way through the 70s, so jack up, jerk it off, and show him how the young folks get down.", },
            new VehicleItem("Dundreary Virgo Classic", true, ItemType.Vehicles) { ModelName = "virgo3", Description = "After hours of crate digging, you know when they've found the perfect sample to butcher on your latest EDM track. This is that record. Remix culture meets auto culture with the Virgo, a car that's already lower and slower than any road-certified vehicle can afford to be - and trust us, you ain't seen nothing yet. Your old man thought this was stately and composed back when he was curb crawling his way through the 70s, so jack up, jerk it off, and show him how the young folks get down.", },
            new VehicleItem("Emperor Habanero", ItemType.Vehicles) { ModelName = "habanero", Description = "Do you hate your family and your job? Are you remortgaged up to your nipples? The last time you ate Mexican food did it give you IBS? If so, then our statistics suggest you're overwhelmingly likely to buy a sensible four-door family SUV with a vaguely peppy name. And here it is.", },
            new VehicleItem("Emperor ETR1", true, ItemType.Vehicles) { ModelName = "sheava", Description = "Every once in a while, a car breaks all the rules. It's cutting edge and timeless. It performs exquisitely on the road and effortlessly on the track. It perfectly embodies both your colossal vanity and your desperate insecurity. More than the sum of its parts, the ETR1 is the only car you will ever need to be able to afford again.", },
            new VehicleItem("Emperor Vectre", true, ItemType.Vehicles) { ModelName = "vectre", Description = "Too extra for the classics? More over-the-top than underground? Forget trying to find that scrapyard bucket and make the only first impression that counts in the Emperor Vectre. Like a supermodel with a mean 100m sprint, this is the kind of narcissism that'll ride you till you're raw and leave you with nothing but gratitude.", },
            new VehicleItem("Enus Cognoscenti 55", true, ItemType.Vehicles) { ModelName = "cog55", Description = "The original Cog Cabrio was a landmark in bringing luxury grand tourers to a wider market, which explains why every five-figure broker with a pinstripe suit and a history of sexual assault has one.  Invest in this new four-door model, with more headroom and a plush interior, and you should stand out of the crowd for at least another couple of months. Armored edition available.", },
            new VehicleItem("Enus Cognoscenti 55 (Armored)", true, ItemType.Vehicles) { ModelName = "cog552", Description = "The original Cog Cabrio was a landmark in bringing luxury grand tourers to a wider market, which explains why every five-figure broker with a pinstripe suit and a history of sexual assault has one. Invest in this new four-door model, with more headroom and a plush interior, and you should stand out of the crowd for at least another couple of months. Armored edition available.", },
            new VehicleItem("Enus Cognoscenti Cabrio", ItemType.Vehicles) { ModelName = "cogcabrio", Description = "The Cog Cabrio is a top-end luxury car that combines elegance with performance. A car that says, 'I'm a man with money but also a modicum of taste'. A car that says, 'I'm not afraid to transfer $185,000 over an insecure internet connection to an unknown entity'. A car that says, 'You never accepted me, Dad, but look at me now'.", },
            new VehicleItem("Enus Cognoscenti", true, ItemType.Vehicles) { ModelName = "cognoscenti" },
            new VehicleItem("Enus Cognoscenti (Armored)", true, ItemType.Vehicles) { ModelName = "cognoscenti2", Description = "The original Cog Cabrio was a landmark in bringing luxury grand tourers to a wider market, which explains why every five-figure broker with a pinstripe suit and a history of sexual assault has one. Invest in this new four-door model, with more headroom and a plush interior, and you should stand out of the crowd for at least another couple of months. This model has an extended wheelbase. Armored edition available.", },
            new VehicleItem("Enus Huntley S", true, ItemType.Vehicles) { ModelName = "huntley", Description = "Merging the style and craftsmanship of a classic English luxury motorcar with the feature set of a high-end SUV, the Huntley S might be the only example of British-American fusion that actually works.", },
            new VehicleItem("Enus Paragon R", true, ItemType.Vehicles) { ModelName = "paragon", Description = "This is it. Wrap it up, folks. Thanks to Enus, humanity's quest to design the perfect grand tourer is finally over. It took generations of privately educated stiff upper lips, all prepared to dig deep into bottomless wells of lazy entitlement - but credit where it's due. The Brits cracked the code. This is the kind of self-assurance that can't be earned. It can only be bought.", },
            new VehicleItem("Enus Paragon R (Armored)", true, ItemType.Vehicles) { ModelName = "paragon2", Description = "This weekâ€™s top prize on the podium is an Armored Enus Paragon R wrapped in the Doing Busy Work livery â€” previously only available as an award for completing all 6 Casino Missions as host and a presence thatâ€™s sure to intimidate anyone unfortunate enough to get stuck at the red light with you.", },
            new VehicleItem("Enus Stafford", true, ItemType.Vehicles) { ModelName = "stafford" },
            new VehicleItem("Enus Super Diamond", ItemType.Vehicles) { ModelName = "superd", Description = "Synonymous with style and luxury, the interior of Super Diamond has so much leather and wood, it's like sitting in a library. The historic British car manufacturer was taken over by the Germans in the late 90s, with no hard feelings whatsoever.", },
            new VehicleItem("Enus Windsor", true, ItemType.Vehicles) { ModelName = "windsor" },
            new VehicleItem("Enus Windsor Drop", true, ItemType.Vehicles) { ModelName = "windsor2", Description = "How do you improve on something as flawlessly pompous as the Enus Windsor? The answer is something you need to experience to believe: so hop inside, mash the touchscreen interface, feel the canvas roof glide away above you, and gaze in triumph at all the lowly pedestrians as your appalling toupee flutters away in the 30mph breeze.", },
            new VehicleItem("Fathom FQ 2", ItemType.Vehicles) { ModelName = "fq2", Description = "You might think that calling something 'utterly nondescript' and 'profoundly unattractive' would be a contradiction in terms, but look closely: the FQ 2 is a masterclass in having things both ways.", },
            new VehicleItem("Gallivanter Baller", ItemType.Vehicles) { ModelName = "Baller" },
            new VehicleItem("Gallivanter Baller 2", ItemType.Vehicles) { ModelName = "baller2" },
            new VehicleItem("Gallivanter Baller LE", true, ItemType.Vehicles) { ModelName = "baller3", Description = "Redefine off-roading with Gallivanter's new and improved Baller: sure, it'll fall to pieces at the slight of rugged terrain, but it'll plough through cycle lanes more smoothly than anything else on the market.", },
            new VehicleItem("Gallivanter Baller LE LWB", true, ItemType.Vehicles) { ModelName = "baller4", Description = "Redefine off-roading with Gallivanter's new and improved Baller: sure, it'll fall to pieces at the slight of rugged terrain, but it'll plough through cycle lanes more smoothly than anything else on the market. This model has an extended wheelbase.", },
            new VehicleItem("Gallivanter Baller LE (Armored)", true, ItemType.Vehicles) { ModelName = "baller5", Description = "Redefine off-roading with Gallivanter's new and improved Baller: sure, it'll fall to pieces at the slight of rugged terrain, but it'll plough through cycle lanes more smoothly than anything else on the market.", },
            new VehicleItem("Gallivanter Baller LE LWB (Armored)", true, ItemType.Vehicles) { ModelName = "baller6", Description = "Redefine off-roading with Gallivanter's new and improved Baller: sure, it'll fall to pieces at the slight of rugged terrain, but it'll plough through cycle lanes more smoothly than anything else on the market. This model has an extended wheelbase.", },
            new VehicleItem("Grotti Bestia GTS", true, ItemType.Vehicles) { ModelName = "bestiagts" },
            new VehicleItem("Grotti Brioso R/A", true, ItemType.Vehicles) { ModelName = "brioso", Description = "You favor light, compact versatile car design. You believe a hot hatch can be just as macho as a lumbering supercar. You describe yourself as having a 'big personality'. You know who you are. Just buy the damn car and get it over with.", },
            new VehicleItem("Grotti Brioso 300", true, ItemType.Vehicles) { ModelName = "brioso2", Description = "Grotti's designers always knew they were missing something vital. Sure their compacts ticked all the boxes â€“ knees by your chin, elbows out the window â€“ but day after day they would return to the drawing board certain there was one trick they'd yet to try. And then, one beautiful genius whispered 'smaller'. That's how the 300 was born.", },
            new VehicleItem("Grotti Carbonizzare", ItemType.Vehicles) { ModelName = "carbonizzare" },
            new VehicleItem("Grotti Cheetah", ItemType.Vehicles) { ModelName = "cheetah" },
            new VehicleItem("Grotti Cheetah Classic", true, ItemType.Vehicles) { ModelName = "cheetah2", Description = "There's a kind of charm that only comes with age, and in today's jaded world nothing's aged better than the Cheetah Classic. It's practical, spacious, understated. It oozes red-blooded panache. You open the door, and you catch the smell of brandy and cigars on its breath. It's eminently respectable, it's constantly groping its secretary, and it doesn't even feel the need to pretend it has friends from minority groups. Welcome to the old world.", },
            new VehicleItem("Grotti Furia", true, ItemType.Vehicles) { ModelName = "furia", Description = "Who needs a super car? Who needs to throw their cash at the finest leather upholstery, godly specs and a body so seductive Aphrodite herself would have bowed down to it? Who needs an all-out-tarmac-tearing-sound-barrier-breaking-sexy-racing-dream-machine? You do. The Grotti Furia - worth selling your second kidney for.", },
            new VehicleItem("Grotti GT500", true, ItemType.Vehicles) { ModelName = "gt500", Description = "If you're looking for a car that puts function ahead of form, you're in the wrong boutique. Sure, you can try to drive the GT500 straight from A to B. But on the way, you'll find you're taking in pretty much every other letter of the alphabet, and they're spelling out something obscene in Italian. Your only choice is light a cigarette, strike a pose, contemplate how stunningly attractive this car makes you look, and enjoy the ride.", },
            new VehicleItem("Grotti Itali GTO", true, ItemType.Vehicles) { ModelName = "italigto", Description = "When you think of lightweight redesigns, you probably think of carbon fiber bodywork and stripped-out interiors. But that's just for beginners. Step inside the Itali GTO, and the air you're breathing has increased hydrogen content for extra lift. Grotti have even taken the controversial step of obliging potential owners to do their part by shaving their body hair and removing at least one kidney. Sometimes you have to suffer for perfection.", },
            new VehicleItem("Grotti Itali RSX", true, ItemType.Vehicles) { ModelName = "italirsx", Description = "Warning: NSFW. There's a sexy single car in your area looking for a ride. Interested? Just open a private tab and check out these candid pics of the RSX's sultry bodywork, hourglass waist, soft front, and silken A-line. But before you take it to the next level and see what's under the hood, turn off your webcam and mute your mic. The conference call you're ignoring is about to see your o-face.", },
            new VehicleItem("Grotti X80 Proto", true, ItemType.Vehicles) { ModelName = "prototipo", Description = "The cause of more UFO sightings across San Andreas than any other production vehicle of the decade, the Proto is the kind of concept car you get when your head of R&amp;D is an 8 year old child with a stack of comics and a bowlful of MDMA. The future is here.", },
            new VehicleItem("Grotti Stinger", ItemType.Vehicles) { ModelName = "stinger" },
            new VehicleItem("Grotti Stinger GT", ItemType.Vehicles) { ModelName = "stingergt", Description = "Don't mistake this for a standard Stinger. The GT is a hard-top, race-bred variant that boasts a top speed of 175mph and 0-60 in less than 6 seconds. With only 40 produced, the Stinger GT is one of the most collectible sportscars in the world. So savor those fleeting moments of enjoyment in between being terrified of crashing it or somebody stealing it.", },
            new VehicleItem("Grotti Turismo Classic", true, ItemType.Vehicles) { ModelName = "turismo2", Description = "This is one for the purists. No hi-tech driving aids. No smart safety features. When you're three nanoseconds away from getting a mouthful of the truck in front, no onboard supercomputer is going to save you. But just like learning a language or killing a stranger with your bare hands, this kind of hard work is its own reward.", },
            new VehicleItem("Grotti Turismo R", true, ItemType.Vehicles) { ModelName = "turismor", Description = "Grotti might have alienated their Old Money consumers by releasing a hybrid sports car, but 'fuel efficient' is relative when you're talking about 799hp. 0-60 in under 3 seconds and a top speed of 210 mph.", },
            new VehicleItem("Grotti Visione", true, ItemType.Vehicles) { ModelName = "visione", Description = "Try to relax. There's a first time for everyone. You take another furtive glance at those ravishing curves, and feel faint with desire. Suddenly it doesn't matter how rich you are: this time, you're out of your league. Your mouth is dry. Your pants, sodden. And then it happens: the doors glide open, you take your seat, and nothing is ever the same again. There's no going back. Welcome to Visione.", },
            new VehicleItem("Hijak Khamelion", ItemType.Vehicles) { ModelName = "khamelion", Description = "The Khamelion is an electric hybrid luxury sports sedan. Don't laugh. It isn't a complete oxymoron. This beauty handles so well, you'd never know you're driving a plug in. Welcome to the future (so long as you have access to a specialized charging station).", },
            new VehicleItem("Hijak Ruston", true, ItemType.Vehicles) { ModelName = "ruston", Description = "You may look, sound and smell like a corporate insurance analyst, but you live for the track. Your flabby, pimply ass is only at home nestled into a low-slung, carbon fiber racing seat. You fall asleep to fantasies of unconventional aerodynamics. You whisper 'monocoque' to yourself while you're jacking off in the shower. And there's only one name you scream out as you dump your load: Ruston, Ruston, Ruston.", },
            new VehicleItem("HVY Barracks Semi", ItemType.Vehicles) { ModelName = "BARRACKS2" },
            new VehicleItem("HVY Biff", ItemType.Vehicles) { ModelName = "Biff" },
            new VehicleItem("HVY Dozer", ItemType.Vehicles) { ModelName = "bulldozer" },
            new VehicleItem("HVY Cutter", ItemType.Vehicles) { ModelName = "cutter" },
            new VehicleItem("HVY Dump", ItemType.Vehicles) { ModelName = "dump" },
            new VehicleItem("HVY Forklift", ItemType.Vehicles) { ModelName = "FORKLIFT" },
            new VehicleItem("HVY Insurgent Pick-Up", true, ItemType.Vehicles) { ModelName = "insurgent" },
            new VehicleItem("HVY Insurgent", true, ItemType.Vehicles) { ModelName = "insurgent2" },
            new VehicleItem("HVY Insurgent Pick-Up Custom", true, ItemType.Vehicles) { ModelName = "insurgent3" },
            new VehicleItem("HVY Menacer", true, ItemType.Vehicles) { ModelName = "menacer" },
            new VehicleItem("HVY Mixer", ItemType.Vehicles) { ModelName = "Mixer" },
            new VehicleItem("HVY Mixer 2", ItemType.Vehicles) { ModelName = "Mixer2" },
            new VehicleItem("HVY Nightshark", true, ItemType.Vehicles) { ModelName = "nightshark" },
            new VehicleItem("HVY Apocalypse Scarab", true, ItemType.Vehicles) { ModelName = "scarab" },
            new VehicleItem("HVY Future Shock Scarab", true, ItemType.Vehicles) { ModelName = "scarab2" },
            new VehicleItem("HVY Nightmare Scarab", true, ItemType.Vehicles) { ModelName = "scarab3" },
            new VehicleItem("Imponte Deluxo", true, ItemType.Vehicles) { Description ="The future is here, and it has gull-wing doors.", ModelName = "deluxo" },
            new VehicleItem("Imponte Dukes", ItemType.Vehicles) { ModelName = "dukes" },
            new VehicleItem("Imponte Duke O'Death", ItemType.Vehicles) { ModelName = "dukes2", Description = "Special edition Dukes muscle car for the post-apocalypse wasteland. Just hope there's not a fuel scarcity when the world ends, because this thing gets worse gas mileage than a one-legged zombie. Exclusive content for returning players.", },
            new VehicleItem("Imponte Beater Dukes", true, ItemType.Vehicles) { ModelName = "dukes3", Description = "Ever wanted to build your very own legend, bolt by bolt, from the ground up? Well here's your opportunity. Take this old beater, give it a lick of paint and a humble Triple Intake Bug Catcher, then power-slide your mid-life crisis right across the sidewalk. Immortality almost certainly awaits.", },
            new VehicleItem("Imponte Nightshade", true, ItemType.Vehicles) { ModelName = "nightshade", Description = "Don't be fooled by the elegant lines and composed styling: like every great muscle car before it, the Nightshade puts out more power than it (or you) can possibly handle. Few cars present as big as a risk to oncoming traffic, or look this good on the back of a tow truck.", },
            new VehicleItem("Imponte Phoenix", ItemType.Vehicles) { ModelName = "Phoenix" },
            new VehicleItem("Imponte Ruiner", ItemType.Vehicles) { ModelName = "ruiner", Description = "What happened in the 80's stays in the 80's. Unless what happened is a little slice of quad-exhaust, side-striped, T-Top heaven - in which case break out the big hair and the trickle-down economics, because we're going to town.", },
            new VehicleItem("Imponte Ruiner 2000", true, ItemType.Vehicles) { ModelName = "ruiner2", Description = "It's better looking than you. It can fire machine guns and rocket launchers more accurately than you. It can jump higher than you and it's always got a parachute. Your mother prefers its company to yours. You know what they say: if you can't beat it, own it, and pray people don't notice that all your belongings are cooler than you are.Note, the production model of this vehicle has a reduced missile capacity.", },
            new VehicleItem("Imponte Ruiner", true, ItemType.Vehicles) { ModelName = "ruiner3" },
            new VehicleItem("Invetero Coquette", ItemType.Vehicles) { ModelName = "coquette" },
            new VehicleItem("Invetero Coquette Classic", true, ItemType.Vehicles) { ModelName = "coquette2", Description = "If work is flying a fighter jet, your weekend ride better be pretty special to compete. The radical lines will make you regret even the slightest crash. But don't worry, the big engine and bad handling will ensure you're too dead to really care.", },
            new VehicleItem("Invetero Coquette BlackFin", true, ItemType.Vehicles) { ModelName = "coquette3", Description = "Ah, America in the 1950s. It's easy to spot the moment when Freudian psychosexual theory met car design and what a glorious pairing it was.  This Coquette couldn't be more phallic if it was dragging a pair of hairy beach balls. It even smells like cigar smoke, conservative values, and semen. Get out your check book because this golden age of repression won't resurrect itself.", },
            new VehicleItem("Invetero Coquette D10", true, ItemType.Vehicles) { ModelName = "coquette4" },
            new VehicleItem("JoBuilt Hauler", ItemType.Vehicles) { ModelName = "Hauler" },
            new VehicleItem("JoBuilt Hauler Custom", true, ItemType.Vehicles) { ModelName = "Hauler2" },
            new VehicleItem("JoBuilt Phantom", ItemType.Vehicles) { ModelName = "Phantom" },
            new VehicleItem("JoBuilt Phantom Wedge", true, ItemType.Vehicles) { ModelName = "phantom2" },
            new VehicleItem("JoBuilt Phantom Custom", true, ItemType.Vehicles) { ModelName = "phantom3" },
            new VehicleItem("JoBuilt Rubble", ItemType.Vehicles) { ModelName = "Rubble" },
            new VehicleItem("Karin Asterope", ItemType.Vehicles) { ModelName = "asterope", Description = "Promoted by Karin as an 'attainable luxury vehicle', the Asterope is the perfect car for the middle manager who knows deep down he'll never be a senior executive but can't quite bring himself to admit it yet.", },
            new VehicleItem("Karin BeeJay XL", ItemType.Vehicles) { ModelName = "bjxl", Description = "The modern SUV is an incredible piece of engineering: stylish, rugged, practical, safe. But if you're looking for something that managed to dodge every single one of those qualities, the BeeJay XL is the only choice on the market.", },
            new VehicleItem("Karin Calico GTF", true, ItemType.Vehicles) { ModelName = "calico" },
            new VehicleItem("Karin Dilettante", ItemType.Vehicles) { ModelName = "dilettante" },
            new VehicleItem("Karin Dilettante 2", ItemType.Vehicles) { ModelName = "dilettante2" },
            new VehicleItem("Karin Everon", true, ItemType.Vehicles) { ModelName = "everon" },
            new VehicleItem("Karin Futo", ItemType.Vehicles) { ModelName = "futo", Description = "The Futo is Karin's gift to a core demographic that needs nothing more than a lightweight chassis, rear wheel drive and dangerously poor traction to have some good wholesome fun.", },
            new VehicleItem("Karin Futo GTX", true, ItemType.Vehicles) { ModelName = "futo2", Description = "In the way that some forty-year-old frat boys only respond to paddles and hardcore splooshing, the Futo GTX needs a firm hand to reach its optimal performance. In fact, it needs less of a driver and more of a disciplinarian. Enter at your at your own risk, and remember: this thing doesn't have a safe word.", },
            new VehicleItem("Karin Intruder", ItemType.Vehicles) { ModelName = "intruder" },
            new VehicleItem("Karin Kuruma", true, ItemType.Vehicles) { ModelName = "kuruma" },
            new VehicleItem("Karin Kuruma (armored)", true, ItemType.Vehicles) { ModelName = "kuruma2", Description = "The perfect car to go with your flesh tunnel earrings, frosted spikes, and oversize jeans. Buy this and you'll never fail to be mistaken for a small town drug dealer again. This edition helps prevent a mistake turning into a tragedy with armor plating.", },
            new VehicleItem("Karin Previon", true, ItemType.Vehicles) { ModelName = "previon", Description = "The Karin Previon goes like a bullet. Well, like a reloaded cartridge that you forgot in the back of the garage for twenty years which might blow your hand off, but hey! How much you overload it in the workshop is up to you. Just remember, eating the steering wheel and going up in smoke is half the fun.", },
            new VehicleItem("Karin Rusty Rebel", ItemType.Vehicles) { ModelName = "Rebel", OverrideLoadBodiesInBed = true, BedLoadOffsetOverride = new Vector3(0f,-1.5f,1.5f) },
            new VehicleItem("Karin Rebel", ItemType.Vehicles) { ModelName = "rebel2", OverrideLoadBodiesInBed = true, BedLoadOffsetOverride = new Vector3(0f,-1.5f,1.5f) },
            new VehicleItem("Karin Sultan", ItemType.Vehicles) { ModelName = "sultan", Description = "For a wide-eyed junior exec in the late 90s nothing said 'I can almost afford to buy European' like the Karin Sultan. These days you're an angry middle aged, pre-diabetic, wannabe rally driver, but the Sultan is still your best friend: with your money and our expertise, this could become the high-octane racer you dreamed of as a teenager and still have no idea how to drive. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Karin Sultan Classic", true, ItemType.Vehicles) { ModelName = "sultan2", Description = "What's that you're saying - the Karin Sultan was already a classic? That might be true, but the Sultan Classic is both an evolution and a throwback. Something like the true essence of a high-octane four-door racer. It's a time machine to the late 90s when haircuts were bad, movies were good, gang warfare was everywhere and the sitcoms you stream were on TV. Get in, take a ride, don't look back.", },
            new VehicleItem("Karin Sultan RS Classic", true, ItemType.Vehicles) { ModelName = "sultan3", Description = "You might think the Sultan RS Classic is the perfect car for that Auntie your parents stopped inviting to Christmas. And it turns out that's true, because your parents suck, and your aunt sneaks out at the weekend to co-run legendary rally championships and stack-out her Hornbills loyalty card. Time to take after the wrong side of the family...", },
            new VehicleItem("Karin Sultan RS", true, ItemType.Vehicles) { ModelName = "sultanrs" },
            new VehicleItem("Karin Technical", true, ItemType.Vehicles) { ModelName = "technical" },
            new VehicleItem("Karin Technical Custom", true, ItemType.Vehicles) { ModelName = "technical3" },
            new VehicleItem("Karin 190z", true, ItemType.Vehicles) { ModelName = "z190", Description = "The Karin 190z changed the world. If European marques harbored any residual sense of innate superiority, this is the car that crushed it utterly and forever. More elegant than the most refined Grotti, classier then the suavest Ocelot, as perfectly engineered as any Pfister, the 190z was a cup of piping hot sake in the face of fifty years of complacency.", },
            new VehicleItem("Lampadati Casco", true, ItemType.Vehicles) { ModelName = "casco", Description = "Good looking and liable to explode at any moment - the only way this could be more of an Italian stereotype would be if it had mommy issues. The Casco is a 50s classic for polymaths and pederasts.", },
            new VehicleItem("Lampadati Felon", ItemType.Vehicles) { ModelName = "felon", Description = "An Italian luxury car company founded in Bologna at the beginning of the 20th Century that focused on racing cars. Having failed to win many Grand Prix, they focused on making sports cars that impress vapid women. They've done this exceptionally well.", },
            new VehicleItem("Lampadati Felon GT", ItemType.Vehicles) { ModelName = "felon2", Description = "The GT is a 2-door convertible variant of the standard Felon because nothing puts the damper on a midlife crisis quicker than a pair of screaming kids in the back seat.", },
            new VehicleItem("Lampadati Furore GT", true, ItemType.Vehicles) { ModelName = "furoregt" },
            new VehicleItem("Lampadati Michelli GT", true, ItemType.Vehicles) { ModelName = "michelli", Description = "There's a lot of things the Lampadati Michelli does not have. Power steering, vacuum servos, air conditioning - all left on the shop floor. What it does have is a startling number of very angry horses under its hood, and given that it weighs about as much as a golf ball you can be sure of two things: first, it's really, really fast, and second, you're never going to look back.", },
            new VehicleItem("Lampadati Pigalle", true, ItemType.Vehicles) { ModelName = "pigalle", Description = "French design and Italian construction, so expect style over substance and regular breakdowns. The Pigalle, a high performance coupÃ©, is an icon of 70s motoring and bad taste. No wonder the ironically disposed love it.", },
            new VehicleItem("Lampadati Tropos Rallye", true, ItemType.Vehicles) { ModelName = "tropos", Description = "Stunningly beautiful and astonishingly violent, the Lampadati Tropos may have a European name, but it's got an American heart. This is the car that defined a whole generation of rally design, so if your favorite pastime is calling other people's choices derivative then this is by far and away your best bet.", },
            new VehicleItem("Lampadati Komoda", true, ItemType.Vehicles) { ModelName = "komoda", Description = "It's a German-style sedan with none of the reliability. An Italian car with none of the flair. Luxurious and mundane. Exciting and banal. Sporty and lame. Never make another decision again. The Lampadati Komoda is a high-performance luxury vehicle for the executive who wants to have it all and nothing at the same time.", },
            new VehicleItem("Lampadati Novak", true, ItemType.Vehicles) { ModelName = "novak", Description = "No, you didn't misread anything. This is a Lampadati. And an SUV. At the same time. Why? Who can say. But if someone tells you they crossbred a thoroughbred race horse with a hippo, do you question their motives? No, you buy it, take a selfie with it, throw it in storage, and start bragging to your friends.", },
            new VehicleItem("Lampadati Tigon", true, ItemType.Vehicles) { ModelName = "tigon", Description = "Look at the Lampadati Tigon, count to three, and try not to adjust your pants. Ready? One. Two. Yep we called it. This car's sole purpose is to please the eye, thrill the heart and leave you sticky with delight.", },
            new VehicleItem("Lampadati Viseris", true, ItemType.Vehicles) { ModelName = "viseris", Description = "There were a lot of things to like about Lampadati's Viseris: the smooth handling, the chiseled good looks, the salt-of-the-earth engineering. More than anything else, you had to love the unmistakable roar of its barbaric V8. But if you thought that sweet, sweet song couldn't be improved, you were wrong: offset by the rattle of twin forward-facing machine guns, it takes on a whole new depth of character.Please note: Weapon modifications can only be applied at a Vehicle Workshop inside an Avenger or Mobile Operations Center.", },
            new VehicleItem("LCC Avarus", true, ItemType.Vehicles) { ModelName = "avarus", Description = "Something deep within the American psyche cries out to do 100mph in a glorified lounge chair, and dammit if LCC aren't going to answer the call when they hear it. Don't get too comfy though, cos the engine sounds like it's gargling rocks and your left ankle is never more than a few inches from an open flybelt.", },
            new VehicleItem("LCC Hexer", ItemType.Vehicles) { ModelName = "hexer" },
            new VehicleItem("LCC Innovation", true, ItemType.Vehicles) { ModelName = "innovation" },
            new VehicleItem("LCC Sanctus", true, ItemType.Vehicles) { ModelName = "sanctus" },
            new VehicleItem("Maibatsu Manchez", true, ItemType.Vehicles) { ModelName = "manchez", Description = "The act of launching yourself off a fat dirt jump, achieving a torrential climax using nothing more than your powerfully throbbing saddle, and then landing upside down in a heap of shattered bones, engine parts and bodily fluids. Part fetish, part deathwish, part bloodsport.", },
            new VehicleItem("Maibatsu Manchez Scout", true, ItemType.Vehicles) { ModelName = "manchez2", Description = "Winner of this year's San Andreas Free Enterprise Awards, the Manchez Scout is the kind of innovation that's only possible when you convince the military industrial complex that the phrase 'weapons-grade dirt bike' is a defense contract waiting to happen.", },
            new VehicleItem("Maibatsu Mule", ItemType.Vehicles) { ModelName = "Mule" },
            new VehicleItem("Maibatsu Mule", ItemType.Vehicles) { ModelName = "Mule2" },
            new VehicleItem("Maibatsu Mule", true, ItemType.Vehicles) { ModelName = "Mule3" },
            new VehicleItem("Maibatsu Mule Custom", true, ItemType.Vehicles) { ModelName = "mule4" },
            new VehicleItem("Maibatsu Penumbra", ItemType.Vehicles) { ModelName = "penumbra", Description = "High performance compact car for the kind of person who drifts around corners with a baby seat in the back. Recently discontinued, so may be worth waiting a decade or so for it to become ironic cool.", },
            new VehicleItem("Maibatsu Penumbra FF", true, ItemType.Vehicles) { ModelName = "penumbra2", Description = "Here at Maibatsu, we get it. In these trying times, it can be really hard to commit to a color scheme for your classic, mid-noughties racecar. So we're working in partnership with LS Customs to provide a livery for anything life can throw at you. Bad hair day? There's a livery for that. Stuck upside down in your rhinestone gravity boots? There's a livery for that. Dad screaming you were a mistake long into the night again? You guessed it. The Penumbra FF. Adding yet more color to an already colorful existence.", },
            new VehicleItem("Maibatsu Sanchez Custom", ItemType.Vehicles) { ModelName = "Sanchez" },
            new VehicleItem("Maibatsu Sanchez", ItemType.Vehicles) { ModelName = "sanchez2" },
            new VehicleItem("Mammoth Patriot", ItemType.Vehicles) { ModelName = "patriot" },
            new VehicleItem("Mammoth Patriot Stretch", true, ItemType.Vehicles) { ModelName = "patriot2", Description = "Are you an excitable minor in rented formalwear whose life ambition is to lean out of the windows and pretend to be drunk on your way to prom? Or maybe you're a narcoterrorist with a large retinue and a really important client meeting? How about a Z-List celebrity trapped in a death spiral of conspicuous consumption? Whatever your needs may be, the Patriot Stretch has room to spare.", },
            new VehicleItem("Mammoth Squaddie", true, ItemType.Vehicles) { ModelName = "squaddie", Description = "Long gone are the days of playing army men with other kids. Now you play army men with grownups. And what's more grownup than giving each other a high and tight before piling into an armored truck, stripping to the waist and making revving noises? It's just like the old days, but your mom's not there to make you snacks.", },
            new VehicleItem("Maxwell Asbo", true, ItemType.Vehicles) { ModelName = "asbo", Description = "You're only young once, so why not spend every spare cent you have turning a tiny, shitty car into a monument to your lack of taste and sophistication. It's rude, it's aggressive, it's annoying, and it doesn't give a shit. The only things that matter to you are speakers, spoilers, and speed - and you're proud of that conviction. Go get an Asbo today.", },
            new VehicleItem("Maxwell Vagrant", true, ItemType.Vehicles) { ModelName = "vagrant" },
            new VehicleItem("MTL Brickade", true, ItemType.Vehicles) { ModelName = "brickade" },
            new VehicleItem("MTL Apocalypse Cerberus", true, ItemType.Vehicles) { ModelName = "cerberus" },
            new VehicleItem("MTL Future Shock Cerberus", true, ItemType.Vehicles) { ModelName = "cerberus2" },
            new VehicleItem("MTL Nightmare Cerberus", true, ItemType.Vehicles) { ModelName = "cerberus3" },
            new VehicleItem("MTL Fire Truck", ItemType.Vehicles) { ModelName = "firetruk" },
            new VehicleItem("MTL Flatbed", ItemType.Vehicles) { ModelName = "FLATBED" },
            new VehicleItem("MTL Packer", ItemType.Vehicles) { ModelName = "Packer" },
            new VehicleItem("MTL Pounder", ItemType.Vehicles) { ModelName = "Pounder" },
            new VehicleItem("MTL Pounder Custom", true, ItemType.Vehicles) { ModelName = "pounder2" },
            new VehicleItem("MTL Dune", true, ItemType.Vehicles) { ModelName = "rallytruck" },
            new VehicleItem("MTL Wastelander", true, ItemType.Vehicles) { ModelName = "wastelander" },
            new VehicleItem("Nagasaki BF400", true, ItemType.Vehicles) { ModelName = "bf400", Description = "When the history books are written, the BF400 will be seen as our age's greatest expression of the pioneer spirit. No other advance in off-road engineering has broughty us this close to our forefathers' dreams of a land where no area of pristine wilderness is safe from noise, smoke, gas and discarded bottles of Pisswasser. Welcome to the brave new world.", },
            new VehicleItem("Nagasaki Blazer", ItemType.Vehicles) { ModelName = "blazer" },
            new VehicleItem("Nagasaki Blazer Lifeguard", ItemType.Vehicles) { ModelName = "blazer2" },
            new VehicleItem("Nagasaki Hot Rod Blazer", ItemType.Vehicles) { ModelName = "blazer3" },
            new VehicleItem("Nagasaki Street Blazer", true, ItemType.Vehicles) { ModelName = "blazer4" },
            new VehicleItem("Nagasaki Carbon RS", ItemType.Vehicles) { ModelName = "carbonrs", Description = "This superbike from Nagasaki is extra lightweight because of its carbon body, resulting in a very fine line between 'joy to drive' and 'infernal deathtrap'. It's a line worth treading.", },
            new VehicleItem("Nagasaki Chimera", true, ItemType.Vehicles) { ModelName = "chimera", Description = "Sure, the last time you rode a trike you were wearing nothing but a freshly filled diaper, but what's the point of being a man-child if you don't get to regress as far as you want? Besides, those reinforced, deep-tread roadkillers at the back definitely don't look like giant training wheels. Go get 'em tiger.", },
            new VehicleItem("Nagasaki Outlaw", true, ItemType.Vehicles) { ModelName = "outlaw" },
            new VehicleItem("Nagasaki Shotaro", true, ItemType.Vehicles) { ModelName = "shotaro" },
            new VehicleItem("Nagasaki Stryder", true, ItemType.Vehicles) { ModelName = "Stryder" },
            new VehicleItem("Obey 8F Drafter", true, ItemType.Vehicles) { ModelName = "drafter" },
            new VehicleItem("Obey 9F", ItemType.Vehicles) { ModelName = "ninef", OverrideCannotLoadBodiesInRear = true, },
            new VehicleItem("Obey 9F Cabrio", ItemType.Vehicles) { ModelName = "ninef2", OverrideCannotLoadBodiesInRear = true, },
            new VehicleItem("Obey Omnis", true, ItemType.Vehicles) { ModelName = "omnis", Description = "The Obey Omnis was the poster child of the golden age of rallying, a period of deregulated innocence when a turbo-charged tin can could plough through a crowd of spectators on a muddy embankment and there wasn't a damn thing the government could do about it. This is one for the fans.", },
            new VehicleItem("Obey Rocoto", ItemType.Vehicles) { ModelName = "rocoto", Description = "The Rocoto won't win any races, but it makes up for it with luxuries that soothe the soul of upper middle class middle-aged angst.", },
            new VehicleItem("Obey Tailgater", ItemType.Vehicles) { ModelName = "tailgater", Description = "Luxury German Sedan. Better than a BF, but not quite a Benefactor. Fast and practical with a classy look, this is just the car to over-leverage on.", },
            new VehicleItem("Obey Tailgater S", true, ItemType.Vehicles) { ModelName = "tailgater2", Description = "After a day of synergizing teleconferences in your underwear, nothing provides an escape like the Tailgater S. Go to the right auto shop and you'll have a car that looks like a street racer on the outside, while on the inside it gives you the kind of air-conditioned comfort you need to pull over and give 110% in your team's 9pm Idea Shower.", },
            new VehicleItem("Ocelot Ardent", true, ItemType.Vehicles) { ModelName = "ardent", Description = "It's a rare car that allows you to be perfectly composed, effortlessly suave and extraordinarily violent all at the same time - and yet, somehow, the Ardent does all that and more. Behind the wheel of this masterpiece there's nothing you can't do: lose your pursuers, pop the dual machine guns, gun down the survivors, drop a pithy remark, open the champagne, have a quickie, drive into the sea, realize you've made a terrible mistake, and swiftly drown.", },
            new VehicleItem("Ocelot F620", ItemType.Vehicles) { ModelName = "f620", Description = "If this car could talk, it would say 'I'm having a midlife crisis'. Just cheaper than the divorce that'll result from having an affair with your personal assistant, but the two are by no means mutually exclusive.", },
            new VehicleItem("Ocelot R88", true, ItemType.Vehicles) { ModelName = "formula2" },
            new VehicleItem("Ocelot Jackal", ItemType.Vehicles) { ModelName = "jackal", Description = "Modern and forward-thinking on the outside, trapped in the 19th century on the inside, the Jackal is as British as they come. More leather and wood paneling than any other luxury car in its class. But don't mention 'class'...", },
            new VehicleItem("Ocelot Jugular", true, ItemType.Vehicles) { ModelName = "jugular", Description = "You might be wondering, does the world really need another high-caliber sports saloon? But remember, there are some things the human race can never get enough of - like sex, or violence. And this isn't just any sex or violence: the Jugular is really kinky sex, and really gratuitous violence. Has that answered your question?", },
            new VehicleItem("Ocelot Locust", true, ItemType.Vehicles) { ModelName = "locust", Description = "The open-top, two-seater Locust pedigree goes all the way back to '69, and this is what you get after forty years of track testing: no roof, no windscreen, no windows, no compromises and no interest in your personal safety. If you ever wondered what it's like to drive around in a logical conclusion, this is your chance to find out.", },
            new VehicleItem("Ocelot Lynx", true, ItemType.Vehicles) { ModelName = "lynx", Description = "On the one hand, it's a pinnacle of British car design: conservative, luxuriant, anally retentive. On the other though, it's brash, liveried, track-ready. Impossible? Put your hands together, and meet the Ocelot Lynx: like getting mugged by someone in a top hat, it's an experience you'll never forget.", },
            new VehicleItem("Ocelot Pariah", true, ItemType.Vehicles) { ModelName = "pariah", Description = "This is not an accessible sports car. It won't rub its avant-garde bodywork in your face and let you grope its dashboard on the first drive. It's dignified, sophisticated - even a little aloof. It will only reveal its charms for just the right handler. But one day, after years of practice, you'll become aware of the utter contempt in which you now hold the rest of the human race, and you'll know you can finally say 'I drive a Pariah.'", },
            new VehicleItem("Ocelot Penetrator", true, ItemType.Vehicles) { ModelName = "penetrator" },
            new VehicleItem("Ocelot Swinger", true, ItemType.Vehicles) { ModelName = "swinger", Description = "The Ocelot Swinger was supposed to be a myth. A few blueprints got passed around by collectors, but it just didn't seem possible: the aerodynamics were decades ahead of their time, the engineering too complex for a classic car, the bodywork so alluring that just feathering the clutch would feel like cheating on your spouse. But now, thanks to the power of assembly robotics and easily monetized nostalgia, the legend has finally hit the streets.", },
            new VehicleItem("Ocelot XA-21", true, ItemType.Vehicles) { ModelName = "xa21", Description = "To those who argue that the supercar is dead and hybrid tech was only a fad, the XA-21 would like a word as soon as it's done banging your mom. And when you're done saying thank you, it'll show you the kind of annihilating performance that can only be achieved by locking a team of world-class engineers in a lab for six months and lacing their food with amphetamines. This is the cutting edge. And you're welcome.", },
            new VehicleItem("Overflod Autarch", true, ItemType.Vehicles) { ModelName = "autarch", Description = "This is not a hypercar. It's not a sports prototype or a concept GT. It's something else. Something much, much better. And this isn't even an advert for whatever it is. The Autarch doesn't need an advert. It doesn't need anything it doesn't have already, least of all the approval of an irrelevance like you. No, you need it: more than you need money, dignity or life itself. Go on, we dare you not to buy it.", },
            new VehicleItem("Overflod Entity XXR", true, ItemType.Vehicles) { ModelName = "entity2", Description = "Sure, it makes sense for a nation of ultra liberal herring-lovers to lead the world in the manufacture of affordable flat-pack furniture. But just when you think you've got the measure of them, the Swedish go and produce a low-slung, heavyweight, pitilessly fast hypercar, and suddenly it's hard to sustain your prejudices when you're experiencing enough G-force to separate your face from your skull. Go figure.", },
            new VehicleItem("Overflod Entity XF", ItemType.Vehicles) { ModelName = "entityxf", Description = "High taxes, socialism, constant darknessâ€¦ Sweden really is proof that, if you fill a country full of hot women, people will put up with a wretched landscape. By pussying out of armed conflicts for the past 200 years and focusing instead on investment in education, healthcare and manufacturing, the Swedes now enjoy one of the highest standards of living in the world. The result is a nation that's terrible at democracy but excellent at making ridiculously fast sportscars.", },
            new VehicleItem("Overflod Imorgon", true, ItemType.Vehicles) { ModelName = "imorgon", Description = "You've dreamt about the ultimate super electric sports car. Now here you are, reading an app, seeing it in the carbon fiber flesh and wondering if it can really be true. It can. The Overflod Imorgon - your electric dreams turned reality. And not the bad, naked and losing your teeth dreams either. We're talking the flying and having parents who are proud of you dreams. You're welcome.", },
            new VehicleItem("Overflod Tyrant", true, ItemType.Vehicles) { ModelName = "tyrant", Description = "The Tyrant is a testament to human ingenuity. We're not sure who's more impressive: the team of engineers who've taken us as close as human beings can get to installing a gear stick and a steering wheel in the front of a hurricane, or the team of lawyers who got it classified as road legal. Take your pick.", },
            new VehicleItem("Pegassi Bati 801", ItemType.Vehicles) { ModelName = "bati" },
            new VehicleItem("Pegassi Bati 801RR", ItemType.Vehicles) { ModelName = "bati2" },
            new VehicleItem("Pegassi Esskey", true, ItemType.Vehicles) { ModelName = "esskey", Description = "This isn't some jumped up vintage throwback. This is what would have happened if the classic designers of the 1960s had stayed in production, hemorrhaging money and creativity with every passing decade, until they were reduced to churning out over-marketed nostalgia trips to trust fund hipsters in their second year of college. An instant classic, then.", },
            new VehicleItem("Pegassi Faggio Sport", true, ItemType.Vehicles) { ModelName = "faggio" },
            new VehicleItem("Pegassi Faggio", ItemType.Vehicles) { ModelName = "faggio2", Description = "A certain kind of man drives a scooter. Is that you?", },
            new VehicleItem("Pegassi Faggio Mod", true, ItemType.Vehicles) { ModelName = "faggio3", Description = "A certain kind of man looks at a scooter and thinks 'it's fine, but it doesn't reflect how unique I am - it needs some interesting mod options'. If that's you (it is) then don't fret, your minute-long search has finally come to an end.", },
            new VehicleItem("Pegassi FCR 1000", true, ItemType.Vehicles) { ModelName = "fcr", Description = "Treading the fine line between old-school, no-frills engineering and over-priced hipster-bait, Pegassi's FCR is every bike to every man. And you know what they say: if it ain't broke, see how much you can mod it. Benny's unique upgrade harnesses all that poise and efficiency beneath a mid-century, stripped-back military aesthetic that'd almost make your grandpa wish he hadn't disowned you.", },
            new VehicleItem("Pegassi FCR 1000 Custom", true, ItemType.Vehicles) { ModelName = "fcr2" },
            new VehicleItem("Pegassi Infernus", ItemType.Vehicles) { ModelName = "infernus" },
            new VehicleItem("Pegassi Infernus Classic", true, ItemType.Vehicles) { ModelName = "infernus2", Description = "Experience tells you that anything this hot must be crazy, and you're not wrong. The Infernus Classic is the kind of car that'll dazzle you with its perfect cheekbones, empty your bank account, and once you're sleeping in the wet patch it'll finish you off with a rusty machete. What's not to love?", },
            new VehicleItem("Pegassi Monroe", ItemType.Vehicles) { ModelName = "monroe", Description = "Remember Italy's glory days before feminism and the Euro ruined everything? When a suitcase full of Lire would buy you an espresso and a pack of cigarettes, if you were lucky? Produced by old-money Italians in the 1960s, driven by new-money guidos in the 2010s, the Monroe is a classic supercar that has been making douchebags look stylish for over 50 years.", },
            new VehicleItem("Pegassi Oppressor", true, ItemType.Vehicles) { ModelName = "oppressor", Description = "There are two kinds of people in the point one percent. There's the balding stock analyst with pituitary issues, staring out the window of his comfortable private jet on the approach into LSIA. And there's the guy mooning him as he screams past on a rocket-powered hyperbike with extendable wings and a front-mounted machine gun. The only question is, which side of the glass do you want to be on?", },
            new VehicleItem("Pegassi Oppressor Mk II", true, ItemType.Vehicles) { ModelName = "oppressor2", Description = "The Oppressor Mk I was a landmark in hybrid vehicle design. Well, the Mk II takes off where its little brother landed - and it never comes down. This is about the closest you can get to throwing a saddle on a rocket engine, bolting on some optional heavy artillery, and pressing the big red button.Please note: This vehicle can only be modified at the Specialized Workshop inside a Terrorbyte.", },
            new VehicleItem("Pegassi Osiris", true, ItemType.Vehicles) { ModelName = "osiris", Description = "Osiris drivers boast the shortest average life expectancy of any consumer demographic of America. They live, briefly, in a world of bygone opulence and hyper-modern engineering. The 0.3 seconds between leaving the showroom and arriving at their first corner are the most exhilarating blur in their short, short lives. Only the stupidly rich need sign up to the waiting list.", },
            new VehicleItem("Pegassi Reaper", true, ItemType.Vehicles) { ModelName = "reaper", Description = "Statistically, use of the accelerator in a Pegassi Reaper is more likely to cause a fatal brain hemorrhage than any other activity known to medical science. Fighter pilots have to undergo years of training before experiencing this kind of G-force, but luckily for you the only qualifications required to get behind the wheel are an above-average credit rating and a hearty contempt for the poor. Diamond-finish cup holders and a live-in butler come as standard.", },
            new VehicleItem("Pegassi Ruffian", ItemType.Vehicles) { ModelName = "ruffian" },
            new VehicleItem("Pegassi Tempesta", true, ItemType.Vehicles) { ModelName = "tempesta", Description = "At some point, asking 'So how fast it is?' is like asking the guy who just put his fist through your ribs 'So how strong are you ?' It's not about the speed anymore. It's not about the style, either, because one touch of the gas and it's little more than a blur. You just know that deep down there's an itch only this car can scratch, and you lack any of the personal qualities you'll need to resist.", },
            new VehicleItem("Pegassi Tezeract", true, ItemType.Vehicles) { ModelName = "tezeract", Description = "Ladies and gentlemen, we have crossed the frontier. The motorcar has evolved. The first member of a new and alien species has arrived, and it does not come in peace. The Tezeract's only purpose is to wage a silent war of annihilation on anything else that dares to call itself a means of transport. As of now, there's a right side of history. Choose wisely.", },
            new VehicleItem("Pegassi Torero", true, ItemType.Vehicles) { ModelName = "torero", Description = "To own a Pegassi Torero is to own a piece of history. This car marked the end of an era: a bygone age when porn stars had luxuriant pubic hair and supercars didn't take risks. Then along came the Torero - a wedge-shaped, scissor-doored stallion fresh from a back, sack and crack - and nothing was ever the same again. Decades later, it still looks like it's rolled straight out of a wet dream wearing nothing but a glint in its eye. This is one for the collectors.", },
            new VehicleItem("Pegassi Toros", true, ItemType.Vehicles) { ModelName = "toros", Description = "What do a 23rd Century hypercar and a family-friendly SUV have in common? More than you might think. They both turn you into a leadfooted, tailgating sociopath the moment you touch the gas. And they both do roughly the same miles per gallon as a burning oil well. With all that shared DNA, it was only a matter of time before someone left them in a dark showroom to see if they would breed â€“ and the Toros is the result.", },
            new VehicleItem("Pegassi Vacca", ItemType.Vehicles) { ModelName = "vacca", Description = "It's hard to measure success, but when you're on a car website and several thousand dollars is the 'affordable option', we think it's fair to say you're doing ok for yourself. Perfect for the middle-aged man trying to get back in the dating game after a divorce. No room for kids. Just enough room for a 90-pound blonde in her early 20's who, thanks to growing up in the Internet age, thinks anal on the first date makes sense.", },
            new VehicleItem("Pegassi Vortex", true, ItemType.Vehicles) { ModelName = "vortex" },
            new VehicleItem("Pegassi Zentorno", true, ItemType.Vehicles) { ModelName = "zentorno", Description = "Make sure the other 99% know you're in a vehicle they can't afford with this loud, brash, in-your-face supercar from Pegassi. Insanely fast with a high-tech interior, this is as close as you can get to a fighter jet on wheels. The only thing that goes up quicker than the 0-60 on this bad boy is your insurance premium.", },
            new VehicleItem("Pegassi Zorrusso", true, ItemType.Vehicles) {ModelName = "zorrusso",Description = "It takes a special kind of visionary to sit behind the wheel of a hypercar while it flirts outrageously with the sound barrier and seriously ask the question 'Hey, wouldn't it be cool if we could put the top down?' But then the folks at Pegassi are nothing if not visionary, and nothing if not special.", },
            new VehicleItem("Pfister Comet", "You always wanted one of these when in high school - and now you can have the car that tells everyone yes, these are implants - on your head and in that dizzy tart next to you. Boom. You go, tiger.", ItemType.Vehicles) { ModelName = "comet2" },
            new VehicleItem("Pfister Comet Retro Custom", "For a whole generation of the San Andreas elite, this isn't just a car. From the onboard champagne cooler to the suede back seat where you pawed your first gold digger - The Pfister Comet was something that made you who you are. And now, thanks to Benny reinventing it as a gnarly, riveted urban dragster, it'll be broadcasting your escalating midlife crisis for years to come.", true, ItemType.Vehicles) { ModelName = "comet3" },
            new VehicleItem("Pfister Comet Safari", "Is there nothing the Pfister Comet cannot do? If you were a venture capitalist looking for the shortest route to your next midlife crisis, the Comet was your first and only choice. If you wanted something that preserved the classic reek of desperation but added a street-racer twist, the Retro Custom was top of the list. And now, if you're looking for something to slam around a hairpin bend in three feet of uphill mud, the Comet Safari has got you covered.", true, ItemType.Vehicles) { ModelName = "comet4" },
            new VehicleItem("Pfister Comet SR", "Forget everything you think you know about the Pfister Comet. Forget cruising through Vinewood with a bellyful of whiskey dropping one-liners about the size of your bonus. Forget picking up sex workers and passing them off as your fiancé at family gatherings. The SR was made for only one thing: to make every other sports car look like it's the asthmatic kid in gym. Now get in line.", true, ItemType.Vehicles) { ModelName = "comet5" },
            new VehicleItem("Pfister Comet S2", "This isn't just a fast car. It's a car with the kind of reputation that no amount of targeted advertising can buy. So, when some people see a Comet they make a wish. Others run screaming for cover, prophesying doom, destruction, and crippling medical expenses. Either way, you made an impression.", true, ItemType.Vehicles) { ModelName = "comet6" },
            new VehicleItem("Pfister Growler", "You prefer the book to the movie. You drink spirits neat. You describe your sense of humor as 'subtle' and your love making as 'imperceptible'. You're The Thinking Person. And you choose handling over speed, control over power, and principle over pleasure. You choose wisely. You choose the Pfister Growler.", true, ItemType.Vehicles) { ModelName = "growler", Description = "You prefer the book to the movie. You drink spirits neat. You describe your sense of humor as 'subtle' and your love making as 'imperceptible'. You're The Thinking Person. And you choose handling over speed, control over power, and principle over pleasure. You choose wisely. You choose the Pfister Growler.", },
            new VehicleItem("Pfister Neon", "When the history of the electric car is written, it will begin with the Pfister Neon. Everything else - all the ridiculous eco-vans and hybrid fetishes - has been foreplay. Now Pfister have dropped their pants, and the battery-powered action can really begin.", true, ItemType.Vehicles) { ModelName = "neon", Description = "When the history of the electric car is written, it will begin with the Pfister Neon. Everything else - all the ridiculous eco-vans and hybrid fetishes - has been foreplay. Now Pfister have dropped their pants, and the battery-powered action can really begin.", },
            new VehicleItem("Pfister 811", "Meet the future of hybrid tech: Pfister took billions of dollars in subsidies for low-carbon research and used it to refine an electric motor until it gives more kick than a turbo charger. And don't worry about accidentally investing in the environment: the assembly process alone produces enough CO2 to offset two thousand acres of otherwise useless rainforest. Win-win.", true, ItemType.Vehicles) { ModelName = "pfister811", Description = "Meet the future of hybrid tech: Pfister took billions of dollars in subsidies for low-carbon research and used it to refine an electric motor until it gives more kick than a turbo charger. And don't worry about accidentally investing in the environment: the assembly process alone produces enough CO2 to offset two thousand acres of otherwise useless rainforest. Win-win.", },
            new VehicleItem("Principe Deveste Eight", "It began as little more than a myth: a list of impossible statistics circulating on the dark net. Then the myth became a legend: a few leaked photographs so provocative that possession was a federal crime. Then the legend became a rumor: a car so exclusive no one could confirm it existed in the real world. And now, thanks to you, that rumor is about to become a very messy headline.", true, ItemType.Vehicles) { ModelName = "deveste", Description = "It began as little more than a myth: a list of impossible statistics circulating on the dark net. Then the myth became a legend: a few leaked photographs so provocative that possession was a federal crime. Then the legend became a rumor: a car so exclusive no one could confirm it existed in the real world. And now, thanks to you, that rumor is about to become a very messy headline.", },
            new VehicleItem("Principe Diabolus", "You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", true, ItemType.Vehicles) { ModelName = "diablous", Description = "You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", },
            new VehicleItem("Principe Diabolus Custom", "You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", true, ItemType.Vehicles) { ModelName = "diablous2", Description = "You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", },
            new VehicleItem("Principe Lectro", "As if this new-school streetfighter didn’t look aggressive enough, once you hit that KERS button you’ll be locked into a death struggle with the laws of physics - and there can only be one winner.", true, ItemType.Vehicles) { ModelName = "lectro", Description = "As if this new-school streetfighter didnâ€™t look aggressive enough, once you hit that KERS button youâ€™ll be locked into a death struggle with the laws of physics - and there can only be one winner.", },
            new VehicleItem("Principe Nemesis", "Super fast, super unshielded. When you're riding a Nemesis, you don't just feel the wind in your hair, you feel it tearing into the back of your eye sockets.", ItemType.Vehicles) { ModelName = "nemesis", Description = "Super fast, super unshielded. When you're riding a Nemesis, you don't just feel the wind in your hair, you feel it tearing into the back of your eye sockets.", },
            new VehicleItem("Progen Emerus", true, ItemType.Vehicles) { ModelName = "emerus", Description = "There is a new word in pioneering automotive design: intuition. Taking your seat in an Emerus is not like getting into a car. It's like discovering a new and perfectly natural extension to your own body, which just happens to be made of 800 angry horses. So, when it tears you mercilessly limb from limb, you'll have no one but yourself to blame.", },
            new VehicleItem("Progen PR4", true, ItemType.Vehicles) { ModelName = "formula" },
            new VehicleItem("Progen GP1", true, ItemType.Vehicles) { ModelName = "gp1", Description = "Seasons will change, fashions will come and go, economies will tank, the wholesome popstars of today will be leaking their own bondage tapes tomorrow - but the GP1 will always remain. This is what defined supercars for a generation, perhaps for all time: as pure and flawless as the smile of a newborn, or a crystal of perfectly refined meth, or the smile of a newborn experiencing perfectly refined meth.", },
            new VehicleItem("Progen Itali GTB", true, ItemType.Vehicles) { ModelName = "italigtb", Description = "Lithe, focused, aggressive: if you've ever made passionate love to an angry jungle cat, you'll have an inkling of what it's like to take Progen's new Itali GTB out for a gentle spin. And if you've ever made passionate love to a stripped down, track-ready jungle cat with a massive rear spoiler, you'll have some idea of what Benny can do to this thing.", },
            new VehicleItem("Progen Itali GTB Custom", true, ItemType.Vehicles) { ModelName = "italigtb2", Description = "Lithe, focused, aggressive: if you've ever made passionate love to an angry jungle cat, you'll have an inkling of what it's like to take Progen's new Itali GTB out for a gentle spin. And if you've ever made passionate love to a stripped down, track-ready jungle cat with a massive rear spoiler, you'll have some idea of what Benny can do to this thing.", },
            new VehicleItem("Progen T20", true, ItemType.Vehicles) { ModelName = "t20", Description = "Tell your liberal neighbors you bought the Progen for its 'fuel efficiency' and reduced 'carbon' emissions, when you really got it because they hooked an electric motor to a twin-turbocharged V8 engine just to give it extra juice. Like a toaster in a bathtub, this is a dangerous synthesis of technologies old and new. Be progressive in the only real sense of the word.", },
            new VehicleItem("Progen Tyrus", true, ItemType.Vehicles) { ModelName = "tyrus", Description = "Originally designed as a road-legal confidence booster for the wealthy but poorly endowed, the Tyrus found its true calling elsewhere. It didn't just beat its race-tuned rivals - it serially teabagged them across every endurance grand prix in competitive motorsport. Which is particularly good for you, because you can pretend this isn't all about your tiny, tiny package.", },
            new VehicleItem("RUNE Cheburek", true, ItemType.Vehicles) { ModelName = "cheburek", Description = "Don't be fooled by a lick of paint and polish: underneath the showroom finish the Cheburek is nothing but a lump of iron curtain that's been smelted down and hastily recast for the glories of the free market. As for the rumors that the exterior design was outsourced to a five-year-old with nothing but a crayon and a crippling hangover, we can only tell you that deregulated entrepreneurship is a wonderful thing and we support it 100%.", },
            new VehicleItem("Schyster Deviant", true, ItemType.Vehicles) { ModelName = "deviant", Description = "It wasn't as beautifully proportioned as the Sabre, or as fast as the Dominator, but that didn't matter - in the 70's, the real connoisseurs of muscle were all about the Deviant. Well now the dark horse is all grown up, and thanks to Team Schyster's ground-breaking remodeling this tricked-out, wild-eyed stallion is all set to put a hoof through your skull at the first opportunity.", },
            new VehicleItem("Schyster Fusilade", ItemType.Vehicles) { ModelName = "fusilade" },
            new VehicleItem("Shitzu Defiler", true, ItemType.Vehicles) { ModelName = "defiler", Description = "A light and nimble streetfighter. With acceleration like this, youâ€™ll be popping a wheelie straight onto the judgeâ€™s desk and mooning the jury in no time.", },
            new VehicleItem("Shitzu Hakuchou", true, ItemType.Vehicles) { ModelName = "hakuchou" },
            new VehicleItem("Shitzu Hakuchou Drag", true, ItemType.Vehicles) { ModelName = "hakuchou2" },
            new VehicleItem("Shitzu PCJ 600", ItemType.Vehicles) { ModelName = "pcj", Description = "A cruiser, for the weekend warrior who is holding on to the dream.", },
            new VehicleItem("Shitzu Vader", ItemType.Vehicles) { ModelName = "Vader" },
            new VehicleItem("Stanley Fieldmaster", ItemType.Vehicles) { ModelName = "tractor2" },
            new VehicleItem("Stanley Fieldmaster", ItemType.Vehicles) { ModelName = "tractor3" },
            new VehicleItem("Truffade Adder", ItemType.Vehicles) { ModelName = "adder", Description = "If cars were porn, this would be the ultimate DVDA scene. Give the liberals something to really protest about with the least environmentally-friendly car on the planet! The Adder's monstrous 8-liter engine burns fuel faster than a blazing oil refinery, but it reaches speeds of 250mph, making it the perfect all-round car for life in a busy urban metropolis.", },
            new VehicleItem("Truffade Nero", true, ItemType.Vehicles) { ModelName = "nero", Description = "Fresh from Truffade's undersea test track, the Nero is a god-emperor among supercars that'll dip its rivals in burning oil, seduce its mother and play the fiddle while the ozone burns. And if all the slack-jawed Adder owners aren't jealous enough already, Benny's deep-vented, mod-ready overhaul is going to have them driving their million-dollar golf buggies into the sea in despair.", },
            new VehicleItem("Truffade Nero Custom", true, ItemType.Vehicles) { ModelName = "nero2", Description = "Fresh from Truffade's undersea test track, the Nero is a god-emperor among supercars that'll dip its rivals in burning oil, seduce its mother and play the fiddle while the ozone burns. And if all the slack-jawed Adder owners aren't jealous enough already, Benny's deep-vented, mod-ready overhaul is going to have them driving their million-dollar golf buggies into the sea in despair.", },
            new VehicleItem("Truffade Thrax", true, ItemType.Vehicles) { ModelName = "thrax", Description = "Forget unitary construction. Put your monocoque back in your pants. The rolling chassis is back, and it's making sweet, naphtha-kerosene-drenched love to the hottest body in the world. Don't worry if you don't understand any of that, just take the corners hard and you feel the future. Trust us.", },
            new VehicleItem("Truffade Z-Type", ItemType.Vehicles) { ModelName = "ztype", Description = "Weather the new Great Depression with a car from the last Great Depression. When this rolled off the production line in 1937, minorities and women knew their place. It was the world's fastest automobile. Now it's the world's most expensive second-hand automobile. One of only 10 ever made, the Z-Type is a car you can really enjoy sitting in, surrounded by armed guards, too terrified to actually drive it anywhere.", },
            new VehicleItem("Ubermacht Oracle XS", ItemType.Vehicles) { ModelName = "oracle", Description = "The ultimate status symbol for the wannabe executive. Let the world know that you're not just a middle-manager anymore. You're a middle-manager who's financially crippled himself with a car he can't afford. Leverage the dream today.", },
            new VehicleItem("Ubermacht Oracle", ItemType.Vehicles) { ModelName = "oracle2", Description = "A fantastic piece of German engineering. So much that an oil change will cost you $500 at the dealership.", },
            new VehicleItem("Ubermacht Revolter", true, ItemType.Vehicles) { ModelName = "revolter", Description = "In your line of work, you demand flexibility. You need a car that can blend seamlessly into a line of executive saloons, but not look amiss when it arrives on the red carpet. It needs to look respectable dropping you off at court, threatening picking you up from Bolingbroke, and when the deal goes south, it needs room for a driver-operated machine gun upgrade. The Revolter can do all that in first gear â€“ just wait till you see fifth.Please note: Weapon modifications can only be applied at a Vehicle Workshop inside an Avenger or Mobile Operations Center.", },
            new VehicleItem("Ubermacht SC1", true, ItemType.Vehicles) { ModelName = "sc1", Description = "Ubermacht's first supercar is a place where powerful forces meet: the past encountering the future; the elegance and status of traditional design coming up against the relentless pursuit of revolutionary performance; your boundless sense of superiority and entitlement battling with your crushing insecurity and hunger for approval. Thanks to the SC1, you can have them all.", },
            new VehicleItem("Ubermacht Sentinel XS", ItemType.Vehicles) { ModelName = "sentinel", Description = "When you're doing 90 in the fast lane, this is the car right on your ass flashing its high beams. If you're quite rich, and really an asshole, and you want everyone to know it, you can't do better.", },
            new VehicleItem("Ubermacht Sentinel 2", ItemType.Vehicles) { ModelName = "sentinel2", Description = "When you're doing 90 in the fast lane, this is the car right on your ass flashing its high beams. If you're quite rich, and really an asshole, and you want everyone to know it, you can't do better.", },
            new VehicleItem("Ubermacht Sentinel 3", true, ItemType.Vehicles) { ModelName = "sentinel3", Description = "There was a time when a road-legal coupe could moonlight as a performance rally car and no one batted an eye. Consumers didn't need the reassurance of a touch-screen interface and integrated GPS. People could get behind a no-frills, lightweight bucket of speed, and their relatives wouldn't sue the manufacturer when it burst into flames. Those were the days of the Sentinel Classic - and now they're back.", },
            new VehicleItem("Ubermacht Zion", ItemType.Vehicles) { ModelName = "zion", Description = "A German beauty of timeless passion and sleek design. When you're interested in class and an overpowered engine, there is no other choice.", },
            new VehicleItem("Ubermacht Zion Cabrio", ItemType.Vehicles) { ModelName = "zion2", Description = "German engineering and design taken very, very seriously. This convertible Zion model is guaranteed to put the wind in any senior executive combover.", },
            new VehicleItem("Ubermacht Zion Classic", true, ItemType.Vehicles) { ModelName = "zion3", Description = "There's no denying it. Something magical happened in the 80's. It's not just that their hair was more buoyant, their choruses catchier, their spandex tighter and their glutes perkier. It's that now they're all in their sixties, and their hair is still buoyant, their choruses are still catchy, their spandex is still tight, and their glutes are still perky. And whatever they've been taking, the Zion Classic has been taking twice the dose.", },
            new VehicleItem("Ubermacht Cypher", true, ItemType.Vehicles) { ModelName = "cypher" },
            new VehicleItem("Ubermacht Rebla GTS", true, ItemType.Vehicles) { ModelName = "rebla", Description = "Here you are again, stuck in the commute, bricked in with all the other SUVs. How much time have you spent here? Weeks? Months? Years? Screw it. Stamp the gas, ram the traffic and take to the hills. You're free. Free from the system. Of course, you're too cowardly for that. But one day you'll snap and you'll be glad you have the reliable Rebla GTS to speed you away from reality.", },
            new VehicleItem("Vapid Benson", ItemType.Vehicles) { ModelName = "Benson" },
            new VehicleItem("Vapid Blade", true, ItemType.Vehicles) { ModelName = "blade" },
            new VehicleItem("Vapid Bobcat XL", ItemType.Vehicles) { ModelName = "bobcatXL" },
            new VehicleItem("Vapid Bullet", ItemType.Vehicles) { ModelName = "bullet", Description = "They don't make 'em like they used to. Which is why Vapid designed the retro-classic Bullet based on the racing cars of the 1960s, back when nobody gave a crap about carbon footprints or the Ozone layer.", },
            new VehicleItem("Vapid Caracara", true, ItemType.Vehicles) { ModelName = "caracara" },
            new VehicleItem("Vapid Caracara 4x4", true, ItemType.Vehicles) { ModelName = "caracara2" },
            new VehicleItem("Vapid Chino", true, ItemType.Vehicles) { ModelName = "chino", Description = "Whether you're planning a game of tennis, an orgy, a (literal) bloodbath or all of the above, there's ample room for it behind the tinted, sound-proof windows of this true American classic. Pop the trunk to find a built in power hose, custom-engineered to remove blood, fecal matter and regret from the panda-belly leather interiors. The choice of the statesman.", },
            new VehicleItem("Vapid Chino Custom", true, ItemType.Vehicles) { ModelName = "chino2", Description = "The Chino may be as close to perfection as a car can get - but then, would you deny your wife just because she's 'naturally beautiful'? A car this size has room for every modification know to man, so if you don't force your extravagant tastes onto every timeless inch of it you may as well move to Europe and die of treason. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Vapid Clique", true, ItemType.Vehicles) { ModelName = "clique", Description = "In the 50's, the Vapid Clique was one thing an anxious parent didn't want to see pulling up outside the white picket fence on prom night. That haze of postwar confidence and unfiltered cigarette smoke meant only one thing - and you could be sure its intentions were not honorable. This lovingly reconstructed model brings modern power and modern reliability, without losing a single drop of the dog whistle bigotry that makes this car the classic it is.", },
            new VehicleItem("Vapid Contender", true, ItemType.Vehicles) { ModelName = "contender", Description = "Are you a VIP in need of tasteful yet robust transport? Aspiring crimelord with an eye for flexible seating and storage space? Survivalist with thinly veiled paramilitary intentions? However deranged your aspirations, the Vapid Contender has got you covered.", },
            new VehicleItem("Vapid Dominator", "Baby boomer teen dream repackaged for the mass market generation. A muscle car without the muscle that's the rental vehicle of choice for tourists looking for a slice of 'real' Americana.", ItemType.Vehicles) { ModelName = "dominator", Description = "Baby boomer teen dream repackaged for the mass market generation. A muscle car without the muscle that's the rental vehicle of choice for tourists looking for a slice of 'real' Americana.", },
            //new VehicleItem("Vapid Pisswasser Dominator", ItemType.Vehicles) { ModelName = "dominator2", Description = "Baby boomer teen dream repackaged for the mass market generation. A muscle car without the muscle that's the rental vehicle of choice for tourists looking for a slice of 'real' Americana. Get it with exclusive Pisswasser Livery.", },
            new VehicleItem("Vapid Dominator GTX", "Step one: take the best-looking muscle car the 60's ever saw, and introduce it to the greatest American supercar of the modern era. When your pedigree is this damn good, there's nothing wrong with keeping it in the family.", ItemType.Vehicles) { ModelName = "dominator3", Description = "Step one: take the best-looking muscle car the 60's ever saw, and introduce it to the greatest American supercar of the modern era. Step two: leave them alone in a quiet garage with a few dozen shots of high octane gas, plenty of axel grease and nothing else to do. Step three: the Dominator GTX is born, and it's hungry. When your pedigree is this damn good, there's nothing wrong with keeping it in the family.", },
            new VehicleItem("Vapid Apocalypse Dominator", true, ItemType.Vehicles) { ModelName = "dominator4", Description = "Twenty-eighth generation muscle car in dire need of some antipsychotics.", },
            new VehicleItem("Vapid Future Shock Dominator", true, ItemType.Vehicles) { ModelName = "dominator5", Description = "Whatever drugs you're dealing in the year 2200, this is your ride.", },
            new VehicleItem("Vapid Nightmare Dominator", true, ItemType.Vehicles) { ModelName = "dominator6", Description = "Where 'Bright neon motifs' and 'plausibly radioactive' meet.", },
            new VehicleItem("Vapid Dominator ASP", true, ItemType.Vehicles) { ModelName = "dominator7", Description = "From the all-American line of Dominators comes the middle generation ASP. Too contemporary to be a classic, too old-school to be cutting edge, this is the kind of car you forget all about until it punches you square in the junk. Now pony up.", },
            new VehicleItem("Vapid Dominator GTT", true, ItemType.Vehicles) {ModelName = "dominator8",Description = "You've met the Vapid Dominator, now it's time to meet its maker. And this quinquagenarian is still the hottest piece of muscle on the road. Don't believe us? Just climb inside and let the vibrations do the rest: zero to 'Don't stop, Daddy' in 4.76 seconds...", },new VehicleItem("Vapid Ellie", true, ItemType.Vehicles) { ModelName = "ellie", Description = "Oversized, oversexed, overpowered and understeered, this is the car that joined cheap contraception and masturbatory guitar solos to form the unholy trinity of Baby Boom Americana. But before you reach for your vintage jacket and the sepia filter on your Snapmatic, be warned: this old dog has torn the limbs off braver hipsters than you...", },
            new VehicleItem("Vapid Flash GT", true, ItemType.Vehicles) { ModelName = "flashgt" },
            new VehicleItem("Vapid FMJ", true, ItemType.Vehicles) { ModelName = "fmj", Description = "As the rate of infant heart disease suggests, the power to weight ratio has never been America's strong suit - until now. With the FMJ, Vapid put the American supercar on a raw food diet and gave it colonic irrigation. The result? With the same primal engine under bodywork that's 90% carbon fiber and 10% patriotic sentiment, this thing will go 0 to 60 on the back of nothing more than a light sneeze.", },
            new VehicleItem("Vapid GB200", true, ItemType.Vehicles) { ModelName = "gb200", Description = "The GB200 is an icon of that golden age of sports car design: a mid-engine, four-wheel drive rocket built with the power of a modern supercar and the handling, brakes and safety features of an angry dog. You can play it cool all you like: no matter how many times you take it over 100, the moment the turbo kicks in will have you clenching so hard you won't know if that was an ecstatic climax or a messy follow-through.", },
            new VehicleItem("Vapid Guardian", true, ItemType.Vehicles) { ModelName = "guardian" },
            new VehicleItem("Vapid Hotknife", ItemType.Vehicles) { ModelName = "hotknife" },
            new VehicleItem("Vapid Hustler", true, ItemType.Vehicles) { ModelName = "hustler", Description = "Don't worry, the confusion is natural. You see that running board swooping low over the front wheels, that high-set radiator grille, the holder for your cigarette holder, and you're safely back where you belong in the 1920s. But then you see the carbon steel reinforcements to the chassis, the turbo charger and the bullbar, and you're somewhere else altogether. Don't try to make sense of it. Just let it do what it needs to do.", },
            new VehicleItem("Vapid Apocalypse Imperator", true, ItemType.Vehicles) { ModelName = "imperator", Description = "They built 'em good in the Seventies. So good, you can stick a meat grinder on the front and drive right through the badlands without losing a single drop of effortless cool.", },
            new VehicleItem("Vapid Future Shock Imperator", true, ItemType.Vehicles) { ModelName = "imperator2", Description = "Behold the classic muscle car's final form: sleek, polished and impregnable. If you needed proof that the Imperator heritage would broadcast your insecurities right into the space age, this is it.", },
            new VehicleItem("Vapid Nightmare Imperator", true, ItemType.Vehicles) { ModelName = "imperator3", Description = "You know what doesn't biodegrade? Cluckin' Bell chicken strips and Seventies cool. With those as your design template, you'll be driving something that'll stay classy and trashy all the way till the end of time.", },
            new VehicleItem("Vapid Minivan", ItemType.Vehicles) { ModelName = "minivan" },
            new VehicleItem("Vapid Minivan Custom", true, ItemType.Vehicles) { ModelName = "minivan2" },
            new VehicleItem("Vapid Monster", true, ItemType.Vehicles) { ModelName = "monster" },
            new VehicleItem("Vapid Peyote", ItemType.Vehicles) { ModelName = "peyote", Description = "Once upon a time, in order to get your hands on a real Peyote you had to scope the right neighborhood, try to hotwire it before anyone saw you, lie low for two days, and only go to the mod shop after you'd dug the bullets out your ass. Now, Benny will personally steal it, change the plates, dig the bullets out of his own ass, supermod it, and tell your friends that you did it all yourself. Eligible for customization at Benny's Original Motor Works. ", },
            new VehicleItem("Vapid Peyote Gasser", true, ItemType.Vehicles) { ModelName = "peyote2", Description = "Back in the 50's, drag racing was a more straightforward affair. You took a beautiful post-war convertible, gave it a serious engine upgrade and a nosebleed stance, and took it onto the streets. Sure, the weight transfer didn't count for much when the front end picked up more air resistance than an open parachute, and the survival rate over 150mph was pretty low. But that's the price you pay for historical accuracy.", },
            new VehicleItem("Vapid Peyote Custom", true, ItemType.Vehicles) { ModelName = "peyote3", Description = "Once upon a time, in order to get your hands on a real Peyote you had to scope the right neighborhood, try to hotwire it before anyone saw you, lie low for two days, and only go to the mod shop after you'd dug the bullets out of your ass. Now, Benny will personally steal it, change the plates, dig the bullets out of his own ass, supermod it, and tell your friends you did it all yourself. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Vapid Radius", ItemType.Vehicles) { ModelName = "radi", Description = "One of the best-selling mid-size crossovers on the market today. Enjoy the feel and functionality of an SUV without everybody berating you about your carbon footprint.", },
            new VehicleItem("Vapid Retinue", true, ItemType.Vehicles) { ModelName = "retinue", Description = "The Vapid Retinue began its life as a blue-collar hero: an overpowered, oversteering, gas-guzzler, built and sold for the working man. From those humble beginnings it became one of the most successful rally cars of all time. And now, all that rich history makes it prime hipster bait for the soulful one-percenter for some authenticity. Yep, that's where you come in.", },
            new VehicleItem("Vapid Retinue Mk II", true, ItemType.Vehicles) { ModelName = "retinue2", Description = "Some sequels try to reinvent what's come before and stand alone on their own terms; the rest are bigger, brasher rehashes of the originals. Vapid knew exactly what they were aiming for with the Retinue MkII - its predecessor's soft curves were replaced with hard angles and the overpowered engine and rally-ready suspension came back with a vengeance. This is an American car for the European boy-racer market that feels all too familiar, and there's no shame in that.", },
            new VehicleItem("Vapid Riata", true, ItemType.Vehicles) { ModelName = "riata" },
            new VehicleItem("Vapid Sadler", ItemType.Vehicles) { ModelName = "Sadler" },
            new VehicleItem("Vapid Sadler 2", ItemType.Vehicles) { ModelName = "sadler2" },
            new VehicleItem("Vapid Sandking XL", ItemType.Vehicles) { ModelName = "sandking", OverrideLoadBodiesInBed = true, BedLoadOffsetOverride = new Vector3(0f,-1.5f,1.5f) },
            new VehicleItem("Vapid Sandking SWB", ItemType.Vehicles) { ModelName = "sandking2", OverrideLoadBodiesInBed = true, BedLoadOffsetOverride = new Vector3(0f,-1.5f,1.5f) },
            new VehicleItem("Vapid Slamtruck", true, ItemType.Vehicles) { ModelName = "slamtruck" },
            new VehicleItem("Vapid Slamvan", true, ItemType.Vehicles) { ModelName = "slamvan" },
            new VehicleItem("Vapid Lost Slamvan", true, ItemType.Vehicles) { ModelName = "slamvan2" },
            new VehicleItem("Vapid Slamvan Custom", true, ItemType.Vehicles) { ModelName = "slamvan3", Description = "The Slamvan is your ride straight back to a time when your shrink prescribed LSD, your President was a bigot, most people were bigots, and pickup trucks were actually pretty cool. Sure, some things have changed since the 50s, but with the brutal mod options we offer on this rig you can show everyone that utility vehicles ain't all about utility... or racist bumper stickers and gun racks.", },
            new VehicleItem("Vapid Apocalypse Slamvan", true, ItemType.Vehicles) { ModelName = "slamvan4", Description = "Never start a cataclysmic resource war without one.", },
            new VehicleItem("Vapid Future Shock Slamvan", true, ItemType.Vehicles) { ModelName = "slamvan5", Description = "The only difference between utopia and dystopia is which side of the 'dozer blade you're on.", },
            new VehicleItem("Vapid Nightmare Slamvan", true, ItemType.Vehicles) { ModelName = "slamvan6", Description = "Perfectly camouflaged for the landfill at the end of the world.", },
            new VehicleItem("Vapid Speedo", ItemType.Vehicles) { ModelName = "speedo" },
            new VehicleItem("Vapid Clown Van", ItemType.Vehicles) { ModelName = "speedo2" },
            new VehicleItem("Vapid Speedo Custom", true, ItemType.Vehicles) { ModelName = "speedo4" },
            new VehicleItem("Vapid Stanier", "If you took a cab or got arrested in the 1990s, there's a high chance you ended up in the back of a Vapid Stanier. Discontinued following widespread reports of fuel tanks exploding on impact in rear-end collisions. So try to avoid that.", ItemType.Vehicles) { ModelName = "stanier", Description = "If you took a cab or got arrested in the 1990s, there's a high chance you ended up in the back of a Vapid Stanier. Discontinued following widespread reports of fuel tanks exploding on impact in rear-end collisions. So try to avoid that.", },
            new VehicleItem("Vapid Trophy Truck", true, ItemType.Vehicles) { ModelName = "trophytruck" },
            new VehicleItem("Vapid Desert Raid", true, ItemType.Vehicles) { ModelName = "trophytruck2" },
            new VehicleItem("Vapid Winky", true, ItemType.Vehicles) { ModelName = "winky" },
            new VehicleItem("Vulcar Fagaloa", true, ItemType.Vehicles) { ModelName = "fagaloa", Description = "Admit it. You took one look and assumed this was just a typical 50's station wagon - and you weren't wrong. But look again at the magnificent box styling, the stance so low you couldn't slide melted butter underneath it, the faint afterglow of casual bigotry, and ask yourself: where did it all go wrong, and fifty years later how the hell did we end up driving SUV's? It's not nostalgia. Things really were better.", },
            new VehicleItem("Vulcar Ingot", ItemType.Vehicles) { ModelName = "ingot", Description = "Reputed to be the safest car ever made, this classic Vulcar station wagon has been the vehicle of choice for really terrible drivers for 20 years. The closest thing you'll ever get to a tank for under $10k.", },
            new VehicleItem("Vulcar Nebula Turbo", true, ItemType.Vehicles) { ModelName = "nebula", Description = "We know what you're thinking. Another mid-seventies brick. But think about it for a moment. A brick is only slow if you don't throw it right. And does a hollowed-out, turbo-charged brick really sound like it's that difficult to throw? That's the spirit. Batter up.", },
            new VehicleItem("Vulcar Warrener", true, ItemType.Vehicles) { ModelName = "warrener", Description = "The legendary Swedish four door sedan. All the boxy design of a 1980s Vulcar, with none of the reliability. For lovers of tight jeans, micro breweries, and 70s cop shows.", },
            new VehicleItem("Vulcar Warrener HKR", true, ItemType.Vehicles) { ModelName = "warrener2", Description = "Looking for something sturdy and reliable? Sometimes the car you need isn't the one you came in to buy. It's like going to the pound and being drawn to the old dog with three legs, one eye, and a real bad compulsion to hump anything that moves. If you can convince yourself that everyone is saying 'ew' and 'wtf' because it's just so gosh-darn adorable, the Warrener HKR is all set to be a companion for life.", },
            new VehicleItem("Vysser Neo", true, ItemType.Vehicles) { ModelName = "neo", Description = "Vysser are the kind of small, artisanal manufacturer who are prepared to think outside the box. The design blueprints for the Neo drew their inspiration from the aerodynamics of a diving falcon, the composure of a ballet dancer, the curve of a suggestively raised eyebrow, and the assertiveness of a cluster bomb. This is the kind of pedigree you need.", },
            new VehicleItem("Weeny Dynasty", true, ItemType.Vehicles) { ModelName = "dynasty", Description = "England. 1956. In just a few years' time, Weeny are going to produce the Issi, and the automotive equivalent of a quadruple shot low fat half caff flat white is going to change the world forever. But for now, they're still the world's most respected manufacturer of middle-of-the-road, middle-class, middle-income, about-to-get-fired-from-middle-management cars. The Dynasty is their magnum opus. And in 1957, they're going to sell the design, and the subcontinent is never going to be the same againï»¿..ï»¿.ï»¿", },
            new VehicleItem("Weeny Issi", ItemType.Vehicles) { ModelName = "issi2", Description = "A favorite in Mirror Park, this sporty little guy is easy to park. Great handling, anti-lock brakes, and a stereo that only plays power pop hits. Ironically. Go team!", },
            new VehicleItem("Weeny Issi Classic", true, ItemType.Vehicles){ ModelName = "issi3",Description = "Sure, you could afford the latest Ocelot, or a vintage Enus - but you're not a show-off. You're just a slightly quirky, totally down-to-earth person looking for a way to tell everyone how slightly quirky and totally down-to-earth you are. Well, look no further, because for the last half-century the Issi Classic has been the closest thing on four wheels to screaming 'I've got nothing to prove to anyone' in the face of every stranger you meet.", },
            new VehicleItem("Weeny Apocalypse Issi", true, ItemType.Vehicles) { ModelName = "issi4", Description = "Highly aggressive things come in small packages.", },
            new VehicleItem("Weeny Future Shock Issi", true, ItemType.Vehicles) { ModelName = "issi5", Description = "So chic you barely notice its only purpose is to kill you.", },
            new VehicleItem("Weeny Nightmare Issi", true, ItemType.Vehicles) { ModelName = "issi6", Description = "Go on, call it cute. See what happens.", },
            new VehicleItem("Weeny Issi Sport", true, ItemType.Vehicles) { ModelName = "issi7", Description = "Underneath the hipster chic, you always suspected the Issi had a bad case of small car syndrome. Well, now the flat whites and ironic power pop are out, and the roll cage, carbon fiber interior and turbo charged four-cylinder are in. Just don't mention its height.", },
            new VehicleItem("Western Bagger", ItemType.Vehicles) { ModelName = "bagger", Description = "As every Bagger owner will tell you without a trace of all-consuming regret, 'It's a great bike for cruising'. Which is another way of saying 'It's a great bike for not going anywhere quickly or efficiently'. So deep into grandpa chic it comes full circle and achieves gangland charm, this is the bike for you if you can't make up your mind and don't care how you look in the meantime.", },
            new VehicleItem("Western Cliffhanger", true, ItemType.Vehicles) { ModelName = "cliffhanger", Description = "There's only one reason to buy a Cliffhanger, but it's all the reason you need. As it sits there between your legs, throbbing gently, a roar of ecstatic virility only a flick of your wrist away, you realize that this is far more than just a series of transparent innuendos: it's the pneumatic appendage you've always dreamed of.", },
            new VehicleItem("Western Daemon LOST", ItemType.Vehicles) { ModelName = "daemon" },
            new VehicleItem("Western Daemon", true, ItemType.Vehicles) { ModelName = "daemon2" },
            new VehicleItem("Western Apocalypse Deathbike", true, ItemType.Vehicles) { ModelName = "deathbike", Description = "Angel of Death meets open sewer.", },
            new VehicleItem("Western Future Shock Deathbike", true, ItemType.Vehicles) { ModelName = "deathbike2", Description = "Whatever this is, it does not come in peace.", },
            new VehicleItem("Western Nightmare Deathbike", true, ItemType.Vehicles) { ModelName = "deathbike3", Description = "The only bike able to climb a 45% hill of discarded PiÃŸwasser bottles.", },
            new VehicleItem("Western Gargoyle", true, ItemType.Vehicles) { ModelName = "gargoyle", Description = "Ah, the age-old question: how do you get a cool vintage motorbike up a near-vertical hillside strewn with dust, rocks and the remains of lesser drivers? Forget carbon fiber panels and onboard computers. Sometimes a simple problem requires a simple solution, like a rear tire taken from an Armored Personnel Carrier and wrapped in steel chains. Time to get back to basics.", },
            new VehicleItem("Western Nightblade", true, ItemType.Vehicles) { ModelName = "nightblade" },
            new VehicleItem("Western Rat Bike", true, ItemType.Vehicles) { ModelName = "ratbike", Description = "Like any real biker you'd rather spend your hard-stolen cash on smokes, liquor and crates of wet wipes. Well look no further, this is the ride for you. We haven't done much more than scrape off the remains of the previous owner, fill it with enough glue and diesel to keep it together at 80mph, and sell it on. It's like recycling, but really bad for the environment.", },
            new VehicleItem("Western Rampant Rocket", true, ItemType.Vehicles) { ModelName = "rrocket", Description = "There was a time when no respectable motorist would admit to owning a trike. It was a kink you worked out on your own, once everyone else was in bed, behind sound-proofed garage doors. And then one day the Rampant Rocket launched, and everything changed. Once that supercharger found its way between your legs, it was impossible to be discreet, and impossible to feel ashamed. This is what liberation looks like.", },
            //new VehicleItem("Western Sovereign", true, ItemType.Vehicles) { ModelName = "sovereign" },
            new VehicleItem("Western Wolfsbane", true, ItemType.Vehicles) { ModelName = "wolfsbane" },
            new VehicleItem("Western Zombie Bobber", true, ItemType.Vehicles) { ModelName = "zombiea" },
            new VehicleItem("Western Zombie Chopper", true, ItemType.Vehicles) { ModelName = "zombieb" },
            new VehicleItem("Willard Faction", true, ItemType.Vehicles) { ModelName = "faction", Description = "With its squared-off bodywork, sensible engineering and T-Top roof, the Faction has recently begun to lose its historic association with high finance and cases of sexual assault. These days, it's old and cheap enough to attract a generation of you artisans who dream of nothing more than finding an old car and hiring someone else to make it look good. Being a pioneer has never been this easy. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Willard Faction Custom", true, ItemType.Vehicles) { ModelName = "faction2", Description = "With its squared-off bodywork, sensible engineering and T-Top roof, the Faction has recently begun to lose its historic association with high finance and cases of sexual assault. These days, it's old and cheap enough to attract a generation of you artisans who dream of nothing more than finding an old car and hiring someone else to make it look good. Being a pioneer has never been this easy. Eligible for customization at Benny's Original Motor Works.", },
            new VehicleItem("Willard Faction Custom Donk", true, ItemType.Vehicles) { ModelName = "faction3", Description = "New and exclusive to Benny's Original Motor Works, a feat of motor engineering so ludicrous there's no point trying to understand it, question it, or stop yourself blowing the last of your kids' college fund on it. And if a giant, motorized junk food billboard with 40 inch chrome wheels and a sky-high center of gravity doesn't immediately make sense to you, it's never going to. Remember, no one's really sure what the competition is anymore, but with one of these you're almost certainly winning.", },
            new VehicleItem("Zirconium Journey", ItemType.Vehicles) { ModelName = "journey" },
            new VehicleItem("Zirconium Stratum", ItemType.Vehicles) { ModelName = "stratum" },

            //Emergency
            new VehicleItem("Vapid Sheriff Stanier", ItemType.Vehicles) { ModelName = "sheriff",OverrideMakeName = "Vapid", OverrideClassName = "Sedan", },
            new VehicleItem("Declasse Sheriff Granger", ItemType.Vehicles) { ModelName = "sheriff2",OverrideMakeName = "Declasse", OverrideClassName = "SUV", },
            new VehicleItem("Vapid Police Stanier", ItemType.Vehicles) { ModelName = "police",OverrideMakeName = "Vapid", OverrideClassName = "Sedan", },
            new VehicleItem("Bravado Police Buffalo", ItemType.Vehicles) { ModelName = "police2",OverrideMakeName = "Bravado", OverrideClassName = "Sedan", },
            new VehicleItem("Vapid Police Interceptor", ItemType.Vehicles) { ModelName = "police3",OverrideMakeName = "Vapid", OverrideClassName = "Sedan", },
            new VehicleItem("Vapid Police Unmarked Stanier", ItemType.Vehicles) { ModelName = "police4",OverrideMakeName = "Vapid", OverrideClassName = "Sedan", },
            new VehicleItem("Declasse Police Transporter", ItemType.Vehicles) { ModelName = "policet",OverrideMakeName = "Declasse", OverrideClassName = "Van", },
            new VehicleItem("Buckingham Police Maverick", ItemType.Vehicles) { ModelName = "polmav",OverrideMakeName = "Buckingham", OverrideClassName = "Helicopter", },
            new VehicleItem("Brute Police Riot", ItemType.Vehicles) { ModelName = "riot",OverrideMakeName = "Brute", OverrideClassName = "Van", },
            new VehicleItem("Western Police Bike", ItemType.Vehicles) { ModelName = "policeb",OverrideMakeName = "Western", OverrideClassName = "Motorcycle", },
            new VehicleItem("Declasse Park Ranger Granger", ItemType.Vehicles) { ModelName = "pranger",OverrideMakeName = "Declasse", OverrideClassName = "SUV", },
            new VehicleItem("Declasse Lifeguard Granger", ItemType.Vehicles) { ModelName = "lguard",OverrideMakeName = "Declasse", OverrideClassName = "SUV", },
            new VehicleItem("Declasse Police Unmarked Granger", ItemType.Vehicles) { ModelName = "fbi2",OverrideMakeName = "Declasse", OverrideClassName = "SUV", },
            new VehicleItem("Bravado Police Unmarked Buffalo", ItemType.Vehicles) { ModelName = "fbi",OverrideMakeName = "Bravado", OverrideClassName = "Sedan", },

            new VehicleItem("MTL Fire Truck", ItemType.Vehicles) { ModelName = "firetruk",OverrideMakeName = "MTL", OverrideClassName = "Utility", },
            new VehicleItem("Brute Ambulance", ItemType.Vehicles) { ModelName = "ambulance" ,OverrideMakeName = "Brute", OverrideClassName = "Van",},
            new VehicleItem("Police Predator", ItemType.Vehicles) { ModelName = "predator" },

            new VehicleItem("Vapid Police Prison Bus", ItemType.Vehicles) { ModelName = "pbus",OverrideMakeName = "Vapid", OverrideClassName = "Van",},



            new VehicleItem("Albany Police Roadcruiser", ItemType.Vehicles) { ModelName = "policeold2",OverrideMakeName = "Albany", OverrideClassName = "Sedan", },
            new VehicleItem("Declasse Police Rancher", ItemType.Vehicles) { ModelName = "policeold1",OverrideMakeName = "Declasse", OverrideClassName = "SUV", },

            new VehicleItem("Speedophile Lifeguard Seashark", ItemType.Vehicles) { ModelName = "seashark2",OverrideMakeName = "Speedophile", OverrideClassName = "Jet Ski", },

            //Industrial
            new VehicleItem("Vapid Tow Truck Full", ItemType.Vehicles) { ModelName = "towtruck",OverrideMakeName = "Vapid", OverrideClassName = "Utility", },
            new VehicleItem("Vapid Tow Truck Utility", ItemType.Vehicles) { ModelName = "towtruck2",OverrideMakeName = "Vapid", OverrideClassName = "Utility", },

            //Heli

            //DLC
            new VehicleItem("Buckingham Conada", ItemType.Vehicles) { ModelName = "conada", RequiresDLC = true, Description = "'What is a utility helicopter?' we hear you ask. Well, a utility helicopter is like a utility belt, but it weighs over 1.6 tons and was responsible for 131 highly spectacular fatalities last year in Los Santos alone. We'd say you can't buy that kind of convenience – but as of now, you can."},
            new VehicleItem("Sparrow", ItemType.Vehicles) { OverrideMakeName = "Western",ModelName = "seasparrow2", RequiresDLC = true, },
            new VehicleItem("Sea Sparrow", ItemType.Vehicles) { OverrideMakeName = "Western",ModelName = "seasparrow3", RequiresDLC = true, },
            new VehicleItem("Nagasaki Havok", true, ItemType.Vehicles) { ModelName = "havok", Description = "In the world of tactical air support, bigger is better, right? Wrong." },
            new VehicleItem("Buckingham Volatus", true, ItemType.Vehicles) { ModelName = "volatus", Description = "The sleekest aerodynamics on the market, double swept blades, fantail rotor: when you're coming off a weekend's team-building ketamine workshop there's no more costly way to move at immense speeds in perfect comfort and near-total silence. This is the real business class." },
            new VehicleItem("Buckingham SuperVolito Carbon", true, ItemType.Vehicles) { ModelName = "supervolito2",Description = "Originally designed for alpine rescue and humanitarian relief, the SuperVolito has since found its true calling as this year's must-have trinket for the high-T exec with a fetish for military hardware." },
            new VehicleItem("Buckingham SuperVolito", true, ItemType.Vehicles) { ModelName = "supervolito",Description = "Originally designed for alpine rescue and humanitarian relief, the SuperVolito has since found its true calling as this year's must-have trinket for the high-T exec with a fetish for military hardware." },
            new VehicleItem("Buckingham Swift Deluxe", true, ItemType.Vehicles) { ModelName = "swift2",Description = "You're not just buying a three-ton, nitro-charged, solid gold helicopter. You're not just subsidizing third world mining corporations, hysterical dictatorships, thousands of child laborers and dozens of NGOs pretending to fight human rights abuse. You're making a statement. You're making a statement about you, and the kind of meaningless decadence the world barely even notices anymore." },
            new VehicleItem("Buckingham Swift", true, ItemType.Vehicles) { ModelName = "swift",Description = "Special edition lightweight, twin-engine, four-seat multi-purpose helicopter." },
            new VehicleItem("Western Cargobob", false, ItemType.Vehicles) { OverrideMakeName = "Western", ModelName = "cargobob2", Description = "This ex-industry Cargobob has transported tons of chemical dispersants and dead sea life up and down the length of the Pacific coast, so you can be sure it'll handle whatever you throw at it." },
            new VehicleItem("HVY Skylift", false, ItemType.Vehicles) { OverrideMakeName = "HVY",ModelName = "skylift" },
            new VehicleItem("Buckingham Maverick", "Used by law enforcement for surveillance operations, you'll often see them hovering above inner city African American neighborhoods.", false, ItemType.Vehicles) { OverrideMakeName = "Buckingham", ModelName = "maverick" },
            new VehicleItem("Maibatsu Frogger", true, ItemType.Vehicles) { OverrideMakeName = "Maibatsu", ModelName = "frogger", Description = "Stylish, roomy, easy to handle with a cruise speed of 130 knots, this 4-seat single-engine light helicopter is popular with both private pilots and charter companies." },
            new VehicleItem("Nagasaki Buzzard", false, ItemType.Vehicles) { OverrideMakeName = "Nagasaki",ModelName = "buzzard2" },
   
            
            //Plane
            new VehicleItem("Buckingham Alpha-Z1", true, ItemType.Vehicles) { ModelName = "alphaz1" },
            new VehicleItem("Buckingham Howard NX-25", true, ItemType.Vehicles) { ModelName = "howard" },
            new VehicleItem("Buckingham Luxor", ItemType.Vehicles) { ModelName = "luxor" },
            new VehicleItem("Buckingham Luxor Deluxe", true, ItemType.Vehicles) { ModelName = "luxor2" },
            new VehicleItem("Buckingham Miljet", true, ItemType.Vehicles) { ModelName = "Miljet" },
            new VehicleItem("Buckingham Nimbus", true, ItemType.Vehicles) { ModelName = "nimbus" },
            new VehicleItem("Buckingham Pyro", true, ItemType.Vehicles) { ModelName = "pyro" },
            new VehicleItem("Buckingham Shamal", "At current gas prices, you can fly a Shamal coast to coast for just $50,000, without even a second thought for the Ozone layer.", ItemType.Vehicles) { ModelName = "Shamal" },
            new VehicleItem("Buckingham Vestra", true, ItemType.Vehicles) { ModelName = "vestra" },
            new VehicleItem("Mammoth Avenger", true, ItemType.Vehicles) { ModelName = "avenger" },
            new VehicleItem("Mammoth Avenger 2", true, ItemType.Vehicles) { ModelName = "avenger2" },
            new VehicleItem("Mammoth Dodo", ItemType.Vehicles) { ModelName = "dodo" },
            new VehicleItem("Mammoth Hydra", true, ItemType.Vehicles) { ModelName = "hydra" },
            new VehicleItem("Mammoth Mogul", true, ItemType.Vehicles) { ModelName = "mogul" },
            new VehicleItem("Mammoth Tula", true, ItemType.Vehicles) { ModelName = "tula" },
            new VehicleItem("Nagasaki Ultralight", true, ItemType.Vehicles) { ModelName = "microlight" },
            new VehicleItem("Western Besra", true, ItemType.Vehicles) { ModelName = "besra" },
            new VehicleItem("Western Rogue", true, ItemType.Vehicles) { ModelName = "rogue" },
            new VehicleItem("Western Seabreeze", true, ItemType.Vehicles) { ModelName = "seabreeze" },



            new VehicleItem("JoBuilt Mammatus", "Affordable, easy to fly, held together with rivets... The Mammatus is one of the best-selling light aircraft in history, and a whole lot of fun to land in high winds.", false, ItemType.Vehicles) { OverrideMakeName = "JoBuilt", ModelName = "mammatus" },
            new VehicleItem("JoBuilt Velum", "The Velum is a high-performance single-engine light aircraft manufactured specifically to fulfill the needs of executive travel because, let's face it, that's the only investment in aviation that happens these days.", false, ItemType.Vehicles) { OverrideMakeName = "JoBuilt",ModelName = "velum" },

            //Boat
            new VehicleItem("Dinka Marquis", ItemType.Vehicles) { ModelName = "marquis" },
            new VehicleItem("Lampadati Toro", true, ItemType.Vehicles) { ModelName = "toro" },
            new VehicleItem("Lampadati Toro", true, ItemType.Vehicles) { ModelName = "toro2" },
            new VehicleItem("Nagasaki Dinghy", ItemType.Vehicles) { ModelName = "Dinghy" },
            new VehicleItem("Nagasaki Dinghy 2", ItemType.Vehicles) { ModelName = "dinghy2" },
            new VehicleItem("Nagasaki Dinghy 3", true, ItemType.Vehicles) { ModelName = "dinghy3" },
            new VehicleItem("Nagasaki Dinghy 4", true, ItemType.Vehicles) { ModelName = "dinghy4" },
            new VehicleItem("Nagasaki Weaponized Dinghy", true, ItemType.Vehicles) { ModelName = "dinghy5" },
            new VehicleItem("Pegassi Speeder", true, ItemType.Vehicles) { ModelName = "speeder" },
            new VehicleItem("Pegassi Speeder", true, ItemType.Vehicles) { ModelName = "speeder2" },
            new VehicleItem("Shitzu Jetmax", ItemType.Vehicles) { ModelName = "jetmax" },
            new VehicleItem("Shitzu Longfin", true, ItemType.Vehicles) { ModelName = "longfin" },
            new VehicleItem("Shitzu Squalo", ItemType.Vehicles) { ModelName = "squalo" },
            new VehicleItem("Shitzu Suntrap", ItemType.Vehicles) { ModelName = "Suntrap" },
            new VehicleItem("Shitzu Tropic", ItemType.Vehicles) { ModelName = "tropic" },
            new VehicleItem("Shitzu Tropic", true, ItemType.Vehicles) { ModelName = "tropic2" },
            new VehicleItem("Speedophile Seashark", ItemType.Vehicles) { ModelName = "seashark" },
            new VehicleItem("Speedophile Seashark 2", ItemType.Vehicles) { ModelName = "seashark2" },
            new VehicleItem("Speedophile Seashark 3", true, ItemType.Vehicles) { ModelName = "seashark3" },

            //New DLC
            new VehicleItem("Obey 10F", ItemType.Vehicles) { ModelName = "tenf", RequiresDLC = true, },
            new VehicleItem("Obey 10F Widebody", ItemType.Vehicles) { ModelName = "tenf2", RequiresDLC = true, },
            new VehicleItem("Annis 300R", ItemType.Vehicles) { ModelName = "r300", RequiresDLC = true, },
            new VehicleItem("Pfister Astron", ItemType.Vehicles) { ModelName = "astron", RequiresDLC = true, },
            new VehicleItem("Gallivanter Baller ST", ItemType.Vehicles) { ModelName = "baller7", RequiresDLC = true, },
            new VehicleItem("Karin Boor", ItemType.Vehicles) { ModelName = "boor", RequiresDLC = true, },
            new VehicleItem("MTL Brickade 6x6", ItemType.Vehicles) { ModelName = "brickade2", RequiresDLC = true, },
            new VehicleItem("Grotti Brioso 300 Widebody", ItemType.Vehicles) { ModelName = "brioso3", RequiresDLC = true, },
            new VehicleItem("Classique Broadway", ItemType.Vehicles) { ModelName = "broadway", RequiresDLC = true, },
            new VehicleItem("Bravado Buffalo STX", ItemType.Vehicles) { ModelName = "buffalo4", RequiresDLC = true, },
            new VehicleItem("Dewbauchee Champion", ItemType.Vehicles) { ModelName = "champion", RequiresDLC = true, },
            new VehicleItem("Lampadati Cinquemila", ItemType.Vehicles) { ModelName = "cinquemila", RequiresDLC = true, },
            new VehicleItem("Pfister Comet S2", ItemType.Vehicles) { ModelName = "comet7", RequiresDLC = true, },
            
            new VehicleItem("Lampadati Corsita", ItemType.Vehicles) { ModelName = "corsita", RequiresDLC = true, },
            new VehicleItem("Enus Deity", ItemType.Vehicles) { ModelName = "deity", RequiresDLC = true, },
            new VehicleItem("Declasse Dragur", ItemType.Vehicles) { ModelName = "draugur", RequiresDLC = true, },
            new VehicleItem("Overflod Entity MT", ItemType.Vehicles) { ModelName = "entity3", RequiresDLC = true, },
            new VehicleItem("Willard Eudora", ItemType.Vehicles) { ModelName = "eudora", RequiresDLC = true, },
            new VehicleItem("Declasse Granger 3600LX", ItemType.Vehicles) { ModelName = "granger2", RequiresDLC = true, },
            new VehicleItem("Bravado Greenwood", ItemType.Vehicles) { ModelName = "greenwood", RequiresDLC = true, },
            new VehicleItem("Karin Hotrin Everon", ItemType.Vehicles) { ModelName = "everon2", RequiresDLC = true, },
            new VehicleItem("Obey I-Wagen", ItemType.Vehicles) { ModelName = "iwagen", RequiresDLC = true, },
            new VehicleItem("Pegassi Ignus", ItemType.Vehicles) { ModelName = "ignus", RequiresDLC = true, },
            new VehicleItem("Weeny Issi Rally", ItemType.Vehicles) { ModelName = "issi8", RequiresDLC = true, },
            new VehicleItem("Zirconium Journey II", ItemType.Vehicles) { ModelName = "journey2", RequiresDLC = true, },
            new VehicleItem("Enus Jubilee", ItemType.Vehicles) { ModelName = "jubilee", RequiresDLC = true, },
            new VehicleItem("Dinka Kanjo SJ", ItemType.Vehicles) { ModelName = "kanjosj", RequiresDLC = true, },
            new VehicleItem("Benefactor LM87", ItemType.Vehicles) { ModelName = "lm87", RequiresDLC = true, },
            new VehicleItem("Maibatsu Mule", ItemType.Vehicles) { ModelName = "mule5", RequiresDLC = true, },
            new VehicleItem("Maibatsu Manchez Scout C", ItemType.Vehicles) { ModelName = "manchez3", RequiresDLC = true, },
            new VehicleItem("Obey Omnis e-GT", ItemType.Vehicles) { ModelName = "omnisegt", RequiresDLC = true, },
            new VehicleItem("Toundra Panthere", ItemType.Vehicles) { ModelName = "panthere", RequiresDLC = true, },
            new VehicleItem("Mammoth Patriot Mil-Spec", ItemType.Vehicles) { ModelName = "patriot3", RequiresDLC = true, },
            new VehicleItem("Dinka Postlude", ItemType.Vehicles) { ModelName = "postlude", RequiresDLC = true, },
            new VehicleItem("Western Powersurge", ItemType.Vehicles) { ModelName = "powersurge", RequiresDLC = true, },
            new VehicleItem("Western Reever", ItemType.Vehicles) { ModelName = "reever", RequiresDLC = true, },
            new VehicleItem("Ubermacht Rhinehart", ItemType.Vehicles) { ModelName = "rhinehart", RequiresDLC = true, },
            new VehicleItem("Imponte Ruiner ZZ-8", ItemType.Vehicles) { ModelName = "ruiner4", RequiresDLC = true, },
            new VehicleItem("Ubermacht Sentinel Classic Widebody", ItemType.Vehicles) { ModelName = "sentinel4", RequiresDLC = true, },
            new VehicleItem("Nagasaki Shinobi", ItemType.Vehicles) { ModelName = "shinobi", RequiresDLC = true, },
            new VehicleItem("Benefactor SM722", ItemType.Vehicles) { ModelName = "sm722", RequiresDLC = true, },
            new VehicleItem("BF Surfer Custom", ItemType.Vehicles) { ModelName = "surfer3", RequiresDLC = true, },
            new VehicleItem("Declasse Tahoma Custom", ItemType.Vehicles) { ModelName = "tahoma", RequiresDLC = true, },
            new VehicleItem("Pegassi Torero XO", ItemType.Vehicles) { ModelName = "torero2", RequiresDLC = true, },
            new VehicleItem("Declasse Tulip M-100", ItemType.Vehicles) { ModelName = "tulip2", RequiresDLC = true, },
            new VehicleItem("Declasse Vigero ZX", ItemType.Vehicles) { ModelName = "vigero2", RequiresDLC = true, },
            new VehicleItem("Ocelot Virtue", ItemType.Vehicles) { ModelName = "virtue", RequiresDLC = true, },
            new VehicleItem("BF Weevil Custom", ItemType.Vehicles) { ModelName = "weevil2", RequiresDLC = true, },
            new VehicleItem("Vapid Youga Custom", ItemType.Vehicles) { ModelName = "youga4", RequiresDLC = true, },
            new VehicleItem("Overflod Zeno", ItemType.Vehicles) { ModelName = "zeno", RequiresDLC = true, },

            //NEW DLC 2
            new VehicleItem("Albany Brigham", ItemType.Vehicles) { ModelName = "brigham", RequiresDLC = true, },
            new VehicleItem("Bravado Hotring Hellfire", ItemType.Vehicles) { ModelName = "gauntlet6", RequiresDLC = true, },
            new VehicleItem("Mammoth F-160 Raiju", ItemType.Vehicles) { ModelName = "raiju", RequiresDLC = true, },
            new VehicleItem("Grotti Itali GTO Stinger TT", ItemType.Vehicles) { ModelName = "stingertt", RequiresDLC = true, },
            new VehicleItem("Vapid Clique Wagon", ItemType.Vehicles) { ModelName = "clique2", RequiresDLC = true, },
            new VehicleItem("Maibatsu MonstroCiti", ItemType.Vehicles) { ModelName = "monstrociti", RequiresDLC = true, },
            new VehicleItem("Vapid Ratel", ItemType.Vehicles) { ModelName = "ratel", RequiresDLC = true, },
            new VehicleItem("Declasse Walton L35", ItemType.Vehicles) { ModelName = "l35", RequiresDLC = true, },
            new VehicleItem("Mammoth Streamer216", ItemType.Vehicles) { ModelName = "streamer216", RequiresDLC = true, },
            new VehicleItem("Bravado Buffalo EVX", ItemType.Vehicles) { ModelName = "buffalo5", RequiresDLC = true, },
            new VehicleItem("Penaud La Coureuse", ItemType.Vehicles) { ModelName = "coureur", RequiresDLC = true, },
            new VehicleItem("Coil Inductor", ItemType.Vehicles) { ModelName = "inductor", RequiresDLC = true, },
            new VehicleItem("Coil Junk Energy Inductor", ItemType.Vehicles) { ModelName = "inductor2", RequiresDLC = true, },
            new VehicleItem("Mammoth Thruster", true, ItemType.Vehicles) { ModelName = "thruster" },

            //CHOP SHOP DLC
            new VehicleItem("Grotti Turismo Omaggio","In a world where it often feels like the best you can hope for is an all-electric fap to some hybrid porn, Grotti's stunning farewell tribute to the mighty V8 is the booty call of your hot, heavy dreams.", ItemType.Vehicles) { ModelName = "turismo3", RequiresDLC = true, },
            new VehicleItem("Karin Asterope GZ","The sleeper of your dreams, and a car so unremarkable they won't see you coming till they're choking on your exhaust.", ItemType.Vehicles) { ModelName = "asterope2", RequiresDLC = true, },
            new VehicleItem("Declasse Vigero ZX Convertible","Want to wield a hunk of muscle that would make the buffest gym bros and lamest ego lifters cry? Forget Bull Shark Testosterone. If you're looking to bulk up, try the Vigero ZX Convertible, and enjoy fast gains that won't leave you with nips the size of dinner plates.", ItemType.Vehicles) { ModelName = "vigero3", RequiresDLC = true, },
            new VehicleItem("Declasse Impaler SZ","The Impaler is the simple solution for serious drag racers who want to go from Point A to Point B as fast as possible before splattering themselves evenly over Point C. If it's not a straight line, don't even think about it. Otherwise, see you at the finish.", ItemType.Vehicles) { ModelName = "impaler5", RequiresDLC = true, },
            new VehicleItem("Declasse Impaler LX","If you drove the Impaler LX in the 80s, you were either a cop or you were running from a cop. Or both. That's the kind of universal appeal that's so hard to find in today's market. So, if you find yourself yearning for the analog thrills of your best years, Declasse is here to give you a sweet hit of muscle memory that doesn't involve dropping your pants. Unless you want it to.", ItemType.Vehicles) { ModelName = "impaler6", RequiresDLC = true, },
            new VehicleItem("Fathom FR36","Can you take 4360lbs of sports car around a mountaintop hairpin bend as smoothly as a 30-year-old shut-in caresses his life-size Princess Robot Bubblegum body pillow? It's an enthusiastic YES YOU CAN from Japan with the oh so driftable Fathom FR36 coupe.", ItemType.Vehicles) { ModelName = "fr36", RequiresDLC = true, },
            new VehicleItem("Karin Vivanite","Certified MILF mobile of Middle America, the Karin Vivanite is the first word in practical family minivans and the last word in unexpected drag racing. So next time you can't decide between taking the kids to soccer practice and melting the asphalt with a class 1 misdemeanor, remember you can always smash two birds to a pulp with one value-conscious stone.", ItemType.Vehicles) { ModelName = "vivanite", RequiresDLC = true, },
            new VehicleItem("Vapid Aleutian","With the cartels and the FIB both purchasing in bulk, the Vapid Aleutian has been the poster child of the war on drugs ever since it rolled off the production line. That's the kind of reliable consumer base that'll put the American car industry back on the map, and with optional Armor Plating, Slick Mines, a Remote Control Unit and Missile Lock-On Jammer, this is a revival you're going to want to be part of.", ItemType.Vehicles) { ModelName = "aleutian", RequiresDLC = true, },
            new VehicleItem("Vapid Dominator GT","From the makers of the Dominator cult classics comes an all-American, pant-shitting, drop-top 4D experience: The Dominator GT. Why be stimulated when you can be overstimulated, as you drag your sensory cortex over mile after mile of burning asphalt? Taste every victory. Feel every collision. Pay every insurance excess. Domination has a new name.", ItemType.Vehicles) { ModelName = "dominator9", RequiresDLC = true, },
            new VehicleItem("Canis Terminus","Meet the only heavyweight 4x4 on the market that didn't miss its anabolic window. If they drug tested cars, this thing's pee would melt the cup. Lucky for you, armor plating and slick mines aren't technically considered performance enhancing drugs, so have at 'em, champ.", ItemType.Vehicles) { ModelName = "terminus", RequiresDLC = true, },
            new VehicleItem("Gallivanter Baller ST-D","Got the itch for a supercharged SUV with serious go-power? Then you might have a case of the Gallivanter Baller ST-D.", ItemType.Vehicles) { ModelName = "baller8", RequiresDLC = true, },
            new VehicleItem("Albany Cavalcade XL","Wasn't the Cavalcade already XL, you ask? Couldn't you already fit more than enough strippers in the back? Do you really need to straddle two disabled parking spots? Well think about it this way. Less isn't more. More isn't more. Even more is more. That's the Albany way.", ItemType.Vehicles) { ModelName = "cavalcade3", RequiresDLC = true, },
            new VehicleItem("Bravado Dorado","The Bison's older, weirder cousin that nobody wants to talk about, the Dorado got a bad rep with the wrong crowd in its formative years. Sure, it's had more meth stuffed in its crevices than a newly recruited member of the Lost MC, and it has the fuel efficiency of a bullet-riddled Youga. For the right demographic, those are some of its best features.", ItemType.Vehicles) { ModelName = "dorado", RequiresDLC = true, },

            new VehicleItem("Bravado Gauntlet LE Hellfire", ItemType.Vehicles) { ModelName = "polgauntlet", RequiresDLC = true, },
            new VehicleItem("Vapid Stanier LE Cruiser", ItemType.Vehicles) { ModelName = "police5", RequiresDLC = true, },

            //Bottom Dollar DLC
            new VehicleItem("Overflod Pipistrello", ItemType.Vehicles) { ModelName = "pipistrello", RequiresDLC = true, },
            new VehicleItem("Invetero Coquette D1", ItemType.Vehicles) { ModelName = "coquette5", RequiresDLC = true, },     
            new VehicleItem("Declasse Yosemite 1500", ItemType.Vehicles) { ModelName = "yosemite1500", RequiresDLC = true, },
            new VehicleItem("Pegassi Pizza Boy", ItemType.Vehicles) { ModelName = "pizzaboy", RequiresDLC = true, },
            new VehicleItem("Bollokan Envisage", ItemType.Vehicles) { ModelName = "envisage", RequiresDLC = true, },
            new VehicleItem("Benefactor Vorschlaghammer", ItemType.Vehicles) { ModelName = "vorschlaghammer", RequiresDLC = true, },    
            new VehicleItem("Canis Castigator", ItemType.Vehicles) { ModelName = "castigator", RequiresDLC = true, },
            new VehicleItem("Ubermacht Niobe", ItemType.Vehicles) { ModelName = "niobe", RequiresDLC = true, },         
            new VehicleItem("Enus Paragon S", ItemType.Vehicles) { ModelName = "paragon3", RequiresDLC = true, },
            new VehicleItem("Annis Euros X32", ItemType.Vehicles) { ModelName = "eurosX32", RequiresDLC = true, },
            new VehicleItem("Vapid Dominator FX", ItemType.Vehicles) { ModelName = "dominator10", RequiresDLC = true, },

            new VehicleItem("Declasse Burrito (Bail Enforcement)", ItemType.Vehicles) { ModelName = "policet3", RequiresDLC = true, },
            new VehicleItem("Vapid Dominator FX Interceptor", ItemType.Vehicles) { ModelName = "poldominator10", RequiresDLC = true, },
            new VehicleItem("Bravado Dorado Cruiser", ItemType.Vehicles) { ModelName = "poldorado", RequiresDLC = true, },
            new VehicleItem("Declasse Impaler LX Cruiser", ItemType.Vehicles) { ModelName = "polimpaler6", RequiresDLC = true, },
            new VehicleItem("Declasse Impaler SZ Cruiser", ItemType.Vehicles) { ModelName = "polimpaler5", RequiresDLC = true, },
            new VehicleItem("Bravado Greenwood Cruiser", ItemType.Vehicles) { ModelName = "polgreenwood", RequiresDLC = true, },

            new VehicleItem("Ubermacht Drift Sentinel Classic Widebody", ItemType.Vehicles) { ModelName = "driftsentinel", RequiresDLC = true, },
            new VehicleItem("Benefactor Drift Vorschlaghammer", ItemType.Vehicles) { ModelName = "driftvorschlag", RequiresDLC = true, },
            new VehicleItem("Vulcar Drift Nebula", ItemType.Vehicles) { ModelName = "driftnebula", RequiresDLC = true, },
            new VehicleItem("Ubermacht Drift Cypher", ItemType.Vehicles) { ModelName = "driftcypher", RequiresDLC = true, },

            //Agents of Sabotage
            new VehicleItem("Bravado Banshee GTS", ItemType.Vehicles) { ModelName = "banshee3", RequiresDLC = true, },
            new VehicleItem("Dinka Chavos V6", ItemType.Vehicles) { ModelName = "chavosv6", RequiresDLC = true, },
            new VehicleItem("Dinka Jester RR Widebody", ItemType.Vehicles) { ModelName = "jester5", RequiresDLC = true, },
            new VehicleItem("Invetero Coquette D5", ItemType.Vehicles) { ModelName = "coquette6", RequiresDLC = true, },
            new VehicleItem("Vapid Firebolt ASP", ItemType.Vehicles) { ModelName = "firebolt", RequiresDLC = true, },
            new VehicleItem("Vapid Uranus LozSpeed", ItemType.Vehicles) { ModelName = "uranus", RequiresDLC = true, },


            //new VehicleItem("Buckingham DH-7 Iron Mule", ItemType.Vehicles) { ModelName = "driftcypher", RequiresDLC = true, },
            //new VehicleItem("Western Company Duster 300-H", ItemType.Vehicles) { ModelName = "driftcypher", RequiresDLC = true, },       
            //new VehicleItem("Eberhard Titan 250 D", ItemType.Vehicles) { ModelName = "driftcypher", RequiresDLC = true, },

            //Drift
            new VehicleItem("Declasse Drift Tampa", ItemType.Vehicles) { ModelName = "drifttampa", RequiresDLC = true, },
            new VehicleItem("Declasse Drift Yosemite", ItemType.Vehicles) { ModelName = "driftyosemite", RequiresDLC = true, },       
            new VehicleItem("Annis Drift Euros", ItemType.Vehicles) { ModelName = "drifteuros", RequiresDLC = true, },
            new VehicleItem("Fathom Drift FR36", ItemType.Vehicles) { ModelName = "driftfr36", RequiresDLC = true, },
            new VehicleItem("Karin Drift Futo", ItemType.Vehicles) { ModelName = "driftfuto", RequiresDLC = true, },
            new VehicleItem("Dinka Drift Jester", ItemType.Vehicles) { ModelName = "driftjester", RequiresDLC = true, },
            new VehicleItem("Annis Drift Remus", ItemType.Vehicles) { ModelName = "driftremus", RequiresDLC = true, },
            new VehicleItem("Annis Drift ZR350", ItemType.Vehicles) { ModelName = "driftzr350", RequiresDLC = true, },

            //Bikes
            new VehicleItem("BMX", ItemType.Vehicles) { Description = "The classic bike for tooling the neighborhood or doing a double peg grind. Reinforced steel, super comfortable seat that wont cut off the circulation to your scrotum too bad. (i.e. for wimps!) A great deal for the cash-strapped.", ModelName = "bmx" },
            new VehicleItem("Cruiser", ItemType.Vehicles) { Description = "This beach cruiser is top of the line relaxation. Made in America, which explains why you're paying 6 times the price of foreign made beach cruisers.", ModelName = "cruiser" },
            new VehicleItem("Fixter", ItemType.Vehicles) { Description = "Gears are for chumps. Fixies can't ever break right?", ModelName = "fixter" },
            new VehicleItem("Scorcher", ItemType.Vehicles) { Description = "San Andreas is full of some epic places to take this off-road beast. Comes with a mud flap so your back doesn't look like you've had a train run on you by lumberjacks. High torque cranks sound really impressive if you know what it means, and also ensure it's nimble on a trail but also stable high speed, when you are on the cusp of losing control and flying over the handlebars.", ModelName = "scorcher" },
            new VehicleItem("Whippet Race Bike", ItemType.Vehicles) { Description = "High strength carbon fiber frame, super-stable fork, alloy rims, this road bike will let people know you are serious about a sport made obsolete by combustion engines years ago. Get serious about working out by spending a lot of money on gear right now.", ModelName = "tribike" },
            new VehicleItem("Endurex Race Bike", ItemType.Vehicles) { Description = "The Endurex is a step up from the original Whippet in that it's red which will perfectly match the bloody knees you will get from trying to maneuver a bike with wheels a half inch wide. You deserve this bike. You feel thinner already.", ModelName = "tribike2" },
            new VehicleItem("Tri-Cycles Race Bike", ItemType.Vehicles) { Description = "Everyone at the triathlon will know you are serious about a sport where participants push themselves to the point of losing control of their bowels when you ride up on this beauty. Adjustable seat and padded grips make for an excellent afternoon in the park, hunched over on a bike you're terrified will get stolen if you stop for lunch. Step up. Pay up. Front up.", ModelName = "tribike3" },

            new VehicleItem("Coil Inductor", ItemType.Vehicles) { RequiresDLC = true, Description = "You might have thought that a mountain bike was already eco-friendly. Not true. The average Los Santos resident's sweat contains enough restricted substances to significantly damage any ecosystem they pedal through. The solution? The nimble torque-pumped Inductor, the electric mountain bike of your freedom-filled dreams. It only takes an open heart, a courageous spirit, and a strong life insurance claim to afford the lithium-ion battery. Completely worth it.", ModelName = "inductor" },
            new VehicleItem("Junk Energy Coil Inductor", ItemType.Vehicles) { RequiresDLC = true, Description = "You might have thought that a mountain bike was already eco-friendly. Not true. The average Los Santos resident's sweat contains enough restricted substances to significantly damage any ecosystem they pedal through. The solution? The nimble torque-pumped Inductor, the electric mountain bike of your freedom-filled dreams. It only takes an open heart, a courageous spirit, and a strong life insurance claim to afford the lithium-ion battery. Completely worth it. Now with JUNK ENERGY MARKINGS!", ModelName = "inductor2" },

            ////IV PACK
            //new VehicleItem("Dundreary Admiral", ItemType.Vehicles) { ModelName = "admiral" },
            //new VehicleItem("Western Angel", ItemType.Vehicles) { ModelName = "angel" },
            //new VehicleItem("HVY APC Tank", ItemType.Vehicles) { ModelName = "apc2" },
            //new VehicleItem("Grotti Blade", ItemType.Vehicles) { ModelName = "blade2" },
            //new VehicleItem("Vapid Bobcat", ItemType.Vehicles) { ModelName = "bobcat" },
            //new VehicleItem("Canis Bodhi (IV)", ItemType.Vehicles) { ModelName = "bodhi" },
            //new VehicleItem("Brute Boxville 6", ItemType.Vehicles) { ModelName = "boxville6" },
            //new VehicleItem("HVY Brickade", ItemType.Vehicles) { ModelName = "brickade3" },
            //new VehicleItem("Albany Buccaneer (IV)", ItemType.Vehicles) { ModelName = "buccaneer3" },
            //new VehicleItem("Brute Bus (IV)", ItemType.Vehicles) { ModelName = "bus2" },
            //new VehicleItem("Schyster Cabby", ItemType.Vehicles) { ModelName = "cabby" },
            //new VehicleItem("Dinka Chavos", ItemType.Vehicles) { ModelName = "chavos" },
            //new VehicleItem("Dinka Chavos (2)", ItemType.Vehicles) { ModelName = "chavos2" },
            //new VehicleItem("Grotti Cheetah (IV)", ItemType.Vehicles) { ModelName = "cheetah3" },
            //new VehicleItem("Vapid Contender", ItemType.Vehicles) { ModelName = "contender2" },
            //new VehicleItem("Vapid Coquette (IV)", ItemType.Vehicles) { ModelName = "coquette5" },
            //new VehicleItem("Imponte DF8-90", ItemType.Vehicles) { ModelName = "df8" },
            //new VehicleItem("Western Diabolus", ItemType.Vehicles) { ModelName = "diabolus" },
            //new VehicleItem("Dinka Double T (IV)", ItemType.Vehicles) { ModelName = "double2" },
            //new VehicleItem("Albany Emperor (IV)", ItemType.Vehicles) { ModelName = "emperor5" },
            //new VehicleItem("Albany Esperanto (IV)", ItemType.Vehicles) { ModelName = "esperanto" },
            //new VehicleItem("Benefactor Feltzer (IV)", ItemType.Vehicles) { ModelName = "feltzer" },
            //new VehicleItem("Bravado Feroci", ItemType.Vehicles) { ModelName = "feroci" },
            //new VehicleItem("FlyUS Feroci", ItemType.Vehicles) { ModelName = "feroci2" },
            //new VehicleItem("MTL Flatbed", ItemType.Vehicles) { ModelName = "flatbed2" },
            //new VehicleItem("Floater", ItemType.Vehicles) { ModelName = "floater" },
            //new VehicleItem("Vapid Fortune", ItemType.Vehicles) { ModelName = "fortune" },
            //new VehicleItem("LCC Freeway", ItemType.Vehicles) { ModelName = "freeway" },
            //new VehicleItem("Karin Futo (IV)", ItemType.Vehicles) { ModelName = "futo3" },
            //new VehicleItem("Albany Cavalcade FXT", ItemType.Vehicles) { ModelName = "fxt" },
            //new VehicleItem("ghawar", ItemType.Vehicles) { ModelName = "ghawar" },
            //new VehicleItem("Shitzu Hakuchou (IV)", ItemType.Vehicles) { ModelName = "hakuchou3" },
            //new VehicleItem("Dinka Hakumai", ItemType.Vehicles) { ModelName = "hakumai" },
            //new VehicleItem("Western Hellfury (IV)", ItemType.Vehicles) { ModelName = "hellfury" },
            //new VehicleItem("Vapid Huntley Sport (IV)", ItemType.Vehicles) { ModelName = "huntley2" },
            //new VehicleItem("interceptor", ItemType.Vehicles) { ModelName = "interceptor" },
            //new VehicleItem("Emperor Lokus", ItemType.Vehicles) { ModelName = "lokus" },
            //new VehicleItem("Liberty Chop Shop Lycan (IV)", ItemType.Vehicles) { ModelName = "lycan" },
            //new VehicleItem("Liberty Chop Shop Lycan 2 (IV)", ItemType.Vehicles) { ModelName = "lycan2" },
            //new VehicleItem("Willard Marbelle", ItemType.Vehicles) { ModelName = "marbelle" },
            //new VehicleItem("Declasse Merit", ItemType.Vehicles) { ModelName = "merit" },
            //new VehicleItem("mrtasty", ItemType.Vehicles) { ModelName = "mrtasty" },
            //new VehicleItem("Liberty Chop Shop Nightblade (IV)", ItemType.Vehicles) { ModelName = "nightblade2" },
            //new VehicleItem("noose", ItemType.Vehicles) { ModelName = "noose" },
            //new VehicleItem("Shitzu NRG-900F", ItemType.Vehicles) { ModelName = "nrg900" },
            //new VehicleItem("nstockade", ItemType.Vehicles) { ModelName = "nstockade" },
            //new VehicleItem("MTL Packer (IV)", ItemType.Vehicles) { ModelName = "packer2" },
            //new VehicleItem("Dinka Perennial", ItemType.Vehicles) { ModelName = "perennial" },
            //new VehicleItem("FlyUS Perennial", ItemType.Vehicles) { ModelName = "perennial2" },
            //new VehicleItem("Imponte Phoenix (IV)", ItemType.Vehicles) { ModelName = "phoenix2" },
            //new VehicleItem("Annis Pinnacle", ItemType.Vehicles) { ModelName = "pinnacle" },
            //new VehicleItem("Schyster PMP 600", ItemType.Vehicles) { ModelName = "pmp600" },
            //new VehicleItem("police6", ItemType.Vehicles) { ModelName = "police6" },
            //new VehicleItem("police7", ItemType.Vehicles) { ModelName = "police7" },
            //new VehicleItem("police8", ItemType.Vehicles) { ModelName = "police8" },
            //new VehicleItem("polpatriot", ItemType.Vehicles) { ModelName = "polpatriot" },
            //new VehicleItem("Declasse Premier (IV)", ItemType.Vehicles) { ModelName = "premier2" },
            //new VehicleItem("Albany Presidente (IV)", ItemType.Vehicles) { ModelName = "pres" },
            //new VehicleItem("Albany Presidente 2 (IV)", ItemType.Vehicles) { ModelName = "pres2" },
            //new VehicleItem("pstockade", ItemType.Vehicles) { ModelName = "pstockade" },
            //new VehicleItem("Declasse Rancher (IV)", ItemType.Vehicles) { ModelName = "rancher" },
            //new VehicleItem("Ubermacht Rebla", ItemType.Vehicles) { ModelName = "reblaiv" },
            //new VehicleItem("reefer", ItemType.Vehicles) { ModelName = "reefer" },
            //new VehicleItem("Dundreary Regina 2 (IV)", ItemType.Vehicles) { ModelName = "regina2" },
            //new VehicleItem("Dundreary Regina 3 (IV)", ItemType.Vehicles) { ModelName = "regina3" },
            //new VehicleItem("Western Revenant", ItemType.Vehicles) { ModelName = "revenant" },
            //new VehicleItem("rom", ItemType.Vehicles) { ModelName = "rom" },
            //new VehicleItem("Declasse Sabre (IV)", ItemType.Vehicles) { ModelName = "sabre" },
            //new VehicleItem("Declasse Sabre 2 (IV)", ItemType.Vehicles) { ModelName = "sabre2" },
            //new VehicleItem("Benefactor Schafter (IV)", ItemType.Vehicles) { ModelName = "schafter" },
            //new VehicleItem("Benefactor Schafter GTR (IV)", ItemType.Vehicles) { ModelName = "schaftergtr" },
            //new VehicleItem("Ubermacht Sentinel (IV)", ItemType.Vehicles) { ModelName = "sentinel5" },
            //new VehicleItem("Grotti Smuggler", ItemType.Vehicles) { ModelName = "smuggler" },
            //new VehicleItem("Willard Solair", ItemType.Vehicles) { ModelName = "solair" },
            //new VehicleItem("Western Motorccycle Company Sovereign (IV)", ItemType.Vehicles) { ModelName = "sovereign2" },
            //new VehicleItem("Vapid Stanier (IV)", ItemType.Vehicles) { ModelName = "stanier2" },
            //new VehicleItem("Vapid Steed", ItemType.Vehicles) { ModelName = "steed" },
            //new VehicleItem("Zirconium Stratum (IV)", ItemType.Vehicles) { ModelName = "stratum2" },
            //new VehicleItem("stretch2", ItemType.Vehicles) { ModelName = "stretch2" },
            //new VehicleItem("stretch3", ItemType.Vehicles) { ModelName = "stretch3" },
            //new VehicleItem("Karin Sultan RS (IV)", ItemType.Vehicles) { ModelName = "sultans" },
            //new VehicleItem("Enus Super Diamond (IV)", ItemType.Vehicles) { ModelName = "superd2" },
            //new VehicleItem("Dewbauchee SuperGT (IV)", ItemType.Vehicles) { ModelName = "supergt" },
            //new VehicleItem("taxi2", ItemType.Vehicles) { ModelName = "taxi2" },
            //new VehicleItem("taxi3", ItemType.Vehicles) { ModelName = "taxi3" },
            //new VehicleItem("tourmav", ItemType.Vehicles) { ModelName = "tourmav" },
            //new VehicleItem("Grotti Turismo (IV)", ItemType.Vehicles) { ModelName = "turismo" },
            //new VehicleItem("typhoon", ItemType.Vehicles) { ModelName = "typhoon" },
            //new VehicleItem("Vapid Uranus", ItemType.Vehicles) { ModelName = "uranus" },
            //new VehicleItem("Declasse Vigero (IV)", ItemType.Vehicles) { ModelName = "vigero3" },
            //new VehicleItem("Maibatsu Vincent (IV)", ItemType.Vehicles) { ModelName = "vincent" },
            //new VehicleItem("violator", ItemType.Vehicles) { ModelName = "violator" },
            //new VehicleItem("Declasse Voodoo (IV)", ItemType.Vehicles) { ModelName = "voodoo3" },
            //new VehicleItem("Western Wayfarer (IV)", ItemType.Vehicles) { ModelName = "wayfarer" },
            //new VehicleItem("Willard Willard (IV)", ItemType.Vehicles) { ModelName = "willard" },
            //new VehicleItem("wolfsbane2", ItemType.Vehicles) { ModelName = "wolfsbane2" },
            //new VehicleItem("yankee2", ItemType.Vehicles) { ModelName = "yankee2" },
            //new VehicleItem("yankee", ItemType.Vehicles) { ModelName = "yankee" },
        });
    }
    private void DefaultConfig_Weapons()
    {
        PossibleItems.WeaponItems.AddRange(new List<WeaponItem>
        {
            new WeaponItem("Flint Hammer","A robust, multi-purpose hammer with wooden handle and curved claw, this old classic still nails the competition.", false, ItemType.Weapons) { ModelName = "weapon_hammer", FindPercentage = 5 },
            new WeaponItem("Flint Hatchet","Add a good old-fashioned hatchet to your armory, and always have a back up for when ammo is hard to come by.", false, ItemType.Weapons) { ModelName = "weapon_hatchet"},
            new WeaponItem("Flint Heavy Duty Pipe Wrench","Perennial favourite of apocalyptic survivalists and violent fathers the world over, apparently it also doubles as some kind of tool.", false, ItemType.Weapons) { ModelName = "weapon_wrench", FindPercentage = 5 },  
            new WeaponItem("Flint Crowbar","Heavy-duty crowbar forged from high quality, tempered steel for that extra leverage you need to get the job done.", false, ItemType.Weapons) { ModelName = "weapon_crowbar", FindPercentage = 5},
            
            new WeaponItem("Vom Feuer Machete","America's West African arms trade isn't just about giving. Rediscover the simple life with this rusty cleaver.", false, ItemType.Weapons) { ModelName = "weapon_machete", FindPercentage = 5},
            
            new WeaponItem("G.E.S. Baseball Bat","Aluminum baseball bat with leather grip. Lightweight yet powerful for all you big hitters out there.", false, ItemType.Weapons) { ModelName = "weapon_bat", FindPercentage = 5},
            new WeaponItem("ProLaps Five Iron Golf Club","Standard length, mid iron golf club with rubber grip for a lethal short game.", false, ItemType.Weapons) { ModelName = "weapon_golfclub", FindPercentage = 5},

            //Melee
            new WeaponItem("Brass Knuckles","Perfect for knocking out gold teeth, or as a gift to the trophy partner who has everything.", false, ItemType.Weapons) { ModelName = "weapon_knuckle",PoliceFindDuringPlayerSearchPercentage = 15 , FindPercentage = 5},
            new WeaponItem("Combat Knife","This carbon steel 7 inch bladed knife is dual edged with a serrated spine to provide improved stabbing and thrusting capabilities.", false, ItemType.Weapons) { ModelName = "weapon_knife",PoliceFindDuringPlayerSearchPercentage = 25, FindPercentage = 5},
            new WeaponItem("Switchblade","From your pocket to hilt-deep in the other guy's ribs in under a second: folding knives will never go out of style.", false, ItemType.Weapons) { ModelName = "weapon_switchblade",PoliceFindDuringPlayerSearchPercentage = 15, FindPercentage = 5 },
            new WeaponItem("Nightstick","24 inch polycarbonate side-handled nightstick.", false, ItemType.Weapons) { ModelName = "weapon_nightstick", FindPercentage = 5 },
            new WeaponItem("Pool Cue","Ah, there's no sound as satisfying as the crack of a perfect break, especially when it's the other guy's spine.", false, ItemType.Weapons) { ModelName = "weapon_poolcue" },

                //Unused
            new WeaponItem("Broken Bottle","", false, ItemType.Weapons) { ModelName = "weapon_bottle" },
            new WeaponItem("Dagger","", false, ItemType.Weapons) { ModelName = "weapon_dagger" },
            new WeaponItem("Flashlight","", false, ItemType.Weapons) { ModelName = "weapon_flashlight" },
            new WeaponItem("Battleaxe","", false, ItemType.Weapons) { ModelName = "weapon_battleaxe" },
            new WeaponItem("Stone Hatchet","", false, ItemType.Weapons) { ModelName = "weapon_stone_hatchet" },
            new WeaponItem("Candy Cane","", false, ItemType.Weapons) { ModelName = "weapon_candycane" },



            //Pistola
            new WeaponItem("Hawk & Little PTF092F","Standard handgun. A 9mm combat pistol with a magazine capacity of 12 rounds that can be extended to 16.", false, ItemType.Weapons) { VanillaName = "Pistol",ModelName = "weapon_pistol",PoliceFindDuringPlayerSearchPercentage = 35, FindPercentage = 10 },
            new WeaponItem("Hawk & Little Thunder","Balance, simplicity, precision: nothing keeps the peace like an extended barrel in the other guy's mouth.", true, ItemType.Weapons) { VanillaName = "Combat Pistol Mk2",ModelName = "weapon_pistol_mk2",PoliceFindDuringPlayerSearchPercentage = 35, FindPercentage = 5},
            new WeaponItem("Hawk & Little Combat Pistol","A compact, lightweight semi-automatic pistol designed for law enforcement and personal defense use. 12-round magazine with option to extend to 16 rounds.", false, ItemType.Weapons) { VanillaName = "Combat Pistol",ModelName = "weapon_combatpistol",PoliceFindDuringPlayerSearchPercentage = 35, FindPercentage = 10},
            new WeaponItem("Hawk & Little Desert Slug","High-impact pistol that delivers immense power but with extremely strong recoil. Holds 9 rounds in magazine.", false, ItemType.Weapons) { VanillaName = "Pistol .50",ModelName = "weapon_pistol50",PoliceFindDuringPlayerSearchPercentage = 55, FindPercentage = 2},
            new WeaponItem("Vom Feuer P69","Not your grandma's ceramics. Although this pint-sized pistol is small enough to fit into her purse and won't set off a metal detector.", true, ItemType.Weapons) { VanillaName = "Ceramic Pistol",ModelName = "weapon_ceramicpistol",PoliceFindDuringPlayerSearchPercentage = 25, FindPercentage = 2 },
            new WeaponItem("Vom Feuer SCRAMP","High-penetration, fully-automatic pistol. Holds 18 rounds in magazine with option to extend to 36 rounds.", false, ItemType.Weapons) { VanillaName = "AP Pistol",ModelName = "weapon_appistol" ,PoliceFindDuringPlayerSearchPercentage = 35, FindPercentage = 1},
            new WeaponItem("Hawk & Little 1919","The heavyweight champion of the magazine fed, semi-automatic handgun world. Delivers accuracy and a serious forearm workout every time.", false, ItemType.Weapons) { VanillaName = "Heavy Pistol",ModelName = "weapon_heavypistol",PoliceFindDuringPlayerSearchPercentage = 35, FindPercentage = 5},
            new WeaponItem("Hawk & Little Raging Mare","A handgun with enough stopping power to drop a crazed rhino, and heavy enough to beat it to death if you're out of ammo.", true, ItemType.Weapons) { VanillaName = "Heavy Revolver",ModelName = "weapon_revolver",PoliceFindDuringPlayerSearchPercentage = 75, FindPercentage = 5},
            new WeaponItem("Hawk & Little Raging Mare Dx","If you can lift it, this is the closest you'll get to shooting someone with a freight train.", true, ItemType.Weapons) { VanillaName = "Heavy Revolver Mk2",ModelName = "weapon_revolver_mk2",PoliceFindDuringPlayerSearchPercentage = 75, FindPercentage = 5},
            new WeaponItem("Shrewsbury S7","Like condoms or hairspray, this fits in your pocket for a night on the town. The price of a bottle at a club, it's half as accurate as a champagne cork, and twice as deadly.", false, ItemType.Weapons) { VanillaName = "SNS Pistol",ModelName = "weapon_snspistol",PoliceFindDuringPlayerSearchPercentage = 25, FindPercentage = 10},
            new WeaponItem("Shrewsbury S7A","The ultimate purse-filler: if you want to make Saturday Night really special, this is your ticket.", true, ItemType.Weapons) { VanillaName = "SNS Pistol Mk2",ModelName = "weapon_snspistol_mk2",PoliceFindDuringPlayerSearchPercentage = 25, FindPercentage = 10},
            new WeaponItem("Coil Tesla","Fires a projectile that administers a voltage capable of temporarily stunning an assailant. It's like, literally stunning.", false, ItemType.Weapons) { VanillaName = "Stun Gun",ModelName = "weapon_stungun",PoliceFindDuringPlayerSearchPercentage = 35, FindPercentage = 5},
            new WeaponItem("BS M1922","What you really need is a more recognisable gun. Stand out from the crowd at an armed robbery with this engraved pistol.", true, ItemType.Weapons) { VanillaName = "Vintage Pistol",ModelName = "weapon_vintagepistol",PoliceFindDuringPlayerSearchPercentage = 35, FindPercentage = 5},
            new WeaponItem("Vom Feuer Gruber","If you think shooting off without lifting a finger is a problem, there's a pill for that. But if you think it's a plus, we've got you covered.", true, ItemType.Weapons) { VanillaName = "WM 29 Pistol",ModelName = "weapon_pistolxm3",PoliceFindDuringPlayerSearchPercentage = 20, FindPercentage = 5},


            new WeaponItem("Malloy/Brando Waterfront","Not for the risk averse. Make it count as you'll be reloading as much as you shoot.", true, ItemType.Weapons) { VanillaName = "Marksman Pistol",ModelName = "weapon_marksmanpistol",PoliceFindDuringPlayerSearchPercentage = 45},
            new WeaponItem("Vom Feuer M1888","Because sometimes revenge is a dish best served six times, in quick succession, right between the eyes.", true, ItemType.Weapons) { VanillaName = "Double Action Revolver",ModelName = "weapon_doubleaction",PoliceFindDuringPlayerSearchPercentage = 25},
            new WeaponItem("Vom Feuer Navy Revolver","A true museum piece. You want to know how the West was won - slow reload speeds and a whole heap of bloodshed.", true, ItemType.Weapons) { VanillaName = "Navy Revolver",ModelName = "weapon_navyrevolver",PoliceFindDuringPlayerSearchPercentage = 45},
            new WeaponItem("Perico Pistol","", true, ItemType.Weapons) { ModelName = "weapon_gadgetpistol",PoliceFindDuringPlayerSearchPercentage = 20},
            new WeaponItem("Coil Tesla MP","", true, ItemType.Weapons) { VanillaName = "Stun Gun MP",ModelName = "weapon_stungun_mp",PoliceFindDuringPlayerSearchPercentage = 20},

            //Shotgun
            new WeaponItem("Shrewsbury 420 Sawed-Off","This single-barrel, sawed-off shotgun compensates for its low range and ammo capacity with devastating efficiency in close combat.", false, ItemType.Weapons) { VanillaName = "Sawed-Off Shotgun",ModelName = "weapon_sawnoffshotgun",PoliceFindDuringPlayerSearchPercentage = 45, FindPercentage = 5},
            new WeaponItem("Shrewsbury 420","Standard shotgun ideal for short-range combat. A high-projectile spread makes up for its lower accuracy at long range.", false, ItemType.Weapons) { VanillaName = "Pump Shotgun",ModelName = "weapon_pumpshotgun",PoliceFindDuringPlayerSearchPercentage = 95, FindPercentage = 5 },
            new WeaponItem("Vom Feuer 569","Only one thing pumps more action than a pump action: watch out, the recoil is almost as deadly as the shot.", true, ItemType.Weapons) { VanillaName = "Pump Shotgun Mk2",ModelName = "weapon_pumpshotgun_mk2",PoliceFindDuringPlayerSearchPercentage = 95, FindPercentage = 5},
            new WeaponItem("Vom Feuer IBS-12","Fully automatic shotgun with 8 round magazine and high rate of fire.", false, ItemType.Weapons) { VanillaName = "Assault Shotgun",ModelName = "weapon_assaultshotgun",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Hawk & Little HLSG","More than makes up for its slow, pump-action rate of fire with its range and spread. Decimates anything in its projectile path.", false, ItemType.Weapons) { VanillaName = "Bullpup Shotgun",ModelName = "weapon_bullpupshotgun",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Shrewsbury Taiga-12","The weapon to reach for when you absolutely need to make a horrible mess of the room. Best used near easy-wipe surfaces only.", true, ItemType.Weapons) { VanillaName = "Heavy Shothgun",ModelName = "weapon_heavyshotgun",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Toto 12 Guage Sawed-Off","Do one thing, do it well. Who needs a high rate of fire when your first shot turns the other guy into a fine mist?.", true, ItemType.Weapons) { VanillaName = "Double Barrel Shotgun",ModelName = "weapon_dbshotgun",PoliceFindDuringPlayerSearchPercentage = 75, FindPercentage = 5},
            new WeaponItem("Shrewsbury Defender","How many effective tools for riot control can you tuck into your pants? Ok, two. But this is the other one.", true, ItemType.Weapons) { VanillaName = "Sweeper Shotgun",ModelName = "weapon_autoshotgun",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Leotardo SPAZ-11","There's only one semi-automatic shotgun with a fire rate that sets the LSFD alarm bells ringing, and you're looking at it.", true, ItemType.Weapons) { VanillaName = "Combat Shotgun", ModelName = "weapon_combatshotgun",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},


            new WeaponItem("Red Beth Musket","Armed with nothing but muskets and a superiority complex, the Brits took over half the world. Own the gun that built an Empire.", true, ItemType.Weapons) { VanillaName = "Musket", ModelName = "weapon_musket",PoliceFindDuringPlayerSearchPercentage = 100},

            //SMG
            new WeaponItem("Shrewsbury Luzi","Combines compact design with a high rate of fire at approximately 700-900 rounds per minute.", false, ItemType.Weapons) { VanillaName = "Micro SMG", ModelName = "weapon_microsmg",PoliceFindDuringPlayerSearchPercentage = 55, FindPercentage = 5 },
            new WeaponItem("Hawk & Little MP6","This is known as a good all-around submachine gun. Lightweight with an accurate sight and 30-round magazine capacity.", false, ItemType.Weapons) { VanillaName = "SMG", ModelName = "weapon_smg",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Hawk & Little XPM","Lightweight, compact, with a rate of fire to die very messily for: turn any confined space into a kill box at the click of a well-oiled trigger.", true, ItemType.Weapons) { VanillaName = "SMG Mk2", ModelName = "weapon_smg_mk2", FindPercentage = 5},
            new WeaponItem("Vom Feuer Fisher","A high-capacity submachine gun that is both compact and lightweight. Holds up to 30 bullets in one magazine.", false, ItemType.Weapons) { VanillaName = "Assault SMG", ModelName = "weapon_assaultsmg",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Coil PXM","Who said personal weaponry couldn't be worthy of military personnel? Thanks to our lobbyists, not Congress. Integral suppressor.", false, ItemType.Weapons) { VanillaName = "Combat PDW", ModelName = "weapon_combatpdw",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Vom Feuer KEK-9","This fully automatic is the snare drum to your twin-engine V8 bass: no drive-by sounds quite right without it.", false, ItemType.Weapons) { VanillaName = "Machine Pistol", ModelName = "weapon_machinepistol",PoliceFindDuringPlayerSearchPercentage = 55, FindPercentage = 5},
            new WeaponItem("Hawk & Little Millipede","Increasingly popular since the marketing team looked beyond spec ops units and started caring about the little guys in low income areas.", false, ItemType.Weapons) { VanillaName = "Mini SMG", ModelName = "weapon_minismg",PoliceFindDuringPlayerSearchPercentage = 55, FindPercentage = 5},
            new WeaponItem("Vom Feuer PMP","The european answer to the drive-by question.", true, ItemType.Weapons) { VanillaName = "Tactical SMG", ModelName = "weapon_tecpistol",PoliceFindDuringPlayerSearchPercentage = 55, FindPercentage = 5},


            //AR
            new WeaponItem("Shrewsbury A7-4K","This standard assault rifle boasts a large capacity magazine and long distance accuracy.", false, ItemType.Weapons) { VanillaName = "Assault Rifle", ModelName = "weapon_assaultrifle",PoliceFindDuringPlayerSearchPercentage = 100 },
            new WeaponItem("Shrewsbury A2-1K","The definitive revision of an all-time classic: all it takes is a little work, and looks can kill after all.", true, ItemType.Weapons) { VanillaName = "Assault Rifle Mk2", ModelName = "weapon_assaultrifle_mk2",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
            new WeaponItem("Vom Feuer A5-1R","Combining long distance accuracy with a high capacity magazine, the Carbine Rifle can be relied on to make the hit.", false, ItemType.Weapons) { VanillaName = "Carbine Rifle", ModelName = "weapon_carbinerifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Vom Feuer A5-1R MK2","This is bespoke, artisan firepower: you couldn't deliver a hail of bullets with more love and care if you inserted them by hand.", true, ItemType.Weapons) { VanillaName = "Carbine Rifle Mk2", ModelName = "weapon_carbinerifle_mk2",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 4, },
            new WeaponItem("Vom Feuer BFR","The most lightweight and compact of all assault rifles, without compromising accuracy and rate of fire.", false, ItemType.Weapons) { VanillaName = "Advanced Rifle", ModelName = "weapon_advancedrifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Vom Feuer SL6","Combining accuracy, maneuverability, firepower and low recoil, this is an extremely versatile assault rifle for any combat situation.", false, ItemType.Weapons) { VanillaName = "Special Carbine",  ModelName = "weapon_specialcarbine",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Vom Feuer SL6 MK2","The jack of all trades just got a serious upgrade: bow to the master.", true, ItemType.Weapons) { VanillaName = "Special Carbine Mk2", ModelName = "weapon_specialcarbine_mk2",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Hawk & Little ZBZ-23","The latest Chinese import taking America by storm, this rifle is known for its balanced handling. Lightweight and very controllable in automatic fire.", false, ItemType.Weapons) { VanillaName = "Bullpup Rifle", ModelName = "weapon_bullpuprifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Hawk & Little ZBZ-25X","So precise, so exquisite, it's not so much a hail of bullets as a symphony.", true, ItemType.Weapons) { VanillaName = "Bullpup Rifle Mk2", ModelName = "weapon_bullpuprifle_mk2",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Shrewsbury Stinkov","Half the size, all the power, double the recoil: there's no riskier way to say 'I'm compensating for something'.", false, ItemType.Weapons) { VanillaName = "Compact Rifle", ModelName = "weapon_compactrifle",PoliceFindDuringPlayerSearchPercentage = 90, FindPercentage = 1},
            new WeaponItem("Vom Feuer GUH-B4","This immensely powerful assault rifle was designed for highly qualified, exceptionally skilled soldiers. Yes, you can buy it.", false, ItemType.Weapons) { VanillaName = "Military Rifle", ModelName = "weapon_militaryrifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Vom Feuer POCK","The no-holds barred 30-round answer to that eternal question: how do I get this guy off my back?", true, ItemType.Weapons) { VanillaName = "Heavy Rifle", ModelName = "weapon_heavyrifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 5},
            new WeaponItem("Vom Feuer DP1 Carbine","This season's must-have hardware for law enforcement, military personnel and anyone locked in a fight to the death with either law enforcement or military personnel.", true, ItemType.Weapons) { VanillaName = "Tactical Rifle", ModelName = "weapon_tacticalrifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 7},//old school m16
            new WeaponItem("Vom Feuer LAR","Find out why it was called the right arm of the free world as you use it to shoot down police helicopters. Not so popular south of the border.",true,ItemType.Weapons) { VanillaName = "Battle Rifle", ModelName = "weapon_battlerifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 7 },


            //LMG
            new WeaponItem("Shrewsbury PDA","General purpose machine gun that combines rugged design with dependable performance. Long range penetrative power. Very effective against large groups.", false, ItemType.Weapons) { VanillaName = "Machine Gun",  ModelName = "weapon_mg",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
            new WeaponItem("Vom Feuer BAT","Lightweight, compact machine gun that combines excellent maneuverability with a high rate of fire to devastating effect.", false, ItemType.Weapons) { VanillaName = "Combat MG", ModelName = "weapon_combatmg",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
            new WeaponItem("Vom Feuer M70E1","You can never have too much of a good thing: after all, if the first shot counts, then the next hundred or so must count for double.", true, ItemType.Weapons) { VanillaName = "Combat MG Mk2", ModelName = "weapon_combatmg_mk2",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
            new WeaponItem("Hawk & Little Kenan","Complete your look with a Prohibition gun. Looks great being fired from an Albany Roosevelt or paired with a pinstripe suit.", false, ItemType.Weapons) { VanillaName = "Gusenberg", ModelName = "weapon_gusenberg",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},

            //SNIPER
            new WeaponItem("Shrewsbury PWN","Standard sniper rifle. Ideal for situations that require accuracy at long range. Limitations include slow reload speed and very low rate of fire.", false, ItemType.Weapons) { VanillaName = "Sniper Rifle", ModelName = "weapon_sniperrifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
            new WeaponItem("Bartlett M92","Features armor-piercing rounds for heavy damage. Comes with laser scope as standard.", false, ItemType.Weapons) { VanillaName = "Heavy Sniper", ModelName = "weapon_heavysniper",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Bartlett M92 Mk2","Far away, yet always intimate: if you're looking for a secure foundation for that long-distance relationship, this is it.", true, ItemType.Weapons) { VanillaName = "Heavy Sniper", ModelName = "weapon_heavysniper_mk2",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
            new WeaponItem("Vom Feuer M23 DBS","Whether you're up close or a disconcertingly long way away, this weapon will get the job done. A multi-range tool for tools.", false, ItemType.Weapons) { VanillaName = "Marksman Rifle", ModelName = "weapon_marksmanrifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
            new WeaponItem("Vom Feuer M23 DBS Scout","Known in military circles as The Dislocator, this mod set will destroy both the target and your shoulder, in that order.", true, ItemType.Weapons) { VanillaName = "Marksman Rifle Mk2", ModelName = "weapon_marksmanrifle_mk2",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},

            new WeaponItem("Vom Feuer 699 PCR","A rifle for perfectionists. Because why settle for right-between-the-eyes, when you could have right-through-the-superior-frontal-gyrus.", true, ItemType.Weapons) { VanillaName = "Precision Rifle", ModelName = "weapon_precisionrifle",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},

            //new WeaponItem("Shrewsbury BFD Dragmeout","Want to give the impression of accuracy while still having greater than 1 MOA? Dragmeout.", true, ItemType.Weapons) { ModelName = "weapon_russiansniper"},
            //HEAVY
            new WeaponItem("RPG-7","A portable, shoulder-launched, anti-tank weapon that fires explosive warheads. Very effective for taking down vehicles or large groups of assailants.", false, ItemType.Weapons) { VanillaName = "RPG", ModelName = "weapon_rpg",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Hawk & Little MGL","A compact, lightweight grenade launcher with semi-automatic functionality. Holds up to 10 rounds.", false, ItemType.Weapons) { VanillaName = "Grenade Launcher", ModelName = "weapon_grenadelauncher",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
            new WeaponItem("Hawk & Little MGL Smoke","A compact, lightweight grenade launcher with semi-automatic functionality. Holds up to 10 rounds. Fires only smoke rounds", false, ItemType.Weapons) { VanillaName = "Smoke Grenade Launcher", ModelName = "weapon_grenadelauncher_smoke",PoliceFindDuringPlayerSearchPercentage = 100, FindPercentage = 1},
                //Unused
            new WeaponItem("Homing Launcher","", true, ItemType.Weapons) { ModelName = "weapon_hominglauncher",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Compact Grenade Launcher","", true, ItemType.Weapons) { ModelName = "weapon_compactlauncher",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Compact EMP Launcher","", true, ItemType.Weapons) { ModelName = "weapon_emplauncher",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Minigun","A devastating 6-barrel machine gun that features Gatling-style rotating barrels.", true, ItemType.Weapons) { ModelName = "weapon_minigun",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Firework Launcher","", true, ItemType.Weapons) { ModelName = "weapon_firework",PoliceFindDuringPlayerSearchPercentage = 100},


            //Thrown & Other
            new WeaponItem("M61 Grenade","Standard fragmentation grenade. Pull pin, throw, then find cover. Ideal for eliminating clustered assailants.", false, ItemType.Weapons) { ModelName = "weapon_grenade",PoliceFindDuringPlayerSearchPercentage = 85, FindPercentage = 1},
            new WeaponItem("Improvised Incendiary","Crude yet highly effective incendiary weapon. No happy hour with this cocktail.", false, ItemType.Weapons) { ModelName = "weapon_molotov",PoliceFindDuringPlayerSearchPercentage = 85, FindPercentage = 5},
            new WeaponItem("BZ Gas Grenade","BZ gas grenade, particularly effective at incapacitating multiple assailants.", false, ItemType.Weapons) { ModelName = "weapon_bzgas",PoliceFindDuringPlayerSearchPercentage = 85, FindPercentage = 5},
            new WeaponItem("Tear Gas Grenade","Tear gas grenade, particularly effective at incapacitating multiple assailants. Sustained exposure can be lethal.", false, ItemType.Weapons) { ModelName = "weapon_smokegrenade",PoliceFindDuringPlayerSearchPercentage = 85, FindPercentage = 5},

            //Unused
            new WeaponItem("Gas Can","", true, ItemType.Weapons) { ModelName = "weapon_petrolcan",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Parachute","", true, ItemType.Weapons) { ModelName = "gadget_parachute",PoliceFindDuringPlayerSearchPercentage = 0},
            new WeaponItem("Fire Extinguisher","", true, ItemType.Weapons) { ModelName = "weapon_fireextinguisher",PoliceFindDuringPlayerSearchPercentage = 0},
            new WeaponItem("Hazardous Jerry Can","", true, ItemType.Weapons) { ModelName = "weapon_hazardcan",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Fertilizer Can","", true, ItemType.Weapons) { ModelName = "weapon_fertilizercan",PoliceFindDuringPlayerSearchPercentage = 100},

            new WeaponItem("Sticky Bomb","", true, ItemType.Weapons) { ModelName = "weapon_stickybomb",PoliceFindDuringPlayerSearchPercentage = 85},
            new WeaponItem("Proximity Mines","", true, ItemType.Weapons) { ModelName = "weapon_proxmine",PoliceFindDuringPlayerSearchPercentage = 85},
            new WeaponItem("Snowball","", true, ItemType.Weapons) { ModelName = "weapon_snowball",PoliceFindDuringPlayerSearchPercentage = 0},
            new WeaponItem("Pipe Bomb","", true, ItemType.Weapons) { ModelName = "weapon_pipebomb",PoliceFindDuringPlayerSearchPercentage = 85},
            new WeaponItem("Baseball","", true, ItemType.Weapons) { ModelName = "weapon_ball",PoliceFindDuringPlayerSearchPercentage = 0},
            new WeaponItem("Flare","", true, ItemType.Weapons) { ModelName = "weapon_flare",PoliceFindDuringPlayerSearchPercentage = 0},
            new WeaponItem("Acid Package","", true, ItemType.Weapons) { ModelName = "weapon_acidpackage",PoliceFindDuringPlayerSearchPercentage = 85},

            //Cringe and unused
            new WeaponItem("Unholy Hellbringer","", true, ItemType.Weapons) { ModelName = "weapon_raycarbine",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Railgun","", true, ItemType.Weapons) { ModelName = "weapon_railgun",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Widowmaker","", true, ItemType.Weapons) { ModelName = "weapon_rayminigun",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Railgun","", true, ItemType.Weapons) { ModelName = "weapon_railgunxm3",PoliceFindDuringPlayerSearchPercentage = 100},
            new WeaponItem("Up-n-Atomizer","", true, ItemType.Weapons) { ModelName = "weapon_raypistol",PoliceFindDuringPlayerSearchPercentage = 85},


            
        });
    }
}