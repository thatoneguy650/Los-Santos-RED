using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IssueableWeapons : IIssuableWeapons
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\IssuableWeapons.xml";
    private List<IssuableWeaponsGroup> IssuableWeaponsGroupLookup = new List<IssuableWeaponsGroup>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("IssuableWeapons*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Issuable Weapons config: {ConfigFile.FullName}", 0);
            IssuableWeaponsGroupLookup = Serialization.DeserializeParams<IssuableWeaponsGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Issuable Weapons config  {ConfigFileName}", 0);
            IssuableWeaponsGroupLookup = Serialization.DeserializeParams<IssuableWeaponsGroup>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Issuable Weapons config found, creating default", 0);
            DefaultConfig();
        }
    }
    private void DefaultConfig()
    {
        //Weapon COPS
        List<IssuableWeapon> AllSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation(), 15),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 15),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 5),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(), 15),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 15),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 10),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 5),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(), 5),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 5),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 5),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 5),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(), 5),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Etched Wood Grip Finish" )}), 5),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 5),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 5),
            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation(), 5),
            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 5),
        };
        List<IssuableWeapon> AllLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(),5),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),5),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(),2),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),5),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight" )}),1),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Holographic Sight" )}),1),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip" )}),5),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),
        };
        List<IssuableWeapon> BestSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 20),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 20),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 20),

            new IssuableWeapon("weapon_pistol50", new WeaponVariation(), 5),
            new IssuableWeapon("weapon_appistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 15),
        };
        List<IssuableWeapon> BestLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight")}),50),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip") }),25),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),


            new IssuableWeapon("weapon_combatmg_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip")}), 5),
            new IssuableWeapon("weapon_smg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Suppressor"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 10),
            new IssuableWeapon("weapon_assaultshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight")}), 5),
        };
        List<IssuableWeapon> MilitarySidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation(),70),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }),10),
        };
        List<IssuableWeapon> MilitaryLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),20),

            new IssuableWeapon("weapon_combatmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip")}),15),
        };
        List<IssuableWeapon> HeliSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip")})),
        };
        List<IssuableWeapon> HeliLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight")})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            //new IssuableWeapon("weapon_marksmanrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Suppressor"), new WeaponComponent("Tracer Rounds" )})),
            //new IssuableWeapon("weapon_marksmanrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Tracer Rounds") })),
        };
        List<IssuableWeapon> LimitedSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(),20),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight")}),20),

            new IssuableWeapon("weapon_vintagepistol", new WeaponVariation(),10),

            new IssuableWeapon("weapon_pistol", new WeaponVariation(),5),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),2),

            new IssuableWeapon("weapon_revolver", new WeaponVariation(),5),

            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation(),5),
            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),5),

        };
        List<IssuableWeapon> LimitedLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),

            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
        };
        List<IssuableWeapon> Tasers = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_stungun", new WeaponVariation(), 100),
        };
        List<IssuableWeapon> Nightsticks = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_nightstick", new WeaponVariation(), 100),
        };

        IssuableWeaponsGroupLookup = new List<IssuableWeaponsGroup>();
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("Tasers", Tasers));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("Nightsticks", Nightsticks));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("AllSidearms", AllSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("AllLongGuns", AllLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("BestSidearms", BestSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("BestLongGuns", BestLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("MilitarySidearms", MilitarySidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("MilitaryLongGuns", MilitaryLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("HeliSidearms", HeliSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("HeliLongGuns", HeliLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("LimitedSidearms", LimitedSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("LimitedLongGuns", LimitedLongGuns));

        //Gangs
        List<IssuableWeapon> MeleeWeapons = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_hatchet", new WeaponVariation()),
            new IssuableWeapon("weapon_knuckle", new WeaponVariation()),
            new IssuableWeapon("weapon_knife", new WeaponVariation()),
            new IssuableWeapon("weapon_machete", new WeaponVariation()),
            new IssuableWeapon("weapon_switchblade", new WeaponVariation()),
            new IssuableWeapon("weapon_nightstick", new WeaponVariation()),
            new IssuableWeapon("weapon_bat", new WeaponVariation()),
            new IssuableWeapon("weapon_crowbar", new WeaponVariation()),
            new IssuableWeapon("weapon_hammer", new WeaponVariation()),
        };
        List<IssuableWeapon> AllGangSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Etched Wood Grip Finish" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol50", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> AllGangLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Holographic Sight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_minismg", new WeaponVariation()),
            new IssuableWeapon("weapon_minismg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_autoshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_assaultrifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_mg", new WeaponVariation()),
            new IssuableWeapon("weapon_mg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_combatmg", new WeaponVariation()),
            new IssuableWeapon("weapon_combatmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_combatmg_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_combatmg_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
        };
        List<IssuableWeapon> FamiliesSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol50", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> FamiliesLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_minismg", new WeaponVariation()),
            new IssuableWeapon("weapon_minismg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_autoshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        List<IssuableWeapon> LostSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Etched Wood Grip Finish" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_revolver_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol50", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> LostLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Holographic Sight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_minismg", new WeaponVariation()),
            new IssuableWeapon("weapon_minismg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_autoshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_assaultrifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_mg", new WeaponVariation()),
            new IssuableWeapon("weapon_mg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_combatmg", new WeaponVariation()),
            new IssuableWeapon("weapon_combatmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_combatmg_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_combatmg_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
        };
        List<IssuableWeapon> VagosSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> VagosLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_autoshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        List<IssuableWeapon> BallasSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> BallasLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        List<IssuableWeapon> MarabuntaSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> MarabuntaLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_assaultrifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        List<IssuableWeapon> VarriosSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol50", new WeaponVariation()),
        };
        List<IssuableWeapon> VarriosLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        List<IssuableWeapon> TriadsSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
        };
        List<IssuableWeapon> TriadsLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_bullpuprifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpuprifle", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpdw", new WeaponVariation()),
        };
        List<IssuableWeapon> KkangpaeSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
        };
        List<IssuableWeapon> KkangpaeLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_bullpuprifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpuprifle", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpdw", new WeaponVariation()),
        };
        List<IssuableWeapon> MafiaSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
        };
        List<IssuableWeapon> MafiaLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation()),
        };

        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("MeleeWeapons", MeleeWeapons));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("AllGangSidearms", AllGangSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("AllGangLongGuns", AllGangLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("FamiliesSidearms", FamiliesSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("FamiliesLongGuns", FamiliesLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("LostSidearms", LostSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("LostLongGuns", LostLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("VagosSidearms", VagosSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("VagosLongGuns", VagosLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("BallasSidearms", BallasSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("BallasLongGuns", BallasLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("MarabuntaSidearms", MarabuntaSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("MarabuntaLongGuns", MarabuntaLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("VarriosSidearms", VarriosSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("VarriosLongGuns", VarriosLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("TriadsSidearms", TriadsSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("TriadsLongGuns", TriadsLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("KkangpaeSidearms", KkangpaeSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("KkangpaeLongGuns", KkangpaeLongGuns));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("MafiaSidearms", MafiaSidearms));
        IssuableWeaponsGroupLookup.Add(new IssuableWeaponsGroup("MafiaLongGuns", MafiaLongGuns));

        Serialization.SerializeParams(IssuableWeaponsGroupLookup, ConfigFileName);
    }
    public List<IssuableWeapon> GetWeaponData(string issuableWeaponsID)
    {
        IssuableWeaponsGroup weaponsGroup = IssuableWeaponsGroupLookup.FirstOrDefault(x => x.IssuableWeaponsID == issuableWeaponsID);
        if(weaponsGroup == null)
        {
            EntryPoint.WriteToConsole($"GetWeaponData NULL {issuableWeaponsID}");
            return null;
        }
        else
        {
            return weaponsGroup.IssuableWeapons;
        }

    }
}

