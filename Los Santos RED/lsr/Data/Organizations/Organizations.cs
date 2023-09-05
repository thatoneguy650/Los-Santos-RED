using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Organizations : IOrganizations
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Organizations.xml";
    private List<Organization> OrganizationsList;
    private Organization DefaultOrganization;
    private Organization DowntownCabCo;
    private Organization VehicleExports;
    private Organization Exportotopia;
    private Organization UndergroundGuns;
    private Organization LSRGuns;

    public Organizations()
    {

    }
    public void ReadConfig()
    {
        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo taskFile = taskDirectory.GetFiles("Organizations*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (taskFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Organizations Config: {taskFile.FullName}", 0);
            OrganizationsList = Serialization.DeserializeParams<Organization>(taskFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Organizations Config  {ConfigFileName}", 0);
            OrganizationsList = Serialization.DeserializeParams<Organization>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Organizations config found, creating default", 0);
            SetupDefault();
            DefaultConfig();
        }
    }
    public Organization GetOrganizations(string AgencyInitials)
    {
        if (string.IsNullOrEmpty(AgencyInitials))
        {
            return null;
        }
        return OrganizationsList.Where(x => x.ID.ToLower() == AgencyInitials.ToLower()).FirstOrDefault();
    }
    public Organization GetOrganizationByContact(string contactName)
    {
        if (string.IsNullOrEmpty(contactName))
        {
            return null;
        }
        return OrganizationsList.Where(x => x.PhoneContact != null && x.PhoneContact.Name.ToLower() == contactName.ToLower()).FirstOrDefault();
    }
    public List<Organization> GetOrganizations(Ped ped)
    {
        return OrganizationsList.Where(x => x.Personnel != null && x.Personnel.Any(b => b.ModelName.ToLower() == ped.Model.Name.ToLower())).ToList();
    }
    public List<Organization> GetOrganizations(Vehicle vehicle)
    {
        return OrganizationsList.Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == vehicle.Model.Name.ToLower())).ToList();
    }
    public List<Organization> GetAssociations()
    {
        return OrganizationsList;
    }
    private void SetupDefault()
    {
        DowntownCabCo = new Organization("~y~", "DTCAB", "Downtown Cab Co.", "Downtown Cab Co.", "Yellow", "", "", "DT ", "Tasers", "", "", "Cabbie") { 
            Description = "In transit since 1922",
            HeadDataGroupID = "AllHeads",
            PhoneContact = new TaxiServiceContact("Downtown Cab Co.") { IconName = "CHAR_TAXI" }
        };
        VehicleExports = new Organization("~w~", "VEHEXP", StaticStrings.VehicleExporterContactName, StaticStrings.VehicleExporterContactName, "White", "", "", "", "", "", "", "Exporter")
        {
            HeadDataGroupID = "AllHeads",
            PhoneContact = new VehicleExporterContact(StaticStrings.VehicleExporterContactName),
        };
        Exportotopia = new Organization("~w~", "EXPORTO", "Exportotopia", "Exportotopia", "White", "", "", "", "", "", "", "Exporter")
        {
            HeadDataGroupID = "AllHeads",
            PhoneContact = new VehicleExporterContact("Exportotopia"),
        };
        UndergroundGuns = new Organization("~r~", "UNDRGUN", StaticStrings.UndergroundGunsContactName, StaticStrings.UndergroundGunsContactName, "Red", "", "", "", "", "", "", "Gun Dealer")
        {
            HeadDataGroupID = "AllHeads",
            PhoneContact = new GunDealerContact(StaticStrings.UndergroundGunsContactName),
        };
        LSRGuns = new Organization("~r~", "LSRGUN", "LSR Gun Dealer", "LSR Gun Dealer", "Red", "", "", "", "", "", "", "Gun Dealer")
        {
            HeadDataGroupID = "AllHeads",
            PhoneContact = new GunDealerContact("LSR Gun Dealer"),
        };
    }
    private void DefaultConfig()
    {
        DefaultOrganization = new Organization("~b~", "ASSOC", "Association", "Association", "Blue", "", "", "LS ", "", "", "", "Employee");
        OrganizationsList = new List<Organization>
        {
            DowntownCabCo,
            VehicleExports,
            Exportotopia,
            UndergroundGuns,
            LSRGuns,
        };
        Serialization.SerializeParams(OrganizationsList, ConfigFileName);
    }
    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons)
    {
        foreach (Organization organization in OrganizationsList)
        {
            organization.LessLethalWeapons = issuableWeapons.GetWeaponData(organization.LessLethalWeaponsID);
            organization.LongGuns = issuableWeapons.GetWeaponData(organization.LongGunsID);
            organization.SideArms = issuableWeapons.GetWeaponData(organization.SideArmsID);
            organization.Personnel = dispatchablePeople.GetPersonData(organization.PersonnelID);
            organization.Vehicles = dispatchableVehicles.GetVehicleData(organization.VehiclesID);
            organization.PossibleHeads = heads.GetHeadData(organization.HeadDataGroupID);
        }
    }

}