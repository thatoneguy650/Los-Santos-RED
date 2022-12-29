using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ModItems : IModItems
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\ModItems.xml";

    public ModItems()
    {
        PossibleItems = new PossibleItems();
    }
    public PossibleItems PossibleItems { get; private set; }
    public ModItem Get(string name)
    {
        return AllItems().FirstOrDefault(x => x.Name == name);
    }
    //public ModItem GetRandomItem()
    //{
    //    return PossibleFoundItems().PickRandom();
    //}
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
        }
    }
    public void WriteToFile()
    {
        Serialization.SerializeParam(PossibleItems, ConfigFileName);
    }
    public List<ModItem> AllItems()
    {
        List<ModItem> AllItems = new List<ModItem>();
        AllItems.AddRange(PossibleItems.FlashlightItems);
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
        AllItems.AddRange(PossibleItems.BinocularsItems);
        AllItems.AddRange(PossibleItems.RadioItems);
        return AllItems;
    }
    public List<ModItem> PropItems()
    {
        List<ModItem> AllItems = new List<ModItem>();
        AllItems.AddRange(PossibleItems.FlashlightItems);
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
        AllItems.AddRange(PossibleItems.BinocularsItems);
        AllItems.AddRange(PossibleItems.RadioItems);

        return AllItems;
    }
    public List<ModItem> PossibleFoundItems()
    {
        List<ModItem> AllItems = new List<ModItem>();
        AllItems.AddRange(PossibleItems.FlashlightItems);
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
        AllItems.AddRange(PossibleItems.BinocularsItems);
        AllItems.AddRange(PossibleItems.RadioItems);
        return AllItems.Where(x => x.FindPercentage > 0).ToList();
    }
    public List<ModItem> InventoryItems()
    {
        List<ModItem> AllItems = new List<ModItem>();
        AllItems.AddRange(PossibleItems.FlashlightItems);
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
        AllItems.AddRange(PossibleItems.BinocularsItems);
        AllItems.AddRange(PossibleItems.RadioItems);
        return AllItems;
    }
    public void Setup(PhysicalItems physicalItems, IWeapons weapons)
    {
        foreach(ModItem modItem in AllItems())
        {
            modItem.Setup(physicalItems, weapons);
        }
    }


    public ModItem GetRandomItem()// List<string> RequiredModels)
    {
        List<ModItem> ToPickFrom = PossibleFoundItems();
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


    private void DefaultConfig()
    {
        DefaultConfig_Drinks();
        DefaultConfig_Food();
        DefaultConfig_Drugs();
        DefaultConfig_Weapons();
        DefaultConfig_Tools();
        DefaultConfig_Vehicles();
        DefaultConfig_Services();
        Serialization.SerializeParam(PossibleItems, ConfigFileName);
    }
    private void DefaultConfig_Drinks()
    {
        PossibleItems.DrinkItems.AddRange(new List<DrinkItem> {
            //Drinks
            //Bottles
            new DrinkItem("Bottle of Raine Water", "The water that rich people drink, and the main reason why there are now entire continents of plastic bottles floating in the ocean", ItemType.Drinks) { 
                ModelItemID = "ba_prop_club_water_bottle",HealthChangeAmount = 20, ThirstChangeAmount = 20.0f, ItemSubType = ItemSubType.Water, FindPercentage = 10 },//slight clipping, no issyes
            new DrinkItem("Bottle of GREY Water", "Expensive water that tastes worse than tap!", ItemType.Drinks){
                ModelItemID = "h4_prop_battle_waterbottle_01a",HealthChangeAmount = 20, ThirstChangeAmount = 20.0f, ItemSubType = ItemSubType.Water, FindPercentage = 10 },//lotsa clipping, does not have gravity
            new DrinkItem("Bottle of JUNK Energy", "The Quick Fix!", ItemType.Drinks){
                ModelItemID = "prop_energy_drink",HealthChangeAmount = 30, ThirstChangeAmount = 20.0f,SleepChangeAmount = 10.0f, ItemSubType = ItemSubType.Soda, FindPercentage = 10 },//fine
            //Beer
            new DrinkItem("Bottle of PiBwasser", "Cheap 11% ABV fighting lager brewed in Germany for export only from rice, barley, hops and the fresh urine of Bavarian virgins", ItemType.Drinks){
                ModelItemID = "prop_amb_beer_bottle",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5 },//is perfecto
            new DrinkItem("Bottle of A.M.", "Mornings Golden Shower", ItemType.Drinks){
                ModelItemID = "prop_beer_am",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Barracho", "Es Playtime!", ItemType.Drinks){
                ModelItemID = "prop_beer_bar",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Blarneys", "Making your mouth feel lucky", ItemType.Drinks){
                ModelItemID = "prop_beer_blr", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Jakeys", "Drink Outdoors With Jakey's", ItemType.Drinks){
                ModelItemID = "prop_beer_jakey", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Logger", "A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned", ItemType.Drinks){
                ModelItemID = "prop_beer_logger",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Patriot", "Never refuse a patriot", ItemType.Drinks){
                ModelItemID = "prop_beer_patriot", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Pride", "Swallow Me", ItemType.Drinks){
                ModelItemID = "prop_beer_pride", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Stronzo", "Birra forte d'Italia", ItemType.Drinks){
                ModelItemID = "prop_beer_stz", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            new DrinkItem("Bottle of Dusche", "Das Ist Gut Ja!", ItemType.Drinks){
                ModelItemID = "prop_beerdusche", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 5},//Does not have gravity, attachmentis too far down
            //Liquor
            new DrinkItem("Bottle of 40 oz", "Drink like a true thug!", ItemType.Drinks){
                ModelItemID = "prop_cs_beer_bot_40oz", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer, FindPercentage = 1},
            new DrinkItem("Bottle of Sinsimito Tequila", "Extra Anejo 100% De Agave. 42% Alcohol by volume", ItemType.Drinks){
                PackageItemID = "h4_prop_h4_t_bottle_02a", IntoxicantName = "High Proof Alcohol",HealthChangeAmount = 15, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Liquor, FindPercentage = 1},
            new DrinkItem("Bottle of Cazafortuna Tequila", "Tequila Anejo. 100% Blue Agave 40% Alcohol by volume", ItemType.Drinks){
                PackageItemID = "h4_prop_h4_t_bottle_01a", IntoxicantName = "High Proof Alcohol",HealthChangeAmount = 15, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Liquor, FindPercentage = 1},
            //Cups & Cans
            new DrinkItem("Can of eCola", "Deliciously Infectious!", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacan_01a", HealthChangeAmount = 10, SleepChangeAmount = 1.0f, ThirstChangeAmount = 10.0f, ItemSubType= ItemSubType.Soda, FindPercentage = 10 },
            new DrinkItem("Can of Sprunk", "Slurp Sprunk Mmm! Delicious", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacan_01b", HealthChangeAmount = 10, SleepChangeAmount = 1.0f, ThirstChangeAmount = 10.0f, ItemSubType= ItemSubType.Soda, FindPercentage = 10},
            new DrinkItem("Can of Orang-O-Tang", "Orange AND Tang! Orang-O-Tang!", ItemType.Drinks){
                ModelItemID = "prop_orang_can_01",HealthChangeAmount = 10, ItemSubType= ItemSubType.Soda, FindPercentage = 10},//needs better attachment
            new DrinkItem("Carton of Milk", "Full Fat. Farmed and produced in U.S.A.", ItemType.Drinks) { HealthChangeAmount = 10, ThirstChangeAmount = 10.0f, HungerChangeAmount = 5.0f, ItemSubType= ItemSubType.Milk },
            new DrinkItem("Cup of eCola", "Deliciously Infectious!", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01a",HealthChangeAmount = 10, SleepChangeAmount = 1.0f, ThirstChangeAmount = 10.0f, ItemSubType= ItemSubType.Soda},//has no gravity, too far down
            new DrinkItem("Cup of Sprunk", "Slurp Sprunk Mmm! Delicious", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01b",HealthChangeAmount = 10, SleepChangeAmount = 1.0f, ThirstChangeAmount = 10.0f, ItemSubType= ItemSubType.Soda},//perfecto
            new DrinkItem("Cup of Coffee", "Finally something without sugar! Sugar on Request", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_02",HealthChangeAmount = 10, SleepChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Can of Hoplivion Double IPA", "So many hops it should be illegal.", ItemType.Drinks){
                ModelItemID = "h4_prop_h4_can_beer_01a",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,},//pretty good, maybeslightly off
            new DrinkItem("Can of Blarneys", "Making your mouth feel lucky", ItemType.Drinks) 
                { IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, FindPercentage = 1 },
            new DrinkItem("Can of Logger", "A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned", ItemType.Drinks) 
                { IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, FindPercentage = 1 },
            //Bean Machine
            new DrinkItem("High Noon Coffee", "Drip coffee, carbonated water, fruit syrup and taurine.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 10, SleepChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("The Eco-ffee", "Decaf light, rain forest rain, saved whale milk, chemically reclaimed freerange organic tofu, and recycled brown sugar", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 12, SleepChangeAmount = 12.0f, ThirstChangeAmount = 12.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Speedball Coffee", "Caffeine tripe-shot, guarana, bat guano, and mate.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 15, SleepChangeAmount = 15.0f, ThirstChangeAmount = 15.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Gunkacchino Coffee", "Caffeine, refined sugar, trans fat, high-fructose corn syrup, and cheesecake base.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 17, SleepChangeAmount = 17.0f, ThirstChangeAmount = 17.0f,},//perfecto
            new DrinkItem("Bratte Coffee", "Double shot latte, and 100 pumps of caramel.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 5, SleepChangeAmount = 5.0f, ThirstChangeAmount = 5.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Flusher Coffee", "Caffeine, organic castor oil, concanetrated OJ, chicken vindaldo, and senna pods.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 10, SleepChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Caffeagra Coffee", "Caffeine (Straight up), rhino horn, oyster shell, and sildenafil citrate.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 17, SleepChangeAmount = 17.0f, ThirstChangeAmount = 17.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new DrinkItem("Big Fruit Smoothie", "Frothalot, watermel, carbonated water, taurine, and fruit syrup.", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 15, SleepChangeAmount = 15.0f, ThirstChangeAmount = 15.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            //UP N ATOM
            new DrinkItem("Jumbo Shake", "Almost a whole cow full of milk", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01c",HealthChangeAmount = 10, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Milk},//no gravity, attached wrong
            //burger shot
            new DrinkItem("Double Shot Coffee", ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 5, SleepChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Coffee },//n gravity,not attached right
            new DrinkItem("Liter of eCola", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01a",HealthChangeAmount = 15, SleepChangeAmount = 2.0f, ThirstChangeAmount = 15.0f, ItemSubType = ItemSubType.Soda},//n gravity,not attached right
            new DrinkItem("Liter of Sprunk", ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01b", HealthChangeAmount = 15, SleepChangeAmount = 2.0f, ThirstChangeAmount = 15.0f,ItemSubType = ItemSubType.Soda },//n gravity,not attached right 
        });;
    }
    private void DefaultConfig_Drugs()
    {
        PossibleItems.SmokeItems.AddRange(new List<SmokeItem>
        {
            //Cigarettes/Cigars
            new SmokeItem("Redwood Regular", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda", ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 15 },
            new SmokeItem("Redwood Mild", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda. Milder version", ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs2",AmountPerPackage = 20, HealthChangeAmount = -5,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 5 },
            new SmokeItem("Debonaire", "Tobacco products marketed at the more sophisticated smoker, whoever that is", ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs3",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 2 },
            new SmokeItem("Debonaire Menthol", "Tobacco products marketed at the more sophisticated smoker, whoever that is. With Menthol!", ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs4",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 2 },
            new SmokeItem("Caradique", "Fine Napoleon Cigarettes", ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs5",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 2 },
            new SmokeItem("69 Brand","Don't let an embargo stop you", ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs6",AmountPerPackage = 20, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette, FindPercentage = 2 },
            //new Vector3(-0.025f,0.01f,0.004f),new Rotator(0f, 0f, 90f) female mouth attach?
            new SmokeItem("Estancia Cigar","Medium Cut. Hand Rolled.", ItemType.Drugs) {
                ModelItemID = "prop_cigar_02",
                PackageItemID = "p_cigar_pack_02_s",AmountPerPackage = 20, HealthChangeAmount = -5,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigar, FindPercentage = 1 },
            //new ModItem("ElectroToke Vape","The Electrotoke uses highly sophisticated micro-molecule atomization technology to make the ingestion of hard drugs healthy, dscreet, pleasurable and, best of all, completely safe.", ItemType.Drugs) {
            //    ModelItemID = "h4_prop_battle_vape_01"), IntoxicantName = "Marijuana", PercentLostOnUse = 0.05f },
            new SmokeItem("Marijuana","Little Jacob Tested, Truth Approved", ItemType.Drugs) {
                ModelItemID = "p_cs_joint_01"//p_amb_joint_01
                ,PackageItemID = "sf_prop_sf_bag_weed_01a", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", ItemSubType = ItemSubType.Narcotic, FindPercentage = 2 },
            //new SmokeItem("White Widow","Among the most famous strains worldwide is White Widow, a balanced hybrid first bred in the Netherlands by Green House Seeds.", ItemType.Drugs) {
            //    ModelItemID = "p_cs_joint_01",PackageItemID = "prop_weed_bottle", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", ItemSubType = ItemSubType.Narcotic, FindPercentage = 2},
            //new SmokeItem("OG Kush","OG Kush, also known as 'Premium OG Kush', was first cultivated in Florida in the early '90s when a marijuana strain from Northern California was supposedly crossed with Chemdawg, Lemon Thai and a Hindu Kush plant from Amsterdam.", ItemType.Drugs) {
            //    ModelItemID = "p_cs_joint_01",PackageItemID = "prop_weed_bottle", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", ItemSubType = ItemSubType.Narcotic, FindPercentage = 2 },
            //new SmokeItem("Northern Lights","Northern Lights, also known as 'NL', is an indica marijuana strain made by crossing Afghani with Thai.", ItemType.Drugs) {
            //    ModelItemID = "p_cs_joint_01",PackageItemID = "prop_weed_bottle", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", ItemSubType = ItemSubType.Narcotic, FindPercentage = 2 },

        });
        PossibleItems.IngestItems.AddRange(new List<IngestItem>
        {
            new IngestItem("Bull Shark Testosterone","More bite than bush elephant testosterone. Become more aggressive, hornier, and irresistible to women! The ultimate man!", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Bull Shark Testosterone" , AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic, FindPercentage = 1},
            new IngestItem("Alco Patch","The Alco Patch. It's the same refreshing feeling of your favorite drink, but delivered transdermally and discreetly. Pick up the Alco Patch at your local pharmacy.", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Alco Patch",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic, FindPercentage = 1},
            new IngestItem("Lax to the Max","Lubricated suppositories. Get flowing again!", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Alco Patch",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic, FindPercentage = 1},
            new IngestItem("Mollis","For outstanding erections. Get the performance you've always dreamed of", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Mollis",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic, FindPercentage = 1},
            new IngestItem("Chesty","Cough suppressant manufactured by Good Aids Pharmacy. Gives 24-hour relief and is available in honey flavour.", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Chesty", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },
            new IngestItem("Equanox","Combats dissatisfaction, lethargy, depression, melancholy, sexual dysfunction. May cause nausea, loss of sleep, blurred vision, leakage, kidney problems and breathing irregularities.", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Equanox", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },
            new IngestItem("Zombix","Painkiller and antidepressant manufactured by O'Deas Pharmacy. ~n~'Go straight for the head.'", ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Zombix", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },
            new IngestItem("SPANK","You looking for some fun? a little.. hmmm? Some SPANK?", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_pills",IntoxicantName = "SPANK", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },
            new IngestItem("Toilet Cleaner","The hot new legal high that takes you to places you never imagined and leaves you forever changed.", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_pills",IntoxicantName = "Toilet Cleaner", ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },

            new IngestItem("Hingmyralgan","For Brain-Ache and other pains", ItemType.Drugs) { IsPossessionIllicit = false,AmountPerPackage = 25,
                ModelItemID = "prop_cs_pills", ItemSubType = ItemSubType.Painkiller,HealthChangeAmount = 20, FindPercentage = 15, AlwaysChangesHealth = true },


            new IngestItem("Deludamol","For a Night You'll Never Remember", ItemType.Drugs) { IsPossessionIllicit = false,AmountPerPackage = 25,
                ModelItemID = "prop_cs_pills", ItemSubType = ItemSubType.Painkiller,HealthChangeAmount = 25,ThirstChangeAmount = -1, FindPercentage = 5, AlwaysChangesHealth = true },

            new IngestItem("Delladamol","Gives A Time You Won't Recall", ItemType.Drugs) { IsPossessionIllicit = false,AmountPerPackage = 25,
                ModelItemID = "prop_cs_pills", ItemSubType = ItemSubType.Painkiller,HealthChangeAmount = 15,ThirstChangeAmount = -3, FindPercentage = 5, AlwaysChangesHealth = true },

        });
        PossibleItems.InhaleItems.AddRange(new List<InhaleItem>
        {
            new InhaleItem("Cocaine","Also known as coke, crack, girl, lady, charlie, caine, tepung, and snow", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "ba_prop_battle_sniffing_pipe"
                ,PackageItemID = "prop_meth_bag_01"
                ,IntoxicantName = "Cocaine", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },
        });
        PossibleItems.InjectItems.AddRange(new List<InjectItem>
        {
            new InjectItem("Heroin","Heroin was first made by C. R. Alder Wright in 1874 from morphine, a natural product of the opium poppy", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_syringe_01"
                ,PackageItemID = "prop_meth_bag_01"
                ,IntoxicantName = "Heroin", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },
        });
        PossibleItems.PipeSmokeItems.AddRange(new List<PipeSmokeItem>
        {
            new PipeSmokeItem("Methamphetamine","Also referred to as Speed, Sabu, Crystal and Meth", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_meth_pipe"
                ,PackageItemID = "prop_meth_bag_01"
                ,IntoxicantName = "Methamphetamine", PercentLostOnUse = 0.25f, MeasurementName = "Gram",  ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },
            new PipeSmokeItem("Crack", ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_crackpipe"
                ,PackageItemID = "prop_meth_bag_01"
                ,IntoxicantName = "Crack", PercentLostOnUse = 0.5f, MeasurementName = "Gram",  ItemSubType = ItemSubType.Narcotic, FindPercentage = 1 },
        });

    }
    private void DefaultConfig_Food()
    {
        PossibleItems.FoodItems.AddRange(new List<FoodItem>
        {
            //Generic Food
            new FoodItem("Hot Dog","Your favorite mystery meat sold on street corners everywhere. Niko would be proud", ItemType.Food) {
                ModelItemID = "prop_cs_hotdog_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Hot Sausage","Get all your jokes out", ItemType.Food) {
                ModelItemID = "prop_cs_hotdog_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Hot Pretzel","You tie me up", ItemType.Food) {HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("3 Mini Pretzels","Like a pretzel, but smaller", ItemType.Food) {HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Nuts","You're gonna love my nuts", ItemType.Food) {HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack },
            new FoodItem("Burger","100% Certified Food", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Donut","MMMMMMM Donuts", ItemType.Food) {
                ModelItemID = "prop_donut_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Snack } ,
            new FoodItem("Bagel Sandwich","Bagel with extras, what more do you need?", ItemType.Food) {
                ModelItemID = "p_amb_bagel_01", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("French Fries","Freedom fries made from true Cataldo potatoes!", ItemType.Food) {
                ModelItemID = "prop_food_chips", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Side },
            new FoodItem("Fries","Freedom fries made from true Cataldo potatoes!", ItemType.Food) {
                ModelItemID = "prop_food_chips", HealthChangeAmount = 5, HungerChangeAmount = 5.0f,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Side },
            new FoodItem("Banana","An elongated, edible fruit – botanically a berry[1][2] – produced by several kinds of large herbaceous flowering plants in the genus Musa", ItemType.Food) {
                ModelItemID = "ng_proc_food_nana1a", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Fruit },
            new FoodItem("Orange","Not just a color", ItemType.Food) {
                ModelItemID = "ng_proc_food_ornge1a", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Fruit },
            new FoodItem("Apple","Certified sleeping death free", ItemType.Food) {
                ModelItemID = "ng_proc_food_aple1a", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = 5.0f, ItemSubType = ItemSubType.Fruit },
            new FoodItem("Ham and Cheese Sandwich","Basic and shitty, just like you", ItemType.Food) {
                ModelItemID = "prop_sandwich_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Turkey Sandwich","The most plain sandwich for the most plain person", ItemType.Food) {
                ModelItemID = "prop_sandwich_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Tuna Sandwich","Haven't got enough heavy metals in you at your job? Try tuna!", ItemType.Food) {
                ModelItemID = "prop_sandwich_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Taco","", ItemType.Food) { HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree, ModelItemID = "prop_taco_01" },
            new FoodItem("Strawberry Rails Cereal","The breakfast food you snort!", ItemType.Food) { HealthChangeAmount = 50, HungerChangeAmount = 30.0f, ItemSubType = ItemSubType.Cereal} ,
            new FoodItem("Crackles O' Dawn Cereal","Smile at the crack!", ItemType.Food) { HealthChangeAmount = 60, HungerChangeAmount = 40.0f, ThirstChangeAmount = -5.0f, ItemSubType = ItemSubType.Cereal} ,
            new FoodItem("White Bread","Extra white, with minimal taste.", ItemType.Food) { HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -5.0f, AmountPerPackage = 25, ItemSubType = ItemSubType.Bread} ,
            //Pizza
            new FoodItem("Slice of Pizza","Caution may be hot", ItemType.Food) {
                ModelItemID = "v_res_tt_pizzaplate", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Small Cheese Pizza","Best when you are home alone.", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 25, HungerChangeAmount = 25.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Small Pepperoni Pizza","Get a load of our pepperonis!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 30, HungerChangeAmount = 30.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Small Supreme Pizza","Get stuffed", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 35, HungerChangeAmount = 35.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Medium Cheese Pizza","Best when you are home alone.", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 50, HungerChangeAmount = 50.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Medium Pepperoni Pizza","Get a load of our pepperonis!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 55, HungerChangeAmount = 55.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Medium Supreme Pizza","Get stuffed", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 60, HungerChangeAmount = 60.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Large Cheese Pizza","Best when you are home alone.", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 65, HungerChangeAmount = 65.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Large Pepperoni Pizza","Get a load of our pepperonis!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 70, HungerChangeAmount = 70.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("Large Supreme Pizza","Get stuffed", ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 75, HungerChangeAmount = 75.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("10 inch Cheese Pizza","Extra cheesy!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 25, HungerChangeAmount = 25.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("10 inch Pepperoni Pizza","Mostly Meat!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 30, HungerChangeAmount = 30.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("10 inch Supreme Pizza","We forgot the kitchen sink!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 35, HungerChangeAmount = 35.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("12 inch Cheese Pizza","Extra cheesy!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 50, HungerChangeAmount = 50.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("12 inch Pepperoni Pizza","Mostly Meat!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 55, HungerChangeAmount = 55.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("12 inch Supreme Pizza","We forgot the kitchen sink!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 60, HungerChangeAmount = 60.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("18 inch Cheese Pizza","Extra cheesy! Extra Large!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 65, HungerChangeAmount = 65.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("18 inch Pepperoni Pizza","Mostly Meat! Extra Large!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 70, HungerChangeAmount = 70.0f, ItemSubType = ItemSubType.Pizza } ,
            new FoodItem("18 inch Supreme Pizza","We forgot the kitchen sink! Extra Large!", ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 75, HungerChangeAmount = 75.0f, ItemSubType = ItemSubType.Pizza } ,
            //Chips
            new FoodItem("Sticky Rib Phat Chips","They are extra phat. Sticky Rib Flavor.", ItemType.Food) {
                ModelItemID = "v_ret_ml_chips1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack, FindPercentage = 10 },
            new FoodItem("Habanero Phat Chips","They are extra phat. Habanero flavor", ItemType.Food) {
                ModelItemID = "v_ret_ml_chips2", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack, FindPercentage = 10 },
            new FoodItem("Supersalt Phat Chips","They are extra phat. Supersalt flavor.", ItemType.Food) {
                ModelItemID = "v_ret_ml_chips3", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack, FindPercentage = 10 },
            new FoodItem("Big Cheese Phat Chips","They are extra phat. Big Cheese flavor.", ItemType.Food) {
                ModelItemID = "v_ret_ml_chips4", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack, FindPercentage = 10 },
            //Candy
            new FoodItem("Ego Chaser Energy Bar","Contains 20,000 Calories! ~n~'It's all about you'", ItemType.Food) {
                ModelItemID = "prop_choc_ego", HealthChangeAmount = 20, HungerChangeAmount = 20.0f,SleepChangeAmount = 5.0f, ItemSubType = ItemSubType.Snack, FindPercentage = 10 },
            new FoodItem("King Size P's & Q's","The candy bar that kids and stoners love. EXTRA Large", ItemType.Food) {
                ModelItemID = "prop_candy_pqs", HealthChangeAmount = 15, HungerChangeAmount = 15.0f,SleepChangeAmount = 5.0f, ItemSubType = ItemSubType.Snack, FindPercentage = 10 },
            new FoodItem("P's & Q's","The candy bar that kids and stoners love", ItemType.Food) {
                ModelItemID = "prop_choc_pq", HealthChangeAmount = 10, HungerChangeAmount = 10.0f,SleepChangeAmount = 5.0f, ItemSubType = ItemSubType.Snack, FindPercentage = 10 },
            new FoodItem("Meteorite Bar","Dark chocolate with a GOOEY core", ItemType.Food) {
                ModelItemID = "prop_choc_meto", HealthChangeAmount = 10, HungerChangeAmount = 10.0f,SleepChangeAmount = 5.0f, ItemSubType = ItemSubType.Snack, FindPercentage = 10 },
            //UPNATOM
            new FoodItem("Triple Burger", "Three times the meat, three times the cholesterol", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Bacon Triple Cheese Melt", "More meat AND more bacon", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            //BurgerShot
            new FoodItem("Money Shot Meal", ItemType.Food) {
                ModelItemID = "prop_food_bs_burg1"
                ,PackageItemID = "prop_food_bs_tray_02", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("The Bleeder Meal","", ItemType.Food) {
                ModelItemID = "prop_food_bs_burg1"
                ,PackageItemID = "prop_food_bs_tray_02", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Torpedo Meal","Torpedo your hunger", ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bs_tray_03", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Meat Free Meal","For the bleeding hearts", ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bs_tray_01", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Freedom Fries", ItemType.Food) {
                ModelItemID = "prop_food_bs_chips", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack },
            //Bite
            new FoodItem("Gut Buster Sandwich", ItemType.Food) {
                ModelItemID = "prop_food_burg2", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Ham and Tuna Sandwich", ItemType.Food) {
                ModelItemID = "prop_food_burg2", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Chef's Salad", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree} ,
            //BeefyBills
            new FoodItem("Megacheese Burger", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Double Burger", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Kingsize Burger", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Bacon Burger", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 17, HungerChangeAmount = 17.0f, ItemSubType = ItemSubType.Entree },
            //Taco Bomb
            new FoodItem("Breakfast Burrito", ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bag1", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Deep Fried Salad", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Beef Bazooka", ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bag1", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Chimichingado Chiquito", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Cheesy Meat Flappers", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Volcano Mudsplatter Nachos", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            //Wigwam
            new FoodItem("Wigwam Burger", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Wigwam Cheeseburger", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Big Wig Burger", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ItemSubType = ItemSubType.Entree },
            //Cluckin Bell
            new FoodItem("Cluckin' Little Meal","May contain meat", ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_03", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ThirstChangeAmount = 5.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new FoodItem("Cluckin' Big Meal","200% bigger breasts", ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_02", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Cluckin' Huge Meal", ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_02", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Wing Piece", ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_03", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new FoodItem("Little Peckers", ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_03", HealthChangeAmount = 5, HungerChangeAmount = 5.0f , ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new FoodItem("Balls & Rings", ItemType.Food) {
                ModelItemID = "prop_food_burg3", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Side },
            new FoodItem("Fowlburger", ItemType.Food) {
                ModelItemID = "prop_food_burg1",
                PackageItemID = "prop_food_burg3", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            //Generic Restaurant
            //FancyDeli
            new FoodItem("Chicken Club Salad", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Spicy Seafood Gumbo", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Muffaletta", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Zucchini Garden Pasta", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Pollo Mexicano", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Italian Cruz Po'boy", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Chipotle Chicken Panini", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //FancyFish
            new FoodItem("Coconut Crusted Prawns", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Crab and Shrimp Louie", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Open-Faced Crab Melt", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("King Salmon", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Ahi Tuna", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Key Lime Pie", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //FancyGeneric
            new FoodItem("Smokehouse Burger", ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 20, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new FoodItem("Chicken Critters Basket", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Prime Rib 16 oz", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Bone-In Ribeye", ItemType.Food) {
                PackageItemID = "prop_cs_steak", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Grilled Pork Chops", ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Grilled Shrimp", ItemType.Food) {
                PackageItemID = "prop_food_bag1" , HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //Noodles
            new FoodItem("Juek Suk tong Mandu", ItemType.Food) {
                ModelItemID = "prop_ff_noodle_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new FoodItem("Hayan Jam Pong", ItemType.Food) {
                ModelItemID = "prop_ff_noodle_02", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ThirstChangeAmount = 5.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Sal Gook Su Jam Pong", ItemType.Food) {
                ModelItemID = "prop_ff_noodle_01", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Chul Pan Bokkeum Jam Pong", ItemType.Food) {
                ModelItemID = "prop_ff_noodle_02", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Deul Gae Udon", ItemType.Food) {
                ModelItemID = "prop_ff_noodle_02", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new FoodItem("Dakgogo Bokkeum Bap", ItemType.Food) {
                ModelItemID = "prop_ff_noodle_01", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
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
    private void DefaultConfig_Tools()
    {

        PossibleItems.RadioItems.AddRange(new List<RadioItem>
        {
            new RadioItem("Schmidt & Priss TL6 Scanner","Ever wonder what the LSPD talks about behind your back? Wonder no further.") {
                ModelItemID = "prop_cs_hand_radio", FindPercentage = 10 },
        });
        PossibleItems.ScrewdriverItems.AddRange(new List<ScrewdriverItem>
        {
            //Generic Tools
            new ScrewdriverItem("Flint Phillips Screwdriver","Might get you into some locked things. No relation.") {
                ModelItemID = "prop_tool_screwdvr01", FindPercentage = 10 },
            new ScrewdriverItem("Flint Flathead Screwdriver","Might get you into some locked things. With a nice flat head.") {
                ModelItemID = "gr_prop_gr_sdriver_01", FindPercentage = 10 },
            new ScrewdriverItem("Flint Multi-Bit Screwdriver","Might get you into some locked things. Now multi-bit!") {
                ModelItemID = "gr_prop_gr_sdriver_02", FindPercentage = 10 },
        });
        PossibleItems.DrillItems.AddRange(new List<DrillItem>
        {
            new DrillItem("Power Metal Cordless Drill","Not recommended for dentistry") {
                ModelItemID = "gr_prop_gr_drill_01a"  },
            new DrillItem("Power Metal Cordless Impact Driver","DRIVE it right in!") {
                ModelItemID = "gr_prop_gr_driver_01a"  },
            new DrillItem("Flint Cordless Drill","2-Speed Battery Drill. Impact-resistant casing. Light, compact and easy to use.") {
                ModelItemID = "prop_tool_drill"  },
        });
        PossibleItems.PliersItems.AddRange(new List<PliersItem>
        {
            new PliersItem("Flint Pliers","For mechanics, pipe bomb makers, and amateur dentists alike. When you really need to grab something.") {
                ModelItemID = "prop_tool_pliers", FindPercentage = 10  },      
        });
        PossibleItems.LighterItems.AddRange(new List<LighterItem>
        {
            new LighterItem("DIC Lighter","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged") {
                ModelItemID = "p_cs_lighter_01", PercentLostOnUse = 0.01f, FindPercentage = 10 },
            new LighterItem("DIC Lighter Ultra","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Long burn version.") {
                ModelItemID = "p_cs_lighter_01", PercentLostOnUse = 0.005f, FindPercentage = 2 },
            new LighterItem("Dippo Lighter","Want to have all the hassle of carrying a lighter only for it to be out of fluid when you need it? Dippo is for you!") {
                ModelItemID = "v_res_tt_lighter", PercentLostOnUse = 0.05f, FindPercentage = 2 },
            new LighterItem("DIC Lighter Silver","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Too poor for gold?") {
                ModelItemID = "ex_prop_exec_lighter_01", PercentLostOnUse = 0.02f, FindPercentage = 1 },
            new LighterItem("DIC Lighter Gold","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Golden so it must be good!") {
                ModelItemID = "lux_prop_lighter_luxe",  PercentLostOnUse = 0.02f, FindPercentage = 1 },
        });
        PossibleItems.TapeItems.AddRange(new List<TapeItem>
        {
            new TapeItem("Flint Duct Tape","~r~CURRENTLY UNUSED~s~ Sticks to anything! Ducts, wrists, windows, mouths, and more.") {
                ModelItemID = "gr_prop_gr_tape_01", FindPercentage = 10 },
        });
        PossibleItems.HammerItems.AddRange(new List<HammerItem>
        {
            new HammerItem("Flint Rubber Mallet","Give it a whack") {
                ModelItemID = "gr_prop_gr_hammer_01"  },
        });
        PossibleItems.BongItems.AddRange(new List<BongItem>
        {
            new BongItem("Bong","~r~CURRENTLY UNUSED~s~ Also known as a water pipe") {
                ModelItemID = "prop_bong_01" } ,
        });
        PossibleItems.FlashlightItems.AddRange(new List<FlashlightItem> {
            new FlashlightItem("iFruit Cellphone","All of the price, none of the features.") {
                ModelItemID = "prop_phone_ing",
                EmissiveDistance = 25.0f,EmissiveBrightness = 0.5f,EmissiveRadius = 8.0f,UseFakeEmissive = false,AllowPropRotation = false,   IsCellphone = true,CanSearch = false,
            },
            new FlashlightItem("Facade Cellphone","Operating system dictators, software monopolists and licensing racketeers.") {
                ModelItemID = "prop_phone_ing_02",
                EmissiveDistance = 25.0f,EmissiveBrightness = 0.5f,EmissiveRadius = 8.0f,UseFakeEmissive = false,AllowPropRotation = false,IsCellphone = true,CanSearch = false,
            },
            new FlashlightItem("Badger Cellphone","A first-world global communications company with third-world cell phone coverage.") {
                ModelItemID = "prop_phone_ing_03",
                EmissiveDistance = 25.0f,EmissiveBrightness = 0.5f,EmissiveRadius = 8.0f,UseFakeEmissive = false,AllowPropRotation = false,IsCellphone = true,CanSearch = false,
            },
            new FlashlightItem("TAG-HARD Flashlight","Need to beat a suspect, but don't have your nightstick? Look no further.") {
                ModelItemID = "prop_cs_police_torch",
                EmissiveRadius = 10f, EmissiveDistance = 75f,EmissiveBrightness = 0.75f, FindPercentage = 1 },
            new FlashlightItem("Flint Handle Flashlight","Light up the jobsite, or the dead hookers.") {
                ModelItemID = "prop_tool_torch",
                EmissiveRadius = 15f, EmissiveDistance = 100f,EmissiveBrightness = 1.0f, },

        });
        PossibleItems.ShovelItems.AddRange(new List<ShovelItem> {
            new ShovelItem("Flint Shovel","A lot of holes in the desert, and a lot of problems are buried in those holes. But you gotta do it right. I mean, you gotta have the hole already dug before you show up with a package in the trunk.") {
                ModelItemID = "prop_tool_shovel"  },
        });
        PossibleItems.UmbrellaItems.AddRange(new List<UmbrellaItem>
        {
            new UmbrellaItem("GASH Blue Umbrella", "Stay out of the acid rain, now in blue."){ ModelItemID = "p_amb_brolly_01" },
            new UmbrellaItem("GASH Black Umbrella", "Stay out of the acid rain in fashionable black.") { ModelItemID = "p_amb_brolly_01_s" },
        });
        PossibleItems.BinocularsItems.AddRange(new List<BinocularsItem> {
            new BinocularsItem("SCHEISS BS Binoculars","Not just for peeping toms. Basic and Trusted.") {
                ModelItemID = "prop_binoc_01",HasThermalVision = false,HasNightVision = false, MinFOV = 15f,MidFOV = 35f,MaxFOV = 55f, FindPercentage = 1  },
            new BinocularsItem("SCHEISS AS Binoculars","Need to spy on a spouse or loved one? Now with more ZOOM!") {
                ModelItemID = "prop_binoc_01",HasThermalVision = false,HasNightVision = false, MinFOV = 12f,MidFOV = 20f,MaxFOV = 50f, FindPercentage = 1  },
            new BinocularsItem("SCHEISS DS Binoculars","Need to spy on spouse or loved one, but in the dark? We have you covered!") {
                ModelItemID = "prop_binoc_01",HasThermalVision = false,HasNightVision = true, MinFOV = 10f,MidFOV = 20f,MaxFOV = 50f, FindPercentage = 1  },
            new BinocularsItem("SCHEISS RP Binoculars","All the bells and whistles. They will never see you coming!") {
                ModelItemID = "prop_binoc_01",HasThermalVision = true,HasNightVision = true, MinFOV = 8f,MidFOV = 20f,MaxFOV = 50f, FindPercentage = 1  },
        });
    }
    private void DefaultConfig_Vehicles()
    {
        PossibleItems.VehicleItems.AddRange(new List<VehicleItem> {
            //Cars & Motorcycles
            new VehicleItem("Albany Alpha", "Blending modern performance and design with the classic luxury styling of a stately car, the Alpha is sleek, sexy and handles so well you'll forget you're driving it. Which could be a problem at 150 mph...", true, ItemType.Vehicles) { ModelItemID = "alpha" },
            new VehicleItem("Albany Roosevelt","Party like it's the Prohibition era in this armored 1920s limousine. Perfect for a gangster and his moll on their first date or their last. Let the Valentine's Day massacres commence.", true, ItemType.Vehicles) { ModelItemID = "btype" },
            new VehicleItem("Albany Fränken Stange","The unlikely product of Albany's design team leafing through a vintage car magazine while in the depths of a masculine overdose. The Franken Stange will make you the envy of goths, emo hipsters and vampire wannabes everywhere. Don't be fooled by what's left of its old world charm; the steering linkage may be from 1910, but the engine has just enough horsepower to tear itself (and you) to pieces at the first bump in the road.", true, ItemType.Vehicles) { ModelItemID = "btype2" },
            new VehicleItem("Albany Roosevelt Valor","They don't make them like they used to, which is a good thing because here at Albany we've completely run out of ideas. Lovingly remodelled, with room for a new suite of personal modifications, the latest edition of our classic Roosevelt represents a new height of criminal refinement, taking you back to the golden age of fraud, racketeering and murder when all you had to worry about were a few charges of tax evasion.", true, ItemType.Vehicles) { ModelItemID = "btype3" },
            new VehicleItem("Albany Buccaneer","With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", ItemType.Vehicles) { ModelItemID = "buccaneer" },
            new VehicleItem("Albany Buccaneer Custom","With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", true, ItemType.Vehicles) { ModelItemID = "buccaneer2" },
            new VehicleItem("Albany Cavalcade","You could scarcely cross the street without getting mown down by a soccer mom or drug dealer in one of these during the early 2000s. The glory days of the excessively-large, gas-guzzling SUV might be over, but the Cavalcade takes no prisoners.", ItemType.Vehicles) { ModelItemID = "cavalcade" },
            new VehicleItem("Albany Cavalcade 2","The old man luxury automobile, but once you sit inside this comfy car that steers like a boat, you'll know why your old man often fell asleep at the wheel.", ItemType.Vehicles) { ModelItemID = "cavalcade2" },
            new VehicleItem("Albany Emperor", ItemType.Vehicles) { ModelItemID = "emperor" },
            new VehicleItem("Albany Emperor 2", ItemType.Vehicles) { ModelItemID = "emperor2" },
            new VehicleItem("Albany Emperor 3", ItemType.Vehicles) { ModelItemID = "emperor3" },
            new VehicleItem("Albany Hermes", true, ItemType.Vehicles) { ModelItemID = "hermes" },
            new VehicleItem("Albany Lurcher", true, ItemType.Vehicles) { ModelItemID = "lurcher" },
            new VehicleItem("Albany Manana", ItemType.Vehicles) { ModelItemID = "manana" },
            new VehicleItem("Albany Manana Custom", true, ItemType.Vehicles) { ModelItemID = "manana2" },
            new VehicleItem("Albany Primo", ItemType.Vehicles) { ModelItemID = "primo" },
            new VehicleItem("Albany Primo Custom", true, ItemType.Vehicles) { ModelItemID = "primo2" },
            new VehicleItem("Albany Virgo", true, ItemType.Vehicles) { ModelItemID = "virgo" },
            new VehicleItem("Albany V-STR", true, ItemType.Vehicles) { ModelItemID = "vstr" },
            new VehicleItem("Albany Washington", ItemType.Vehicles) { ModelItemID = "washington" },
            new VehicleItem("Annis Elegy Retro Custom", true, ItemType.Vehicles) { ModelItemID = "elegy" },
            new VehicleItem("Annis Elegy RH8", ItemType.Vehicles) { ModelItemID = "elegy2" },
            new VehicleItem("Annis Euros", true, ItemType.Vehicles) { ModelItemID = "Euros" },
            new VehicleItem("Annis Hellion", true, ItemType.Vehicles) { ModelItemID = "hellion" },
            new VehicleItem("Annis RE-7B", true, ItemType.Vehicles) { ModelItemID = "le7b" },
            new VehicleItem("Annis Remus", true, ItemType.Vehicles) { ModelItemID = "remus" },
            new VehicleItem("Annis S80RR", true, ItemType.Vehicles) { ModelItemID = "s80" },
            new VehicleItem("Annis Savestra", true, ItemType.Vehicles) { ModelItemID = "savestra" },
            new VehicleItem("Annis ZR350", true, ItemType.Vehicles) { ModelItemID = "zr350" },
            new VehicleItem("Annis Apocalypse ZR380", true, ItemType.Vehicles) { ModelItemID = "zr380" },
            new VehicleItem("Annis Future Shock ZR380", true, ItemType.Vehicles) { ModelItemID = "zr3802" },
            new VehicleItem("Annis Nightmare ZR380", true, ItemType.Vehicles) { ModelItemID = "zr3803" },
            new VehicleItem("Benefactor Apocalypse Bruiser", true, ItemType.Vehicles) { ModelItemID = "bruiser" },
            new VehicleItem("Benefactor Future Shock Bruiser", true, ItemType.Vehicles) { ModelItemID = "bruiser2" },
            new VehicleItem("Benefactor Nightmare Bruiser", true, ItemType.Vehicles) { ModelItemID = "bruiser3" },
            new VehicleItem("Benefactor Dubsta", ItemType.Vehicles) { ModelItemID = "dubsta" },
            new VehicleItem("Benefactor Dubsta 2", ItemType.Vehicles) { ModelItemID = "dubsta2" },
            new VehicleItem("Benefactor Dubsta 6x6", true, ItemType.Vehicles) { ModelItemID = "dubsta3" },
            new VehicleItem("Benefactor Feltzer", ItemType.Vehicles) { ModelItemID = "feltzer2" },
            new VehicleItem("Benefactor Stirling GT", true, ItemType.Vehicles) { ModelItemID = "feltzer3" },
            new VehicleItem("Benefactor Glendale", true, ItemType.Vehicles) { ModelItemID = "glendale" },
            new VehicleItem("Benefactor Glendale Custom", true, ItemType.Vehicles) { ModelItemID = "glendale2" },
            new VehicleItem("Benefactor Turreted Limo", true, ItemType.Vehicles) { ModelItemID = "limo2" },
            new VehicleItem("Benefactor BR8", true, ItemType.Vehicles) { ModelItemID = "openwheel1" },
            new VehicleItem("Benefactor Panto", true, ItemType.Vehicles) { ModelItemID = "panto" },
            new VehicleItem("Benefactor Schafter", "Good-looking yet utilitarian, sexy yet asexual, slender yet terrifyingly powerful, the Schafter is German engineering at its very finest.", ItemType.Vehicles) { ModelItemID = "schafter2" },
            new VehicleItem("Benefactor Schafter V12", "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has a V12 engine.", true, ItemType.Vehicles) { ModelItemID = "schafter3" },
            new VehicleItem("Benefactor Schafter LWB", "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has an extended wheelbase.", true, ItemType.Vehicles) { ModelItemID = "schafter4" },
            new VehicleItem("Benefactor Schafter V12 (Armored)", true, ItemType.Vehicles) { ModelItemID = "schafter5" },
            new VehicleItem("Benefactor Schafter LWB (Armored)", true, ItemType.Vehicles) { ModelItemID = "schafter6" },
            new VehicleItem("Benefactor Schwartzer", "Say what you will about the Germans - they know luxury. And their economy is the only one worth a crap in Europe. This model has all kinds of extras - too many to list for legal reasons.", ItemType.Vehicles) { ModelItemID = "schwarzer" },
            new VehicleItem("Benefactor Serrano", "Fun fact: what's the fastest growing market in the American auto industry? That's right! Compact SUVs! And do you know why? That's right! Neither do we! And is that a good enough reason to buy one? That's right! It had better be!", ItemType.Vehicles) { ModelItemID = "serrano" },
            new VehicleItem("Benefactor Surano", "This is luxury reasserted. Right in your neighbour's face. Boom. You like that. That's right, you are better than him, and you could have his wife if you wanted. Try it on with her as soon as she sees this ride. You'll be a double benefactor.", ItemType.Vehicles) { ModelItemID = "Surano" },
            new VehicleItem("Benefactor XLS", true, ItemType.Vehicles) { ModelItemID = "xls" },
            new VehicleItem("Benefactor XLS (Armored)", true, ItemType.Vehicles) { ModelItemID = "xls2" },
            new VehicleItem("Benefactor Krieger", true, ItemType.Vehicles) { ModelItemID = "krieger" },
            new VehicleItem("Benefactor Schlagen GT", true, ItemType.Vehicles) { ModelItemID = "schlagen" },
            new VehicleItem("Benefactor Streiter", true, ItemType.Vehicles) { ModelItemID = "streiter" },
            new VehicleItem("Benefactor Terrorbyte", true, ItemType.Vehicles) { ModelItemID = "terbyte" },
            new VehicleItem("BF Injection", ItemType.Vehicles) { ModelItemID = "BfInjection" },
            new VehicleItem("BF Bifta", true, ItemType.Vehicles) { ModelItemID = "bifta" },
            new VehicleItem("BF Club", true, ItemType.Vehicles) { ModelItemID = "club" },
            new VehicleItem("BF Dune Buggy", ItemType.Vehicles) { ModelItemID = "dune" },
            new VehicleItem("BF Dune FAV", true, ItemType.Vehicles) { ModelItemID = "dune3" },
            new VehicleItem("BF Raptor", true, ItemType.Vehicles) { ModelItemID = "raptor" },
            new VehicleItem("BF Surfer", ItemType.Vehicles) { ModelItemID = "SURFER" },
            new VehicleItem("BF Surfer", ItemType.Vehicles) { ModelItemID = "Surfer2" },
            new VehicleItem("BF Weevil", true, ItemType.Vehicles) { ModelItemID = "weevil" },
            new VehicleItem("Bollokan Prairie", ItemType.Vehicles) { ModelItemID = "prairie" },
            new VehicleItem("Bravado Banshee", ItemType.Vehicles) { ModelItemID = "banshee" },
            new VehicleItem("Bravado Banshee 900R", true, ItemType.Vehicles) { ModelItemID = "banshee2" },
            new VehicleItem("Bravado Bison", ItemType.Vehicles) { ModelItemID = "bison" },
            new VehicleItem("Bravado Bison 2", ItemType.Vehicles) { ModelItemID = "Bison2" },
            new VehicleItem("Bravado Bison 3", ItemType.Vehicles) { ModelItemID = "Bison3" },
            new VehicleItem("Bravado Buffalo", ItemType.Vehicles) { ModelItemID = "buffalo" },
            new VehicleItem("Bravado Buffalo S", ItemType.Vehicles) { ModelItemID = "buffalo2" },
            new VehicleItem("Bravado Sprunk Buffalo", ItemType.Vehicles) { ModelItemID = "buffalo3" },
            new VehicleItem("Bravado Duneloader", ItemType.Vehicles) { ModelItemID = "dloader" },
            new VehicleItem("Bravado Gauntlet", ItemType.Vehicles) { ModelItemID = "Gauntlet" },
            new VehicleItem("Bravado Redwood Gauntlet", ItemType.Vehicles) { ModelItemID = "gauntlet2" },
            new VehicleItem("Bravado Gauntlet Classic", true, ItemType.Vehicles) { ModelItemID = "gauntlet3" },
            new VehicleItem("Bravado Gauntlet Hellfire", true, ItemType.Vehicles) { ModelItemID = "gauntlet4" },
            new VehicleItem("Bravado Gauntlet Classic Custom", true, ItemType.Vehicles) { ModelItemID = "gauntlet5" },
            new VehicleItem("Bravado Gresley", ItemType.Vehicles) { ModelItemID = "gresley" },
            new VehicleItem("Bravado Half-track", true, ItemType.Vehicles) { ModelItemID = "halftrack" },
            new VehicleItem("Bravado Apocalypse Sasquatch", true, ItemType.Vehicles) { ModelItemID = "monster3" },
            new VehicleItem("Bravado Future Shock Sasquatch", true, ItemType.Vehicles) { ModelItemID = "monster4" },
            new VehicleItem("Bravado Nightmare Sasquatch", true, ItemType.Vehicles) { ModelItemID = "monster5" },
            new VehicleItem("Bravado Paradise", true, ItemType.Vehicles) { ModelItemID = "paradise" },
            new VehicleItem("Bravado Rat-Truck", true, ItemType.Vehicles) { ModelItemID = "ratloader2" },
            new VehicleItem("Bravado Rumpo", ItemType.Vehicles) { ModelItemID = "rumpo" },
            new VehicleItem("Bravado Rumpo 2", ItemType.Vehicles) { ModelItemID = "rumpo2" },
            new VehicleItem("Bravado Rumpo Custom", true, ItemType.Vehicles) { ModelItemID = "rumpo3" },
            new VehicleItem("Bravado Verlierer", true, ItemType.Vehicles) { ModelItemID = "verlierer2" },
            new VehicleItem("Bravado Youga", ItemType.Vehicles) { ModelItemID = "youga" },
            new VehicleItem("Bravado Youga Classic", true, ItemType.Vehicles) { ModelItemID = "youga2" },
            new VehicleItem("Bravado Youga Classic 4x4", true, ItemType.Vehicles) { ModelItemID = "youga3" },
            new VehicleItem("Brute Boxville", ItemType.Vehicles) { ModelItemID = "boxville" },
            new VehicleItem("Brute Boxville 3", ItemType.Vehicles) { ModelItemID = "boxville3" },
            new VehicleItem("Brute Boxville 4", true, ItemType.Vehicles) { ModelItemID = "boxville4" },
            new VehicleItem("Brute Camper", ItemType.Vehicles) { ModelItemID = "CAMPER" },
            new VehicleItem("Brute Pony", ItemType.Vehicles) { ModelItemID = "pony" },
            new VehicleItem("Brute Pony 2", ItemType.Vehicles) { ModelItemID = "pony2" },
            new VehicleItem("Brute Stockade", ItemType.Vehicles) { ModelItemID = "stockade" },
            new VehicleItem("Brute Stockade 3", ItemType.Vehicles) { ModelItemID = "stockade3" },
            new VehicleItem("Brute Tipper", ItemType.Vehicles) { ModelItemID = "TipTruck" },
            new VehicleItem("Canis Bodhi", ItemType.Vehicles) { ModelItemID = "Bodhi2" },
            new VehicleItem("Canis Crusader", ItemType.Vehicles) { ModelItemID = "CRUSADER" },
            new VehicleItem("Canis Freecrawler", true, ItemType.Vehicles) { ModelItemID = "freecrawler" },
            new VehicleItem("Canis Kalahari", true, ItemType.Vehicles) { ModelItemID = "kalahari" },
            new VehicleItem("Canis Kamacho", true, ItemType.Vehicles) { ModelItemID = "kamacho" },
            new VehicleItem("Canis Mesa", ItemType.Vehicles) { ModelItemID = "MESA" },
            new VehicleItem("Canis Mesa 2", ItemType.Vehicles) { ModelItemID = "mesa2" },
            new VehicleItem("Canis Mesa 3", ItemType.Vehicles) { ModelItemID = "MESA3" },
            new VehicleItem("Canis Seminole", ItemType.Vehicles) { ModelItemID = "Seminole" },
            new VehicleItem("Canis Seminole Frontier", true, ItemType.Vehicles) { ModelItemID = "seminole2" },
            new VehicleItem("Chariot Romero Hearse", ItemType.Vehicles) { ModelItemID = "romero" },
            new VehicleItem("Cheval Fugitive", ItemType.Vehicles) { ModelItemID = "fugitive" },
            new VehicleItem("Cheval Marshall", ItemType.Vehicles) { ModelItemID = "marshall" },
            new VehicleItem("Cheval Picador", ItemType.Vehicles) { ModelItemID = "picador" },
            new VehicleItem("Cheval Surge", ItemType.Vehicles) { ModelItemID = "surge" },
            new VehicleItem("Cheval Taipan", true, ItemType.Vehicles) { ModelItemID = "taipan" },
            new VehicleItem("Coil Brawler", true, ItemType.Vehicles) { ModelItemID = "brawler" },
            new VehicleItem("Coil Cyclone", true, ItemType.Vehicles) { ModelItemID = "cyclone" },
            new VehicleItem("Coil Raiden", true, ItemType.Vehicles) { ModelItemID = "raiden" },
            new VehicleItem("Coil Voltic", ItemType.Vehicles) { ModelItemID = "voltic" },
            new VehicleItem("Coil Rocket Voltic", true, ItemType.Vehicles) { ModelItemID = "voltic2" },
            new VehicleItem("Declasse Asea", ItemType.Vehicles) { ModelItemID = "asea" },
            new VehicleItem("Declasse Asea", ItemType.Vehicles) { ModelItemID = "asea2" },
            new VehicleItem("Declasse Apocalypse Brutus", true, ItemType.Vehicles) { ModelItemID = "brutus" },
            new VehicleItem("Declasse Future Shock Brutus", true, ItemType.Vehicles) { ModelItemID = "brutus2" },
            new VehicleItem("Declasse Nightmare Brutus", true, ItemType.Vehicles) { ModelItemID = "brutus3" },
            new VehicleItem("Declasse Burrito", ItemType.Vehicles) { ModelItemID = "Burrito" },
            new VehicleItem("Declasse Bugstars Burrito", ItemType.Vehicles) { ModelItemID = "burrito2" },
            new VehicleItem("Declasse Burrito 3", ItemType.Vehicles) { ModelItemID = "burrito3" },
            new VehicleItem("Declasse Burrito 4", ItemType.Vehicles) { ModelItemID = "Burrito4" },
            new VehicleItem("Declasse Burrito 5", ItemType.Vehicles) { ModelItemID = "burrito5" },
            new VehicleItem("Declasse Gang Burrito", ItemType.Vehicles) { ModelItemID = "gburrito" },
            new VehicleItem("Declasse Gang Burrito 2", true, ItemType.Vehicles) { ModelItemID = "gburrito2" },
            new VehicleItem("Declasse Granger", ItemType.Vehicles) { ModelItemID = "GRANGER" },
            new VehicleItem("Declasse Hotring Sabre", true, ItemType.Vehicles) { ModelItemID = "hotring" },
            new VehicleItem("Declasse Impaler", true, ItemType.Vehicles) { ModelItemID = "impaler" },
            new VehicleItem("Declasse Apocalypse Impaler", true, ItemType.Vehicles) { ModelItemID = "impaler2" },
            new VehicleItem("Declasse Future Shock Impaler", true, ItemType.Vehicles) { ModelItemID = "impaler3" },
            new VehicleItem("Declasse Nightmare Impaler", true, ItemType.Vehicles) { ModelItemID = "impaler4" },
            new VehicleItem("Declasse Lifeguard", ItemType.Vehicles) { ModelItemID = "lguard" },
            new VehicleItem("Declasse Mamba", true, ItemType.Vehicles) { ModelItemID = "mamba" },
            new VehicleItem("Declasse Moonbeam", true, ItemType.Vehicles) { ModelItemID = "moonbeam" },
            new VehicleItem("Declasse Moonbeam Custom", true, ItemType.Vehicles) { ModelItemID = "moonbeam2" },
            new VehicleItem("Declasse DR1", true, ItemType.Vehicles) { ModelItemID = "openwheel2" },
            new VehicleItem("Declasse Premier", ItemType.Vehicles) { ModelItemID = "premier" },
            new VehicleItem("Declasse Rancher XL", ItemType.Vehicles) { ModelItemID = "RancherXL" },
            new VehicleItem("Declasse Rancher XL 2", ItemType.Vehicles) { ModelItemID = "rancherxl2" },
            new VehicleItem("Declasse Rhapsody", true, ItemType.Vehicles) { ModelItemID = "rhapsody" },
            new VehicleItem("Declasse Sabre Turbo", ItemType.Vehicles) { ModelItemID = "sabregt" },
            new VehicleItem("Declasse Sabre Turbo Custom", true, ItemType.Vehicles) { ModelItemID = "sabregt2" },
            new VehicleItem("Declasse Scramjet", true, ItemType.Vehicles) { ModelItemID = "scramjet" },
            new VehicleItem("Declasse Stallion", ItemType.Vehicles) { ModelItemID = "stalion" },
            new VehicleItem("Declasse Burger Shot Stallion", ItemType.Vehicles) { ModelItemID = "stalion2" },
            new VehicleItem("Declasse Tampa", true, ItemType.Vehicles) { ModelItemID = "tampa" },
            new VehicleItem("Declasse Drift Tampa", true, ItemType.Vehicles) { ModelItemID = "tampa2" },
            new VehicleItem("Declasse Weaponized Tampa", true, ItemType.Vehicles) { ModelItemID = "tampa3" },
            new VehicleItem("Declasse Tornado", ItemType.Vehicles) { ModelItemID = "tornado" },
            new VehicleItem("Declasse Tornado 2", ItemType.Vehicles) { ModelItemID = "tornado2" },
            new VehicleItem("Declasse Tornado 3", ItemType.Vehicles) { ModelItemID = "tornado3" },
            new VehicleItem("Declasse Tornado 4", ItemType.Vehicles) { ModelItemID = "tornado4" },
            new VehicleItem("Declasse Tornado Custom", true, ItemType.Vehicles) { ModelItemID = "tornado5" },
            new VehicleItem("Declasse Tornado Rat Rod", true, ItemType.Vehicles) { ModelItemID = "tornado6" },
            new VehicleItem("Declasse Tulip", true, ItemType.Vehicles) { ModelItemID = "tulip" },
            new VehicleItem("Declasse Vamos", true, ItemType.Vehicles) { ModelItemID = "vamos" },
            new VehicleItem("Declasse Vigero", ItemType.Vehicles) { ModelItemID = "vigero" },
            new VehicleItem("Declasse Voodoo Custom", true, ItemType.Vehicles) { ModelItemID = "voodoo" },
            new VehicleItem("Declasse Voodoo", ItemType.Vehicles) { ModelItemID = "voodoo2" },
            new VehicleItem("Declasse Yosemite", true, ItemType.Vehicles) { ModelItemID = "yosemite" },
            new VehicleItem("Declasse Drift Yosemite", true, ItemType.Vehicles) { ModelItemID = "yosemite2" },
            new VehicleItem("Declasse Yosemite Rancher", true, ItemType.Vehicles) { ModelItemID = "yosemite3" },
            new VehicleItem("Dewbauchee Exemplar", ItemType.Vehicles) { ModelItemID = "exemplar" },
            new VehicleItem("Dewbauchee JB 700", ItemType.Vehicles) { ModelItemID = "jb700" },
            new VehicleItem("Dewbauchee JB 700W", true, ItemType.Vehicles) { ModelItemID = "jb7002" },
            new VehicleItem("Dewbauchee Massacro", true, ItemType.Vehicles) { ModelItemID = "massacro" },
            new VehicleItem("Dewbauchee Massacro (Racecar)", true, ItemType.Vehicles) { ModelItemID = "massacro2" },
            new VehicleItem("Dewbauchee Rapid GT", ItemType.Vehicles) { ModelItemID = "RapidGT" },
            new VehicleItem("Dewbauchee Rapid GT 2", ItemType.Vehicles) { ModelItemID = "RapidGT2" },
            new VehicleItem("Dewbauchee Rapid GT Classic", true, ItemType.Vehicles) { ModelItemID = "rapidgt3" },
            new VehicleItem("Dewbauchee Seven-70", true, ItemType.Vehicles) { ModelItemID = "SEVEN70" },
            new VehicleItem("Dewbauchee Specter", true, ItemType.Vehicles) { ModelItemID = "SPECTER" },
            new VehicleItem("Dewbauchee Specter Custom", true, ItemType.Vehicles) { ModelItemID = "SPECTER2" },
            new VehicleItem("Dewbauchee Vagner", true, ItemType.Vehicles) { ModelItemID = "vagner" },
            new VehicleItem("Dinka Akuma", ItemType.Vehicles) { ModelItemID = "akuma" },
            new VehicleItem("Dinka Blista", ItemType.Vehicles) { ModelItemID = "blista" },
            new VehicleItem("Dinka Blista Compact", ItemType.Vehicles) { ModelItemID = "blista2" },
            new VehicleItem("Dinka Go Go Monkey Blista", ItemType.Vehicles) { ModelItemID = "blista3" },
            new VehicleItem("Dinka Double-T", ItemType.Vehicles) { ModelItemID = "double" },
            new VehicleItem("Dinka Enduro", true, ItemType.Vehicles) { ModelItemID = "enduro" },
            new VehicleItem("Dinka Jester", true, ItemType.Vehicles) { ModelItemID = "jester" },
            new VehicleItem("Dinka Jester (Racecar)", true, ItemType.Vehicles) { ModelItemID = "jester2" },
            new VehicleItem("Dinka Jester Classic", true, ItemType.Vehicles) { ModelItemID = "jester3" },
            new VehicleItem("Dinka Jester RR", true, ItemType.Vehicles) { ModelItemID = "jester4" },
            new VehicleItem("Dinka Blista Kanjo", true, ItemType.Vehicles) { ModelItemID = "kanjo" },
            new VehicleItem("Dinka RT3000", true, ItemType.Vehicles) { ModelItemID = "rt3000" },
            new VehicleItem("Dinka Sugoi", true, ItemType.Vehicles) { ModelItemID = "Sugoi" },
            new VehicleItem("Dinka Thrust", true, ItemType.Vehicles) { ModelItemID = "thrust" },
            new VehicleItem("Dinka Verus", true, ItemType.Vehicles) { ModelItemID = "verus" },
            new VehicleItem("Dinka Veto Classic", true, ItemType.Vehicles) { ModelItemID = "veto" },
            new VehicleItem("Dinka Veto Modern", true, ItemType.Vehicles) { ModelItemID = "veto2" },
            new VehicleItem("Dinka Vindicator", true, ItemType.Vehicles) { ModelItemID = "vindicator" },
            new VehicleItem("Dundreary Landstalker", ItemType.Vehicles) { ModelItemID = "landstalker" },
            new VehicleItem("Dundreary Landstalker XL", true, ItemType.Vehicles) { ModelItemID = "landstalker2" },
            new VehicleItem("Dundreary Regina", ItemType.Vehicles) { ModelItemID = "regina" },
            new VehicleItem("Dundreary Stretch", ItemType.Vehicles) { ModelItemID = "stretch" },
            new VehicleItem("Dundreary Virgo Classic Custom", true, ItemType.Vehicles) { ModelItemID = "virgo2" },
            new VehicleItem("Dundreary Virgo Classic", true, ItemType.Vehicles) { ModelItemID = "virgo3" },
            new VehicleItem("Emperor Habanero", ItemType.Vehicles) { ModelItemID = "habanero" },
            new VehicleItem("Emperor ETR1", true, ItemType.Vehicles) { ModelItemID = "sheava" },
            new VehicleItem("Emperor Vectre", true, ItemType.Vehicles) { ModelItemID = "vectre" },
            new VehicleItem("Enus Cognoscenti 55", true, ItemType.Vehicles) { ModelItemID = "cog55" },
            new VehicleItem("Enus Cognoscenti 55 (Armored)", true, ItemType.Vehicles) { ModelItemID = "cog552" },
            new VehicleItem("Enus Cognoscenti Cabrio", ItemType.Vehicles) { ModelItemID = "cogcabrio" },
            new VehicleItem("Enus Cognoscenti", true, ItemType.Vehicles) { ModelItemID = "cognoscenti" },
            new VehicleItem("Enus Cognoscenti (Armored)", true, ItemType.Vehicles) { ModelItemID = "cognoscenti2" },
            new VehicleItem("Enus Huntley S", true, ItemType.Vehicles) { ModelItemID = "huntley" },
            new VehicleItem("Enus Paragon R", true, ItemType.Vehicles) { ModelItemID = "paragon" },
            new VehicleItem("Enus Paragon R (Armored)", true, ItemType.Vehicles) { ModelItemID = "paragon2" },
            new VehicleItem("Enus Stafford", true, ItemType.Vehicles) { ModelItemID = "stafford" },
            new VehicleItem("Enus Super Diamond", ItemType.Vehicles) { ModelItemID = "superd" },
            new VehicleItem("Enus Windsor", true, ItemType.Vehicles) { ModelItemID = "windsor" },
            new VehicleItem("Enus Windsor Drop", true, ItemType.Vehicles) { ModelItemID = "windsor2" },
            new VehicleItem("Fathom FQ 2", ItemType.Vehicles) { ModelItemID = "fq2" },
            new VehicleItem("Gallivanter Baller", ItemType.Vehicles) { ModelItemID = "Baller" },
            new VehicleItem("Gallivanter Baller 2", ItemType.Vehicles) { ModelItemID = "baller2" },
            new VehicleItem("Gallivanter Baller LE", true, ItemType.Vehicles) { ModelItemID = "baller3" },
            new VehicleItem("Gallivanter Baller LE LWB", true, ItemType.Vehicles) { ModelItemID = "baller4" },
            new VehicleItem("Gallivanter Baller LE (Armored)", true, ItemType.Vehicles) { ModelItemID = "baller5" },
            new VehicleItem("Gallivanter Baller LE LWB (Armored)", true, ItemType.Vehicles) { ModelItemID = "baller6" },
            new VehicleItem("Grotti Bestia GTS", true, ItemType.Vehicles) { ModelItemID = "bestiagts" },
            new VehicleItem("Grotti Brioso R/A", true, ItemType.Vehicles) { ModelItemID = "brioso" },
            new VehicleItem("Grotti Brioso 300", true, ItemType.Vehicles) { ModelItemID = "brioso2" },
            new VehicleItem("Grotti Carbonizzare", ItemType.Vehicles) { ModelItemID = "carbonizzare" },
            new VehicleItem("Grotti Cheetah", ItemType.Vehicles) { ModelItemID = "cheetah" },
            new VehicleItem("Grotti Cheetah Classic", true, ItemType.Vehicles) { ModelItemID = "cheetah2" },
            new VehicleItem("Grotti Furia", true, ItemType.Vehicles) { ModelItemID = "furia" },
            new VehicleItem("Grotti GT500", true, ItemType.Vehicles) { ModelItemID = "gt500" },
            new VehicleItem("Grotti Itali GTO", true, ItemType.Vehicles) { ModelItemID = "italigto" },
            new VehicleItem("Grotti Itali RSX", true, ItemType.Vehicles) { ModelItemID = "italirsx" },
            new VehicleItem("Grotti X80 Proto", true, ItemType.Vehicles) { ModelItemID = "prototipo" },
            new VehicleItem("Grotti Stinger", ItemType.Vehicles) { ModelItemID = "stinger" },
            new VehicleItem("Grotti Stinger GT", ItemType.Vehicles) { ModelItemID = "stingergt" },
            new VehicleItem("Grotti Turismo Classic", true, ItemType.Vehicles) { ModelItemID = "turismo2" },
            new VehicleItem("Grotti Turismo R", true, ItemType.Vehicles) { ModelItemID = "turismor" },
            new VehicleItem("Grotti Visione", true, ItemType.Vehicles) { ModelItemID = "visione" },
            new VehicleItem("Hijak Khamelion", ItemType.Vehicles) { ModelItemID = "khamelion" },
            new VehicleItem("Hijak Ruston", true, ItemType.Vehicles) { ModelItemID = "ruston" },
            new VehicleItem("HVY Barracks Semi", ItemType.Vehicles) { ModelItemID = "BARRACKS2" },
            new VehicleItem("HVY Biff", ItemType.Vehicles) { ModelItemID = "Biff" },
            new VehicleItem("HVY Dozer", ItemType.Vehicles) { ModelItemID = "bulldozer" },
            new VehicleItem("HVY Cutter", ItemType.Vehicles) { ModelItemID = "cutter" },
            new VehicleItem("HVY Dump", ItemType.Vehicles) { ModelItemID = "dump" },
            new VehicleItem("HVY Forklift", ItemType.Vehicles) { ModelItemID = "FORKLIFT" },
            new VehicleItem("HVY Insurgent Pick-Up", true, ItemType.Vehicles) { ModelItemID = "insurgent" },
            new VehicleItem("HVY Insurgent", true, ItemType.Vehicles) { ModelItemID = "insurgent2" },
            new VehicleItem("HVY Insurgent Pick-Up Custom", true, ItemType.Vehicles) { ModelItemID = "insurgent3" },
            new VehicleItem("HVY Menacer", true, ItemType.Vehicles) { ModelItemID = "menacer" },
            new VehicleItem("HVY Mixer", ItemType.Vehicles) { ModelItemID = "Mixer" },
            new VehicleItem("HVY Mixer 2", ItemType.Vehicles) { ModelItemID = "Mixer2" },
            new VehicleItem("HVY Nightshark", true, ItemType.Vehicles) { ModelItemID = "nightshark" },
            new VehicleItem("HVY Apocalypse Scarab", true, ItemType.Vehicles) { ModelItemID = "scarab" },
            new VehicleItem("HVY Future Shock Scarab", true, ItemType.Vehicles) { ModelItemID = "scarab2" },
            new VehicleItem("HVY Nightmare Scarab", true, ItemType.Vehicles) { ModelItemID = "scarab3" },
            new VehicleItem("Imponte Deluxo", true, ItemType.Vehicles) { ModelItemID = "deluxo" },
            new VehicleItem("Imponte Dukes", ItemType.Vehicles) { ModelItemID = "dukes" },
            new VehicleItem("Imponte Duke O'Death", ItemType.Vehicles) { ModelItemID = "dukes2" },
            new VehicleItem("Imponte Beater Dukes", true, ItemType.Vehicles) { ModelItemID = "dukes3" },
            new VehicleItem("Imponte Nightshade", true, ItemType.Vehicles) { ModelItemID = "nightshade" },
            new VehicleItem("Imponte Phoenix", ItemType.Vehicles) { ModelItemID = "Phoenix" },
            new VehicleItem("Imponte Ruiner", ItemType.Vehicles) { ModelItemID = "ruiner" },
            new VehicleItem("Imponte Ruiner 2000", true, ItemType.Vehicles) { ModelItemID = "ruiner2" },
            new VehicleItem("Imponte Ruiner", true, ItemType.Vehicles) { ModelItemID = "ruiner3" },
            new VehicleItem("Invetero Coquette", ItemType.Vehicles) { ModelItemID = "coquette" },
            new VehicleItem("Invetero Coquette Classic", true, ItemType.Vehicles) { ModelItemID = "coquette2" },
            new VehicleItem("Invetero Coquette BlackFin", true, ItemType.Vehicles) { ModelItemID = "coquette3" },
            new VehicleItem("Invetero Coquette D10", true, ItemType.Vehicles) { ModelItemID = "coquette4" },
            new VehicleItem("JoBuilt Hauler", ItemType.Vehicles) { ModelItemID = "Hauler" },
            new VehicleItem("JoBuilt Hauler Custom", true, ItemType.Vehicles) { ModelItemID = "Hauler2" },
            new VehicleItem("JoBuilt Phantom", ItemType.Vehicles) { ModelItemID = "Phantom" },
            new VehicleItem("JoBuilt Phantom Wedge", true, ItemType.Vehicles) { ModelItemID = "phantom2" },
            new VehicleItem("JoBuilt Phantom Custom", true, ItemType.Vehicles) { ModelItemID = "phantom3" },
            new VehicleItem("JoBuilt Rubble", ItemType.Vehicles) { ModelItemID = "Rubble" },
            new VehicleItem("Karin Asterope", ItemType.Vehicles) { ModelItemID = "asterope" },
            new VehicleItem("Karin BeeJay XL", ItemType.Vehicles) { ModelItemID = "BjXL" },
            new VehicleItem("Karin Calico GTF", true, ItemType.Vehicles) { ModelItemID = "calico" },
            new VehicleItem("Karin Dilettante", ItemType.Vehicles) { ModelItemID = "dilettante" },
            new VehicleItem("Karin Dilettante 2", ItemType.Vehicles) { ModelItemID = "dilettante2" },
            new VehicleItem("Karin Everon", true, ItemType.Vehicles) { ModelItemID = "everon" },
            new VehicleItem("Karin Futo", ItemType.Vehicles) { ModelItemID = "futo" },
            new VehicleItem("Karin Futo GTX", true, ItemType.Vehicles) { ModelItemID = "futo2" },
            new VehicleItem("Karin Intruder", ItemType.Vehicles) { ModelItemID = "intruder" },
            new VehicleItem("Karin Kuruma", true, ItemType.Vehicles) { ModelItemID = "kuruma" },
            new VehicleItem("Karin Kuruma (armored)", true, ItemType.Vehicles) { ModelItemID = "kuruma2" },
            new VehicleItem("Karin Previon", true, ItemType.Vehicles) { ModelItemID = "previon" },
            new VehicleItem("Karin Rusty Rebel", ItemType.Vehicles) { ModelItemID = "Rebel" },
            new VehicleItem("Karin Rebel", ItemType.Vehicles) { ModelItemID = "rebel2" },
            new VehicleItem("Karin Sultan", ItemType.Vehicles) { ModelItemID = "sultan" },
            new VehicleItem("Karin Sultan Classic", true, ItemType.Vehicles) { ModelItemID = "sultan2" },
            new VehicleItem("Karin Sultan RS Classic", true, ItemType.Vehicles) { ModelItemID = "sultan3" },
            new VehicleItem("Karin Sultan RS", true, ItemType.Vehicles) { ModelItemID = "sultanrs" },
            new VehicleItem("Karin Technical", true, ItemType.Vehicles) { ModelItemID = "technical" },
            new VehicleItem("Karin Technical Custom", true, ItemType.Vehicles) { ModelItemID = "technical3" },
            new VehicleItem("Karin 190z", true, ItemType.Vehicles) { ModelItemID = "z190" },
            new VehicleItem("Lampadati Casco", true, ItemType.Vehicles) { ModelItemID = "casco" },
            new VehicleItem("Lampadati Felon", ItemType.Vehicles) { ModelItemID = "felon" },
            new VehicleItem("Lampadati Felon GT", ItemType.Vehicles) { ModelItemID = "felon2" },
            new VehicleItem("Lampadati Furore GT", true, ItemType.Vehicles) { ModelItemID = "furoregt" },
            new VehicleItem("Lampadati Michelli GT", true, ItemType.Vehicles) { ModelItemID = "michelli" },
            new VehicleItem("Lampadati Pigalle", true, ItemType.Vehicles) { ModelItemID = "pigalle" },
            new VehicleItem("Lampadati Tropos Rallye", true, ItemType.Vehicles) { ModelItemID = "tropos" },
            new VehicleItem("Lampadati Komoda", true, ItemType.Vehicles) { ModelItemID = "komoda" },
            new VehicleItem("Lampadati Novak", true, ItemType.Vehicles) { ModelItemID = "Novak" },
            new VehicleItem("Lampadati Tigon", true, ItemType.Vehicles) { ModelItemID = "tigon" },
            new VehicleItem("Lampadati Viseris", true, ItemType.Vehicles) { ModelItemID = "viseris" },
            new VehicleItem("LCC Avarus", true, ItemType.Vehicles) { ModelItemID = "avarus" },
            new VehicleItem("LCC Hexer", ItemType.Vehicles) { ModelItemID = "hexer" },
            new VehicleItem("LCC Innovation", true, ItemType.Vehicles) { ModelItemID = "innovation" },
            new VehicleItem("LCC Sanctus", true, ItemType.Vehicles) { ModelItemID = "sanctus" },
            new VehicleItem("Maibatsu Manchez", true, ItemType.Vehicles) { ModelItemID = "manchez" },
            new VehicleItem("Maibatsu Manchez Scout", true, ItemType.Vehicles) { ModelItemID = "manchez2" },
            new VehicleItem("Maibatsu Mule", ItemType.Vehicles) { ModelItemID = "Mule" },
            new VehicleItem("Maibatsu Mule", ItemType.Vehicles) { ModelItemID = "Mule2" },
            new VehicleItem("Maibatsu Mule", true, ItemType.Vehicles) { ModelItemID = "Mule3" },
            new VehicleItem("Maibatsu Mule Custom", true, ItemType.Vehicles) { ModelItemID = "mule4" },
            new VehicleItem("Maibatsu Penumbra", ItemType.Vehicles) { ModelItemID = "penumbra" },
            new VehicleItem("Maibatsu Penumbra FF", true, ItemType.Vehicles) { ModelItemID = "penumbra2" },
            new VehicleItem("Maibatsu Sanchez Custom", ItemType.Vehicles) { ModelItemID = "Sanchez" },
            new VehicleItem("Maibatsu Sanchez", ItemType.Vehicles) { ModelItemID = "sanchez2" },
            new VehicleItem("Mammoth Patriot", ItemType.Vehicles) { ModelItemID = "patriot" },
            new VehicleItem("Mammoth Patriot Stretch", true, ItemType.Vehicles) { ModelItemID = "patriot2" },
            new VehicleItem("Mammoth Squaddie", true, ItemType.Vehicles) { ModelItemID = "squaddie" },
            new VehicleItem("Maxwell Asbo", true, ItemType.Vehicles) { ModelItemID = "asbo" },
            new VehicleItem("Maxwell Vagrant", true, ItemType.Vehicles) { ModelItemID = "vagrant" },
            new VehicleItem("MTL Brickade", true, ItemType.Vehicles) { ModelItemID = "brickade" },
            new VehicleItem("MTL Apocalypse Cerberus", true, ItemType.Vehicles) { ModelItemID = "cerberus" },
            new VehicleItem("MTL Future Shock Cerberus", true, ItemType.Vehicles) { ModelItemID = "cerberus2" },
            new VehicleItem("MTL Nightmare Cerberus", true, ItemType.Vehicles) { ModelItemID = "cerberus3" },
            new VehicleItem("MTL Fire Truck", ItemType.Vehicles) { ModelItemID = "firetruk" },
            new VehicleItem("MTL Flatbed", ItemType.Vehicles) { ModelItemID = "FLATBED" },
            new VehicleItem("MTL Packer", ItemType.Vehicles) { ModelItemID = "Packer" },
            new VehicleItem("MTL Pounder", ItemType.Vehicles) { ModelItemID = "Pounder" },
            new VehicleItem("MTL Pounder Custom", true, ItemType.Vehicles) { ModelItemID = "pounder2" },
            new VehicleItem("MTL Dune", true, ItemType.Vehicles) { ModelItemID = "rallytruck" },
            new VehicleItem("MTL Wastelander", true, ItemType.Vehicles) { ModelItemID = "wastelander" },
            new VehicleItem("Nagasaki BF400", true, ItemType.Vehicles) { ModelItemID = "bf400" },
            new VehicleItem("Nagasaki Blazer", ItemType.Vehicles) { ModelItemID = "blazer" },
            new VehicleItem("Nagasaki Blazer Lifeguard", ItemType.Vehicles) { ModelItemID = "blazer2" },
            new VehicleItem("Nagasaki Hot Rod Blazer", ItemType.Vehicles) { ModelItemID = "blazer3" },
            new VehicleItem("Nagasaki Street Blazer", true, ItemType.Vehicles) { ModelItemID = "blazer4" },
            new VehicleItem("Nagasaki Carbon RS", ItemType.Vehicles) { ModelItemID = "carbonrs" },
            new VehicleItem("Nagasaki Chimera", true, ItemType.Vehicles) { ModelItemID = "chimera" },
            new VehicleItem("Nagasaki Outlaw", true, ItemType.Vehicles) { ModelItemID = "outlaw" },
            new VehicleItem("Nagasaki Shotaro", true, ItemType.Vehicles) { ModelItemID = "shotaro" },
            new VehicleItem("Nagasaki Stryder", true, ItemType.Vehicles) { ModelItemID = "Stryder" },
            new VehicleItem("Obey 8F Drafter", true, ItemType.Vehicles) { ModelItemID = "drafter" },
            new VehicleItem("Obey 9F", ItemType.Vehicles) { ModelItemID = "ninef" },
            new VehicleItem("Obey 9F Cabrio", ItemType.Vehicles) { ModelItemID = "ninef2" },
            new VehicleItem("Obey Omnis", true, ItemType.Vehicles) { ModelItemID = "omnis" },
            new VehicleItem("Obey Rocoto", ItemType.Vehicles) { ModelItemID = "rocoto" },
            new VehicleItem("Obey Tailgater", ItemType.Vehicles) { ModelItemID = "tailgater" },
            new VehicleItem("Obey Tailgater S", true, ItemType.Vehicles) { ModelItemID = "tailgater2" },
            new VehicleItem("Ocelot Ardent", true, ItemType.Vehicles) { ModelItemID = "ardent" },
            new VehicleItem("Ocelot F620", ItemType.Vehicles) { ModelItemID = "f620" },
            new VehicleItem("Ocelot R88", true, ItemType.Vehicles) { ModelItemID = "formula2" },
            new VehicleItem("Ocelot Jackal", ItemType.Vehicles) { ModelItemID = "jackal" },
            new VehicleItem("Ocelot Jugular", true, ItemType.Vehicles) { ModelItemID = "jugular" },
            new VehicleItem("Ocelot Locust", true, ItemType.Vehicles) { ModelItemID = "locust" },
            new VehicleItem("Ocelot Lynx", true, ItemType.Vehicles) { ModelItemID = "lynx" },
            new VehicleItem("Ocelot Pariah", true, ItemType.Vehicles) { ModelItemID = "pariah" },
            new VehicleItem("Ocelot Penetrator", true, ItemType.Vehicles) { ModelItemID = "penetrator" },
            new VehicleItem("Ocelot Swinger", true, ItemType.Vehicles) { ModelItemID = "swinger" },
            new VehicleItem("Ocelot XA-21", true, ItemType.Vehicles) { ModelItemID = "xa21" },
            new VehicleItem("Overflod Autarch", true, ItemType.Vehicles) { ModelItemID = "autarch" },
            new VehicleItem("Overflod Entity XXR", true, ItemType.Vehicles) { ModelItemID = "entity2" },
            new VehicleItem("Overflod Entity XF", ItemType.Vehicles) { ModelItemID = "entityxf" },
            new VehicleItem("Overflod Imorgon", true, ItemType.Vehicles) { ModelItemID = "imorgon" },
            new VehicleItem("Overflod Tyrant", true, ItemType.Vehicles) { ModelItemID = "tyrant" },
            new VehicleItem("Pegassi Bati 801", ItemType.Vehicles) { ModelItemID = "bati" },
            new VehicleItem("Pegassi Bati 801RR", ItemType.Vehicles) { ModelItemID = "bati2" },
            new VehicleItem("Pegassi Esskey", true, ItemType.Vehicles) { ModelItemID = "esskey" },
            new VehicleItem("Pegassi Faggio Sport", true, ItemType.Vehicles) { ModelItemID = "faggio" },
            new VehicleItem("Pegassi Faggio", ItemType.Vehicles) { ModelItemID = "faggio2" },
            new VehicleItem("Pegassi Faggio Mod", true, ItemType.Vehicles) { ModelItemID = "faggio3" },
            new VehicleItem("Pegassi FCR 1000", true, ItemType.Vehicles) { ModelItemID = "fcr" },
            new VehicleItem("Pegassi FCR 1000 Custom", true, ItemType.Vehicles) { ModelItemID = "fcr2" },
            new VehicleItem("Pegassi Infernus", ItemType.Vehicles) { ModelItemID = "infernus" },
            new VehicleItem("Pegassi Infernus Classic", true, ItemType.Vehicles) { ModelItemID = "infernus2" },
            new VehicleItem("Pegassi Monroe", ItemType.Vehicles) { ModelItemID = "monroe" },
            new VehicleItem("Pegassi Oppressor", true, ItemType.Vehicles) { ModelItemID = "oppressor" },
            new VehicleItem("Pegassi Oppressor Mk II", true, ItemType.Vehicles) { ModelItemID = "oppressor2" },
            new VehicleItem("Pegassi Osiris", true, ItemType.Vehicles) { ModelItemID = "osiris" },
            new VehicleItem("Pegassi Reaper", true, ItemType.Vehicles) { ModelItemID = "reaper" },
            new VehicleItem("Pegassi Ruffian", ItemType.Vehicles) { ModelItemID = "ruffian" },
            new VehicleItem("Pegassi Tempesta", true, ItemType.Vehicles) { ModelItemID = "tempesta" },
            new VehicleItem("Pegassi Tezeract", true, ItemType.Vehicles) { ModelItemID = "tezeract" },
            new VehicleItem("Pegassi Torero", true, ItemType.Vehicles) { ModelItemID = "torero" },
            new VehicleItem("Pegassi Toros", true, ItemType.Vehicles) { ModelItemID = "toros" },
            new VehicleItem("Pegassi Vacca", ItemType.Vehicles) { ModelItemID = "vacca" },
            new VehicleItem("Pegassi Vortex", true, ItemType.Vehicles) { ModelItemID = "vortex" },
            new VehicleItem("Pegassi Zentorno", true, ItemType.Vehicles) { ModelItemID = "zentorno" },
            new VehicleItem("Pegassi Zorrusso", true, ItemType.Vehicles) { ModelItemID = "zorrusso" },
            new VehicleItem("Pfister Comet", "You always wanted one of these when in high school - and now you can have the car that tells everyone yes, these are implants - on your head and in that dizzy tart next to you. Boom. You go, tiger.", ItemType.Vehicles) { ModelItemID = "comet2" },
            new VehicleItem("Pfister Comet Retro Custom", "For a whole generation of the San Andreas elite, this isn't just a car. From the onboard champagne cooler to the suede back seat where you pawed your first gold digger - The Pfister Comet was something that made you who you are. And now, thanks to Benny reinventing it as a gnarly, riveted urban dragster, it'll be broadcasting your escalating midlife crisis for years to come.", true, ItemType.Vehicles) { ModelItemID = "comet3" },
            new VehicleItem("Pfister Comet Safari", "Is there nothing the Pfister Comet cannot do? If you were a venture capitalist looking for the shortest route to your next midlife crisis, the Comet was your first and only choice. If you wanted something that preserved the classic reek of desperation but added a street-racer twist, the Retro Custom was top of the list. And now, if you're looking for something to slam around a hairpin bend in three feet of uphill mud, the Comet Safari has got you covered.", true, ItemType.Vehicles) { ModelItemID = "comet4" },
            new VehicleItem("Pfister Comet SR", "Forget everything you think you know about the Pfister Comet. Forget cruising through Vinewood with a bellyful of whiskey dropping one-liners about the size of your bonus. Forget picking up sex workers and passing them off as your fiancé at family gatherings. The SR was made for only one thing: to make every other sports car look like it's the asthmatic kid in gym. Now get in line.", true, ItemType.Vehicles) { ModelItemID = "comet5" },
            new VehicleItem("Pfister Comet S2", "This isn't just a fast car. It's a car with the kind of reputation that no amount of targeted advertising can buy. So, when some people see a Comet they make a wish. Others run screaming for cover, prophesying doom, destruction, and crippling medical expenses. Either way, you made an impression.", true, ItemType.Vehicles) { ModelItemID = "comet6" },
            new VehicleItem("Pfister Growler","You prefer the book to the movie. You drink spirits neat. You describe your sense of humor as 'subtle' and your love making as 'imperceptible'. You're The Thinking Person. And you choose handling over speed, control over power, and principle over pleasure. You choose wisely. You choose the Pfister Growler.", true, ItemType.Vehicles) { ModelItemID = "growler" },
            new VehicleItem("Pfister Neon","When the history of the electric car is written, it will begin with the Pfister Neon. Everything else - all the ridiculous eco-vans and hybrid fetishes - has been foreplay. Now Pfister have dropped their pants, and the battery-powered action can really begin.", true, ItemType.Vehicles) { ModelItemID = "neon" },
            new VehicleItem("Pfister 811","Meet the future of hybrid tech: Pfister took billions of dollars in subsidies for low-carbon research and used it to refine an electric motor until it gives more kick than a turbo charger. And don't worry about accidentally investing in the environment: the assembly process alone produces enough CO2 to offset two thousand acres of otherwise useless rainforest. Win-win.", true, ItemType.Vehicles) { ModelItemID = "pfister811" },
            new VehicleItem("Principe Deveste Eight","It began as little more than a myth: a list of impossible statistics circulating on the dark net. Then the myth became a legend: a few leaked photographs so provocative that possession was a federal crime. Then the legend became a rumor: a car so exclusive no one could confirm it existed in the real world. And now, thanks to you, that rumor is about to become a very messy headline.", true, ItemType.Vehicles) { ModelItemID = "deveste" },
            new VehicleItem("Principe Diabolus","You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", true, ItemType.Vehicles) { ModelItemID = "diablous" },
            new VehicleItem("Principe Diabolus Custom","You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", true, ItemType.Vehicles) { ModelItemID = "diablous2" },
            new VehicleItem("Principe Lectro","As if this new-school streetfighter didn’t look aggressive enough, once you hit that KERS button you’ll be locked into a death struggle with the laws of physics - and there can only be one winner.", true, ItemType.Vehicles) { ModelItemID = "lectro" },
            new VehicleItem("Principe Nemesis","Super fast, super unshielded. When you're riding a Nemesis, you don't just feel the wind in your hair, you feel it tearing into the back of your eye sockets.", ItemType.Vehicles) { ModelItemID = "nemesis" },
            new VehicleItem("Progen Emerus", true, ItemType.Vehicles) { ModelItemID = "emerus" },
            new VehicleItem("Progen PR4", true, ItemType.Vehicles) { ModelItemID = "formula" },
            new VehicleItem("Progen GP1", true, ItemType.Vehicles) { ModelItemID = "gp1" },
            new VehicleItem("Progen Itali GTB", true, ItemType.Vehicles) { ModelItemID = "italigtb" },
            new VehicleItem("Progen Itali GTB Custom", true, ItemType.Vehicles) { ModelItemID = "italigtb2" },
            new VehicleItem("Progen T20", true, ItemType.Vehicles) { ModelItemID = "t20" },
            new VehicleItem("Progen Tyrus", true, ItemType.Vehicles) { ModelItemID = "tyrus" },
            new VehicleItem("RUNE Cheburek", true, ItemType.Vehicles) { ModelItemID = "cheburek" },
            new VehicleItem("Schyster Deviant", true, ItemType.Vehicles) { ModelItemID = "deviant" },
            new VehicleItem("Schyster Fusilade", ItemType.Vehicles) { ModelItemID = "fusilade" },
            new VehicleItem("Shitzu Defiler", true, ItemType.Vehicles) { ModelItemID = "defiler" },
            new VehicleItem("Shitzu Hakuchou", true, ItemType.Vehicles) { ModelItemID = "hakuchou" },
            new VehicleItem("Shitzu Hakuchou Drag", true, ItemType.Vehicles) { ModelItemID = "hakuchou2" },
            new VehicleItem("Shitzu PCJ 600", ItemType.Vehicles) { ModelItemID = "pcj" },
            new VehicleItem("Shitzu Vader", ItemType.Vehicles) { ModelItemID = "Vader" },
            new VehicleItem("Stanley Fieldmaster", ItemType.Vehicles) { ModelItemID = "tractor2" },
            new VehicleItem("Stanley Fieldmaster", ItemType.Vehicles) { ModelItemID = "tractor3" },
            new VehicleItem("Truffade Adder", ItemType.Vehicles) { ModelItemID = "adder" },
            new VehicleItem("Truffade Nero", true, ItemType.Vehicles) { ModelItemID = "nero" },
            new VehicleItem("Truffade Nero Custom", true, ItemType.Vehicles) { ModelItemID = "nero2" },
            new VehicleItem("Truffade Thrax", true, ItemType.Vehicles) { ModelItemID = "thrax" },
            new VehicleItem("Truffade Z-Type", ItemType.Vehicles) { ModelItemID = "Ztype" },
            new VehicleItem("Ubermacht Oracle XS", ItemType.Vehicles) { ModelItemID = "oracle" },
            new VehicleItem("Ubermacht Oracle", ItemType.Vehicles) { ModelItemID = "oracle2" },
            new VehicleItem("Ubermacht Revolter", true, ItemType.Vehicles) { ModelItemID = "revolter" },
            new VehicleItem("Ubermacht SC1", true, ItemType.Vehicles) { ModelItemID = "sc1" },
            new VehicleItem("Ubermacht Sentinel XS", ItemType.Vehicles) { ModelItemID = "sentinel" },
            new VehicleItem("Ubermacht Sentinel 2", ItemType.Vehicles) { ModelItemID = "sentinel2" },
            new VehicleItem("Ubermacht Sentinel 3", true, ItemType.Vehicles) { ModelItemID = "sentinel3" },
            new VehicleItem("Ubermacht Zion", ItemType.Vehicles) { ModelItemID = "zion" },
            new VehicleItem("Ubermacht Zion Cabrio", ItemType.Vehicles) { ModelItemID = "zion2" },
            new VehicleItem("Ubermacht Zion Classic", true, ItemType.Vehicles) { ModelItemID = "zion3" },
            new VehicleItem("Ubermacht Cypher", true, ItemType.Vehicles) { ModelItemID = "cypher" },
            new VehicleItem("Ubermacht Rebla GTS", true, ItemType.Vehicles) { ModelItemID = "rebla" },
            new VehicleItem("Vapid Benson", ItemType.Vehicles) { ModelItemID = "Benson" },
            new VehicleItem("Vapid Blade", true, ItemType.Vehicles) { ModelItemID = "blade" },
            new VehicleItem("Vapid Bobcat XL", ItemType.Vehicles) { ModelItemID = "bobcatXL" },
            new VehicleItem("Vapid Bullet", ItemType.Vehicles) { ModelItemID = "bullet" },
            new VehicleItem("Vapid Caracara", true, ItemType.Vehicles) { ModelItemID = "caracara" },
            new VehicleItem("Vapid Caracara 4x4", true, ItemType.Vehicles) { ModelItemID = "caracara2" },
            new VehicleItem("Vapid Chino", true, ItemType.Vehicles) { ModelItemID = "chino" },
            new VehicleItem("Vapid Chino Custom", true, ItemType.Vehicles) { ModelItemID = "chino2" },
            new VehicleItem("Vapid Clique", true, ItemType.Vehicles) { ModelItemID = "clique" },
            new VehicleItem("Vapid Contender", true, ItemType.Vehicles) { ModelItemID = "contender" },
            new VehicleItem("Vapid Dominator", "Baby boomer teen dream repackaged for the mass market generation. A muscle car without the muscle that's the rental vehicle of choice for tourists looking for a slice of 'real' Americana.", ItemType.Vehicles) { ModelItemID = "Dominator" },
            new VehicleItem("Vapid Pisswasser Dominator", ItemType.Vehicles) { ModelItemID = "dominator2" },
            new VehicleItem("Vapid Dominator GTX", "Step one: take the best-looking muscle car the 60's ever saw, and introduce it to the greatest American supercar of the modern era. When your pedigree is this damn good, there's nothing wrong with keeping it in the family.", ItemType.Vehicles) { ModelItemID = "dominator3" },
            new VehicleItem("Vapid Apocalypse Dominator", true, ItemType.Vehicles) { ModelItemID = "dominator4" },
            new VehicleItem("Vapid Future Shock Dominator", true, ItemType.Vehicles) { ModelItemID = "dominator5" },
            new VehicleItem("Vapid Nightmare Dominator", true, ItemType.Vehicles) { ModelItemID = "dominator6" },
            new VehicleItem("Vapid Dominator ASP", true, ItemType.Vehicles) { ModelItemID = "dominator7" },
            new VehicleItem("Vapid Dominator GTT", true, ItemType.Vehicles) { ModelItemID = "dominator8" },
            new VehicleItem("Vapid Ellie", true, ItemType.Vehicles) { ModelItemID = "ellie" },
            new VehicleItem("Vapid Flash GT", true, ItemType.Vehicles) { ModelItemID = "flashgt" },
            new VehicleItem("Vapid FMJ", true, ItemType.Vehicles) { ModelItemID = "fmj" },
            new VehicleItem("Vapid GB200", true, ItemType.Vehicles) { ModelItemID = "gb200" },
            new VehicleItem("Vapid Guardian", true, ItemType.Vehicles) { ModelItemID = "guardian" },
            new VehicleItem("Vapid Hotknife", ItemType.Vehicles) { ModelItemID = "hotknife" },
            new VehicleItem("Vapid Hustler", true, ItemType.Vehicles) { ModelItemID = "hustler" },
            new VehicleItem("Vapid Apocalypse Imperator", true, ItemType.Vehicles) { ModelItemID = "imperator" },
            new VehicleItem("Vapid Future Shock Imperator", true, ItemType.Vehicles) { ModelItemID = "imperator2" },
            new VehicleItem("Vapid Nightmare Imperator", true, ItemType.Vehicles) { ModelItemID = "imperator3" },
            new VehicleItem("Vapid Minivan", ItemType.Vehicles) { ModelItemID = "minivan" },
            new VehicleItem("Vapid Minivan Custom", true, ItemType.Vehicles) { ModelItemID = "minivan2" },
            new VehicleItem("Vapid Monster", true, ItemType.Vehicles) { ModelItemID = "monster" },
            new VehicleItem("Vapid Peyote", ItemType.Vehicles) { ModelItemID = "peyote" },
            new VehicleItem("Vapid Peyote Gasser", true, ItemType.Vehicles) { ModelItemID = "peyote2" },
            new VehicleItem("Vapid Peyote Custom", true, ItemType.Vehicles) { ModelItemID = "peyote3" },
            new VehicleItem("Vapid Radius", ItemType.Vehicles) { ModelItemID = "radi" },
            new VehicleItem("Vapid Retinue", true, ItemType.Vehicles) { ModelItemID = "retinue" },
            new VehicleItem("Vapid Retinue Mk II", true, ItemType.Vehicles) { ModelItemID = "retinue2" },
            new VehicleItem("Vapid Riata", true, ItemType.Vehicles) { ModelItemID = "riata" },
            new VehicleItem("Vapid Sadler", ItemType.Vehicles) { ModelItemID = "Sadler" },
            new VehicleItem("Vapid Sadler 2", ItemType.Vehicles) { ModelItemID = "sadler2" },
            new VehicleItem("Vapid Sandking XL", ItemType.Vehicles) { ModelItemID = "sandking" },
            new VehicleItem("Vapid Sandking SWB", ItemType.Vehicles) { ModelItemID = "sandking2" },
            new VehicleItem("Vapid Slamtruck", true, ItemType.Vehicles) { ModelItemID = "slamtruck" },
            new VehicleItem("Vapid Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan" },
            new VehicleItem("Vapid Lost Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan2" },
            new VehicleItem("Vapid Slamvan Custom", true, ItemType.Vehicles) { ModelItemID = "slamvan3" },
            new VehicleItem("Vapid Apocalypse Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan4" },
            new VehicleItem("Vapid Future Shock Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan5" },
            new VehicleItem("Vapid Nightmare Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan6" },
            new VehicleItem("Vapid Speedo", ItemType.Vehicles) { ModelItemID = "speedo" },
            new VehicleItem("Vapid Clown Van", ItemType.Vehicles) { ModelItemID = "speedo2" },
            new VehicleItem("Vapid Speedo Custom", true, ItemType.Vehicles) { ModelItemID = "speedo4" },
            new VehicleItem("Vapid Stanier", "If you took a cab or got arrested in the 1990s, there's a high chance you ended up in the back of a Vapid Stanier. Discontinued following widespread reports of fuel tanks exploding on impact in rear-end collisions. So try to avoid that.", ItemType.Vehicles) { ModelItemID = "stanier" },
            new VehicleItem("Vapid Trophy Truck", true, ItemType.Vehicles) { ModelItemID = "trophytruck" },
            new VehicleItem("Vapid Desert Raid", true, ItemType.Vehicles) { ModelItemID = "trophytruck2" },
            new VehicleItem("Vapid Winky", true, ItemType.Vehicles) { ModelItemID = "winky" },
            new VehicleItem("Vulcar Fagaloa", true, ItemType.Vehicles) { ModelItemID = "fagaloa" },
            new VehicleItem("Vulcar Ingot", ItemType.Vehicles) { ModelItemID = "ingot" },
            new VehicleItem("Vulcar Nebula Turbo", true, ItemType.Vehicles) { ModelItemID = "nebula" },
            new VehicleItem("Vulcar Warrener", true, ItemType.Vehicles) { ModelItemID = "warrener" },
            new VehicleItem("Vulcar Warrener HKR", true, ItemType.Vehicles) { ModelItemID = "warrener2" },
            new VehicleItem("Vysser Neo", true, ItemType.Vehicles) { ModelItemID = "neo" },
            new VehicleItem("Weeny Dynasty", true, ItemType.Vehicles) { ModelItemID = "Dynasty" },
            new VehicleItem("Weeny Issi", ItemType.Vehicles) { ModelItemID = "issi2" },
            new VehicleItem("Weeny Issi Classic", true, ItemType.Vehicles) { ModelItemID = "issi3" },
            new VehicleItem("Weeny Apocalypse Issi", true, ItemType.Vehicles) { ModelItemID = "issi4" },
            new VehicleItem("Weeny Future Shock Issi", true, ItemType.Vehicles) { ModelItemID = "issi5" },
            new VehicleItem("Weeny Nightmare Issi", true, ItemType.Vehicles) { ModelItemID = "issi6" },
            new VehicleItem("Weeny Issi Sport", true, ItemType.Vehicles) { ModelItemID = "issi7" },
            new VehicleItem("Western Bagger", ItemType.Vehicles) { ModelItemID = "bagger" },
            new VehicleItem("Western Cliffhanger", true, ItemType.Vehicles) { ModelItemID = "cliffhanger" },
            new VehicleItem("Western Daemon LOST", ItemType.Vehicles) { ModelItemID = "daemon" },
            new VehicleItem("Western Daemon", true, ItemType.Vehicles) { ModelItemID = "daemon2" },
            new VehicleItem("Western Apocalypse Deathbike", true, ItemType.Vehicles) { ModelItemID = "deathbike" },
            new VehicleItem("Western Future Shock Deathbike", true, ItemType.Vehicles) { ModelItemID = "deathbike2" },
            new VehicleItem("Western Nightmare Deathbike", true, ItemType.Vehicles) { ModelItemID = "deathbike3" },
            new VehicleItem("Western Gargoyle", true, ItemType.Vehicles) { ModelItemID = "gargoyle" },
            new VehicleItem("Western Nightblade", true, ItemType.Vehicles) { ModelItemID = "nightblade" },
            new VehicleItem("Western Rat Bike", true, ItemType.Vehicles) { ModelItemID = "ratbike" },
            new VehicleItem("Western Rampant Rocket", true, ItemType.Vehicles) { ModelItemID = "rrocket" },
            new VehicleItem("Western Sovereign", true, ItemType.Vehicles) { ModelItemID = "sovereign" },
            new VehicleItem("Western Wolfsbane", true, ItemType.Vehicles) { ModelItemID = "wolfsbane" },
            new VehicleItem("Western Zombie Bobber", true, ItemType.Vehicles) { ModelItemID = "zombiea" },
            new VehicleItem("Western Zombie Chopper", true, ItemType.Vehicles) { ModelItemID = "zombieb" },
            new VehicleItem("Willard Faction", true, ItemType.Vehicles) { ModelItemID = "faction" },
            new VehicleItem("Willard Faction Custom", true, ItemType.Vehicles) { ModelItemID = "faction2" },
            new VehicleItem("Willard Faction Custom Donk", true, ItemType.Vehicles) { ModelItemID = "faction3" },
            new VehicleItem("Zirconium Journey", ItemType.Vehicles) { ModelItemID = "journey" },
            new VehicleItem("Zirconium Stratum", ItemType.Vehicles) { ModelItemID = "stratum" },

            //Heli
            new VehicleItem("Buckingham SuperVolito", true, ItemType.Vehicles) { ModelItemID = "supervolito" },
            new VehicleItem("Buckingham SuperVolito Carbon", true, ItemType.Vehicles) { ModelItemID = "supervolito2" },
            new VehicleItem("Buckingham Swift", true, ItemType.Vehicles) { ModelItemID = "swift" },
            new VehicleItem("Buckingham Swift Deluxe", true, ItemType.Vehicles) { ModelItemID = "swift2" },
            new VehicleItem("Buckingham Volatus", true, ItemType.Vehicles) { ModelItemID = "volatus" },
            new VehicleItem("Mammoth Thruster", true, ItemType.Vehicles) { ModelItemID = "thruster" },
            new VehicleItem("Nagasaki Havok", true, ItemType.Vehicles) { ModelItemID = "havok" },

            //Plane
            new VehicleItem("Buckingham Alpha-Z1", true, ItemType.Vehicles) { ModelItemID = "alphaz1" },
            new VehicleItem("Buckingham Howard NX-25", true, ItemType.Vehicles) { ModelItemID = "howard" },
            new VehicleItem("Buckingham Luxor", ItemType.Vehicles) { ModelItemID = "luxor" },
            new VehicleItem("Buckingham Luxor Deluxe", true, ItemType.Vehicles) { ModelItemID = "luxor2" },
            new VehicleItem("Buckingham Miljet", true, ItemType.Vehicles) { ModelItemID = "Miljet" },
            new VehicleItem("Buckingham Nimbus", true, ItemType.Vehicles) { ModelItemID = "nimbus" },
            new VehicleItem("Buckingham Pyro", true, ItemType.Vehicles) { ModelItemID = "pyro" },
            new VehicleItem("Buckingham Shamal", ItemType.Vehicles) { ModelItemID = "Shamal" },
            new VehicleItem("Buckingham Vestra", true, ItemType.Vehicles) { ModelItemID = "vestra" },
            new VehicleItem("Mammoth Avenger", true, ItemType.Vehicles) { ModelItemID = "avenger" },
            new VehicleItem("Mammoth Avenger 2", true, ItemType.Vehicles) { ModelItemID = "avenger2" },
            new VehicleItem("Mammoth Dodo", ItemType.Vehicles) { ModelItemID = "dodo" },
            new VehicleItem("Mammoth Hydra", true, ItemType.Vehicles) { ModelItemID = "hydra" },
            new VehicleItem("Mammoth Mogul", true, ItemType.Vehicles) { ModelItemID = "mogul" },
            new VehicleItem("Mammoth Tula", true, ItemType.Vehicles) { ModelItemID = "tula" },
            new VehicleItem("Nagasaki Ultralight", true, ItemType.Vehicles) { ModelItemID = "microlight" },
            new VehicleItem("Western Besra", true, ItemType.Vehicles) { ModelItemID = "besra" },
            new VehicleItem("Western Rogue", true, ItemType.Vehicles) { ModelItemID = "rogue" },
            new VehicleItem("Western Seabreeze", true, ItemType.Vehicles) { ModelItemID = "seabreeze" },

            //Boat
            new VehicleItem("Dinka Marquis", ItemType.Vehicles) { ModelItemID = "marquis" },
            new VehicleItem("Lampadati Toro", true, ItemType.Vehicles) { ModelItemID = "toro" },
            new VehicleItem("Lampadati Toro", true, ItemType.Vehicles) { ModelItemID = "toro2" },
            new VehicleItem("Nagasaki Dinghy", ItemType.Vehicles) { ModelItemID = "Dinghy" },
            new VehicleItem("Nagasaki Dinghy 2", ItemType.Vehicles) { ModelItemID = "dinghy2" },
            new VehicleItem("Nagasaki Dinghy 3", true, ItemType.Vehicles) { ModelItemID = "dinghy3" },
            new VehicleItem("Nagasaki Dinghy 4", true, ItemType.Vehicles) { ModelItemID = "dinghy4" },
            new VehicleItem("Nagasaki Weaponized Dinghy", true, ItemType.Vehicles) { ModelItemID = "dinghy5" },
            new VehicleItem("Pegassi Speeder", true, ItemType.Vehicles) { ModelItemID = "speeder" },
            new VehicleItem("Pegassi Speeder", true, ItemType.Vehicles) { ModelItemID = "speeder2" },
            new VehicleItem("Shitzu Jetmax", ItemType.Vehicles) { ModelItemID = "jetmax" },
            new VehicleItem("Shitzu Longfin", true, ItemType.Vehicles) { ModelItemID = "longfin" },
            new VehicleItem("Shitzu Squalo", ItemType.Vehicles) { ModelItemID = "squalo" },
            new VehicleItem("Shitzu Suntrap", ItemType.Vehicles) { ModelItemID = "Suntrap" },
            new VehicleItem("Shitzu Tropic", ItemType.Vehicles) { ModelItemID = "tropic" },
            new VehicleItem("Shitzu Tropic", true, ItemType.Vehicles) { ModelItemID = "tropic2" },
            new VehicleItem("Speedophile Seashark", ItemType.Vehicles) { ModelItemID = "seashark" },
            new VehicleItem("Speedophile Seashark 2", ItemType.Vehicles) { ModelItemID = "seashark2" },
            new VehicleItem("Speedophile Seashark 3", true, ItemType.Vehicles) { ModelItemID = "seashark3" },
        });
    }
    private void DefaultConfig_Weapons()
    {
        PossibleItems.WeaponItems.AddRange(new List<WeaponItem>
        {
            new WeaponItem("Flint Hammer","A robust, multi-purpose hammer with wooden handle and curved claw, this old classic still nails the competition.", false, ItemType.Weapons) { ModelItemID = "weapon_hammer"},
            new WeaponItem("Flint Hatchet","Add a good old-fashioned hatchet to your armory, and always have a back up for when ammo is hard to come by.", false, ItemType.Weapons) { ModelItemID = "weapon_hatchet"},
            new WeaponItem("Flint Heavy Duty Pipe Wrench","Perennial favourite of apocalyptic survivalists and violent fathers the world over, apparently it also doubles as some kind of tool.", false, ItemType.Weapons) { ModelItemID = "weapon_wrench"},  
            new WeaponItem("Flint Crowbar","Heavy-duty crowbar forged from high quality, tempered steel for that extra leverage you need to get the job done.", false, ItemType.Weapons) { ModelItemID = "weapon_crowbar"},
            
            new WeaponItem("Vom Feuer Machete","America's West African arms trade isn't just about giving. Rediscover the simple life with this rusty cleaver.", false, ItemType.Weapons) { ModelItemID = "weapon_machete"},
            
            new WeaponItem("G.E.S. Baseball Bat","Aluminum baseball bat with leather grip. Lightweight yet powerful for all you big hitters out there.", false, ItemType.Weapons) { ModelItemID = "weapon_bat"},
            new WeaponItem("ProLaps Five Iron Golf Club","Standard length, mid iron golf club with rubber grip for a lethal short game.", false, ItemType.Weapons) { ModelItemID = "weapon_golfclub"},

            //Melee
            new WeaponItem("Brass Knuckles","Perfect for knocking out gold teeth, or as a gift to the trophy partner who has everything.", false, ItemType.Weapons) { ModelItemID = "weapon_knuckle"},
            new WeaponItem("Combat Knife","This carbon steel 7 inch bladed knife is dual edged with a serrated spine to provide improved stabbing and thrusting capabilities.", false, ItemType.Weapons) { ModelItemID = "weapon_knife"},
            new WeaponItem("Switchblade","From your pocket to hilt-deep in the other guy's ribs in under a second: folding knives will never go out of style.", false, ItemType.Weapons) { ModelItemID = "weapon_switchblade" },
            new WeaponItem("Nightstick","24 inch polycarbonate side-handled nightstick.", false, ItemType.Weapons) { ModelItemID = "weapon_nightstick"},
            new WeaponItem("Pool Cue","Ah, there's no sound as satisfying as the crack of a perfect break, especially when it's the other guy's spine.", false, ItemType.Weapons) { ModelItemID = "weapon_poolcue"},

            //Pistola
            new WeaponItem("Hawk & Little PTF092F","Standard handgun. A 9mm combat pistol with a magazine capacity of 12 rounds that can be extended to 16.", false, ItemType.Weapons) { ModelItemID = "weapon_pistol"},
            new WeaponItem("Hawk & Little Thunder","Balance, simplicity, precision: nothing keeps the peace like an extended barrel in the other guy's mouth.", true, ItemType.Weapons) { ModelItemID = "weapon_pistol_mk2"},
            new WeaponItem("Hawk & Little Combat Pistol","A compact, lightweight semi-automatic pistol designed for law enforcement and personal defense use. 12-round magazine with option to extend to 16 rounds.", false, ItemType.Weapons) { ModelItemID = "weapon_combatpistol"},
            new WeaponItem("Hawk & Little Desert Slug","High-impact pistol that delivers immense power but with extremely strong recoil. Holds 9 rounds in magazine.", false, ItemType.Weapons) { ModelItemID = "weapon_pistol50"},
            new WeaponItem("Vom Feuer P69","Not your grandma's ceramics. Although this pint-sized pistol is small enough to fit into her purse and won't set off a metal detector.", true, ItemType.Weapons) { ModelItemID = "weapon_ceramicpistol"},
            new WeaponItem("Vom Feuer SCRAMP","High-penetration, fully-automatic pistol. Holds 18 rounds in magazine with option to extend to 36 rounds.", false, ItemType.Weapons) { ModelItemID = "weapon_appistol"},
            new WeaponItem("Hawk & Little 1919","The heavyweight champion of the magazine fed, semi-automatic handgun world. Delivers accuracy and a serious forearm workout every time.", false, ItemType.Weapons) { ModelItemID = "weapon_heavypistol"},
            new WeaponItem("Hawk & Little Raging Mare","A handgun with enough stopping power to drop a crazed rhino, and heavy enough to beat it to death if you're out of ammo.", true, ItemType.Weapons) { ModelItemID = "weapon_revolver"},
            new WeaponItem("Hawk & Little Raging Mare Dx","If you can lift it, this is the closest you'll get to shooting someone with a freight train.", true, ItemType.Weapons) { ModelItemID = "weapon_revolver_mk2"},
            new WeaponItem("Shrewsbury S7","Like condoms or hairspray, this fits in your pocket for a night on the town. The price of a bottle at a club, it's half as accurate as a champagne cork, and twice as deadly.", false, ItemType.Weapons) { ModelItemID = "weapon_snspistol"},
            new WeaponItem("Shrewsbury S7A","The ultimate purse-filler: if you want to make Saturday Night really special, this is your ticket.", true, ItemType.Weapons) { ModelItemID = "weapon_snspistol_mk2"},
            new WeaponItem("Coil Tesla","Fires a projectile that administers a voltage capable of temporarily stunning an assailant. It's like, literally stunning.", false, ItemType.Weapons) { ModelItemID = "weapon_stungun"},
            new WeaponItem("BS M1922","What you really need is a more recognisable gun. Stand out from the crowd at an armed robbery with this engraved pistol.", true, ItemType.Weapons) { ModelItemID = "weapon_vintagepistol"},
            new WeaponItem("Vom Feuer Gruber","If you think shooting off without lifting a finger is a problem, there's a pill for that. But if you think it's a plus, we've got you covered.", true, ItemType.Weapons) { ModelItemID = "weapon_pistolxm3"},

            //Shotgun
            new WeaponItem("Shrewsbury 420 Sawed-Off","This single-barrel, sawed-off shotgun compensates for its low range and ammo capacity with devastating efficiency in close combat.", false, ItemType.Weapons) { ModelItemID = "weapon_sawnoffshotgun"},
            new WeaponItem("Shrewsbury 420","Standard shotgun ideal for short-range combat. A high-projectile spread makes up for its lower accuracy at long range.", false, ItemType.Weapons) { ModelItemID = "weapon_pumpshotgun"},
            new WeaponItem("Vom Feuer 569","Only one thing pumps more action than a pump action: watch out, the recoil is almost as deadly as the shot.", true, ItemType.Weapons) { ModelItemID = "weapon_pumpshotgun_mk2"},
            new WeaponItem("Vom Feuer IBS-12","Fully automatic shotgun with 8 round magazine and high rate of fire.", false, ItemType.Weapons) { ModelItemID = "weapon_assaultshotgun"},
            new WeaponItem("Hawk & Little HLSG","More than makes up for its slow, pump-action rate of fire with its range and spread. Decimates anything in its projectile path.", false, ItemType.Weapons) { ModelItemID = "weapon_bullpupshotgun"},
            new WeaponItem("Shrewsbury Taiga-12","The weapon to reach for when you absolutely need to make a horrible mess of the room. Best used near easy-wipe surfaces only.", true, ItemType.Weapons) { ModelItemID = "weapon_heavyshotgun"},
            new WeaponItem("Toto 12 Guage Sawed-Off","Do one thing, do it well. Who needs a high rate of fire when your first shot turns the other guy into a fine mist?.", true, ItemType.Weapons) { ModelItemID = "weapon_dbshotgun"},
            new WeaponItem("Shrewsbury Defender","How many effective tools for riot control can you tuck into your pants? Ok, two. But this is the other one.", true, ItemType.Weapons) { ModelItemID = "weapon_autoshotgun"},
            new WeaponItem("Leotardo SPAZ-11","There's only one semi-automatic shotgun with a fire rate that sets the LSFD alarm bells ringing, and you're looking at it.", true, ItemType.Weapons) { ModelItemID = "weapon_combatshotgun"},

            //SMG
            new WeaponItem("Shrewsbury Luzi","Combines compact design with a high rate of fire at approximately 700-900 rounds per minute.", false, ItemType.Weapons) { ModelItemID = "weapon_microsmg"},
            new WeaponItem("Hawk & Little MP6","This is known as a good all-around submachine gun. Lightweight with an accurate sight and 30-round magazine capacity.", false, ItemType.Weapons) { ModelItemID = "weapon_smg"},
            new WeaponItem("Hawk & Little XPM","Lightweight, compact, with a rate of fire to die very messily for: turn any confined space into a kill box at the click of a well-oiled trigger.", true, ItemType.Weapons) { ModelItemID = "weapon_smg_mk2"},
            new WeaponItem("Vom Feuer Fisher","A high-capacity submachine gun that is both compact and lightweight. Holds up to 30 bullets in one magazine.", false, ItemType.Weapons) { ModelItemID = "weapon_assaultsmg"},
            new WeaponItem("Coil PXM","Who said personal weaponry couldn't be worthy of military personnel? Thanks to our lobbyists, not Congress. Integral suppressor.", false, ItemType.Weapons) { ModelItemID = "weapon_combatpdw"},
            new WeaponItem("Vom Feuer KEK-9","This fully automatic is the snare drum to your twin-engine V8 bass: no drive-by sounds quite right without it.", false, ItemType.Weapons) { ModelItemID = "weapon_machinepistol"},
            new WeaponItem("Hawk & Little Millipede","Increasingly popular since the marketing team looked beyond spec ops units and started caring about the little guys in low income areas.", false, ItemType.Weapons) { ModelItemID = "weapon_minismg"},

            //AR
            new WeaponItem("Shrewsbury A7-4K","This standard assault rifle boasts a large capacity magazine and long distance accuracy.", false, ItemType.Weapons) { ModelItemID = "weapon_assaultrifle"},
            new WeaponItem("Shrewsbury A2-1K","The definitive revision of an all-time classic: all it takes is a little work, and looks can kill after all.", true, ItemType.Weapons) { ModelItemID = "weapon_assaultrifle_mk2"},
            new WeaponItem("Vom Feuer A5-1R","Combining long distance accuracy with a high capacity magazine, the Carbine Rifle can be relied on to make the hit.", false, ItemType.Weapons) { ModelItemID = "weapon_carbinerifle"},
            new WeaponItem("Vom Feuer A5-1R MK2","This is bespoke, artisan firepower: you couldn't deliver a hail of bullets with more love and care if you inserted them by hand.", true, ItemType.Weapons) { ModelItemID = "weapon_carbinerifle_mk2" },
            new WeaponItem("Vom Feuer BFR","The most lightweight and compact of all assault rifles, without compromising accuracy and rate of fire.", false, ItemType.Weapons) { ModelItemID = "weapon_advancedrifle"},
            new WeaponItem("Vom Feuer SL6","Combining accuracy, maneuverability, firepower and low recoil, this is an extremely versatile assault rifle for any combat situation.", false, ItemType.Weapons) { ModelItemID = "weapon_specialcarbine"},
            new WeaponItem("Vom Feuer SL6 MK2","The jack of all trades just got a serious upgrade: bow to the master.", true, ItemType.Weapons) { ModelItemID = "weapon_specialcarbine_mk2"},
            new WeaponItem("Hawk & Little ZBZ-23","The latest Chinese import taking America by storm, this rifle is known for its balanced handling. Lightweight and very controllable in automatic fire.", false, ItemType.Weapons) { ModelItemID = "weapon_bullpuprifle"},
            new WeaponItem("Hawk & Little ZBZ-25X","So precise, so exquisite, it's not so much a hail of bullets as a symphony.", true, ItemType.Weapons) { ModelItemID = "weapon_bullpuprifle_mk2"},
            new WeaponItem("Shrewsbury Stinkov","Half the size, all the power, double the recoil: there's no riskier way to say 'I'm compensating for something'.", false, ItemType.Weapons) { ModelItemID = "weapon_compactrifle"},
            new WeaponItem("Vom Feuer GUH-B4","This immensely powerful assault rifle was designed for highly qualified, exceptionally skilled soldiers. Yes, you can buy it.", false, ItemType.Weapons) { ModelItemID = "weapon_militaryrifle"},
            new WeaponItem("Vom Feuer POCK","The no-holds barred 30-round answer to that eternal question: how do I get this guy off my back?", true, ItemType.Weapons) { ModelItemID = "weapon_heavyrifle"},


            new WeaponItem("Vom Feuer DP1 Carbine","This season's must-have hardware for law enforcement, military personnel and anyone locked in a fight to the death with either law enforcement or military personnel.", true, ItemType.Weapons) { ModelItemID = "weapon_tacticalrifle"},//old school m16

            //LMG
            new WeaponItem("Shrewsbury PDA","General purpose machine gun that combines rugged design with dependable performance. Long range penetrative power. Very effective against large groups.", false, ItemType.Weapons) { ModelItemID = "weapon_mg"},
            new WeaponItem("Vom Feuer BAT","Lightweight, compact machine gun that combines excellent maneuverability with a high rate of fire to devastating effect.", false, ItemType.Weapons) { ModelItemID = "weapon_combatmg"},
            new WeaponItem("Vom Feuer M70E1","You can never have too much of a good thing: after all, if the first shot counts, then the next hundred or so must count for double.", true, ItemType.Weapons) { ModelItemID = "weapon_combatmg_mk2"},
            new WeaponItem("Hawk & Little Kenan","Complete your look with a Prohibition gun. Looks great being fired from an Albany Roosevelt or paired with a pinstripe suit.", false, ItemType.Weapons) { ModelItemID = "weapon_gusenberg"},

            //SNIPER
            new WeaponItem("Shrewsbury PWN","Standard sniper rifle. Ideal for situations that require accuracy at long range. Limitations include slow reload speed and very low rate of fire.", false, ItemType.Weapons) { ModelItemID = "weapon_sniperrifle"},
            new WeaponItem("Bartlett M92","Features armor-piercing rounds for heavy damage. Comes with laser scope as standard.", false, ItemType.Weapons) { ModelItemID = "weapon_heavysniper"},
            new WeaponItem("Bartlett M92 Mk2","Far away, yet always intimate: if you're looking for a secure foundation for that long-distance relationship, this is it.", true, ItemType.Weapons) { ModelItemID = "weapon_heavysniper_mk2"},
            new WeaponItem("Vom Feuer M23 DBS","Whether you're up close or a disconcertingly long way away, this weapon will get the job done. A multi-range tool for tools.", false, ItemType.Weapons) { ModelItemID = "weapon_marksmanrifle"},
            new WeaponItem("Vom Feuer M23 DBS Scout","Known in military circles as The Dislocator, this mod set will destroy both the target and your shoulder, in that order.", true, ItemType.Weapons) { ModelItemID = "weapon_marksmanrifle_mk2"},

            new WeaponItem("Vom Feuer 699 PCR","A rifle for perfectionists. Because why settle for right-between-the-eyes, when you could have right-through-the-superior-frontal-gyrus.", true, ItemType.Weapons) { ModelItemID = "weapon_precisionrifle"},

            //new WeaponItem("Shrewsbury BFD Dragmeout","Want to give the impression of accuracy while still having greater than 1 MOA? Dragmeout.", true, ItemType.Weapons) { ModelItemID = "weapon_russiansniper"},


            //OTHER
            new WeaponItem("RPG-7","A portable, shoulder-launched, anti-tank weapon that fires explosive warheads. Very effective for taking down vehicles or large groups of assailants.", false, ItemType.Weapons) { ModelItemID = "weapon_rpg"},
            new WeaponItem("Hawk & Little MGL","A compact, lightweight grenade launcher with semi-automatic functionality. Holds up to 10 rounds.", false, ItemType.Weapons) { ModelItemID = "weapon_grenadelauncher"},
            new WeaponItem("M61 Grenade","Standard fragmentation grenade. Pull pin, throw, then find cover. Ideal for eliminating clustered assailants.", false, ItemType.Weapons) { ModelItemID = "weapon_grenade"},
            new WeaponItem("Improvised Incendiary","Crude yet highly effective incendiary weapon. No happy hour with this cocktail.", false, ItemType.Weapons) { ModelItemID = "weapon_molotov"},
            new WeaponItem("BZ Gas Grenade","BZ gas grenade, particularly effective at incapacitating multiple assailants.", false, ItemType.Weapons) { ModelItemID = "weapon_bzgas"},
            new WeaponItem("Tear Gas Grenade","Tear gas grenade, particularly effective at incapacitating multiple assailants. Sustained exposure can be lethal.", false, ItemType.Weapons) { ModelItemID = "weapon_smokegrenade"},
        });
    }






}