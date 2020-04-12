using ExtensionsMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

public static class GTAWeapons
{
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Weapons.xml";
    public static List<GTAWeapon> WeaponsList;
   // public static List<GTAWeapon.WeaponComponent> WeaponComponentsList;

    public static void Initialize()
    {
        ReadConfig();
    }
    public static void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            WeaponsList = LosSantosRED.DeserializeParams<GTAWeapon>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            LosSantosRED.SerializeParams(WeaponsList, ConfigFileName);
        }
    }

    public static void Dispose()
    {

    }
    private static void DefaultConfig()
    {
        WeaponsList = new List<GTAWeapon>
        {

            //Melee
            new GTAWeapon("weapon_dagger", 0, GTAWeapon.WeaponCategory.Melee, 0, 2460120199, false, false, false, true),
            new GTAWeapon("weapon_bat", 0, GTAWeapon.WeaponCategory.Melee, 0, 2508868239, false, false, false, true),
            new GTAWeapon("weapon_bottle", 0, GTAWeapon.WeaponCategory.Melee, 0, 4192643659, false, false, false, true),
            new GTAWeapon("weapon_crowbar", 0, GTAWeapon.WeaponCategory.Melee, 0, 2227010557, false, false, false, true),
            new GTAWeapon("weapon_flashlight", 0, GTAWeapon.WeaponCategory.Melee, 0, 2343591895, false, false, false, true),
            new GTAWeapon("weapon_golfclub", 0, GTAWeapon.WeaponCategory.Melee, 0, 1141786504, false, false, false, true),
            new GTAWeapon("weapon_hammer", 0, GTAWeapon.WeaponCategory.Melee, 0, 1317494643, false, false, false, true),
            new GTAWeapon("weapon_hatchet", 0, GTAWeapon.WeaponCategory.Melee, 0, 4191993645, false, false, false, true)
        };

        List<GTAWeapon.WeaponComponent> KnuckleComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Base Model", "COMPONENT_KNUCKLE_VARMOD_BASE", 0xF3462F33, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The Pimp", "COMPONENT_KNUCKLE_VARMOD_PIMP", 0xC613F685, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The Ballas", "COMPONENT_KNUCKLE_VARMOD_BALLAS", 0xEED9FD63, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The Hustler", "COMPONENT_KNUCKLE_VARMOD_DOLLAR", 0x50910C31, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The Rock", "COMPONENT_KNUCKLE_VARMOD_DIAMOND", 0x9761D9DC, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The Hater", "COMPONENT_KNUCKLE_VARMOD_HATE", 0x7DECFE30, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The Lover", "COMPONENT_KNUCKLE_VARMOD_LOVE", 0x3F4E8AA6, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The Player", "COMPONENT_KNUCKLE_VARMOD_PLAYER", 0x8B808BB, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The King", "COMPONENT_KNUCKLE_VARMOD_KING", 0xE28BABEF, false, "weapon_knuckle"),
            new GTAWeapon.WeaponComponent("The Vagos", "COMPONENT_KNUCKLE_VARMOD_VAGOS", 0x7AF3F785, false, "weapon_knuckle")
        };
        WeaponsList.Add(new GTAWeapon("weapon_knuckle", 0, GTAWeapon.WeaponCategory.Melee, 0, 3638508604, false, false, false, true) { IsRegularGun = false, PossibleComponents = KnuckleComponents });
        WeaponsList.Add(new GTAWeapon("weapon_knife", 0, GTAWeapon.WeaponCategory.Melee, 0, 2578778090, false, false, false, true));
        WeaponsList.Add(new GTAWeapon("weapon_machete", 0, GTAWeapon.WeaponCategory.Melee, 0, 3713923289, false, false, false, true));

        List<GTAWeapon.WeaponComponent> SwitchbladeComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Handle", "COMPONENT_SWITCHBLADE_VARMOD_BASE", 0x9137A500, false, "weapon_switchblade"),
            new GTAWeapon.WeaponComponent("VIP Variant", "COMPONENT_SWITCHBLADE_VARMOD_VAR1", 0x5B3E7DB6, false, "weapon_switchblade"),
            new GTAWeapon.WeaponComponent("Bodyguard Variant", "COMPONENT_SWITCHBLADE_VARMOD_VAR2", 0xE7939662, false, "weapon_switchblade")
        };
        WeaponsList.Add(new GTAWeapon("weapon_switchblade", 0, GTAWeapon.WeaponCategory.Melee, 0, 3756226112, false, false, false, false) { PossibleComponents = SwitchbladeComponents });

        WeaponsList.Add(new GTAWeapon("weapon_nightstick", 0, GTAWeapon.WeaponCategory.Melee, 0, 1737195953, false, false, false, true));
        WeaponsList.Add(new GTAWeapon("weapon_wrench", 0, GTAWeapon.WeaponCategory.Melee, 0, 0x19044EE0, false, false, false, true));
        WeaponsList.Add(new GTAWeapon("weapon_battleaxe", 0, GTAWeapon.WeaponCategory.Melee, 0, 3441901897, false, false, false, true) { IsRegularGun = false });
        WeaponsList.Add(new GTAWeapon("weapon_poolcue", 0, GTAWeapon.WeaponCategory.Melee, 0, 0x94117305, false, false, false, true));
        WeaponsList.Add(new GTAWeapon("weapon_stone_hatchet", 0, GTAWeapon.WeaponCategory.Melee, 0, 0x3813FC08, false, false, false, true) { IsRegularGun = false });
        //Pistol
        List<GTAWeapon.WeaponComponent> PistolComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_PISTOL_CLIP_01", 0xFED0FD71, false, "weapon_pistol"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_CLIP_02", 0xED265A1C, false, "weapon_pistol"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_pistol"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP_02", 0x65EA7EBB, false, "weapon_pistol"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_PISTOL_VARMOD_LUXE", 0xD7391086, false, "weapon_pistol")
        };
        WeaponsList.Add(new GTAWeapon("weapon_pistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 453432689, true, true, false, true) { PossibleComponents = PistolComponents });

        List<GTAWeapon.WeaponComponent> PistolMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_PISTOL_MK2_CLIP_01", 0x94F42D62, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_MK2_CLIP_02", 0x5ED6C128, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_PISTOL_MK2_CLIP_TRACER", 0x25CAAEAF, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_PISTOL_MK2_CLIP_INCENDIARY", 0x2BBD7A3A, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Hollow Point Rounds", "COMPONENT_PISTOL_MK2_CLIP_HOLLOWPOINT", 0x85FEA109, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_PISTOL_MK2_CLIP_FMJ", 0x4F37DF2A, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Mounted Scope", "COMPONENT_AT_PI_RAIL", 0x8ED4BB70, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH_02", 0x43FD595B, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP_02", 0x65EA7EBB, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Compensator", "COMPONENT_AT_PI_COMP", 0x21E34793, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO", 0x5C6C749C, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_PISTOL_MK2_CAMO_02", 0x15F7A390, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_PISTOL_MK2_CAMO_03", 0x968E24DB, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_PISTOL_MK2_CAMO_04", 0x17BFA99, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_PISTOL_MK2_CAMO_05", 0xF2685C72, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_PISTOL_MK2_CAMO_06", 0xDD2231E6, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_PISTOL_MK2_CAMO_07", 0xBB43EE76, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_PISTOL_MK2_CAMO_08", 0x4D901310, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_PISTOL_MK2_CAMO_09", 0x5F31B653, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_PISTOL_MK2_CAMO_10", 0x697E19A0, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_PISTOL_MK2_CAMO_IND_01", 0x930CB951, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_SLIDE", 0xB4FC92B0, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_02_SLIDE", 0x1A1F1260, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_03_SLIDE", 0xE4E00B70, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_04_SLIDE", 0x2C298B2B, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_05_SLIDE", 0xDFB79725, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_06_SLIDE", 0x6BD7228C, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_07_SLIDE", 0x9DDBCF8C, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_08_SLIDE", 0xB319A52C, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_09_SLIDE", 0xC6836E12, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_10_SLIDE", 0x43B1B173, false, "weapon_pistol_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_PISTOL_MK2_CAMO_IND_01_SLIDE", 0x4ABDA3FA, false, "weapon_pistol_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_pistol_mk2", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0xBFE256D4, true, true, false, true) { PossibleComponents = PistolMK2Components });

        List<GTAWeapon.WeaponComponent> CombatPistolComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_COMBATPISTOL_CLIP_01", 0x721B079, false, "weapon_combatpistol"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_COMBATPISTOL_CLIP_02", 0xD67B4F2D, false, "weapon_combatpistol"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_combatpistol"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_combatpistol"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_COMBATPISTOL_VARMOD_LOWRIDER", 0xC6654D72, false, "weapon_combatpistol")
        };
        WeaponsList.Add(new GTAWeapon("weapon_combatpistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 1593441988, true, true, false, true) { PossibleComponents = CombatPistolComponents });


        List<GTAWeapon.WeaponComponent> APPistolComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_APPISTOL_CLIP_01", 0x31C4B22A, false, "weapon_appistol"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_APPISTOL_CLIP_02", 0x249A17D5, false, "weapon_appistol"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_appistol"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_appistol"),
            new GTAWeapon.WeaponComponent("Gilded Gun Metal Finish", "COMPONENT_APPISTOL_VARMOD_LUXE", 0x9B76C72C, false, "weapon_appistol")
        };
        WeaponsList.Add(new GTAWeapon("weapon_appistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 584646201, false, true, false, false) { PossibleComponents = APPistolComponents });

        WeaponsList.Add(new GTAWeapon("weapon_stungun", 0, GTAWeapon.WeaponCategory.Pistol, 1, 911657153, false, true, false, true) { IsRegularGun = false });
        List<GTAWeapon.WeaponComponent> Pistol50Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_PISTOL50_CLIP_01", 0x2297BE19, false, "weapon_pistol50"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_PISTOL50_CLIP_02", 0xD9D3AC92, false, "weapon_pistol50"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_pistol50"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_pistol50"),
            new GTAWeapon.WeaponComponent("Platinum Pearl Deluxe Finish", "COMPONENT_PISTOL50_VARMOD_LUXE", 0x77B8AB2F, false, "weapon_pistol50")
        };
        WeaponsList.Add(new GTAWeapon("weapon_pistol50", 60, GTAWeapon.WeaponCategory.Pistol, 1, 2578377531, false, true, false, true) { PossibleComponents = Pistol50Components });

        List<GTAWeapon.WeaponComponent> SNSPistolComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_SNSPISTOL_CLIP_01", 0xF8802ED9, false, "weapon_snspistol"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_SNSPISTOL_CLIP_02", 0x7B0033B3, false, "weapon_snspistol"),
            new GTAWeapon.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_SNSPISTOL_VARMOD_LOWRIDER", 0x8033ECAF, false, "weapon_snspistol")
        };
        WeaponsList.Add(new GTAWeapon("weapon_snspistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 3218215474, false, true, false, true) { PossibleComponents = SNSPistolComponents });

        List<GTAWeapon.WeaponComponent> SNSPistolMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_SNSPISTOL_MK2_CLIP_01", 0x1466CE6, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_SNSPISTOL_MK2_CLIP_02", 0xCE8C0772, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_SNSPISTOL_MK2_CLIP_TRACER", 0x902DA26E, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_SNSPISTOL_MK2_CLIP_INCENDIARY", 0xE6AD5F79, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Hollow Point Rounds", "COMPONENT_SNSPISTOL_MK2_CLIP_HOLLOWPOINT", 0x8D107402, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_SNSPISTOL_MK2_CLIP_FMJ", 0xC111EB26, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH_03", 0x4A4965F3, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Mounted Scope", "COMPONENT_AT_PI_RAIL_02", 0x47DE9258, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP_02", 0x65EA7EBB, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Compensator", "COMPONENT_AT_PI_COMP_02", 0xAA8283BF, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_SNSPISTOL_MK2_CAMO", 0xF7BEEDD, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_02", 0x8A612EF6, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_03", 0x76FA8829, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_SNSPISTOL_MK2_CAMO_04", 0xA93C6CAC, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_SNSPISTOL_MK2_CAMO_05", 0x9C905354, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_SNSPISTOL_MK2_CAMO_06", 0x4DFA3621, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_SNSPISTOL_MK2_CAMO_07", 0x42E91FFF, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_SNSPISTOL_MK2_CAMO_08", 0x54A8437D, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_SNSPISTOL_MK2_CAMO_09", 0x68C2746, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_SNSPISTOL_MK2_CAMO_10", 0x2366E467, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01", 0x441882E6, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_SLIDE", 0xE7EE68EA, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_02_SLIDE", 0x29366D21, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_03_SLIDE", 0x3ADE514B, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_SNSPISTOL_MK2_CAMO_04_SLIDE", 0xE64513E9, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_SNSPISTOL_MK2_CAMO_05_SLIDE", 0xCD7AEB9A, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_SNSPISTOL_MK2_CAMO_06_SLIDE", 0xFA7B27A6, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_SNSPISTOL_MK2_CAMO_07_SLIDE", 0xE285CA9A, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_SNSPISTOL_MK2_CAMO_08_SLIDE", 0x2B904B19, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_SNSPISTOL_MK2_CAMO_09_SLIDE", 0x22C24F9C, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_SNSPISTOL_MK2_CAMO_10_SLIDE", 0x8D0D5ECD, false, "weapon_snspistol_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01_SLIDE", 0x1F07150A, false, "weapon_snspistol_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_snspistol_mk2", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0x88374054, false, true, false, true));

        List<GTAWeapon.WeaponComponent> HeavyPistolComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_HEAVYPISTOL_CLIP_01", 0xD4A969A, false, "weapon_heavypistol"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_HEAVYPISTOL_CLIP_02", 0x64F9C62B, false, "weapon_heavypistol"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_heavypistol"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_heavypistol"),
            new GTAWeapon.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_HEAVYPISTOL_VARMOD_LUXE", 0x7A6A7B7B, false, "weapon_heavypistol")
        };
        WeaponsList.Add(new GTAWeapon("weapon_heavypistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 3523564046, true, true, false, true) { PossibleComponents = HeavyPistolComponents });

        List<GTAWeapon.WeaponComponent> VintagePistolComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_VINTAGEPISTOL_CLIP_01", 0x45A3B6BB, false, "weapon_vintagepistol"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_VINTAGEPISTOL_CLIP_02", 0x33BA12E8, false, "weapon_vintagepistol"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_vintagepistol")
        };
        WeaponsList.Add(new GTAWeapon("weapon_vintagepistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 137902532, false, true, false, true) { PossibleComponents = VintagePistolComponents });
        

        WeaponsList.Add(new GTAWeapon("weapon_flaregun", 60, GTAWeapon.WeaponCategory.Pistol, 1, 1198879012, false, true, false, true) { IsRegularGun = false });
        WeaponsList.Add(new GTAWeapon("weapon_marksmanpistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 3696079510, false, true, false, true) { IsRegularGun = false });

        List<GTAWeapon.WeaponComponent> RevolverComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("VIP Variant", "COMPONENT_REVOLVER_VARMOD_BOSS", 0x16EE3040, false, "weapon_revolver"),
            new GTAWeapon.WeaponComponent("Bodyguard Variant", "COMPONENT_REVOLVER_VARMOD_GOON", 0x9493B80D, false, "weapon_revolver"),
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_REVOLVER_CLIP_01", 0xE9867CE3, false, "weapon_revolver")
        };
        WeaponsList.Add(new GTAWeapon("weapon_revolver", 60, GTAWeapon.WeaponCategory.Pistol, 1, 3249783761, false, true, false, true) { PossibleComponents = RevolverComponents });

        List<GTAWeapon.WeaponComponent> RevolverMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Rounds", "COMPONENT_REVOLVER_MK2_CLIP_01", 0xBA23D8BE, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_REVOLVER_MK2_CLIP_TRACER", 0xC6D8E476, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_REVOLVER_MK2_CLIP_INCENDIARY", 0xEFBF25, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Hollow Point Rounds", "COMPONENT_REVOLVER_MK2_CLIP_HOLLOWPOINT", 0x10F42E8F, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_REVOLVER_MK2_CLIP_FMJ", 0xDC8BA3F, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Compensator", "COMPONENT_AT_PI_COMP_03", 0x27077CCB, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_REVOLVER_MK2_CAMO", 0xC03FED9F, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_REVOLVER_MK2_CAMO_02", 0xB5DE24, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_REVOLVER_MK2_CAMO_03", 0xA7FF1B8, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_REVOLVER_MK2_CAMO_04", 0xF2E24289, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_REVOLVER_MK2_CAMO_05", 0x11317F27, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_REVOLVER_MK2_CAMO_06", 0x17C30C42, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_REVOLVER_MK2_CAMO_07", 0x257927AE, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_REVOLVER_MK2_CAMO_08", 0x37304B1C, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_REVOLVER_MK2_CAMO_09", 0x48DAEE71, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_REVOLVER_MK2_CAMO_10", 0x20ED9B5B, false, "weapon_revolver_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_REVOLVER_MK2_CAMO_IND_01", 0xD951E867, false, "weapon_revolver_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_revolver_mk2", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0xCB96392F, false, true, false, true));
        
        WeaponsList.Add(new GTAWeapon("weapon_doubleaction", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0x97EA20B8, false, true, false, true));
        List<GTAWeapon.WeaponComponent> RayGunComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Festive tint", "COMPONENT_RAYPISTOL_VARMOD_XMAS18", 0xD7DBF707, false, "weapon_raypistol")
        };
        WeaponsList.Add(new GTAWeapon("weapon_raypistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0xAF3696A1, false, true, false, false) { IsRegularGun = false, PossibleComponents = RayGunComponents });

        //Shotgun
        List<GTAWeapon.WeaponComponent> PumpShotgunComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_SR_SUPP", 0xE608B35E, false, "weapon_pumpshotgun"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_PUMPSHOTGUN_VARMOD_LOWRIDER", 0xA2D79DDB, false, "weapon_pumpshotgun")
        };
        WeaponsList.Add(new GTAWeapon("weapon_pumpshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 487013001, true, false, true, true) { PossibleComponents = PumpShotgunComponents });

        List<GTAWeapon.WeaponComponent> PumpShotgunMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Shells", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_01", 0xCD940141, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Dragon's Breath Shells", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_INCENDIARY", 0x9F8A1BF5, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Steel Buckshot Shells", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_ARMORPIERCING", 0x4E65B425, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Flechette Shells", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_HOLLOWPOINT", 0xE9582927, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Explosive Slugs", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_EXPLOSIVE", 0x3BE4465D, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Medium Scope", "COMPONENT_AT_SCOPE_SMALL_MK2", 0x3F3C8181, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_SR_SUPP_03", 0xAC42DF71, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Squared Muzzle Brake", "COMPONENT_AT_MUZZLE_08", 0x5F7DCE4D, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_PUMPSHOTGUN_MK2_CAMO", 0xE3BD9E44, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_02", 0x17148F9B, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_03", 0x24D22B16, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_04", 0xF2BEC6F0, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_05", 0x85627D, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_06", 0xDC2919C5, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_07", 0xE184247B, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_08", 0xD8EF9356, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_09", 0xEF29BFCA, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_10", 0x67AEB165, false, "weapon_pumpshotgun_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_IND_01", 0x46411A1D, false, "weapon_pumpshotgun_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_pumpshotgun_mk2", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 0x555AF99A, true, false, true, true) { PossibleComponents = PumpShotgunMK2Components });

        List<GTAWeapon.WeaponComponent> SawnOffShotgunComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Gilded Gun Metal Finish", "COMPONENT_SAWNOFFSHOTGUN_VARMOD_LUXE", 0x85A64DF9, false, "weapon_sawnoffshotgun")
        };
        WeaponsList.Add(new GTAWeapon("weapon_sawnoffshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 2017895192, false, false, true, false) { PossibleComponents = SawnOffShotgunComponents });

        List<GTAWeapon.WeaponComponent> AssaultShotgunComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_ASSAULTSHOTGUN_CLIP_01", 0x94E81BC7, false, "weapon_assaultshotgun"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_ASSAULTSHOTGUN_CLIP_02", 0x86BD7F72, false, "weapon_assaultshotgun"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_assaultshotgun"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_assaultshotgun"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_assaultshotgun")
        };
        WeaponsList.Add(new GTAWeapon("weapon_assaultshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 3800352039, false, false, true, false) { PossibleComponents = AssaultShotgunComponents });

        List<GTAWeapon.WeaponComponent> BullpupShotgunComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_bullpupshotgun"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_bullpupshotgun"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_bullpupshotgun")
        };
        WeaponsList.Add(new GTAWeapon("weapon_bullpupshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 2640438543, false, false, true, true) { PossibleComponents = BullpupShotgunComponents });
        
        WeaponsList.Add(new GTAWeapon("weapon_musket", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 2828843422, false, false, true, true) { IsRegularGun = false });

        List<GTAWeapon.WeaponComponent> HeavyShotgunComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_HEAVYSHOTGUN_CLIP_01", 0x324F2D5F, false, "weapon_heavyshotgun"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_HEAVYSHOTGUN_CLIP_02", 0x971CF6FD, false, "weapon_heavyshotgun"),
            new GTAWeapon.WeaponComponent("Drum Magazine", "COMPONENT_HEAVYSHOTGUN_CLIP_03", 0x88C7DA53, false, "weapon_heavyshotgun"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_heavyshotgun"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_heavyshotgun"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_heavyshotgun")
        };
        WeaponsList.Add(new GTAWeapon("weapon_heavyshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 984333226, false, false, true, true) { PossibleComponents = HeavyShotgunComponents });
        
        WeaponsList.Add(new GTAWeapon("weapon_dbshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 4019527611, false, false, true, false));
        WeaponsList.Add(new GTAWeapon("weapon_autoshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 317205821, false, false, true, false));
        //SMG

        List<GTAWeapon.WeaponComponent> MicroSMGComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_MICROSMG_CLIP_01", 0xCB48AEF0, false, "weapon_microsmg"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MICROSMG_CLIP_02", 0x10E6BA2B, false, "weapon_microsmg"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_microsmg"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MACRO", 0x9D2FBF29, false, "weapon_microsmg"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_microsmg"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_MICROSMG_VARMOD_LUXE", 0x487AAE09, false, "weapon_microsmg")
        };      
        WeaponsList.Add(new GTAWeapon("weapon_microsmg", 32, GTAWeapon.WeaponCategory.SMG, 2, 324215364, false, true, false, false) { PossibleComponents = MicroSMGComponents });


        List<GTAWeapon.WeaponComponent> SMGComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_SMG_CLIP_01", 0x26574997, false, "weapon_smg"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_SMG_CLIP_02", 0x350966FB, false, "weapon_smg"),
            new GTAWeapon.WeaponComponent("Drum Magazine", "COMPONENT_SMG_CLIP_03", 0x79C77076, false, "weapon_smg"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_smg"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MACRO_02", 0x3CC6BA57, false, "weapon_smg"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_smg"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_SMG_VARMOD_LUXE", 0x27872C90, false, "weapon_smg")
        };
        WeaponsList.Add(new GTAWeapon("weapon_smg", 32, GTAWeapon.WeaponCategory.SMG, 2, 736523883, false, false, true, false) { PossibleComponents = SMGComponents });


        List<GTAWeapon.WeaponComponent> SMGMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_SMG_MK2_CLIP_01", 0x4C24806E, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_SMG_MK2_CLIP_02", 0xB9835B2E, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_SMG_MK2_CLIP_TRACER", 0x7FEA36EC, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_SMG_MK2_CLIP_INCENDIARY", 0xD99222E5, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Hollow Point Rounds", "COMPONENT_SMG_MK2_CLIP_HOLLOWPOINT", 0x3A1BD6FA, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_SMG_MK2_CLIP_FMJ", 0xB5A715F, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS_SMG", 0x9FDB5652, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_02_SMG_MK2", 0xE502AB6B, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Medium Scope", "COMPONENT_AT_SCOPE_SMALL_SMG_MK2", 0x3DECC7DA, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Default Barrel", "COMPONENT_AT_SB_BARREL_01", 0xD9103EE1, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Barrel", "COMPONENT_AT_SB_BARREL_02", 0xA564D78B, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_SMG_MK2_CAMO", 0xC4979067, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_SMG_MK2_CAMO_02", 0x3815A945, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_SMG_MK2_CAMO_03", 0x4B4B4FB0, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_SMG_MK2_CAMO_04", 0xEC729200, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_SMG_MK2_CAMO_05", 0x48F64B22, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_SMG_MK2_CAMO_06", 0x35992468, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_SMG_MK2_CAMO_07", 0x24B782A5, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_SMG_MK2_CAMO_08", 0xA2E67F01, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_SMG_MK2_CAMO_09", 0x2218FD68, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_SMG_MK2_CAMO_10", 0x45C5C3C5, false, "weapon_smg_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_SMG_MK2_CAMO_IND_01", 0x399D558F, false, "weapon_smg_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_smg_mk2", 32, GTAWeapon.WeaponCategory.SMG, 2, 0x78A97CD0, false, false, true, false) { PossibleComponents = SMGMK2Components });


        List<GTAWeapon.WeaponComponent> AssaultSMGComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_ASSAULTSMG_CLIP_01", 0x8D1307B0, false, "weapon_assaultsmg"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_ASSAULTSMG_CLIP_02", 0xBB46E417, false, "weapon_assaultsmg"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_assaultsmg"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MACRO", 0x9D2FBF29, false, "weapon_assaultsmg"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_assaultsmg"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_ASSAULTSMG_VARMOD_LOWRIDER", 0x278C78AF, false, "weapon_assaultsmg")
        };
        WeaponsList.Add(new GTAWeapon("weapon_assaultsmg", 32, GTAWeapon.WeaponCategory.SMG, 2, 4024951519, false, false, true, false) { PossibleComponents = AssaultSMGComponents });


        List<GTAWeapon.WeaponComponent> CombatPDWComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_COMBATPDW_CLIP_01", 0x4317F19E, false, "weapon_combatpdw"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_COMBATPDW_CLIP_02", 0x334A5203, false, "weapon_combatpdw"),
            new GTAWeapon.WeaponComponent("Drum Magazine", "COMPONENT_COMBATPDW_CLIP_03", 0x6EB8C8DB, false, "weapon_combatpdw"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_combatpdw"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatpdw"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_combatpdw")
        };
        WeaponsList.Add(new GTAWeapon("weapon_combatpdw", 32, GTAWeapon.WeaponCategory.SMG, 2, 171789620, true, false, true, false) { PossibleComponents = CombatPDWComponents });



        List<GTAWeapon.WeaponComponent> MachinePistolComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_MACHINEPISTOL_CLIP_01", 0x476E85FF, false, "weapon_machinepistol"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MACHINEPISTOL_CLIP_02", 0xB92C6979, false, "weapon_machinepistol"),
            new GTAWeapon.WeaponComponent("Drum Magazine", "COMPONENT_MACHINEPISTOL_CLIP_03", 0xA9E9CAF4, false, "weapon_machinepistol"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_machinepistol")
        };
        WeaponsList.Add(new GTAWeapon("weapon_machinepistol", 32, GTAWeapon.WeaponCategory.SMG, 2, 3675956304, false, true, false, false) { PossibleComponents = MachinePistolComponents });



        List<GTAWeapon.WeaponComponent> MiniSMGComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_MINISMG_CLIP_01", 0x84C8B2D3, false, "weapon_minismg"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MINISMG_CLIP_02", 0x937ED0B7, false, "weapon_minismg")
        };
        WeaponsList.Add(new GTAWeapon("weapon_minismg", 32, GTAWeapon.WeaponCategory.SMG, 2, 3173288789, false, true, false, false) { PossibleComponents = MiniSMGComponents });

        WeaponsList.Add(new GTAWeapon("weapon_raycarbine", 32, GTAWeapon.WeaponCategory.SMG, 2, 0x476BF155, false, true, false, false) { IsRegularGun = false });
        //AR
        List<GTAWeapon.WeaponComponent> ARComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_ASSAULTRIFLE_CLIP_01", 0xBE5EEA16, false, "weapon_assaultrifle"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_ASSAULTRIFLE_CLIP_02", 0xB1214F9B, false, "weapon_assaultrifle"),
            new GTAWeapon.WeaponComponent("Drum Magazine", "COMPONENT_ASSAULTRIFLE_CLIP_03", 0xDBF0A53D, false, "weapon_assaultrifle"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_assaultrifle"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MACRO", 0x9D2FBF29, false, "weapon_assaultrifle"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_assaultrifle"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_assaultrifle"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_ASSAULTRIFLE_VARMOD_LUXE", 0x4EAD7533, false, "weapon_assaultrifle")
        };
        WeaponsList.Add(new GTAWeapon("weapon_assaultrifle", 120, GTAWeapon.WeaponCategory.AR, 3, 3220176749, false, false, true, false) { PossibleComponents = ARComponents });


        List<GTAWeapon.WeaponComponent> ARMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_01", 0x8610343F, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_02", 0xD12ACA6F, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_TRACER", 0xEF2C78C1, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_INCENDIARY", 0xFB70D853, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Armor Piercing Rounds", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_ARMORPIERCING", 0xA7DD1E58, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_FMJ", 0x63E0A098, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Default Barrel", "COMPONENT_AT_AR_BARREL_01", 0x43A49D26, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Barrel", "COMPONENT_AT_AR_BARREL_02", 0x5646C26A, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_ASSAULTRIFLE_MK2_CAMO", 0x911B24AF, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_02", 0x37E5444B, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_03", 0x538B7B97, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_04", 0x25789F72, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_05", 0xC5495F2D, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_06", 0xCF8B73B1, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_07", 0xA9BB2811, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_08", 0xFC674D54, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_09", 0x7C7FCD9B, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_10", 0xA5C38392, false, "weapon_assaultrifle_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_IND_01", 0xB9B15DB0, false, "weapon_assaultrifle_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_assaultrifle_mk2", 120, GTAWeapon.WeaponCategory.AR, 3, 0x394F415C, false, false, true, false) { PossibleComponents = ARMK2Components });


        List<GTAWeapon.WeaponComponent> CarbineRifleComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_CARBINERIFLE_CLIP_01", 0x9FBE33EC, false, "weapon_carbinerifle"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_CARBINERIFLE_CLIP_02", 0x91109691, false, "weapon_carbinerifle"),
            new GTAWeapon.WeaponComponent("Box Magazine", "COMPONENT_CARBINERIFLE_CLIP_03", 0xBA62E935, false, "weapon_carbinerifle"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_carbinerifle"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, false, "weapon_carbinerifle"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_carbinerifle"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_carbinerifle"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_CARBINERIFLE_VARMOD_LUXE", 0xD89B9658, false, "weapon_carbinerifle")
        };
        WeaponsList.Add(new GTAWeapon("weapon_carbinerifle", 120, GTAWeapon.WeaponCategory.AR, 3, 2210333304, true, false, true, false) { PossibleComponents = CarbineRifleComponents });


        List<GTAWeapon.WeaponComponent> CarbineRifleMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_CARBINERIFLE_MK2_CLIP_01", 0x4C7A391E, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_CARBINERIFLE_MK2_CLIP_02", 0x5DD5DBD5, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_CARBINERIFLE_MK2_CLIP_TRACER", 0x1757F566, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_CARBINERIFLE_MK2_CLIP_INCENDIARY", 0x3D25C2A7, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Armor Piercing Rounds", "COMPONENT_CARBINERIFLE_MK2_CLIP_ARMORPIERCING", 0x255D5D57, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_CARBINERIFLE_MK2_CLIP_FMJ", 0x44032F11, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Default Barrel", "COMPONENT_AT_CR_BARREL_01", 0x833637FF, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Barrel", "COMPONENT_AT_CR_BARREL_02", 0x8B3C480B, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_CARBINERIFLE_MK2_CAMO", 0x4BDD6F16, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_CARBINERIFLE_MK2_CAMO_02", 0x406A7908, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_CARBINERIFLE_MK2_CAMO_03", 0x2F3856A4, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_CARBINERIFLE_MK2_CAMO_04", 0xE50C424D, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_CARBINERIFLE_MK2_CAMO_05", 0xD37D1F2F, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_CARBINERIFLE_MK2_CAMO_06", 0x86268483, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_CARBINERIFLE_MK2_CAMO_07", 0xF420E076, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_CARBINERIFLE_MK2_CAMO_08", 0xAAE14DF8, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_CARBINERIFLE_MK2_CAMO_09", 0x9893A95D, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_CARBINERIFLE_MK2_CAMO_10", 0x6B13CD3E, false, "weapon_carbinerifle_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01", 0xDA55CD3F, false, "weapon_carbinerifle_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_carbinerifle_mk2", 120, GTAWeapon.WeaponCategory.AR, 3, 0xFAD1F1C9, true, false, true, false) { PossibleComponents = CarbineRifleMK2Components });


        List<GTAWeapon.WeaponComponent> AdvancedRifleComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_ADVANCEDRIFLE_CLIP_01", 0xFA8FA10F, false, "weapon_advancedrifle"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_ADVANCEDRIFLE_CLIP_02", 0x8EC1C979, false, "weapon_advancedrifle"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_advancedrifle"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_advancedrifle"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_advancedrifle"),
            new GTAWeapon.WeaponComponent("Gilded Gun Metal Finish", "COMPONENT_ADVANCEDRIFLE_VARMOD_LUXE", 0x377CD377, false, "weapon_advancedrifle")
        };
        WeaponsList.Add(new GTAWeapon("weapon_advancedrifle", 120, GTAWeapon.WeaponCategory.AR, 3, 2937143193, false, false, true, false) { PossibleComponents = AdvancedRifleComponents });


        List<GTAWeapon.WeaponComponent> SpecialCarbineComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_SPECIALCARBINE_CLIP_01", 0xC6C7E581, false, "weapon_specialcarbine"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_SPECIALCARBINE_CLIP_02", 0x7C8BD10E, false, "weapon_specialcarbine"),
            new GTAWeapon.WeaponComponent("Drum Magazine", "COMPONENT_SPECIALCARBINE_CLIP_03", 0x6B59AEAA, false, "weapon_specialcarbine"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_specialcarbine"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, false, "weapon_specialcarbine"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_specialcarbine"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_specialcarbine"),
            new GTAWeapon.WeaponComponent("Etched Gun Metal Finish", "COMPONENT_SPECIALCARBINE_VARMOD_LOWRIDER", 0x730154F2, false, "weapon_specialcarbine")
        };
        WeaponsList.Add(new GTAWeapon("weapon_specialcarbine", 120, GTAWeapon.WeaponCategory.AR, 3, 3231910285, false, false, true, false) { PossibleComponents = SpecialCarbineComponents });


        List<GTAWeapon.WeaponComponent> SpecialCarbineMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_SPECIALCARBINE_MK2_CLIP_01", 0x16C69281, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_SPECIALCARBINE_MK2_CLIP_02", 0xDE1FA12C, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_SPECIALCARBINE_MK2_CLIP_TRACER", 0x8765C68A, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_SPECIALCARBINE_MK2_CLIP_INCENDIARY", 0xDE011286, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Armor Piercing Rounds", "COMPONENT_SPECIALCARBINE_MK2_CLIP_ARMORPIERCING", 0x51351635, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_SPECIALCARBINE_MK2_CLIP_FMJ", 0x503DEA90, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Default Barrel", "COMPONENT_AT_SC_BARREL_01", 0xE73653A9, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Barrel", "COMPONENT_AT_SC_BARREL_02", 0xF97F783B, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_SPECIALCARBINE_MK2_CAMO", 0xD40BB53B, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_SPECIALCARBINE_MK2_CAMO_02", 0x431B238B, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_SPECIALCARBINE_MK2_CAMO_03", 0x34CF86F4, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_SPECIALCARBINE_MK2_CAMO_04", 0xB4C306DD, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_SPECIALCARBINE_MK2_CAMO_05", 0xEE677A25, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_SPECIALCARBINE_MK2_CAMO_06", 0xDF90DC78, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_SPECIALCARBINE_MK2_CAMO_07", 0xA4C31EE, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_SPECIALCARBINE_MK2_CAMO_08", 0x89CFB0F7, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_SPECIALCARBINE_MK2_CAMO_09", 0x7B82145C, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_SPECIALCARBINE_MK2_CAMO_10", 0x899CAF75, false, "weapon_specialcarbine_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_SPECIALCARBINE_MK2_CAMO_IND_01", 0x5218C819, false, "weapon_specialcarbine_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_specialcarbine_mk2", 120, GTAWeapon.WeaponCategory.AR, 3, 0x969C3D67, false, false, true, false) { PossibleComponents = SpecialCarbineMK2Components });


        List<GTAWeapon.WeaponComponent> BullpupRifleComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_BULLPUPRIFLE_CLIP_01", 0xC5A12F80, false, "weapon_bullpuprifle"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_BULLPUPRIFLE_CLIP_02", 0xB3688B0F, false, "weapon_bullpuprifle"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_bullpuprifle"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_bullpuprifle"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_bullpuprifle"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_bullpuprifle"),
            new GTAWeapon.WeaponComponent("Gilded Gun Metal Finish", "COMPONENT_BULLPUPRIFLE_VARMOD_LOW", 0xA857BC78, false, "weapon_bullpuprifle")
        };
        WeaponsList.Add(new GTAWeapon("weapon_bullpuprifle", 120, GTAWeapon.WeaponCategory.AR, 3, 2132975508, false, false, true, false) { PossibleComponents = BullpupRifleComponents });


        List<GTAWeapon.WeaponComponent> BullpulRifleMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_01", 0x18929DA, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_02", 0xEFB00628, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_TRACER", 0x822060A9, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_INCENDIARY", 0xA99CF95A, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Armor Piercing Rounds", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_ARMORPIERCING", 0xFAA7F5ED, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_FMJ", 0x43621710, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_02_MK2", 0xC7ADD105, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Medium Scope", "COMPONENT_AT_SCOPE_SMALL_MK2", 0x3F3C8181, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Default Barrel", "COMPONENT_AT_BP_BARREL_01", 0x659AC11B, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Barrel", "COMPONENT_AT_BP_BARREL_02", 0x3BF26DC7, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_BULLPUPRIFLE_MK2_CAMO", 0xAE4055B7, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_02", 0xB905ED6B, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_03", 0xA6C448E8, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_04", 0x9486246C, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_05", 0x8A390FD2, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_06", 0x2337FC5, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_07", 0xEFFFDB5E, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_08", 0xDDBDB6DA, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_09", 0xCB631225, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_10", 0xA87D541E, false, "weapon_bullpuprifle_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_IND_01", 0xC5E9AE52, false, "weapon_bullpuprifle_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_bullpuprifle_mk2", 120, GTAWeapon.WeaponCategory.AR, 3, 0x84D6FAFD, false, false, true, false) { PossibleComponents = BullpulRifleMK2Components });


        List<GTAWeapon.WeaponComponent> CompactRifleComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_COMPACTRIFLE_CLIP_01", 0x513F0A63, false, "weapon_compactrifle"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_COMPACTRIFLE_CLIP_02", 0x59FF9BF8, false, "weapon_compactrifle"),
            new GTAWeapon.WeaponComponent("Drum Magazine", "COMPONENT_COMPACTRIFLE_CLIP_03", 0xC607740E, false, "weapon_compactrifle")
        };
        WeaponsList.Add(new GTAWeapon("weapon_compactrifle", 120, GTAWeapon.WeaponCategory.AR, 3, 1649403952, false, false, true, false) { PossibleComponents = CompactRifleComponents });

        //LMG

        List<GTAWeapon.WeaponComponent> MGComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_MG_CLIP_01", 0xF434EF84, false, "weapon_mg"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MG_CLIP_02", 0x82158B47, false, "weapon_mg"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL_02", 0x3C00AFED, false, "weapon_mg"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_MG_VARMOD_LOWRIDER", 0xD6DABABE, false, "weapon_mg")
        };
        WeaponsList.Add(new GTAWeapon("weapon_mg", 200, GTAWeapon.WeaponCategory.LMG, 4, 2634544996, false, false, true, false) { PossibleComponents = MGComponents });


        List<GTAWeapon.WeaponComponent> CombatMGComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_COMBATMG_CLIP_01", 0xE1FFB34A, false, "weapon_combatmg"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_COMBATMG_CLIP_02", 0xD6C59CD6, false, "weapon_combatmg"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, false, "weapon_combatmg"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatmg"),
            new GTAWeapon.WeaponComponent("Etched Gun Metal Finish", "COMPONENT_COMBATMG_VARMOD_LOWRIDER", 0x92FECCDD, false, "weapon_combatmg")
        };
        WeaponsList.Add(new GTAWeapon("weapon_combatmg", 200, GTAWeapon.WeaponCategory.LMG, 4, 2144741730, false, false, true, false) { PossibleComponents = CombatMGComponents });


        List<GTAWeapon.WeaponComponent> CombatMGMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_COMBATMG_MK2_CLIP_01", 0x492B257C, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_COMBATMG_MK2_CLIP_02", 0x17DF42E9, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_COMBATMG_MK2_CLIP_TRACER", 0xF6649745, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_COMBATMG_MK2_CLIP_INCENDIARY", 0xC326BDBA, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Armor Piercing Rounds", "COMPONENT_COMBATMG_MK2_CLIP_ARMORPIERCING", 0x29882423, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_COMBATMG_MK2_CLIP_FMJ", 0x57EF1CC8, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Medium Scope", "COMPONENT_AT_SCOPE_SMALL_MK2", 0x3F3C8181, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Default Barrel", "COMPONENT_AT_MG_BARREL_01", 0xC34EF234, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Barrel", "COMPONENT_AT_MG_BARREL_02", 0xB5E2575B, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_COMBATMG_MK2_CAMO", 0x4A768CB5, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_COMBATMG_MK2_CAMO_02", 0xCCE06BBD, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_COMBATMG_MK2_CAMO_03", 0xBE94CF26, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_COMBATMG_MK2_CAMO_04", 0x7609BE11, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_COMBATMG_MK2_CAMO_05", 0x48AF6351, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_COMBATMG_MK2_CAMO_06", 0x9186750A, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_COMBATMG_MK2_CAMO_07", 0x84555AA8, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_COMBATMG_MK2_CAMO_08", 0x1B4C088B, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_COMBATMG_MK2_CAMO_09", 0xE046DFC, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_COMBATMG_MK2_CAMO_10", 0x28B536E, false, "weapon_combatmg_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_COMBATMG_MK2_CAMO_IND_01", 0xD703C94D, false, "weapon_combatmg_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_combatmg_mk2", 200, GTAWeapon.WeaponCategory.LMG, 4, 0xDBBD7280, false, false, true, false) { PossibleComponents = CombatMGMK2Components });

        List<GTAWeapon.WeaponComponent> GusenbergComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_GUSENBERG_CLIP_01", 0x1CE5A6A5, false, "weapon_gusenberg"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_GUSENBERG_CLIP_02", 0xEAC8C270, false, "weapon_gusenberg")
        };
        WeaponsList.Add(new GTAWeapon("weapon_gusenberg", 200, GTAWeapon.WeaponCategory.LMG, 4, 1627465347, false, false, true, false) { PossibleComponents = GusenbergComponents });

        //Sniper

        List<GTAWeapon.WeaponComponent> SniperRifleComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_SNIPERRIFLE_CLIP_01", 0x9BC64089, false, "weapon_sniperrifle"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_sniperrifle"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_LARGE", 0xD2443DDC, false, "weapon_sniperrifle"),
            new GTAWeapon.WeaponComponent("Advanced Scope", "COMPONENT_AT_SCOPE_MAX", 0xBC54DA77, false, "weapon_sniperrifle"),
            new GTAWeapon.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_SNIPERRIFLE_VARMOD_LUXE", 0x4032B5E7, false, "weapon_sniperrifle")
        };
        WeaponsList.Add(new GTAWeapon("weapon_sniperrifle", 40, GTAWeapon.WeaponCategory.Sniper, 4, 100416529, false, false, true, true) { PossibleComponents = SniperRifleComponents });


        List<GTAWeapon.WeaponComponent> HeavySniperComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_HEAVYSNIPER_CLIP_01", 0x476F52F4, false, "weapon_heavysniper"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_LARGE", 0xD2443DDC, false, "weapon_heavysniper"),
            new GTAWeapon.WeaponComponent("Advanced Scope", "COMPONENT_AT_SCOPE_MAX", 0xBC54DA77, false, "weapon_heavysniper")
        };
        WeaponsList.Add(new GTAWeapon("weapon_heavysniper", 40, GTAWeapon.WeaponCategory.Sniper, 4, 205991906, false, false, true, true) { PossibleComponents = HeavySniperComponents });


        List<GTAWeapon.WeaponComponent> HeavySniperMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_HEAVYSNIPER_MK2_CLIP_01", 0xFA1E1A28, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_HEAVYSNIPER_MK2_CLIP_02", 0x2CD8FF9D, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_HEAVYSNIPER_MK2_CLIP_INCENDIARY", 0xEC0F617, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Armor Piercing Rounds", "COMPONENT_HEAVYSNIPER_MK2_CLIP_ARMORPIERCING", 0xF835D6D4, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_HEAVYSNIPER_MK2_CLIP_FMJ", 0x3BE948F6, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Explosive Rounds", "COMPONENT_HEAVYSNIPER_MK2_CLIP_EXPLOSIVE", 0x89EBDAA7, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Zoom Scope", "COMPONENT_AT_SCOPE_LARGE_MK2", 0x82C10383, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Advanced Scope", "COMPONENT_AT_SCOPE_MAX", 0xBC54DA77, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Night Vision Scope", "COMPONENT_AT_SCOPE_NV", 0xB68010B0, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Thermal Scope", "COMPONENT_AT_SCOPE_THERMAL", 0x2E43DA41, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_SR_SUPP_03", 0xAC42DF71, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Squared Muzzle Brake", "COMPONENT_AT_MUZZLE_08", 0x5F7DCE4D, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Bell-End Muzzle Brake", "COMPONENT_AT_MUZZLE_09", 0x6927E1A1, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Default Barrel", "COMPONENT_AT_SR_BARREL_01", 0x909630B7, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Barrel", "COMPONENT_AT_SR_BARREL_02", 0x108AB09E, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_HEAVYSNIPER_MK2_CAMO", 0xF8337D02, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_HEAVYSNIPER_MK2_CAMO_02", 0xC5BEDD65, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_HEAVYSNIPER_MK2_CAMO_03", 0xE9712475, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_HEAVYSNIPER_MK2_CAMO_04", 0x13AA78E7, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_HEAVYSNIPER_MK2_CAMO_05", 0x26591E50, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_HEAVYSNIPER_MK2_CAMO_06", 0x302731EC, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_HEAVYSNIPER_MK2_CAMO_07", 0xAC722A78, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_HEAVYSNIPER_MK2_CAMO_08", 0xBEA4CEDD, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_HEAVYSNIPER_MK2_CAMO_09", 0xCD776C82, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_HEAVYSNIPER_MK2_CAMO_10", 0xABC5ACC7, false, "weapon_heavysniper_mk2"),
            new GTAWeapon.WeaponComponent("Patriotic", "COMPONENT_HEAVYSNIPER_MK2_CAMO_IND_01", 0x6C32D2EB, false, "weapon_heavysniper_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_heavysniper_mk2", 40, GTAWeapon.WeaponCategory.Sniper, 4, 0xA914799, false, false, true, true) { PossibleComponents = HeavySniperMK2Components });


        List<GTAWeapon.WeaponComponent> MarksmanRifleComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_MARKSMANRIFLE_CLIP_01", 0xD83B4141, false, "weapon_marksmanrifle"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MARKSMANRIFLE_CLIP_02", 0xCCFD2AC5, false, "weapon_marksmanrifle"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM", 0x1C221B1A, false, "weapon_marksmanrifle"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_marksmanrifle"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_marksmanrifle"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_marksmanrifle"),
            new GTAWeapon.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_MARKSMANRIFLE_VARMOD_LUXE", 0x161E9241, false, "weapon_marksmanrifle")
        };
        WeaponsList.Add(new GTAWeapon("weapon_marksmanrifle", 40, GTAWeapon.WeaponCategory.Sniper, 4, 3342088282, false, false, true, true) { PossibleComponents = MarksmanRifleComponents });


        List<GTAWeapon.WeaponComponent> MarksmanRifleMK2Components = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_01", 0x94E12DCE, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_02", 0xE6CFD1AA, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Tracer Rounds", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_TRACER", 0xD77A22D2, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Incendiary Rounds", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_INCENDIARY", 0x6DD7A86E, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Armor Piercing Rounds", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_ARMORPIERCING", 0xF46FD079, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_FMJ", 0xE14A9ED3, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Zoom Scope", "COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM_MK2", 0x5B1C713C, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Default Barrel", "COMPONENT_AT_MRFL_BARREL_01", 0x381B5D89, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Heavy Barrel", "COMPONENT_AT_MRFL_BARREL_02", 0x68373DDC, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Digital Camo", "COMPONENT_MARKSMANRIFLE_MK2_CAMO", 0x9094FBA0, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Brushstroke Camo", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_02", 0x7320F4B2, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Woodland Camo", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_03", 0x60CF500F, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Skull", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_04", 0xFE668B3F, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Sessanta Nove", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_05", 0xF3757559, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Perseus", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_06", 0x193B40E8, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Leopard", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_07", 0x107D2F6C, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Zebra", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_08", 0xC4E91841, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Geometric", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_09", 0x9BB1C5D3, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_10", 0x3B61040B, false, "weapon_marksmanrifle_mk2"),
            new GTAWeapon.WeaponComponent("Boom!", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_IND_01", 0xB7A316DA, false, "weapon_marksmanrifle_mk2")
        };
        WeaponsList.Add(new GTAWeapon("weapon_marksmanrifle_mk2", 40, GTAWeapon.WeaponCategory.Sniper, 4, 0x6A6C02E0, false, false, true, true) { PossibleComponents = MarksmanRifleMK2Components });

        //Heavy
        WeaponsList.Add(new GTAWeapon("weapon_rpg", 3, GTAWeapon.WeaponCategory.Heavy, 4, 2982836145, false, false, true, false));

        List<GTAWeapon.WeaponComponent> GrenadeLauncherComponents = new List<GTAWeapon.WeaponComponent>
        {
            new GTAWeapon.WeaponComponent("Default Clip", "COMPONENT_GRENADELAUNCHER_CLIP_01", 0x11AE5C97, false, "weapon_grenadelauncher"),
            new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_grenadelauncher"),
            new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_grenadelauncher"),
            new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_grenadelauncher")
        };
        WeaponsList.Add(new GTAWeapon("weapon_grenadelauncher", 32, GTAWeapon.WeaponCategory.Heavy, 4, 2726580491, false, false, true, false) { PossibleComponents = GrenadeLauncherComponents });

        WeaponsList.Add(new GTAWeapon("weapon_grenadelauncher_smoke", 32, GTAWeapon.WeaponCategory.Heavy, 4, 1305664598, false, false, true, false) { IsRegularGun = false });
        WeaponsList.Add(new GTAWeapon("weapon_minigun", 500, GTAWeapon.WeaponCategory.Heavy, 4, 1119849093, false, false, true, false));
        WeaponsList.Add(new GTAWeapon("weapon_firework", 20, GTAWeapon.WeaponCategory.Heavy, 4, 0x7F7497E5, false, false, true, false) { IsRegularGun = false });
        WeaponsList.Add(new GTAWeapon("weapon_railgun", 50, GTAWeapon.WeaponCategory.Heavy, 4, 0x6D544C99, false, false, true, false) { IsRegularGun = false });
        WeaponsList.Add(new GTAWeapon("weapon_hominglauncher", 3, GTAWeapon.WeaponCategory.Heavy, 4, 0x63AB0442, false, false, true, false) { IsRegularGun = false });
        WeaponsList.Add(new GTAWeapon("weapon_compactlauncher", 10, GTAWeapon.WeaponCategory.Heavy, 4, 125959754, false, false, true, false));
        WeaponsList.Add(new GTAWeapon("weapon_rayminigun", 50, GTAWeapon.WeaponCategory.Heavy, 4, 0xB62D1F67, false, false, true, false) { IsRegularGun = false });

        foreach (GTAWeapon Weapon in WeaponsList.Where(x => x.Category == GTAWeapon.WeaponCategory.Pistol))
        {
            if (Weapon.Name == "weapon_marksmanpistol" || Weapon.Name == "weapon_stungun" || Weapon.Name == "weapon_flaregun" || Weapon.Name == "weapon_raypistol")
                Weapon.CanPistolSuicide = false;
            else
                Weapon.CanPistolSuicide = true;

            if (Weapon.Name == "weapon_pistol")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_pistol"));
                Weapon.PoliceVariations.Add(Police2);

                GTAWeapon.WeaponVariation Police3 = new GTAWeapon.WeaponVariation(0);
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_CLIP_02", 0xED265A1C, true, "weapon_pistol"));
                Weapon.PoliceVariations.Add(Police3);

                GTAWeapon.WeaponVariation Police4 = new GTAWeapon.WeaponVariation(0);
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_pistol"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_CLIP_02", 0xED265A1C, true, "weapon_pistol"));
                Weapon.PoliceVariations.Add(Police4);

                GTAWeapon.WeaponVariation Player1 = new GTAWeapon.WeaponVariation(0);
                Player1.Components.Add(new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP_02", 0x65EA7EBB, false, "weapon_pistol"));
                Weapon.PlayerVariations.Add(Player1);

                GTAWeapon.WeaponVariation Player2 = new GTAWeapon.WeaponVariation(0);
                Weapon.PlayerVariations.Add(Player2);
            }
            if (Weapon.Name == "weapon_pistol_mk2")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH_02", 0x43FD595B, true, "weapon_pistol_mk2"));
                Weapon.PoliceVariations.Add(Police2);

                GTAWeapon.WeaponVariation Police3 = new GTAWeapon.WeaponVariation(0);
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_MK2_CLIP_02", 0x5ED6C128, true, "weapon_pistol_mk2"));
                Weapon.PoliceVariations.Add(Police3);

                GTAWeapon.WeaponVariation Police4 = new GTAWeapon.WeaponVariation(0);
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH_02", 0x43FD595B, true, "weapon_pistol_mk2"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_MK2_CLIP_02", 0x5ED6C128, true, "weapon_pistol_mk2"));
                Weapon.PoliceVariations.Add(Police4);

                GTAWeapon.WeaponVariation Player1 = new GTAWeapon.WeaponVariation(0);
                Player1.Components.Add(new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP_02", 0x65EA7EBB, false, "weapon_pistol_mk2"));
                Weapon.PlayerVariations.Add(Player1);

                GTAWeapon.WeaponVariation Player2 = new GTAWeapon.WeaponVariation(0);
                Weapon.PlayerVariations.Add(Player2);
            }
            if (Weapon.Name == "weapon_combatpistol")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_combatpistol"));
                Weapon.PoliceVariations.Add(Police2);

                GTAWeapon.WeaponVariation Police3 = new GTAWeapon.WeaponVariation(0);
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_COMBATPISTOL_CLIP_02", 0xD67B4F2D, true, "weapon_combatpistol"));
                Weapon.PoliceVariations.Add(Police3);

                GTAWeapon.WeaponVariation Police4 = new GTAWeapon.WeaponVariation(0);
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_combatpistol"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_COMBATPISTOL_CLIP_02", 0xD67B4F2D, true, "weapon_combatpistol"));
                Weapon.PoliceVariations.Add(Police4);

                GTAWeapon.WeaponVariation Player1 = new GTAWeapon.WeaponVariation(0);
                Player1.Components.Add(new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_combatpistol"));
                Weapon.PlayerVariations.Add(Player1);

                GTAWeapon.WeaponVariation Player2 = new GTAWeapon.WeaponVariation(0);
                Weapon.PlayerVariations.Add(Player2);
            }
            if (Weapon.Name == "weapon_heavypistol")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_HEAVYPISTOL_VARMOD_LUXE", 0x7A6A7B7B, true, "weapon_heavypistol"));
                Weapon.PoliceVariations.Add(Police2);

                GTAWeapon.WeaponVariation Police3 = new GTAWeapon.WeaponVariation(0);
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_heavypistol"));
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_HEAVYPISTOL_CLIP_02", 0x64F9C62B, true, "weapon_heavypistol"));
                Weapon.PoliceVariations.Add(Police3);

                GTAWeapon.WeaponVariation Police4 = new GTAWeapon.WeaponVariation(0);
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_heavypistol"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_HEAVYPISTOL_VARMOD_LUXE", 0x7A6A7B7B, true, "weapon_heavypistol"));
                Weapon.PoliceVariations.Add(Police4);
            }
        }
        foreach (GTAWeapon Weapon in WeaponsList.Where(x => x.Category == GTAWeapon.WeaponCategory.AR))
        {
            if (Weapon.Name == "weapon_carbinerifle")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, true, "weapon_carbinerifle"));
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, true, "weapon_carbinerifle"));
                Weapon.PoliceVariations.Add(Police2);

                GTAWeapon.WeaponVariation Police3 = new GTAWeapon.WeaponVariation(0);
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, true, "weapon_carbinerifle"));
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, true, "weapon_carbinerifle"));
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_CARBINERIFLE_CLIP_02", 0x91109691, true, "weapon_carbinerifle"));
                Weapon.PoliceVariations.Add(Police3);

                GTAWeapon.WeaponVariation Police4 = new GTAWeapon.WeaponVariation(0);
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, true, "weapon_carbinerifle"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, true, "weapon_carbinerifle"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, true, "weapon_carbinerifle"));
                Weapon.PoliceVariations.Add(Police4);
            }
            if (Weapon.Name == "weapon_carbinerifle_mk2")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, true, "weapon_carbinerifle_mk2"));
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, true, "weapon_carbinerifle_mk2"));
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, true, "weapon_carbinerifle_mk2"));
                Weapon.PoliceVariations.Add(Police2);

                GTAWeapon.WeaponVariation Police3 = new GTAWeapon.WeaponVariation(0);
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, true, "weapon_carbinerifle_mk2"));
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, true, "weapon_carbinerifle_mk2"));
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_CARBINERIFLE_MK2_CLIP_02", 0x5DD5DBD5, true, "weapon_carbinerifle_mk2"));
                Weapon.PoliceVariations.Add(Police3);

                GTAWeapon.WeaponVariation Police4 = new GTAWeapon.WeaponVariation(0);
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, true, "weapon_carbinerifle_mk2"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, true, "weapon_carbinerifle_mk2"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, true, "weapon_carbinerifle_mk2"));
                Weapon.PoliceVariations.Add(Police4);
            }
        }
        foreach (GTAWeapon Weapon in WeaponsList.Where(x => x.Category == GTAWeapon.WeaponCategory.SMG))
        {
            if (Weapon.Name == "weapon_minismg" || Weapon.Name == "weapon_machinepistol" || Weapon.Name == "weapon_microsmg")
                Weapon.CanPistolSuicide = true;
            else
                Weapon.CanPistolSuicide = false;

            if (Weapon.Name == "weapon_combatpdw")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_combatpdw"));
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatpdw"));
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_combatpdw"));
                Weapon.PoliceVariations.Add(Police2);

                GTAWeapon.WeaponVariation Police3 = new GTAWeapon.WeaponVariation(0);
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_combatpdw"));
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatpdw"));
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_COMBATPDW_CLIP_02", 0x334A5203, false, "weapon_combatpdw"));
                Weapon.PoliceVariations.Add(Police3);

                GTAWeapon.WeaponVariation Police4 = new GTAWeapon.WeaponVariation(0);
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_combatpdw"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatpdw"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_combatpdw"));
                Weapon.PoliceVariations.Add(Police4);
            }
            if (Weapon.Name == "weapon_microsmg")
            {
                GTAWeapon.WeaponVariation Player1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PlayerVariations.Add(Player1);

                GTAWeapon.WeaponVariation Player2 = new GTAWeapon.WeaponVariation(0);
                Player2.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MICROSMG_CLIP_02", 0x10E6BA2B, false, "weapon_microsmg"));
                Player2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_microsmg"));
                Weapon.PlayerVariations.Add(Player2);

                GTAWeapon.WeaponVariation Player3 = new GTAWeapon.WeaponVariation(0);
                Player3.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MICROSMG_CLIP_02", 0x10E6BA2B, false, "weapon_microsmg"));
                Weapon.PlayerVariations.Add(Player3);

                GTAWeapon.WeaponVariation Player4 = new GTAWeapon.WeaponVariation(0);
                Player4.Components.Add(new GTAWeapon.WeaponComponent("Extended Clip", "COMPONENT_MICROSMG_CLIP_02", 0x10E6BA2B, false, "weapon_microsmg"));
                Player4.Components.Add(new GTAWeapon.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_microsmg"));
                Weapon.PlayerVariations.Add(Player4);
            }

        }

        foreach (GTAWeapon Weapon in WeaponsList.Where(x => x.Category == GTAWeapon.WeaponCategory.Shotgun))
        {
            if (Weapon.Name == "weapon_dbshotgun")
                Weapon.CanPistolSuicide = true;
            else
                Weapon.CanPistolSuicide = false;

            if (Weapon.Name == "weapon_pumpshotgun")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun"));
                Weapon.PoliceVariations.Add(Police2);
            }
            if (Weapon.Name == "weapon_pumpshotgun_mk2")
            {
                GTAWeapon.WeaponVariation Police1 = new GTAWeapon.WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                GTAWeapon.WeaponVariation Police2 = new GTAWeapon.WeaponVariation(0);
                Police2.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun_mk2"));
                Weapon.PoliceVariations.Add(Police2);

                GTAWeapon.WeaponVariation Police3 = new GTAWeapon.WeaponVariation(0);
                Police3.Components.Add(new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_pumpshotgun_mk2"));
                Weapon.PoliceVariations.Add(Police3);

                GTAWeapon.WeaponVariation Police4 = new GTAWeapon.WeaponVariation(0);
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_pumpshotgun_mk2"));
                Police4.Components.Add(new GTAWeapon.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun_mk2"));
                Weapon.PoliceVariations.Add(Police4);
            }
        }
    }
    public static GTAWeapon GetWeaponFromHash(ulong WeaponHash)
    {
        return WeaponsList.Where(x => x.Hash == WeaponHash).FirstOrDefault();
    }
    public static List<GTAWeapon.WeaponComponent> GetWeaponVariations(string GunName)
    {
        return WeaponsList.Where(x => x.Name == GunName).FirstOrDefault().PossibleComponents;
    }
    public static GTAWeapon GetRandomWeaponByCategory(GTAWeapon.WeaponCategory MyCategory)
    {
        return WeaponsList.Where(x => x.Category == MyCategory).PickRandom();
    }
    public static GTAWeapon GetRandomWeapon(GTAWeapon.WeaponCategory Category)
    {
        return WeaponsList.Where(s => s.Category == Category && s.IsRegularGun).PickRandom();
    }
}
[Serializable()]
public class GTAWeapon
{
    public enum WeaponCategory
    {
        Melee = 0,
        Pistol = 1,
        Shotgun = 2,
        SMG = 3,
        AR = 4,
        LMG = 5,
        Sniper = 6,
        Heavy = 7,
        Unknown = 8,
    }
    public GTAWeapon()
    {

    }
    public GTAWeapon(String _Name, short _AmmoAmount, WeaponCategory _Category, int _WeaponLevel, ulong _Hash, bool _isPoliceIssue, bool _IsOneHanded, bool _IsTwoHanded,bool _IsLegal)
    {
        Name = _Name;
        AmmoAmount = _AmmoAmount;
        Category = _Category;
        WeaponLevel = _WeaponLevel;
        Hash = _Hash;
        isPoliceIssue = _isPoliceIssue;
        IsOneHanded = _IsOneHanded;
        IsTwoHanded = _IsTwoHanded;
        IsLegal = _IsLegal;
    }
    public string Name;
    public short AmmoAmount;
    public WeaponCategory Category;
    public int WeaponLevel;
    public ulong Hash;
    public string ScannerFile;
    public bool isPoliceIssue = false;
    public bool CanPistolSuicide = false;
    public bool IsTwoHanded = false;
    public bool IsOneHanded = false;
    public bool IsLegal = false;
    public bool IsRegularGun = true;
    public List<WeaponVariation> PoliceVariations = new List<WeaponVariation>();
    public List<WeaponVariation> PlayerVariations = new List<WeaponVariation>();
    public List<WeaponComponent> PossibleComponents;
    public class WeaponVariation
    {
        public int Tint;
        public List<WeaponComponent> Components = new List<WeaponComponent>();
        public WeaponVariation(List<WeaponComponent> _Components)
        {
            Components = _Components;
        }
        public WeaponVariation()
        {

        }
        public WeaponVariation(int _Tint, List<WeaponComponent> _Components)
        {
            Tint = _Tint;
            Components = _Components;
        }
        public WeaponVariation(int _Tint)
        {
            Tint = _Tint;
        }
    }
    public class WeaponComponent
    {
        public string Name;
        public string HashKey;
        public ulong Hash;
        public string BaseWeapon;
        public bool Enabled;
        public WeaponComponent()
        {

        }
        public WeaponComponent(string _Name, string _HashKey, ulong _Hash, bool _Enabled)
        {
            Name = _Name;
            HashKey = _HashKey;
            Hash = _Hash;
            Enabled = _Enabled;
        }
        public WeaponComponent(string _Name, string _HashKey, ulong _Hash, bool _Enabled, string _BaseWeapon)
        {
            Name = _Name;
            HashKey = _HashKey;
            Hash = _Hash;
            Enabled = _Enabled;
            BaseWeapon = _BaseWeapon;
        }
    }
}