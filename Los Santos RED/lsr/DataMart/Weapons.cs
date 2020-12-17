using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

public class Weapons
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Weapons.xml";
    private List<WeaponInformation> WeaponsList;
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            WeaponsList = Serialization.DeserializeParams<WeaponInformation>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(WeaponsList, ConfigFileName);
        }
    }
    public bool CanPlayerWeaponSuicide(Ped Pedestrian)
    {
        if (Pedestrian.Inventory.EquippedWeapon != null)
        {
            return false;
        }
        else
        {
            return WeaponsList.Any(x => (WeaponHash)x.Hash == Pedestrian.Inventory.EquippedWeapon.Hash && x.CanPistolSuicide);
        }
    }
    public WeaponInformation GetDamagingWeapon(Ped Pedestrian)
    {
        foreach (WeaponInformation MyWeapon in WeaponsList)
        {
            if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, MyWeapon.Hash, 0))
            {
                NativeFunction.CallByName<bool>("CLEAR_PED_LAST_WEAPON_DAMAGE", Pedestrian);
                return MyWeapon;
            }
        }
        if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, 0, 1))
            return new WeaponInformation("Generic Melee", 0, WeaponCategory.Melee, 0, 0, false, false, false);

        if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, 0, 2))
            return new WeaponInformation("Generic Weapon", 0, WeaponCategory.Melee, 0, 0, false, false, false);

        if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE", Pedestrian))
            return new WeaponInformation("Vehicle Injury", 0, WeaponCategory.Vehicle, 0, 0, false, false, false);
        else
            return new WeaponInformation("Unknown", 0, WeaponCategory.Unknown, 0, 0, false, false, false);
    }
    public WeaponInformation GetWeapon(ulong WeaponHash)
    {
        return WeaponsList.Where(x => x.Hash == WeaponHash).FirstOrDefault();
    }
    public WeaponInformation GetWeapon(string WeaponName)
    {
        return WeaponsList.Where(x => x.ModelName.ToLower() == WeaponName.ToLower()).FirstOrDefault();
    }
    public WeaponInformation GetRandomRegularWeapon(WeaponCategory MyCategory)
    {
        return WeaponsList.Where(x => x.Category == MyCategory && x.IsRegular).PickRandom();
    }
    public WeaponInformation GetRandomLowEndRegularWeapon()
    {
        return WeaponsList.Where(x => x.IsLowEnd && x.IsRegular).PickRandom();
    }
    public WeaponInformation GetCurrentWeapon(Ped Pedestrian)
    {
        if (Pedestrian.Inventory.EquippedWeapon == null)
            return null;
        ulong myHash = (ulong)Pedestrian.Inventory.EquippedWeapon.Hash;
        WeaponInformation CurrentGun = GetWeapon(myHash);
        if (CurrentGun != null)
            return CurrentGun;
        else
            return null;
    }
    public WeaponVariation GetWeaponVariation(Ped WeaponOwner, uint WeaponHash)
    {
        int Tint = NativeFunction.CallByName<int>("GET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash);
        WeaponInformation MyGun = GetWeapon(WeaponHash);
        if (MyGun == null)
            return new WeaponVariation("Variation1", Tint);

        List<string> ComponentsOnGun = new List<string>();

        foreach (WeaponComponent PossibleComponent in MyGun.PossibleComponents)
        {
            if (NativeFunction.CallByName<bool>("HAS_PED_GOT_WEAPON_COMPONENT", WeaponOwner, WeaponHash, PossibleComponent.Hash))
            {
                ComponentsOnGun.Add(PossibleComponent.Name);
            }

        }
        return new WeaponVariation("Variation1", Tint, ComponentsOnGun);

    }
    private void DefaultConfig()
    {
        WeaponsList = new List<WeaponInformation>
        {

            //Melee
            new WeaponInformation("weapon_dagger", 0, WeaponCategory.Melee, 0, 2460120199, false, false, true),
            new WeaponInformation("weapon_bat", 0, WeaponCategory.Melee, 0, 2508868239, false, false, true),
            new WeaponInformation("weapon_bottle", 0, WeaponCategory.Melee, 0, 4192643659, false, false, true),
            new WeaponInformation("weapon_crowbar", 0, WeaponCategory.Melee, 0, 2227010557, false, false, true),
            new WeaponInformation("weapon_flashlight", 0, WeaponCategory.Melee, 0, 2343591895, false, false, true),
            new WeaponInformation("weapon_golfclub", 0, WeaponCategory.Melee, 0, 1141786504, false, false, true),
            new WeaponInformation("weapon_hammer", 0, WeaponCategory.Melee, 0, 1317494643, false, false, true),
            new WeaponInformation("weapon_hatchet", 0, WeaponCategory.Melee, 0, 4191993645, false, false, true)
        };

        List<WeaponComponent> KnuckleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Base Model", 0xF3462F33, "weapon_knuckle"),
            new WeaponComponent("The Pimp", 0xC613F685, "weapon_knuckle"),
            new WeaponComponent("The Ballas", 0xEED9FD63, "weapon_knuckle"),
            new WeaponComponent("The Hustler", 0x50910C31, "weapon_knuckle"),
            new WeaponComponent("The Rock", 0x9761D9DC, "weapon_knuckle"),
            new WeaponComponent("The Hater", 0x7DECFE30, "weapon_knuckle"),
            new WeaponComponent("The Lover", 0x3F4E8AA6, "weapon_knuckle"),
            new WeaponComponent("The Player", 0x8B808BB, "weapon_knuckle"),
            new WeaponComponent("The King", 0xE28BABEF, "weapon_knuckle"),
            new WeaponComponent("The Vagos", 0x7AF3F785, "weapon_knuckle")
        };
        WeaponsList.Add(new WeaponInformation("weapon_knuckle", 0, WeaponCategory.Melee, 0, 3638508604, false, false, true) { IsRegular = false, PossibleComponents = KnuckleComponents });
        WeaponsList.Add(new WeaponInformation("weapon_knife", 0, WeaponCategory.Melee, 0, 2578778090, false, false, true));
        WeaponsList.Add(new WeaponInformation("weapon_machete", 0, WeaponCategory.Melee, 0, 3713923289, false, false, true));

        List<WeaponComponent> SwitchbladeComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Handle", 0x9137A500, "weapon_switchblade"),
            new WeaponComponent("VIP Variant", 0x5B3E7DB6, "weapon_switchblade"),
            new WeaponComponent("Bodyguard Variant", 0xE7939662, "weapon_switchblade")
        };
        WeaponsList.Add(new WeaponInformation("weapon_switchblade", 0, WeaponCategory.Melee, 0, 3756226112, false, false, false) { PossibleComponents = SwitchbladeComponents });

        WeaponsList.Add(new WeaponInformation("weapon_nightstick", 0, WeaponCategory.Melee, 0, 1737195953, false, false, true));
        WeaponsList.Add(new WeaponInformation("weapon_wrench", 0, WeaponCategory.Melee, 0, 0x19044EE0, false, false, true));
        WeaponsList.Add(new WeaponInformation("weapon_battleaxe", 0, WeaponCategory.Melee, 0, 3441901897, false, false, true) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_poolcue", 0, WeaponCategory.Melee, 0, 0x94117305, false, false, true));
        WeaponsList.Add(new WeaponInformation("weapon_stone_hatchet", 0, WeaponCategory.Melee, 0, 0x3813FC08, false, false, true) { IsRegular = false });
        //Pistol
        List<WeaponComponent> PistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xFED0FD71, "weapon_pistol"),
            new WeaponComponent("Extended Clip", 0xED265A1C, "weapon_pistol"),
            new WeaponComponent("Flashlight", 0x359B7AAE, "weapon_pistol"),
            new WeaponComponent("Suppressor", 0x65EA7EBB, "weapon_pistol"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD7391086, "weapon_pistol")
        };
        List<WeaponVariation> PistolVariations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Flashlight" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Extended Clip" }),
            //new WeaponVariation("Police3", 0, new List<string> { "Flashlight","Extended Clip" }),
            new WeaponVariation("Player0", 0),
            new WeaponVariation("Player1", 0, new List<string> { "Suppressor" }),

        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol", 60, WeaponCategory.Pistol, 1, 453432689, true, false, true) { PossibleComponents = PistolComponents, Variations = PistolVariations, CanPistolSuicide = true });

        List<WeaponComponent> PistolMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94F42D62, "weapon_pistol_mk2"),
            new WeaponComponent("Extended Clip", 0x5ED6C128, "weapon_pistol_mk2"),
            new WeaponComponent("Tracer Rounds", 0x25CAAEAF, "weapon_pistol_mk2"),
            new WeaponComponent("Incendiary Rounds", 0x2BBD7A3A, "weapon_pistol_mk2"),
            new WeaponComponent("Hollow Point Rounds", 0x85FEA109, "weapon_pistol_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0x4F37DF2A, "weapon_pistol_mk2"),
            new WeaponComponent("Mounted Scope", 0x8ED4BB70, "weapon_pistol_mk2"),
            new WeaponComponent("Flashlight", 0x43FD595B, "weapon_pistol_mk2"),
            new WeaponComponent("Suppressor", 0x65EA7EBB, "weapon_pistol_mk2"),
            new WeaponComponent("Compensator", 0x21E34793, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0x5C6C749C, "weapon_pistol_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x15F7A390, "weapon_pistol_mk2"),
            new WeaponComponent("Woodland Camo", 0x968E24DB, "weapon_pistol_mk2"),
            new WeaponComponent("Skull", 0x17BFA99, "weapon_pistol_mk2"),
            new WeaponComponent("Sessanta Nove", 0xF2685C72, "weapon_pistol_mk2"),
            new WeaponComponent("Perseus", 0xDD2231E6, "weapon_pistol_mk2"),
            new WeaponComponent("Leopard", 0xBB43EE76, "weapon_pistol_mk2"),
            new WeaponComponent("Zebra", 0x4D901310, "weapon_pistol_mk2"),
            new WeaponComponent("Geometric", 0x5F31B653, "weapon_pistol_mk2"),
            new WeaponComponent("Boom!", 0x697E19A0, "weapon_pistol_mk2"),
            new WeaponComponent("Patriotic", 0x930CB951, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0xB4FC92B0, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0x1A1F1260, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0xE4E00B70, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0x2C298B2B, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0xDFB79725, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0x6BD7228C, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0x9DDBCF8C, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0xB319A52C, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0xC6836E12, "weapon_pistol_mk2"),
            new WeaponComponent("Digital Camo", 0x43B1B173, "weapon_pistol_mk2"),
            new WeaponComponent("Patriotic", 0x4ABDA3FA, "weapon_pistol_mk2")
        };
        List<WeaponVariation> PistolMK2Variations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Flashlight" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Extended Clip" }),
            //new WeaponVariation("Police3", 0, new List<string> { "Flashlight","Extended Clip" }),
            new WeaponVariation("Player0", 0),
            new WeaponVariation("Player1", 0, new List<string> { "Suppressor" }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol_mk2", 60, WeaponCategory.Pistol, 1, 0xBFE256D4, true, false, true) { PossibleComponents = PistolMK2Components, Variations = PistolMK2Variations, CanPistolSuicide = true });

        List<WeaponComponent> CombatPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x721B079, "weapon_combatpistol"),
            new WeaponComponent("Extended Clip", 0xD67B4F2D, "weapon_combatpistol"),
            new WeaponComponent("Flashlight", 0x359B7AAE, "weapon_combatpistol"),
            new WeaponComponent("Suppressor", 0xC304849A, "weapon_combatpistol"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xC6654D72, "weapon_combatpistol")
        };
        List<WeaponVariation> CombatPistolVariations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Flashlight" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Extended Clip" }),
            //new WeaponVariation("Police3", 0, new List<string> { "Flashlight","Extended Clip" }),
            new WeaponVariation("Player0", 0),
            new WeaponVariation("Player1", 0, new List<string> { "Suppressor" }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatpistol", 60, WeaponCategory.Pistol, 1, 1593441988, true, false, true) { PossibleComponents = CombatPistolComponents, Variations = CombatPistolVariations, CanPistolSuicide = true });


        List<WeaponComponent> APPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x31C4B22A, "weapon_appistol"),
            new WeaponComponent("Extended Clip", 0x249A17D5, "weapon_appistol"),
            new WeaponComponent("Flashlight", 0x359B7AAE, "weapon_appistol"),
            new WeaponComponent("Suppressor", 0xC304849A, "weapon_appistol"),
            new WeaponComponent("Gilded Gun Metal Finish", 0x9B76C72C, "weapon_appistol")
        };
        WeaponsList.Add(new WeaponInformation("weapon_appistol", 60, WeaponCategory.Pistol, 1, 584646201, true, false, false) { PossibleComponents = APPistolComponents, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_stungun", 0, WeaponCategory.Melee, 0, 911657153, true, false, true) { CanPistolSuicide = false });

        List<WeaponComponent> Pistol50Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x2297BE19, "weapon_pistol50"),
            new WeaponComponent("Extended Clip", 0xD9D3AC92, "weapon_pistol50"),
            new WeaponComponent("Flashlight", 0x359B7AAE, "weapon_pistol50"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_pistol50"),
            new WeaponComponent("Platinum Pearl Deluxe Finish", 0x77B8AB2F, "weapon_pistol50")
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol50", 60, WeaponCategory.Pistol, 1, 2578377531, true, false, true) { PossibleComponents = Pistol50Components, CanPistolSuicide = true });

        List<WeaponComponent> SNSPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xF8802ED9, "weapon_snspistol"),
            new WeaponComponent("Extended Clip", 0x7B0033B3, "weapon_snspistol"),
            new WeaponComponent("Etched Wood Grip Finish", 0x8033ECAF, "weapon_snspistol")
        };
        WeaponsList.Add(new WeaponInformation("weapon_snspistol", 60, WeaponCategory.Pistol, 1, 3218215474, true, false, true) { PossibleComponents = SNSPistolComponents, CanPistolSuicide = true });

        List<WeaponComponent> SNSPistolMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x1466CE6, "weapon_snspistol_mk2"),
            new WeaponComponent("Extended Clip", 0xCE8C0772, "weapon_snspistol_mk2"),
            new WeaponComponent("Tracer Rounds", 0x902DA26E, "weapon_snspistol_mk2"),
            new WeaponComponent("Incendiary Rounds", 0xE6AD5F79, "weapon_snspistol_mk2"),
            new WeaponComponent("Hollow Point Rounds", 0x8D107402, "weapon_snspistol_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0xC111EB26, "weapon_snspistol_mk2"),
            new WeaponComponent("Flashlight", 0x4A4965F3, "weapon_snspistol_mk2"),
            new WeaponComponent("Mounted Scope", 0x47DE9258, "weapon_snspistol_mk2"),
            new WeaponComponent("Suppressor", 0x65EA7EBB, "weapon_snspistol_mk2"),
            new WeaponComponent("Compensator", 0xAA8283BF, "weapon_snspistol_mk2"),
            new WeaponComponent("Digital Camo", 0xF7BEEDD, "weapon_snspistol_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x8A612EF6, "weapon_snspistol_mk2"),
            new WeaponComponent("Woodland Camo", 0x76FA8829, "weapon_snspistol_mk2"),
            new WeaponComponent("Skull", 0xA93C6CAC, "weapon_snspistol_mk2"),
            new WeaponComponent("Sessanta Nove", 0x9C905354, "weapon_snspistol_mk2"),
            new WeaponComponent("Perseus", 0x4DFA3621, "weapon_snspistol_mk2"),
            new WeaponComponent("Leopard", 0x42E91FFF, "weapon_snspistol_mk2"),
            new WeaponComponent("Zebra", 0x54A8437D, "weapon_snspistol_mk2"),
            new WeaponComponent("Geometric", 0x68C2746, "weapon_snspistol_mk2"),
            new WeaponComponent("Boom!", 0x2366E467, "weapon_snspistol_mk2"),
            new WeaponComponent("Boom!", 0x441882E6, "weapon_snspistol_mk2"),
            new WeaponComponent("Digital Camo", 0xE7EE68EA, "weapon_snspistol_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x29366D21, "weapon_snspistol_mk2"),
            new WeaponComponent("Woodland Camo", 0x3ADE514B, "weapon_snspistol_mk2"),
            new WeaponComponent("Skull", 0xE64513E9, "weapon_snspistol_mk2"),
            new WeaponComponent("Sessanta Nove", 0xCD7AEB9A, "weapon_snspistol_mk2"),
            new WeaponComponent("Perseus", 0xFA7B27A6, "weapon_snspistol_mk2"),
            new WeaponComponent("Leopard", 0xE285CA9A, "weapon_snspistol_mk2"),
            new WeaponComponent("Zebra", 0x2B904B19, "weapon_snspistol_mk2"),
            new WeaponComponent("Geometric", 0x22C24F9C, "weapon_snspistol_mk2"),
            new WeaponComponent("Boom!", 0x8D0D5ECD, "weapon_snspistol_mk2"),
            new WeaponComponent("Patriotic", 0x1F07150A, "weapon_snspistol_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_snspistol_mk2", 60, WeaponCategory.Pistol, 1, 0x88374054, true, false, true) { PossibleComponents = SNSPistolMK2Components, CanPistolSuicide = true });

        List<WeaponComponent> HeavyPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xD4A969A, "weapon_heavypistol"),
            new WeaponComponent("Extended Clip", 0x64F9C62B, "weapon_heavypistol"),
            new WeaponComponent("Flashlight", 0x359B7AAE, "weapon_heavypistol"),
            new WeaponComponent("Suppressor", 0xC304849A, "weapon_heavypistol"),
            new WeaponComponent("Etched Wood Grip Finish", 0x7A6A7B7B, "weapon_heavypistol")
        };
        List<WeaponVariation> HeavyPistolVariations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Etched Wood Grip Finish" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Flashlight","Extended Clip" }),
            new WeaponVariation("Player0", 0),
            new WeaponVariation("Player1", 0, new List<string> { "Flashlight","Etched Wood Grip Finish" }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavypistol", 60, WeaponCategory.Pistol, 1, 3523564046, true, false, true) { PossibleComponents = HeavyPistolComponents, Variations = HeavyPistolVariations, CanPistolSuicide = true });

        List<WeaponComponent> VintagePistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x45A3B6BB, "weapon_vintagepistol"),
            new WeaponComponent("Extended Clip", 0x33BA12E8, "weapon_vintagepistol"),
            new WeaponComponent("Suppressor", 0xC304849A, "weapon_vintagepistol")
        };
        WeaponsList.Add(new WeaponInformation("weapon_vintagepistol", 60, WeaponCategory.Pistol, 1, 137902532, true, false, true) { PossibleComponents = VintagePistolComponents, CanPistolSuicide = true });


        WeaponsList.Add(new WeaponInformation("weapon_flaregun", 60, WeaponCategory.Pistol, 1, 1198879012, true, false, true) { IsRegular = false, CanPistolSuicide = false });
        WeaponsList.Add(new WeaponInformation("weapon_marksmanpistol", 60, WeaponCategory.Pistol, 1, 3696079510, true, false, true) { IsRegular = false, CanPistolSuicide = false });

        List<WeaponComponent> RevolverComponents = new List<WeaponComponent>
        {
            new WeaponComponent("VIP Variant", 0x16EE3040, "weapon_revolver"),
            new WeaponComponent("Bodyguard Variant", 0x9493B80D, "weapon_revolver"),
            new WeaponComponent("Default Clip", 0xE9867CE3, "weapon_revolver")
        };
        WeaponsList.Add(new WeaponInformation("weapon_revolver", 60, WeaponCategory.Pistol, 1, 3249783761, true, false, true) { PossibleComponents = RevolverComponents, CanPistolSuicide = true });

        List<WeaponComponent> RevolverMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Rounds", 0xBA23D8BE, "weapon_revolver_mk2"),
            new WeaponComponent("Tracer Rounds", 0xC6D8E476, "weapon_revolver_mk2"),
            new WeaponComponent("Incendiary Rounds", 0xEFBF25, "weapon_revolver_mk2"),
            new WeaponComponent("Hollow Point Rounds", 0x10F42E8F, "weapon_revolver_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0xDC8BA3F, "weapon_revolver_mk2"),
            new WeaponComponent("Holographic Sight", 0x420FD713, "weapon_revolver_mk2"),
            new WeaponComponent("Small Scope", 0x49B2945, "weapon_revolver_mk2"),
            new WeaponComponent("Flashlight", 0x359B7AAE, "weapon_revolver_mk2"),
            new WeaponComponent("Compensator", 0x27077CCB, "weapon_revolver_mk2"),
            new WeaponComponent("Digital Camo", 0xC03FED9F, "weapon_revolver_mk2"),
            new WeaponComponent("Brushstroke Camo", 0xB5DE24, "weapon_revolver_mk2"),
            new WeaponComponent("Woodland Camo", 0xA7FF1B8, "weapon_revolver_mk2"),
            new WeaponComponent("Skull", 0xF2E24289, "weapon_revolver_mk2"),
            new WeaponComponent("Sessanta Nove", 0x11317F27, "weapon_revolver_mk2"),
            new WeaponComponent("Perseus", 0x17C30C42, "weapon_revolver_mk2"),
            new WeaponComponent("Leopard", 0x257927AE, "weapon_revolver_mk2"),
            new WeaponComponent("Zebra", 0x37304B1C, "weapon_revolver_mk2"),
            new WeaponComponent("Geometric", 0x48DAEE71, "weapon_revolver_mk2"),
            new WeaponComponent("Boom!", 0x20ED9B5B, "weapon_revolver_mk2"),
            new WeaponComponent("Patriotic", 0xD951E867, "weapon_revolver_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_revolver_mk2", 60, WeaponCategory.Pistol, 1, 0xCB96392F, true, false, true) { PossibleComponents = RevolverMK2Components, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_doubleaction", 60, WeaponCategory.Pistol, 1, 0x97EA20B8, true, false, true) { CanPistolSuicide = true });
        List<WeaponComponent> RayGunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Festive tint", 0xD7DBF707, "weapon_raypistol")
        };
        WeaponsList.Add(new WeaponInformation("weapon_raypistol", 60, WeaponCategory.Pistol, 1, 0xAF3696A1, true, false, false) { IsRegular = false, PossibleComponents = RayGunComponents, CanPistolSuicide = false });

        //Shotgun
        List<WeaponComponent> PumpShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_pumpshotgun"),
            new WeaponComponent("Suppressor", 0xE608B35E, "weapon_pumpshotgun"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xA2D79DDB, "weapon_pumpshotgun")
        };
        List<WeaponVariation> PumpShotgunVariations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Flashlight" }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_pumpshotgun", 32, WeaponCategory.Shotgun, 2, 487013001, false, true, true) { PossibleComponents = PumpShotgunComponents, Variations = PumpShotgunVariations });

        List<WeaponComponent> PumpShotgunMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Shells", 0xCD940141, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Dragon's Breath Shells", 0x9F8A1BF5, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Steel Buckshot Shells", 0x4E65B425, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Flechette Shells", 0xE9582927, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Explosive Slugs", 0x3BE4465D, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Holographic Sight", 0x420FD713, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Small Scope", 0x49B2945, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Medium Scope", 0x3F3C8181, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Suppressor", 0xAC42DF71, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Squared Muzzle Brake", 0x5F7DCE4D, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Digital Camo", 0xE3BD9E44, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x17148F9B, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Woodland Camo", 0x24D22B16, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Skull", 0xF2BEC6F0, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Sessanta Nove", 0x85627D, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Perseus", 0xDC2919C5, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Leopard", 0xE184247B, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Zebra", 0xD8EF9356, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Geometric", 0xEF29BFCA, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Boom!", 0x67AEB165, "weapon_pumpshotgun_mk2"),
            new WeaponComponent("Patriotic", 0x46411A1D, "weapon_pumpshotgun_mk2")
        };
        List<WeaponVariation> PumpShotgunMK2Variations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Flashlight" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Holographic Sight"}),
            //new WeaponVariation("Police3", 0, new List<string> { "Holographic Sight","Flashlight" }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_pumpshotgun_mk2", 32, WeaponCategory.Shotgun, 2, 0x555AF99A, false, true, true) { PossibleComponents = PumpShotgunMK2Components, Variations = PumpShotgunMK2Variations });

        List<WeaponComponent> SawnOffShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Gilded Gun Metal Finish", 0x85A64DF9, "weapon_sawnoffshotgun")
        };
        WeaponsList.Add(new WeaponInformation("weapon_sawnoffshotgun", 32, WeaponCategory.Shotgun, 2, 2017895192, false, true, false) { PossibleComponents = SawnOffShotgunComponents });

        List<WeaponComponent> AssaultShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94E81BC7, "weapon_assaultshotgun"),
            new WeaponComponent("Extended Clip", 0x86BD7F72, "weapon_assaultshotgun"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_assaultshotgun"),
            new WeaponComponent("Suppressor", 0x837445AA, "weapon_assaultshotgun"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_assaultshotgun")
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultshotgun", 32, WeaponCategory.Shotgun, 2, 3800352039, false, true, false) { PossibleComponents = AssaultShotgunComponents });

        List<WeaponComponent> BullpupShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_bullpupshotgun"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_bullpupshotgun"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_bullpupshotgun")
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpupshotgun", 32, WeaponCategory.Shotgun, 2, 2640438543, false, true, true) { PossibleComponents = BullpupShotgunComponents });

        WeaponsList.Add(new WeaponInformation("weapon_musket", 32, WeaponCategory.Shotgun, 2, 2828843422, false, true, true) { IsRegular = false });

        List<WeaponComponent> HeavyShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x324F2D5F, "weapon_heavyshotgun"),
            new WeaponComponent("Extended Clip", 0x971CF6FD, "weapon_heavyshotgun"),
            new WeaponComponent("Drum Magazine", 0x88C7DA53, "weapon_heavyshotgun"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_heavyshotgun"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_heavyshotgun"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_heavyshotgun")
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavyshotgun", 32, WeaponCategory.Shotgun, 2, 984333226, false, true, true) { PossibleComponents = HeavyShotgunComponents });

        WeaponsList.Add(new WeaponInformation("weapon_dbshotgun", 32, WeaponCategory.Shotgun, 2, 4019527611, false, true, false) { CanPistolSuicide = true });
        WeaponsList.Add(new WeaponInformation("weapon_autoshotgun", 32, WeaponCategory.Shotgun, 2, 317205821, false, true, false));
        //SMG

        List<WeaponComponent> MicroSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xCB48AEF0, "weapon_microsmg"),
            new WeaponComponent("Extended Clip", 0x10E6BA2B, "weapon_microsmg"),
            new WeaponComponent("Flashlight", 0x359B7AAE, "weapon_microsmg"),
            new WeaponComponent("Scope", 0x9D2FBF29, "weapon_microsmg"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_microsmg"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x487AAE09, "weapon_microsmg")
        };
        List<WeaponVariation> MicroSMGVariations = new List<WeaponVariation>
        {
            new WeaponVariation("Player0", 0),
            new WeaponVariation("Player1", 0, new List<string> { "Extended Clip", "Flashlight" }),
            new WeaponVariation("Player2", 0, new List<string> { "Extended Clip" }),
            new WeaponVariation("Player3", 0, new List<string> { "Extended Clip", "Suppressor" }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_microsmg", 32, WeaponCategory.SMG, 2, 324215364, true, false, false) { PossibleComponents = MicroSMGComponents, Variations = MicroSMGVariations, CanPistolSuicide = true });


        List<WeaponComponent> SMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x26574997, "weapon_smg"),
            new WeaponComponent("Extended Clip", 0x350966FB, "weapon_smg"),
            new WeaponComponent("Drum Magazine", 0x79C77076, "weapon_smg"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_smg"),
            new WeaponComponent("Scope", 0x3CC6BA57, "weapon_smg"),
            new WeaponComponent("Suppressor", 0xC304849A, "weapon_smg"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x27872C90, "weapon_smg")
        };
        WeaponsList.Add(new WeaponInformation("weapon_smg", 32, WeaponCategory.SMG, 2, 736523883, false, true, false) { PossibleComponents = SMGComponents });


        List<WeaponComponent> SMGMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4C24806E, "weapon_smg_mk2"),
            new WeaponComponent("Extended Clip", 0xB9835B2E, "weapon_smg_mk2"),
            new WeaponComponent("Tracer Rounds", 0x7FEA36EC, "weapon_smg_mk2"),
            new WeaponComponent("Incendiary Rounds", 0xD99222E5, "weapon_smg_mk2"),
            new WeaponComponent("Hollow Point Rounds", 0x3A1BD6FA, "weapon_smg_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0xB5A715F, "weapon_smg_mk2"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_smg_mk2"),
            new WeaponComponent("Holographic Sight", 0x9FDB5652, "weapon_smg_mk2"),
            new WeaponComponent("Small Scope", 0xE502AB6B, "weapon_smg_mk2"),
            new WeaponComponent("Medium Scope", 0x3DECC7DA, "weapon_smg_mk2"),
            new WeaponComponent("Suppressor", 0xC304849A, "weapon_smg_mk2"),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4, "weapon_smg_mk2"),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B, "weapon_smg_mk2"),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF, "weapon_smg_mk2"),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC, "weapon_smg_mk2"),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A, "weapon_smg_mk2"),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC, "weapon_smg_mk2"),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE, "weapon_smg_mk2"),
            new WeaponComponent("Default Barrel", 0xD9103EE1, "weapon_smg_mk2"),
            new WeaponComponent("Heavy Barrel", 0xA564D78B, "weapon_smg_mk2"),
            new WeaponComponent("Digital Camo", 0xC4979067, "weapon_smg_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x3815A945, "weapon_smg_mk2"),
            new WeaponComponent("Woodland Camo", 0x4B4B4FB0, "weapon_smg_mk2"),
            new WeaponComponent("Skull", 0xEC729200, "weapon_smg_mk2"),
            new WeaponComponent("Sessanta Nove", 0x48F64B22, "weapon_smg_mk2"),
            new WeaponComponent("Perseus", 0x35992468, "weapon_smg_mk2"),
            new WeaponComponent("Leopard", 0x24B782A5, "weapon_smg_mk2"),
            new WeaponComponent("Zebra", 0xA2E67F01, "weapon_smg_mk2"),
            new WeaponComponent("Geometric", 0x2218FD68, "weapon_smg_mk2"),
            new WeaponComponent("Boom!", 0x45C5C3C5, "weapon_smg_mk2"),
            new WeaponComponent("Patriotic", 0x399D558F, "weapon_smg_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_smg_mk2", 32, WeaponCategory.SMG, 2, 0x78A97CD0, false, true, false) { PossibleComponents = SMGMK2Components });


        List<WeaponComponent> AssaultSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x8D1307B0, "weapon_assaultsmg"),
            new WeaponComponent("Extended Clip", 0xBB46E417, "weapon_assaultsmg"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_assaultsmg"),
            new WeaponComponent("Scope", 0x9D2FBF29, "weapon_assaultsmg"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_assaultsmg"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x278C78AF, "weapon_assaultsmg")
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultsmg", 32, WeaponCategory.SMG, 2, 4024951519, false, true, false) { PossibleComponents = AssaultSMGComponents });


        List<WeaponComponent> CombatPDWComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4317F19E, "weapon_combatpdw"),
            new WeaponComponent("Extended Clip", 0x334A5203, "weapon_combatpdw"),
            new WeaponComponent("Drum Magazine", 0x6EB8C8DB, "weapon_combatpdw"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_combatpdw"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_combatpdw"),
            new WeaponComponent("Scope", 0xAA2C45B4, "weapon_combatpdw")
        };
        List<WeaponVariation> CombatPDWVariations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Scope","Grip","Flashlight" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Scope", "Grip","Flashlight" }),
            //new WeaponVariation("Police3", 0, new List<string> { "Scope", "Grip","Extended Clip" }),
            new WeaponVariation("Player0", 0),
            new WeaponVariation("Player1", 0, new List<string> { "Scope", "Grip","Flashlight","Extended Clip" }),
            new WeaponVariation("Player2", 0, new List<string> { "Scope", "Grip" }),
            new WeaponVariation("Player3", 0, new List<string> { "Grip","Flashlight","Extended Clip" }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatpdw", 32, WeaponCategory.SMG, 2, 171789620, false, true, false) { PossibleComponents = CombatPDWComponents, Variations = CombatPDWVariations });


        List<WeaponComponent> MachinePistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x476E85FF, "weapon_machinepistol"),
            new WeaponComponent("Extended Clip", 0xB92C6979, "weapon_machinepistol"),
            new WeaponComponent("Drum Magazine", 0xA9E9CAF4, "weapon_machinepistol"),
            new WeaponComponent("Suppressor", 0xC304849A, "weapon_machinepistol")
        };
        WeaponsList.Add(new WeaponInformation("weapon_machinepistol", 32, WeaponCategory.SMG, 2, 3675956304, true, false, false) { PossibleComponents = MachinePistolComponents, CanPistolSuicide = true });



        List<WeaponComponent> MiniSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x84C8B2D3, "weapon_minismg"),
            new WeaponComponent("Extended Clip", 0x937ED0B7, "weapon_minismg")
        };
        WeaponsList.Add(new WeaponInformation("weapon_minismg", 32, WeaponCategory.SMG, 2, 3173288789, true, false, false) { PossibleComponents = MiniSMGComponents, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_raycarbine", 32, WeaponCategory.SMG, 2, 0x476BF155, true, false, false) { IsRegular = false });
        //AR
        List<WeaponComponent> ARComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xBE5EEA16, "weapon_assaultrifle"),
            new WeaponComponent("Extended Clip", 0xB1214F9B, "weapon_assaultrifle"),
            new WeaponComponent("Drum Magazine", 0xDBF0A53D, "weapon_assaultrifle"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_assaultrifle"),
            new WeaponComponent("Scope", 0x9D2FBF29, "weapon_assaultrifle"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_assaultrifle"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_assaultrifle"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x4EAD7533, "weapon_assaultrifle")
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultrifle", 120, WeaponCategory.AR, 3, 3220176749, false, true, false) { PossibleComponents = ARComponents });


        List<WeaponComponent> ARMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x8610343F, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Extended Clip", 0xD12ACA6F, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Tracer Rounds", 0xEF2C78C1, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Incendiary Rounds", 0xFB70D853, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Armor Piercing Rounds", 0xA7DD1E58, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0x63E0A098, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Grip", 0x9D65907A, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Holographic Sight", 0x420FD713, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Small Scope", 0x49B2945, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Large Scope", 0xC66B6542, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Default Barrel", 0x43A49D26, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Heavy Barrel", 0x5646C26A, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Digital Camo", 0x911B24AF, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x37E5444B, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Woodland Camo", 0x538B7B97, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Skull", 0x25789F72, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Sessanta Nove", 0xC5495F2D, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Perseus", 0xCF8B73B1, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Leopard", 0xA9BB2811, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Zebra", 0xFC674D54, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Geometric", 0x7C7FCD9B, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Boom!", 0xA5C38392, "weapon_assaultrifle_mk2"),
            new WeaponComponent("Patriotic", 0xB9B15DB0, "weapon_assaultrifle_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultrifle_mk2", 120, WeaponCategory.AR, 3, 0x394F415C, false, true, false) { PossibleComponents = ARMK2Components });


        List<WeaponComponent> CarbineRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x9FBE33EC, "weapon_carbinerifle"),
            new WeaponComponent("Extended Clip", 0x91109691, "weapon_carbinerifle"),
            new WeaponComponent("Box Magazine", 0xBA62E935, "weapon_carbinerifle"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_carbinerifle"),
            new WeaponComponent("Scope", 0xA0D89C42, "weapon_carbinerifle"),
            new WeaponComponent("Suppressor", 0x837445AA, "weapon_carbinerifle"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_carbinerifle"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD89B9658, "weapon_carbinerifle")
        };
        List<WeaponVariation> CarbineRifleVariations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Grip","Flashlight" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Scope", "Grip","Flashlight" }),
            //new WeaponVariation("Police3", 0, new List<string> { "Scope", "Grip","Flashlight","Extended Clip" }),
            new WeaponVariation("Player0", 0),
            new WeaponVariation("Player1", 0, new List<string> { "Scope", "Grip","Flashlight","Extended Clip" }),
            new WeaponVariation("Player2", 0, new List<string> { "Scope", "Grip" }),
            new WeaponVariation("Player3", 0, new List<string> { "Grip","Flashlight","Extended Clip" }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_carbinerifle", 120, WeaponCategory.AR, 3, 2210333304, false, true, false) { PossibleComponents = CarbineRifleComponents, Variations = CarbineRifleVariations });


        List<WeaponComponent> CarbineRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4C7A391E, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Extended Clip", 0x5DD5DBD5, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Tracer Rounds", 0x1757F566, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Incendiary Rounds", 0x3D25C2A7, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Armor Piercing Rounds", 0x255D5D57, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0x44032F11, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Grip", 0x9D65907A, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Holographic Sight", 0x420FD713, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Small Scope", 0x49B2945, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Large Scope", 0xC66B6542, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Suppressor", 0x837445AA, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Default Barrel", 0x833637FF, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Heavy Barrel", 0x8B3C480B, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Digital Camo", 0x4BDD6F16, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x406A7908, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Woodland Camo", 0x2F3856A4, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Skull", 0xE50C424D, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Sessanta Nove", 0xD37D1F2F, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Perseus", 0x86268483, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Leopard", 0xF420E076, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Zebra", 0xAAE14DF8, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Geometric", 0x9893A95D, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Boom!", 0x6B13CD3E, "weapon_carbinerifle_mk2"),
            new WeaponComponent("Patriotic", 0xDA55CD3F, "weapon_carbinerifle_mk2")
        };
        List<WeaponVariation> CarbineRifleMK2Variations = new List<WeaponVariation>
        {
            //new WeaponVariation("Police0", 0),
            //new WeaponVariation("Police1", 0, new List<string> { "Holographic Sight","Grip","Flashlight" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Holographic Sight", "Grip","Extended Clip" }),
            //new WeaponVariation("Police2", 0, new List<string> { "Large Scope", "Grip","Flashlight","Extended Clip" }),
            new WeaponVariation("Player0", 0),
            new WeaponVariation("Player1", 0, new List<string> { "Holographic Sight","Grip","Flashlight" }),
            new WeaponVariation("Player2", 0, new List<string> { "Holographic Sight", "Grip","Flashlight","Extended Clip"  }),
            new WeaponVariation("Player3", 0, new List<string> { "Large Scope", "Grip","Flashlight","Extended Clip"  }),
        };
        WeaponsList.Add(new WeaponInformation("weapon_carbinerifle_mk2", 120, WeaponCategory.AR, 3, 0xFAD1F1C9, false, true, false) { PossibleComponents = CarbineRifleMK2Components, Variations = CarbineRifleMK2Variations });


        List<WeaponComponent> AdvancedRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xFA8FA10F, "weapon_advancedrifle"),
            new WeaponComponent("Extended Clip", 0x8EC1C979, "weapon_advancedrifle"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_advancedrifle"),
            new WeaponComponent("Scope", 0xAA2C45B4, "weapon_advancedrifle"),
            new WeaponComponent("Suppressor", 0x837445AA, "weapon_advancedrifle"),
            new WeaponComponent("Gilded Gun Metal Finish", 0x377CD377, "weapon_advancedrifle")
        };
        WeaponsList.Add(new WeaponInformation("weapon_advancedrifle", 120, WeaponCategory.AR, 3, 2937143193, false, true, false) { PossibleComponents = AdvancedRifleComponents });


        List<WeaponComponent> SpecialCarbineComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xC6C7E581, "weapon_specialcarbine"),
            new WeaponComponent("Extended Clip", 0x7C8BD10E, "weapon_specialcarbine"),
            new WeaponComponent("Drum Magazine", 0x6B59AEAA, "weapon_specialcarbine"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_specialcarbine"),
            new WeaponComponent("Scope", 0xA0D89C42, "weapon_specialcarbine"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_specialcarbine"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_specialcarbine"),
            new WeaponComponent("Etched Gun Metal Finish", 0x730154F2, "weapon_specialcarbine")
        };
        WeaponsList.Add(new WeaponInformation("weapon_specialcarbine", 120, WeaponCategory.AR, 3, 3231910285, false, true, false) { PossibleComponents = SpecialCarbineComponents });


        List<WeaponComponent> SpecialCarbineMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x16C69281, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Extended Clip", 0xDE1FA12C, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Tracer Rounds", 0x8765C68A, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Incendiary Rounds", 0xDE011286, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Armor Piercing Rounds", 0x51351635, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0x503DEA90, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Holographic Sight", 0x420FD713, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Small Scope", 0x49B2945, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Large Scope", 0xC66B6542, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Grip", 0x9D65907A, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Default Barrel", 0xE73653A9, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Heavy Barrel", 0xF97F783B, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Digital Camo", 0xD40BB53B, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x431B238B, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Woodland Camo", 0x34CF86F4, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Skull", 0xB4C306DD, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Sessanta Nove", 0xEE677A25, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Perseus", 0xDF90DC78, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Leopard", 0xA4C31EE, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Zebra", 0x89CFB0F7, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Geometric", 0x7B82145C, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Boom!", 0x899CAF75, "weapon_specialcarbine_mk2"),
            new WeaponComponent("Patriotic", 0x5218C819, "weapon_specialcarbine_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_specialcarbine_mk2", 120, WeaponCategory.AR, 3, 0x969C3D67, false, true, false) { PossibleComponents = SpecialCarbineMK2Components });


        List<WeaponComponent> BullpupRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xC5A12F80, "weapon_bullpuprifle"),
            new WeaponComponent("Extended Clip", 0xB3688B0F, "weapon_bullpuprifle"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_bullpuprifle"),
            new WeaponComponent("Scope", 0xAA2C45B4, "weapon_bullpuprifle"),
            new WeaponComponent("Suppressor", 0x837445AA, "weapon_bullpuprifle"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_bullpuprifle"),
            new WeaponComponent("Gilded Gun Metal Finish", 0xA857BC78, "weapon_bullpuprifle")
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpuprifle", 120, WeaponCategory.AR, 3, 2132975508, false, true, false) { PossibleComponents = BullpupRifleComponents });


        List<WeaponComponent> BullpulRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x18929DA, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Extended Clip", 0xEFB00628, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Tracer Rounds", 0x822060A9, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Incendiary Rounds", 0xA99CF95A, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Armor Piercing Rounds", 0xFAA7F5ED, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0x43621710, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Holographic Sight", 0x420FD713, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Small Scope", 0xC7ADD105, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Medium Scope", 0x3F3C8181, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Default Barrel", 0x659AC11B, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Heavy Barrel", 0x3BF26DC7, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Suppressor", 0x837445AA, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Grip", 0x9D65907A, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Digital Camo", 0xAE4055B7, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Brushstroke Camo", 0xB905ED6B, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Woodland Camo", 0xA6C448E8, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Skull", 0x9486246C, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Sessanta Nove", 0x8A390FD2, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Perseus", 0x2337FC5, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Leopard", 0xEFFFDB5E, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Zebra", 0xDDBDB6DA, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Geometric", 0xCB631225, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Boom!", 0xA87D541E, "weapon_bullpuprifle_mk2"),
            new WeaponComponent("Patriotic", 0xC5E9AE52, "weapon_bullpuprifle_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpuprifle_mk2", 120, WeaponCategory.AR, 3, 0x84D6FAFD, false, true, false) { PossibleComponents = BullpulRifleMK2Components });


        List<WeaponComponent> CompactRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x513F0A63, "weapon_compactrifle"),
            new WeaponComponent("Extended Clip", 0x59FF9BF8, "weapon_compactrifle"),
            new WeaponComponent("Drum Magazine", 0xC607740E, "weapon_compactrifle")
        };
        WeaponsList.Add(new WeaponInformation("weapon_compactrifle", 120, WeaponCategory.AR, 3, 1649403952, false, true, false) { PossibleComponents = CompactRifleComponents });

        //LMG

        List<WeaponComponent> MGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xF434EF84, "weapon_mg"),
            new WeaponComponent("Extended Clip", 0x82158B47, "weapon_mg"),
            new WeaponComponent("Scope", 0x3C00AFED, "weapon_mg"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD6DABABE, "weapon_mg")
        };
        WeaponsList.Add(new WeaponInformation("weapon_mg", 200, WeaponCategory.LMG, 4, 2634544996, false, true, false) { PossibleComponents = MGComponents });


        List<WeaponComponent> CombatMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xE1FFB34A, "weapon_combatmg"),
            new WeaponComponent("Extended Clip", 0xD6C59CD6, "weapon_combatmg"),
            new WeaponComponent("Scope", 0xA0D89C42, "weapon_combatmg"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_combatmg"),
            new WeaponComponent("Etched Gun Metal Finish", 0x92FECCDD, "weapon_combatmg")
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatmg", 200, WeaponCategory.LMG, 4, 2144741730, false, true, false) { PossibleComponents = CombatMGComponents });


        List<WeaponComponent> CombatMGMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x492B257C, "weapon_combatmg_mk2"),
            new WeaponComponent("Extended Clip", 0x17DF42E9, "weapon_combatmg_mk2"),
            new WeaponComponent("Tracer Rounds", 0xF6649745, "weapon_combatmg_mk2"),
            new WeaponComponent("Incendiary Rounds", 0xC326BDBA, "weapon_combatmg_mk2"),
            new WeaponComponent("Armor Piercing Rounds", 0x29882423, "weapon_combatmg_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0x57EF1CC8, "weapon_combatmg_mk2"),
            new WeaponComponent("Grip", 0x9D65907A, "weapon_combatmg_mk2"),
            new WeaponComponent("Holographic Sight", 0x420FD713, "weapon_combatmg_mk2"),
            new WeaponComponent("Medium Scope", 0x3F3C8181, "weapon_combatmg_mk2"),
            new WeaponComponent("Large Scope", 0xC66B6542, "weapon_combatmg_mk2"),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4, "weapon_combatmg_mk2"),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B, "weapon_combatmg_mk2"),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF, "weapon_combatmg_mk2"),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC, "weapon_combatmg_mk2"),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A, "weapon_combatmg_mk2"),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC, "weapon_combatmg_mk2"),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE, "weapon_combatmg_mk2"),
            new WeaponComponent("Default Barrel", 0xC34EF234, "weapon_combatmg_mk2"),
            new WeaponComponent("Heavy Barrel", 0xB5E2575B, "weapon_combatmg_mk2"),
            new WeaponComponent("Digital Camo", 0x4A768CB5, "weapon_combatmg_mk2"),
            new WeaponComponent("Brushstroke Camo", 0xCCE06BBD, "weapon_combatmg_mk2"),
            new WeaponComponent("Woodland Camo", 0xBE94CF26, "weapon_combatmg_mk2"),
            new WeaponComponent("Skull", 0x7609BE11, "weapon_combatmg_mk2"),
            new WeaponComponent("Sessanta Nove", 0x48AF6351, "weapon_combatmg_mk2"),
            new WeaponComponent("Perseus", 0x9186750A, "weapon_combatmg_mk2"),
            new WeaponComponent("Leopard", 0x84555AA8, "weapon_combatmg_mk2"),
            new WeaponComponent("Zebra", 0x1B4C088B, "weapon_combatmg_mk2"),
            new WeaponComponent("Geometric", 0xE046DFC, "weapon_combatmg_mk2"),
            new WeaponComponent("Boom!", 0x28B536E, "weapon_combatmg_mk2"),
            new WeaponComponent("Patriotic", 0xD703C94D, "weapon_combatmg_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatmg_mk2", 200, WeaponCategory.LMG, 4, 0xDBBD7280, false, true, false) { PossibleComponents = CombatMGMK2Components });

        List<WeaponComponent> GusenbergComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x1CE5A6A5, "weapon_gusenberg"),
            new WeaponComponent("Extended Clip", 0xEAC8C270, "weapon_gusenberg")
        };
        WeaponsList.Add(new WeaponInformation("weapon_gusenberg", 200, WeaponCategory.LMG, 4, 1627465347, false, true, false) { PossibleComponents = GusenbergComponents });

        //Sniper

        List<WeaponComponent> SniperRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x9BC64089, "weapon_sniperrifle"),
            new WeaponComponent("Suppressor", 0xA73D4664, "weapon_sniperrifle"),
            new WeaponComponent("Scope", 0xD2443DDC, "weapon_sniperrifle"),
            new WeaponComponent("Advanced Scope", 0xBC54DA77, "weapon_sniperrifle"),
            new WeaponComponent("Etched Wood Grip Finish", 0x4032B5E7, "weapon_sniperrifle")
        };
        WeaponsList.Add(new WeaponInformation("weapon_sniperrifle", 40, WeaponCategory.Sniper, 4, 100416529, false, true, true) { PossibleComponents = SniperRifleComponents });


        List<WeaponComponent> HeavySniperComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x476F52F4, "weapon_heavysniper"),
            new WeaponComponent("Scope", 0xD2443DDC, "weapon_heavysniper"),
            new WeaponComponent("Advanced Scope", 0xBC54DA77, "weapon_heavysniper")
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavysniper", 40, WeaponCategory.Sniper, 4, 205991906, false, true, true) { PossibleComponents = HeavySniperComponents });


        List<WeaponComponent> HeavySniperMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xFA1E1A28, "weapon_heavysniper_mk2"),
            new WeaponComponent("Extended Clip", 0x2CD8FF9D, "weapon_heavysniper_mk2"),
            new WeaponComponent("Incendiary Rounds", 0xEC0F617, "weapon_heavysniper_mk2"),
            new WeaponComponent("Armor Piercing Rounds", 0xF835D6D4, "weapon_heavysniper_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0x3BE948F6, "weapon_heavysniper_mk2"),
            new WeaponComponent("Explosive Rounds", 0x89EBDAA7, "weapon_heavysniper_mk2"),
            new WeaponComponent("Zoom Scope", 0x82C10383, "weapon_heavysniper_mk2"),
            new WeaponComponent("Advanced Scope", 0xBC54DA77, "weapon_heavysniper_mk2"),
            new WeaponComponent("Night Vision Scope", 0xB68010B0, "weapon_heavysniper_mk2"),
            new WeaponComponent("Thermal Scope", 0x2E43DA41, "weapon_heavysniper_mk2"),
            new WeaponComponent("Suppressor", 0xAC42DF71, "weapon_heavysniper_mk2"),
            new WeaponComponent("Squared Muzzle Brake", 0x5F7DCE4D, "weapon_heavysniper_mk2"),
            new WeaponComponent("Bell-End Muzzle Brake", 0x6927E1A1, "weapon_heavysniper_mk2"),
            new WeaponComponent("Default Barrel", 0x909630B7, "weapon_heavysniper_mk2"),
            new WeaponComponent("Heavy Barrel", 0x108AB09E, "weapon_heavysniper_mk2"),
            new WeaponComponent("Digital Camo", 0xF8337D02, "weapon_heavysniper_mk2"),
            new WeaponComponent("Brushstroke Camo", 0xC5BEDD65, "weapon_heavysniper_mk2"),
            new WeaponComponent("Woodland Camo", 0xE9712475, "weapon_heavysniper_mk2"),
            new WeaponComponent("Skull", 0x13AA78E7, "weapon_heavysniper_mk2"),
            new WeaponComponent("Sessanta Nove", 0x26591E50, "weapon_heavysniper_mk2"),
            new WeaponComponent("Perseus", 0x302731EC, "weapon_heavysniper_mk2"),
            new WeaponComponent("Leopard", 0xAC722A78, "weapon_heavysniper_mk2"),
            new WeaponComponent("Zebra", 0xBEA4CEDD, "weapon_heavysniper_mk2"),
            new WeaponComponent("Geometric", 0xCD776C82, "weapon_heavysniper_mk2"),
            new WeaponComponent("Boom!", 0xABC5ACC7, "weapon_heavysniper_mk2"),
            new WeaponComponent("Patriotic", 0x6C32D2EB, "weapon_heavysniper_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavysniper_mk2", 40, WeaponCategory.Sniper, 4, 0xA914799, false, true, true) { PossibleComponents = HeavySniperMK2Components });


        List<WeaponComponent> MarksmanRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xD83B4141, "weapon_marksmanrifle"),
            new WeaponComponent("Extended Clip", 0xCCFD2AC5, "weapon_marksmanrifle"),
            new WeaponComponent("Scope", 0x1C221B1A, "weapon_marksmanrifle"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_marksmanrifle"),
            new WeaponComponent("Suppressor", 0x837445AA, "weapon_marksmanrifle"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_marksmanrifle"),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x161E9241, "weapon_marksmanrifle")
        };
        WeaponsList.Add(new WeaponInformation("weapon_marksmanrifle", 40, WeaponCategory.Sniper, 4, 3342088282, false, true, true) { PossibleComponents = MarksmanRifleComponents });


        List<WeaponComponent> MarksmanRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94E12DCE, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Extended Clip", 0xE6CFD1AA, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Tracer Rounds", 0xD77A22D2, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Incendiary Rounds", 0x6DD7A86E, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Armor Piercing Rounds", 0xF46FD079, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Full Metal Jacket Rounds", 0xE14A9ED3, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Holographic Sight", 0x420FD713, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Large Scope", 0xC66B6542, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Zoom Scope", 0x5B1C713C, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Suppressor", 0x837445AA, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Default Barrel", 0x381B5D89, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Heavy Barrel", 0x68373DDC, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Grip", 0x9D65907A, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Digital Camo", 0x9094FBA0, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Brushstroke Camo", 0x7320F4B2, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Woodland Camo", 0x60CF500F, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Skull", 0xFE668B3F, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Sessanta Nove", 0xF3757559, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Perseus", 0x193B40E8, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Leopard", 0x107D2F6C, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Zebra", 0xC4E91841, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Geometric", 0x9BB1C5D3, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Boom!", 0x3B61040B, "weapon_marksmanrifle_mk2"),
            new WeaponComponent("Boom!", 0xB7A316DA, "weapon_marksmanrifle_mk2")
        };
        WeaponsList.Add(new WeaponInformation("weapon_marksmanrifle_mk2", 40, WeaponCategory.Sniper, 4, 0x6A6C02E0, false, true, true) { PossibleComponents = MarksmanRifleMK2Components });

        //Heavy
        WeaponsList.Add(new WeaponInformation("weapon_rpg", 3, WeaponCategory.Heavy, 4, 2982836145, false, true, false));

        List<WeaponComponent> GrenadeLauncherComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x11AE5C97, "weapon_grenadelauncher"),
            new WeaponComponent("Flashlight", 0x7BC4CDDC, "weapon_grenadelauncher"),
            new WeaponComponent("Grip", 0xC164F53, "weapon_grenadelauncher"),
            new WeaponComponent("Scope", 0xAA2C45B4, "weapon_grenadelauncher")
        };
        WeaponsList.Add(new WeaponInformation("weapon_grenadelauncher", 32, WeaponCategory.Heavy, 4, 2726580491, false, true, false) { PossibleComponents = GrenadeLauncherComponents });

        WeaponsList.Add(new WeaponInformation("weapon_grenadelauncher_smoke", 32, WeaponCategory.Heavy, 4, 1305664598, false, true, false) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_minigun", 500, WeaponCategory.Heavy, 4, 1119849093, false, true, false));
        WeaponsList.Add(new WeaponInformation("weapon_firework", 20, WeaponCategory.Heavy, 4, 0x7F7497E5, false, true, false) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_railgun", 50, WeaponCategory.Heavy, 4, 0x6D544C99, false, true, false) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_hominglauncher", 3, WeaponCategory.Heavy, 4, 0x63AB0442, false, true, false) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_compactlauncher", 10, WeaponCategory.Heavy, 4, 125959754, false, true, false));
        WeaponsList.Add(new WeaponInformation("weapon_rayminigun", 50, WeaponCategory.Heavy, 4, 0xB62D1F67, false, true, false) { IsRegular = false });

        //Throwable
        WeaponsList.Add(new WeaponInformation("weapon_grenade", 10, WeaponCategory.Throwable, 2, 0x93E220BD, false, false, false));


        WeaponsList.Add(new WeaponInformation("weapon_bzgas", 10, WeaponCategory.Throwable, 2, 0xA0973D5E, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_molotov", 10, WeaponCategory.Throwable, 2, 0x24B17070, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_stickybomb", 10, WeaponCategory.Throwable, 2, 0x2C3731D9, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_proxmine", 10, WeaponCategory.Throwable, 2, 0xAB564B93, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_snowball", 10, WeaponCategory.Throwable, 2, 0x787F0BB, false, false, false) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_pipebomb", 10, WeaponCategory.Throwable, 2, 0xBA45E8B8, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_ball", 10, WeaponCategory.Throwable, 2, 0x23C9F95C, false, false, false) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_smokegrenade", 10, WeaponCategory.Throwable, 2, 0xFDBC8A50, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_flare", 10, WeaponCategory.Throwable, 2, 0x497FACC3, false, false, false));

        WeaponsList.Add(new WeaponInformation("weapon_petrolcan", 2, WeaponCategory.Misc, 0, 0x34A67B97, false, false, false));
        WeaponsList.Add(new WeaponInformation("gadget_parachute", 10, WeaponCategory.Misc, 0, 0xFBAB5776, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_fireextinguisher", 10, WeaponCategory.Misc, 0, 0x060EC506, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_hazardcan", 10, WeaponCategory.Misc, 0, 0xBA536372, false, false, false));

    }
}
