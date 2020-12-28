using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
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

public class Weapons : IWeapons
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
        {
            return new WeaponInformation("Generic Melee", 0, WeaponCategory.Melee, 0, 0, false, false, false);
        }

        if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, 0, 2))
        {
            return new WeaponInformation("Generic Weapon", 0, WeaponCategory.Melee, 0, 0, false, false, false);
        }

        if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE", Pedestrian))
        {
            return new WeaponInformation("Vehicle Injury", 0, WeaponCategory.Vehicle, 0, 0, false, false, false);
        }
        else
        {
            return new WeaponInformation("Unknown", 0, WeaponCategory.Unknown, 0, 0, false, false, false);
        }
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
            return new WeaponVariation(Tint);

        List<WeaponComponent> ComponentsOnGun = new List<WeaponComponent>();

        foreach (WeaponComponent PossibleComponent in MyGun.PossibleComponents)
        {
            if (NativeFunction.CallByName<bool>("HAS_PED_GOT_WEAPON_COMPONENT", WeaponOwner, WeaponHash, PossibleComponent.Hash))
            {
                ComponentsOnGun.Add(PossibleComponent);
            }

        }
        return new WeaponVariation(ComponentsOnGun);

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
            new WeaponComponent("Base Model", 0xF3462F33),
            new WeaponComponent("The Pimp", 0xC613F685),
            new WeaponComponent("The Ballas", 0xEED9FD63),
            new WeaponComponent("The Hustler", 0x50910C31),
            new WeaponComponent("The Rock", 0x9761D9DC),
            new WeaponComponent("The Hater", 0x7DECFE30),
            new WeaponComponent("The Lover", 0x3F4E8AA6),
            new WeaponComponent("The Player", 0x8B808BB),
            new WeaponComponent("The King", 0xE28BABEF),
            new WeaponComponent("The Vagos", 0x7AF3F785)
        };
        WeaponsList.Add(new WeaponInformation("weapon_knuckle", 0, WeaponCategory.Melee, 0, 3638508604, false, false, true) { IsRegular = false, PossibleComponents = KnuckleComponents });
        WeaponsList.Add(new WeaponInformation("weapon_knife", 0, WeaponCategory.Melee, 0, 2578778090, false, false, true));
        WeaponsList.Add(new WeaponInformation("weapon_machete", 0, WeaponCategory.Melee, 0, 3713923289, false, false, true));

        List<WeaponComponent> SwitchbladeComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Handle", 0x9137A500),
            new WeaponComponent("VIP Variant", 0x5B3E7DB6),
            new WeaponComponent("Bodyguard Variant", 0xE7939662)
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
            new WeaponComponent("Default Clip", 0xFED0FD71),
            new WeaponComponent("Extended Clip", 0xED265A1C),
            new WeaponComponent("Flashlight", 0x359B7AAE),
            new WeaponComponent("Suppressor", 0x65EA7EBB),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD7391086)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol", 60, WeaponCategory.Pistol, 1, 453432689, true, false, true) { PossibleComponents = PistolComponents, CanPistolSuicide = true });

        List<WeaponComponent> PistolMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94F42D62),
            new WeaponComponent("Extended Clip", 0x5ED6C128),
            new WeaponComponent("Tracer Rounds", 0x25CAAEAF),
            new WeaponComponent("Incendiary Rounds", 0x2BBD7A3A),
            new WeaponComponent("Hollow Point Rounds", 0x85FEA109),
            new WeaponComponent("Full Metal Jacket Rounds", 0x4F37DF2A),
            new WeaponComponent("Mounted Scope", 0x8ED4BB70),
            new WeaponComponent("Flashlight", 0x43FD595B),
            new WeaponComponent("Suppressor", 0x65EA7EBB),
            new WeaponComponent("Compensator", 0x21E34793),
            new WeaponComponent("Digital Camo", 0x5C6C749C),
            new WeaponComponent("Brushstroke Camo", 0x15F7A390),
            new WeaponComponent("Woodland Camo", 0x968E24DB),
            new WeaponComponent("Skull", 0x17BFA99),
            new WeaponComponent("Sessanta Nove", 0xF2685C72),
            new WeaponComponent("Perseus", 0xDD2231E6),
            new WeaponComponent("Leopard", 0xBB43EE76),
            new WeaponComponent("Zebra", 0x4D901310),
            new WeaponComponent("Geometric", 0x5F31B653),
            new WeaponComponent("Boom!", 0x697E19A0),
            new WeaponComponent("Patriotic", 0x930CB951),
            new WeaponComponent("Digital Camo", 0xB4FC92B0),
            new WeaponComponent("Digital Camo", 0x1A1F1260),
            new WeaponComponent("Digital Camo", 0xE4E00B70),
            new WeaponComponent("Digital Camo", 0x2C298B2B),
            new WeaponComponent("Digital Camo", 0xDFB79725),
            new WeaponComponent("Digital Camo", 0x6BD7228C),
            new WeaponComponent("Digital Camo", 0x9DDBCF8C),
            new WeaponComponent("Digital Camo", 0xB319A52C),
            new WeaponComponent("Digital Camo", 0xC6836E12),
            new WeaponComponent("Digital Camo", 0x43B1B173),
            new WeaponComponent("Patriotic", 0x4ABDA3FA)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol_mk2", 60, WeaponCategory.Pistol, 1, 0xBFE256D4, true, false, true) { PossibleComponents = PistolMK2Components, CanPistolSuicide = true });

        List<WeaponComponent> CombatPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x721B079),
            new WeaponComponent("Extended Clip", 0xD67B4F2D),
            new WeaponComponent("Flashlight", 0x359B7AAE),
            new WeaponComponent("Suppressor", 0xC304849A),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xC6654D72)
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatpistol", 60, WeaponCategory.Pistol, 1, 1593441988, true, false, true) { PossibleComponents = CombatPistolComponents, CanPistolSuicide = true });


        List<WeaponComponent> APPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x31C4B22A),
            new WeaponComponent("Extended Clip", 0x249A17D5),
            new WeaponComponent("Flashlight", 0x359B7AAE),
            new WeaponComponent("Suppressor", 0xC304849A),
            new WeaponComponent("Gilded Gun Metal Finish", 0x9B76C72C)
        };
        WeaponsList.Add(new WeaponInformation("weapon_appistol", 60, WeaponCategory.Pistol, 1, 584646201, true, false, false) { PossibleComponents = APPistolComponents, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_stungun", 0, WeaponCategory.Melee, 0, 911657153, true, false, true) { CanPistolSuicide = false });

        List<WeaponComponent> Pistol50Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x2297BE19),
            new WeaponComponent("Extended Clip", 0xD9D3AC92),
            new WeaponComponent("Flashlight", 0x359B7AAE),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Platinum Pearl Deluxe Finish", 0x77B8AB2F)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol50", 60, WeaponCategory.Pistol, 1, 2578377531, true, false, true) { PossibleComponents = Pistol50Components, CanPistolSuicide = true });

        List<WeaponComponent> SNSPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xF8802ED9),
            new WeaponComponent("Extended Clip", 0x7B0033B3),
            new WeaponComponent("Etched Wood Grip Finish", 0x8033ECAF)
        };
        WeaponsList.Add(new WeaponInformation("weapon_snspistol", 60, WeaponCategory.Pistol, 1, 3218215474, true, false, true) { PossibleComponents = SNSPistolComponents, CanPistolSuicide = true });

        List<WeaponComponent> SNSPistolMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x1466CE6),
            new WeaponComponent("Extended Clip", 0xCE8C0772),
            new WeaponComponent("Tracer Rounds", 0x902DA26E),
            new WeaponComponent("Incendiary Rounds", 0xE6AD5F79),
            new WeaponComponent("Hollow Point Rounds", 0x8D107402),
            new WeaponComponent("Full Metal Jacket Rounds", 0xC111EB26),
            new WeaponComponent("Flashlight", 0x4A4965F3),
            new WeaponComponent("Mounted Scope", 0x47DE9258),
            new WeaponComponent("Suppressor", 0x65EA7EBB),
            new WeaponComponent("Compensator", 0xAA8283BF),
            new WeaponComponent("Digital Camo", 0xF7BEEDD),
            new WeaponComponent("Brushstroke Camo", 0x8A612EF6),
            new WeaponComponent("Woodland Camo", 0x76FA8829),
            new WeaponComponent("Skull", 0xA93C6CAC),
            new WeaponComponent("Sessanta Nove", 0x9C905354),
            new WeaponComponent("Perseus", 0x4DFA3621),
            new WeaponComponent("Leopard", 0x42E91FFF),
            new WeaponComponent("Zebra", 0x54A8437D),
            new WeaponComponent("Geometric", 0x68C2746),
            new WeaponComponent("Boom!", 0x2366E467),
            new WeaponComponent("Boom!", 0x441882E6),
            new WeaponComponent("Digital Camo", 0xE7EE68EA),
            new WeaponComponent("Brushstroke Camo", 0x29366D21),
            new WeaponComponent("Woodland Camo", 0x3ADE514B),
            new WeaponComponent("Skull", 0xE64513E9),
            new WeaponComponent("Sessanta Nove", 0xCD7AEB9A),
            new WeaponComponent("Perseus", 0xFA7B27A6),
            new WeaponComponent("Leopard", 0xE285CA9A),
            new WeaponComponent("Zebra", 0x2B904B19),
            new WeaponComponent("Geometric", 0x22C24F9C),
            new WeaponComponent("Boom!", 0x8D0D5ECD),
            new WeaponComponent("Patriotic", 0x1F07150A)
        };
        WeaponsList.Add(new WeaponInformation("weapon_snspistol_mk2", 60, WeaponCategory.Pistol, 1, 0x88374054, true, false, true) { PossibleComponents = SNSPistolMK2Components, CanPistolSuicide = true });

        List<WeaponComponent> HeavyPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xD4A969A),
            new WeaponComponent("Extended Clip", 0x64F9C62B),
            new WeaponComponent("Flashlight", 0x359B7AAE),
            new WeaponComponent("Suppressor", 0xC304849A),
            new WeaponComponent("Etched Wood Grip Finish", 0x7A6A7B7B)
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavypistol", 60, WeaponCategory.Pistol, 1, 3523564046, true, false, true) { PossibleComponents = HeavyPistolComponents, CanPistolSuicide = true });

        List<WeaponComponent> VintagePistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x45A3B6BB),
            new WeaponComponent("Extended Clip", 0x33BA12E8),
            new WeaponComponent("Suppressor", 0xC304849A)
        };
        WeaponsList.Add(new WeaponInformation("weapon_vintagepistol", 60, WeaponCategory.Pistol, 1, 137902532, true, false, true) { PossibleComponents = VintagePistolComponents, CanPistolSuicide = true });


        WeaponsList.Add(new WeaponInformation("weapon_flaregun", 60, WeaponCategory.Pistol, 1, 1198879012, true, false, true) { IsRegular = false, CanPistolSuicide = false });
        WeaponsList.Add(new WeaponInformation("weapon_marksmanpistol", 60, WeaponCategory.Pistol, 1, 3696079510, true, false, true) { IsRegular = false, CanPistolSuicide = false });

        List<WeaponComponent> RevolverComponents = new List<WeaponComponent>
        {
            new WeaponComponent("VIP Variant", 0x16EE3040),
            new WeaponComponent("Bodyguard Variant", 0x9493B80D),
            new WeaponComponent("Default Clip", 0xE9867CE3)
        };
        WeaponsList.Add(new WeaponInformation("weapon_revolver", 60, WeaponCategory.Pistol, 1, 3249783761, true, false, true) { PossibleComponents = RevolverComponents, CanPistolSuicide = true });

        List<WeaponComponent> RevolverMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Rounds", 0xBA23D8BE),
            new WeaponComponent("Tracer Rounds", 0xC6D8E476),
            new WeaponComponent("Incendiary Rounds", 0xEFBF25),
            new WeaponComponent("Hollow Point Rounds", 0x10F42E8F),
            new WeaponComponent("Full Metal Jacket Rounds", 0xDC8BA3F),
            new WeaponComponent("Holographic Sight", 0x420FD713),
            new WeaponComponent("Small Scope", 0x49B2945),
            new WeaponComponent("Flashlight", 0x359B7AAE),
            new WeaponComponent("Compensator", 0x27077CCB),
            new WeaponComponent("Digital Camo", 0xC03FED9F),
            new WeaponComponent("Brushstroke Camo", 0xB5DE24),
            new WeaponComponent("Woodland Camo", 0xA7FF1B8),
            new WeaponComponent("Skull", 0xF2E24289),
            new WeaponComponent("Sessanta Nove", 0x11317F27),
            new WeaponComponent("Perseus", 0x17C30C42),
            new WeaponComponent("Leopard", 0x257927AE),
            new WeaponComponent("Zebra", 0x37304B1C),
            new WeaponComponent("Geometric", 0x48DAEE71),
            new WeaponComponent("Boom!", 0x20ED9B5B),
            new WeaponComponent("Patriotic", 0xD951E867)
        };
        WeaponsList.Add(new WeaponInformation("weapon_revolver_mk2", 60, WeaponCategory.Pistol, 1, 0xCB96392F, true, false, true) { PossibleComponents = RevolverMK2Components, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_doubleaction", 60, WeaponCategory.Pistol, 1, 0x97EA20B8, true, false, true) { CanPistolSuicide = true });
        List<WeaponComponent> RayGunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Festive tint", 0xD7DBF707)
        };
        WeaponsList.Add(new WeaponInformation("weapon_raypistol", 60, WeaponCategory.Pistol, 1, 0xAF3696A1, true, false, false) { IsRegular = false, PossibleComponents = RayGunComponents, CanPistolSuicide = false });

        //Shotgun
        List<WeaponComponent> PumpShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Suppressor", 0xE608B35E),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xA2D79DDB)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pumpshotgun", 32, WeaponCategory.Shotgun, 2, 487013001, false, true, true) { PossibleComponents = PumpShotgunComponents});

        List<WeaponComponent> PumpShotgunMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Shells", 0xCD940141),
            new WeaponComponent("Dragon's Breath Shells", 0x9F8A1BF5),
            new WeaponComponent("Steel Buckshot Shells", 0x4E65B425),
            new WeaponComponent("Flechette Shells", 0xE9582927),
            new WeaponComponent("Explosive Slugs", 0x3BE4465D),
            new WeaponComponent("Holographic Sight", 0x420FD713),
            new WeaponComponent("Small Scope", 0x49B2945),
            new WeaponComponent("Medium Scope", 0x3F3C8181),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Suppressor", 0xAC42DF71),
            new WeaponComponent("Squared Muzzle Brake", 0x5F7DCE4D),
            new WeaponComponent("Digital Camo", 0xE3BD9E44),
            new WeaponComponent("Brushstroke Camo", 0x17148F9B),
            new WeaponComponent("Woodland Camo", 0x24D22B16),
            new WeaponComponent("Skull", 0xF2BEC6F0),
            new WeaponComponent("Sessanta Nove", 0x85627D),
            new WeaponComponent("Perseus", 0xDC2919C5),
            new WeaponComponent("Leopard", 0xE184247B),
            new WeaponComponent("Zebra", 0xD8EF9356),
            new WeaponComponent("Geometric", 0xEF29BFCA),
            new WeaponComponent("Boom!", 0x67AEB165),
            new WeaponComponent("Patriotic", 0x46411A1D)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pumpshotgun_mk2", 32, WeaponCategory.Shotgun, 2, 0x555AF99A, false, true, true) { PossibleComponents = PumpShotgunMK2Components });

        List<WeaponComponent> SawnOffShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Gilded Gun Metal Finish", 0x85A64DF9)
        };
        WeaponsList.Add(new WeaponInformation("weapon_sawnoffshotgun", 32, WeaponCategory.Shotgun, 2, 2017895192, false, true, false) { PossibleComponents = SawnOffShotgunComponents });

        List<WeaponComponent> AssaultShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94E81BC7),
            new WeaponComponent("Extended Clip", 0x86BD7F72),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Suppressor", 0x837445AA),
            new WeaponComponent("Grip", 0xC164F53)
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultshotgun", 32, WeaponCategory.Shotgun, 2, 3800352039, false, true, false) { PossibleComponents = AssaultShotgunComponents });

        List<WeaponComponent> BullpupShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Grip", 0xC164F53)
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpupshotgun", 32, WeaponCategory.Shotgun, 2, 2640438543, false, true, true) { PossibleComponents = BullpupShotgunComponents });

        WeaponsList.Add(new WeaponInformation("weapon_musket", 32, WeaponCategory.Shotgun, 2, 2828843422, false, true, true) { IsRegular = false });

        List<WeaponComponent> HeavyShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x324F2D5F),
            new WeaponComponent("Extended Clip", 0x971CF6FD),
            new WeaponComponent("Drum Magazine", 0x88C7DA53),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Grip", 0xC164F53)
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavyshotgun", 32, WeaponCategory.Shotgun, 2, 984333226, false, true, true) { PossibleComponents = HeavyShotgunComponents });

        WeaponsList.Add(new WeaponInformation("weapon_dbshotgun", 32, WeaponCategory.Shotgun, 2, 4019527611, false, true, false) { CanPistolSuicide = true });
        WeaponsList.Add(new WeaponInformation("weapon_autoshotgun", 32, WeaponCategory.Shotgun, 2, 317205821, false, true, false));
        //SMG

        List<WeaponComponent> MicroSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xCB48AEF0),
            new WeaponComponent("Extended Clip", 0x10E6BA2B),
            new WeaponComponent("Flashlight", 0x359B7AAE),
            new WeaponComponent("Scope", 0x9D2FBF29),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x487AAE09)
        };
        WeaponsList.Add(new WeaponInformation("weapon_microsmg", 32, WeaponCategory.SMG, 2, 324215364, true, false, false) { PossibleComponents = MicroSMGComponents, CanPistolSuicide = true });


        List<WeaponComponent> SMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x26574997),
            new WeaponComponent("Extended Clip", 0x350966FB),
            new WeaponComponent("Drum Magazine", 0x79C77076),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Scope", 0x3CC6BA57),
            new WeaponComponent("Suppressor", 0xC304849A),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x27872C90)
        };
        WeaponsList.Add(new WeaponInformation("weapon_smg", 32, WeaponCategory.SMG, 2, 736523883, false, true, false) { PossibleComponents = SMGComponents });


        List<WeaponComponent> SMGMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4C24806E),
            new WeaponComponent("Extended Clip", 0xB9835B2E),
            new WeaponComponent("Tracer Rounds", 0x7FEA36EC),
            new WeaponComponent("Incendiary Rounds", 0xD99222E5),
            new WeaponComponent("Hollow Point Rounds", 0x3A1BD6FA),
            new WeaponComponent("Full Metal Jacket Rounds", 0xB5A715F),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Holographic Sight", 0x9FDB5652),
            new WeaponComponent("Small Scope", 0xE502AB6B),
            new WeaponComponent("Medium Scope", 0x3DECC7DA),
            new WeaponComponent("Suppressor", 0xC304849A),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE),
            new WeaponComponent("Default Barrel", 0xD9103EE1),
            new WeaponComponent("Heavy Barrel", 0xA564D78B),
            new WeaponComponent("Digital Camo", 0xC4979067),
            new WeaponComponent("Brushstroke Camo", 0x3815A945),
            new WeaponComponent("Woodland Camo", 0x4B4B4FB0),
            new WeaponComponent("Skull", 0xEC729200),
            new WeaponComponent("Sessanta Nove", 0x48F64B22),
            new WeaponComponent("Perseus", 0x35992468),
            new WeaponComponent("Leopard", 0x24B782A5),
            new WeaponComponent("Zebra", 0xA2E67F01),
            new WeaponComponent("Geometric", 0x2218FD68),
            new WeaponComponent("Boom!", 0x45C5C3C5),
            new WeaponComponent("Patriotic", 0x399D558F)
        };
        WeaponsList.Add(new WeaponInformation("weapon_smg_mk2", 32, WeaponCategory.SMG, 2, 0x78A97CD0, false, true, false) { PossibleComponents = SMGMK2Components });


        List<WeaponComponent> AssaultSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x8D1307B0),
            new WeaponComponent("Extended Clip", 0xBB46E417),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Scope", 0x9D2FBF29),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x278C78AF)
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultsmg", 32, WeaponCategory.SMG, 2, 4024951519, false, true, false) { PossibleComponents = AssaultSMGComponents });


        List<WeaponComponent> CombatPDWComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4317F19E),
            new WeaponComponent("Extended Clip", 0x334A5203),
            new WeaponComponent("Drum Magazine", 0x6EB8C8DB),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Grip", 0xC164F53),
            new WeaponComponent("Scope", 0xAA2C45B4)
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatpdw", 32, WeaponCategory.SMG, 2, 171789620, false, true, false) { PossibleComponents = CombatPDWComponents});


        List<WeaponComponent> MachinePistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x476E85FF),
            new WeaponComponent("Extended Clip", 0xB92C6979),
            new WeaponComponent("Drum Magazine", 0xA9E9CAF4),
            new WeaponComponent("Suppressor", 0xC304849A)
        };
        WeaponsList.Add(new WeaponInformation("weapon_machinepistol", 32, WeaponCategory.SMG, 2, 3675956304, true, false, false) { PossibleComponents = MachinePistolComponents, CanPistolSuicide = true });



        List<WeaponComponent> MiniSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x84C8B2D3),
            new WeaponComponent("Extended Clip", 0x937ED0B7)
        };
        WeaponsList.Add(new WeaponInformation("weapon_minismg", 32, WeaponCategory.SMG, 2, 3173288789, true, false, false) { PossibleComponents = MiniSMGComponents, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_raycarbine", 32, WeaponCategory.SMG, 2, 0x476BF155, true, false, false) { IsRegular = false });
        //AR
        List<WeaponComponent> ARComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xBE5EEA16),
            new WeaponComponent("Extended Clip", 0xB1214F9B),
            new WeaponComponent("Drum Magazine", 0xDBF0A53D),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Scope", 0x9D2FBF29),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Grip", 0xC164F53),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x4EAD7533)
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultrifle", 120, WeaponCategory.AR, 3, 3220176749, false, true, false) { PossibleComponents = ARComponents });


        List<WeaponComponent> ARMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x8610343F),
            new WeaponComponent("Extended Clip", 0xD12ACA6F),
            new WeaponComponent("Tracer Rounds", 0xEF2C78C1),
            new WeaponComponent("Incendiary Rounds", 0xFB70D853),
            new WeaponComponent("Armor Piercing Rounds", 0xA7DD1E58),
            new WeaponComponent("Full Metal Jacket Rounds", 0x63E0A098),
            new WeaponComponent("Grip", 0x9D65907A),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Holographic Sight", 0x420FD713),
            new WeaponComponent("Small Scope", 0x49B2945),
            new WeaponComponent("Large Scope", 0xC66B6542),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE),
            new WeaponComponent("Default Barrel", 0x43A49D26),
            new WeaponComponent("Heavy Barrel", 0x5646C26A),
            new WeaponComponent("Digital Camo", 0x911B24AF),
            new WeaponComponent("Brushstroke Camo", 0x37E5444B),
            new WeaponComponent("Woodland Camo", 0x538B7B97),
            new WeaponComponent("Skull", 0x25789F72),
            new WeaponComponent("Sessanta Nove", 0xC5495F2D),
            new WeaponComponent("Perseus", 0xCF8B73B1),
            new WeaponComponent("Leopard", 0xA9BB2811),
            new WeaponComponent("Zebra", 0xFC674D54),
            new WeaponComponent("Geometric", 0x7C7FCD9B),
            new WeaponComponent("Boom!", 0xA5C38392),
            new WeaponComponent("Patriotic", 0xB9B15DB0)
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultrifle_mk2", 120, WeaponCategory.AR, 3, 0x394F415C, false, true, false) { PossibleComponents = ARMK2Components });


        List<WeaponComponent> CarbineRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x9FBE33EC),
            new WeaponComponent("Extended Clip", 0x91109691),
            new WeaponComponent("Box Magazine", 0xBA62E935),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Scope", 0xA0D89C42),
            new WeaponComponent("Suppressor", 0x837445AA),
            new WeaponComponent("Grip", 0xC164F53),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD89B9658)
        };
        WeaponsList.Add(new WeaponInformation("weapon_carbinerifle", 120, WeaponCategory.AR, 3, 2210333304, false, true, false) { PossibleComponents = CarbineRifleComponents });


        List<WeaponComponent> CarbineRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4C7A391E),
            new WeaponComponent("Extended Clip", 0x5DD5DBD5),
            new WeaponComponent("Tracer Rounds", 0x1757F566),
            new WeaponComponent("Incendiary Rounds", 0x3D25C2A7),
            new WeaponComponent("Armor Piercing Rounds", 0x255D5D57),
            new WeaponComponent("Full Metal Jacket Rounds", 0x44032F11),
            new WeaponComponent("Grip", 0x9D65907A),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Holographic Sight", 0x420FD713),
            new WeaponComponent("Small Scope", 0x49B2945),
            new WeaponComponent("Large Scope", 0xC66B6542),
            new WeaponComponent("Suppressor", 0x837445AA),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE),
            new WeaponComponent("Default Barrel", 0x833637FF),
            new WeaponComponent("Heavy Barrel", 0x8B3C480B),
            new WeaponComponent("Digital Camo", 0x4BDD6F16),
            new WeaponComponent("Brushstroke Camo", 0x406A7908),
            new WeaponComponent("Woodland Camo", 0x2F3856A4),
            new WeaponComponent("Skull", 0xE50C424D),
            new WeaponComponent("Sessanta Nove", 0xD37D1F2F),
            new WeaponComponent("Perseus", 0x86268483),
            new WeaponComponent("Leopard", 0xF420E076),
            new WeaponComponent("Zebra", 0xAAE14DF8),
            new WeaponComponent("Geometric", 0x9893A95D),
            new WeaponComponent("Boom!", 0x6B13CD3E),
            new WeaponComponent("Patriotic", 0xDA55CD3F)
        };
        WeaponsList.Add(new WeaponInformation("weapon_carbinerifle_mk2", 120, WeaponCategory.AR, 3, 0xFAD1F1C9, false, true, false) { PossibleComponents = CarbineRifleMK2Components });


        List<WeaponComponent> AdvancedRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xFA8FA10F),
            new WeaponComponent("Extended Clip", 0x8EC1C979),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Scope", 0xAA2C45B4),
            new WeaponComponent("Suppressor", 0x837445AA),
            new WeaponComponent("Gilded Gun Metal Finish", 0x377CD377)
        };
        WeaponsList.Add(new WeaponInformation("weapon_advancedrifle", 120, WeaponCategory.AR, 3, 2937143193, false, true, false) { PossibleComponents = AdvancedRifleComponents });


        List<WeaponComponent> SpecialCarbineComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xC6C7E581),
            new WeaponComponent("Extended Clip", 0x7C8BD10E),
            new WeaponComponent("Drum Magazine", 0x6B59AEAA),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Scope", 0xA0D89C42),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Grip", 0xC164F53),
            new WeaponComponent("Etched Gun Metal Finish", 0x730154F2)
        };
        WeaponsList.Add(new WeaponInformation("weapon_specialcarbine", 120, WeaponCategory.AR, 3, 3231910285, false, true, false) { PossibleComponents = SpecialCarbineComponents });


        List<WeaponComponent> SpecialCarbineMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x16C69281),
            new WeaponComponent("Extended Clip", 0xDE1FA12C),
            new WeaponComponent("Tracer Rounds", 0x8765C68A),
            new WeaponComponent("Incendiary Rounds", 0xDE011286),
            new WeaponComponent("Armor Piercing Rounds", 0x51351635),
            new WeaponComponent("Full Metal Jacket Rounds", 0x503DEA90),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Holographic Sight", 0x420FD713),
            new WeaponComponent("Small Scope", 0x49B2945),
            new WeaponComponent("Large Scope", 0xC66B6542),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE),
            new WeaponComponent("Grip", 0x9D65907A),
            new WeaponComponent("Default Barrel", 0xE73653A9),
            new WeaponComponent("Heavy Barrel", 0xF97F783B),
            new WeaponComponent("Digital Camo", 0xD40BB53B),
            new WeaponComponent("Brushstroke Camo", 0x431B238B),
            new WeaponComponent("Woodland Camo", 0x34CF86F4),
            new WeaponComponent("Skull", 0xB4C306DD),
            new WeaponComponent("Sessanta Nove", 0xEE677A25),
            new WeaponComponent("Perseus", 0xDF90DC78),
            new WeaponComponent("Leopard", 0xA4C31EE),
            new WeaponComponent("Zebra", 0x89CFB0F7),
            new WeaponComponent("Geometric", 0x7B82145C),
            new WeaponComponent("Boom!", 0x899CAF75),
            new WeaponComponent("Patriotic", 0x5218C819)
        };
        WeaponsList.Add(new WeaponInformation("weapon_specialcarbine_mk2", 120, WeaponCategory.AR, 3, 0x969C3D67, false, true, false) { PossibleComponents = SpecialCarbineMK2Components });


        List<WeaponComponent> BullpupRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xC5A12F80),
            new WeaponComponent("Extended Clip", 0xB3688B0F),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Scope", 0xAA2C45B4),
            new WeaponComponent("Suppressor", 0x837445AA),
            new WeaponComponent("Grip", 0xC164F53),
            new WeaponComponent("Gilded Gun Metal Finish", 0xA857BC78)
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpuprifle", 120, WeaponCategory.AR, 3, 2132975508, false, true, false) { PossibleComponents = BullpupRifleComponents });


        List<WeaponComponent> BullpulRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x18929DA),
            new WeaponComponent("Extended Clip", 0xEFB00628),
            new WeaponComponent("Tracer Rounds", 0x822060A9),
            new WeaponComponent("Incendiary Rounds", 0xA99CF95A),
            new WeaponComponent("Armor Piercing Rounds", 0xFAA7F5ED),
            new WeaponComponent("Full Metal Jacket Rounds", 0x43621710),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Holographic Sight", 0x420FD713),
            new WeaponComponent("Small Scope", 0xC7ADD105),
            new WeaponComponent("Medium Scope", 0x3F3C8181),
            new WeaponComponent("Default Barrel", 0x659AC11B),
            new WeaponComponent("Heavy Barrel", 0x3BF26DC7),
            new WeaponComponent("Suppressor", 0x837445AA),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE),
            new WeaponComponent("Grip", 0x9D65907A),
            new WeaponComponent("Digital Camo", 0xAE4055B7),
            new WeaponComponent("Brushstroke Camo", 0xB905ED6B),
            new WeaponComponent("Woodland Camo", 0xA6C448E8),
            new WeaponComponent("Skull", 0x9486246C),
            new WeaponComponent("Sessanta Nove", 0x8A390FD2),
            new WeaponComponent("Perseus", 0x2337FC5),
            new WeaponComponent("Leopard", 0xEFFFDB5E),
            new WeaponComponent("Zebra", 0xDDBDB6DA),
            new WeaponComponent("Geometric", 0xCB631225),
            new WeaponComponent("Boom!", 0xA87D541E),
            new WeaponComponent("Patriotic", 0xC5E9AE52)
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpuprifle_mk2", 120, WeaponCategory.AR, 3, 0x84D6FAFD, false, true, false) { PossibleComponents = BullpulRifleMK2Components });


        List<WeaponComponent> CompactRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x513F0A63),
            new WeaponComponent("Extended Clip", 0x59FF9BF8),
            new WeaponComponent("Drum Magazine", 0xC607740E)
        };
        WeaponsList.Add(new WeaponInformation("weapon_compactrifle", 120, WeaponCategory.AR, 3, 1649403952, false, true, false) { PossibleComponents = CompactRifleComponents });

        //LMG

        List<WeaponComponent> MGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xF434EF84),
            new WeaponComponent("Extended Clip", 0x82158B47),
            new WeaponComponent("Scope", 0x3C00AFED),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD6DABABE)
        };
        WeaponsList.Add(new WeaponInformation("weapon_mg", 200, WeaponCategory.LMG, 4, 2634544996, false, true, false) { PossibleComponents = MGComponents });


        List<WeaponComponent> CombatMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xE1FFB34A),
            new WeaponComponent("Extended Clip", 0xD6C59CD6),
            new WeaponComponent("Scope", 0xA0D89C42),
            new WeaponComponent("Grip", 0xC164F53),
            new WeaponComponent("Etched Gun Metal Finish", 0x92FECCDD)
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatmg", 200, WeaponCategory.LMG, 4, 2144741730, false, true, false) { PossibleComponents = CombatMGComponents });


        List<WeaponComponent> CombatMGMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x492B257C),
            new WeaponComponent("Extended Clip", 0x17DF42E9),
            new WeaponComponent("Tracer Rounds", 0xF6649745),
            new WeaponComponent("Incendiary Rounds", 0xC326BDBA),
            new WeaponComponent("Armor Piercing Rounds", 0x29882423),
            new WeaponComponent("Full Metal Jacket Rounds", 0x57EF1CC8),
            new WeaponComponent("Grip", 0x9D65907A),
            new WeaponComponent("Holographic Sight", 0x420FD713),
            new WeaponComponent("Medium Scope", 0x3F3C8181),
            new WeaponComponent("Large Scope", 0xC66B6542),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE),
            new WeaponComponent("Default Barrel", 0xC34EF234),
            new WeaponComponent("Heavy Barrel", 0xB5E2575B),
            new WeaponComponent("Digital Camo", 0x4A768CB5),
            new WeaponComponent("Brushstroke Camo", 0xCCE06BBD),
            new WeaponComponent("Woodland Camo", 0xBE94CF26),
            new WeaponComponent("Skull", 0x7609BE11),
            new WeaponComponent("Sessanta Nove", 0x48AF6351),
            new WeaponComponent("Perseus", 0x9186750A),
            new WeaponComponent("Leopard", 0x84555AA8),
            new WeaponComponent("Zebra", 0x1B4C088B),
            new WeaponComponent("Geometric", 0xE046DFC),
            new WeaponComponent("Boom!", 0x28B536E),
            new WeaponComponent("Patriotic", 0xD703C94D)
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatmg_mk2", 200, WeaponCategory.LMG, 4, 0xDBBD7280, false, true, false) { PossibleComponents = CombatMGMK2Components });

        List<WeaponComponent> GusenbergComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x1CE5A6A5),
            new WeaponComponent("Extended Clip", 0xEAC8C270)
        };
        WeaponsList.Add(new WeaponInformation("weapon_gusenberg", 200, WeaponCategory.LMG, 4, 1627465347, false, true, false) { PossibleComponents = GusenbergComponents });

        //Sniper

        List<WeaponComponent> SniperRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x9BC64089),
            new WeaponComponent("Suppressor", 0xA73D4664),
            new WeaponComponent("Scope", 0xD2443DDC),
            new WeaponComponent("Advanced Scope", 0xBC54DA77),
            new WeaponComponent("Etched Wood Grip Finish", 0x4032B5E7)
        };
        WeaponsList.Add(new WeaponInformation("weapon_sniperrifle", 40, WeaponCategory.Sniper, 4, 100416529, false, true, true) { PossibleComponents = SniperRifleComponents });


        List<WeaponComponent> HeavySniperComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x476F52F4),
            new WeaponComponent("Scope", 0xD2443DDC),
            new WeaponComponent("Advanced Scope", 0xBC54DA77)
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavysniper", 40, WeaponCategory.Sniper, 4, 205991906, false, true, true) { PossibleComponents = HeavySniperComponents });


        List<WeaponComponent> HeavySniperMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xFA1E1A28),
            new WeaponComponent("Extended Clip", 0x2CD8FF9D),
            new WeaponComponent("Incendiary Rounds", 0xEC0F617),
            new WeaponComponent("Armor Piercing Rounds", 0xF835D6D4),
            new WeaponComponent("Full Metal Jacket Rounds", 0x3BE948F6),
            new WeaponComponent("Explosive Rounds", 0x89EBDAA7),
            new WeaponComponent("Zoom Scope", 0x82C10383),
            new WeaponComponent("Advanced Scope", 0xBC54DA77),
            new WeaponComponent("Night Vision Scope", 0xB68010B0),
            new WeaponComponent("Thermal Scope", 0x2E43DA41),
            new WeaponComponent("Suppressor", 0xAC42DF71),
            new WeaponComponent("Squared Muzzle Brake", 0x5F7DCE4D),
            new WeaponComponent("Bell-End Muzzle Brake", 0x6927E1A1),
            new WeaponComponent("Default Barrel", 0x909630B7),
            new WeaponComponent("Heavy Barrel", 0x108AB09E),
            new WeaponComponent("Digital Camo", 0xF8337D02),
            new WeaponComponent("Brushstroke Camo", 0xC5BEDD65),
            new WeaponComponent("Woodland Camo", 0xE9712475),
            new WeaponComponent("Skull", 0x13AA78E7),
            new WeaponComponent("Sessanta Nove", 0x26591E50),
            new WeaponComponent("Perseus", 0x302731EC),
            new WeaponComponent("Leopard", 0xAC722A78),
            new WeaponComponent("Zebra", 0xBEA4CEDD),
            new WeaponComponent("Geometric", 0xCD776C82),
            new WeaponComponent("Boom!", 0xABC5ACC7),
            new WeaponComponent("Patriotic", 0x6C32D2EB)
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavysniper_mk2", 40, WeaponCategory.Sniper, 4, 0xA914799, false, true, true) { PossibleComponents = HeavySniperMK2Components });


        List<WeaponComponent> MarksmanRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xD83B4141),
            new WeaponComponent("Extended Clip", 0xCCFD2AC5),
            new WeaponComponent("Scope", 0x1C221B1A),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Suppressor", 0x837445AA),
            new WeaponComponent("Grip", 0xC164F53),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x161E9241)
        };
        WeaponsList.Add(new WeaponInformation("weapon_marksmanrifle", 40, WeaponCategory.Sniper, 4, 3342088282, false, true, true) { PossibleComponents = MarksmanRifleComponents });


        List<WeaponComponent> MarksmanRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94E12DCE),
            new WeaponComponent("Extended Clip", 0xE6CFD1AA),
            new WeaponComponent("Tracer Rounds", 0xD77A22D2),
            new WeaponComponent("Incendiary Rounds", 0x6DD7A86E),
            new WeaponComponent("Armor Piercing Rounds", 0xF46FD079),
            new WeaponComponent("Full Metal Jacket Rounds", 0xE14A9ED3),
            new WeaponComponent("Holographic Sight", 0x420FD713),
            new WeaponComponent("Large Scope", 0xC66B6542),
            new WeaponComponent("Zoom Scope", 0x5B1C713C),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Suppressor", 0x837445AA),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE),
            new WeaponComponent("Default Barrel", 0x381B5D89),
            new WeaponComponent("Heavy Barrel", 0x68373DDC),
            new WeaponComponent("Grip", 0x9D65907A),
            new WeaponComponent("Digital Camo", 0x9094FBA0),
            new WeaponComponent("Brushstroke Camo", 0x7320F4B2),
            new WeaponComponent("Woodland Camo", 0x60CF500F),
            new WeaponComponent("Skull", 0xFE668B3F),
            new WeaponComponent("Sessanta Nove", 0xF3757559),
            new WeaponComponent("Perseus", 0x193B40E8),
            new WeaponComponent("Leopard", 0x107D2F6C),
            new WeaponComponent("Zebra", 0xC4E91841),
            new WeaponComponent("Geometric", 0x9BB1C5D3),
            new WeaponComponent("Boom!", 0x3B61040B),
            new WeaponComponent("Boom!", 0xB7A316DA)
        };
        WeaponsList.Add(new WeaponInformation("weapon_marksmanrifle_mk2", 40, WeaponCategory.Sniper, 4, 0x6A6C02E0, false, true, true) { PossibleComponents = MarksmanRifleMK2Components });

        //Heavy
        WeaponsList.Add(new WeaponInformation("weapon_rpg", 3, WeaponCategory.Heavy, 4, 2982836145, false, true, false));

        List<WeaponComponent> GrenadeLauncherComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x11AE5C97),
            new WeaponComponent("Flashlight", 0x7BC4CDDC),
            new WeaponComponent("Grip", 0xC164F53),
            new WeaponComponent("Scope", 0xAA2C45B4)
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
