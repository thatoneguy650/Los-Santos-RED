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
    private List<ModItem> ModItemsList;
    public List<ModItem> Items => ModItemsList;
    public ModItem Get(string name)
    {
        return ModItemsList.FirstOrDefault(x => x.Name == name);
    }
    public ModItem GetRandomItem()
    {
        return ModItemsList.Where(x => x.ModelItem?.Type != ePhysicalItemType.Vehicle && x.ModelItem?.Type != ePhysicalItemType.Weapon && x.ModelItem?.Type != ePhysicalItemType.Ped && !x.ConsumeOnPurchase).PickRandom();
    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("ModItems*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Mod Items config: {ConfigFile.FullName}",0);
            ModItemsList = Serialization.DeserializeParams<ModItem>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Mod Items config  {ConfigFileName}",0);
            ModItemsList = Serialization.DeserializeParams<ModItem>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Mod Items config found, creating default", 0);
            DefaultConfig();
        }
    }

    public void Setup(PhysicalItems propItems)
    {
        foreach (ModItem mi in ModItemsList)
        {
            if (mi.ItemType == ItemType.Vehicles)
            {
                mi.ModelItem = new PhysicalItem(mi.ModelItemID, ePhysicalItemType.Vehicle);
            }
            else if (mi.ItemType == ItemType.Weapons)
            {
                mi.ModelItem = new PhysicalItem(mi.ModelItemID, Game.GetHashKey(mi.ModelItemID), ePhysicalItemType.Weapon);
            }
            else
            {
                if (mi.ModelItemID != "")
                {
                    mi.ModelItem = propItems.Get(mi.ModelItemID);
                }
                if (mi.PackageItemID != "")
                {
                    mi.PackageItem = propItems.Get(mi.PackageItemID);
                }
            }
        }
    }

    private void DefaultConfig()
    {
        ModItemsList = new List<ModItem> { };
        DefaultConfig_Drinks();
        DefaultConfig_Food();
        DefaultConfig_Drugs();
        DefaultConfig_Weapons();
        DefaultConfig_Tools();
        DefaultConfig_Vehicles();
        DefaultConfig_Services();
        Serialization.SerializeParams(ModItemsList, ConfigFileName);
    }

    private void DefaultConfig_Drinks()
    {
        ModItemsList.AddRange(new List<ModItem> {
            //Drinks
            //Bottles
            new ModItem("Bottle of Raine Water", "The water that rich people drink, and the main reason why there are now entire continents of plastic bottles floating in the ocean", eConsumableType.Drink, ItemType.Drinks) { 
                ModelItemID = "ba_prop_club_water_bottle",HealthChangeAmount = 20, ThirstChangeAmount = 20.0f, ItemSubType = ItemSubType.Water },//slight clipping, no issyes
            new ModItem("Bottle of GREY Water", "Expensive water that tastes worse than tap!", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "h4_prop_battle_waterbottle_01a",HealthChangeAmount = 20, ThirstChangeAmount = 20.0f,CleanupItemImmediately = true, ItemSubType = ItemSubType.Water},//lotsa clipping, does not have gravity
            new ModItem("Bottle of JUNK Energy", "The Quick Fix!", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_energy_drink",HealthChangeAmount = 30, ThirstChangeAmount = 20.0f,SleepChangeAmount = 10.0f, ItemSubType = ItemSubType.Soda},//fine
            //Beer
            new ModItem("Bottle of PiBwasser", "Cheap 11% ABV fighting lager brewed in Germany for export only from rice, barley, hops and the fresh urine of Bavarian virgins", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_amb_beer_bottle",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer},//is perfecto
            new ModItem("Bottle of A.M.", "Mornings Golden Shower", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beer_am",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Barracho", "Es Playtime!", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beer_bar",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Blarneys", "Making your mouth feel lucky", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beer_blr", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Jakeys", "Drink Outdoors With Jakey's", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beer_jakey", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Logger", "A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beer_logger",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Patriot", "Never refuse a patriot", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beer_patriot", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Pride", "Swallow Me", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beer_pride", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Stronzo", "Birra forte d'Italia", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beer_stz", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Dusche", "Das Ist Gut Ja!", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_beerdusche", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            //Liquor
            new ModItem("Bottle of 40 oz", "Drink like a true thug!", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_cs_beer_bot_40oz", IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, ItemSubType= ItemSubType.Beer},
            new ModItem("Bottle of Sinsimito Tequila", "Extra Anejo 100% De Agave. 42% Alcohol by volume", eConsumableType.Drink, ItemType.Drinks){
                PackageItemID = "h4_prop_h4_t_bottle_02a", IntoxicantName = "High Proof Alcohol",HealthChangeAmount = 15, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Liquor},
            new ModItem("Bottle of Cazafortuna Tequila", "Tequila Anejo. 100% Blue Agave 40% Alcohol by volume", eConsumableType.Drink, ItemType.Drinks){
                PackageItemID = "h4_prop_h4_t_bottle_01a", IntoxicantName = "High Proof Alcohol",HealthChangeAmount = 15, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,CleanupItemImmediately = true, ItemSubType= ItemSubType.Liquor},
            //Cups & Cans
            new ModItem("Can of eCola", "Deliciously Infectious!", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "ng_proc_sodacan_01a", HealthChangeAmount = 10, SleepChangeAmount = 1.0f, ThirstChangeAmount = 10.0f, ItemSubType= ItemSubType.Soda},
            new ModItem("Can of Sprunk", "Slurp Sprunk Mmm! Delicious", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "ng_proc_sodacan_01b", HealthChangeAmount = 10, SleepChangeAmount = 1.0f, ThirstChangeAmount = 10.0f, ItemSubType= ItemSubType.Soda},
            new ModItem("Can of Orang-O-Tang", "Orange AND Tang! Orang-O-Tang!", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "prop_orang_can_01",HealthChangeAmount = 10, ItemSubType= ItemSubType.Soda},//needs better attachment
            new ModItem("Carton of Milk", "Full Fat. Farmed and produced in U.S.A.", eConsumableType.Drink, ItemType.Drinks) { HealthChangeAmount = 10, ThirstChangeAmount = 10.0f, HungerChangeAmount = 5.0f, ItemSubType= ItemSubType.Milk },
            new ModItem("Cup of eCola", "Deliciously Infectious!", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01a",HealthChangeAmount = 10, SleepChangeAmount = 1.0f, ThirstChangeAmount = 10.0f, ItemSubType= ItemSubType.Soda},//has no gravity, too far down
            new ModItem("Cup of Sprunk", "Slurp Sprunk Mmm! Delicious", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01b",HealthChangeAmount = 10, SleepChangeAmount = 1.0f, ThirstChangeAmount = 10.0f, ItemSubType= ItemSubType.Soda},//perfecto
            new ModItem("Cup of Coffee", "Finally something without sugar! Sugar on Request", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_02",HealthChangeAmount = 10, SleepChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Can of Hoplivion Double IPA", "So many hops it should be illegal.", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "h4_prop_h4_can_beer_01a",IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f,},//pretty good, maybeslightly off
            new ModItem("Can of Blarneys", "Making your mouth feel lucky", eConsumableType.Drink, ItemType.Drinks) { IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, },
            new ModItem("Can of Logger", "A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned", eConsumableType.Drink, ItemType.Drinks) { IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, SleepChangeAmount = -2.0f,HungerChangeAmount = 2.0f,ThirstChangeAmount = 5.0f, },
            //Bean Machine
            new ModItem("High Noon Coffee", "Drip coffee, carbonated water, fruit syrup and taurine.", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 10, SleepChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("The Eco-ffee", "Decaf light, rain forest rain, saved whale milk, chemically reclaimed freerange organic tofu, and recycled brown sugar", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 12, SleepChangeAmount = 12.0f, ThirstChangeAmount = 12.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Speedball Coffee", "Caffeine tripe-shot, guarana, bat guano, and mate.", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 15, SleepChangeAmount = 15.0f, ThirstChangeAmount = 15.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Gunkacchino Coffee", "Caffeine, refined sugar, trans fat, high-fructose corn syrup, and cheesecake base.", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 17, SleepChangeAmount = 17.0f, ThirstChangeAmount = 17.0f,},//perfecto
            new ModItem("Bratte Coffee", "Double shot latte, and 100 pumps of caramel.", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 5, SleepChangeAmount = 5.0f, ThirstChangeAmount = 5.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Flusher Coffee", "Caffeine, organic castor oil, concanetrated OJ, chicken vindaldo, and senna pods.", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 10, SleepChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Caffeagra Coffee", "Caffeine (Straight up), rhino horn, oyster shell, and sildenafil citrate.", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 17, SleepChangeAmount = 17.0f, ThirstChangeAmount = 17.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Big Fruit Smoothie", "Frothalot, watermel, carbonated water, taurine, and fruit syrup.", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 15, SleepChangeAmount = 15.0f, ThirstChangeAmount = 15.0f, ItemSubType = ItemSubType.Coffee},//perfecto
            //UP N ATOM
            new ModItem("Jumbo Shake", "Almost a whole cow full of milk", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01c",HealthChangeAmount = 10, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Milk},//no gravity, attached wrong
            //burger shot
            new ModItem("Double Shot Coffee", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "p_ing_coffeecup_01",HealthChangeAmount = 5, SleepChangeAmount = 10.0f, ThirstChangeAmount = 10.0f,CleanupItemImmediately = true, ItemSubType = ItemSubType.Coffee },//n gravity,not attached right
            new ModItem("Liter of eCola", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01a",HealthChangeAmount = 15, SleepChangeAmount = 2.0f, ThirstChangeAmount = 15.0f,CleanupItemImmediately = true, ItemSubType = ItemSubType.Soda},//n gravity,not attached right
            new ModItem("Liter of Sprunk", eConsumableType.Drink, ItemType.Drinks){
                ModelItemID = "ng_proc_sodacup_01b", HealthChangeAmount = 15, SleepChangeAmount = 2.0f, ThirstChangeAmount = 15.0f,CleanupItemImmediately = true, ItemSubType = ItemSubType.Soda },//n gravity,not attached right 
        });;
    }
    private void DefaultConfig_Drugs()
    {
        ModItemsList.AddRange(new List<ModItem>
        {
            //Cigarettes/Cigars
            new ModItem("Redwood Regular", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs",AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette },
            new ModItem("Redwood Mild", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda. Milder version",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs2",AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -5,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette },
            new ModItem("Debonaire", "Tobacco products marketed at the more sophisticated smoker, whoever that is",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs3",AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette },
            new ModItem("Debonaire Menthol", "Tobacco products marketed at the more sophisticated smoker, whoever that is. With Menthol!",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs4",AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette },
            new ModItem("Caradique", "Fine Napoleon Cigarettes",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs5",AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette },
            new ModItem("69 Brand","Don't let an embargo stop you",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "ng_proc_cigarette01a",
                PackageItemID = "v_ret_ml_cigs6",AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigarette },
            //new Vector3(-0.025f,0.01f,0.004f),new Rotator(0f, 0f, 90f) female mouth attach?



            new ModItem("Estancia Cigar","Medium Cut. Hand Rolled.",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "prop_cigar_02",
                PackageItemID = "p_cigar_pack_02_s",AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -5,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Cigar },
            //new ModItem("ElectroToke Vape","The Electrotoke uses highly sophisticated micro-molecule atomization technology to make the ingestion of hard drugs healthy, dscreet, pleasurable and, best of all, completely safe.",eConsumableType.Smoke, ItemType.Drugs) {
            //    ModelItemID = "h4_prop_battle_vape_01"), IntoxicantName = "Marijuana", PercentLostOnUse = 0.05f },





            //Legal Drugs
            new ModItem("White Widow","Among the most famous strains worldwide is White Widow, a balanced hybrid first bred in the Netherlands by Green House Seeds.",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "p_cs_joint_01",PackageItemID = "prop_weed_bottle", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter , ItemSubType = ItemSubType.Narcotic},
            new ModItem("OG Kush","OG Kush, also known as 'Premium OG Kush', was first cultivated in Florida in the early '90s when a marijuana strain from Northern California was supposedly crossed with Chemdawg, Lemon Thai and a Hindu Kush plant from Amsterdam.",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "p_cs_joint_01",PackageItemID = "prop_weed_bottle", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Northern Lights","Northern Lights, also known as 'NL', is an indica marijuana strain made by crossing Afghani with Thai.",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "p_cs_joint_01",PackageItemID = "prop_weed_bottle", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Bull Shark Testosterone","More bite than bush elephant testosterone. Become more aggressive, hornier, and irresistible to women! The ultimate man!",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Bull Shark Testosterone" , AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic},
            new ModItem("Alco Patch","The Alco Patch. It's the same refreshing feeling of your favorite drink, but delivered transdermally and discreetly. Pick up the Alco Patch at your local pharmacy.",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Alco Patch",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic},
            new ModItem("Lax to the Max","Lubricated suppositories. Get flowing again!",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Alco Patch",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic},
            new ModItem("Mollis","For outstanding erections. Get the performance you've always dreamed of",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Mollis",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic},
            new ModItem("Chesty","Cough suppressant manufactured by Good Aids Pharmacy. Gives 24-hour relief and is available in honey flavour.",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Chesty", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Equanox","Combats dissatisfaction, lethargy, depression, melancholy, sexual dysfunction. Equanox may cause nausea, loss of sleep, blurred vision, leakage, kidney problems and breathing irregularities.",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Equanox", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Zombix","Painkiller and antidepressant manufactured by O'Deas Pharmacy. ~n~'Go straight for the head.'",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItemID = "prop_cs_pills",IntoxicantName = "Zombix", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic },
            //Illegal Drugs
            new ModItem("Marijuana","Little Jacob Tested, Truth Approved",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItemID = "p_cs_joint_01"//p_amb_joint_01
                ,PackageItemID = "sf_prop_sf_bag_weed_01a", PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
            new ModItem("SPANK","You looking for some fun? a little.. hmmm? Some SPANK?",eConsumableType.Ingest, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_pills",IntoxicantName = "SPANK", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic },
            new ModItem("Toilet Cleaner","Meth brought you forbidden fruits of incest. Bath salts brought you the taboo joys of cannibalism. It's time to step things up a level. The hot new legal high that takes you to places you never imagined and leaves you forever changed - Toilet Cleaner.",eConsumableType.Ingest, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_pills",IntoxicantName = "Toilet Cleaner", ItemSubType = ItemSubType.Narcotic },
            new ModItem("Cocaine","Also known as coke, crack, girl, lady, charlie, caine, tepung, and snow",eConsumableType.Snort, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "ba_prop_battle_sniffing_pipe"
                ,PackageItemID = "prop_meth_bag_01"
                ,IntoxicantName = "Cocaine", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic },
            new ModItem("Crack","",eConsumableType.AltSmoke, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_crackpipe"
                ,PackageItemID = "prop_meth_bag_01"
                ,IntoxicantName = "Crack", PercentLostOnUse = 0.5f, MeasurementName = "Gram", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Heroin","Heroin was first made by C. R. Alder Wright in 1874 from morphine, a natural product of the opium poppy",eConsumableType.Inject, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_syringe_01"
                ,PackageItemID = "prop_meth_bag_01"
                ,IntoxicantName = "Heroin", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic },
            new ModItem("Methamphetamine","Also referred to as Speed, Sabu, Crystal and Meth",eConsumableType.AltSmoke, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItemID = "prop_cs_meth_pipe"
                ,PackageItemID = "prop_meth_bag_01"
                ,IntoxicantName = "Methamphetamine", PercentLostOnUse = 0.25f, MeasurementName = "Gram", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
        });
    }
    private void DefaultConfig_Food()
    {
        ModItemsList.AddRange(new List<ModItem>
        {
            //Generic Food
            new ModItem("Hot Dog","Your favorite mystery meat sold on street corners everywhere. Niko would be proud",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_hotdog_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Hot Sausage","Get all your jokes out",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_hotdog_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Hot Pretzel","You tie me up",eConsumableType.Eat, ItemType.Food) {HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("3 Mini Pretzels","Like a pretzel, but smaller",eConsumableType.Eat, ItemType.Food) {HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Nuts","You're gonna love my nuts",eConsumableType.Eat, ItemType.Food) {HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack },
            new ModItem("Burger","100% Certified Food",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Donut","MMMMMMM Donuts",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_donut_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Snack } ,
            new ModItem("Bagel Sandwich","Bagel with extras, what more do you need?",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "p_amb_bagel_01", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("French Fries","Freedom fries made from true Cataldo potatoes!",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_chips", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Side },
            new ModItem("Fries","Freedom fries made from true Cataldo potatoes!",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_chips", HealthChangeAmount = 5, HungerChangeAmount = 5.0f,ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Side },
            new ModItem("Banana","An elongated, edible fruit – botanically a berry[1][2] – produced by several kinds of large herbaceous flowering plants in the genus Musa",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "ng_proc_food_nana1a", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Fruit },
            new ModItem("Orange","Not just a color",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "ng_proc_food_ornge1a", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Fruit },
            new ModItem("Apple","Certified sleeping death free",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "ng_proc_food_aple1a", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = 5.0f, ItemSubType = ItemSubType.Fruit },
            new ModItem("Ham and Cheese Sandwich","Basic and shitty, just like you",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_sandwich_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Turkey Sandwich","The most plain sandwich for the most plain person",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_sandwich_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Tuna Sandwich","Haven't got enough heavy metals in you at your job? Try tuna!",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_sandwich_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Taco",eConsumableType.Eat, ItemType.Food) { HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree, ModelItemID = "prop_taco_01" },
            new ModItem("Strawberry Rails Cereal","The breakfast food you snort!",eConsumableType.Eat, ItemType.Food) { HealthChangeAmount = 50, HungerChangeAmount = 30.0f, ItemSubType = ItemSubType.Cereal} ,
            new ModItem("Crackles O' Dawn Cereal","Smile at the crack!",eConsumableType.Eat, ItemType.Food) { HealthChangeAmount = 60, HungerChangeAmount = 40.0f, ThirstChangeAmount = -5.0f, ItemSubType = ItemSubType.Cereal} ,
            new ModItem("White Bread","Extra white, with minimal taste.",eConsumableType.Eat, ItemType.Food) { HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -5.0f, AmountPerPackage = 25, ItemSubType = ItemSubType.Bread} ,
            //Pizza
            new ModItem("Slice of Pizza","Caution may be hot",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "v_res_tt_pizzaplate", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Small Cheese Pizza","Best when you are home alone.",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 25, HungerChangeAmount = 25.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Small Pepperoni Pizza","Get a load of our pepperonis!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 30, HungerChangeAmount = 30.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Small Supreme Pizza","Get stuffed",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 35, HungerChangeAmount = 35.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Medium Cheese Pizza","Best when you are home alone.",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 50, HungerChangeAmount = 50.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Medium Pepperoni Pizza","Get a load of our pepperonis!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 55, HungerChangeAmount = 55.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Medium Supreme Pizza","Get stuffed",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 60, HungerChangeAmount = 60.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Large Cheese Pizza","Best when you are home alone.",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 65, HungerChangeAmount = 65.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Large Pepperoni Pizza","Get a load of our pepperonis!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 70, HungerChangeAmount = 70.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Large Supreme Pizza","Get stuffed",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_02", HealthChangeAmount = 75, HungerChangeAmount = 75.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("10 inch Cheese Pizza","Extra cheesy!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 25, HungerChangeAmount = 25.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("10 inch Pepperoni Pizza","Mostly Meat!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 30, HungerChangeAmount = 30.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("10 inch Supreme Pizza","We forgot the kitchen sink!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 35, HungerChangeAmount = 35.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("12 inch Cheese Pizza","Extra cheesy!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 50, HungerChangeAmount = 50.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("12 inch Pepperoni Pizza","Mostly Meat!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 55, HungerChangeAmount = 55.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("12 inch Supreme Pizza","We forgot the kitchen sink!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 60, HungerChangeAmount = 60.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("18 inch Cheese Pizza","Extra cheesy! Extra Large!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 65, HungerChangeAmount = 65.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("18 inch Pepperoni Pizza","Mostly Meat! Extra Large!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 70, HungerChangeAmount = 70.0f, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("18 inch Supreme Pizza","We forgot the kitchen sink! Extra Large!",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_pizza_box_01", HealthChangeAmount = 75, HungerChangeAmount = 75.0f, ItemSubType = ItemSubType.Pizza } ,
            //Chips
            new ModItem("Sticky Rib Phat Chips","They are extra phat. Sticky Rib Flavor.",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "v_ret_ml_chips1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack },
            new ModItem("Habanero Phat Chips","They are extra phat. Habanero flavor",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "v_ret_ml_chips2", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack },
            new ModItem("Supersalt Phat Chips","They are extra phat. Supersalt flavor.",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "v_ret_ml_chips3", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack },
            new ModItem("Big Cheese Phat Chips","They are extra phat. Big Cheese flavor.",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "v_ret_ml_chips4", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack },
            //Candy
            new ModItem("Ego Chaser Energy Bar","Contains 20,000 Calories! ~n~'It's all about you'",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_choc_ego", HealthChangeAmount = 20, HungerChangeAmount = 20.0f,SleepChangeAmount = 5.0f, ItemSubType = ItemSubType.Snack },
            new ModItem("King Size P's & Q's","The candy bar that kids and stoners love. EXTRA Large",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_candy_pqs", HealthChangeAmount = 15, HungerChangeAmount = 15.0f,SleepChangeAmount = 5.0f, ItemSubType = ItemSubType.Snack },
            new ModItem("P's & Q's","The candy bar that kids and stoners love",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_choc_pq", HealthChangeAmount = 10, HungerChangeAmount = 10.0f,SleepChangeAmount = 5.0f, ItemSubType = ItemSubType.Snack },
            new ModItem("Meteorite Bar","Dark chocolate with a GOOEY core",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_choc_meto", HealthChangeAmount = 10, HungerChangeAmount = 10.0f,SleepChangeAmount = 5.0f, ItemSubType = ItemSubType.Snack },
            //UPNATOM
            new ModItem("Triple Burger", "Three times the meat, three times the cholesterol", eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Bacon Triple Cheese Melt", "More meat AND more bacon", eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            //BurgerShot
            new ModItem("Money Shot Meal",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_bs_burg1"
                ,PackageItemID = "prop_food_bs_tray_02", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("The Bleeder Meal","",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_bs_burg1"
                ,PackageItemID = "prop_food_bs_tray_02", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Torpedo Meal","Torpedo your hunger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bs_tray_03", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Meat Free Meal","For the bleeding hearts",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bs_tray_01", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Freedom Fries",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_bs_chips", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ThirstChangeAmount = -1.0f, ItemSubType = ItemSubType.Snack },
            //Bite
            new ModItem("Gut Buster Sandwich",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_burg2", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Ham and Tuna Sandwich",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_burg2", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Chef's Salad",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree} ,
            //BeefyBills
            new ModItem("Megacheese Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Double Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Kingsize Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Bacon Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 17, HungerChangeAmount = 17.0f, ItemSubType = ItemSubType.Entree },
            //Taco Bomb
            new ModItem("Breakfast Burrito",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bag1", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Deep Fried Salad",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Beef Bazooka",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_bs_burger2"
                ,PackageItemID = "prop_food_bag1", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Chimichingado Chiquito",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Cheesy Meat Flappers",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Volcano Mudsplatter Nachos",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            //Wigwam
            new ModItem("Wigwam Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Wigwam Cheeseburger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Big Wig Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 12, HungerChangeAmount = 12.0f, ItemSubType = ItemSubType.Entree },
            //Cluckin Bell
            new ModItem("Cluckin' Little Meal","May contain meat",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_03", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ThirstChangeAmount = 5.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new ModItem("Cluckin' Big Meal","200% bigger breasts",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_02", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Cluckin' Huge Meal",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_02", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Wing Piece",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_03", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new ModItem("Little Peckers",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_cb_tray_03", HealthChangeAmount = 5, HungerChangeAmount = 5.0f , ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Balls & Rings",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_burg3", HealthChangeAmount = 5, HungerChangeAmount = 5.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Side },
            new ModItem("Fowlburger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_food_burg1",
                PackageItemID = "prop_food_burg3", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            //Generic Restaurant
            //FancyDeli
            new ModItem("Chicken Club Salad",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Spicy Seafood Gumbo",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Muffaletta",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Zucchini Garden Pasta",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Pollo Mexicano",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Italian Cruz Po'boy",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Chipotle Chicken Panini",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //FancyFish
            new ModItem("Coconut Crusted Prawns",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Crab and Shrimp Louie",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Open-Faced Crab Melt",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("King Salmon",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Ahi Tuna",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Key Lime Pie",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //FancyGeneric
            new ModItem("Smokehouse Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_cs_burger_01", HealthChangeAmount = 20, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new ModItem("Chicken Critters Basket",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Prime Rib 16 oz",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Bone-In Ribeye",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_cs_steak", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Grilled Pork Chops",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Grilled Shrimp",eConsumableType.Eat, ItemType.Food) {
                PackageItemID = "prop_food_bag1" , HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //Noodles
            new ModItem("Juek Suk tong Mandu",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_ff_noodle_01", HealthChangeAmount = 10, HungerChangeAmount = 10.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree },
            new ModItem("Hayan Jam Pong",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_ff_noodle_02", HealthChangeAmount = 15, HungerChangeAmount = 15.0f, ThirstChangeAmount = 5.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Sal Gook Su Jam Pong",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_ff_noodle_01", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Chul Pan Bokkeum Jam Pong",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_ff_noodle_02", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Deul Gae Udon",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_ff_noodle_02", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Dakgogo Bokkeum Bap",eConsumableType.Eat, ItemType.Food) {
                ModelItemID = "prop_ff_noodle_01", HealthChangeAmount = 20, HungerChangeAmount = 20.0f, ThirstChangeAmount = 10.0f, ItemSubType = ItemSubType.Entree } ,
        });
    }
    private void DefaultConfig_Services()
    {
        ModItemsList.AddRange(new List<ModItem>
        {
            //Generic Hotel
            new ModItem("Room: Single Twin","Cheapest room for the most discerning client",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Room: Single Queen","Clean sheets on request",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Room: Double Queen","Have a little company, but don't want to get too close?",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Room: Single King","Please clean off all mirrors after use",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },

            //Viceroy Hotel
            new ModItem("City View King","Standard room with a view of the city",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("City View Deluxe King","Deluxe room with view of the city.",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Ocean View King","Standard room a full view of the ocean. ",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Grande King","XL Deluxe room with plenty of space and amenities.",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Grande Ocean View King","XL Deluxe room with with plenty of space and amenities and a view of the ocean",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Monarch Suite","Penthouse suite, reserved for the most discerning tastes",eConsumableType.Service, ItemType.Services) {ConsumeOnPurchase = true, MeasurementName = "Night" },
        });
    }
    private void DefaultConfig_Tools()
    {
        ModItemsList.AddRange(new List<ModItem>
        {
            //Generic Tools
            new ModItem("Screwdriver","Might get you into some locked things", ItemType.Tools) {
                ModelItemID = "prop_tool_screwdvr01", ToolType = ToolTypes.Screwdriver },
            new ModItem("Drill","2-Speed Battery Drill. Impact-resistant casing. Light, compact and easy to use.", ItemType.Tools) {
                ModelItemID = "prop_tool_drill", ToolType = ToolTypes.Drill  },
            new ModItem("Pliers","For mechanics, pipe bomb makers, and amateur dentists alike. When you really need to grab something.", ItemType.Tools) {
                ModelItemID = "prop_tool_pliers", ToolType = ToolTypes.Pliers  },
            new ModItem("Shovel","A lot of holes in the desert, and a lot of problems are buried in those holes. But you gotta do it right. I mean, you gotta have the hole already dug before you show up with a package in the trunk.", ItemType.Tools) {
                ModelItemID = "prop_tool_shovel", ToolType = ToolTypes.Shovel  },
            new ModItem("DIC Lighter","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged", ItemType.Tools) {
                ModelItemID = "p_cs_lighter_01", ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.01f },
            new ModItem("Bong","Also known as a water pipe", ItemType.Tools) {
                ModelItemID = "prop_bong_01", ToolType = ToolTypes.Bong } ,
            new ModItem("DIC Lighter Ultra","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Long burn version.", ItemType.Tools) {
                ModelItemID = "p_cs_lighter_01", ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.005f },
            new ModItem("Dippo Lighter","Want to have all the hassle of carrying a lighter only for it to be out of fluid when you need it? Dippo is for you!", ItemType.Tools) {
                ModelItemID = "v_res_tt_lighter", ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.05f },
            new ModItem("DIC Lighter Silver","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Too poor for gold?", ItemType.Tools) {
                ModelItemID = "ex_prop_exec_lighter_01", ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.02f },
            new ModItem("DIC Lighter Gold","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Golden so it must be good!", ItemType.Tools) {
                ModelItemID = "lux_prop_lighter_luxe", ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.02f },
        });
    }
    private void DefaultConfig_Vehicles()
    {
        ModItemsList.AddRange(new List<ModItem> {
            //Cars & Motorcycles
            new ModItem("Albany Alpha", "Blending modern performance and design with the classic luxury styling of a stately car, the Alpha is sleek, sexy and handles so well you'll forget you're driving it. Which could be a problem at 150 mph...", true, ItemType.Vehicles) { ModelItemID = "alpha" },
            new ModItem("Albany Roosevelt","Party like it's the Prohibition era in this armored 1920s limousine. Perfect for a gangster and his moll on their first date or their last. Let the Valentine's Day massacres commence.", true, ItemType.Vehicles) { ModelItemID = "btype" },
            new ModItem("Albany Fränken Stange","The unlikely product of Albany's design team leafing through a vintage car magazine while in the depths of a masculine overdose. The Franken Stange will make you the envy of goths, emo hipsters and vampire wannabes everywhere. Don't be fooled by what's left of its old world charm; the steering linkage may be from 1910, but the engine has just enough horsepower to tear itself (and you) to pieces at the first bump in the road.", true, ItemType.Vehicles) { ModelItemID = "btype2" },
            new ModItem("Albany Roosevelt Valor","They don't make them like they used to, which is a good thing because here at Albany we've completely run out of ideas. Lovingly remodelled, with room for a new suite of personal modifications, the latest edition of our classic Roosevelt represents a new height of criminal refinement, taking you back to the golden age of fraud, racketeering and murder when all you had to worry about were a few charges of tax evasion.", true, ItemType.Vehicles) { ModelItemID = "btype3" },
            new ModItem("Albany Buccaneer","With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", ItemType.Vehicles) { ModelItemID = "buccaneer" },
            new ModItem("Albany Buccaneer Custom","With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", true, ItemType.Vehicles) { ModelItemID = "buccaneer2" },
            new ModItem("Albany Cavalcade","You could scarcely cross the street without getting mown down by a soccer mom or drug dealer in one of these during the early 2000s. The glory days of the excessively-large, gas-guzzling SUV might be over, but the Cavalcade takes no prisoners.", ItemType.Vehicles) { ModelItemID = "cavalcade" },
            new ModItem("Albany Cavalcade 2","The old man luxury automobile, but once you sit inside this comfy car that steers like a boat, you'll know why your old man often fell asleep at the wheel.", ItemType.Vehicles) { ModelItemID = "cavalcade2" },
            new ModItem("Albany Emperor", ItemType.Vehicles) { ModelItemID = "emperor" },
            new ModItem("Albany Emperor 2", ItemType.Vehicles) { ModelItemID = "emperor2" },
            new ModItem("Albany Emperor 3", ItemType.Vehicles) { ModelItemID = "emperor3" },
            new ModItem("Albany Hermes", true, ItemType.Vehicles) { ModelItemID = "hermes" },
            new ModItem("Albany Lurcher", true, ItemType.Vehicles) { ModelItemID = "lurcher" },
            new ModItem("Albany Manana", ItemType.Vehicles) { ModelItemID = "manana" },
            new ModItem("Albany Manana Custom", true, ItemType.Vehicles) { ModelItemID = "manana2" },
            new ModItem("Albany Primo", ItemType.Vehicles) { ModelItemID = "primo" },
            new ModItem("Albany Primo Custom", true, ItemType.Vehicles) { ModelItemID = "primo2" },
            new ModItem("Albany Virgo", true, ItemType.Vehicles) { ModelItemID = "virgo" },
            new ModItem("Albany V-STR", true, ItemType.Vehicles) { ModelItemID = "vstr" },
            new ModItem("Albany Washington", ItemType.Vehicles) { ModelItemID = "washington" },
            new ModItem("Annis Elegy Retro Custom", true, ItemType.Vehicles) { ModelItemID = "elegy" },
            new ModItem("Annis Elegy RH8", ItemType.Vehicles) { ModelItemID = "elegy2" },
            new ModItem("Annis Euros", true, ItemType.Vehicles) { ModelItemID = "Euros" },
            new ModItem("Annis Hellion", true, ItemType.Vehicles) { ModelItemID = "hellion" },
            new ModItem("Annis RE-7B", true, ItemType.Vehicles) { ModelItemID = "le7b" },
            new ModItem("Annis Remus", true, ItemType.Vehicles) { ModelItemID = "remus" },
            new ModItem("Annis S80RR", true, ItemType.Vehicles) { ModelItemID = "s80" },
            new ModItem("Annis Savestra", true, ItemType.Vehicles) { ModelItemID = "savestra" },
            new ModItem("Annis ZR350", true, ItemType.Vehicles) { ModelItemID = "zr350" },
            new ModItem("Annis Apocalypse ZR380", true, ItemType.Vehicles) { ModelItemID = "zr380" },
            new ModItem("Annis Future Shock ZR380", true, ItemType.Vehicles) { ModelItemID = "zr3802" },
            new ModItem("Annis Nightmare ZR380", true, ItemType.Vehicles) { ModelItemID = "zr3803" },
            new ModItem("Benefactor Apocalypse Bruiser", true, ItemType.Vehicles) { ModelItemID = "bruiser" },
            new ModItem("Benefactor Future Shock Bruiser", true, ItemType.Vehicles) { ModelItemID = "bruiser2" },
            new ModItem("Benefactor Nightmare Bruiser", true, ItemType.Vehicles) { ModelItemID = "bruiser3" },
            new ModItem("Benefactor Dubsta", ItemType.Vehicles) { ModelItemID = "dubsta" },
            new ModItem("Benefactor Dubsta 2", ItemType.Vehicles) { ModelItemID = "dubsta2" },
            new ModItem("Benefactor Dubsta 6x6", true, ItemType.Vehicles) { ModelItemID = "dubsta3" },
            new ModItem("Benefactor Feltzer", ItemType.Vehicles) { ModelItemID = "feltzer2" },
            new ModItem("Benefactor Stirling GT", true, ItemType.Vehicles) { ModelItemID = "feltzer3" },
            new ModItem("Benefactor Glendale", true, ItemType.Vehicles) { ModelItemID = "glendale" },
            new ModItem("Benefactor Glendale Custom", true, ItemType.Vehicles) { ModelItemID = "glendale2" },
            new ModItem("Benefactor Turreted Limo", true, ItemType.Vehicles) { ModelItemID = "limo2" },
            new ModItem("Benefactor BR8", true, ItemType.Vehicles) { ModelItemID = "openwheel1" },
            new ModItem("Benefactor Panto", true, ItemType.Vehicles) { ModelItemID = "panto" },
            new ModItem("Benefactor Schafter", "Good-looking yet utilitarian, sexy yet asexual, slender yet terrifyingly powerful, the Schafter is German engineering at its very finest.", ItemType.Vehicles) { ModelItemID = "schafter2" },
            new ModItem("Benefactor Schafter V12", "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has a V12 engine.", true, ItemType.Vehicles) { ModelItemID = "schafter3" },
            new ModItem("Benefactor Schafter LWB", "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has an extended wheelbase.", true, ItemType.Vehicles) { ModelItemID = "schafter4" },
            new ModItem("Benefactor Schafter V12 (Armored)", true, ItemType.Vehicles) { ModelItemID = "schafter5" },
            new ModItem("Benefactor Schafter LWB (Armored)", true, ItemType.Vehicles) { ModelItemID = "schafter6" },
            new ModItem("Benefactor Schwartzer", "Say what you will about the Germans - they know luxury. And their economy is the only one worth a crap in Europe. This model has all kinds of extras - too many to list for legal reasons.", ItemType.Vehicles) { ModelItemID = "schwarzer" },
            new ModItem("Benefactor Serrano", "Fun fact: what's the fastest growing market in the American auto industry? That's right! Compact SUVs! And do you know why? That's right! Neither do we! And is that a good enough reason to buy one? That's right! It had better be!", ItemType.Vehicles) { ModelItemID = "serrano" },
            new ModItem("Benefactor Surano", "This is luxury reasserted. Right in your neighbour's face. Boom. You like that. That's right, you are better than him, and you could have his wife if you wanted. Try it on with her as soon as she sees this ride. You'll be a double benefactor.", ItemType.Vehicles) { ModelItemID = "Surano" },
            new ModItem("Benefactor XLS", true, ItemType.Vehicles) { ModelItemID = "xls" },
            new ModItem("Benefactor XLS (Armored)", true, ItemType.Vehicles) { ModelItemID = "xls2" },
            new ModItem("Benefactor Krieger", true, ItemType.Vehicles) { ModelItemID = "krieger" },
            new ModItem("Benefactor Schlagen GT", true, ItemType.Vehicles) { ModelItemID = "schlagen" },
            new ModItem("Benefactor Streiter", true, ItemType.Vehicles) { ModelItemID = "streiter" },
            new ModItem("Benefactor Terrorbyte", true, ItemType.Vehicles) { ModelItemID = "terbyte" },
            new ModItem("BF Injection", ItemType.Vehicles) { ModelItemID = "BfInjection" },
            new ModItem("BF Bifta", true, ItemType.Vehicles) { ModelItemID = "bifta" },
            new ModItem("BF Club", true, ItemType.Vehicles) { ModelItemID = "club" },
            new ModItem("BF Dune Buggy", ItemType.Vehicles) { ModelItemID = "dune" },
            new ModItem("BF Dune FAV", true, ItemType.Vehicles) { ModelItemID = "dune3" },
            new ModItem("BF Raptor", true, ItemType.Vehicles) { ModelItemID = "raptor" },
            new ModItem("BF Surfer", ItemType.Vehicles) { ModelItemID = "SURFER" },
            new ModItem("BF Surfer", ItemType.Vehicles) { ModelItemID = "Surfer2" },
            new ModItem("BF Weevil", true, ItemType.Vehicles) { ModelItemID = "weevil" },
            new ModItem("Bollokan Prairie", ItemType.Vehicles) { ModelItemID = "prairie" },
            new ModItem("Bravado Banshee", ItemType.Vehicles) { ModelItemID = "banshee" },
            new ModItem("Bravado Banshee 900R", true, ItemType.Vehicles) { ModelItemID = "banshee2" },
            new ModItem("Bravado Bison", ItemType.Vehicles) { ModelItemID = "bison" },
            new ModItem("Bravado Bison 2", ItemType.Vehicles) { ModelItemID = "Bison2" },
            new ModItem("Bravado Bison 3", ItemType.Vehicles) { ModelItemID = "Bison3" },
            new ModItem("Bravado Buffalo", ItemType.Vehicles) { ModelItemID = "buffalo" },
            new ModItem("Bravado Buffalo S", ItemType.Vehicles) { ModelItemID = "buffalo2" },
            new ModItem("Bravado Sprunk Buffalo", ItemType.Vehicles) { ModelItemID = "buffalo3" },
            new ModItem("Bravado Duneloader", ItemType.Vehicles) { ModelItemID = "dloader" },
            new ModItem("Bravado Gauntlet", ItemType.Vehicles) { ModelItemID = "Gauntlet" },
            new ModItem("Bravado Redwood Gauntlet", ItemType.Vehicles) { ModelItemID = "gauntlet2" },
            new ModItem("Bravado Gauntlet Classic", true, ItemType.Vehicles) { ModelItemID = "gauntlet3" },
            new ModItem("Bravado Gauntlet Hellfire", true, ItemType.Vehicles) { ModelItemID = "gauntlet4" },
            new ModItem("Bravado Gauntlet Classic Custom", true, ItemType.Vehicles) { ModelItemID = "gauntlet5" },
            new ModItem("Bravado Gresley", ItemType.Vehicles) { ModelItemID = "gresley" },
            new ModItem("Bravado Half-track", true, ItemType.Vehicles) { ModelItemID = "halftrack" },
            new ModItem("Bravado Apocalypse Sasquatch", true, ItemType.Vehicles) { ModelItemID = "monster3" },
            new ModItem("Bravado Future Shock Sasquatch", true, ItemType.Vehicles) { ModelItemID = "monster4" },
            new ModItem("Bravado Nightmare Sasquatch", true, ItemType.Vehicles) { ModelItemID = "monster5" },
            new ModItem("Bravado Paradise", true, ItemType.Vehicles) { ModelItemID = "paradise" },
            new ModItem("Bravado Rat-Truck", true, ItemType.Vehicles) { ModelItemID = "ratloader2" },
            new ModItem("Bravado Rumpo", ItemType.Vehicles) { ModelItemID = "rumpo" },
            new ModItem("Bravado Rumpo 2", ItemType.Vehicles) { ModelItemID = "rumpo2" },
            new ModItem("Bravado Rumpo Custom", true, ItemType.Vehicles) { ModelItemID = "rumpo3" },
            new ModItem("Bravado Verlierer", true, ItemType.Vehicles) { ModelItemID = "verlierer2" },
            new ModItem("Bravado Youga", ItemType.Vehicles) { ModelItemID = "youga" },
            new ModItem("Bravado Youga Classic", true, ItemType.Vehicles) { ModelItemID = "youga2" },
            new ModItem("Bravado Youga Classic 4x4", true, ItemType.Vehicles) { ModelItemID = "youga3" },
            new ModItem("Brute Boxville", ItemType.Vehicles) { ModelItemID = "boxville" },
            new ModItem("Brute Boxville 3", ItemType.Vehicles) { ModelItemID = "boxville3" },
            new ModItem("Brute Boxville 4", true, ItemType.Vehicles) { ModelItemID = "boxville4" },
            new ModItem("Brute Camper", ItemType.Vehicles) { ModelItemID = "CAMPER" },
            new ModItem("Brute Pony", ItemType.Vehicles) { ModelItemID = "pony" },
            new ModItem("Brute Pony 2", ItemType.Vehicles) { ModelItemID = "pony2" },
            new ModItem("Brute Stockade", ItemType.Vehicles) { ModelItemID = "stockade" },
            new ModItem("Brute Stockade 3", ItemType.Vehicles) { ModelItemID = "stockade3" },
            new ModItem("Brute Tipper", ItemType.Vehicles) { ModelItemID = "TipTruck" },
            new ModItem("Canis Bodhi", ItemType.Vehicles) { ModelItemID = "Bodhi2" },
            new ModItem("Canis Crusader", ItemType.Vehicles) { ModelItemID = "CRUSADER" },
            new ModItem("Canis Freecrawler", true, ItemType.Vehicles) { ModelItemID = "freecrawler" },
            new ModItem("Canis Kalahari", true, ItemType.Vehicles) { ModelItemID = "kalahari" },
            new ModItem("Canis Kamacho", true, ItemType.Vehicles) { ModelItemID = "kamacho" },
            new ModItem("Canis Mesa", ItemType.Vehicles) { ModelItemID = "MESA" },
            new ModItem("Canis Mesa 2", ItemType.Vehicles) { ModelItemID = "mesa2" },
            new ModItem("Canis Mesa 3", ItemType.Vehicles) { ModelItemID = "MESA3" },
            new ModItem("Canis Seminole", ItemType.Vehicles) { ModelItemID = "Seminole" },
            new ModItem("Canis Seminole Frontier", true, ItemType.Vehicles) { ModelItemID = "seminole2" },
            new ModItem("Chariot Romero Hearse", ItemType.Vehicles) { ModelItemID = "romero" },
            new ModItem("Cheval Fugitive", ItemType.Vehicles) { ModelItemID = "fugitive" },
            new ModItem("Cheval Marshall", ItemType.Vehicles) { ModelItemID = "marshall" },
            new ModItem("Cheval Picador", ItemType.Vehicles) { ModelItemID = "picador" },
            new ModItem("Cheval Surge", ItemType.Vehicles) { ModelItemID = "surge" },
            new ModItem("Cheval Taipan", true, ItemType.Vehicles) { ModelItemID = "taipan" },
            new ModItem("Coil Brawler", true, ItemType.Vehicles) { ModelItemID = "brawler" },
            new ModItem("Coil Cyclone", true, ItemType.Vehicles) { ModelItemID = "cyclone" },
            new ModItem("Coil Raiden", true, ItemType.Vehicles) { ModelItemID = "raiden" },
            new ModItem("Coil Voltic", ItemType.Vehicles) { ModelItemID = "voltic" },
            new ModItem("Coil Rocket Voltic", true, ItemType.Vehicles) { ModelItemID = "voltic2" },
            new ModItem("Declasse Asea", ItemType.Vehicles) { ModelItemID = "asea" },
            new ModItem("Declasse Asea", ItemType.Vehicles) { ModelItemID = "asea2" },
            new ModItem("Declasse Apocalypse Brutus", true, ItemType.Vehicles) { ModelItemID = "brutus" },
            new ModItem("Declasse Future Shock Brutus", true, ItemType.Vehicles) { ModelItemID = "brutus2" },
            new ModItem("Declasse Nightmare Brutus", true, ItemType.Vehicles) { ModelItemID = "brutus3" },
            new ModItem("Declasse Burrito", ItemType.Vehicles) { ModelItemID = "Burrito" },
            new ModItem("Declasse Bugstars Burrito", ItemType.Vehicles) { ModelItemID = "burrito2" },
            new ModItem("Declasse Burrito 3", ItemType.Vehicles) { ModelItemID = "burrito3" },
            new ModItem("Declasse Burrito 4", ItemType.Vehicles) { ModelItemID = "Burrito4" },
            new ModItem("Declasse Burrito 5", ItemType.Vehicles) { ModelItemID = "burrito5" },
            new ModItem("Declasse Gang Burrito", ItemType.Vehicles) { ModelItemID = "gburrito" },
            new ModItem("Declasse Gang Burrito 2", true, ItemType.Vehicles) { ModelItemID = "gburrito2" },
            new ModItem("Declasse Granger", ItemType.Vehicles) { ModelItemID = "GRANGER" },
            new ModItem("Declasse Hotring Sabre", true, ItemType.Vehicles) { ModelItemID = "hotring" },
            new ModItem("Declasse Impaler", true, ItemType.Vehicles) { ModelItemID = "impaler" },
            new ModItem("Declasse Apocalypse Impaler", true, ItemType.Vehicles) { ModelItemID = "impaler2" },
            new ModItem("Declasse Future Shock Impaler", true, ItemType.Vehicles) { ModelItemID = "impaler3" },
            new ModItem("Declasse Nightmare Impaler", true, ItemType.Vehicles) { ModelItemID = "impaler4" },
            new ModItem("Declasse Lifeguard", ItemType.Vehicles) { ModelItemID = "lguard" },
            new ModItem("Declasse Mamba", true, ItemType.Vehicles) { ModelItemID = "mamba" },
            new ModItem("Declasse Moonbeam", true, ItemType.Vehicles) { ModelItemID = "moonbeam" },
            new ModItem("Declasse Moonbeam Custom", true, ItemType.Vehicles) { ModelItemID = "moonbeam2" },
            new ModItem("Declasse DR1", true, ItemType.Vehicles) { ModelItemID = "openwheel2" },
            new ModItem("Declasse Premier", ItemType.Vehicles) { ModelItemID = "premier" },
            new ModItem("Declasse Rancher XL", ItemType.Vehicles) { ModelItemID = "RancherXL" },
            new ModItem("Declasse Rancher XL 2", ItemType.Vehicles) { ModelItemID = "rancherxl2" },
            new ModItem("Declasse Rhapsody", true, ItemType.Vehicles) { ModelItemID = "rhapsody" },
            new ModItem("Declasse Sabre Turbo", ItemType.Vehicles) { ModelItemID = "sabregt" },
            new ModItem("Declasse Sabre Turbo Custom", true, ItemType.Vehicles) { ModelItemID = "sabregt2" },
            new ModItem("Declasse Scramjet", true, ItemType.Vehicles) { ModelItemID = "scramjet" },
            new ModItem("Declasse Stallion", ItemType.Vehicles) { ModelItemID = "stalion" },
            new ModItem("Declasse Burger Shot Stallion", ItemType.Vehicles) { ModelItemID = "stalion2" },
            new ModItem("Declasse Tampa", true, ItemType.Vehicles) { ModelItemID = "tampa" },
            new ModItem("Declasse Drift Tampa", true, ItemType.Vehicles) { ModelItemID = "tampa2" },
            new ModItem("Declasse Weaponized Tampa", true, ItemType.Vehicles) { ModelItemID = "tampa3" },
            new ModItem("Declasse Tornado", ItemType.Vehicles) { ModelItemID = "tornado" },
            new ModItem("Declasse Tornado 2", ItemType.Vehicles) { ModelItemID = "tornado2" },
            new ModItem("Declasse Tornado 3", ItemType.Vehicles) { ModelItemID = "tornado3" },
            new ModItem("Declasse Tornado 4", ItemType.Vehicles) { ModelItemID = "tornado4" },
            new ModItem("Declasse Tornado Custom", true, ItemType.Vehicles) { ModelItemID = "tornado5" },
            new ModItem("Declasse Tornado Rat Rod", true, ItemType.Vehicles) { ModelItemID = "tornado6" },
            new ModItem("Declasse Tulip", true, ItemType.Vehicles) { ModelItemID = "tulip" },
            new ModItem("Declasse Vamos", true, ItemType.Vehicles) { ModelItemID = "vamos" },
            new ModItem("Declasse Vigero", ItemType.Vehicles) { ModelItemID = "vigero" },
            new ModItem("Declasse Voodoo Custom", true, ItemType.Vehicles) { ModelItemID = "voodoo" },
            new ModItem("Declasse Voodoo", ItemType.Vehicles) { ModelItemID = "voodoo2" },
            new ModItem("Declasse Yosemite", true, ItemType.Vehicles) { ModelItemID = "yosemite" },
            new ModItem("Declasse Drift Yosemite", true, ItemType.Vehicles) { ModelItemID = "yosemite2" },
            new ModItem("Declasse Yosemite Rancher", true, ItemType.Vehicles) { ModelItemID = "yosemite3" },
            new ModItem("Dewbauchee Exemplar", ItemType.Vehicles) { ModelItemID = "exemplar" },
            new ModItem("Dewbauchee JB 700", ItemType.Vehicles) { ModelItemID = "jb700" },
            new ModItem("Dewbauchee JB 700W", true, ItemType.Vehicles) { ModelItemID = "jb7002" },
            new ModItem("Dewbauchee Massacro", true, ItemType.Vehicles) { ModelItemID = "massacro" },
            new ModItem("Dewbauchee Massacro (Racecar)", true, ItemType.Vehicles) { ModelItemID = "massacro2" },
            new ModItem("Dewbauchee Rapid GT", ItemType.Vehicles) { ModelItemID = "RapidGT" },
            new ModItem("Dewbauchee Rapid GT 2", ItemType.Vehicles) { ModelItemID = "RapidGT2" },
            new ModItem("Dewbauchee Rapid GT Classic", true, ItemType.Vehicles) { ModelItemID = "rapidgt3" },
            new ModItem("Dewbauchee Seven-70", true, ItemType.Vehicles) { ModelItemID = "SEVEN70" },
            new ModItem("Dewbauchee Specter", true, ItemType.Vehicles) { ModelItemID = "SPECTER" },
            new ModItem("Dewbauchee Specter Custom", true, ItemType.Vehicles) { ModelItemID = "SPECTER2" },
            new ModItem("Dewbauchee Vagner", true, ItemType.Vehicles) { ModelItemID = "vagner" },
            new ModItem("Dinka Akuma", ItemType.Vehicles) { ModelItemID = "akuma" },
            new ModItem("Dinka Blista", ItemType.Vehicles) { ModelItemID = "blista" },
            new ModItem("Dinka Blista Compact", ItemType.Vehicles) { ModelItemID = "blista2" },
            new ModItem("Dinka Go Go Monkey Blista", ItemType.Vehicles) { ModelItemID = "blista3" },
            new ModItem("Dinka Double-T", ItemType.Vehicles) { ModelItemID = "double" },
            new ModItem("Dinka Enduro", true, ItemType.Vehicles) { ModelItemID = "enduro" },
            new ModItem("Dinka Jester", true, ItemType.Vehicles) { ModelItemID = "jester" },
            new ModItem("Dinka Jester (Racecar)", true, ItemType.Vehicles) { ModelItemID = "jester2" },
            new ModItem("Dinka Jester Classic", true, ItemType.Vehicles) { ModelItemID = "jester3" },
            new ModItem("Dinka Jester RR", true, ItemType.Vehicles) { ModelItemID = "jester4" },
            new ModItem("Dinka Blista Kanjo", true, ItemType.Vehicles) { ModelItemID = "kanjo" },
            new ModItem("Dinka RT3000", true, ItemType.Vehicles) { ModelItemID = "rt3000" },
            new ModItem("Dinka Sugoi", true, ItemType.Vehicles) { ModelItemID = "Sugoi" },
            new ModItem("Dinka Thrust", true, ItemType.Vehicles) { ModelItemID = "thrust" },
            new ModItem("Dinka Verus", true, ItemType.Vehicles) { ModelItemID = "verus" },
            new ModItem("Dinka Veto Classic", true, ItemType.Vehicles) { ModelItemID = "veto" },
            new ModItem("Dinka Veto Modern", true, ItemType.Vehicles) { ModelItemID = "veto2" },
            new ModItem("Dinka Vindicator", true, ItemType.Vehicles) { ModelItemID = "vindicator" },
            new ModItem("Dundreary Landstalker", ItemType.Vehicles) { ModelItemID = "landstalker" },
            new ModItem("Dundreary Landstalker XL", true, ItemType.Vehicles) { ModelItemID = "landstalker2" },
            new ModItem("Dundreary Regina", ItemType.Vehicles) { ModelItemID = "regina" },
            new ModItem("Dundreary Stretch", ItemType.Vehicles) { ModelItemID = "stretch" },
            new ModItem("Dundreary Virgo Classic Custom", true, ItemType.Vehicles) { ModelItemID = "virgo2" },
            new ModItem("Dundreary Virgo Classic", true, ItemType.Vehicles) { ModelItemID = "virgo3" },
            new ModItem("Emperor Habanero", ItemType.Vehicles) { ModelItemID = "habanero" },
            new ModItem("Emperor ETR1", true, ItemType.Vehicles) { ModelItemID = "sheava" },
            new ModItem("Emperor Vectre", true, ItemType.Vehicles) { ModelItemID = "vectre" },
            new ModItem("Enus Cognoscenti 55", true, ItemType.Vehicles) { ModelItemID = "cog55" },
            new ModItem("Enus Cognoscenti 55 (Armored)", true, ItemType.Vehicles) { ModelItemID = "cog552" },
            new ModItem("Enus Cognoscenti Cabrio", ItemType.Vehicles) { ModelItemID = "cogcabrio" },
            new ModItem("Enus Cognoscenti", true, ItemType.Vehicles) { ModelItemID = "cognoscenti" },
            new ModItem("Enus Cognoscenti (Armored)", true, ItemType.Vehicles) { ModelItemID = "cognoscenti2" },
            new ModItem("Enus Huntley S", true, ItemType.Vehicles) { ModelItemID = "huntley" },
            new ModItem("Enus Paragon R", true, ItemType.Vehicles) { ModelItemID = "paragon" },
            new ModItem("Enus Paragon R (Armored)", true, ItemType.Vehicles) { ModelItemID = "paragon2" },
            new ModItem("Enus Stafford", true, ItemType.Vehicles) { ModelItemID = "stafford" },
            new ModItem("Enus Super Diamond", ItemType.Vehicles) { ModelItemID = "superd" },
            new ModItem("Enus Windsor", true, ItemType.Vehicles) { ModelItemID = "windsor" },
            new ModItem("Enus Windsor Drop", true, ItemType.Vehicles) { ModelItemID = "windsor2" },
            new ModItem("Fathom FQ 2", ItemType.Vehicles) { ModelItemID = "fq2" },
            new ModItem("Gallivanter Baller", ItemType.Vehicles) { ModelItemID = "Baller" },
            new ModItem("Gallivanter Baller 2", ItemType.Vehicles) { ModelItemID = "baller2" },
            new ModItem("Gallivanter Baller LE", true, ItemType.Vehicles) { ModelItemID = "baller3" },
            new ModItem("Gallivanter Baller LE LWB", true, ItemType.Vehicles) { ModelItemID = "baller4" },
            new ModItem("Gallivanter Baller LE (Armored)", true, ItemType.Vehicles) { ModelItemID = "baller5" },
            new ModItem("Gallivanter Baller LE LWB (Armored)", true, ItemType.Vehicles) { ModelItemID = "baller6" },
            new ModItem("Grotti Bestia GTS", true, ItemType.Vehicles) { ModelItemID = "bestiagts" },
            new ModItem("Grotti Brioso R/A", true, ItemType.Vehicles) { ModelItemID = "brioso" },
            new ModItem("Grotti Brioso 300", true, ItemType.Vehicles) { ModelItemID = "brioso2" },
            new ModItem("Grotti Carbonizzare", ItemType.Vehicles) { ModelItemID = "carbonizzare" },
            new ModItem("Grotti Cheetah", ItemType.Vehicles) { ModelItemID = "cheetah" },
            new ModItem("Grotti Cheetah Classic", true, ItemType.Vehicles) { ModelItemID = "cheetah2" },
            new ModItem("Grotti Furia", true, ItemType.Vehicles) { ModelItemID = "furia" },
            new ModItem("Grotti GT500", true, ItemType.Vehicles) { ModelItemID = "gt500" },
            new ModItem("Grotti Itali GTO", true, ItemType.Vehicles) { ModelItemID = "italigto" },
            new ModItem("Grotti Itali RSX", true, ItemType.Vehicles) { ModelItemID = "italirsx" },
            new ModItem("Grotti X80 Proto", true, ItemType.Vehicles) { ModelItemID = "prototipo" },
            new ModItem("Grotti Stinger", ItemType.Vehicles) { ModelItemID = "stinger" },
            new ModItem("Grotti Stinger GT", ItemType.Vehicles) { ModelItemID = "stingergt" },
            new ModItem("Grotti Turismo Classic", true, ItemType.Vehicles) { ModelItemID = "turismo2" },
            new ModItem("Grotti Turismo R", true, ItemType.Vehicles) { ModelItemID = "turismor" },
            new ModItem("Grotti Visione", true, ItemType.Vehicles) { ModelItemID = "visione" },
            new ModItem("Hijak Khamelion", ItemType.Vehicles) { ModelItemID = "khamelion" },
            new ModItem("Hijak Ruston", true, ItemType.Vehicles) { ModelItemID = "ruston" },
            new ModItem("HVY Barracks Semi", ItemType.Vehicles) { ModelItemID = "BARRACKS2" },
            new ModItem("HVY Biff", ItemType.Vehicles) { ModelItemID = "Biff" },
            new ModItem("HVY Dozer", ItemType.Vehicles) { ModelItemID = "bulldozer" },
            new ModItem("HVY Cutter", ItemType.Vehicles) { ModelItemID = "cutter" },
            new ModItem("HVY Dump", ItemType.Vehicles) { ModelItemID = "dump" },
            new ModItem("HVY Forklift", ItemType.Vehicles) { ModelItemID = "FORKLIFT" },
            new ModItem("HVY Insurgent Pick-Up", true, ItemType.Vehicles) { ModelItemID = "insurgent" },
            new ModItem("HVY Insurgent", true, ItemType.Vehicles) { ModelItemID = "insurgent2" },
            new ModItem("HVY Insurgent Pick-Up Custom", true, ItemType.Vehicles) { ModelItemID = "insurgent3" },
            new ModItem("HVY Menacer", true, ItemType.Vehicles) { ModelItemID = "menacer" },
            new ModItem("HVY Mixer", ItemType.Vehicles) { ModelItemID = "Mixer" },
            new ModItem("HVY Mixer 2", ItemType.Vehicles) { ModelItemID = "Mixer2" },
            new ModItem("HVY Nightshark", true, ItemType.Vehicles) { ModelItemID = "nightshark" },
            new ModItem("HVY Apocalypse Scarab", true, ItemType.Vehicles) { ModelItemID = "scarab" },
            new ModItem("HVY Future Shock Scarab", true, ItemType.Vehicles) { ModelItemID = "scarab2" },
            new ModItem("HVY Nightmare Scarab", true, ItemType.Vehicles) { ModelItemID = "scarab3" },
            new ModItem("Imponte Deluxo", true, ItemType.Vehicles) { ModelItemID = "deluxo" },
            new ModItem("Imponte Dukes", ItemType.Vehicles) { ModelItemID = "dukes" },
            new ModItem("Imponte Duke O'Death", ItemType.Vehicles) { ModelItemID = "dukes2" },
            new ModItem("Imponte Beater Dukes", true, ItemType.Vehicles) { ModelItemID = "dukes3" },
            new ModItem("Imponte Nightshade", true, ItemType.Vehicles) { ModelItemID = "nightshade" },
            new ModItem("Imponte Phoenix", ItemType.Vehicles) { ModelItemID = "Phoenix" },
            new ModItem("Imponte Ruiner", ItemType.Vehicles) { ModelItemID = "ruiner" },
            new ModItem("Imponte Ruiner 2000", true, ItemType.Vehicles) { ModelItemID = "ruiner2" },
            new ModItem("Imponte Ruiner", true, ItemType.Vehicles) { ModelItemID = "ruiner3" },
            new ModItem("Invetero Coquette", ItemType.Vehicles) { ModelItemID = "coquette" },
            new ModItem("Invetero Coquette Classic", true, ItemType.Vehicles) { ModelItemID = "coquette2" },
            new ModItem("Invetero Coquette BlackFin", true, ItemType.Vehicles) { ModelItemID = "coquette3" },
            new ModItem("Invetero Coquette D10", true, ItemType.Vehicles) { ModelItemID = "coquette4" },
            new ModItem("JoBuilt Hauler", ItemType.Vehicles) { ModelItemID = "Hauler" },
            new ModItem("JoBuilt Hauler Custom", true, ItemType.Vehicles) { ModelItemID = "Hauler2" },
            new ModItem("JoBuilt Phantom", ItemType.Vehicles) { ModelItemID = "Phantom" },
            new ModItem("JoBuilt Phantom Wedge", true, ItemType.Vehicles) { ModelItemID = "phantom2" },
            new ModItem("JoBuilt Phantom Custom", true, ItemType.Vehicles) { ModelItemID = "phantom3" },
            new ModItem("JoBuilt Rubble", ItemType.Vehicles) { ModelItemID = "Rubble" },
            new ModItem("Karin Asterope", ItemType.Vehicles) { ModelItemID = "asterope" },
            new ModItem("Karin BeeJay XL", ItemType.Vehicles) { ModelItemID = "BjXL" },
            new ModItem("Karin Calico GTF", true, ItemType.Vehicles) { ModelItemID = "calico" },
            new ModItem("Karin Dilettante", ItemType.Vehicles) { ModelItemID = "dilettante" },
            new ModItem("Karin Dilettante 2", ItemType.Vehicles) { ModelItemID = "dilettante2" },
            new ModItem("Karin Everon", true, ItemType.Vehicles) { ModelItemID = "everon" },
            new ModItem("Karin Futo", ItemType.Vehicles) { ModelItemID = "futo" },
            new ModItem("Karin Futo GTX", true, ItemType.Vehicles) { ModelItemID = "futo2" },
            new ModItem("Karin Intruder", ItemType.Vehicles) { ModelItemID = "intruder" },
            new ModItem("Karin Kuruma", true, ItemType.Vehicles) { ModelItemID = "kuruma" },
            new ModItem("Karin Kuruma (armored)", true, ItemType.Vehicles) { ModelItemID = "kuruma2" },
            new ModItem("Karin Previon", true, ItemType.Vehicles) { ModelItemID = "previon" },
            new ModItem("Karin Rusty Rebel", ItemType.Vehicles) { ModelItemID = "Rebel" },
            new ModItem("Karin Rebel", ItemType.Vehicles) { ModelItemID = "rebel2" },
            new ModItem("Karin Sultan", ItemType.Vehicles) { ModelItemID = "sultan" },
            new ModItem("Karin Sultan Classic", true, ItemType.Vehicles) { ModelItemID = "sultan2" },
            new ModItem("Karin Sultan RS Classic", true, ItemType.Vehicles) { ModelItemID = "sultan3" },
            new ModItem("Karin Sultan RS", true, ItemType.Vehicles) { ModelItemID = "sultanrs" },
            new ModItem("Karin Technical", true, ItemType.Vehicles) { ModelItemID = "technical" },
            new ModItem("Karin Technical Custom", true, ItemType.Vehicles) { ModelItemID = "technical3" },
            new ModItem("Karin 190z", true, ItemType.Vehicles) { ModelItemID = "z190" },
            new ModItem("Lampadati Casco", true, ItemType.Vehicles) { ModelItemID = "casco" },
            new ModItem("Lampadati Felon", ItemType.Vehicles) { ModelItemID = "felon" },
            new ModItem("Lampadati Felon GT", ItemType.Vehicles) { ModelItemID = "felon2" },
            new ModItem("Lampadati Furore GT", true, ItemType.Vehicles) { ModelItemID = "furoregt" },
            new ModItem("Lampadati Michelli GT", true, ItemType.Vehicles) { ModelItemID = "michelli" },
            new ModItem("Lampadati Pigalle", true, ItemType.Vehicles) { ModelItemID = "pigalle" },
            new ModItem("Lampadati Tropos Rallye", true, ItemType.Vehicles) { ModelItemID = "tropos" },
            new ModItem("Lampadati Komoda", true, ItemType.Vehicles) { ModelItemID = "komoda" },
            new ModItem("Lampadati Novak", true, ItemType.Vehicles) { ModelItemID = "Novak" },
            new ModItem("Lampadati Tigon", true, ItemType.Vehicles) { ModelItemID = "tigon" },
            new ModItem("Lampadati Viseris", true, ItemType.Vehicles) { ModelItemID = "viseris" },
            new ModItem("LCC Avarus", true, ItemType.Vehicles) { ModelItemID = "avarus" },
            new ModItem("LCC Hexer", ItemType.Vehicles) { ModelItemID = "hexer" },
            new ModItem("LCC Innovation", true, ItemType.Vehicles) { ModelItemID = "innovation" },
            new ModItem("LCC Sanctus", true, ItemType.Vehicles) { ModelItemID = "sanctus" },
            new ModItem("Maibatsu Manchez", true, ItemType.Vehicles) { ModelItemID = "manchez" },
            new ModItem("Maibatsu Manchez Scout", true, ItemType.Vehicles) { ModelItemID = "manchez2" },
            new ModItem("Maibatsu Mule", ItemType.Vehicles) { ModelItemID = "Mule" },
            new ModItem("Maibatsu Mule", ItemType.Vehicles) { ModelItemID = "Mule2" },
            new ModItem("Maibatsu Mule", true, ItemType.Vehicles) { ModelItemID = "Mule3" },
            new ModItem("Maibatsu Mule Custom", true, ItemType.Vehicles) { ModelItemID = "mule4" },
            new ModItem("Maibatsu Penumbra", ItemType.Vehicles) { ModelItemID = "penumbra" },
            new ModItem("Maibatsu Penumbra FF", true, ItemType.Vehicles) { ModelItemID = "penumbra2" },
            new ModItem("Maibatsu Sanchez (livery)", ItemType.Vehicles) { ModelItemID = "Sanchez" },
            new ModItem("Maibatsu Sanchez", ItemType.Vehicles) { ModelItemID = "sanchez2" },
            new ModItem("Mammoth Patriot", ItemType.Vehicles) { ModelItemID = "patriot" },
            new ModItem("Mammoth Patriot Stretch", true, ItemType.Vehicles) { ModelItemID = "patriot2" },
            new ModItem("Mammoth Squaddie", true, ItemType.Vehicles) { ModelItemID = "squaddie" },
            new ModItem("Maxwell Asbo", true, ItemType.Vehicles) { ModelItemID = "asbo" },
            new ModItem("Maxwell Vagrant", true, ItemType.Vehicles) { ModelItemID = "vagrant" },
            new ModItem("MTL Brickade", true, ItemType.Vehicles) { ModelItemID = "brickade" },
            new ModItem("MTL Apocalypse Cerberus", true, ItemType.Vehicles) { ModelItemID = "cerberus" },
            new ModItem("MTL Future Shock Cerberus", true, ItemType.Vehicles) { ModelItemID = "cerberus2" },
            new ModItem("MTL Nightmare Cerberus", true, ItemType.Vehicles) { ModelItemID = "cerberus3" },
            new ModItem("MTL Fire Truck", ItemType.Vehicles) { ModelItemID = "firetruk" },
            new ModItem("MTL Flatbed", ItemType.Vehicles) { ModelItemID = "FLATBED" },
            new ModItem("MTL Packer", ItemType.Vehicles) { ModelItemID = "Packer" },
            new ModItem("MTL Pounder", ItemType.Vehicles) { ModelItemID = "Pounder" },
            new ModItem("MTL Pounder Custom", true, ItemType.Vehicles) { ModelItemID = "pounder2" },
            new ModItem("MTL Dune", true, ItemType.Vehicles) { ModelItemID = "rallytruck" },
            new ModItem("MTL Wastelander", true, ItemType.Vehicles) { ModelItemID = "wastelander" },
            new ModItem("Nagasaki BF400", true, ItemType.Vehicles) { ModelItemID = "bf400" },
            new ModItem("Nagasaki Blazer", ItemType.Vehicles) { ModelItemID = "blazer" },
            new ModItem("Nagasaki Blazer Lifeguard", ItemType.Vehicles) { ModelItemID = "blazer2" },
            new ModItem("Nagasaki Hot Rod Blazer", ItemType.Vehicles) { ModelItemID = "blazer3" },
            new ModItem("Nagasaki Street Blazer", true, ItemType.Vehicles) { ModelItemID = "blazer4" },
            new ModItem("Nagasaki Carbon RS", ItemType.Vehicles) { ModelItemID = "carbonrs" },
            new ModItem("Nagasaki Chimera", true, ItemType.Vehicles) { ModelItemID = "chimera" },
            new ModItem("Nagasaki Outlaw", true, ItemType.Vehicles) { ModelItemID = "outlaw" },
            new ModItem("Nagasaki Shotaro", true, ItemType.Vehicles) { ModelItemID = "shotaro" },
            new ModItem("Nagasaki Stryder", true, ItemType.Vehicles) { ModelItemID = "Stryder" },
            new ModItem("Obey 8F Drafter", true, ItemType.Vehicles) { ModelItemID = "drafter" },
            new ModItem("Obey 9F", ItemType.Vehicles) { ModelItemID = "ninef" },
            new ModItem("Obey 9F Cabrio", ItemType.Vehicles) { ModelItemID = "ninef2" },
            new ModItem("Obey Omnis", true, ItemType.Vehicles) { ModelItemID = "omnis" },
            new ModItem("Obey Rocoto", ItemType.Vehicles) { ModelItemID = "rocoto" },
            new ModItem("Obey Tailgater", ItemType.Vehicles) { ModelItemID = "tailgater" },
            new ModItem("Obey Tailgater S", true, ItemType.Vehicles) { ModelItemID = "tailgater2" },
            new ModItem("Ocelot Ardent", true, ItemType.Vehicles) { ModelItemID = "ardent" },
            new ModItem("Ocelot F620", ItemType.Vehicles) { ModelItemID = "f620" },
            new ModItem("Ocelot R88", true, ItemType.Vehicles) { ModelItemID = "formula2" },
            new ModItem("Ocelot Jackal", ItemType.Vehicles) { ModelItemID = "jackal" },
            new ModItem("Ocelot Jugular", true, ItemType.Vehicles) { ModelItemID = "jugular" },
            new ModItem("Ocelot Locust", true, ItemType.Vehicles) { ModelItemID = "locust" },
            new ModItem("Ocelot Lynx", true, ItemType.Vehicles) { ModelItemID = "lynx" },
            new ModItem("Ocelot Pariah", true, ItemType.Vehicles) { ModelItemID = "pariah" },
            new ModItem("Ocelot Penetrator", true, ItemType.Vehicles) { ModelItemID = "penetrator" },
            new ModItem("Ocelot Swinger", true, ItemType.Vehicles) { ModelItemID = "swinger" },
            new ModItem("Ocelot XA-21", true, ItemType.Vehicles) { ModelItemID = "xa21" },
            new ModItem("Overflod Autarch", true, ItemType.Vehicles) { ModelItemID = "autarch" },
            new ModItem("Overflod Entity XXR", true, ItemType.Vehicles) { ModelItemID = "entity2" },
            new ModItem("Overflod Entity XF", ItemType.Vehicles) { ModelItemID = "entityxf" },
            new ModItem("Overflod Imorgon", true, ItemType.Vehicles) { ModelItemID = "imorgon" },
            new ModItem("Overflod Tyrant", true, ItemType.Vehicles) { ModelItemID = "tyrant" },
            new ModItem("Pegassi Bati 801", ItemType.Vehicles) { ModelItemID = "bati" },
            new ModItem("Pegassi Bati 801RR", ItemType.Vehicles) { ModelItemID = "bati2" },
            new ModItem("Pegassi Esskey", true, ItemType.Vehicles) { ModelItemID = "esskey" },
            new ModItem("Pegassi Faggio Sport", true, ItemType.Vehicles) { ModelItemID = "faggio" },
            new ModItem("Pegassi Faggio", ItemType.Vehicles) { ModelItemID = "faggio2" },
            new ModItem("Pegassi Faggio Mod", true, ItemType.Vehicles) { ModelItemID = "faggio3" },
            new ModItem("Pegassi FCR 1000", true, ItemType.Vehicles) { ModelItemID = "fcr" },
            new ModItem("Pegassi FCR 1000 Custom", true, ItemType.Vehicles) { ModelItemID = "fcr2" },
            new ModItem("Pegassi Infernus", ItemType.Vehicles) { ModelItemID = "infernus" },
            new ModItem("Pegassi Infernus Classic", true, ItemType.Vehicles) { ModelItemID = "infernus2" },
            new ModItem("Pegassi Monroe", ItemType.Vehicles) { ModelItemID = "monroe" },
            new ModItem("Pegassi Oppressor", true, ItemType.Vehicles) { ModelItemID = "oppressor" },
            new ModItem("Pegassi Oppressor Mk II", true, ItemType.Vehicles) { ModelItemID = "oppressor2" },
            new ModItem("Pegassi Osiris", true, ItemType.Vehicles) { ModelItemID = "osiris" },
            new ModItem("Pegassi Reaper", true, ItemType.Vehicles) { ModelItemID = "reaper" },
            new ModItem("Pegassi Ruffian", ItemType.Vehicles) { ModelItemID = "ruffian" },
            new ModItem("Pegassi Tempesta", true, ItemType.Vehicles) { ModelItemID = "tempesta" },
            new ModItem("Pegassi Tezeract", true, ItemType.Vehicles) { ModelItemID = "tezeract" },
            new ModItem("Pegassi Torero", true, ItemType.Vehicles) { ModelItemID = "torero" },
            new ModItem("Pegassi Toros", true, ItemType.Vehicles) { ModelItemID = "toros" },
            new ModItem("Pegassi Vacca", ItemType.Vehicles) { ModelItemID = "vacca" },
            new ModItem("Pegassi Vortex", true, ItemType.Vehicles) { ModelItemID = "vortex" },
            new ModItem("Pegassi Zentorno", true, ItemType.Vehicles) { ModelItemID = "zentorno" },
            new ModItem("Pegassi Zorrusso", true, ItemType.Vehicles) { ModelItemID = "zorrusso" },
            new ModItem("Pfister Comet", "You always wanted one of these when in high school - and now you can have the car that tells everyone yes, these are implants - on your head and in that dizzy tart next to you. Boom. You go, tiger.", ItemType.Vehicles) { ModelItemID = "comet2" },
            new ModItem("Pfister Comet Retro Custom", "For a whole generation of the San Andreas elite, this isn't just a car. From the onboard champagne cooler to the suede back seat where you pawed your first gold digger - The Pfister Comet was something that made you who you are. And now, thanks to Benny reinventing it as a gnarly, riveted urban dragster, it'll be broadcasting your escalating midlife crisis for years to come.", true, ItemType.Vehicles) { ModelItemID = "comet3" },
            new ModItem("Pfister Comet Safari", "Is there nothing the Pfister Comet cannot do? If you were a venture capitalist looking for the shortest route to your next midlife crisis, the Comet was your first and only choice. If you wanted something that preserved the classic reek of desperation but added a street-racer twist, the Retro Custom was top of the list. And now, if you're looking for something to slam around a hairpin bend in three feet of uphill mud, the Comet Safari has got you covered.", true, ItemType.Vehicles) { ModelItemID = "comet4" },
            new ModItem("Pfister Comet SR", "Forget everything you think you know about the Pfister Comet. Forget cruising through Vinewood with a bellyful of whiskey dropping one-liners about the size of your bonus. Forget picking up sex workers and passing them off as your fiancé at family gatherings. The SR was made for only one thing: to make every other sports car look like it's the asthmatic kid in gym. Now get in line.", true, ItemType.Vehicles) { ModelItemID = "comet5" },
            new ModItem("Pfister Comet S2", "This isn't just a fast car. It's a car with the kind of reputation that no amount of targeted advertising can buy. So, when some people see a Comet they make a wish. Others run screaming for cover, prophesying doom, destruction, and crippling medical expenses. Either way, you made an impression.", true, ItemType.Vehicles) { ModelItemID = "comet6" },
            new ModItem("Pfister Growler","You prefer the book to the movie. You drink spirits neat. You describe your sense of humor as 'subtle' and your love making as 'imperceptible'. You're The Thinking Person. And you choose handling over speed, control over power, and principle over pleasure. You choose wisely. You choose the Pfister Growler.", true, ItemType.Vehicles) { ModelItemID = "growler" },
            new ModItem("Pfister Neon","When the history of the electric car is written, it will begin with the Pfister Neon. Everything else - all the ridiculous eco-vans and hybrid fetishes - has been foreplay. Now Pfister have dropped their pants, and the battery-powered action can really begin.", true, ItemType.Vehicles) { ModelItemID = "neon" },
            new ModItem("Pfister 811","Meet the future of hybrid tech: Pfister took billions of dollars in subsidies for low-carbon research and used it to refine an electric motor until it gives more kick than a turbo charger. And don't worry about accidentally investing in the environment: the assembly process alone produces enough CO2 to offset two thousand acres of otherwise useless rainforest. Win-win.", true, ItemType.Vehicles) { ModelItemID = "pfister811" },
            new ModItem("Principe Deveste Eight","It began as little more than a myth: a list of impossible statistics circulating on the dark net. Then the myth became a legend: a few leaked photographs so provocative that possession was a federal crime. Then the legend became a rumor: a car so exclusive no one could confirm it existed in the real world. And now, thanks to you, that rumor is about to become a very messy headline.", true, ItemType.Vehicles) { ModelItemID = "deveste" },
            new ModItem("Principe Diabolus","You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", true, ItemType.Vehicles) { ModelItemID = "diablous" },
            new ModItem("Principe Diabolus Custom","You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", true, ItemType.Vehicles) { ModelItemID = "diablous2" },
            new ModItem("Principe Lectro","As if this new-school streetfighter didn’t look aggressive enough, once you hit that KERS button you’ll be locked into a death struggle with the laws of physics - and there can only be one winner.", true, ItemType.Vehicles) { ModelItemID = "lectro" },
            new ModItem("Principe Nemesis","Super fast, super unshielded. When you're riding a Nemesis, you don't just feel the wind in your hair, you feel it tearing into the back of your eye sockets.", ItemType.Vehicles) { ModelItemID = "nemesis" },
            new ModItem("Progen Emerus", true, ItemType.Vehicles) { ModelItemID = "emerus" },
            new ModItem("Progen PR4", true, ItemType.Vehicles) { ModelItemID = "formula" },
            new ModItem("Progen GP1", true, ItemType.Vehicles) { ModelItemID = "gp1" },
            new ModItem("Progen Itali GTB", true, ItemType.Vehicles) { ModelItemID = "italigtb" },
            new ModItem("Progen Itali GTB Custom", true, ItemType.Vehicles) { ModelItemID = "italigtb2" },
            new ModItem("Progen T20", true, ItemType.Vehicles) { ModelItemID = "t20" },
            new ModItem("Progen Tyrus", true, ItemType.Vehicles) { ModelItemID = "tyrus" },
            new ModItem("RUNE Cheburek", true, ItemType.Vehicles) { ModelItemID = "cheburek" },
            new ModItem("Schyster Deviant", true, ItemType.Vehicles) { ModelItemID = "deviant" },
            new ModItem("Schyster Fusilade", ItemType.Vehicles) { ModelItemID = "fusilade" },
            new ModItem("Shitzu Defiler", true, ItemType.Vehicles) { ModelItemID = "defiler" },
            new ModItem("Shitzu Hakuchou", true, ItemType.Vehicles) { ModelItemID = "hakuchou" },
            new ModItem("Shitzu Hakuchou Drag", true, ItemType.Vehicles) { ModelItemID = "hakuchou2" },
            new ModItem("Shitzu PCJ 600", ItemType.Vehicles) { ModelItemID = "pcj" },
            new ModItem("Shitzu Vader", ItemType.Vehicles) { ModelItemID = "Vader" },
            new ModItem("Stanley Fieldmaster", ItemType.Vehicles) { ModelItemID = "tractor2" },
            new ModItem("Stanley Fieldmaster", ItemType.Vehicles) { ModelItemID = "tractor3" },
            new ModItem("Truffade Adder", ItemType.Vehicles) { ModelItemID = "adder" },
            new ModItem("Truffade Nero", true, ItemType.Vehicles) { ModelItemID = "nero" },
            new ModItem("Truffade Nero Custom", true, ItemType.Vehicles) { ModelItemID = "nero2" },
            new ModItem("Truffade Thrax", true, ItemType.Vehicles) { ModelItemID = "thrax" },
            new ModItem("Truffade Z-Type", ItemType.Vehicles) { ModelItemID = "Ztype" },
            new ModItem("Ubermacht Oracle XS", ItemType.Vehicles) { ModelItemID = "oracle" },
            new ModItem("Ubermacht Oracle", ItemType.Vehicles) { ModelItemID = "oracle2" },
            new ModItem("Ubermacht Revolter", true, ItemType.Vehicles) { ModelItemID = "revolter" },
            new ModItem("Ubermacht SC1", true, ItemType.Vehicles) { ModelItemID = "sc1" },
            new ModItem("Ubermacht Sentinel XS", ItemType.Vehicles) { ModelItemID = "sentinel" },
            new ModItem("Ubermacht Sentinel 2", ItemType.Vehicles) { ModelItemID = "sentinel2" },
            new ModItem("Ubermacht Sentinel 3", true, ItemType.Vehicles) { ModelItemID = "sentinel3" },
            new ModItem("Ubermacht Zion", ItemType.Vehicles) { ModelItemID = "zion" },
            new ModItem("Ubermacht Zion Cabrio", ItemType.Vehicles) { ModelItemID = "zion2" },
            new ModItem("Ubermacht Zion Classic", true, ItemType.Vehicles) { ModelItemID = "zion3" },
            new ModItem("Ubermacht Cypher", true, ItemType.Vehicles) { ModelItemID = "cypher" },
            new ModItem("Ubermacht Rebla GTS", true, ItemType.Vehicles) { ModelItemID = "rebla" },
            new ModItem("Vapid Benson", ItemType.Vehicles) { ModelItemID = "Benson" },
            new ModItem("Vapid Blade", true, ItemType.Vehicles) { ModelItemID = "blade" },
            new ModItem("Vapid Bobcat XL", ItemType.Vehicles) { ModelItemID = "bobcatXL" },
            new ModItem("Vapid Bullet", ItemType.Vehicles) { ModelItemID = "bullet" },
            new ModItem("Vapid Caracara", true, ItemType.Vehicles) { ModelItemID = "caracara" },
            new ModItem("Vapid Caracara 4x4", true, ItemType.Vehicles) { ModelItemID = "caracara2" },
            new ModItem("Vapid Chino", true, ItemType.Vehicles) { ModelItemID = "chino" },
            new ModItem("Vapid Chino Custom", true, ItemType.Vehicles) { ModelItemID = "chino2" },
            new ModItem("Vapid Clique", true, ItemType.Vehicles) { ModelItemID = "clique" },
            new ModItem("Vapid Contender", true, ItemType.Vehicles) { ModelItemID = "contender" },
            new ModItem("Vapid Dominator", "Baby boomer teen dream repackaged for the mass market generation. A muscle car without the muscle that's the rental vehicle of choice for tourists looking for a slice of 'real' Americana.", ItemType.Vehicles) { ModelItemID = "Dominator" },
            new ModItem("Vapid Pisswasser Dominator", ItemType.Vehicles) { ModelItemID = "dominator2" },
            new ModItem("Vapid Dominator GTX*", "Step one: take the best-looking muscle car the 60's ever saw, and introduce it to the greatest American supercar of the modern era. When your pedigree is this damn good, there's nothing wrong with keeping it in the family.", ItemType.Vehicles) { ModelItemID = "dominator3" },
            new ModItem("Vapid Apocalypse Dominator", true, ItemType.Vehicles) { ModelItemID = "dominator4" },
            new ModItem("Vapid Future Shock Dominator", true, ItemType.Vehicles) { ModelItemID = "dominator5" },
            new ModItem("Vapid Nightmare Dominator", true, ItemType.Vehicles) { ModelItemID = "dominator6" },
            new ModItem("Vapid Dominator ASP", true, ItemType.Vehicles) { ModelItemID = "dominator7" },
            new ModItem("Vapid Dominator GTT", true, ItemType.Vehicles) { ModelItemID = "dominator8" },
            new ModItem("Vapid Ellie", true, ItemType.Vehicles) { ModelItemID = "ellie" },
            new ModItem("Vapid Flash GT", true, ItemType.Vehicles) { ModelItemID = "flashgt" },
            new ModItem("Vapid FMJ", true, ItemType.Vehicles) { ModelItemID = "fmj" },
            new ModItem("Vapid GB200", true, ItemType.Vehicles) { ModelItemID = "gb200" },
            new ModItem("Vapid Guardian", true, ItemType.Vehicles) { ModelItemID = "guardian" },
            new ModItem("Vapid Hotknife", ItemType.Vehicles) { ModelItemID = "hotknife" },
            new ModItem("Vapid Hustler", true, ItemType.Vehicles) { ModelItemID = "hustler" },
            new ModItem("Vapid Apocalypse Imperator", true, ItemType.Vehicles) { ModelItemID = "imperator" },
            new ModItem("Vapid Future Shock Imperator", true, ItemType.Vehicles) { ModelItemID = "imperator2" },
            new ModItem("Vapid Nightmare Imperator", true, ItemType.Vehicles) { ModelItemID = "imperator3" },
            new ModItem("Vapid Minivan", ItemType.Vehicles) { ModelItemID = "minivan" },
            new ModItem("Vapid Minivan Custom", true, ItemType.Vehicles) { ModelItemID = "minivan2" },
            new ModItem("Vapid Monster", true, ItemType.Vehicles) { ModelItemID = "monster" },
            new ModItem("Vapid Peyote", ItemType.Vehicles) { ModelItemID = "peyote" },
            new ModItem("Vapid Peyote Gasser", true, ItemType.Vehicles) { ModelItemID = "peyote2" },
            new ModItem("Vapid Peyote Custom", true, ItemType.Vehicles) { ModelItemID = "peyote3" },
            new ModItem("Vapid Radius", ItemType.Vehicles) { ModelItemID = "radi" },
            new ModItem("Vapid Retinue", true, ItemType.Vehicles) { ModelItemID = "retinue" },
            new ModItem("Vapid Retinue Mk II", true, ItemType.Vehicles) { ModelItemID = "retinue2" },
            new ModItem("Vapid Riata", true, ItemType.Vehicles) { ModelItemID = "riata" },
            new ModItem("Vapid Sadler", ItemType.Vehicles) { ModelItemID = "Sadler" },
            new ModItem("Vapid Sadler 2", ItemType.Vehicles) { ModelItemID = "sadler2" },
            new ModItem("Vapid Sandking XL", ItemType.Vehicles) { ModelItemID = "sandking" },
            new ModItem("Vapid Sandking SWB", ItemType.Vehicles) { ModelItemID = "sandking2" },
            new ModItem("Vapid Slamtruck", true, ItemType.Vehicles) { ModelItemID = "slamtruck" },
            new ModItem("Vapid Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan" },
            new ModItem("Vapid Lost Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan2" },
            new ModItem("Vapid Slamvan Custom", true, ItemType.Vehicles) { ModelItemID = "slamvan3" },
            new ModItem("Vapid Apocalypse Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan4" },
            new ModItem("Vapid Future Shock Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan5" },
            new ModItem("Vapid Nightmare Slamvan", true, ItemType.Vehicles) { ModelItemID = "slamvan6" },
            new ModItem("Vapid Speedo", ItemType.Vehicles) { ModelItemID = "speedo" },
            new ModItem("Vapid Clown Van", ItemType.Vehicles) { ModelItemID = "speedo2" },
            new ModItem("Vapid Speedo Custom", true, ItemType.Vehicles) { ModelItemID = "speedo4" },
            new ModItem("Vapid Stanier", "If you took a cab or got arrested in the 1990s, there's a high chance you ended up in the back of a Vapid Stanier. Discontinued following widespread reports of fuel tanks exploding on impact in rear-end collisions. So try to avoid that.", ItemType.Vehicles) { ModelItemID = "stanier" },
            new ModItem("Vapid Trophy Truck", true, ItemType.Vehicles) { ModelItemID = "trophytruck" },
            new ModItem("Vapid Desert Raid", true, ItemType.Vehicles) { ModelItemID = "trophytruck2" },
            new ModItem("Vapid Winky", true, ItemType.Vehicles) { ModelItemID = "winky" },
            new ModItem("Vulcar Fagaloa", true, ItemType.Vehicles) { ModelItemID = "fagaloa" },
            new ModItem("Vulcar Ingot", ItemType.Vehicles) { ModelItemID = "ingot" },
            new ModItem("Vulcar Nebula Turbo", true, ItemType.Vehicles) { ModelItemID = "nebula" },
            new ModItem("Vulcar Warrener", true, ItemType.Vehicles) { ModelItemID = "warrener" },
            new ModItem("Vulcar Warrener HKR", true, ItemType.Vehicles) { ModelItemID = "warrener2" },
            new ModItem("Vysser Neo", true, ItemType.Vehicles) { ModelItemID = "neo" },
            new ModItem("Weeny Dynasty", true, ItemType.Vehicles) { ModelItemID = "Dynasty" },
            new ModItem("Weeny Issi", ItemType.Vehicles) { ModelItemID = "issi2" },
            new ModItem("Weeny Issi Classic", true, ItemType.Vehicles) { ModelItemID = "issi3" },
            new ModItem("Weeny Apocalypse Issi", true, ItemType.Vehicles) { ModelItemID = "issi4" },
            new ModItem("Weeny Future Shock Issi", true, ItemType.Vehicles) { ModelItemID = "issi5" },
            new ModItem("Weeny Nightmare Issi", true, ItemType.Vehicles) { ModelItemID = "issi6" },
            new ModItem("Weeny Issi Sport", true, ItemType.Vehicles) { ModelItemID = "issi7" },
            new ModItem("Western Bagger", ItemType.Vehicles) { ModelItemID = "bagger" },
            new ModItem("Western Cliffhanger", true, ItemType.Vehicles) { ModelItemID = "cliffhanger" },
            new ModItem("Western Daemon LOST", ItemType.Vehicles) { ModelItemID = "daemon" },
            new ModItem("Western Daemon", true, ItemType.Vehicles) { ModelItemID = "daemon2" },
            new ModItem("Western Apocalypse Deathbike", true, ItemType.Vehicles) { ModelItemID = "deathbike" },
            new ModItem("Western Future Shock Deathbike", true, ItemType.Vehicles) { ModelItemID = "deathbike2" },
            new ModItem("Western Nightmare Deathbike", true, ItemType.Vehicles) { ModelItemID = "deathbike3" },
            new ModItem("Western Gargoyle", true, ItemType.Vehicles) { ModelItemID = "gargoyle" },
            new ModItem("Western Nightblade", true, ItemType.Vehicles) { ModelItemID = "nightblade" },
            new ModItem("Western Rat Bike", true, ItemType.Vehicles) { ModelItemID = "ratbike" },
            new ModItem("Western Rampant Rocket", true, ItemType.Vehicles) { ModelItemID = "rrocket" },
            new ModItem("Western Sovereign", true, ItemType.Vehicles) { ModelItemID = "sovereign" },
            new ModItem("Western Wolfsbane", true, ItemType.Vehicles) { ModelItemID = "wolfsbane" },
            new ModItem("Western Zombie Bobber", true, ItemType.Vehicles) { ModelItemID = "zombiea" },
            new ModItem("Western Zombie Chopper", true, ItemType.Vehicles) { ModelItemID = "zombieb" },
            new ModItem("Willard Faction", true, ItemType.Vehicles) { ModelItemID = "faction" },
            new ModItem("Willard Faction Custom", true, ItemType.Vehicles) { ModelItemID = "faction2" },
            new ModItem("Willard Faction Custom Donk", true, ItemType.Vehicles) { ModelItemID = "faction3" },
            new ModItem("Zirconium Journey", ItemType.Vehicles) { ModelItemID = "journey" },
            new ModItem("Zirconium Stratum", ItemType.Vehicles) { ModelItemID = "stratum" },

            //Heli
            new ModItem("Buckingham SuperVolito", true, ItemType.Vehicles) { ModelItemID = "supervolito" },
            new ModItem("Buckingham SuperVolito Carbon", true, ItemType.Vehicles) { ModelItemID = "supervolito2" },
            new ModItem("Buckingham Swift", true, ItemType.Vehicles) { ModelItemID = "swift" },
            new ModItem("Buckingham Swift Deluxe", true, ItemType.Vehicles) { ModelItemID = "swift2" },
            new ModItem("Buckingham Volatus", true, ItemType.Vehicles) { ModelItemID = "volatus" },
            new ModItem("Mammoth Thruster", true, ItemType.Vehicles) { ModelItemID = "thruster" },
            new ModItem("Nagasaki Havok", true, ItemType.Vehicles) { ModelItemID = "havok" },

            //Plane
            new ModItem("Buckingham Alpha-Z1", true, ItemType.Vehicles) { ModelItemID = "alphaz1" },
            new ModItem("Buckingham Howard NX-25", true, ItemType.Vehicles) { ModelItemID = "howard" },
            new ModItem("Buckingham Luxor", ItemType.Vehicles) { ModelItemID = "luxor" },
            new ModItem("Buckingham Luxor Deluxe", true, ItemType.Vehicles) { ModelItemID = "luxor2" },
            new ModItem("Buckingham Miljet", true, ItemType.Vehicles) { ModelItemID = "Miljet" },
            new ModItem("Buckingham Nimbus", true, ItemType.Vehicles) { ModelItemID = "nimbus" },
            new ModItem("Buckingham Pyro", true, ItemType.Vehicles) { ModelItemID = "pyro" },
            new ModItem("Buckingham Shamal", ItemType.Vehicles) { ModelItemID = "Shamal" },
            new ModItem("Buckingham Vestra", true, ItemType.Vehicles) { ModelItemID = "vestra" },
            new ModItem("Mammoth Avenger", true, ItemType.Vehicles) { ModelItemID = "avenger" },
            new ModItem("Mammoth Avenger 2", true, ItemType.Vehicles) { ModelItemID = "avenger2" },
            new ModItem("Mammoth Dodo", ItemType.Vehicles) { ModelItemID = "dodo" },
            new ModItem("Mammoth Hydra", true, ItemType.Vehicles) { ModelItemID = "hydra" },
            new ModItem("Mammoth Mogul", true, ItemType.Vehicles) { ModelItemID = "mogul" },
            new ModItem("Mammoth Tula", true, ItemType.Vehicles) { ModelItemID = "tula" },
            new ModItem("Nagasaki Ultralight", true, ItemType.Vehicles) { ModelItemID = "microlight" },
            new ModItem("Western Besra", true, ItemType.Vehicles) { ModelItemID = "besra" },
            new ModItem("Western Rogue", true, ItemType.Vehicles) { ModelItemID = "rogue" },
            new ModItem("Western Seabreeze", true, ItemType.Vehicles) { ModelItemID = "seabreeze" },

            //Boat
            new ModItem("Dinka Marquis", ItemType.Vehicles) { ModelItemID = "marquis" },
            new ModItem("Lampadati Toro", true, ItemType.Vehicles) { ModelItemID = "toro" },
            new ModItem("Lampadati Toro", true, ItemType.Vehicles) { ModelItemID = "toro2" },
            new ModItem("Nagasaki Dinghy", ItemType.Vehicles) { ModelItemID = "Dinghy" },
            new ModItem("Nagasaki Dinghy 2", ItemType.Vehicles) { ModelItemID = "dinghy2" },
            new ModItem("Nagasaki Dinghy 3", true, ItemType.Vehicles) { ModelItemID = "dinghy3" },
            new ModItem("Nagasaki Dinghy 4", true, ItemType.Vehicles) { ModelItemID = "dinghy4" },
            new ModItem("Nagasaki Weaponized Dinghy", true, ItemType.Vehicles) { ModelItemID = "dinghy5" },
            new ModItem("Pegassi Speeder", true, ItemType.Vehicles) { ModelItemID = "speeder" },
            new ModItem("Pegassi Speeder", true, ItemType.Vehicles) { ModelItemID = "speeder2" },
            new ModItem("Shitzu Jetmax", ItemType.Vehicles) { ModelItemID = "jetmax" },
            new ModItem("Shitzu Longfin", true, ItemType.Vehicles) { ModelItemID = "longfin" },
            new ModItem("Shitzu Squalo", ItemType.Vehicles) { ModelItemID = "squalo" },
            new ModItem("Shitzu Suntrap", ItemType.Vehicles) { ModelItemID = "Suntrap" },
            new ModItem("Shitzu Tropic", ItemType.Vehicles) { ModelItemID = "tropic" },
            new ModItem("Shitzu Tropic", true, ItemType.Vehicles) { ModelItemID = "tropic2" },
            new ModItem("Speedophile Seashark", ItemType.Vehicles) { ModelItemID = "seashark" },
            new ModItem("Speedophile Seashark 2", ItemType.Vehicles) { ModelItemID = "seashark2" },
            new ModItem("Speedophile Seashark 3", true, ItemType.Vehicles) { ModelItemID = "seashark3" },
        });
    }
    private void DefaultConfig_Weapons()
    {
        ModItemsList.AddRange(new List<ModItem>
        {
            //Melee
            new ModItem("Baseball Bat","Aluminum baseball bat with leather grip. Lightweight yet powerful for all you big hitters out there.", false, ItemType.Weapons) { ModelItemID = "weapon_bat"},
            new ModItem("Crowbar","Heavy-duty crowbar forged from high quality, tempered steel for that extra leverage you need to get the job done.", false, ItemType.Weapons) { ModelItemID = "weapon_crowbar"},
            new ModItem("Golf Club","Standard length, mid iron golf club with rubber grip for a lethal short game.", false, ItemType.Weapons) { ModelItemID = "weapon_golfclub"},
            new ModItem("Hammer","A robust, multi-purpose hammer with wooden handle and curved claw, this old classic still nails the competition.", false, ItemType.Weapons) { ModelItemID = "weapon_hammer"},
            new ModItem("Hatchet","Add a good old-fashioned hatchet to your armory, and always have a back up for when ammo is hard to come by.", false, ItemType.Weapons) { ModelItemID = "weapon_hatchet"},
            new ModItem("Brass Knuckles","Perfect for knocking out gold teeth, or as a gift to the trophy partner who has everything.", false, ItemType.Weapons) { ModelItemID = "weapon_knuckle"},
            new ModItem("Combat Knife","This carbon steel 7 inch bladed knife is dual edged with a serrated spine to provide improved stabbing and thrusting capabilities.", false, ItemType.Weapons) { ModelItemID = "weapon_knife"},
            new ModItem("Machete","America's West African arms trade isn't just about giving. Rediscover the simple life with this rusty cleaver.", false, ItemType.Weapons) { ModelItemID = "weapon_machete"},
            new ModItem("Switchblade","From your pocket to hilt-deep in the other guy's ribs in under a second: folding knives will never go out of style.", false, ItemType.Weapons) { ModelItemID = "weapon_switchblade" },
            new ModItem("Nightstick","24 inch polycarbonate side-handled nightstick.", false, ItemType.Weapons) { ModelItemID = "weapon_nightstick"},
            new ModItem("Wrench","Perennial favourite of apocalyptic survivalists and violent fathers the world over, apparently it also doubles as some kind of tool.", false, ItemType.Weapons) { ModelItemID = "weapon_wrench"},
            new ModItem("Pool Cue","Ah, there's no sound as satisfying as the crack of a perfect break, especially when it's the other guy's spine.", false, ItemType.Weapons) { ModelItemID = "weapon_poolcue"},

            //Pistola
            new ModItem("Hawk & Little PTF092F","Standard handgun. A 9mm combat pistol with a magazine capacity of 12 rounds that can be extended to 16.", false, ItemType.Weapons) { ModelItemID = "weapon_pistol"},
            new ModItem("Hawk & Little Thunder","Balance, simplicity, precision: nothing keeps the peace like an extended barrel in the other guy's mouth.", true, ItemType.Weapons) { ModelItemID = "weapon_pistol_mk2"},
            new ModItem("Hawk & Little Combat Pistol","A compact, lightweight semi-automatic pistol designed for law enforcement and personal defense use. 12-round magazine with option to extend to 16 rounds.", false, ItemType.Weapons) { ModelItemID = "weapon_combatpistol"},
            new ModItem("Hawk & Little Desert Slug","High-impact pistol that delivers immense power but with extremely strong recoil. Holds 9 rounds in magazine.", false, ItemType.Weapons) { ModelItemID = "weapon_pistol50"},
            new ModItem("Vom Feuer P69","Not your grandma's ceramics. Although this pint-sized pistol is small enough to fit into her purse and won't set off a metal detector.", true, ItemType.Weapons) { ModelItemID = "weapon_ceramicpistol"},
            new ModItem("Vom Feuer SCRAMP","High-penetration, fully-automatic pistol. Holds 18 rounds in magazine with option to extend to 36 rounds.", false, ItemType.Weapons) { ModelItemID = "weapon_appistol"},
            new ModItem("Hawk & Little 1919","The heavyweight champion of the magazine fed, semi-automatic handgun world. Delivers accuracy and a serious forearm workout every time.", false, ItemType.Weapons) { ModelItemID = "weapon_heavypistol"},
            new ModItem("Hawk & Little Raging Mare","A handgun with enough stopping power to drop a crazed rhino, and heavy enough to beat it to death if you're out of ammo.", true, ItemType.Weapons) { ModelItemID = "weapon_revolver"},
            new ModItem("Hawk & Little Raging Mare Dx","If you can lift it, this is the closest you'll get to shooting someone with a freight train.", true, ItemType.Weapons) { ModelItemID = "weapon_revolver_mk2"},
            new ModItem("Shrewsbury S7","Like condoms or hairspray, this fits in your pocket for a night on the town. The price of a bottle at a club, it's half as accurate as a champagne cork, and twice as deadly.", false, ItemType.Weapons) { ModelItemID = "weapon_snspistol"},
            new ModItem("Shrewsbury S7A","The ultimate purse-filler: if you want to make Saturday Night really special, this is your ticket.", true, ItemType.Weapons) { ModelItemID = "weapon_snspistol_mk2"},
            new ModItem("Coil Tesla","Fires a projectile that administers a voltage capable of temporarily stunning an assailant. It's like, literally stunning.", false, ItemType.Weapons) { ModelItemID = "weapon_stungun"},
            new ModItem("BS M1922","What you really need is a more recognisable gun. Stand out from the crowd at an armed robbery with this engraved pistol.", true, ItemType.Weapons) { ModelItemID = "weapon_vintagepistol"},

            //Shotgun
            new ModItem("Shrewsbury 420 Sawed-Off","This single-barrel, sawed-off shotgun compensates for its low range and ammo capacity with devastating efficiency in close combat.", false, ItemType.Weapons) { ModelItemID = "weapon_sawnoffshotgun"},
            new ModItem("Shrewsbury 420","Standard shotgun ideal for short-range combat. A high-projectile spread makes up for its lower accuracy at long range.", false, ItemType.Weapons) { ModelItemID = "weapon_pumpshotgun"},
            new ModItem("Vom Feuer 569","Only one thing pumps more action than a pump action: watch out, the recoil is almost as deadly as the shot.", true, ItemType.Weapons) { ModelItemID = "weapon_pumpshotgun_mk2"},
            new ModItem("Vom Feuer IBS-12","Fully automatic shotgun with 8 round magazine and high rate of fire.", false, ItemType.Weapons) { ModelItemID = "weapon_assaultshotgun"},
            new ModItem("Hawk & Little HLSG","More than makes up for its slow, pump-action rate of fire with its range and spread. Decimates anything in its projectile path.", false, ItemType.Weapons) { ModelItemID = "weapon_bullpupshotgun"},
            new ModItem("Shrewsbury Taiga-12","The weapon to reach for when you absolutely need to make a horrible mess of the room. Best used near easy-wipe surfaces only.", true, ItemType.Weapons) { ModelItemID = "weapon_heavyshotgun"},
            new ModItem("Toto 12 Guage Sawed-Off","Do one thing, do it well. Who needs a high rate of fire when your first shot turns the other guy into a fine mist?.", true, ItemType.Weapons) { ModelItemID = "weapon_dbshotgun"},
            new ModItem("Shrewsbury Defender","How many effective tools for riot control can you tuck into your pants? Ok, two. But this is the other one.", true, ItemType.Weapons) { ModelItemID = "weapon_autoshotgun"},
            new ModItem("Leotardo SPAZ-11","There's only one semi-automatic shotgun with a fire rate that sets the LSFD alarm bells ringing, and you're looking at it.", true, ItemType.Weapons) { ModelItemID = "weapon_combatshotgun"},

            //SMG
            new ModItem("Shrewsbury Luzi","Combines compact design with a high rate of fire at approximately 700-900 rounds per minute.", false, ItemType.Weapons) { ModelItemID = "weapon_microsmg"},
            new ModItem("Hawk & Little MP6","This is known as a good all-around submachine gun. Lightweight with an accurate sight and 30-round magazine capacity.", false, ItemType.Weapons) { ModelItemID = "weapon_smg"},
            new ModItem("Hawk & Little XPM","Lightweight, compact, with a rate of fire to die very messily for: turn any confined space into a kill box at the click of a well-oiled trigger.", true, ItemType.Weapons) { ModelItemID = "weapon_smg_mk2"},
            new ModItem("Vom Feuer Fisher","A high-capacity submachine gun that is both compact and lightweight. Holds up to 30 bullets in one magazine.", false, ItemType.Weapons) { ModelItemID = "weapon_assaultsmg"},
            new ModItem("Coil PXM","Who said personal weaponry couldn't be worthy of military personnel? Thanks to our lobbyists, not Congress. Integral suppressor.", false, ItemType.Weapons) { ModelItemID = "weapon_combatpdw"},
            new ModItem("Vom Feuer KEK-9","This fully automatic is the snare drum to your twin-engine V8 bass: no drive-by sounds quite right without it.", false, ItemType.Weapons) { ModelItemID = "weapon_machinepistol"},
            new ModItem("Hawk & Little Millipede","Increasingly popular since the marketing team looked beyond spec ops units and started caring about the little guys in low income areas.", false, ItemType.Weapons) { ModelItemID = "weapon_minismg"},

            //AR
            new ModItem("Shrewsbury A7-4K","This standard assault rifle boasts a large capacity magazine and long distance accuracy.", false, ItemType.Weapons) { ModelItemID = "weapon_assaultrifle"},
            new ModItem("Shrewsbury A2-1K","The definitive revision of an all-time classic: all it takes is a little work, and looks can kill after all.", true, ItemType.Weapons) { ModelItemID = "weapon_assaultrifle_mk2"},
            new ModItem("Vom Feuer A5-1R","Combining long distance accuracy with a high capacity magazine, the Carbine Rifle can be relied on to make the hit.", false, ItemType.Weapons) { ModelItemID = "weapon_carbinerifle"},
            new ModItem("Vom Feuer A5-1R MK2","This is bespoke, artisan firepower: you couldn't deliver a hail of bullets with more love and care if you inserted them by hand.", true, ItemType.Weapons) { ModelItemID = "weapon_carbinerifle_mk2" },
            new ModItem("Vom Feuer BFR","The most lightweight and compact of all assault rifles, without compromising accuracy and rate of fire.", false, ItemType.Weapons) { ModelItemID = "weapon_advancedrifle"},
            new ModItem("Vom Feuer SL6","Combining accuracy, maneuverability, firepower and low recoil, this is an extremely versatile assault rifle for any combat situation.", false, ItemType.Weapons) { ModelItemID = "weapon_specialcarbine"},
            new ModItem("Vom Feuer SL6 MK2","The jack of all trades just got a serious upgrade: bow to the master.", true, ItemType.Weapons) { ModelItemID = "weapon_specialcarbine_mk2"},
            new ModItem("Hawk & Little ZBZ-23","The latest Chinese import taking America by storm, this rifle is known for its balanced handling. Lightweight and very controllable in automatic fire.", false, ItemType.Weapons) { ModelItemID = "weapon_bullpuprifle"},
            new ModItem("Hawk & Little ZBZ-25X","So precise, so exquisite, it's not so much a hail of bullets as a symphony.", true, ItemType.Weapons) { ModelItemID = "weapon_bullpuprifle_mk2"},
            new ModItem("Shrewsbury Stinkov","Half the size, all the power, double the recoil: there's no riskier way to say 'I'm compensating for something'.", false, ItemType.Weapons) { ModelItemID = "weapon_compactrifle"},
            new ModItem("Vom Feuer GUH-B4","This immensely powerful assault rifle was designed for highly qualified, exceptionally skilled soldiers. Yes, you can buy it.", false, ItemType.Weapons) { ModelItemID = "weapon_militaryrifle"},
            new ModItem("Vom Feuer POCK","The no-holds barred 30-round answer to that eternal question: how do I get this guy off my back?", true, ItemType.Weapons) { ModelItemID = "weapon_heavyrifle"},

            //LMG
            new ModItem("Shrewsbury PDA","General purpose machine gun that combines rugged design with dependable performance. Long range penetrative power. Very effective against large groups.", false, ItemType.Weapons) { ModelItemID = "weapon_mg"},
            new ModItem("Vom Feuer BAT","Lightweight, compact machine gun that combines excellent maneuverability with a high rate of fire to devastating effect.", false, ItemType.Weapons) { ModelItemID = "weapon_combatmg"},
            new ModItem("Vom Feuer M70E1","You can never have too much of a good thing: after all, if the first shot counts, then the next hundred or so must count for double.", true, ItemType.Weapons) { ModelItemID = "weapon_combatmg_mk2"},
            new ModItem("Hawk & Little Kenan","Complete your look with a Prohibition gun. Looks great being fired from an Albany Roosevelt or paired with a pinstripe suit.", false, ItemType.Weapons) { ModelItemID = "weapon_gusenberg"},

            //SNIPER
            new ModItem("Shrewsbury PWN","Standard sniper rifle. Ideal for situations that require accuracy at long range. Limitations include slow reload speed and very low rate of fire.", false, ItemType.Weapons) { ModelItemID = "weapon_sniperrifle"},
            new ModItem("Bartlett M92","Features armor-piercing rounds for heavy damage. Comes with laser scope as standard.", false, ItemType.Weapons) { ModelItemID = "weapon_heavysniper"},
            new ModItem("Bartlett M92 Mk2","Far away, yet always intimate: if you're looking for a secure foundation for that long-distance relationship, this is it.", true, ItemType.Weapons) { ModelItemID = "weapon_heavysniper_mk2"},
            new ModItem("Vom Feuer M23 DBS","Whether you're up close or a disconcertingly long way away, this weapon will get the job done. A multi-range tool for tools.", false, ItemType.Weapons) { ModelItemID = "weapon_marksmanrifle"},
            new ModItem("Vom Feuer M23 DBS Scout","Known in military circles as The Dislocator, this mod set will destroy both the target and your shoulder, in that order.", true, ItemType.Weapons) { ModelItemID = "weapon_marksmanrifle_mk2"},



            //new ModItem("Shrewsbury BFD Dragmeout","Want to give the impression of accuracy while still having greater than 1 MOA? Dragmeout.", true, ItemType.Weapons) { ModelItemID = "weapon_russiansniper"},


            //OTHER
            new ModItem("RPG-7","A portable, shoulder-launched, anti-tank weapon that fires explosive warheads. Very effective for taking down vehicles or large groups of assailants.", false, ItemType.Weapons) { ModelItemID = "weapon_rpg"},
            new ModItem("Hawk & Little MGL","A compact, lightweight grenade launcher with semi-automatic functionality. Holds up to 10 rounds.", false, ItemType.Weapons) { ModelItemID = "weapon_grenadelauncher"},
            new ModItem("M61 Grenade","Standard fragmentation grenade. Pull pin, throw, then find cover. Ideal for eliminating clustered assailants.", false, ItemType.Weapons) { ModelItemID = "weapon_grenade"},
            new ModItem("Improvised Incendiary","Crude yet highly effective incendiary weapon. No happy hour with this cocktail.", false, ItemType.Weapons) { ModelItemID = "weapon_molotov"},
            new ModItem("BZ Gas Grenade","BZ gas grenade, particularly effective at incapacitating multiple assailants.", false, ItemType.Weapons) { ModelItemID = "weapon_bzgas"},
            new ModItem("Tear Gas Grenade","Tear gas grenade, particularly effective at incapacitating multiple assailants. Sustained exposure can be lethal.", false, ItemType.Weapons) { ModelItemID = "weapon_smokegrenade"},
        });
    }
}