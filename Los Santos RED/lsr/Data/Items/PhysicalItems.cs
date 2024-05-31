using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class PhysicalItems : IPropItems
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\PhysicalItems.xml";
    private List<PhysicalItem> PhysicalItemsList;

    public PhysicalItems()
    {

    }

    public List<PhysicalItem> Items => PhysicalItemsList;
    public PhysicalItem Get(string ID)
    {
        return PhysicalItemsList.FirstOrDefault(x => x.ID == ID);
    }
    public PhysicalItem GetRandomItem()
    {
        return PhysicalItemsList.Where(x => x.Type != ePhysicalItemType.Vehicle && x.Type != ePhysicalItemType.Weapon && x.Type != ePhysicalItemType.Ped).PickRandom();
    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("PhysicalItems*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Physical Items config: {ConfigFile.FullName}", 0);
            PhysicalItemsList = Serialization.DeserializeParams<PhysicalItem>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Physical Items config  {ConfigFileName}", 0);
            PhysicalItemsList = Serialization.DeserializeParams<PhysicalItem>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Physical Items config found, creating default", 0);
            DefaultConfig();
        }
    }
    private void DefaultConfig()
    {
        PhysicalItemsList = new List<PhysicalItem> { };
        DefaultConfig_Drinks();
        DefaultConfig_Food();
        DefaultConfig_Drugs();
        DefaultConfig_Tools();
        DefaultConfig_ArmorHealth();
        DefaultConfig_FEE();
        Serialization.SerializeParams(PhysicalItemsList, ConfigFileName);
    }

    private void DefaultConfig_FEE()
    {
        PhysicalItemsList.AddRange(new List<PhysicalItem> 
        { 
            //Drinks
            new PhysicalItem("prop_pisswassercan_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(-0.02f, -0.08999999f, -0.03f),new Rotator(-70f, 10f, 0f)), }),
            new PhysicalItem("prop_pisswassercan_01b", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(-0.02f, -0.08999999f, -0.03f),new Rotator(-70f, 10f, 0f)), }),
            new PhysicalItem("prop_ecolacan_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(-0.02f, -0.08999999f, -0.03f),new Rotator(-70f, 10f, 0f)), }),
            new PhysicalItem("prop_sprunkcan_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(-0.02f, -0.08999999f, -0.03f),new Rotator(-70f, 10f, 0f)), }),
            //Food
            new PhysicalItem("prop_strawberryrailsbox_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, -0.0f),new Rotator(0f, 0f, 0f)), }),
        });
    }

    private void DefaultConfig_ArmorHealth()
    {
        PhysicalItemsList.AddRange(new List<PhysicalItem> {

            new PhysicalItem("prop_armour_pickup", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }){ IsLarge = true },
            new PhysicalItem("prop_bodyarmour_02", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }){ IsLarge = true },
            new PhysicalItem("prop_bodyarmour_03", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }){ IsLarge = true },
            new PhysicalItem("prop_bodyarmour_04", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }){ IsLarge = true },
            new PhysicalItem("prop_bodyarmour_05", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }){ IsLarge = true },
            new PhysicalItem("prop_bodyarmour_06", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }){ IsLarge = true },
            new PhysicalItem("prop_ld_health_pack", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }),
            //prop_ld_health_pack
        });
        //prop_armour_pickup
    }

    private void DefaultConfig_Drinks()
    {
        PhysicalItemsList.AddRange(new List<PhysicalItem> {
            //Drinks
            //Bottles
            new PhysicalItem("ba_prop_club_water_bottle", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.05f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(-0.01f, -0.11f, -0.05999999f),new Rotator(-50f, 0f, 0f)), }),
            new PhysicalItem("h4_prop_battle_waterbottle_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.05f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, -0.01f, 0.03f),new Rotator(-60f, 20f, 0f)),}) { CleanupItemImmediately = true },

            new PhysicalItem("prop_energy_drink", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, 0.02f, 0.03f),new Rotator(-80f, 20f, 0f)), }),


            new PhysicalItem("prop_ld_flow_bottle", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, 0.03f, 0.02f),new Rotator(-70f, 20f, 0f)), }),

            new PhysicalItem("prop_amb_beer_bottle", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)), }),
            new PhysicalItem("prop_beer_am", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },
            new PhysicalItem("prop_beer_bar", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },
            new PhysicalItem("prop_beer_blr", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },
            new PhysicalItem("prop_beer_jakey", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },
            new PhysicalItem("prop_beer_logger", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },
            new PhysicalItem("prop_beer_patriot", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },





            new PhysicalItem("prop_beer_pride", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },
            new PhysicalItem("prop_beer_stz", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },
            new PhysicalItem("prop_beerdusche", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.15f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.05f, 0.03f),new Rotator(-70f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.05f, -0.03f, -0.05f),new Rotator(-70f, 20f, 0f)),}) { CleanupItemImmediately = true },

            new PhysicalItem("prop_cs_beer_bot_40oz", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.05f), new Rotator(0.0f, 0.0f, 0.0f)) }),

            new PhysicalItem("h4_prop_h4_t_bottle_02a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }){ CleanupItemImmediately = true },
            new PhysicalItem("h4_prop_h4_t_bottle_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }){ CleanupItemImmediately = true },

            new PhysicalItem("ng_proc_sodacan_01a", new List<PropAttachment>() 
            { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),
                //new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(-0.02f, -0.08999999f, -0.03f),new Rotator(-70f, 10f, 0f)),


                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, -0.06999999f, -0.01f),new Rotator(-80f, 0f, 0f)),//new

                new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.11f, -0.05999999f, -0.05999999f),new Rotator(-80f, 0f, 0f)),



            }),











            new PhysicalItem("ng_proc_sodacan_01b", new List<PropAttachment>() 
            { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(-0.02f, -0.08999999f, -0.03f),new Rotator(-70f, 10f, 0f)),
                new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.11f, -0.05999999f, -0.05999999f),new Rotator(-80f, 0f, 0f)),
            }),
            new PhysicalItem("prop_orang_can_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.01f, 0f, 0.02f),new Rotator(-80f, 20f, 0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.09999999f, 0.02f, -0.04f),new Rotator(-80f, 10f, 0f)), }),

            new PhysicalItem("h4_prop_h4_can_beer_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.1f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("RightHandSteal", "BONETAG_R_PH_HAND", new Vector3(0.09999999f, -0.04f, -0.05f),new Rotator(-70f, 10f, 0f)), }),

            new PhysicalItem("p_ing_coffeecup_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }){ CleanupItemImmediately = true },
            new PhysicalItem("p_ing_coffeecup_02", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),

            new PhysicalItem("ng_proc_sodacup_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.2f), new Rotator(0.0f, 0.0f, 0.0f)) }){ CleanupItemImmediately = true },
            new PhysicalItem("ng_proc_sodacup_01b", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.2f), new Rotator(0.0f, 0.0f, 0.0f)) }){ CleanupItemImmediately = true },
            new PhysicalItem("ng_proc_sodacup_01c", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, -0.2f), new Rotator(0.0f, 0.0f, 0.0f)) }),



             new PhysicalItem("prop_cs_milk_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             

             new PhysicalItem("ba_prop_battle_whiskey_bottle_s", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("ba_prop_battle_whiskey_bottle_2_s", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),

             new PhysicalItem("prop_bottle_brandy", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_bottle_cognac", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_bottle_macbeth", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_bottle_richard", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),

             new PhysicalItem("prop_rum_bottle", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_tequila_bottle", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_vodka_bottle", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_whiskey_bottle", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),


             new PhysicalItem("prop_champ_01a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),

             new PhysicalItem("prop_cherenkov_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_cherenkov_02", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_cherenkov_03", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
             new PhysicalItem("prop_cherenkov_04", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),

             //prop_bottle_brandy.yft
             //ba_prop_battle_whiskey_bottle_s.ydr
             //prop_cs_whiskey_bottle

        });

        //
    }
    private void DefaultConfig_Drugs()
    {
        PhysicalItemsList.AddRange(new List<PhysicalItem>
        {
            //Cigarettes/Cigars
            new PhysicalItem("ng_proc_cigarette01a", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0f, 0f)),
                new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.007f, 0.13f, 0.01f),new Rotator(0.0f, -175f, 91f)) {Gender = "M" },
                new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.02f, 0.1f, 0.01f),new Rotator(0f, 0f, -80f)) {Gender = "F" },}),//looks good



                //Packages
                new PhysicalItem("v_ret_ml_cigs", new List<PropAttachment>() { new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0f, 0.02f),new Rotator(-70f, 20f, 0f)), }),
                new PhysicalItem("v_ret_ml_cigs2", new List<PropAttachment>() { new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0f, 0.02f),new Rotator(-70f, 20f, 0f)), }),
                new PhysicalItem("v_ret_ml_cigs3", new List<PropAttachment>() { new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0f, 0.02f),new Rotator(-70f, 20f, 0f)), }),
                new PhysicalItem("v_ret_ml_cigs4", new List<PropAttachment>() { new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0f, 0.02f),new Rotator(-70f, 20f, 0f)), }),
                new PhysicalItem("v_ret_ml_cigs5", new List<PropAttachment>() { new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0f, 0.02f),new Rotator(-70f, 20f, 0f)), }),
                new PhysicalItem("v_ret_ml_cigs6", new List<PropAttachment>() { new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0f, 0.02f),new Rotator(-70f, 20f, 0f)), }),
                new PhysicalItem("p_cigar_pack_02_s", new List<PropAttachment>() { new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.01f, 0.01f, 0.03f),new Rotator(-60f, 40f, 0f)), }),



            new PhysicalItem("prop_cigar_02", new List<PropAttachment>() { 
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(-0.02f, 0.0f, 0.0f), new Rotator(0.0f, 180f, 0f)) {Gender = "M" },
                //new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.007f, 0.13f, 0.01f),new Rotator(0.0f, -175f, 91f)) {Gender = "M" },//doesnt look so good on franklin
                new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.023f,0.087f,0.014f), new Rotator(50f, 0f, 90f)) { Gender = "M" },//a little close in for franklin
                ////OLD
                //new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(-0.015f,0.117f,0.01f),new Rotator(90f, 90f, 0f)) { Gender = "F" },
                //new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.023f,0.087f,0.014f), new Rotator(50f, 0f, 90f)) { Gender = "F" },
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.01f,0.01f,0.01f),new Rotator(0f, 0f, -180f)) { Gender = "F",IsMP = true },
                new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.01f,0.1f,-0.01f), new Rotator(0f, 0f, 90f)) { Gender = "F",IsMP = true },
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(-0.01f,0.01f,-0.01f),new Rotator(0f, 0f, -180f)) { Gender = "F",IsMP = false },
                new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.02f,0.08f,-0.01f), new Rotator(0f, 0f, 90f)) { Gender = "F",IsMP = false },
                new PropAttachment("Particle", "", new Vector3(0.072f,0.0f,0.0f), new Rotator(0f, 0f, 0f)) { Gender = "U" },
            }),//looksgood besides player mouth attach       
            //Other Drugs
            new PhysicalItem("p_cs_joint_01", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0f, 0f)),
                new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.007f, 0.13f, 0.01f),new Rotator(0.0f, -175f, 91f)) {Gender = "M" },
                new PropAttachment("Head", "BONETAG_HEAD", new Vector3(-0.02f, 0.1f, 0.01f),new Rotator(0f, 0f, -80f)) {Gender = "F" },
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),
            }),//looks good
            new PhysicalItem("prop_weed_bottle", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, 0f, 0.03f),new Rotator(-60f, 0f, 0f)),
            }),
            new PhysicalItem("prop_cs_pills", new List<PropAttachment>() { 
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0f, 0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.02f, 0.04f),new Rotator(-60.0f, 0f, 0f)),
            }),
            new PhysicalItem("sf_prop_sf_bag_weed_01a", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.06f, 0.02f, 0.02f),new Rotator(30.0f, 20f, 0f)),
            }),
            new PhysicalItem("ba_prop_battle_sniffing_pipe", new List<PropAttachment>() { 
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),
            }),
            new PhysicalItem("prop_meth_bag_01", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.01f, 0.04f, 0.05f),new Rotator(-60f, 0f, 0f)),
            }),
            new PhysicalItem("prop_cs_crackpipe", new List<PropAttachment>() { 
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, 0f, 0f),new Rotator(60f, 200f, 0f)),
            }),
            new PhysicalItem("prop_syringe_01", new List<PropAttachment>() { 
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.02f, 0.02f, 0.05f),new Rotator(-60f, 20f, 0f)),
            }),//inject
            new PhysicalItem("prop_cs_meth_pipe", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, 0.01f, 0.03f),new Rotator(50f, 180f, -10f)),
            }),//Doesnt attach right

            //prop_cs_script_bottle_01.ydr
            new PhysicalItem("prop_cs_script_bottle_01", new List<PropAttachment>() { 
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0f, 0f)),
                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, -0.02f, 0.01f),new Rotator(-60f, 0f, 0f)),
            }),
        });
    }
    private void DefaultConfig_Food()
    {
        //all of this is no longer attached right for some reason....
        PhysicalItemsList.AddRange(new List<PhysicalItem>
        {
            //Generic Food
            new PhysicalItem("prop_cs_hotdog_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(178.0f, 28.0f, 0.0f)) }),
            new PhysicalItem("prop_cs_burger_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 28.0f, 0.0f)) }),
            new PhysicalItem("prop_donut_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-15.0f, 17.0f, 0.0f)) }),
            new PhysicalItem("p_amb_bagel_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-15.0f, 17.0f, 0.0f)) }),
            new PhysicalItem("prop_food_chips", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("ng_proc_food_nana1a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("ng_proc_food_ornge1a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("ng_proc_food_aple1a", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("prop_sandwich_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(178.0f, 28.0f, 0.0f)) }),
            new PhysicalItem("v_res_tt_pizzaplate", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("prop_pizza_box_02", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("prop_pizza_box_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("v_ret_ml_chips1", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("v_ret_ml_chips2", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("v_ret_ml_chips3", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("v_ret_ml_chips4", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("prop_choc_ego", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(25f, -11f, -95f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.03f, 0.07f, 0.05f),new Rotator(-40f, 20f, -100f)), }),
            new PhysicalItem("prop_candy_pqs", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-178f, -169f, 169f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.05f, 0.04f, 0.03f),new Rotator(30f, 20f, -20f)), }),
            new PhysicalItem("prop_choc_pq", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-178f, -169f, 79f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0f, 0f, 0.02f),new Rotator(-10f, 30f, 250f)), }),
            new PhysicalItem("prop_choc_meto", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(169f, 170f, 76f)),new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.03f, 0.04f, 0.04f),new Rotator(-10f, 20f, -120f)), }),
            new PhysicalItem("prop_food_bs_burg1", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("prop_food_bs_tray_02", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }),
            new PhysicalItem("prop_food_bs_burger2", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("prop_food_bs_tray_03", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)) }),
            new PhysicalItem("prop_food_bs_tray_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
            new PhysicalItem("prop_food_bs_chips", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(-77.0f, 23.0f, 0.0f)) }),
            new PhysicalItem("prop_food_bag1", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
            new PhysicalItem("prop_food_bs_burger2", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(178.0f, 28.0f, 0.0f)) }),
            new PhysicalItem("prop_food_cb_tray_03", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
            new PhysicalItem("prop_food_cb_tray_02", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
            new PhysicalItem("prop_food_burg3", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(178.0f, 28.0f, 0.0f)) }),
            new PhysicalItem("prop_food_burg2", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)) }),
            new PhysicalItem("prop_food_burg1", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 28.0f, 0.0f)) }),
            new PhysicalItem("prop_ff_noodle_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),
            new PhysicalItem("prop_ff_noodle_02", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),

            new PhysicalItem("prop_taco_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),

        });
    }
    private void DefaultConfig_Tools()
    {
        PhysicalItemsList.AddRange(new List<PhysicalItem>
        {

            new PhysicalItem("prop_cash_pile_02", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)),
                //new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),


                new PropAttachment("RightHandPass", "BONETAG_R_PH_HAND", new Vector3(0.09f, 0.01f, -0.01f),new Rotator(10f, 0f, 0f)),



                new PropAttachment("LeftHandPass", "BONETAG_L_PH_HAND", new Vector3(0.1f, 0f, 0.03f),new Rotator(-10f, -50f, 0f)),
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.1f, 0f, 0.03f),new Rotator(-10f, -50f, 0f)),
                //new PropAttachment("LeftHandPass", "BONETAG_L_PH_HAND", new Vector3(0.06f, 0f, 0.02f),new Rotator(0f, 0f, 0f)),
                //new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.06f, 0f, 0.02f),new Rotator(0f, 0f, 0f)),

            }),



            new PhysicalItem("prop_cs_hand_radio", new List<PropAttachment>() { 
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)),
                new PropAttachment("Belt", "BONETAG_PELVIS", new Vector3(0f,0f,0.21f),new Rotator(-90f, -90f, 0f)),

            }),

            new PhysicalItem("prop_bong_01", new List<PropAttachment>() { new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0.0f, 0.0f, 0.0f)),new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0.0f, 0.0f, 0.0f)) }),

            new PhysicalItem("p_cs_papers_03", new List<PropAttachment>() { }),



            new PhysicalItem("p_cs_lighter_01", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.04f,0f,0f),new Rotator(-90f, 10f, 0f)),
                new PropAttachment("Flames", "", new Vector3(0.0f, 0.0f, 0.05f), new Rotator(0f, 0f, 0f))
            }),
            new PhysicalItem("v_res_tt_lighter", new List<PropAttachment>() { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.04f,0f,0f),new Rotator(-90f, 10f, 0f)),
                new PropAttachment("Flames", "", new Vector3(0.0f, 0.0f, 0.05f), new Rotator(0f, 0f, 0f))
            }),
            new PhysicalItem("ex_prop_exec_lighter_01", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.04f,0f,0f),new Rotator(-90f, 10f, 0f)),
                new PropAttachment("Flames", "", new Vector3(0.0f, 0.0f, 0.05f), new Rotator(0f, 0f, 0f))
            }),
            new PhysicalItem("lux_prop_lighter_luxe", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.04f,0f,0f),new Rotator(-90f, 10f, 0f)),
                new PropAttachment("Flames", "", new Vector3(0.0f, 0.0f, 0.05f), new Rotator(0f, 0f, 0f))
            }),

            new PhysicalItem("p_amb_brolly_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(-0.01f, 0.01f, 0.05f), new Rotator(0f, -40f, 0f)) }) { IsLarge = true },//blue umbrella
            new PhysicalItem("p_amb_brolly_01_s", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(-0.01f, 0.01f, 0.05f), new Rotator(0f, -40f, 0f)) }) { IsLarge = true },//black umbrellal
            new PhysicalItem("gr_prop_gr_tape_01", new List<PropAttachment>() { new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)) }),//flint duct tape

            new PhysicalItem("prop_tool_pliers", new List<PropAttachment>() { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
                new PropAttachment("RightHandWeapon","BONETAG_R_PH_HAND",new Vector3(0.06f,0.07f,0.03f),new Rotator(170f, 260f, -90f)),
            }){ AliasWeaponHash = 1317494643 },

            new PhysicalItem("prop_tool_shovel", new List<PropAttachment>() { 
                
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.005f, 0.006f, -0.048f), new Rotator(3f, -183f, 0f))
                ,new PropAttachment("RightHandWeapon", "BONETAG_R_PH_HAND", new Vector3(-0.03f,-0.277f,-0.062f),new Rotator(20f, -101f, 81f)) 
            }) { AliasWeaponHash = 2508868239, IsLarge = true },





            new PhysicalItem("prop_tool_screwdvr01", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
                new PropAttachment("RightHandWeapon","BONETAG_R_PH_HAND",new Vector3(0.04f,0.08f,0.01f),new Rotator(100f, 0f, 0f)),
            }) { AliasWeaponHash = 2578778090 },//generic screwdriver
            new PhysicalItem("gr_prop_gr_sdriver_01", new List<PropAttachment>() { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.04f,0.07f,-0.01f),new Rotator(0f, 0f, -180f)),
                new PropAttachment("RightHandWeapon","BONETAG_R_PH_HAND",new Vector3(0.04f,0.07f,-0.01f),new Rotator(0f, 0f, -180f)),
            }){ AliasWeaponHash = 2578778090 },//flint flathead screwdriver
            new PhysicalItem("gr_prop_gr_sdriver_02", new List<PropAttachment>() { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.04f,0.07f,-0.01f),new Rotator(0f, 0f, -180f)),
                new PropAttachment("RightHandWeapon","BONETAG_R_PH_HAND",new Vector3(0.04f,0.07f,-0.01f),new Rotator(0f, 0f, -180f)),
            }){ AliasWeaponHash = 2578778090 },//flint multi bit screwdriver





            new PhysicalItem("gr_prop_gr_hammer_01", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandWeapon","BONETAG_R_PH_HAND",new Vector3(0.02f,0f,-0.03f),new Rotator(0f, 0f, 0f)),      
            }) { AliasWeaponHash = 1317494643,IsLarge = true },//flint rubber hammer



            new PhysicalItem("prop_tool_drill", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
                new PropAttachment("RightHandWeapon","BONETAG_R_PH_HAND",new Vector3(0.03f,0.05f,0f),new Rotator(70f, 280f, -90f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(-0.01f, 0.05f, 0.05f),new Rotator(0f, 0f, 90f)),
            }){ AliasWeaponHash = 1317494643 },



            new PhysicalItem("gr_prop_gr_driver_01a", new List<PropAttachment>() { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandWeapon","BONETAG_R_PH_HAND",new Vector3(0.03f,-0.11f,-0.07f),new Rotator(160f, 243f, 278f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0f, 0.05f, -0.12f),new Rotator(0f, 0f, 0f)),
            }){ AliasWeaponHash = 1317494643 },//power metal impact driver





            new PhysicalItem("gr_prop_gr_drill_01a", new List<PropAttachment>() { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandWeapon","BONETAG_R_PH_HAND",new Vector3(0.19f,-0.1f,-0.09f),new Rotator(10f, 70f, 104f)),
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(-0.01f, -0.05f, -0.14f),new Rotator(0f, 0f, 0f)),
            }){ AliasWeaponHash = 1317494643 },//power metal cordless drill




            new PhysicalItem("hei_prop_heist_drill", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0f, 0f, 0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandWeapon", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.02f, -0.02f),new Rotator(-180f, -80f, -90f)),
                new PropAttachment("RightHand","BONETAG_R_PH_HAND",new Vector3(0f,0f,0f),new Rotator(0f, 0f, 0f)),
            }){ AliasWeaponHash = 1317494643 },
            new PhysicalItem("ch_prop_ch_heist_drill", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0f, 0f, 0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandWeapon", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.02f, -0.02f),new Rotator(-180f, -80f, -90f)),
                new PropAttachment("RightHand","BONETAG_R_PH_HAND",new Vector3(0f,0f,0f),new Rotator(0f, 0f, 0f)),
            }){ AliasWeaponHash = 1317494643 },
            new PhysicalItem("ch_prop_laserdrill_01a", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0f, 0f, 0f), new Rotator(0f, 0f, 0f)),
                new PropAttachment("RightHandWeapon", "BONETAG_R_PH_HAND", new Vector3(0.14f, 0.02f, -0.02f),new Rotator(-180f, -80f, -90f)),
                new PropAttachment("RightHand","BONETAG_R_PH_HAND",new Vector3(0f,0f,0f),new Rotator(0f, 0f, 0f)),
            }){ AliasWeaponHash = 1317494643 },








            new PhysicalItem("prop_binoc_01", new List<PropAttachment>() {
                new PropAttachment("RightHand", "BONETAG_R_PH_HAND", new Vector3(0.0f, 0.0f, 0.0f), Rotator.Zero),
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.128f, 0.015f, 0.087f), new Rotator(-21f, -249f, -6f)),
                new PropAttachment("ExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.1f, -0.1f, 0.0f), Rotator.Zero),
            }),

            new PhysicalItem("prop_cs_police_torch", new List<PropAttachment>() { ///police maglite
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0f, 0.002f, 0.002f), new Rotator(-180f, -130f, -100f)),
                new PropAttachment("ExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.0f, -0.12f, 0.0f), Rotator.Zero),//new PropAttachment("ExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.0f, -0.07f, 0.0f), Rotator.Zero),//new PropAttachment("ExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.0f, -0.05f, 0.0f), Rotator.Zero),
                new PropAttachment("EmissiveExtraDistance", "BONETAG_L_PH_HAND", new Vector3(-0.12f, -0.2f, 0.0f), Rotator.Zero),
                new PropAttachment("FrontRotation", "BONETAG_L_PH_HAND", new Vector3(90f, -1.0f, -1.0f), Rotator.Zero),
            }),
            new PhysicalItem("prop_tool_torch", new List<PropAttachment>() {//flint handle flashlight
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.12f, -0.02f, -0.08f), new Rotator(0f, 0f, -100f)),
                new PropAttachment("ExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.1f, 0.35f, 0.0f), Rotator.Zero),
                new PropAttachment("EmissiveExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.0f, 0.1f, 0.0f), Rotator.Zero),
                new PropAttachment("FrontRotation", "BONETAG_L_PH_HAND", new Vector3(-90f, 1.0f, 1.0f), Rotator.Zero),
            }),

            new PhysicalItem("prop_phone_ing", new List<PropAttachment>() { 
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.04f,-0.05f,-0.01f), new Rotator(-20f, -290f, -60f)),
                new PropAttachment("ExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.01f, 0.1f, 0.0f), Rotator.Zero),
                new PropAttachment("EmissiveExtraDistance", "BONETAG_L_PH_HAND", new Vector3(-0.05f, 0.2f, -0.1f), Rotator.Zero),
                new PropAttachment("FrontRotation", "BONETAG_L_PH_HAND", new Vector3(0f, 0.0f, 0.0f), Rotator.Zero),
            }),
            new PhysicalItem("prop_phone_ing_02", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.04f,-0.05f,-0.01f), new Rotator(-20f, -290f, -60f)),
                new PropAttachment("ExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.01f, 0.1f, 0.0f), Rotator.Zero),
                new PropAttachment("EmissiveExtraDistance", "BONETAG_L_PH_HAND", new Vector3(-0.05f, 0.2f, -0.1f), Rotator.Zero),
                new PropAttachment("FrontRotation", "BONETAG_L_PH_HAND", new Vector3(0f, 0.0f, 0.0f), Rotator.Zero),
            }),
            new PhysicalItem("prop_phone_ing_03", new List<PropAttachment>() {
                new PropAttachment("LeftHand", "BONETAG_L_PH_HAND", new Vector3(0.04f,-0.05f,-0.01f), new Rotator(-20f, -290f, -60f)),
                new PropAttachment("ExtraDistance", "BONETAG_L_PH_HAND", new Vector3(0.01f, 0.1f, 0.0f), Rotator.Zero),
                new PropAttachment("EmissiveExtraDistance", "BONETAG_L_PH_HAND", new Vector3(-0.05f, 0.2f, -0.1f), Rotator.Zero),
                new PropAttachment("FrontRotation", "BONETAG_L_PH_HAND", new Vector3(0f, 0.0f, 0.0f), Rotator.Zero),
            }),

        });
    }
}