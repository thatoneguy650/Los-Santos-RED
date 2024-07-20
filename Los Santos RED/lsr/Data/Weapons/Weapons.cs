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
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Weapons*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Weapons config: {ConfigFile.FullName}",0);
            WeaponsList = Serialization.DeserializeParams<WeaponInformation>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Issuable Weapons config  {ConfigFileName}",0);
            WeaponsList = Serialization.DeserializeParams<WeaponInformation>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Weapons config found, creating default", 0);
            DefaultConfig();
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
            if (NativeFunction.Natives.HAS_PED_BEEN_DAMAGED_BY_WEAPON<bool>(Pedestrian, MyWeapon.Hash, 0))
            {
                NativeFunction.Natives.CLEAR_PED_LAST_WEAPON_DAMAGE<bool>(Pedestrian);
                return MyWeapon;
            }
        }
        if (NativeFunction.Natives.HAS_PED_BEEN_DAMAGED_BY_WEAPON<bool>(Pedestrian, 0, 1))
        {
            return new WeaponInformation("Generic Melee", 0, WeaponCategory.Melee, 0, 0, false, false, false);
        }

        if (NativeFunction.Natives.HAS_PED_BEEN_DAMAGED_BY_WEAPON<bool>(Pedestrian, 0, 2))
        {
            return new WeaponInformation("Generic Weapon", 0, WeaponCategory.Melee, 0, 0, false, false, false);
        }

        if (NativeFunction.Natives.HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE<bool>(Pedestrian))
        {
            return new WeaponInformation("Vehicle Injury", 0, WeaponCategory.Vehicle, 0, 0, false, false, false);
        }
        else
        {
            return new WeaponInformation("Unknown", 0, WeaponCategory.Unknown, 0, 0, false, false, false);
        }
    }
    public List<WeaponInformation> GetAllWeapons() => WeaponsList;
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
    public WeaponInformation GetRandomRegularWeapon()
    {
        return WeaponsList.Where(x => x.IsRegular).PickRandom();
    }
    public WeaponInformation GetCurrentWeapon(Ped Pedestrian)
    {
        if (Pedestrian.Inventory.EquippedWeapon == null)
            return null;
        ulong myHash = (ulong)Pedestrian.Inventory.EquippedWeapon.Hash;
        WeaponInformation CurrentGun = GetWeapon(myHash);
        if (CurrentGun != null && CurrentGun.Hash != (ulong)2725352035)//is not unarmed weapon
            return CurrentGun;
        else
            return null;
    }
    public WeaponVariation GetWeaponVariation(Ped WeaponOwner, uint WeaponHash)
    {
        int Tint = NativeFunction.Natives.GET_PED_WEAPON_TINT_INDEX<int>(WeaponOwner, WeaponHash);
        WeaponInformation MyGun = GetWeapon(WeaponHash);
        if (MyGun == null)
            return new WeaponVariation(Tint);

        List<WeaponComponent> ComponentsOnGun = new List<WeaponComponent>();

        foreach (WeaponComponent PossibleComponent in MyGun.PossibleComponents)
        {
            if (NativeFunction.Natives.HAS_PED_GOT_WEAPON_COMPONENT<bool>(WeaponOwner, WeaponHash, PossibleComponent.Hash))
            {
                ComponentsOnGun.Add(PossibleComponent);
            }

        }
        return new WeaponVariation(ComponentsOnGun);

    }
    public WeaponVariation GetRandomVariation(uint WeaponHash, float ComponentChance)
    {
        WeaponVariation myVar = new WeaponVariation(0);
        WeaponInformation MyGun = GetWeapon(WeaponHash);
        if (MyGun == null)
        {
            return myVar;
        }   
        foreach (ComponentSlot componentSlot in MyGun.PossibleComponents.GroupBy(x=> x.ComponentSlot).Select(x=> x.Key))
        {
            if (RandomItems.RandomPercent(ComponentChance))
            {
                WeaponComponent weaponComponent = MyGun.PossibleComponents.Where(x => x.ComponentSlot == componentSlot).PickRandom();
                if (weaponComponent == null)
                {
                    continue;
                }
                myVar.Components.Add(weaponComponent);
            }
        }
        return myVar;  
    }
    private void DefaultConfig()
    {
        WeaponsList = new List<WeaponInformation>();
        DefaultConfig_Melee();
        DefaultConfig_Pistol();
        DefaultConfig_Shotgun();
        DefaultConfig_SMG();
        DefaultConfig_AR();
        DefaultConfig_LMG();
        DefaultConfig_Sniper();
        DefaultConfig_Heavy();
        DefaultConfig_Throwable();
        DefaultConfig_Other();
        Serialization.SerializeParams(WeaponsList, ConfigFileName);
    }

    private void DefaultConfig_Other()
    {

    }

    private void DefaultConfig_Melee()
    {
        //Melee
        WeaponsList.Add(new WeaponInformation("weapon_dagger", 0, WeaponCategory.Melee, 0, 2460120199, false, false, true));
        WeaponsList.Add(new WeaponInformation("weapon_bat", 0, WeaponCategory.Melee, 0, 2508868239, false, false, true) { DoesNotTriggerBrandishing = true });
        WeaponsList.Add(new WeaponInformation("weapon_bottle", 0, WeaponCategory.Melee, 0, 4192643659, false, false, true) { DoesNotTriggerBrandishing = true });
        WeaponsList.Add(new WeaponInformation("weapon_crowbar", 0, WeaponCategory.Melee, 0, 2227010557, false, false, true) { DoesNotTriggerBrandishing = true });
        WeaponsList.Add(new WeaponInformation("weapon_flashlight", 0, WeaponCategory.Melee, 0, 2343591895, false, false, true) { DoesNotTriggerBrandishing = true });
        WeaponsList.Add(new WeaponInformation("weapon_golfclub", 0, WeaponCategory.Melee, 0, 1141786504, false, false, true) { DoesNotTriggerBrandishing = true });
        WeaponsList.Add(new WeaponInformation("weapon_hammer", 0, WeaponCategory.Melee, 0, 1317494643, false, false, true) { DoesNotTriggerBrandishing = true });
        WeaponsList.Add(new WeaponInformation("weapon_hatchet", 0, WeaponCategory.Melee, 0, 4191993645, false, false, true));

        List<WeaponComponent> KnuckleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Base Model", 0xF3462F33,ComponentSlot.Coloring),
            new WeaponComponent("The Pimp", 0xC613F685,ComponentSlot.Coloring),
            new WeaponComponent("The Ballas", 0xEED9FD63,ComponentSlot.Coloring),
            new WeaponComponent("The Hustler", 0x50910C31,ComponentSlot.Coloring),
            new WeaponComponent("The Rock", 0x9761D9DC,ComponentSlot.Coloring),
            new WeaponComponent("The Hater", 0x7DECFE30,ComponentSlot.Coloring),
            new WeaponComponent("The Lover", 0x3F4E8AA6,ComponentSlot.Coloring),
            new WeaponComponent("The Player", 0x8B808BB,ComponentSlot.Coloring),
            new WeaponComponent("The King", 0xE28BABEF,ComponentSlot.Coloring),
            new WeaponComponent("The Vagos", 0x7AF3F785,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_knuckle", 0, WeaponCategory.Melee, 0, 3638508604, false, false, true) { IsRegular = false, PossibleComponents = KnuckleComponents });
        WeaponsList.Add(new WeaponInformation("weapon_knife", 0, WeaponCategory.Melee, 0, 2578778090, false, false, true));
        WeaponsList.Add(new WeaponInformation("weapon_machete", 0, WeaponCategory.Melee, 0, 3713923289, false, false, true));

        List<WeaponComponent> SwitchbladeComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Handle", 0x9137A500,ComponentSlot.Coloring),
            new WeaponComponent("VIP Variant", 0x5B3E7DB6,ComponentSlot.Coloring),
            new WeaponComponent("Bodyguard Variant", 0xE7939662,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_switchblade", 0, WeaponCategory.Melee, 0, 3756226112, false, false, false) { PossibleComponents = SwitchbladeComponents });

        WeaponsList.Add(new WeaponInformation("weapon_nightstick", 0, WeaponCategory.Melee, 0, 1737195953, false, false, true));
        WeaponsList.Add(new WeaponInformation("weapon_wrench", 0, WeaponCategory.Melee, 0, 0x19044EE0, false, false, true) { DoesNotTriggerBrandishing = true });
        WeaponsList.Add(new WeaponInformation("weapon_battleaxe", 0, WeaponCategory.Melee, 0, 3441901897, false, false, true) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_poolcue", 0, WeaponCategory.Melee, 0, 0x94117305, false, false, true) { DoesNotTriggerBrandishing = true });
        WeaponsList.Add(new WeaponInformation("weapon_stone_hatchet", 0, WeaponCategory.Melee, 0, 0x3813FC08, false, false, true) { IsRegular = false });


        WeaponsList.Add(new WeaponInformation("weapon_fireextinguisher", 0, WeaponCategory.Melee, 0, 0x060EC506, false, false, true) { DoesNotTriggerBrandishing = true, SelectorOptions = SelectorOptions.FullAuto | SelectorOptions.Safe, IsRegular = false });

    }
    private void DefaultConfig_Pistol()
    {
        //Pistol
        List<WeaponComponent> PistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xFED0FD71,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xED265A1C,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x359B7AAE,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0x65EA7EBB,ComponentSlot.Muzzle),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD7391086,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol", 60, WeaponCategory.Pistol, 1, 453432689, true, false, true, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = PistolComponents, CanPistolSuicide = true });

        List<WeaponComponent> PistolMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94F42D62,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x5ED6C128,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0x25CAAEAF,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0x2BBD7A3A,ComponentSlot.Rounds),
            new WeaponComponent("Hollow Point Rounds", 0x85FEA109,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0x4F37DF2A,ComponentSlot.Rounds),
            new WeaponComponent("Mounted Scope", 0x8ED4BB70, ComponentSlot.Optic),
            new WeaponComponent("Flashlight", 0x43FD595B,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0x65EA7EBB,ComponentSlot.Muzzle),
            new WeaponComponent("Compensator", 0x21E34793,ComponentSlot.Muzzle),
            new WeaponComponent("Digital Camo", 0x5C6C749C,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x15F7A390,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x968E24DB,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0x17BFA99,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0xF2685C72,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0xDD2231E6,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0xBB43EE76,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0x4D901310,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x5F31B653,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x697E19A0,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0x930CB951,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0xB4FC92B0,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0x1A1F1260,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0xE4E00B70,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0x2C298B2B,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0xDFB79725,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0x6BD7228C,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0x9DDBCF8C,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0xB319A52C,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0xC6836E12,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0x43B1B173,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0x4ABDA3FA,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol_mk2", 60, WeaponCategory.Pistol, 1, 0xBFE256D4, true, false, true, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = PistolMK2Components, CanPistolSuicide = true });

        List<WeaponComponent> CombatPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x721B079,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xD67B4F2D,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x359B7AAE,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xC304849A,ComponentSlot.Muzzle),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xC6654D72,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatpistol", 60, WeaponCategory.Pistol, 1, 1593441988, true, false, true, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = CombatPistolComponents, CanPistolSuicide = true });


        List<WeaponComponent> APPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x31C4B22A,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x249A17D5,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x359B7AAE,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xC304849A,ComponentSlot.Muzzle),
            new WeaponComponent("Gilded Gun Metal Finish", 0x9B76C72C,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_appistol", 60, WeaponCategory.Pistol, 1, 584646201, true, false, false, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = APPistolComponents, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_stungun", 0, WeaponCategory.Melee, 0, 911657153, true, false, true, 0.2f, 0.3f, 0.1f, 0.2f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { CanPistolSuicide = false, IsTaser = true });

        List<WeaponComponent> Pistol50Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x2297BE19,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xD9D3AC92,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x359B7AAE,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Platinum Pearl Deluxe Finish", 0x77B8AB2F,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistol50", 60, WeaponCategory.Pistol, 1, 2578377531, true, false, true, 1.5f, 1.7f, 1.5f, 1.7f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = Pistol50Components, CanPistolSuicide = true });

        List<WeaponComponent> SNSPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xF8802ED9,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x7B0033B3,ComponentSlot.Magazine),
            new WeaponComponent("Etched Wood Grip Finish", 0x8033ECAF,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_snspistol", 60, WeaponCategory.Pistol, 1, 3218215474, true, false, true, 1.2f, 1.4f, 0.9f, 1.2f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = SNSPistolComponents, CanPistolSuicide = true });

        List<WeaponComponent> SNSPistolMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x1466CE6,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xCE8C0772,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0x902DA26E,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0xE6AD5F79,ComponentSlot.Rounds),
            new WeaponComponent("Hollow Point Rounds", 0x8D107402,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0xC111EB26,ComponentSlot.Rounds),
            new WeaponComponent("Flashlight", 0x4A4965F3,ComponentSlot.Light),
            new WeaponComponent("Mounted Scope", 0x47DE9258, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0x65EA7EBB,ComponentSlot.Muzzle),
            new WeaponComponent("Compensator", 0xAA8283BF,ComponentSlot.Muzzle),
            new WeaponComponent("Digital Camo", 0xF7BEEDD,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x8A612EF6,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x76FA8829,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0xA93C6CAC,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0x9C905354,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0x4DFA3621,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0x42E91FFF,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0x54A8437D,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x68C2746,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x2366E467,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x441882E6,ComponentSlot.Coloring),
            new WeaponComponent("Digital Camo", 0xE7EE68EA,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x29366D21,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x3ADE514B,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0xE64513E9,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0xCD7AEB9A,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0xFA7B27A6,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0xE285CA9A,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0x2B904B19,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x22C24F9C,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x8D0D5ECD,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0x1F07150A,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_snspistol_mk2", 60, WeaponCategory.Pistol, 1, 0x88374054, true, false, true, 1.2f, 1.4f, 0.9f, 1.2f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = SNSPistolMK2Components, CanPistolSuicide = true });

        List<WeaponComponent> HeavyPistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xD4A969A,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x64F9C62B,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x359B7AAE,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xC304849A,ComponentSlot.Muzzle),
            new WeaponComponent("Etched Wood Grip Finish", 0x7A6A7B7B,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavypistol", 60, WeaponCategory.Pistol, 1, 3523564046, true, false, true, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = HeavyPistolComponents, CanPistolSuicide = true });

        List<WeaponComponent> VintagePistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x45A3B6BB,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x33BA12E8,ComponentSlot.Magazine),
            new WeaponComponent("Suppressor", 0xC304849A,ComponentSlot.Muzzle)
        };
        WeaponsList.Add(new WeaponInformation("weapon_vintagepistol", 60, WeaponCategory.Pistol, 1, 137902532, true, false, true, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = VintagePistolComponents, CanPistolSuicide = true });


        WeaponsList.Add(new WeaponInformation("weapon_flaregun", 60, WeaponCategory.Pistol, 1, 1198879012, true, false, true, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false, CanPistolSuicide = false });
        WeaponsList.Add(new WeaponInformation("weapon_marksmanpistol", 60, WeaponCategory.Pistol, 1, 3696079510, true, false, true, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false, CanPistolSuicide = false });

        List<WeaponComponent> RevolverComponents = new List<WeaponComponent>
        {
            new WeaponComponent("VIP Variant", 0x16EE3040,ComponentSlot.Coloring),
            new WeaponComponent("Bodyguard Variant", 0x9493B80D,ComponentSlot.Coloring),
            new WeaponComponent("Default Clip", 0xE9867CE3,ComponentSlot.Magazine)
        };
        WeaponsList.Add(new WeaponInformation("weapon_revolver", 60, WeaponCategory.Pistol, 1, 3249783761, true, false, true, 1.5f, 1.7f, 1.5f, 1.7f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = RevolverComponents, CanPistolSuicide = true });

        List<WeaponComponent> RevolverMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Rounds", 0xBA23D8BE,ComponentSlot.Rounds),
            new WeaponComponent("Tracer Rounds", 0xC6D8E476,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0xEFBF25,ComponentSlot.Rounds),
            new WeaponComponent("Hollow Point Rounds", 0x10F42E8F,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0xDC8BA3F,ComponentSlot.Rounds),
            new WeaponComponent("Holographic Sight", 0x420FD713, ComponentSlot.Optic),
            new WeaponComponent("Small Scope", 0x49B2945, ComponentSlot.Optic),
            new WeaponComponent("Flashlight", 0x359B7AAE,ComponentSlot.Light),
            new WeaponComponent("Compensator", 0x27077CCB, ComponentSlot.Muzzle),
            new WeaponComponent("Digital Camo", 0xC03FED9F,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0xB5DE24,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0xA7FF1B8,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0xF2E24289,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0x11317F27,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0x17C30C42,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0x257927AE,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0x37304B1C,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x48DAEE71,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x20ED9B5B,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0xD951E867,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_revolver_mk2", 60, WeaponCategory.Pistol, 1, 0xCB96392F, true, false, true, 1.5f, 1.7f, 1.5f, 1.7f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = RevolverMK2Components, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_doubleaction", 60, WeaponCategory.Pistol, 1, 0x97EA20B8, true, false, true, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { CanPistolSuicide = true });
        List<WeaponComponent> RayGunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Festive tint", 0xD7DBF707,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_raypistol", 60, WeaponCategory.Pistol, 1, 0xAF3696A1, true, false, false, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false, PossibleComponents = RayGunComponents, CanPistolSuicide = false });

        WeaponsList.Add(new WeaponInformation("weapon_gadgetpistol", 60, WeaponCategory.Pistol, 1, 0x57A4368C, true, false, false, 1.0f, 1.2f, 0.7f, 0.9f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_navyrevolver", 60, WeaponCategory.Pistol, 1, 0x917F6C8C, true, false, false, 1.0f, 1.2f, 0.7f, 0.9f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false, CanPistolSuicide = true });


        List<WeaponComponent> CeramicComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x54D41361,ComponentSlot.Magazine)
            ,new WeaponComponent("Extended Clip", 0x81786CA9,ComponentSlot.Magazine)
            ,new WeaponComponent("Suppressor", 0x9307D6FA,ComponentSlot.Muzzle)
        };
        WeaponsList.Add(new WeaponInformation("weapon_ceramicpistol", 60, WeaponCategory.Pistol, 1, 0x2B5EF5EC, true, false, false, 1.2f, 1.4f, 0.9f, 1.2f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = true, PossibleComponents = CeramicComponents, CanPistolSuicide = true });


        List<WeaponComponent> WM29Componenets = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x1663E75E,ComponentSlot.Magazine)
            ,new WeaponComponent("Suppressor", 0x1E02B7E0,ComponentSlot.Muzzle)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pistolxm3", 60, WeaponCategory.Pistol, 1, 0x1BC4FDB9, true, false, false, 1.2f, 1.4f, 0.9f, 1.2f, 0.75f, 1.0f, 0.75f, 1.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto)
        { IsRegular = true, PossibleComponents = WM29Componenets, CanPistolSuicide = true });
    }
    private void DefaultConfig_Shotgun()
    {
        //Shotgun
        List<WeaponComponent> PumpShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xE608B35E,ComponentSlot.Muzzle),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xA2D79DDB,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pumpshotgun", 32, WeaponCategory.Shotgun, 2, 487013001, false, true, true, 1.2f, 1.4f, 0.9f, 1.2f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = PumpShotgunComponents });

        List<WeaponComponent> PumpShotgunMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Shells", 0xCD940141,ComponentSlot.Rounds),
            new WeaponComponent("Dragon's Breath Shells", 0x9F8A1BF5,ComponentSlot.Rounds),
            new WeaponComponent("Steel Buckshot Shells", 0x4E65B425,ComponentSlot.Rounds),
            new WeaponComponent("Flechette Shells", 0xE9582927,ComponentSlot.Rounds),
            new WeaponComponent("Explosive Slugs", 0x3BE4465D,ComponentSlot.Rounds),
            new WeaponComponent("Holographic Sight", 0x420FD713, ComponentSlot.Optic),
            new WeaponComponent("Small Scope", 0x49B2945, ComponentSlot.Optic),
            new WeaponComponent("Medium Scope", 0x3F3C8181, ComponentSlot.Optic),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xAC42DF71,ComponentSlot.Muzzle),
            new WeaponComponent("Squared Muzzle Brake", 0x5F7DCE4D,ComponentSlot.Muzzle),
            new WeaponComponent("Digital Camo", 0xE3BD9E44,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x17148F9B,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x24D22B16,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0xF2BEC6F0,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0x85627D,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0xDC2919C5,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0xE184247B,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0xD8EF9356,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0xEF29BFCA,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x67AEB165,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0x46411A1D,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_pumpshotgun_mk2", 32, WeaponCategory.Shotgun, 2, 0x555AF99A, false, true, true, 1.2f, 1.4f, 0.9f, 1.2f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = PumpShotgunMK2Components });

        List<WeaponComponent> SawnOffShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Gilded Gun Metal Finish", 0x85A64DF9,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_sawnoffshotgun", 32, WeaponCategory.Shotgun, 2, 2017895192, false, true, false, 1.5f, 1.9f, 1.5f, 1.8f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = SawnOffShotgunComponents });

        List<WeaponComponent> AssaultShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94E81BC7,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x86BD7F72,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip)
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultshotgun", 32, WeaponCategory.Shotgun, 2, 3800352039, false, true, false, 1.2f, 1.4f, 0.9f, 1.2f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = AssaultShotgunComponents });

        List<WeaponComponent> BullpupShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip)
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpupshotgun", 32, WeaponCategory.Shotgun, 2, 2640438543, false, true, true, 1.2f, 1.4f, 0.9f, 1.2f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = BullpupShotgunComponents });

        WeaponsList.Add(new WeaponInformation("weapon_musket", 32, WeaponCategory.Shotgun, 2, 2828843422, false, true, true, 1.2f, 1.4f, 0.9f, 1.2f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false });

        List<WeaponComponent> HeavyShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x324F2D5F,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x971CF6FD,ComponentSlot.Magazine),
            new WeaponComponent("Drum Magazine", 0x88C7DA53, ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip)
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavyshotgun", 32, WeaponCategory.Shotgun, 2, 984333226, false, true, true, 1.2f, 1.4f, 0.9f, 1.2f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = HeavyShotgunComponents });

        WeaponsList.Add(new WeaponInformation("weapon_dbshotgun", 32, WeaponCategory.Shotgun, 2, 4019527611, false, true, false, 1.5f, 1.9f, 1.5f, 1.8f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst) { CanPistolSuicide = true });
        WeaponsList.Add(new WeaponInformation("weapon_autoshotgun", 32, WeaponCategory.Shotgun, 2, 317205821, false, true, false, 1.2f, 1.4f, 0.9f, 1.2f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto));

        List<WeaponComponent> CombatShotgunComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatshotgun", 32, WeaponCategory.Shotgun, 2, 0x5A96BA4, false, true, true, 1.2f, 1.4f, 0.9f, 1.2f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = CombatShotgunComponents });

    }
    private void DefaultConfig_SMG()
    {
        //SMG

        List<WeaponComponent> MicroSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xCB48AEF0,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x10E6BA2B,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x359B7AAE,ComponentSlot.Light),
            new WeaponComponent("Scope", 0x9D2FBF29, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x487AAE09,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_microsmg", 90, WeaponCategory.SMG, 2, 324215364, true, false, false, 0.9f, 1.2f, 0.5f, 0.7f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = MicroSMGComponents, CanPistolSuicide = true });


        List<WeaponComponent> SMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x26574997,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x350966FB,ComponentSlot.Magazine),
            new WeaponComponent("Drum Magazine", 0x79C77076, ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Scope", 0x3CC6BA57, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xC304849A,ComponentSlot.Muzzle),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x27872C90,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_smg", 90, WeaponCategory.SMG, 2, 736523883, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst | SelectorOptions.TwoRoundBurst | SelectorOptions.FullAuto) { PossibleComponents = SMGComponents });


        List<WeaponComponent> SMGMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4C24806E,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xB9835B2E,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0x7FEA36EC,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0xD99222E5,ComponentSlot.Rounds),
            new WeaponComponent("Hollow Point Rounds", 0x3A1BD6FA,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0xB5A715F,ComponentSlot.Rounds),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Holographic Sight", 0x9FDB5652, ComponentSlot.Optic),
            new WeaponComponent("Small Scope", 0xE502AB6B, ComponentSlot.Optic),
            new WeaponComponent("Medium Scope", 0x3DECC7DA, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xC304849A,ComponentSlot.Muzzle),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4, ComponentSlot.Muzzle),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B, ComponentSlot.Muzzle),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF, ComponentSlot.Muzzle),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC, ComponentSlot.Muzzle),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A, ComponentSlot.Muzzle),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC, ComponentSlot.Muzzle),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE, ComponentSlot.Muzzle),
            new WeaponComponent("Default Barrel", 0xD9103EE1, ComponentSlot.Barrel),
            new WeaponComponent("Heavy Barrel", 0xA564D78B, ComponentSlot.Barrel),
            new WeaponComponent("Digital Camo", 0xC4979067,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x3815A945,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x4B4B4FB0,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0xEC729200,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0x48F64B22,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0x35992468,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0x24B782A5,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0xA2E67F01,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x2218FD68,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x45C5C3C5,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0x399D558F,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_smg_mk2", 90, WeaponCategory.SMG, 2, 0x78A97CD0, false, true, false, 0.9f, 1.2f, 0.5f, 0.7f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst | SelectorOptions.TwoRoundBurst | SelectorOptions.FullAuto) { PossibleComponents = SMGMK2Components });//no stock


        List<WeaponComponent> AssaultSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x8D1307B0,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xBB46E417,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Scope", 0x9D2FBF29, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x278C78AF,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultsmg", 32, WeaponCategory.SMG, 2, 4024951519, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst | SelectorOptions.FullAuto) { PossibleComponents = AssaultSMGComponents });


        List<WeaponComponent> CombatPDWComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4317F19E,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x334A5203,ComponentSlot.Magazine),
            new WeaponComponent("Drum Magazine", 0x6EB8C8DB, ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
            new WeaponComponent("Scope", 0xAA2C45B4, ComponentSlot.Optic)
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatpdw", 90, WeaponCategory.SMG, 2, 171789620, false, true, false, 0.2f, 0.3f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst | SelectorOptions.TwoRoundBurst | SelectorOptions.FullAuto) { PossibleComponents = CombatPDWComponents });


        List<WeaponComponent> MachinePistolComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x476E85FF,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xB92C6979,ComponentSlot.Magazine),
            new WeaponComponent("Drum Magazine", 0xA9E9CAF4,ComponentSlot.Magazine),
            new WeaponComponent("Suppressor", 0xC304849A,ComponentSlot.Muzzle)
        };
        WeaponsList.Add(new WeaponInformation("weapon_machinepistol", 90, WeaponCategory.SMG, 2, 3675956304, true, false, false, 0.9f, 1.2f, 0.5f, 0.7f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = MachinePistolComponents, CanPistolSuicide = true });



        List<WeaponComponent> MiniSMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x84C8B2D3,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x937ED0B7,ComponentSlot.Magazine)
        };
        WeaponsList.Add(new WeaponInformation("weapon_minismg", 90, WeaponCategory.SMG, 2, 3173288789, true, false, false, 0.9f, 1.2f, 0.5f, 0.7f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = MiniSMGComponents, CanPistolSuicide = true });

        WeaponsList.Add(new WeaponInformation("weapon_raycarbine", 32, WeaponCategory.SMG, 2, 0x476BF155, true, false, false, 0.9f, 1.2f, 0.5f, 0.7f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false });


        List<WeaponComponent> TecPistolComponenets = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x383664EE,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x1286198E,ComponentSlot.Magazine),
            new WeaponComponent("Scope", 0x9D2FBF29, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle)
        };
        WeaponsList.Add(new WeaponInformation("weapon_tecpistol", 90, WeaponCategory.SMG, 2, 350597077, true, false, false, 0.9f, 1.2f, 0.5f, 0.7f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = TecPistolComponenets, CanPistolSuicide = true });

        //350597077
    }
    private void DefaultConfig_AR()
    {
        List<WeaponComponent> ARComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xBE5EEA16,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xB1214F9B,ComponentSlot.Magazine),
            new WeaponComponent("Drum Magazine", 0xDBF0A53D, ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Scope", 0x9D2FBF29, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x4EAD7533,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultrifle", 120, WeaponCategory.AR, 3, 3220176749, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = ARComponents });
        List<WeaponComponent> ARMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x8610343F,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xD12ACA6F,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0xEF2C78C1,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0xFB70D853,ComponentSlot.Rounds),
            new WeaponComponent("Armor Piercing Rounds", 0xA7DD1E58,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0x63E0A098,ComponentSlot.Rounds),
            new WeaponComponent("Grip", 0x9D65907A, ComponentSlot.ForwardGrip),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Holographic Sight", 0x420FD713, ComponentSlot.Optic),
            new WeaponComponent("Small Scope", 0x49B2945, ComponentSlot.Optic),
            new WeaponComponent("Large Scope", 0xC66B6542, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4,ComponentSlot.Muzzle),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B,ComponentSlot.Muzzle),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF,ComponentSlot.Muzzle),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC,ComponentSlot.Muzzle),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A,ComponentSlot.Muzzle),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC,ComponentSlot.Muzzle),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE,ComponentSlot.Muzzle),
            new WeaponComponent("Default Barrel", 0x43A49D26,ComponentSlot.Barrel),
            new WeaponComponent("Heavy Barrel", 0x5646C26A,ComponentSlot.Barrel),
            new WeaponComponent("Digital Camo", 0x911B24AF,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x37E5444B,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x538B7B97,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0x25789F72,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0xC5495F2D,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0xCF8B73B1,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0xA9BB2811,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0xFC674D54,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x7C7FCD9B,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0xA5C38392,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0xB9B15DB0,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_assaultrifle_mk2", 120, WeaponCategory.AR, 3, 0x394F415C, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = ARMK2Components });
        List<WeaponComponent> CarbineRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x9FBE33EC,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x91109691,ComponentSlot.Magazine),
            new WeaponComponent("Box Magazine", 0xBA62E935,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Scope", 0xA0D89C42, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD89B9658,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_carbinerifle", 120, WeaponCategory.AR, 3, 2210333304, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = CarbineRifleComponents });
        List<WeaponComponent> CarbineRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x4C7A391E,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x5DD5DBD5,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0x1757F566,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0x3D25C2A7,ComponentSlot.Rounds),
            new WeaponComponent("Armor Piercing Rounds", 0x255D5D57,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0x44032F11,ComponentSlot.Rounds),
            new WeaponComponent("Grip", 0x9D65907A, ComponentSlot.ForwardGrip),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Holographic Sight", 0x420FD713, ComponentSlot.Optic),
            new WeaponComponent("Small Scope", 0x49B2945, ComponentSlot.Optic),
            new WeaponComponent("Large Scope", 0xC66B6542, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4,ComponentSlot.Muzzle),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B,ComponentSlot.Muzzle),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF,ComponentSlot.Muzzle),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC,ComponentSlot.Muzzle),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A,ComponentSlot.Muzzle),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC,ComponentSlot.Muzzle),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE,ComponentSlot.Muzzle),
            new WeaponComponent("Default Barrel", 0x833637FF,ComponentSlot.Barrel),
            new WeaponComponent("Heavy Barrel", 0x8B3C480B,ComponentSlot.Barrel),
            new WeaponComponent("Digital Camo", 0x4BDD6F16,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x406A7908,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x2F3856A4,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0xE50C424D,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0xD37D1F2F,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0x86268483,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0xF420E076,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0xAAE14DF8,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x9893A95D,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x6B13CD3E,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0xDA55CD3F,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_carbinerifle_mk2", 120, WeaponCategory.AR, 3, 0xFAD1F1C9, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = CarbineRifleMK2Components });
        List<WeaponComponent> AdvancedRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xFA8FA10F,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x8EC1C979,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Scope", 0xAA2C45B4, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Gilded Gun Metal Finish", 0x377CD377,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_advancedrifle", 120, WeaponCategory.AR, 3, 2937143193, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst | SelectorOptions.FullAuto) { PossibleComponents = AdvancedRifleComponents });
        List<WeaponComponent> SpecialCarbineComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xC6C7E581,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x7C8BD10E,ComponentSlot.Magazine),
            new WeaponComponent("Drum Magazine", 0x6B59AEAA,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Scope", 0xA0D89C42, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
            new WeaponComponent("Etched Gun Metal Finish", 0x730154F2,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_specialcarbine", 120, WeaponCategory.AR, 3, 3231910285, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst | SelectorOptions.ThreeRoundBurst | SelectorOptions.FullAuto) { PossibleComponents = SpecialCarbineComponents });
        List<WeaponComponent> SpecialCarbineMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x16C69281,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xDE1FA12C,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0x8765C68A,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0xDE011286,ComponentSlot.Rounds),
            new WeaponComponent("Armor Piercing Rounds", 0x51351635,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0x503DEA90,ComponentSlot.Rounds),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Holographic Sight", 0x420FD713, ComponentSlot.Optic),
            new WeaponComponent("Small Scope", 0x49B2945, ComponentSlot.Optic),
            new WeaponComponent("Large Scope", 0xC66B6542, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4,ComponentSlot.Muzzle),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B,ComponentSlot.Muzzle),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF,ComponentSlot.Muzzle),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC,ComponentSlot.Muzzle),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A,ComponentSlot.Muzzle),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC,ComponentSlot.Muzzle),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0x9D65907A, ComponentSlot.ForwardGrip),
            new WeaponComponent("Default Barrel", 0xE73653A9,ComponentSlot.Barrel),
            new WeaponComponent("Heavy Barrel", 0xF97F783B,ComponentSlot.Barrel),
            new WeaponComponent("Digital Camo", 0xD40BB53B,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x431B238B,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x34CF86F4,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0xB4C306DD,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0xEE677A25,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0xDF90DC78,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0xA4C31EE,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0x89CFB0F7,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x7B82145C,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x899CAF75,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0x5218C819,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_specialcarbine_mk2", 120, WeaponCategory.AR, 3, 0x969C3D67, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst | SelectorOptions.ThreeRoundBurst | SelectorOptions.FullAuto) { PossibleComponents = SpecialCarbineMK2Components });
        List<WeaponComponent> BullpupRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xC5A12F80,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xB3688B0F,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Scope", 0xAA2C45B4, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
            new WeaponComponent("Gilded Gun Metal Finish", 0xA857BC78,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpuprifle", 120, WeaponCategory.AR, 3, 2132975508, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = BullpupRifleComponents });
        List<WeaponComponent> BullpulRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x18929DA,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xEFB00628,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0x822060A9,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0xA99CF95A,ComponentSlot.Rounds),
            new WeaponComponent("Armor Piercing Rounds", 0xFAA7F5ED,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0x43621710,ComponentSlot.Rounds),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Holographic Sight", 0x420FD713, ComponentSlot.Optic),
            new WeaponComponent("Small Scope", 0xC7ADD105, ComponentSlot.Optic),
            new WeaponComponent("Medium Scope", 0x3F3C8181, ComponentSlot.Optic),
            new WeaponComponent("Default Barrel", 0x659AC11B,ComponentSlot.Barrel),
            new WeaponComponent("Heavy Barrel", 0x3BF26DC7,ComponentSlot.Barrel),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4,ComponentSlot.Muzzle),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B,ComponentSlot.Muzzle),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF,ComponentSlot.Muzzle),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC,ComponentSlot.Muzzle),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A,ComponentSlot.Muzzle),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC,ComponentSlot.Muzzle),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0x9D65907A, ComponentSlot.ForwardGrip),
            new WeaponComponent("Digital Camo", 0xAE4055B7,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0xB905ED6B,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0xA6C448E8,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0x9486246C,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0x8A390FD2,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0x2337FC5,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0xEFFFDB5E,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0xDDBDB6DA,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0xCB631225,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0xA87D541E,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0xC5E9AE52,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_bullpuprifle_mk2", 120, WeaponCategory.AR, 3, 0x84D6FAFD, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = BullpulRifleMK2Components });
        List<WeaponComponent> CompactRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x513F0A63,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x59FF9BF8,ComponentSlot.Magazine),
            new WeaponComponent("Drum Magazine", 0xC607740E, ComponentSlot.Magazine)
        };
        WeaponsList.Add(new WeaponInformation("weapon_compactrifle", 120, WeaponCategory.AR, 3, 1649403952, false, true, false, 1.5f, 1.9f, 0.7f, 0.9f, 1.0f, 1.5f, 1.0f, 1.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = CompactRifleComponents });
        List<WeaponComponent> MilitaryRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x2D46D83B,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x684ACE42,ComponentSlot.Magazine),
            new WeaponComponent("Iron Sights", 0x6B82F395, ComponentSlot.Optic),
            new WeaponComponent("Scope", 0xAA2C45B4, ComponentSlot.Optic),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
        };
        WeaponsList.Add(new WeaponInformation("weapon_militaryrifle", 120, WeaponCategory.AR, 3, 0x9D1F17E6, false, true, false, 0.55f, 0.65f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = MilitaryRifleComponents });
       
        
        
        
        
        
        
        
        List<WeaponComponent> HeavyRifleCompoinents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x3749B8BB,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x8594554F,ComponentSlot.Magazine),
            new WeaponComponent("Iron Sights", 0x6B82F395, ComponentSlot.Optic),
            new WeaponComponent("Scope", 0xA0D89C42, ComponentSlot.Optic),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavyrifle", 120, WeaponCategory.AR, 3, 0xC78D71B4, false, true, false, 0.55f, 0.65f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.TwoRoundBurst | SelectorOptions.FullAuto) {  PossibleComponents = HeavyRifleCompoinents });
       
        
        
        
        
        
        
        
        
        
        List<WeaponComponent> TacticalRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x3749B8BB,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x8594554F,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x9DB1E023,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
        };
        WeaponsList.Add(new WeaponInformation("weapon_tacticalrifle", 120, WeaponCategory.AR, 3, 0xD1D5F52B, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f,
            SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) 
        { PossibleComponents = TacticalRifleComponents });

        List<WeaponComponent> BattleRifleComponenets = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xEE91D10E,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x1D7E2EEA,ComponentSlot.Magazine),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
        };
        WeaponsList.Add(new WeaponInformation("weapon_battlerifle", 120, WeaponCategory.AR, 3, 0x72B66B11, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.5f, 0.5f, 0.5f, 0.5f,
            SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto)
        { PossibleComponents = BattleRifleComponenets });



        // Vom Feuer Battle Rifle
    }
    private void DefaultConfig_LMG()
    {
        List<WeaponComponent> MGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xF434EF84,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x82158B47,ComponentSlot.Magazine),
            new WeaponComponent("Scope", 0x3C00AFED, ComponentSlot.Optic),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0xD6DABABE,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_mg", 200, WeaponCategory.LMG, 3, 2634544996, false, true, false, 0.4f, 0.5f, 0.15f, 0.25f, 0.75f, 0.75f, 0.75f, 0.75f, SelectorOptions.Safe | SelectorOptions.FullAuto) { PossibleComponents = MGComponents });
        List<WeaponComponent> CombatMGComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xE1FFB34A,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xD6C59CD6,ComponentSlot.Magazine),
            new WeaponComponent("Scope", 0xA0D89C42, ComponentSlot.Optic),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
            new WeaponComponent("Etched Gun Metal Finish", 0x92FECCDD,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatmg", 200, WeaponCategory.LMG, 3, 2144741730, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.75f, 0.75f, 0.75f, 0.75f, SelectorOptions.Safe | SelectorOptions.FullAuto) { PossibleComponents = CombatMGComponents });
        List<WeaponComponent> CombatMGMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x492B257C,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x17DF42E9,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0xF6649745,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0xC326BDBA,ComponentSlot.Rounds),
            new WeaponComponent("Armor Piercing Rounds", 0x29882423,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0x57EF1CC8,ComponentSlot.Rounds),
            new WeaponComponent("Grip", 0x9D65907A, ComponentSlot.ForwardGrip),
            new WeaponComponent("Holographic Sight", 0x420FD713, ComponentSlot.Optic),
            new WeaponComponent("Medium Scope", 0x3F3C8181, ComponentSlot.Optic),
            new WeaponComponent("Large Scope", 0xC66B6542, ComponentSlot.Optic),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4,ComponentSlot.Muzzle),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B,ComponentSlot.Muzzle),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF,ComponentSlot.Muzzle),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC,ComponentSlot.Muzzle),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A,ComponentSlot.Muzzle),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC,ComponentSlot.Muzzle),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE,ComponentSlot.Muzzle),
            new WeaponComponent("Default Barrel", 0xC34EF234,ComponentSlot.Barrel),
            new WeaponComponent("Heavy Barrel", 0xB5E2575B,ComponentSlot.Barrel),
            new WeaponComponent("Digital Camo", 0x4A768CB5,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0xCCE06BBD,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0xBE94CF26,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0x7609BE11,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0x48AF6351,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0x9186750A,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0x84555AA8,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0x1B4C088B,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0xE046DFC,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x28B536E,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0xD703C94D,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_combatmg_mk2", 200, WeaponCategory.LMG, 3, 0xDBBD7280, false, true, false, 0.35f, 0.55f, 0.2f, 0.3f, 0.75f, 0.75f, 0.75f, 0.75f, SelectorOptions.Safe | SelectorOptions.FullAuto) { PossibleComponents = CombatMGMK2Components });
        List<WeaponComponent> GusenbergComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x1CE5A6A5,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xEAC8C270,ComponentSlot.Magazine)
        };
        WeaponsList.Add(new WeaponInformation("weapon_gusenberg", 200, WeaponCategory.LMG, 3, 1627465347, false, true, false, 0.5f, 0.7f, 0.2f, 0.3f, 0.75f, 0.75f, 0.75f, 0.75f, SelectorOptions.Safe | SelectorOptions.SemiAuto | SelectorOptions.FullAuto) { PossibleComponents = GusenbergComponents });
    }
    private void DefaultConfig_Sniper()
    {
        List<WeaponComponent> SniperRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x9BC64089,ComponentSlot.Magazine),
            new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
            new WeaponComponent("Scope", 0xD2443DDC, ComponentSlot.Optic),
            new WeaponComponent("Advanced Scope", 0xBC54DA77, ComponentSlot.Optic),
            new WeaponComponent("Etched Wood Grip Finish", 0x4032B5E7,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_sniperrifle", 40, WeaponCategory.Sniper, 3, 100416529, false, true, true, 0.5f, 0.75f, 0.1f, 0.2f, 0.0f, 0.0f, 0.0f, 0.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = SniperRifleComponents });
        List<WeaponComponent> HeavySniperComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x476F52F4,ComponentSlot.Magazine),
            new WeaponComponent("Scope", 0xD2443DDC, ComponentSlot.Optic),
            new WeaponComponent("Advanced Scope", 0xBC54DA77, ComponentSlot.Optic)
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavysniper", 40, WeaponCategory.Sniper, 3, 205991906, false, true, true, 0.5f, 0.75f, 0.1f, 0.2f, 0.0f, 0.0f, 0.0f, 0.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = HeavySniperComponents });
        List<WeaponComponent> HeavySniperMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xFA1E1A28,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0x2CD8FF9D,ComponentSlot.Magazine),
            new WeaponComponent("Incendiary Rounds", 0xEC0F617,ComponentSlot.Rounds),
            new WeaponComponent("Armor Piercing Rounds", 0xF835D6D4,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0x3BE948F6,ComponentSlot.Rounds),
            new WeaponComponent("Explosive Rounds", 0x89EBDAA7,ComponentSlot.Rounds),
            new WeaponComponent("Zoom Scope", 0x82C10383, ComponentSlot.Optic),
            new WeaponComponent("Advanced Scope", 0xBC54DA77, ComponentSlot.Optic),
            new WeaponComponent("Night Vision Scope", 0xB68010B0, ComponentSlot.Optic),
            new WeaponComponent("Thermal Scope", 0x2E43DA41, ComponentSlot.Optic),
            new WeaponComponent("Suppressor", 0xAC42DF71,ComponentSlot.Muzzle),
            new WeaponComponent("Squared Muzzle Brake", 0x5F7DCE4D,ComponentSlot.Muzzle),
            new WeaponComponent("Bell-End Muzzle Brake", 0x6927E1A1,ComponentSlot.Muzzle),
            new WeaponComponent("Default Barrel", 0x909630B7,ComponentSlot.Barrel),
            new WeaponComponent("Heavy Barrel", 0x108AB09E,ComponentSlot.Barrel),
            new WeaponComponent("Digital Camo", 0xF8337D02,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0xC5BEDD65,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0xE9712475,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0x13AA78E7,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0x26591E50,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0x302731EC,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0xAC722A78,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0xBEA4CEDD,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0xCD776C82,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0xABC5ACC7,ComponentSlot.Coloring),
            new WeaponComponent("Patriotic", 0x6C32D2EB,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_heavysniper_mk2", 40, WeaponCategory.Sniper, 3, 0xA914799, false, true, true, 0.5f, 0.75f, 0.1f, 0.2f, 0.0f, 0.0f, 0.0f, 0.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = HeavySniperMK2Components });
        List<WeaponComponent> MarksmanRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0xD83B4141,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xCCFD2AC5,ComponentSlot.Magazine),
            new WeaponComponent("Scope", 0x1C221B1A, ComponentSlot.Optic),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
            new WeaponComponent("Yusuf Amir Luxury Finish", 0x161E9241,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_marksmanrifle", 40, WeaponCategory.Sniper, 3, 3342088282, false, true, true, 0.5f, 0.75f, 0.1f, 0.2f, 0.0001f, 0.0001f, 0.0001f, 0.0001f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = MarksmanRifleComponents });
        //List<WeaponComponent> RussianSniperComponents = new List<WeaponComponent>
        //{
        //    new WeaponComponent("Default Clip", 0xD83B4141,ComponentSlot.Magazine),
        //    new WeaponComponent("Extended Clip", 0xCCFD2AC5,ComponentSlot.Magazine),
        //    new WeaponComponent("Scope", 0x1C221B1A, ComponentSlot.Optic),
        //    new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
        //    new WeaponComponent("Suppressor", 0xA73D4664,ComponentSlot.Muzzle),
        //};
        //WeaponsList.Add(new WeaponInformation("weapon_russiansniper", 40, WeaponCategory.Sniper, 4, 0xBE64A6AB, false, true, true, 0.5f, 0.75f, 0.1f, 0.2f, 0.0001f, 0.0001f, 0.0001f, 0.0001f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = RussianSniperComponents });
        List<WeaponComponent> MarksmanRifleMK2Components = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x94E12DCE,ComponentSlot.Magazine),
            new WeaponComponent("Extended Clip", 0xE6CFD1AA,ComponentSlot.Magazine),
            new WeaponComponent("Tracer Rounds", 0xD77A22D2,ComponentSlot.Rounds),
            new WeaponComponent("Incendiary Rounds", 0x6DD7A86E,ComponentSlot.Rounds),
            new WeaponComponent("Armor Piercing Rounds", 0xF46FD079,ComponentSlot.Rounds),
            new WeaponComponent("Full Metal Jacket Rounds", 0xE14A9ED3,ComponentSlot.Rounds),
            new WeaponComponent("Holographic Sight", 0x420FD713, ComponentSlot.Optic),
            new WeaponComponent("Large Scope", 0xC66B6542, ComponentSlot.Optic),
            new WeaponComponent("Zoom Scope", 0x5B1C713C, ComponentSlot.Optic),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Suppressor", 0x837445AA,ComponentSlot.Muzzle),
            new WeaponComponent("Flat Muzzle Brake", 0xB99402D4,ComponentSlot.Muzzle),
            new WeaponComponent("Tactical Muzzle Brake", 0xC867A07B,ComponentSlot.Muzzle),
            new WeaponComponent("Fat-End Muzzle Brake", 0xDE11CBCF,ComponentSlot.Muzzle),
            new WeaponComponent("Precision Muzzle Brake", 0xEC9068CC,ComponentSlot.Muzzle),
            new WeaponComponent("Heavy Duty Muzzle Brake", 0x2E7957A,ComponentSlot.Muzzle),
            new WeaponComponent("Slanted Muzzle Brake", 0x347EF8AC,ComponentSlot.Muzzle),
            new WeaponComponent("Split-End Muzzle Brake", 0x4DB62ABE,ComponentSlot.Muzzle),
            new WeaponComponent("Default Barrel", 0x381B5D89,ComponentSlot.Barrel),
            new WeaponComponent("Heavy Barrel", 0x68373DDC,ComponentSlot.Barrel),
            new WeaponComponent("Grip", 0x9D65907A, ComponentSlot.ForwardGrip),
            new WeaponComponent("Digital Camo", 0x9094FBA0,ComponentSlot.Coloring),
            new WeaponComponent("Brushstroke Camo", 0x7320F4B2,ComponentSlot.Coloring),
            new WeaponComponent("Woodland Camo", 0x60CF500F,ComponentSlot.Coloring),
            new WeaponComponent("Skull", 0xFE668B3F,ComponentSlot.Coloring),
            new WeaponComponent("Sessanta Nove", 0xF3757559,ComponentSlot.Coloring),
            new WeaponComponent("Perseus", 0x193B40E8,ComponentSlot.Coloring),
            new WeaponComponent("Leopard", 0x107D2F6C,ComponentSlot.Coloring),
            new WeaponComponent("Zebra", 0xC4E91841,ComponentSlot.Coloring),
            new WeaponComponent("Geometric", 0x9BB1C5D3,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0x3B61040B,ComponentSlot.Coloring),
            new WeaponComponent("Boom!", 0xB7A316DA,ComponentSlot.Coloring)
        };
        WeaponsList.Add(new WeaponInformation("weapon_marksmanrifle_mk2", 40, WeaponCategory.Sniper, 3, 0x6A6C02E0, false, true, true, 0.5f, 0.75f, 0.1f, 0.2f, 0.0001f, 0.0001f, 0.0001f, 0.0001f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = MarksmanRifleMK2Components });
        List<WeaponComponent> PrecisionRifleComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x9BC64089,ComponentSlot.Magazine),
        };
        WeaponsList.Add(new WeaponInformation("weapon_precisionrifle", 40, WeaponCategory.Sniper, 3, 1853742572, false, true, true, 0.5f, 0.75f, 0.1f, 0.2f, 0.0f, 0.0f, 0.0f, 0.0f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = PrecisionRifleComponents });

    }
    private void DefaultConfig_Heavy()
    {
        WeaponsList.Add(new WeaponInformation("weapon_rpg", 3, WeaponCategory.Heavy, 4, 2982836145, false, true, false, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto));
        List<WeaponComponent> GrenadeLauncherComponents = new List<WeaponComponent>
        {
            new WeaponComponent("Default Clip", 0x11AE5C97,ComponentSlot.Magazine),
            new WeaponComponent("Flashlight", 0x7BC4CDDC,ComponentSlot.Light),
            new WeaponComponent("Grip", 0xC164F53, ComponentSlot.ForwardGrip),
            new WeaponComponent("Scope", 0xAA2C45B4, ComponentSlot.Optic)
        };
        WeaponsList.Add(new WeaponInformation("weapon_grenadelauncher", 32, WeaponCategory.Heavy, 4, 2726580491, false, true, false, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { PossibleComponents = GrenadeLauncherComponents });
        WeaponsList.Add(new WeaponInformation("weapon_grenadelauncher_smoke", 32, WeaponCategory.Heavy, 4, 1305664598, false, true, false, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_minigun", 500, WeaponCategory.Heavy, 4, 1119849093, false, true, false, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.FullAuto));
        WeaponsList.Add(new WeaponInformation("weapon_firework", 20, WeaponCategory.Heavy, 3, 0x7F7497E5, false, true, false, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_railgun", 50, WeaponCategory.Heavy, 4, 0x6D544C99, false, true, false, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_hominglauncher", 3, WeaponCategory.Heavy, 4, 0x63AB0442, false, true, false, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_compactlauncher", 10, WeaponCategory.Heavy, 4, 125959754, false, true, false, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto));
        WeaponsList.Add(new WeaponInformation("weapon_rayminigun", 50, WeaponCategory.Heavy, 4, 0xB62D1F67, false, true, false, 0.4f, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_railgunxm3", 50, WeaponCategory.Heavy, 4, 0xFEA23564, false, true, false, 0.4f, 0.1f, 0.1f, 0.1f, 0.1f, 0.5f, 0.5f, 0.5f, SelectorOptions.Safe | SelectorOptions.SemiAuto) { IsRegular = false });

    }
    private void DefaultConfig_Throwable()
    {
        WeaponsList.Add(new WeaponInformation("weapon_grenade", 1, WeaponCategory.Throwable, 2, 0x93E220BD, false, false, false) {  });
        WeaponsList.Add(new WeaponInformation("weapon_bzgas", 1, WeaponCategory.Throwable, 2, 0xA0973D5E, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_molotov", 1, WeaponCategory.Throwable, 2, 0x24B17070, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_stickybomb", 1, WeaponCategory.Throwable, 2, 0x2C3731D9, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_proxmine", 1, WeaponCategory.Throwable, 2, 0xAB564B93, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_snowball", 1, WeaponCategory.Throwable, 2, 0x787F0BB, false, false, false) { IsLegal = true, DoesNotTriggerBrandishing = true, IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_pipebomb", 1, WeaponCategory.Throwable, 2, 0xBA45E8B8, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_ball", 1, WeaponCategory.Throwable, 2, 0x23C9F95C, false, false, false) { IsLegal = true, DoesNotTriggerBrandishing = true, IsRegular = false });
        WeaponsList.Add(new WeaponInformation("weapon_smokegrenade", 1, WeaponCategory.Throwable, 2, 0xFDBC8A50, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_flare", 1, WeaponCategory.Throwable, 2, 0x497FACC3, false, false, false));
        WeaponsList.Add(new WeaponInformation("weapon_petrolcan", 1, WeaponCategory.Misc, 0, 0x34A67B97, false, false, false));
        WeaponsList.Add(new WeaponInformation("gadget_parachute", 1, WeaponCategory.Misc, 0, 0xFBAB5776, false, false, false) { IsLegal = true, DoesNotTriggerBrandishing = true,IsRegular = false, });
        WeaponsList.Add(new WeaponInformation("weapon_fireextinguisher", 1, WeaponCategory.Misc, 0, 0x060EC506, false, false, false) { IsLegal = true, DoesNotTriggerBrandishing = true,IsRegular= false  });
        WeaponsList.Add(new WeaponInformation("weapon_hazardcan", 1, WeaponCategory.Misc, 0, 0xBA536372, false, false, false));
    }
}
