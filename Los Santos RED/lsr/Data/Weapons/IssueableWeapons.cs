using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
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
    private List<IssuableWeapon> AllSidearms;
    private List<IssuableWeapon> AllLongGuns;
    private List<IssuableWeapon> BestSidearms;
    private List<IssuableWeapon> BestLongGuns;
    private List<IssuableWeapon> MilitarySidearms;
    private List<IssuableWeapon> MilitaryLongGuns;
    private List<IssuableWeapon> HeliSidearms;
    private List<IssuableWeapon> HeliLongGuns;
    private List<IssuableWeapon> LimitedSidearms;
    private List<IssuableWeapon> LimitedLongGuns;
    private List<IssuableWeapon> Tasers;
    private List<IssuableWeapon> Nightsticks;
    private List<IssuableWeapon> GoodSniperLongGuns;
    private List<IssuableWeapon> ConcealableSidearms;
    private List<IssuableWeapon> GangMeleeWeapons;
    private List<IssuableWeapon> AllGangSidearms;
    private List<IssuableWeapon> AllGangLongGuns;
    private List<IssuableWeapon> FamiliesSidearms;
    private List<IssuableWeapon> FamiliesLongGuns;
    private List<IssuableWeapon> LostSidearms;
    private List<IssuableWeapon> LostLongGuns;
    private List<IssuableWeapon> VagosSidearms;
    private List<IssuableWeapon> VagosLongGuns;
    private List<IssuableWeapon> BallasSidearms;
    private List<IssuableWeapon> BallasLongGuns;
    private List<IssuableWeapon> MarabuntaSidearms;
    private List<IssuableWeapon> MarabuntaLongGuns;
    private List<IssuableWeapon> VarriosSidearms;
    private List<IssuableWeapon> VarriosLongGuns;
    private List<IssuableWeapon> TriadsSidearms;
    private List<IssuableWeapon> TriadsLongGuns;
    private List<IssuableWeapon> KkangpaeSidearms;
    private List<IssuableWeapon> KkangpaeLongGuns;
    private List<IssuableWeapon> MafiaSidearms;
    private List<IssuableWeapon> MafiaLongGuns;
    private List<IssuableWeapon> Minigun;
    private List<IssuableWeapon> FireExtinguisher;
    private List<IssuableWeapon> TaxiSidearms;
    private List<IssuableWeapon> TaxiLongGuns;
    private List<IssuableWeapon> VendorMeleeWeapons;
    private List<IssuableWeapon> VendorSidearms;
    private List<IssuableWeapon> VendorLongGuns;

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
            DefaultConfig_FullModernJurisdiction();
            DefaultConfig_LosSantos2008();
        }
    }



    private void DefaultConfig()
    {
        //COPS
        AllSidearms = new List<IssuableWeapon>()
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
        AllLongGuns = new List<IssuableWeapon>()
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
        BestSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 20),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 20),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 20),

            new IssuableWeapon("weapon_pistol50", new WeaponVariation(), 5),
            new IssuableWeapon("weapon_appistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 15),
        };
        BestLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight")}),50),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip") }),25),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),


            new IssuableWeapon("weapon_combatmg_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip")}), 5),
            new IssuableWeapon("weapon_smg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Suppressor"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 10),
            new IssuableWeapon("weapon_assaultshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight")}), 5),
        };
        MilitarySidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation(),70),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }),10),
        };
        MilitaryLongGuns = new List<IssuableWeapon>()
        {

            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),20),
            new IssuableWeapon("weapon_combatmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip")}),15),
        };
        HeliSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip")})),
        };
        HeliLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight")})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            //new IssuableWeapon("weapon_marksmanrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Suppressor"), new WeaponComponent("Tracer Rounds" )})),
            //new IssuableWeapon("weapon_marksmanrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Tracer Rounds") })),
        };
        LimitedSidearms = new List<IssuableWeapon>()
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
        LimitedLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),

            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
        };
        Tasers = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_stungun", new WeaponVariation(), 100),
        };
        Nightsticks = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_nightstick", new WeaponVariation(), 100),
        };
        GoodSniperLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_sniperrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope")})),
            new IssuableWeapon("weapon_marksmanrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Suppressor"), new WeaponComponent("Tracer Rounds" )})),
            new IssuableWeapon("weapon_marksmanrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Tracer Rounds") })),
        };
        ConcealableSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_snspistol", new WeaponVariation(),25),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation(),25),
            new IssuableWeapon("weapon_pistolxm3", new WeaponVariation(),25),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation(),25),
        };

        //Gangs
        GangMeleeWeapons = new List<IssuableWeapon>()
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
        AllGangSidearms = new List<IssuableWeapon>()
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
        AllGangLongGuns = new List<IssuableWeapon>()
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
        FamiliesSidearms = new List<IssuableWeapon>()
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
        FamiliesLongGuns = new List<IssuableWeapon>()
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
        LostSidearms = new List<IssuableWeapon>()
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
        LostLongGuns = new List<IssuableWeapon>()
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
        VagosSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        VagosLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_autoshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        BallasSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        BallasLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        MarabuntaSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        MarabuntaLongGuns = new List<IssuableWeapon>()
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
        VarriosSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol50", new WeaponVariation()),
        };
        VarriosLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        TriadsSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
        };
        TriadsLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_bullpuprifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpuprifle", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpdw", new WeaponVariation()),
        };
        KkangpaeSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
        };
        KkangpaeLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_bullpuprifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpuprifle", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpdw", new WeaponVariation()),
        };
        MafiaSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
        };
        MafiaLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation()),
        };

        //Other
        Minigun = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_minigun", new WeaponVariation()),
        };
        FireExtinguisher = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_fireextinguisher", new WeaponVariation()),
        };

        //TAXI
        TaxiSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(),20),
            new IssuableWeapon("weapon_vintagepistol", new WeaponVariation(),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(),5),
            new IssuableWeapon("weapon_revolver", new WeaponVariation(),5),
        };
        TaxiLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
        };

        //Vendor
        VendorMeleeWeapons = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_hatchet", new WeaponVariation()),
            new IssuableWeapon("weapon_knife", new WeaponVariation()),
            new IssuableWeapon("weapon_machete", new WeaponVariation()),
            new IssuableWeapon("weapon_bat", new WeaponVariation()),
            new IssuableWeapon("weapon_crowbar", new WeaponVariation()),
            new IssuableWeapon("weapon_hammer", new WeaponVariation()),
        };
        VendorSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(),20),
            new IssuableWeapon("weapon_vintagepistol", new WeaponVariation(),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(),5),
            new IssuableWeapon("weapon_revolver", new WeaponVariation(),5),
        };
        VendorLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
        };

        IssuableWeaponsGroupLookup = new List<IssuableWeaponsGroup>
        {
            new IssuableWeaponsGroup("Tasers", Tasers),
            new IssuableWeaponsGroup("Nightsticks", Nightsticks),
            new IssuableWeaponsGroup("AllSidearms", AllSidearms),
            new IssuableWeaponsGroup("AllLongGuns", AllLongGuns),
            new IssuableWeaponsGroup("BestSidearms", BestSidearms),
            new IssuableWeaponsGroup("BestLongGuns", BestLongGuns),
            new IssuableWeaponsGroup("MilitarySidearms", MilitarySidearms),
            new IssuableWeaponsGroup("MilitaryLongGuns", MilitaryLongGuns),
            new IssuableWeaponsGroup("HeliSidearms", HeliSidearms),
            new IssuableWeaponsGroup("HeliLongGuns", HeliLongGuns),
            new IssuableWeaponsGroup("LimitedSidearms", LimitedSidearms),
            new IssuableWeaponsGroup("LimitedLongGuns", LimitedLongGuns),
            new IssuableWeaponsGroup("GoodSniperLongGuns", GoodSniperLongGuns),
            new IssuableWeaponsGroup("TaxiSidearms", TaxiSidearms),
            new IssuableWeaponsGroup("TaxiLongGuns", TaxiLongGuns),
            new IssuableWeaponsGroup("VendorMeleeWeapons", VendorMeleeWeapons),
            new IssuableWeaponsGroup("VendorSidearms", VendorSidearms),
            new IssuableWeaponsGroup("VendorLongGuns", VendorLongGuns),
            new IssuableWeaponsGroup("MeleeWeapons", GangMeleeWeapons),
            new IssuableWeaponsGroup("AllGangSidearms", AllGangSidearms),
            new IssuableWeaponsGroup("AllGangLongGuns", AllGangLongGuns),
            new IssuableWeaponsGroup("FamiliesSidearms", FamiliesSidearms),
            new IssuableWeaponsGroup("FamiliesLongGuns", FamiliesLongGuns),
            new IssuableWeaponsGroup("LostSidearms", LostSidearms),
            new IssuableWeaponsGroup("LostLongGuns", LostLongGuns),
            new IssuableWeaponsGroup("VagosSidearms", VagosSidearms),
            new IssuableWeaponsGroup("VagosLongGuns", VagosLongGuns),
            new IssuableWeaponsGroup("BallasSidearms", BallasSidearms),
            new IssuableWeaponsGroup("BallasLongGuns", BallasLongGuns),
            new IssuableWeaponsGroup("MarabuntaSidearms", MarabuntaSidearms),
            new IssuableWeaponsGroup("MarabuntaLongGuns", MarabuntaLongGuns),
            new IssuableWeaponsGroup("VarriosSidearms", VarriosSidearms),
            new IssuableWeaponsGroup("VarriosLongGuns", VarriosLongGuns),
            new IssuableWeaponsGroup("TriadsSidearms", TriadsSidearms),
            new IssuableWeaponsGroup("TriadsLongGuns", TriadsLongGuns),
            new IssuableWeaponsGroup("KkangpaeSidearms", KkangpaeSidearms),
            new IssuableWeaponsGroup("KkangpaeLongGuns", KkangpaeLongGuns),
            new IssuableWeaponsGroup("MafiaSidearms", MafiaSidearms),
            new IssuableWeaponsGroup("MafiaLongGuns", MafiaLongGuns),
            new IssuableWeaponsGroup("Minigun", Minigun),
            new IssuableWeaponsGroup("FireExtinguisher", FireExtinguisher),
            new IssuableWeaponsGroup("ConcealableSidearms", ConcealableSidearms)
        };
        Serialization.SerializeParams(IssuableWeaponsGroupLookup, ConfigFileName);
    }
    private void DefaultConfig_LosSantos2008()
    {
        //COPS
        List<IssuableWeapon> AllSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation(), 15),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 15),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 5),
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
        List<IssuableWeapon> AllLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(),5),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),
            new IssuableWeapon("weapon_tacticalrifle", new WeaponVariation(),5),
            new IssuableWeapon("weapon_tacticalrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_tacticalrifle", new WeaponVariation(),5),
        };
        List<IssuableWeapon> BestSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 20),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 20),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 20),

            new IssuableWeapon("weapon_pistol50", new WeaponVariation(), 5),
            new IssuableWeapon("weapon_appistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 15),
        };
        List<IssuableWeapon> BestLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight")}),50),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") }),25),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {   new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),
            new IssuableWeapon("weapon_combatmg_mk2", new WeaponVariation(new List<WeaponComponent> {   new WeaponComponent("Grip")}), 5),
        };
        List<IssuableWeapon> MilitarySidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation(),70),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }),10),
        };
        List<IssuableWeapon> MilitaryLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation()  ,20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {   new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {   new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),20),
            new IssuableWeapon("weapon_combatmg", new WeaponVariation(),15),
        };
        List<IssuableWeapon> HeliSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip")})),
        };
        List<IssuableWeapon> HeliLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight")})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {   new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
        };
        List<IssuableWeapon> LimitedSidearms_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> LimitedLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),

        };
        List<IssuableWeapon> Tasers_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_stungun", new WeaponVariation(), 100),
        };
        List<IssuableWeapon> Nightsticks_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_nightstick", new WeaponVariation(), 100),
        };
        List<IssuableWeapon> ConcealableSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_snspistol", new WeaponVariation(),25),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation(),25),
            new IssuableWeapon("weapon_pistolxm3", new WeaponVariation(),25),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation(),25),
        };

        //TAXI
        List<IssuableWeapon> TaxiSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(),20),
            new IssuableWeapon("weapon_vintagepistol", new WeaponVariation(),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(),5),
            new IssuableWeapon("weapon_revolver", new WeaponVariation(),5),
        };
        List<IssuableWeapon> TaxiLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
        };

        //Vendor
        List<IssuableWeapon> VendorMeleeWeapons_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_hatchet", new WeaponVariation()),
            new IssuableWeapon("weapon_knife", new WeaponVariation()),
            new IssuableWeapon("weapon_machete", new WeaponVariation()),
            new IssuableWeapon("weapon_bat", new WeaponVariation()),
            new IssuableWeapon("weapon_crowbar", new WeaponVariation()),
            new IssuableWeapon("weapon_hammer", new WeaponVariation()),
        };
        List<IssuableWeapon> VendorSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(),20),
            new IssuableWeapon("weapon_vintagepistol", new WeaponVariation(),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(),5),
            new IssuableWeapon("weapon_revolver", new WeaponVariation(),5),
        };
        List<IssuableWeapon> VendorLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
        };

        //Other
        List<IssuableWeapon> Minigun_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_minigun", new WeaponVariation()),
        };
        List<IssuableWeapon> FireExtinguisher_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_fireextinguisher", new WeaponVariation()),
        };

        //Gangs
        List<IssuableWeapon> MeleeWeapons_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> AllGangSidearms_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> AllGangLongGuns_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> FamiliesSidearms_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> FamiliesLongGuns_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> LostSidearms_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> LostLongGuns_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> VagosSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> VagosLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_autoshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_assaultrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation()),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_compactrifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        List<IssuableWeapon> BallasSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> BallasLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        List<IssuableWeapon> MarabuntaSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol_mk2", new WeaponVariation()),
        };
        List<IssuableWeapon> MarabuntaLongGuns_2008 = new List<IssuableWeapon>()
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
        List<IssuableWeapon> VarriosSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_snspistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol50", new WeaponVariation()),
        };
        List<IssuableWeapon> VarriosLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_dbshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation()),
            new IssuableWeapon("weapon_microsmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation()),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_machinepistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Drum Magazine") })),
        };
        List<IssuableWeapon> TriadsSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
        };
        List<IssuableWeapon> TriadsLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_bullpuprifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpuprifle", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpdw", new WeaponVariation()),
        };
        List<IssuableWeapon> KkangpaeSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_appistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
        };
        List<IssuableWeapon> KkangpaeLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_bullpuprifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpuprifle", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpdw", new WeaponVariation()),
        };
        List<IssuableWeapon> MafiaSidearms_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_ceramicpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
        };
        List<IssuableWeapon> MafiaLongGuns_2008 = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_sawnoffshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_bullpupshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation()),
        };

        List<IssuableWeaponsGroup> IssuableWeaponsGroupLookup_Old = new List<IssuableWeaponsGroup>
        {
            new IssuableWeaponsGroup("Tasers", Tasers_2008),
            new IssuableWeaponsGroup("Nightsticks", Nightsticks_2008),
            new IssuableWeaponsGroup("AllSidearms", AllSidearms_2008),
            new IssuableWeaponsGroup("AllLongGuns", AllLongGuns_2008),
            new IssuableWeaponsGroup("BestSidearms", BestSidearms_2008),
            new IssuableWeaponsGroup("BestLongGuns", BestLongGuns_2008),
            new IssuableWeaponsGroup("MilitarySidearms", MilitarySidearms_2008),
            new IssuableWeaponsGroup("MilitaryLongGuns", MilitaryLongGuns_2008),
            new IssuableWeaponsGroup("HeliSidearms", HeliSidearms_2008),
            new IssuableWeaponsGroup("HeliLongGuns", HeliLongGuns_2008),
            new IssuableWeaponsGroup("LimitedSidearms", LimitedSidearms_2008),
            new IssuableWeaponsGroup("LimitedLongGuns", LimitedLongGuns_2008),
            new IssuableWeaponsGroup("TaxiSidearms", TaxiSidearms_2008),
            new IssuableWeaponsGroup("TaxiLongGuns", TaxiLongGuns_2008),
            new IssuableWeaponsGroup("VendorMeleeWeapons", VendorMeleeWeapons_2008),
            new IssuableWeaponsGroup("VendorSidearms", VendorSidearms_2008),
            new IssuableWeaponsGroup("VendorLongGuns", VendorLongGuns_2008),
            new IssuableWeaponsGroup("MeleeWeapons", MeleeWeapons_2008),
            new IssuableWeaponsGroup("AllGangSidearms", AllGangSidearms_2008),
            new IssuableWeaponsGroup("AllGangLongGuns", AllGangLongGuns_2008),
            new IssuableWeaponsGroup("FamiliesSidearms", FamiliesSidearms_2008),
            new IssuableWeaponsGroup("FamiliesLongGuns", FamiliesLongGuns_2008),
            new IssuableWeaponsGroup("LostSidearms", LostSidearms_2008),
            new IssuableWeaponsGroup("LostLongGuns", LostLongGuns_2008),
            new IssuableWeaponsGroup("VagosSidearms", VagosSidearms_2008),
            new IssuableWeaponsGroup("VagosLongGuns", VagosLongGuns_2008),
            new IssuableWeaponsGroup("BallasSidearms", BallasSidearms_2008),
            new IssuableWeaponsGroup("BallasLongGuns", BallasLongGuns_2008),
            new IssuableWeaponsGroup("MarabuntaSidearms", MarabuntaSidearms_2008),
            new IssuableWeaponsGroup("MarabuntaLongGuns", MarabuntaLongGuns_2008),
            new IssuableWeaponsGroup("VarriosSidearms", VarriosSidearms_2008),
            new IssuableWeaponsGroup("VarriosLongGuns", VarriosLongGuns_2008),
            new IssuableWeaponsGroup("TriadsSidearms", TriadsSidearms_2008),
            new IssuableWeaponsGroup("TriadsLongGuns", TriadsLongGuns_2008),
            new IssuableWeaponsGroup("KkangpaeSidearms", KkangpaeSidearms_2008),
            new IssuableWeaponsGroup("KkangpaeLongGuns", KkangpaeLongGuns_2008),
            new IssuableWeaponsGroup("MafiaSidearms", MafiaSidearms_2008),
            new IssuableWeaponsGroup("MafiaLongGuns", MafiaLongGuns_2008),
            new IssuableWeaponsGroup("Minigun", Minigun_2008),
            new IssuableWeaponsGroup("FireExtinguisher", FireExtinguisher_2008),
            new IssuableWeaponsGroup("ConcealableSidearms", ConcealableSidearms_2008)
        };
        Serialization.SerializeParams(IssuableWeaponsGroupLookup_Old, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\IssuableWeapons_LosSantos2008.xml");
    }
    private void DefaultConfig_FullModernJurisdiction()
    {
        //COPS
        List<IssuableWeapon> AllSidearms_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation(), 15),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 15),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 25),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(), 15),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 15),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 10),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 25),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(), 5),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 5),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 5),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 25),

            new IssuableWeapon("weapon_ceramicpistol",new WeaponVariation(),5),
            new IssuableWeapon("weapon_ceramicpistol",new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 20),
        };
        List<IssuableWeapon> AllLongGuns_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip" )}),5),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),
        };
        List<IssuableWeapon> BestSidearms_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 20),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 20),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 20),

            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}), 20),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 20),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 20),

            new IssuableWeapon("weapon_ceramicpistol",new WeaponVariation(),20),
            new IssuableWeapon("weapon_ceramicpistol",new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}), 20),

            new IssuableWeapon("weapon_pistol50", new WeaponVariation(), 5),
            new IssuableWeapon("weapon_appistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }), 15),
        };
        List<IssuableWeapon> BestLongGuns_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight")}),50),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip") }),25),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),5),


            new IssuableWeapon("weapon_combatmg_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip")}), 5),
            new IssuableWeapon("weapon_smg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Suppressor"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}), 10),
            new IssuableWeapon("weapon_assaultshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight")}), 5),
        };
        List<IssuableWeapon> MilitarySidearms_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation(),70),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )}),10),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") }),10),
        };
        List<IssuableWeapon> MilitaryLongGuns_Modern = new List<IssuableWeapon>()
        {

            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(),5),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),20),
            new IssuableWeapon("weapon_combatmg", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip")}),15),
        };
        List<IssuableWeapon> HeliSidearms_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip")})),
        };
        List<IssuableWeapon> HeliLongGuns_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight")})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
        };
        List<IssuableWeapon> LimitedSidearms_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(),10),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight")}),10),

            new IssuableWeapon("weapon_pistol", new WeaponVariation(),90),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )}),90),

            new IssuableWeapon("weapon_ceramicpistol",new WeaponVariation(),20),

        };
        List<IssuableWeapon> LimitedLongGuns_Modern = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> { new WeaponComponent("Scope"), new WeaponComponent("Grip")}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> { new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )}),20),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )}),20),
        };


        List<IssuableWeaponsGroup> IssuableWeaponsGroupLookup_Modern = new List<IssuableWeaponsGroup>
        {
            new IssuableWeaponsGroup("Tasers", Tasers),
            new IssuableWeaponsGroup("Nightsticks", Nightsticks),
            new IssuableWeaponsGroup("AllSidearms", AllSidearms_Modern),
            new IssuableWeaponsGroup("AllLongGuns", AllLongGuns_Modern),
            new IssuableWeaponsGroup("BestSidearms", BestSidearms_Modern),
            new IssuableWeaponsGroup("BestLongGuns", BestLongGuns_Modern),
            new IssuableWeaponsGroup("MilitarySidearms", MilitarySidearms_Modern),
            new IssuableWeaponsGroup("MilitaryLongGuns", MilitaryLongGuns_Modern),
            new IssuableWeaponsGroup("HeliSidearms", HeliSidearms_Modern),
            new IssuableWeaponsGroup("HeliLongGuns", HeliLongGuns_Modern),
            new IssuableWeaponsGroup("LimitedSidearms", LimitedSidearms_Modern),
            new IssuableWeaponsGroup("LimitedLongGuns", LimitedLongGuns_Modern),

            new IssuableWeaponsGroup("Minigun", Minigun),
            new IssuableWeaponsGroup("FireExtinguisher", FireExtinguisher),
            new IssuableWeaponsGroup("ConcealableSidearms", ConcealableSidearms),
            new IssuableWeaponsGroup("TaxiSidearms", TaxiSidearms),
            new IssuableWeaponsGroup("TaxiLongGuns", TaxiLongGuns),
            new IssuableWeaponsGroup("VendorMeleeWeapons", VendorMeleeWeapons),
            new IssuableWeaponsGroup("VendorSidearms", VendorSidearms),
            new IssuableWeaponsGroup("VendorLongGuns", VendorLongGuns),
            new IssuableWeaponsGroup("MeleeWeapons", GangMeleeWeapons),
            new IssuableWeaponsGroup("AllGangSidearms", AllGangSidearms),
            new IssuableWeaponsGroup("AllGangLongGuns", AllGangLongGuns),
            new IssuableWeaponsGroup("FamiliesSidearms", FamiliesSidearms),
            new IssuableWeaponsGroup("FamiliesLongGuns", FamiliesLongGuns),
            new IssuableWeaponsGroup("LostSidearms", LostSidearms),
            new IssuableWeaponsGroup("LostLongGuns", LostLongGuns),
            new IssuableWeaponsGroup("VagosSidearms", VagosSidearms),
            new IssuableWeaponsGroup("VagosLongGuns", VagosLongGuns),
            new IssuableWeaponsGroup("BallasSidearms", BallasSidearms),
            new IssuableWeaponsGroup("BallasLongGuns", BallasLongGuns),
            new IssuableWeaponsGroup("MarabuntaSidearms", MarabuntaSidearms),
            new IssuableWeaponsGroup("MarabuntaLongGuns", MarabuntaLongGuns),
            new IssuableWeaponsGroup("VarriosSidearms", VarriosSidearms),
            new IssuableWeaponsGroup("VarriosLongGuns", VarriosLongGuns),
            new IssuableWeaponsGroup("TriadsSidearms", TriadsSidearms),
            new IssuableWeaponsGroup("TriadsLongGuns", TriadsLongGuns),
            new IssuableWeaponsGroup("KkangpaeSidearms", KkangpaeSidearms),
            new IssuableWeaponsGroup("KkangpaeLongGuns", KkangpaeLongGuns),
            new IssuableWeaponsGroup("MafiaSidearms", MafiaSidearms),
            new IssuableWeaponsGroup("MafiaLongGuns", MafiaLongGuns),

        };
        Serialization.SerializeParams(IssuableWeaponsGroupLookup_Modern, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\IssuableWeapons_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(IssuableWeaponsGroupLookup_Modern, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Vanilla Peds\\IssuableWeapons_FullExpandedJurisdiction.xml");
    }


    public List<IssuableWeapon> GetWeaponData(string issuableWeaponsID)
    {
        if (string.IsNullOrEmpty(issuableWeaponsID))
        {
            return null;
        }
        IssuableWeaponsGroup weaponsGroup = IssuableWeaponsGroupLookup.FirstOrDefault(x => x.IssuableWeaponsID == issuableWeaponsID);
        if(weaponsGroup == null)
        {
            return null;
        }
        else
        {
            return weaponsGroup.IssuableWeapons;
        }

    }
}

