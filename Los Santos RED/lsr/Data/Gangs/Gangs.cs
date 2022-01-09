using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Gangs : IGangs
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Gangs.xml";
    private bool UseVanillaConfig = true;
    private List<Gang> GangsList;
    private Gang DefaultGang;
    public Gangs()
    {

    }
    public void ReadConfig()
    {
#if DEBUG
        UseVanillaConfig = true;
#else
            UseVanillaConfig = true;
#endif
        if (File.Exists(ConfigFileName))
        {
            GangsList = Serialization.DeserializeParams<Gang>(ConfigFileName);
        }
        else
        {
            if (UseVanillaConfig)
            {
                DefaultConfig();
            }
            else
            {
                //CustomConfig();
            }
            Serialization.SerializeParams(GangsList, ConfigFileName);
        }
    }
    public Gang GetGang(string GangInitials)
    {
        return GangsList.Where(x => x.ID.ToLower() == GangInitials.ToLower()).FirstOrDefault();
    }
    public List<Gang> GetGangs(Ped ped)
    {
        return GangsList.Where(x => x.Personnel != null && x.Personnel.Any(b => b.ModelName.ToLower() == ped.Model.Name.ToLower())).ToList();
    }
    public List<Gang> GetGangs(Vehicle vehicle)
    {
        return GangsList.Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == vehicle.Model.Name.ToLower())).ToList();
    }
    public List<Gang> GetSpawnableGangs(int WantedLevel)
    {
        return GangsList.Where(x => x.CanSpawnAnywhere && x.CanSpawn(WantedLevel)).ToList();
    }
    private void DefaultConfig()
    {
        //Peds
        List<DispatchablePerson> LostMCPEds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_lost_01",30,30),
            new DispatchablePerson("g_m_y_lost_02",30,30),
            new DispatchablePerson("g_m_y_lost_03",30,30),
            new DispatchablePerson("g_f_y_lost_01",10,10) };

        List<DispatchablePerson> VagosPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_mexgoon_01",30,30),
            new DispatchablePerson("g_m_y_mexgoon_02",30,30),
            new DispatchablePerson("g_m_y_mexgoon_03",30,30),
            new DispatchablePerson("g_f_y_vagos_01",10,10) };

        List<DispatchablePerson> FamiliesPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_famca_01",30,30),
            new DispatchablePerson("g_m_y_famdnf_01",30,30),
            new DispatchablePerson("g_m_y_famfor_01",30,30),
            new DispatchablePerson("g_f_y_families_01",10,10) };

        List<DispatchablePerson> BallasPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_ballasout_01",30,30),
            new DispatchablePerson("g_m_y_ballaeast_01",30,30),
            new DispatchablePerson("g_m_y_ballaorig_01",30,30),
            new DispatchablePerson("g_f_y_ballas_01",10,10) };

        List<DispatchablePerson> MarabuntaPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_salvaboss_01",30,30),
            new DispatchablePerson("g_m_y_salvagoon_01",30,30),
            new DispatchablePerson("g_m_y_salvagoon_02",30,30),
            new DispatchablePerson("g_m_y_salvagoon_03",10,10) };

        List<DispatchablePerson> AltruistPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_acult_01",30,30),
            new DispatchablePerson("a_m_o_acult_01",30,30),
            new DispatchablePerson("a_m_o_acult_02",30,30),
            new DispatchablePerson("a_m_y_acult_01",10,10),
            new DispatchablePerson("a_m_y_acult_02",10,10),
            new DispatchablePerson("a_f_m_fatcult_01",10,10),
        };

        List<DispatchablePerson> VarriosPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_azteca_01",100,100) };

        List<DispatchablePerson> TriadsPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_chigoon_01",33,33),
            new DispatchablePerson("g_m_m_chigoon_02",33,33),
            new DispatchablePerson("g_m_m_korboss_01",33,33) };

        List<DispatchablePerson> KoreanPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_korean_01",33,33),
            new DispatchablePerson("g_m_y_korean_02",33,33),
            new DispatchablePerson("g_m_y_korlieut_01",33,33) };

        List<DispatchablePerson> RedneckPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_hillbilly_01",30,30),
            new DispatchablePerson("a_m_m_hillbilly_02",30,30),
            new DispatchablePerson("a_m_m_hillbilly_01",30,30),
            new DispatchablePerson("a_m_m_hillbilly_02",10,10) };

        List<DispatchablePerson> ArmenianPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_armboss_01",30,30),
            new DispatchablePerson("g_m_m_armgoon_01",30,30),
            new DispatchablePerson("g_m_m_armlieut_01",30,30),
            new DispatchablePerson("g_m_y_armgoon_02",10,10) };

        List<DispatchablePerson> CartelPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_mexboss_01",30,30),
            new DispatchablePerson("g_m_m_mexboss_02",30,30),
            new DispatchablePerson("g_m_y_mexgang_01",30,30) };

        List<DispatchablePerson> ItalianPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_highsec_01",30,30),
            new DispatchablePerson("s_m_m_highsec_02",30,30), };

        //Vehicles
        List<DispatchableVehicle> LostMCVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("daemon", 70, 70) { MaxOccupants = 1 },
            new DispatchableVehicle("slamvan2", 15, 15) { MaxOccupants = 1 },
            new DispatchableVehicle("gburrito", 15, 15) { MaxOccupants = 1 },};

        List<DispatchableVehicle> GenericVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 70, 70),
            new DispatchableVehicle("manana", 15, 15),
            new DispatchableVehicle("tornado", 15, 15),};
        //Weapon
        List<IssuableWeapon> AllSidearms = new List<IssuableWeapon>()
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
        };
        List<IssuableWeapon> AllLongGuns = new List<IssuableWeapon>()
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
        };
        DefaultGang = new Gang("~s~", "UNK", "Unknown Gang","Unk", "White", null, null, "", null, null) { MaxWantedLevelSpawn = 0 };
        GangsList = new List<Gang>
        {
            new Gang("~w~", "AMBIENT_GANG_LOST", "The Lost MC","LOST", "White", LostMCPEds, LostMCVehicles, "LOST ",AllSidearms,AllLongGuns),
            new Gang("~o~", "AMBIENT_GANG_MEXICAN", "Los Santos Vagos","Vagos", "Orange", VagosPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~g~", "AMBIENT_GANG_FAMILY", "The Families","Families", "Green", FamiliesPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~p~", "AMBIENT_GANG_BALLAS", "Ballas","Ballas", "Purple", BallasPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~b~", "AMBIENT_GANG_MARABUNTE", "Marabunta Grande","Marabunta", "Blue", MarabuntaPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~w~", "AMBIENT_GANG_CULT", "Altruist Cult","Altruist", "White", AltruistPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~y~", "AMBIENT_GANG_SALVA", "Varrios Los Aztecas","Varrios", "Yellow", VarriosPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~r~", "AMBIENT_GANG_WEICHENG", "Los Santos Triads","Triads", "Red", TriadsPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~b~", "AMBIENT_GANG_HILLBILLY", "Rednecks","Rednecks", "Black", RedneckPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~q~", "AMBIENT_GANG_KKANGPAE", "Kkangpae","Kkangpae", "Pink", KoreanPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~g~", "AMBIENT_GANG_GAMBETTI", "Gambetti Crime Family","Gambetti", "Green", ItalianPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~r~", "AMBIENT_GANG_MADRAZO", "Madrazo Cartel","Cartel", "Red", CartelPeds, GenericVehicles, "",AllSidearms,AllLongGuns),
            new Gang("~b~", "AMBIENT_GANG_ARMENIAN", "Armenian Mob","Armenian", "Black", ArmenianPeds, GenericVehicles, "",AllSidearms,AllLongGuns),


            DefaultGang
        };
    }


   
}