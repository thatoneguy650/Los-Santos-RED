using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Associations : IAssociations
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Associations.xml";
    private List<Association> AssociationsList;
    private Association DefaultAssociation;
    private Association DowntownCabCo;
    private Association VehicleExports;
    private Association Exportotopia;
    private Association UndergroundGuns;
    private Association LSRGuns;

    public Associations()
    {

    }
    public void ReadConfig()
    {
        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo taskFile = taskDirectory.GetFiles("Associations*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (taskFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Associations Config: {taskFile.FullName}", 0);
            AssociationsList = Serialization.DeserializeParams<Association>(taskFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Associations Config  {ConfigFileName}", 0);
            AssociationsList = Serialization.DeserializeParams<Association>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Associations config found, creating default", 0);
            SetupDefault();
            DefaultConfig();
        }
    }
    public Association GetAssociation(string AgencyInitials)
    {
        if (string.IsNullOrEmpty(AgencyInitials))
        {
            return null;
        }
        return AssociationsList.Where(x => x.ID.ToLower() == AgencyInitials.ToLower()).FirstOrDefault();
    }
    public Association GetAssociationByContact(string contactName)
    {
        if (string.IsNullOrEmpty(contactName))
        {
            return null;
        }
        return AssociationsList.Where(x => x.PhoneContact != null && x.PhoneContact.Name.ToLower() == contactName.ToLower()).FirstOrDefault();
    }
    public List<Association> GetAssociations(Ped ped)
    {
        return AssociationsList.Where(x => x.Personnel != null && x.Personnel.Any(b => b.ModelName.ToLower() == ped.Model.Name.ToLower())).ToList();
    }
    public List<Association> GetAssociations(Vehicle vehicle)
    {
        return AssociationsList.Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == vehicle.Model.Name.ToLower())).ToList();
    }
    public List<Association> GetAssociations()
    {
        return AssociationsList;
    }
    private void SetupDefault()
    {
        DowntownCabCo = new Association("~y~", "DTCAB", "Downtown Cab Co.", "Downtown Cab Co.", "Yellow", "", "", "DT ", "Tasers", "", "", "Cabbie") { 
            Description = "In transit since 1922",
            HeadDataGroupID = "AllHeads",
            PhoneContact = new TaxiServiceContact("Downtown Cab Co.") { IconName = "CHAR_TAXI" }
        };
        VehicleExports = new Association("~w~", "VEHEXP", StaticStrings.VehicleExporterContactName, StaticStrings.VehicleExporterContactName, "White", "", "", "", "", "", "", "Exporter")
        {
            HeadDataGroupID = "AllHeads",
            PhoneContact = new VehicleExporterContact(StaticStrings.VehicleExporterContactName),
        };
        Exportotopia = new Association("~w~", "EXPORTO", "Exportotopia", "Exportotopia", "White", "", "", "", "", "", "", "Exporter")
        {
            HeadDataGroupID = "AllHeads",
            PhoneContact = new VehicleExporterContact("Exportotopia"),
        };
        UndergroundGuns = new Association("~r~", "UNDRGUN", StaticStrings.UndergroundGunsContactName, StaticStrings.UndergroundGunsContactName, "Red", "", "", "", "", "", "", "Gun Dealer")
        {
            HeadDataGroupID = "AllHeads",
            PhoneContact = new GunDealerContact(StaticStrings.UndergroundGunsContactName),
        };
        LSRGuns = new Association("~r~", "LSRGUN", "LSR Gun Dealer", "LSR Gun Dealer", "Red", "", "", "", "", "", "", "Gun Dealer")
        {
            HeadDataGroupID = "AllHeads",
            PhoneContact = new GunDealerContact("LSR Gun Dealer"),
        };
    }
    private void DefaultConfig()
    {
        DefaultAssociation = new Association("~b~", "ASSOC", "Association", "Association", "Blue", "", "", "LS ", "", "", "", "Employee");
        AssociationsList = new List<Association>
        {
            DowntownCabCo,
            VehicleExports,
            Exportotopia,
            UndergroundGuns,
            LSRGuns,
        };
        Serialization.SerializeParams(AssociationsList, ConfigFileName);
    }
    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons)
    {
        foreach (Association association in AssociationsList)
        {
            association.LessLethalWeapons = issuableWeapons.GetWeaponData(association.LessLethalWeaponsID);
            association.LongGuns = issuableWeapons.GetWeaponData(association.LongGunsID);
            association.SideArms = issuableWeapons.GetWeaponData(association.SideArmsID);
            association.Personnel = dispatchablePeople.GetPersonData(association.PersonnelID);
            association.Vehicles = dispatchableVehicles.GetVehicleData(association.VehiclesID);
            association.PossibleHeads = heads.GetHeadData(association.HeadDataGroupID);
        }
    }

}