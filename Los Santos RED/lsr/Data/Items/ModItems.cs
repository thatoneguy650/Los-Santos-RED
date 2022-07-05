using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
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
            EntryPoint.WriteToConsole($"Deserializing 1 {ConfigFile.FullName}");
            ModItemsList = Serialization.DeserializeParams<ModItem>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Deserializing 2 {ConfigFileName}");
            ModItemsList = Serialization.DeserializeParams<ModItem>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
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
                ModelItem = new PhysicalItem("ba_prop_club_water_bottle", new Vector3(0.0f, 0.0f, -0.05f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 20, ItemSubType = ItemSubType.Water },//slight clipping, no issyes
            new ModItem("Bottle of GREY Water", "Expensive water that tastes worse than tap!", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("h4_prop_battle_waterbottle_01a", new Vector3(0.0f, 0.0f, -0.05f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 20,CleanupItemImmediately = true, ItemSubType = ItemSubType.Water},//lotsa clipping, does not have gravity
            new ModItem("Bottle of JUNK Energy", "The Quick Fix!", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_energy_drink"),HealthChangeAmount = 30, ItemSubType = ItemSubType.Soda},//fine
            //Beer
            new ModItem("Bottle of PiBwasser", "Cheap 11% ABV fighting lager brewed in Germany for export only from rice, barley, hops and the fresh urine of Bavarian virgins", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_amb_beer_bottle"),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5, ItemSubType= ItemSubType.Beer},//is perfecto
            new ModItem("Bottle of A.M.", "Mornings Golden Shower", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beer_am", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Barracho", "Es Playtime!", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beer_bar", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Blarneys", "Making your mouth feel lucky", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beer_blr", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Jakeys", "Drink Outdoors With Jakey's", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beer_jakey", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Logger", "A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beer_logger", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Patriot", "Never refuse a patriot", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beer_patriot", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Pride", "Swallow Me", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beer_pride", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Stronzo", "Birra forte d'Italia", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beer_stz", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            new ModItem("Bottle of Dusche", "Das Ist Gut Ja!", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_beerdusche", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType= ItemSubType.Beer},//Does not have gravity, attachmentis too far down
            //Liquor
            new ModItem("Bottle of 40 oz", "Drink like a true thug!", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_cs_beer_bot_40oz", new Vector3(0.0f, 0.0f, -0.05f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol", ItemSubType= ItemSubType.Beer},
            new ModItem("Bottle of Sinsimito Tequila", "Extra Anejo 100% De Agave. 42% Alcohol by volume", eConsumableType.Drink, ItemType.Drinks){
                PackageItem = new PhysicalItem("h4_prop_h4_t_bottle_02a", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "High Proof Alcohol",HealthChangeAmount = 15,CleanupItemImmediately = true, ItemSubType= ItemSubType.Liquor},
            new ModItem("Bottle of Cazafortuna Tequila", "Tequila Anejo. 100% Blue Agave 40% Alcohol by volume", eConsumableType.Drink, ItemType.Drinks){
                PackageItem = new PhysicalItem("h4_prop_h4_t_bottle_01a", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "High Proof Alcohol",HealthChangeAmount = 15,CleanupItemImmediately = true, ItemSubType= ItemSubType.Liquor},
            //Cups & Cans
            new ModItem("Can of eCola", "Deliciously Infectious!", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("ng_proc_sodacan_01a", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 10, ItemSubType= ItemSubType.Soda},
            new ModItem("Can of Sprunk", "Slurp Sprunk Mmm! Delicious", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("ng_proc_sodacan_01b", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 10, ItemSubType= ItemSubType.Soda},
            new ModItem("Can of Orang-O-Tang", "Orange AND Tang! Orang-O-Tang!", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("prop_orang_can_01"),HealthChangeAmount = 10, ItemSubType= ItemSubType.Soda},//needs better attachment
            new ModItem("Carton of Milk", "Full Fat. Farmed and produced in U.S.A.", eConsumableType.Drink, ItemType.Drinks) { HealthChangeAmount = 10, ItemSubType= ItemSubType.Milk },
            new ModItem("Cup of eCola", "Deliciously Infectious!", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("ng_proc_sodacup_01a", new Vector3(0.0f, 0.0f, -0.2f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 10, ItemSubType= ItemSubType.Soda},//has no gravity, too far down
            new ModItem("Cup of Sprunk", "Slurp Sprunk Mmm! Delicious", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("ng_proc_sodacup_01b", new Vector3(0.0f, 0.0f, -0.2f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 10, ItemSubType= ItemSubType.Soda},//perfecto
            new ModItem("Cup of Coffee", "Finally something without sugar! Sugar on Request", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_02"),HealthChangeAmount = 10, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Can of Hoplivion Double IPA", "So many hops it should be illegal.", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("h4_prop_h4_can_beer_01a", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),IntoxicantName = "Low Proof Alcohol",HealthChangeAmount = 5},//pretty good, maybeslightly off
            new ModItem("Can of Blarneys", "Making your mouth feel lucky", eConsumableType.Drink, ItemType.Drinks) { IntoxicantName = "Low Proof Alcohol", HealthChangeAmount = 5 },
            new ModItem("Can of Logger", "A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned", eConsumableType.Drink, ItemType.Drinks) { IntoxicantName = "Low Proof Alcohol", HealthChangeAmount = 5 },
            //Bean Machine
            new ModItem("High Noon Coffee", "Drip coffee, carbonated water, fruit syrup and taurine.", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 10, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("The Eco-ffee", "Decaf light, rain forest rain, saved whale milk, chemically reclaimed freerange organic tofu, and recycled brown sugar", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 12, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Speedball Coffee", "Caffeine tripe-shot, guarana, bat guano, and mate.", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 15, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Gunkacchino Coffee", "Caffeine, refined sugar, trans fat, high-fructose corn syrup, and cheesecake base.", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 17},//perfecto
            new ModItem("Bratte Coffee", "Double shot latte, and 100 pumps of caramel.", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 5, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Flusher Coffee", "Caffeine, organic castor oil, concanetrated OJ, chicken vindaldo, and senna pods.", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 10, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Caffeagra Coffee", "Caffeine (Straight up), rhino horn, oyster shell, and sildenafil citrate.", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 17, ItemSubType = ItemSubType.Coffee},//perfecto
            new ModItem("Big Fruit Smoothie", "Frothalot, watermel, carbonated water, taurine, and fruit syrup.", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 15, ItemSubType = ItemSubType.Coffee},//perfecto
            //UP N ATOM
            new ModItem("Jumbo Shake", "Almost a whole cow full of milk", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("ng_proc_sodacup_01c", new Vector3(0.0f, 0.0f, -0.2f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 10, ItemSubType = ItemSubType.Milk},//no gravity, attached wrong
            //burger shot
            new ModItem("Double Shot Coffee", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("p_ing_coffeecup_01"),HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType = ItemSubType.Coffee },//n gravity,not attached right
            new ModItem("Liter of eCola", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("ng_proc_sodacup_01a", new Vector3(0.0f, 0.0f, -0.2f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType = ItemSubType.Soda},//n gravity,not attached right
            new ModItem("Liter of Sprunk", eConsumableType.Drink, ItemType.Drinks){
                ModelItem = new PhysicalItem("ng_proc_sodacup_01b", new Vector3(0.0f, 0.0f, -0.2f), new Rotator(0.0f, 0.0f, 0.0f)),HealthChangeAmount = 5,CleanupItemImmediately = true, ItemSubType = ItemSubType.Soda },//n gravity,not attached right 
        });
    }
    private void DefaultConfig_Drugs()
    {
        ModItemsList.AddRange(new List<ModItem>
        {
            //Cigarettes/Cigars
            new ModItem("Redwood Regular", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a"),
                PackageItem = new PhysicalItem("v_ret_ml_cigs", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10, ItemSubType = ItemSubType.Cigarette },
            new ModItem("Redwood Mild", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda. Milder version",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a"),
                PackageItem = new PhysicalItem("v_ret_ml_cigs2", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -5, ItemSubType = ItemSubType.Cigarette },
            new ModItem("Debonaire", "Tobacco products marketed at the more sophisticated smoker, whoever that is",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a"),
                PackageItem = new PhysicalItem("v_ret_ml_cigs3", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10, ItemSubType = ItemSubType.Cigarette },
            new ModItem("Debonaire Menthol", "Tobacco products marketed at the more sophisticated smoker, whoever that is. With Menthol!",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a"),
                PackageItem = new PhysicalItem("v_ret_ml_cigs4", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10, ItemSubType = ItemSubType.Cigarette },
            new ModItem("Caradique", "Fine Napoleon Cigarettes",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a"),
                PackageItem = new PhysicalItem("v_ret_ml_cigs5", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10, ItemSubType = ItemSubType.Cigarette },
            new ModItem("69 Brand","Don't let an embargo stop you",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a"),
                PackageItem = new PhysicalItem("v_ret_ml_cigs6", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -10, ItemSubType = ItemSubType.Cigarette },
            //new Vector3(-0.025f,0.01f,0.004f),new Rotator(0f, 0f, 90f) female mouth attach?



            new ModItem("Estancia Cigar","Medium Cut. Hand Rolled.",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("prop_cigar_02", new Vector3(-0.02f, 0.0f, 0.0f), new Rotator(0.0f, 180f, 0f),new Vector3(-0.015f,0.117f,0.01f),new Rotator(90f, 90f, 0f)) { SecondaryAttachOffsetFemaleOverride = new Vector3(-0.023f,0.087f,0.014f), SecondaryAttachRotationFemaleOverride =  new Rotator(50f, 0f, 90f) },
                PackageItem = new PhysicalItem("p_cigar_pack_02_s", new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter, HealthChangeAmount = -5, ItemSubType = ItemSubType.Cigar },
            //new ModItem("ElectroToke Vape","The Electrotoke uses highly sophisticated micro-molecule atomization technology to make the ingestion of hard drugs healthy, dscreet, pleasurable and, best of all, completely safe.",eConsumableType.Smoke, ItemType.Drugs) {
            //    ModelItem = new PhysicalItem("h4_prop_battle_vape_01"), IntoxicantName = "Marijuana", PercentLostOnUse = 0.05f },





            //Legal Drugs
            new ModItem("White Widow","Among the most famous strains worldwide is White Widow, a balanced hybrid first bred in the Netherlands by Green House Seeds.",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("p_cs_joint_01"),PackageItem = new PhysicalItem("prop_weed_bottle"), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter , ItemSubType = ItemSubType.Narcotic},
            new ModItem("OG Kush","OG Kush, also known as 'Premium OG Kush', was first cultivated in Florida in the early '90s when a marijuana strain from Northern California was supposedly crossed with Chemdawg, Lemon Thai and a Hindu Kush plant from Amsterdam.",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("p_cs_joint_01"),PackageItem = new PhysicalItem("prop_weed_bottle"), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Northern Lights","Northern Lights, also known as 'NL', is an indica marijuana strain made by crossing Afghani with Thai.",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("p_cs_joint_01"),PackageItem = new PhysicalItem("prop_weed_bottle"), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Bull Shark Testosterone","More bite than bush elephant testosterone. Become more aggressive, hornier, and irresistible to women! The ultimate man!",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Bull Shark Testosterone" , AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic},
            new ModItem("Alco Patch","The Alco Patch. It's the same refreshing feeling of your favorite drink, but delivered transdermally and discreetly. Pick up the Alco Patch at your local pharmacy.",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Alco Patch",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic},
            new ModItem("Lax to the Max","Lubricated suppositories. Get flowing again!",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Alco Patch",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic},
            new ModItem("Mollis","For outstanding erections. Get the performance you've always dreamed of",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Mollis",AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic},
            new ModItem("Chesty","Cough suppressant manufactured by Good Aids Pharmacy. Gives 24-hour relief and is available in honey flavour.",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Chesty", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Equanox","Combats dissatisfaction, lethargy, depression, melancholy, sexual dysfunction. Equanox may cause nausea, loss of sleep, blurred vision, leakage, kidney problems and breathing irregularities.",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Equanox", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Zombix","Painkiller and antidepressant manufactured by O'Deas Pharmacy. ~n~'Go straight for the head.'",eConsumableType.Ingest, ItemType.Drugs) {
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Zombix", AmountPerPackage = 10, ItemSubType = ItemSubType.Narcotic },
            //Illegal Drugs
            new ModItem("Marijuana","Little Jacob Tested, Truth Approved",eConsumableType.Smoke, ItemType.Drugs) {
                ModelItem = new PhysicalItem("p_cs_joint_01")//p_amb_joint_01
                ,PackageItem = new PhysicalItem("sf_prop_sf_bag_weed_01a"), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
            new ModItem("SPANK","You looking for some fun? a little.. hmmm? Some SPANK?",eConsumableType.Ingest, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "SPANK", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic },
            new ModItem("Toilet Cleaner","Meth brought you forbidden fruits of incest. Bath salts brought you the taboo joys of cannibalism. It's time to step things up a level. The hot new legal high that takes you to places you never imagined and leaves you forever changed - Toilet Cleaner.",eConsumableType.Ingest, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItem = new PhysicalItem("prop_cs_pills",new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Toilet Cleaner", ItemSubType = ItemSubType.Narcotic },
            new ModItem("Cocaine","Also known as coke, crack, girl, lady, charlie, caine, tepung, and snow",eConsumableType.Snort, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItem = new PhysicalItem("ba_prop_battle_sniffing_pipe",new Vector3(0.11f, 0.0f, -0.02f),new Rotator(-179f, 72f, -28f))
                ,PackageItem = new PhysicalItem("prop_meth_bag_01")
                ,IntoxicantName = "Cocaine", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic },
            new ModItem("Crack","",eConsumableType.AltSmoke, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItem = new PhysicalItem("prop_cs_crackpipe",new Vector3(0.14f, 0.07f, 0.02f),new Rotator(-119f, 47f, 0f))
                ,PackageItem = new PhysicalItem("prop_meth_bag_01")
                ,IntoxicantName = "Crack", PercentLostOnUse = 0.5f, MeasurementName = "Gram", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
            new ModItem("Heroin","Heroin was first made by C. R. Alder Wright in 1874 from morphine, a natural product of the opium poppy",eConsumableType.Inject, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItem = new PhysicalItem("prop_syringe_01",new Vector3(0.16f, 0.02f, -0.07f),new Rotator(-170f, -148f, -36f))
                ,PackageItem = new PhysicalItem("prop_meth_bag_01")
                ,IntoxicantName = "Heroin", PercentLostOnUse = 0.5f, MeasurementName = "Gram", ItemSubType = ItemSubType.Narcotic },
            new ModItem("Methamphetamine","Also referred to as Speed, Sabu, Crystal and Meth",eConsumableType.AltSmoke, ItemType.Drugs) { IsPossessionIllicit = true,
                ModelItem = new PhysicalItem("prop_cs_meth_pipe",new Vector3(0.14f, 0.05f, -0.01f),new Rotator(-119f, 0f, 0f))
                ,PackageItem = new PhysicalItem("prop_meth_bag_01")
                ,IntoxicantName = "Methamphetamine", PercentLostOnUse = 0.25f, MeasurementName = "Gram", RequiredToolType = ToolTypes.Lighter, ItemSubType = ItemSubType.Narcotic },
        });
    }
    private void DefaultConfig_Food()
    {
        ModItemsList.AddRange(new List<ModItem>
        {
            //Generic Food
            new ModItem("Hot Dog","Your favorite mystery meat sold on street corners everywhere. Niko would be proud",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_hotdog_01",new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Hot Sausage","Get all your jokes out",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_hotdog_01",new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Hot Pretzel","You tie me up",eConsumableType.Eat, ItemType.Food) {HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("3 Mini Pretzels","Like a pretzel, but smaller",eConsumableType.Eat, ItemType.Food) {HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Nuts","You're gonna love my nuts",eConsumableType.Eat, ItemType.Food) {HealthChangeAmount = 10, ItemSubType = ItemSubType.Snack },
            new ModItem("Burger","100% Certified Food",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Donut","MMMMMMM Donuts",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_donut_01",new Vector3(0.15f, 0.01f, -0.03f),new Rotator(-15.0f, 17.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Snack } ,
            new ModItem("Bagel Sandwich","Bagel with extras, what more do you need?",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("p_amb_bagel_01",new Vector3(0.15f, 0.01f, -0.03f),new Rotator(-15.0f, 17.0f, 0.0f)), HealthChangeAmount = 12, ItemSubType = ItemSubType.Entree } ,
            new ModItem("French Fries","Freedom fries made from true Cataldo potatoes!",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_chips",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 5, ItemSubType = ItemSubType.Side },
            new ModItem("Fries","Freedom fries made from true Cataldo potatoes!",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_chips",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 5, ItemSubType = ItemSubType.Side },
            new ModItem("Banana","An elongated, edible fruit – botanically a berry[1][2] – produced by several kinds of large herbaceous flowering plants in the genus Musa",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("ng_proc_food_nana1a",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Fruit },
            new ModItem("Orange","Not just a color",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("ng_proc_food_ornge1a",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Fruit },
            new ModItem("Apple","Certified sleeping death free",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("ng_proc_food_aple1a",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Fruit },
            new ModItem("Ham and Cheese Sandwich","Basic and shitty, just like you",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_sandwich_01",new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Turkey Sandwich","The most plain sandwich for the most plain person",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_sandwich_01",new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Tuna Sandwich","Haven't got enough heavy metals in you at your job? Try tuna!",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_sandwich_01",new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Taco",eConsumableType.Eat, ItemType.Food) { HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree, ModelItem = new PhysicalItem("prop_taco_01") },
            new ModItem("Strawberry Rails Cereal","The breakfast food you snort!",eConsumableType.Eat, ItemType.Food) { HealthChangeAmount = 50, ItemSubType = ItemSubType.Cereal} ,
            new ModItem("Crackles O' Dawn Cereal","Smile at the crack!",eConsumableType.Eat, ItemType.Food) { HealthChangeAmount = 60, ItemSubType = ItemSubType.Cereal} ,
            new ModItem("White Bread","Extra white, with minimal taste.",eConsumableType.Eat, ItemType.Food) { HealthChangeAmount = 10, AmountPerPackage = 25, ItemSubType = ItemSubType.Bread} ,
            //Pizza
            new ModItem("Slice of Pizza","Caution may be hot",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("v_res_tt_pizzaplate",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Small Cheese Pizza","Best when you are home alone.",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 25, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Small Pepperoni Pizza","Get a load of our pepperonis!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 30, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Small Supreme Pizza","Get stuffed",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 35, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Medium Cheese Pizza","Best when you are home alone.",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 50, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Medium Pepperoni Pizza","Get a load of our pepperonis!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 55, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Medium Supreme Pizza","Get stuffed",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 60, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Large Cheese Pizza","Best when you are home alone.",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 65, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Large Pepperoni Pizza","Get a load of our pepperonis!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 70, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("Large Supreme Pizza","Get stuffed",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_02",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 75, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("10 inch Cheese Pizza","Extra cheesy!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 25, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("10 inch Pepperoni Pizza","Mostly Meat!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 30, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("10 inch Supreme Pizza","We forgot the kitchen sink!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 35, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("12 inch Cheese Pizza","Extra cheesy!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 50, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("12 inch Pepperoni Pizza","Mostly Meat!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 55, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("12 inch Supreme Pizza","We forgot the kitchen sink!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 60, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("18 inch Cheese Pizza","Extra cheesy! Extra Large!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 65, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("18 inch Pepperoni Pizza","Mostly Meat! Extra Large!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 70, ItemSubType = ItemSubType.Pizza } ,
            new ModItem("18 inch Supreme Pizza","We forgot the kitchen sink! Extra Large!",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_pizza_box_01",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 75, ItemSubType = ItemSubType.Pizza } ,
            //Chips
            new ModItem("Sticky Rib Phat Chips","They are extra phat. Sticky Rib Flavor.",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("v_ret_ml_chips1",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Snack },
            new ModItem("Habanero Phat Chips","They are extra phat. Habanero flavor",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("v_ret_ml_chips2",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Snack },
            new ModItem("Supersalt Phat Chips","They are extra phat. Supersalt flavor.",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("v_ret_ml_chips3",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Snack },
            new ModItem("Big Cheese Phat Chips","They are extra phat. Big Cheese flavor.",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("v_ret_ml_chips4",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Snack },
            //Candy
            new ModItem("Ego Chaser Energy Bar","Contains 20,000 Calories! ~n~'It's all about you'",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_choc_ego",new Vector3(0.13f, 0.05f, -0.02f),new Rotator(25f, -11f, -95f)), HealthChangeAmount = 20, ItemSubType = ItemSubType.Snack },
            new ModItem("King Size P's & Q's","The candy bar that kids and stoners love. EXTRA Large",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_candy_pqs",new Vector3(0.16f, 0.01f, -0.02f),new Rotator(-178f, -169f, 169f)), HealthChangeAmount = 15, ItemSubType = ItemSubType.Snack },
            new ModItem("P's & Q's","The candy bar that kids and stoners love",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_choc_pq",new Vector3(0.12f, 0.02f, -0.02f),new Rotator(-178f, -169f, 79f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Snack },
            new ModItem("Meteorite Bar","Dark chocolate with a GOOEY core",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_choc_meto",new Vector3(0.12f, 0.03f, -0.02f),new Rotator(169f, 170f, 76f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Snack },
            //UPNATOM
            new ModItem("Triple Burger", "Three times the meat, three times the cholesterol", eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Bacon Triple Cheese Melt", "More meat AND more bacon", eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            //BurgerShot
            new ModItem("Money Shot Meal",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_bs_burg1",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_02"), HealthChangeAmount = 12, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("The Bleeder Meal","",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_bs_burg1",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_02"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Torpedo Meal","Torpedo your hunger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_03"), HealthChangeAmount = 15, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Meat Free Meal","For the bleeding hearts",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_01"), HealthChangeAmount = 5, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Freedom Fries",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_bs_chips",new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthChangeAmount = 5, ItemSubType = ItemSubType.Snack },
            //Bite
            new ModItem("Gut Buster Sandwich",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_burg2",new Vector3(0.14f, 0.01f, -0.06f),new Rotator(0f, 0f, 0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Ham and Tuna Sandwich",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_burg2",new Vector3(0.14f, 0.01f, -0.06f),new Rotator(0f, 0f, 0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Chef's Salad",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 5, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree} ,
            //BeefyBills
            new ModItem("Megacheese Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Double Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 12, ItemSubType = ItemSubType.Entree },
            new ModItem("Kingsize Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 15, ItemSubType = ItemSubType.Entree },
            new ModItem("Bacon Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 17, ItemSubType = ItemSubType.Entree },
            //Taco Bomb
            new ModItem("Breakfast Burrito",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 12, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Deep Fried Salad",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 10, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Beef Bazooka",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 15, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Chimichingado Chiquito",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 10, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Cheesy Meat Flappers",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 10, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Volcano Mudsplatter Nachos",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 10, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            //Wigwam
            new ModItem("Wigwam Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Wigwam Cheeseburger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 12, ItemSubType = ItemSubType.Entree },
            new ModItem("Big Wig Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 12, ItemSubType = ItemSubType.Entree },
            //Cluckin Bell
            new ModItem("Cluckin' Little Meal","May contain meat",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthChangeAmount = 5, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new ModItem("Cluckin' Big Meal","200% bigger breasts",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_02"), HealthChangeAmount = 10, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Cluckin' Huge Meal",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_02"), HealthChangeAmount = 15, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Wing Piece",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthChangeAmount = 10, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new ModItem("Little Peckers",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthChangeAmount = 5 , ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree},
            new ModItem("Balls & Rings",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_burg3",new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthChangeAmount = 5, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Side },
            new ModItem("Fowlburger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_food_burg1",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
                PackageItem = new PhysicalItem("prop_food_burg3"), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree } ,
            //Generic Restaurant
            //FancyDeli
            new ModItem("Chicken Club Salad",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Spicy Seafood Gumbo",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Muffaletta",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Zucchini Garden Pasta",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Pollo Mexicano",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Italian Cruz Po'boy",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Chipotle Chicken Panini",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //FancyFish
            new ModItem("Coconut Crusted Prawns",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Crab and Shrimp Louie",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Open-Faced Crab Melt",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("King Salmon",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Ahi Tuna",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Key Lime Pie",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //FancyGeneric
            new ModItem("Smokehouse Burger",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree },
            new ModItem("Chicken Critters Basket",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Prime Rib 16 oz",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Bone-In Ribeye",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_cs_steak"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Grilled Pork Chops",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Grilled Shrimp",eConsumableType.Eat, ItemType.Food) {
                PackageItem = new PhysicalItem("prop_food_bag1") , HealthChangeAmount = 20, ConsumeOnPurchase = true, ItemSubType = ItemSubType.Entree } ,
            //Noodles
            new ModItem("Juek Suk tong Mandu",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_ff_noodle_01"), HealthChangeAmount = 10, ItemSubType = ItemSubType.Entree },
            new ModItem("Hayan Jam Pong",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_ff_noodle_02"), HealthChangeAmount = 15, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Sal Gook Su Jam Pong",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_ff_noodle_01"), HealthChangeAmount = 20, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Chul Pan Bokkeum Jam Pong",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_ff_noodle_02"), HealthChangeAmount = 20, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Deul Gae Udon",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_ff_noodle_02"), HealthChangeAmount = 20, ItemSubType = ItemSubType.Entree } ,
            new ModItem("Dakgogo Bokkeum Bap",eConsumableType.Eat, ItemType.Food) {
                ModelItem = new PhysicalItem("prop_ff_noodle_01"), HealthChangeAmount = 20, ItemSubType = ItemSubType.Entree } ,
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
                ModelItem = new PhysicalItem("prop_tool_screwdvr01",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Screwdriver },
            new ModItem("Drill","2-Speed Battery Drill. Impact-resistant casing. Light, compact and easy to use.", ItemType.Tools) {
                ModelItem = new PhysicalItem("prop_tool_drill",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Drill  },
            new ModItem("Pliers","For mechanics, pipe bomb makers, and amateur dentists alike. When you really need to grab something.", ItemType.Tools) {
                ModelItem = new PhysicalItem("prop_tool_pliers",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Pliers  },
            new ModItem("Shovel","A lot of holes in the desert, and a lot of problems are buried in those holes. But you gotta do it right. I mean, you gotta have the hole already dug before you show up with a package in the trunk.", ItemType.Tools) {
                ModelItem = new PhysicalItem("prop_tool_shovel",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) { IsLarge = true }, ToolType = ToolTypes.Shovel  },
            new ModItem("DIC Lighter","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged", ItemType.Tools) {
                ModelItem = new PhysicalItem("p_cs_lighter_01",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.01f },
            new ModItem("Bong","Also known as a water pipe", ItemType.Tools) {
                ModelItem = new PhysicalItem("prop_bong_01"), ToolType = ToolTypes.Bong } ,
            new ModItem("DIC Lighter Ultra","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Long burn version.", ItemType.Tools) {
                ModelItem = new PhysicalItem("p_cs_lighter_01",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.005f },
            new ModItem("Dippo Lighter","Want to have all the hassle of carrying a lighter only for it to be out of fluid when you need it? Dippo is for you!", ItemType.Tools) {
                ModelItem = new PhysicalItem("v_res_tt_lighter",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.05f },
            new ModItem("DIC Lighter Silver","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Too poor for gold?", ItemType.Tools) {
                ModelItem = new PhysicalItem("ex_prop_exec_lighter_01",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.02f },
            new ModItem("DIC Lighter Gold","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Golden so it must be good!", ItemType.Tools) {
                ModelItem = new PhysicalItem("lux_prop_lighter_luxe",new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.02f },
        });
    }
    private void DefaultConfig_Vehicles()
    {
        ModItemsList.AddRange(new List<ModItem> {
            //Cars & Motorcycles
            new ModItem("Albany Alpha", "Blending modern performance and design with the classic luxury styling of a stately car, the Alpha is sleek, sexy and handles so well you'll forget you're driving it. Which could be a problem at 150 mph...", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("alpha") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Roosevelt","Party like it's the Prohibition era in this armored 1920s limousine. Perfect for a gangster and his moll on their first date or their last. Let the Valentine's Day massacres commence.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("btype") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Fränken Stange","The unlikely product of Albany's design team leafing through a vintage car magazine while in the depths of a masculine overdose. The Franken Stange will make you the envy of goths, emo hipsters and vampire wannabes everywhere. Don't be fooled by what's left of its old world charm; the steering linkage may be from 1910, but the engine has just enough horsepower to tear itself (and you) to pieces at the first bump in the road.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("btype2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Roosevelt Valor","They don't make them like they used to, which is a good thing because here at Albany we've completely run out of ideas. Lovingly remodelled, with room for a new suite of personal modifications, the latest edition of our classic Roosevelt represents a new height of criminal refinement, taking you back to the golden age of fraud, racketeering and murder when all you had to worry about were a few charges of tax evasion.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("btype3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Buccaneer","With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", ItemType.Vehicles) { ModelItem = new PhysicalItem("buccaneer") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Buccaneer Custom","With the kind of pedigree that just screams 'organized crime', the Buccaneer has always been the vehicle of choice for thugs with delusions of grandeur. But while the mobsters of yesterday had to settle for being classy and understated, today you have access to the kind of modification that will make you a target for racially motivated policing across the length and breadth of San Andreas. Eligible for customization at Benny's Original Motor Works.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("buccaneer2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Cavalcade","You could scarcely cross the street without getting mown down by a soccer mom or drug dealer in one of these during the early 2000s. The glory days of the excessively-large, gas-guzzling SUV might be over, but the Cavalcade takes no prisoners.", ItemType.Vehicles) { ModelItem = new PhysicalItem("cavalcade") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Cavalcade 2","The old man luxury automobile, but once you sit inside this comfy car that steers like a boat, you'll know why your old man often fell asleep at the wheel.", ItemType.Vehicles) { ModelItem = new PhysicalItem("cavalcade2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Emperor", ItemType.Vehicles) { ModelItem = new PhysicalItem("emperor") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Emperor 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("Emperor2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Emperor 3", ItemType.Vehicles) { ModelItem = new PhysicalItem("emperor3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Hermes", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("hermes") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Lurcher", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("lurcher") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Manana", ItemType.Vehicles) { ModelItem = new PhysicalItem("manana") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Manana Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("manana2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Primo", ItemType.Vehicles) { ModelItem = new PhysicalItem("primo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Primo Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("primo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Virgo", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("virgo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany V-STR", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("vstr") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Albany Washington", ItemType.Vehicles) { ModelItem = new PhysicalItem("washington") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Elegy Retro Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("elegy") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Elegy RH8", ItemType.Vehicles) { ModelItem = new PhysicalItem("elegy2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Euros", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("Euros") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Hellion", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("hellion") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis RE-7B", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("le7b") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Remus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("remus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis S80RR", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("s80") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Savestra", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("savestra") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis ZR350", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zr350") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Apocalypse ZR380", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zr380") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Future Shock ZR380", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zr3802") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Annis Nightmare ZR380", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zr3803") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Apocalypse Bruiser", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("bruiser") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Future Shock Bruiser", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("bruiser2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Nightmare Bruiser", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("bruiser3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Dubsta", ItemType.Vehicles) { ModelItem = new PhysicalItem("dubsta") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Dubsta 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("dubsta2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Dubsta 6x6", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dubsta3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Feltzer", ItemType.Vehicles) { ModelItem = new PhysicalItem("feltzer2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Stirling GT", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("feltzer3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Glendale", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("glendale") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Glendale Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("glendale2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Turreted Limo", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("limo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor BR8", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("openwheel1") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Panto", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("panto") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Schafter", "Good-looking yet utilitarian, sexy yet asexual, slender yet terrifyingly powerful, the Schafter is German engineering at its very finest.", ItemType.Vehicles) { ModelItem = new PhysicalItem("schafter2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Schafter V12", "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has a V12 engine.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("schafter3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Schafter LWB", "Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has an extended wheelbase.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("schafter4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Schafter V12 (Armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("schafter5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Schafter LWB (Armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("schafter6") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Schwartzer", "Say what you will about the Germans - they know luxury. And their economy is the only one worth a crap in Europe. This model has all kinds of extras - too many to list for legal reasons.", ItemType.Vehicles) { ModelItem = new PhysicalItem("schwarzer") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Serrano", "Fun fact: what's the fastest growing market in the American auto industry? That's right! Compact SUVs! And do you know why? That's right! Neither do we! And is that a good enough reason to buy one? That's right! It had better be!", ItemType.Vehicles) { ModelItem = new PhysicalItem("serrano") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Surano", "This is luxury reasserted. Right in your neighbour's face. Boom. You like that. That's right, you are better than him, and you could have his wife if you wanted. Try it on with her as soon as she sees this ride. You'll be a double benefactor.", ItemType.Vehicles) { ModelItem = new PhysicalItem("Surano") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor XLS", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("xls") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor XLS (Armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("xls2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Krieger", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("krieger") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Schlagen GT", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("schlagen") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Streiter", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("streiter") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Benefactor Terrorbyte", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("terbyte") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Injection", ItemType.Vehicles) { ModelItem = new PhysicalItem("BfInjection") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Bifta", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("bifta") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Club", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("club") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Dune Buggy", ItemType.Vehicles) { ModelItem = new PhysicalItem("dune") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Dune FAV", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dune3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Raptor", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("raptor") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Surfer", ItemType.Vehicles) { ModelItem = new PhysicalItem("SURFER") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Surfer", ItemType.Vehicles) { ModelItem = new PhysicalItem("Surfer2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("BF Weevil", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("weevil") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bollokan Prairie", ItemType.Vehicles) { ModelItem = new PhysicalItem("prairie") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Banshee", ItemType.Vehicles) { ModelItem = new PhysicalItem("banshee") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Banshee 900R", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("banshee2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Bison", ItemType.Vehicles) { ModelItem = new PhysicalItem("bison") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Bison 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("Bison2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Bison 3", ItemType.Vehicles) { ModelItem = new PhysicalItem("Bison3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Buffalo", ItemType.Vehicles) { ModelItem = new PhysicalItem("buffalo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Buffalo S", ItemType.Vehicles) { ModelItem = new PhysicalItem("buffalo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Sprunk Buffalo", ItemType.Vehicles) { ModelItem = new PhysicalItem("buffalo3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Duneloader", ItemType.Vehicles) { ModelItem = new PhysicalItem("dloader") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Gauntlet", ItemType.Vehicles) { ModelItem = new PhysicalItem("Gauntlet") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Redwood Gauntlet", ItemType.Vehicles) { ModelItem = new PhysicalItem("gauntlet2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Gauntlet Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("gauntlet3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Gauntlet Hellfire", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("gauntlet4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Gauntlet Classic Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("gauntlet5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Gresley", ItemType.Vehicles) { ModelItem = new PhysicalItem("gresley") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Half-track", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("halftrack") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Apocalypse Sasquatch", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("monster3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Future Shock Sasquatch", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("monster4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Nightmare Sasquatch", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("monster5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Paradise", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("paradise") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Rat-Truck", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("ratloader2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Rumpo", ItemType.Vehicles) { ModelItem = new PhysicalItem("rumpo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Rumpo 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("rumpo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Rumpo Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("rumpo3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Verlierer", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("verlierer2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Youga", ItemType.Vehicles) { ModelItem = new PhysicalItem("youga") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Youga Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("youga2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Bravado Youga Classic 4x4", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("youga3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Boxville", ItemType.Vehicles) { ModelItem = new PhysicalItem("boxville") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Boxville 3", ItemType.Vehicles) { ModelItem = new PhysicalItem("boxville3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Boxville 4", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("boxville4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Camper", ItemType.Vehicles) { ModelItem = new PhysicalItem("CAMPER") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Pony", ItemType.Vehicles) { ModelItem = new PhysicalItem("pony") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Pony 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("pony2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Stockade", ItemType.Vehicles) { ModelItem = new PhysicalItem("stockade") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Stockade 3", ItemType.Vehicles) { ModelItem = new PhysicalItem("stockade3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Brute Tipper", ItemType.Vehicles) { ModelItem = new PhysicalItem("TipTruck") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Bodhi", ItemType.Vehicles) { ModelItem = new PhysicalItem("Bodhi2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Crusader", ItemType.Vehicles) { ModelItem = new PhysicalItem("CRUSADER") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Freecrawler", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("freecrawler") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Kalahari", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("kalahari") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Kamacho", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("kamacho") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Mesa", ItemType.Vehicles) { ModelItem = new PhysicalItem("MESA") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Mesa 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("mesa2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Mesa 3", ItemType.Vehicles) { ModelItem = new PhysicalItem("MESA3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Seminole", ItemType.Vehicles) { ModelItem = new PhysicalItem("Seminole") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Canis Seminole Frontier", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("seminole2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Chariot Romero Hearse", ItemType.Vehicles) { ModelItem = new PhysicalItem("romero") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Cheval Fugitive", ItemType.Vehicles) { ModelItem = new PhysicalItem("fugitive") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Cheval Marshall", ItemType.Vehicles) { ModelItem = new PhysicalItem("marshall") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Cheval Picador", ItemType.Vehicles) { ModelItem = new PhysicalItem("picador") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Cheval Surge", ItemType.Vehicles) { ModelItem = new PhysicalItem("surge") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Cheval Taipan", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("taipan") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Coil Brawler", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("brawler") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Coil Cyclone", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cyclone") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Coil Raiden", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("raiden") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Coil Voltic", ItemType.Vehicles) { ModelItem = new PhysicalItem("voltic") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Coil Rocket Voltic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("voltic2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Asea", ItemType.Vehicles) { ModelItem = new PhysicalItem("asea") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Asea", ItemType.Vehicles) { ModelItem = new PhysicalItem("asea2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Apocalypse Brutus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("brutus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Future Shock Brutus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("brutus2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Nightmare Brutus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("brutus3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Burrito", ItemType.Vehicles) { ModelItem = new PhysicalItem("Burrito") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Bugstars Burrito", ItemType.Vehicles) { ModelItem = new PhysicalItem("burrito2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Burrito 3", ItemType.Vehicles) { ModelItem = new PhysicalItem("burrito3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Burrito 4", ItemType.Vehicles) { ModelItem = new PhysicalItem("Burrito4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Burrito 5", ItemType.Vehicles) { ModelItem = new PhysicalItem("burrito5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Gang Burrito", ItemType.Vehicles) { ModelItem = new PhysicalItem("gburrito") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Gang Burrito 2", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("gburrito2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Granger", ItemType.Vehicles) { ModelItem = new PhysicalItem("GRANGER") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Hotring Sabre", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("hotring") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Impaler", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("impaler") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Apocalypse Impaler", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("impaler2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Future Shock Impaler", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("impaler3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Nightmare Impaler", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("impaler4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Lifeguard", ItemType.Vehicles) { ModelItem = new PhysicalItem("lguard") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Mamba", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("mamba") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Moonbeam", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("moonbeam") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Moonbeam Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("moonbeam2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse DR1", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("openwheel2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Premier", ItemType.Vehicles) { ModelItem = new PhysicalItem("premier") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Rancher XL", ItemType.Vehicles) { ModelItem = new PhysicalItem("RancherXL") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Rancher XL 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("rancherxl2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Rhapsody", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("rhapsody") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Sabre Turbo", ItemType.Vehicles) { ModelItem = new PhysicalItem("sabregt") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Sabre Turbo Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sabregt2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Scramjet", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("scramjet") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Stallion", ItemType.Vehicles) { ModelItem = new PhysicalItem("stalion") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Burger Shot Stallion", ItemType.Vehicles) { ModelItem = new PhysicalItem("stalion2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Tampa", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tampa") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Drift Tampa", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tampa2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Weaponized Tampa", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tampa3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Tornado", ItemType.Vehicles) { ModelItem = new PhysicalItem("tornado") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Tornado 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("tornado2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Tornado 3", ItemType.Vehicles) { ModelItem = new PhysicalItem("tornado3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Tornado 4", ItemType.Vehicles) { ModelItem = new PhysicalItem("tornado4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Tornado Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tornado5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Tornado Rat Rod", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tornado6") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Tulip", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tulip") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Vamos", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("vamos") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Vigero", ItemType.Vehicles) { ModelItem = new PhysicalItem("vigero") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Voodoo Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("voodoo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Voodoo", ItemType.Vehicles) { ModelItem = new PhysicalItem("voodoo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Yosemite", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("yosemite") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Drift Yosemite", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("yosemite2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Declasse Yosemite Rancher", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("yosemite3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Exemplar", ItemType.Vehicles) { ModelItem = new PhysicalItem("exemplar") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee JB 700", ItemType.Vehicles) { ModelItem = new PhysicalItem("jb700") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee JB 700W", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("jb7002") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Massacro", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("massacro") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Massacro (Racecar)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("massacro2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Rapid GT", ItemType.Vehicles) { ModelItem = new PhysicalItem("RapidGT") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Rapid GT 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("RapidGT2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Rapid GT Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("rapidgt3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Seven-70", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("SEVEN70") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Specter", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("SPECTER") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Specter Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("SPECTER2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dewbauchee Vagner", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("vagner") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Akuma", ItemType.Vehicles) { ModelItem = new PhysicalItem("akuma") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Blista", ItemType.Vehicles) { ModelItem = new PhysicalItem("blista") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Blista Compact", ItemType.Vehicles) { ModelItem = new PhysicalItem("blista2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Go Go Monkey Blista", ItemType.Vehicles) { ModelItem = new PhysicalItem("blista3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Double-T", ItemType.Vehicles) { ModelItem = new PhysicalItem("double") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Enduro", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("enduro") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Jester", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("jester") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Jester (Racecar)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("jester2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Jester Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("jester3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Jester RR", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("jester4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Blista Kanjo", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("kanjo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka RT3000", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("rt3000") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Sugoi", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("Sugoi") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Thrust", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("thrust") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Verus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("verus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Veto Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("veto") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Veto Modern", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("veto2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dinka Vindicator", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("vindicator") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dundreary Landstalker", ItemType.Vehicles) { ModelItem = new PhysicalItem("landstalker") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dundreary Landstalker XL", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("landstalker2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dundreary Regina", ItemType.Vehicles) { ModelItem = new PhysicalItem("regina") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dundreary Stretch", ItemType.Vehicles) { ModelItem = new PhysicalItem("stretch") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dundreary Virgo Classic Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("virgo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Dundreary Virgo Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("virgo3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Emperor Habanero", ItemType.Vehicles) { ModelItem = new PhysicalItem("habanero") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Emperor ETR1", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sheava") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Emperor Vectre", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("vectre") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Cognoscenti 55", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cog55") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Cognoscenti 55 (Armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cog552") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Cognoscenti Cabrio", ItemType.Vehicles) { ModelItem = new PhysicalItem("cogcabrio") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Cognoscenti", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cognoscenti") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Cognoscenti (Armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cognoscenti2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Huntley S", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("huntley") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Paragon R", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("paragon") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Paragon R (Armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("paragon2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Stafford", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("stafford") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Super Diamond", ItemType.Vehicles) { ModelItem = new PhysicalItem("superd") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Windsor", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("windsor") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Enus Windsor Drop", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("windsor2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Fathom FQ 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("fq2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Gallivanter Baller", ItemType.Vehicles) { ModelItem = new PhysicalItem("Baller") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Gallivanter Baller 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("baller2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Gallivanter Baller LE", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("baller3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Gallivanter Baller LE LWB", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("baller4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Gallivanter Baller LE (Armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("baller5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Gallivanter Baller LE LWB (Armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("baller6") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Bestia GTS", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("bestiagts") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Brioso R/A", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("brioso") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Brioso 300", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("brioso2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Carbonizzare", ItemType.Vehicles) { ModelItem = new PhysicalItem("carbonizzare") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Cheetah", ItemType.Vehicles) { ModelItem = new PhysicalItem("cheetah") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Cheetah Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cheetah2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Furia", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("furia") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti GT500", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("gt500") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Itali GTO", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("italigto") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Itali RSX", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("italirsx") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti X80 Proto", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("prototipo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Stinger", ItemType.Vehicles) { ModelItem = new PhysicalItem("stinger") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Stinger GT", ItemType.Vehicles) { ModelItem = new PhysicalItem("stingergt") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Turismo Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("turismo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Turismo R", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("turismor") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Grotti Visione", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("visione") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Hijak Khamelion", ItemType.Vehicles) { ModelItem = new PhysicalItem("khamelion") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Hijak Ruston", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("ruston") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Barracks Semi", ItemType.Vehicles) { ModelItem = new PhysicalItem("BARRACKS2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Biff", ItemType.Vehicles) { ModelItem = new PhysicalItem("Biff") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Dozer", ItemType.Vehicles) { ModelItem = new PhysicalItem("bulldozer") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Cutter", ItemType.Vehicles) { ModelItem = new PhysicalItem("cutter") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Dump", ItemType.Vehicles) { ModelItem = new PhysicalItem("dump") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Forklift", ItemType.Vehicles) { ModelItem = new PhysicalItem("FORKLIFT") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Insurgent Pick-Up", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("insurgent") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Insurgent", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("insurgent2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Insurgent Pick-Up Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("insurgent3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Menacer", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("menacer") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Mixer", ItemType.Vehicles) { ModelItem = new PhysicalItem("Mixer") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Mixer 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("Mixer2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Nightshark", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("nightshark") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Apocalypse Scarab", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("scarab") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Future Shock Scarab", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("scarab2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("HVY Nightmare Scarab", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("scarab3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Deluxo", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("deluxo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Dukes", ItemType.Vehicles) { ModelItem = new PhysicalItem("dukes") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Duke O'Death", ItemType.Vehicles) { ModelItem = new PhysicalItem("dukes2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Beater Dukes", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dukes3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Nightshade", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("nightshade") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Phoenix", ItemType.Vehicles) { ModelItem = new PhysicalItem("Phoenix") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Ruiner", ItemType.Vehicles) { ModelItem = new PhysicalItem("ruiner") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Ruiner 2000", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("ruiner2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Imponte Ruiner", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("ruiner3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Invetero Coquette", ItemType.Vehicles) { ModelItem = new PhysicalItem("coquette") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Invetero Coquette Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("coquette2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Invetero Coquette BlackFin", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("coquette3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Invetero Coquette D10", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("coquette4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("JoBuilt Hauler", ItemType.Vehicles) { ModelItem = new PhysicalItem("Hauler") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("JoBuilt Hauler Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("Hauler2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("JoBuilt Phantom", ItemType.Vehicles) { ModelItem = new PhysicalItem("Phantom") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("JoBuilt Phantom Wedge", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("phantom2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("JoBuilt Phantom Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("phantom3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("JoBuilt Rubble", ItemType.Vehicles) { ModelItem = new PhysicalItem("Rubble") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Asterope", ItemType.Vehicles) { ModelItem = new PhysicalItem("asterope") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin BeeJay XL", ItemType.Vehicles) { ModelItem = new PhysicalItem("BjXL") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Calico GTF", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("calico") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Dilettante", ItemType.Vehicles) { ModelItem = new PhysicalItem("dilettante") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Dilettante 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("dilettante2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Everon", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("everon") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Futo", ItemType.Vehicles) { ModelItem = new PhysicalItem("futo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Futo GTX", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("futo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Intruder", ItemType.Vehicles) { ModelItem = new PhysicalItem("intruder") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Kuruma", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("kuruma") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Kuruma (armored)", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("kuruma2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Previon", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("previon") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Rusty Rebel", ItemType.Vehicles) { ModelItem = new PhysicalItem("Rebel") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Rebel", ItemType.Vehicles) { ModelItem = new PhysicalItem("rebel2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Sultan", ItemType.Vehicles) { ModelItem = new PhysicalItem("sultan") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Sultan Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sultan2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Sultan RS Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sultan3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Sultan RS", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sultanrs") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Technical", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("technical") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin Technical Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("technical3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Karin 190z", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("z190") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Casco", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("casco") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Felon", ItemType.Vehicles) { ModelItem = new PhysicalItem("felon") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Felon GT", ItemType.Vehicles) { ModelItem = new PhysicalItem("felon2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Furore GT", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("furoregt") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Michelli GT", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("michelli") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Pigalle", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("pigalle") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Tropos Rallye", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tropos") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Komoda", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("komoda") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Novak", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("Novak") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Tigon", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tigon") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Viseris", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("viseris") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("LCC Avarus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("avarus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("LCC Hexer", ItemType.Vehicles) { ModelItem = new PhysicalItem("hexer") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("LCC Innovation", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("innovation") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("LCC Sanctus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sanctus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Manchez", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("manchez") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Manchez Scout", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("manchez2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Mule", ItemType.Vehicles) { ModelItem = new PhysicalItem("Mule") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Mule", ItemType.Vehicles) { ModelItem = new PhysicalItem("Mule2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Mule", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("Mule3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Mule Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("mule4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Penumbra", ItemType.Vehicles) { ModelItem = new PhysicalItem("penumbra") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Penumbra FF", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("penumbra2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Sanchez (livery)", ItemType.Vehicles) { ModelItem = new PhysicalItem("Sanchez") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maibatsu Sanchez", ItemType.Vehicles) { ModelItem = new PhysicalItem("sanchez2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Patriot", ItemType.Vehicles) { ModelItem = new PhysicalItem("patriot") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Patriot Stretch", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("patriot2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Squaddie", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("squaddie") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maxwell Asbo", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("asbo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Maxwell Vagrant", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("vagrant") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Brickade", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("brickade") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Apocalypse Cerberus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cerberus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Future Shock Cerberus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cerberus2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Nightmare Cerberus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cerberus3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Fire Truck", ItemType.Vehicles) { ModelItem = new PhysicalItem("firetruk") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Flatbed", ItemType.Vehicles) { ModelItem = new PhysicalItem("FLATBED") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Packer", ItemType.Vehicles) { ModelItem = new PhysicalItem("Packer") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Pounder", ItemType.Vehicles) { ModelItem = new PhysicalItem("Pounder") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Pounder Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("pounder2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Dune", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("rallytruck") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("MTL Wastelander", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("wastelander") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki BF400", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("bf400") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Blazer", ItemType.Vehicles) { ModelItem = new PhysicalItem("blazer") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Blazer Lifeguard", ItemType.Vehicles) { ModelItem = new PhysicalItem("blazer2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Hot Rod Blazer", ItemType.Vehicles) { ModelItem = new PhysicalItem("blazer3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Street Blazer", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("blazer4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Carbon RS", ItemType.Vehicles) { ModelItem = new PhysicalItem("carbonrs") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Chimera", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("chimera") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Outlaw", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("outlaw") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Shotaro", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("shotaro") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Stryder", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("Stryder") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Obey 8F Drafter", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("drafter") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Obey 9F", ItemType.Vehicles) { ModelItem = new PhysicalItem("ninef") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Obey 9F Cabrio", ItemType.Vehicles) { ModelItem = new PhysicalItem("ninef2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Obey Omnis", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("omnis") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Obey Rocoto", ItemType.Vehicles) { ModelItem = new PhysicalItem("rocoto") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Obey Tailgater", ItemType.Vehicles) { ModelItem = new PhysicalItem("tailgater") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Obey Tailgater S", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tailgater2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot Ardent", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("ardent") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot F620", ItemType.Vehicles) { ModelItem = new PhysicalItem("f620") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot R88", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("formula2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot Jackal", ItemType.Vehicles) { ModelItem = new PhysicalItem("jackal") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot Jugular", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("jugular") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot Locust", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("locust") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot Lynx", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("lynx") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot Pariah", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("pariah") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot Penetrator", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("penetrator") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot Swinger", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("swinger") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ocelot XA-21", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("xa21") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Overflod Autarch", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("autarch") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Overflod Entity XXR", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("entity2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Overflod Entity XF", ItemType.Vehicles) { ModelItem = new PhysicalItem("entityxf") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Overflod Imorgon", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("imorgon") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Overflod Tyrant", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tyrant") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Bati 801", ItemType.Vehicles) { ModelItem = new PhysicalItem("bati") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Bati 801RR", ItemType.Vehicles) { ModelItem = new PhysicalItem("bati2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Esskey", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("esskey") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Faggio Sport", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("faggio") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Faggio", ItemType.Vehicles) { ModelItem = new PhysicalItem("faggio2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Faggio Mod", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("faggio3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi FCR 1000", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("fcr") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi FCR 1000 Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("fcr2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Infernus", ItemType.Vehicles) { ModelItem = new PhysicalItem("infernus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Infernus Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("infernus2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Monroe", ItemType.Vehicles) { ModelItem = new PhysicalItem("monroe") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Oppressor", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("oppressor") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Oppressor Mk II", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("oppressor2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Osiris", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("osiris") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Reaper", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("reaper") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Ruffian", ItemType.Vehicles) { ModelItem = new PhysicalItem("ruffian") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Tempesta", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tempesta") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Tezeract", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tezeract") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Torero", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("torero") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Toros", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("toros") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Vacca", ItemType.Vehicles) { ModelItem = new PhysicalItem("vacca") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Vortex", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("vortex") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Zentorno", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zentorno") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Zorrusso", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zorrusso") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pfister Comet", "You always wanted one of these when in high school - and now you can have the car that tells everyone yes, these are implants - on your head and in that dizzy tart next to you. Boom. You go, tiger.", ItemType.Vehicles) { ModelItem = new PhysicalItem("comet2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pfister Comet Retro Custom", "For a whole generation of the San Andreas elite, this isn't just a car. From the onboard champagne cooler to the suede back seat where you pawed your first gold digger - The Pfister Comet was something that made you who you are. And now, thanks to Benny reinventing it as a gnarly, riveted urban dragster, it'll be broadcasting your escalating midlife crisis for years to come.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("comet3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pfister Comet Safari", "Is there nothing the Pfister Comet cannot do? If you were a venture capitalist looking for the shortest route to your next midlife crisis, the Comet was your first and only choice. If you wanted something that preserved the classic reek of desperation but added a street-racer twist, the Retro Custom was top of the list. And now, if you're looking for something to slam around a hairpin bend in three feet of uphill mud, the Comet Safari has got you covered.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("comet4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pfister Comet SR", "Forget everything you think you know about the Pfister Comet. Forget cruising through Vinewood with a bellyful of whiskey dropping one-liners about the size of your bonus. Forget picking up sex workers and passing them off as your fiancé at family gatherings. The SR was made for only one thing: to make every other sports car look like it's the asthmatic kid in gym. Now get in line.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("comet5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pfister Comet S2", "This isn't just a fast car. It's a car with the kind of reputation that no amount of targeted advertising can buy. So, when some people see a Comet they make a wish. Others run screaming for cover, prophesying doom, destruction, and crippling medical expenses. Either way, you made an impression.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("comet6") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pfister Growler","You prefer the book to the movie. You drink spirits neat. You describe your sense of humor as 'subtle' and your love making as 'imperceptible'. You're The Thinking Person. And you choose handling over speed, control over power, and principle over pleasure. You choose wisely. You choose the Pfister Growler.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("growler") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pfister Neon","When the history of the electric car is written, it will begin with the Pfister Neon. Everything else - all the ridiculous eco-vans and hybrid fetishes - has been foreplay. Now Pfister have dropped their pants, and the battery-powered action can really begin.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("neon") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pfister 811","Meet the future of hybrid tech: Pfister took billions of dollars in subsidies for low-carbon research and used it to refine an electric motor until it gives more kick than a turbo charger. And don't worry about accidentally investing in the environment: the assembly process alone produces enough CO2 to offset two thousand acres of otherwise useless rainforest. Win-win.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("pfister811") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Principe Deveste Eight","It began as little more than a myth: a list of impossible statistics circulating on the dark net. Then the myth became a legend: a few leaked photographs so provocative that possession was a federal crime. Then the legend became a rumor: a car so exclusive no one could confirm it existed in the real world. And now, thanks to you, that rumor is about to become a very messy headline.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("deveste") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Principe Diabolus","You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("diablous") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Principe Diabolus Custom","You need to be careful inviting a Diabolus into your life. Sure, it'll provide a swift injection of all the ruggedness and suavity you never had. But before long it'll be wearing your slippers, smoking your slimline cigarettes and conducting a torrid affair with your spouse. And when that happens, Benny has the only solution: a savage reworking into a tightly wound street racer, complete with docked handlebars and track ergonomics. You're welcome.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("diablous2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Principe Lectro","As if this new-school streetfighter didn’t look aggressive enough, once you hit that KERS button you’ll be locked into a death struggle with the laws of physics - and there can only be one winner.", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("lectro") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Principe Nemesis","Super fast, super unshielded. When you're riding a Nemesis, you don't just feel the wind in your hair, you feel it tearing into the back of your eye sockets.", ItemType.Vehicles) { ModelItem = new PhysicalItem("nemesis") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Progen Emerus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("emerus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Progen PR4", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("formula") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Progen GP1", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("gp1") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Progen Itali GTB", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("italigtb") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Progen Itali GTB Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("italigtb2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Progen T20", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("t20") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Progen Tyrus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tyrus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("RUNE Cheburek", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cheburek") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Schyster Deviant", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("deviant") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Schyster Fusilade", ItemType.Vehicles) { ModelItem = new PhysicalItem("fusilade") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Defiler", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("defiler") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Hakuchou", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("hakuchou") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Hakuchou Drag", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("hakuchou2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu PCJ 600", ItemType.Vehicles) { ModelItem = new PhysicalItem("pcj") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Vader", ItemType.Vehicles) { ModelItem = new PhysicalItem("Vader") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Stanley Fieldmaster", ItemType.Vehicles) { ModelItem = new PhysicalItem("tractor2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Stanley Fieldmaster", ItemType.Vehicles) { ModelItem = new PhysicalItem("tractor3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Truffade Adder", ItemType.Vehicles) { ModelItem = new PhysicalItem("adder") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Truffade Nero", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("nero") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Truffade Nero Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("nero2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Truffade Thrax", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("thrax") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Truffade Z-Type", ItemType.Vehicles) { ModelItem = new PhysicalItem("Ztype") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Oracle XS", ItemType.Vehicles) { ModelItem = new PhysicalItem("oracle") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Oracle", ItemType.Vehicles) { ModelItem = new PhysicalItem("oracle2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Revolter", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("revolter") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht SC1", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sc1") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Sentinel XS", ItemType.Vehicles) { ModelItem = new PhysicalItem("sentinel") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Sentinel 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("sentinel2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Sentinel 3", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sentinel3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Zion", ItemType.Vehicles) { ModelItem = new PhysicalItem("zion") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Zion Cabrio", ItemType.Vehicles) { ModelItem = new PhysicalItem("zion2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Zion Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zion3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Cypher", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cypher") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Ubermacht Rebla GTS", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("rebla") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Benson", ItemType.Vehicles) { ModelItem = new PhysicalItem("Benson") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Blade", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("blade") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Bobcat XL", ItemType.Vehicles) { ModelItem = new PhysicalItem("bobcatXL") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Bullet", ItemType.Vehicles) { ModelItem = new PhysicalItem("bullet") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Caracara", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("caracara") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Caracara 4x4", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("caracara2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Chino", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("chino") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Chino Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("chino2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Clique", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("clique") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Contender", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("contender") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Dominator", "Baby boomer teen dream repackaged for the mass market generation. A muscle car without the muscle that's the rental vehicle of choice for tourists looking for a slice of 'real' Americana.", ItemType.Vehicles) { ModelItem = new PhysicalItem("Dominator") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Pisswasser Dominator", ItemType.Vehicles) { ModelItem = new PhysicalItem("dominator2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Dominator GTX*", "Step one: take the best-looking muscle car the 60's ever saw, and introduce it to the greatest American supercar of the modern era. When your pedigree is this damn good, there's nothing wrong with keeping it in the family.", ItemType.Vehicles) { ModelItem = new PhysicalItem("dominator3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Apocalypse Dominator", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dominator4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Future Shock Dominator", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dominator5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Nightmare Dominator", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dominator6") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Dominator ASP", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dominator7") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Dominator GTT", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dominator8") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Ellie", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("ellie") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Flash GT", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("flashgt") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid FMJ", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("fmj") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid GB200", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("gb200") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Guardian", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("guardian") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Hotknife", ItemType.Vehicles) { ModelItem = new PhysicalItem("hotknife") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Hustler", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("hustler") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Apocalypse Imperator", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("imperator") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Future Shock Imperator", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("imperator2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Nightmare Imperator", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("imperator3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Minivan", ItemType.Vehicles) { ModelItem = new PhysicalItem("minivan") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Minivan Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("minivan2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Monster", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("monster") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Peyote", ItemType.Vehicles) { ModelItem = new PhysicalItem("peyote") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Peyote Gasser", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("peyote2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Peyote Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("peyote3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Radius", ItemType.Vehicles) { ModelItem = new PhysicalItem("radi") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Retinue", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("retinue") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Retinue Mk II", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("retinue2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Riata", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("riata") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Sadler", ItemType.Vehicles) { ModelItem = new PhysicalItem("Sadler") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Sadler 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("sadler2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Sandking XL", ItemType.Vehicles) { ModelItem = new PhysicalItem("sandking") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Sandking SWB", ItemType.Vehicles) { ModelItem = new PhysicalItem("sandking2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Slamtruck", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("slamtruck") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Slamvan", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("slamvan") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Lost Slamvan", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("slamvan2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Slamvan Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("slamvan3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Apocalypse Slamvan", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("slamvan4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Future Shock Slamvan", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("slamvan5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Nightmare Slamvan", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("slamvan6") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Speedo", ItemType.Vehicles) { ModelItem = new PhysicalItem("speedo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Clown Van", ItemType.Vehicles) { ModelItem = new PhysicalItem("speedo2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Speedo Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("speedo4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Stanier", "If you took a cab or got arrested in the 1990s, there's a high chance you ended up in the back of a Vapid Stanier. Discontinued following widespread reports of fuel tanks exploding on impact in rear-end collisions. So try to avoid that.", ItemType.Vehicles) { ModelItem = new PhysicalItem("stanier") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Trophy Truck", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("trophytruck") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Desert Raid", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("trophytruck2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vapid Winky", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("winky") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vulcar Fagaloa", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("fagaloa") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vulcar Ingot", ItemType.Vehicles) { ModelItem = new PhysicalItem("ingot") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vulcar Nebula Turbo", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("nebula") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vulcar Warrener", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("warrener") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vulcar Warrener HKR", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("warrener2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Vysser Neo", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("neo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Weeny Dynasty", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("Dynasty") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Weeny Issi", ItemType.Vehicles) { ModelItem = new PhysicalItem("issi2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Weeny Issi Classic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("issi3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Weeny Apocalypse Issi", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("issi4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Weeny Future Shock Issi", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("issi5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Weeny Nightmare Issi", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("issi6") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Weeny Issi Sport", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("issi7") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Bagger", ItemType.Vehicles) { ModelItem = new PhysicalItem("bagger") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Cliffhanger", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("cliffhanger") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Daemon", ItemType.Vehicles) { ModelItem = new PhysicalItem("daemon") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Daemon 2", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("daemon2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Apocalypse Deathbike", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("deathbike") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Future Shock Deathbike", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("deathbike2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Nightmare Deathbike", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("deathbike3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Gargoyle", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("gargoyle") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Nightblade", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("nightblade") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Rat Bike", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("ratbike") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Rampant Rocket", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("rrocket") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Sovereign", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("sovereign") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Wolfsbane", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("wolfsbane") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Zombie Bobber", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zombiea") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Zombie Chopper", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("zombieb") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Willard Faction", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("faction") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Willard Faction Custom", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("faction2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Willard Faction Custom Donk", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("faction3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Zirconium Journey", ItemType.Vehicles) { ModelItem = new PhysicalItem("journey") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Zirconium Stratum", ItemType.Vehicles) { ModelItem = new PhysicalItem("stratum") { Type = ePhysicalItemType.Vehicle } },

            //Heli
            new ModItem("Buckingham SuperVolito", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("supervolito") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham SuperVolito Carbon", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("supervolito2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Swift", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("swift") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Swift Deluxe", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("swift2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Volatus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("volatus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Thruster", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("thruster") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Havok", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("havok") { Type = ePhysicalItemType.Vehicle } },

            //Plane
            new ModItem("Buckingham Alpha-Z1", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("alphaz1") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Howard NX-25", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("howard") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Luxor", ItemType.Vehicles) { ModelItem = new PhysicalItem("luxor") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Luxor Deluxe", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("luxor2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Miljet", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("Miljet") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Nimbus", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("nimbus") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Pyro", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("pyro") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Shamal", ItemType.Vehicles) { ModelItem = new PhysicalItem("Shamal") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Buckingham Vestra", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("vestra") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Avenger", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("avenger") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Avenger 2", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("avenger2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Dodo", ItemType.Vehicles) { ModelItem = new PhysicalItem("dodo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Hydra", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("hydra") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Mogul", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("mogul") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Mammoth Tula", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tula") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Ultralight", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("microlight") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Besra", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("besra") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Rogue", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("rogue") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Western Seabreeze", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("seabreeze") { Type = ePhysicalItemType.Vehicle } },

            //Boat
            new ModItem("Dinka Marquis", ItemType.Vehicles) { ModelItem = new PhysicalItem("marquis") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Toro", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("toro") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Lampadati Toro", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("toro2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Dinghy", ItemType.Vehicles) { ModelItem = new PhysicalItem("Dinghy") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Dinghy 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("dinghy2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Dinghy 3", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dinghy3") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Dinghy 4", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dinghy4") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Nagasaki Weaponized Dinghy", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("dinghy5") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Speeder", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("speeder") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Pegassi Speeder", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("speeder2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Jetmax", ItemType.Vehicles) { ModelItem = new PhysicalItem("jetmax") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Longfin", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("longfin") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Squalo", ItemType.Vehicles) { ModelItem = new PhysicalItem("squalo") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Suntrap", ItemType.Vehicles) { ModelItem = new PhysicalItem("Suntrap") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Tropic", ItemType.Vehicles) { ModelItem = new PhysicalItem("tropic") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Shitzu Tropic", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("tropic2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Speedophile Seashark", ItemType.Vehicles) { ModelItem = new PhysicalItem("seashark") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Speedophile Seashark 2", ItemType.Vehicles) { ModelItem = new PhysicalItem("seashark2") { Type = ePhysicalItemType.Vehicle } },
            new ModItem("Speedophile Seashark 3", true, ItemType.Vehicles) { ModelItem = new PhysicalItem("seashark3") { Type = ePhysicalItemType.Vehicle } },
        });
    }
    private void DefaultConfig_Weapons()
    {
        ModItemsList.AddRange(new List<ModItem>
        {
            //Melee
            new ModItem("Baseball Bat","Aluminum baseball bat with leather grip. Lightweight yet powerful for all you big hitters out there.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_bat",0x958A4A8F) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Crowbar","Heavy-duty crowbar forged from high quality, tempered steel for that extra leverage you need to get the job done.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_crowbar",0x84BD7BFD) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Golf Club","Standard length, mid iron golf club with rubber grip for a lethal short game.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_golfclub",0x440E4788) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hammer","A robust, multi-purpose hammer with wooden handle and curved claw, this old classic still nails the competition.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_hammer",0x4E875F73) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hatchet","Add a good old-fashioned hatchet to your armory, and always have a back up for when ammo is hard to come by.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_hatchet",0xF9DCBF2D) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Brass Knuckles","Perfect for knocking out gold teeth, or as a gift to the trophy partner who has everything.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_knuckle",0xD8DF3C3C) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Combat Knife","This carbon steel 7 inch bladed knife is dual edged with a serrated spine to provide improved stabbing and thrusting capabilities.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_knife",0x99B507EA) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Machete","America's West African arms trade isn't just about giving. Rediscover the simple life with this rusty cleaver.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_machete",0xDD5DF8D9) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Switchblade","From your pocket to hilt-deep in the other guy's ribs in under a second: folding knives will never go out of style.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_switchblade",0xDFE37640) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Nightstick","24 inch polycarbonate side-handled nightstick.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_nightstick",0x678B81B1) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Wrench","Perennial favourite of apocalyptic survivalists and violent fathers the world over, apparently it also doubles as some kind of tool.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_wrench",0x19044EE0) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Pool Cue","Ah, there's no sound as satisfying as the crack of a perfect break, especially when it's the other guy's spine.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_poolcue",0x94117305) { Type = ePhysicalItemType.Weapon }},

            //Pistola
            new ModItem("Hawk & Little PTF092F","Standard handgun. A 9mm combat pistol with a magazine capacity of 12 rounds that can be extended to 16.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_pistol",0x1B06D571) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little Thunder","Balance, simplicity, precision: nothing keeps the peace like an extended barrel in the other guy's mouth.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_pistol_mk2",0xBFE256D4) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little Combat Pistol","A compact, lightweight semi-automatic pistol designed for law enforcement and personal defense use. 12-round magazine with option to extend to 16 rounds.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_combatpistol",0x5EF9FEC4) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little Desert Slug","High-impact pistol that delivers immense power but with extremely strong recoil. Holds 9 rounds in magazine.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_pistol50",0x99AEEB3B) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer P69","Not your grandma's ceramics. Although this pint-sized pistol is small enough to fit into her purse and won't set off a metal detector.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_ceramicpistol",0x2B5EF5EC) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer SCRAMP","High-penetration, fully-automatic pistol. Holds 18 rounds in magazine with option to extend to 36 rounds.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_appistol",0x22D8FE39) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little 1919","The heavyweight champion of the magazine fed, semi-automatic handgun world. Delivers accuracy and a serious forearm workout every time.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_heavypistol",0xD205520E) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little Raging Mare","A handgun with enough stopping power to drop a crazed rhino, and heavy enough to beat it to death if you're out of ammo.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_revolver",0xC1B3C3D1) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little Raging Mare Dx","If you can lift it, this is the closest you'll get to shooting someone with a freight train.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_revolver_mk2",0xCB96392F) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Shrewsbury S7","Like condoms or hairspray, this fits in your pocket for a night on the town. The price of a bottle at a club, it's half as accurate as a champagne cork, and twice as deadly.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_snspistol",0xBFD21232) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Shrewsbury S7A","The ultimate purse-filler: if you want to make Saturday Night really special, this is your ticket.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_snspistol_mk2",0x88374054) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Coil Tesla","Fires a projectile that administers a voltage capable of temporarily stunning an assailant. It's like, literally stunning.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_stungun",0x45CD9CF3) { Type = ePhysicalItemType.Weapon }},
            new ModItem("BS M1922","What you really need is a more recognisable gun. Stand out from the crowd at an armed robbery with this engraved pistol.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_vintagepistol",0x83839C4) { Type = ePhysicalItemType.Weapon }},

            //Shotgun
            new ModItem("Shrewsbury 420 Sawed-Off","This single-barrel, sawed-off shotgun compensates for its low range and ammo capacity with devastating efficiency in close combat.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_sawnoffshotgun",0x7846A318) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Shrewsbury 420","Standard shotgun ideal for short-range combat. A high-projectile spread makes up for its lower accuracy at long range.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_pumpshotgun",0x1D073A89) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer 569","Only one thing pumps more action than a pump action: watch out, the recoil is almost as deadly as the shot.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_pumpshotgun_mk2",0x555AF99A) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer IBS-12","Fully automatic shotgun with 8 round magazine and high rate of fire.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_assaultshotgun",0xE284C527) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little HLSG","More than makes up for its slow, pump-action rate of fire with its range and spread. Decimates anything in its projectile path.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_bullpupshotgun",0x9D61E50F) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Shrewsbury Taiga-12","The weapon to reach for when you absolutely need to make a horrible mess of the room. Best used near easy-wipe surfaces only.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_heavyshotgun",0x3AABBBAA) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Toto 12 Guage Sawed-Off","Do one thing, do it well. Who needs a high rate of fire when your first shot turns the other guy into a fine mist?.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_dbshotgun",0xEF951FBB) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Shrewsbury Defender","How many effective tools for riot control can you tuck into your pants? Ok, two. But this is the other one.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_autoshotgun",0x12E82D3D) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Leotardo SPAZ-11","There's only one semi-automatic shotgun with a fire rate that sets the LSFD alarm bells ringing, and you're looking at it.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_combatshotgun",0x5A96BA4) { Type = ePhysicalItemType.Weapon }},

            //SMG
            new ModItem("Shrewsbury Luzi","Combines compact design with a high rate of fire at approximately 700-900 rounds per minute.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_microsmg",0x13532244) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little MP6","This is known as a good all-around submachine gun. Lightweight with an accurate sight and 30-round magazine capacity.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_smg",0x2BE6766B) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little XPM","Lightweight, compact, with a rate of fire to die very messily for: turn any confined space into a kill box at the click of a well-oiled trigger.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_smg_mk2",0x78A97CD0) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer Fisher","A high-capacity submachine gun that is both compact and lightweight. Holds up to 30 bullets in one magazine.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_assaultsmg",0xEFE7E2DF) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Coil PXM","Who said personal weaponry couldn't be worthy of military personnel? Thanks to our lobbyists, not Congress. Integral suppressor.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_combatpdw",0x0A3D4D34) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer KEK-9","This fully automatic is the snare drum to your twin-engine V8 bass: no drive-by sounds quite right without it.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_machinepistol",0xDB1AA450) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little Millipede","Increasingly popular since the marketing team looked beyond spec ops units and started caring about the little guys in low income areas.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_minismg",0xBD248B55) { Type = ePhysicalItemType.Weapon }},

            //AR
            new ModItem("Shrewsbury A7-4K","This standard assault rifle boasts a large capacity magazine and long distance accuracy.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_assaultrifle",0xBFEFFF6D) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Shrewsbury A2-1K","The definitive revision of an all-time classic: all it takes is a little work, and looks can kill after all.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_assaultrifle_mk2",0x394F415C) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer A5-1R","Combining long distance accuracy with a high capacity magazine, the Carbine Rifle can be relied on to make the hit.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_carbinerifle",0x83BF0278) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer A5-1R MK2","This is bespoke, artisan firepower: you couldn't deliver a hail of bullets with more love and care if you inserted them by hand.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_carbinerifle_mk2",0xFAD1F1C9) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer BFR","The most lightweight and compact of all assault rifles, without compromising accuracy and rate of fire.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_advancedrifle",0xAF113F99) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer SL6","Combining accuracy, maneuverability, firepower and low recoil, this is an extremely versatile assault rifle for any combat situation.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_specialcarbine",0xC0A3098D) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer SL6 MK2","The jack of all trades just got a serious upgrade: bow to the master.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_specialcarbine_mk2",0x969C3D67) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little ZBZ-23","The latest Chinese import taking America by storm, this rifle is known for its balanced handling. Lightweight and very controllable in automatic fire.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_bullpuprifle",0x7F229F94) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little ZBZ-25X","So precise, so exquisite, it's not so much a hail of bullets as a symphony.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_bullpuprifle_mk2",0x84D6FAFD) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Shrewsbury Stinkov","Half the size, all the power, double the recoil: there's no riskier way to say 'I'm compensating for something'.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_compactrifle",0x624FE830) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer GUH-B4","This immensely powerful assault rifle was designed for highly qualified, exceptionally skilled soldiers. Yes, you can buy it.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_militaryrifle",0x9D1F17E6) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer POCK","The no-holds barred 30-round answer to that eternal question: how do I get this guy off my back?", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_heavyrifle",0xC78D71B4) { Type = ePhysicalItemType.Weapon }},

            //LMG
            new ModItem("Shrewsbury PDA","General purpose machine gun that combines rugged design with dependable performance. Long range penetrative power. Very effective against large groups.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_mg",0x9D07F764) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer BAT","Lightweight, compact machine gun that combines excellent maneuverability with a high rate of fire to devastating effect.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_combatmg",0x7FD62962) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer M70E1","You can never have too much of a good thing: after all, if the first shot counts, then the next hundred or so must count for double.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_combatmg_mk2",0xDBBD7280) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little Kenan","Complete your look with a Prohibition gun. Looks great being fired from an Albany Roosevelt or paired with a pinstripe suit.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_gusenberg",0x61012683) { Type = ePhysicalItemType.Weapon }},

            //SNIPER
            new ModItem("Shrewsbury PWN","Standard sniper rifle. Ideal for situations that require accuracy at long range. Limitations include slow reload speed and very low rate of fire.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_sniperrifle",0x05FC3C11) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Bartlett M92","Features armor-piercing rounds for heavy damage. Comes with laser scope as standard.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_heavysniper",0x0C472FE2) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Bartlett M92 Mk2","Far away, yet always intimate: if you're looking for a secure foundation for that long-distance relationship, this is it.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_heavysniper_mk2",0xA914799) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer M23 DBS","Whether you're up close or a disconcertingly long way away, this weapon will get the job done. A multi-range tool for tools.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_marksmanrifle",0xC734385A) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Vom Feuer M23 DBS Scout","Known in military circles as The Dislocator, this mod set will destroy both the target and your shoulder, in that order.", true, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_marksmanrifle_mk2",0x6A6C02E0) { Type = ePhysicalItemType.Weapon }},

            //OTHER
            new ModItem("RPG-7","A portable, shoulder-launched, anti-tank weapon that fires explosive warheads. Very effective for taking down vehicles or large groups of assailants.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_rpg",0xB1CA77B1) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Hawk & Little MGL","A compact, lightweight grenade launcher with semi-automatic functionality. Holds up to 10 rounds.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_grenadelauncher",0xA284510B) { Type = ePhysicalItemType.Weapon }},
            new ModItem("M61 Grenade","Standard fragmentation grenade. Pull pin, throw, then find cover. Ideal for eliminating clustered assailants.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_grenade",0x93E220BD) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Improvised Incendiary","Crude yet highly effective incendiary weapon. No happy hour with this cocktail.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_molotov",0x24B17070) { Type = ePhysicalItemType.Weapon }},
            new ModItem("BZ Gas Grenade","BZ gas grenade, particularly effective at incapacitating multiple assailants.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_bzgas",0xA0973D5E) { Type = ePhysicalItemType.Weapon }},
            new ModItem("Tear Gas Grenade","Tear gas grenade, particularly effective at incapacitating multiple assailants. Sustained exposure can be lethal.", false, ItemType.Weapons) { ModelItem = new PhysicalItem("weapon_smokegrenade",0xBFE256D4) { Type = ePhysicalItemType.Weapon }},
        });
    }
}