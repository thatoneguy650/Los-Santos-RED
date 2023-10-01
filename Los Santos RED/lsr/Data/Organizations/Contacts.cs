using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Contacts : IContacts
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Contacts.xml";
    private TaxiServiceContact downtownCabContact;
    private EmergencyServicesContact emergencyServicesContact;
    private VehicleExporterContact vehicleExporterContact;
    private CorruptCopContact corruptCopContact;
    private GunDealerContact gunDealerContact;

    public PossibleContacts PossibleContacts { get; private set; }
    public Contacts()
    {
        PossibleContacts = new PossibleContacts();
    }
    public void ReadConfig()
    {
        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo taskFile = taskDirectory.GetFiles("Contacts*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (taskFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Contacts Config: {taskFile.FullName}", 0);
            PossibleContacts = Serialization.DeserializeParam<PossibleContacts>(taskFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Contacts Config  {ConfigFileName}", 0);
            PossibleContacts = Serialization.DeserializeParam<PossibleContacts>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Contacts config found, creating default", 0);
            SetupDefault();
            DefaultConfig_FullExpanded();
            DefaultConfig();
        }
    }
    public GangContact GetGangContactData(string contactName)
    {
        GangContact toReturn = PossibleContacts.GangContacts.FirstOrDefault(x => x.Name == contactName);
        if(toReturn != null)
        {
            return toReturn;
        }
        return new GangContact(contactName, "CHAR_BLANK_ENTRY");
    }
    public PhoneContact GetContactData(string contactName)
    {
        return PossibleContacts.AllContacts().FirstOrDefault(x => x.Name == contactName);
    }
    public List<PhoneContact> GetDefaultContacts()
    {
        return PossibleContacts.AllContacts().Where(x => x.IsDefault).ToList();
    }
    private void SetupDefault()
    {
        emergencyServicesContact = new EmergencyServicesContact(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911") { Number = "911", FullNumber = "911", IsDefault = true };
        downtownCabContact = new TaxiServiceContact(StaticStrings.DowntownCabCoContactName, "CHAR_TAXI") { FullNumber = "3235555555", Number = "5555555" };
        gunDealerContact = new GunDealerContact(StaticStrings.UndergroundGunsContactName);
        corruptCopContact = new CorruptCopContact(StaticStrings.OfficerFriendlyContactName);
        vehicleExporterContact = new VehicleExporterContact(StaticStrings.VehicleExporterContactName);  
    }
    private void DefaultConfig()
    {
        PossibleContacts = new PossibleContacts();
        PossibleContacts.GunDealerContacts.Add(gunDealerContact);
        PossibleContacts.CorruptCopContact = corruptCopContact;
        PossibleContacts.VehicleExporterContacts.Add(vehicleExporterContact);
        PossibleContacts.EmergencyServicesContact = emergencyServicesContact;
        PossibleContacts.TaxiServiceContacts.Add(downtownCabContact);
        Serialization.SerializeParam(PossibleContacts, ConfigFileName);
    }
    private void DefaultConfig_FullExpanded()
    {
        PossibleContacts PossibleContacts_FullExpanded = new PossibleContacts();
        PossibleContacts_FullExpanded.GunDealerContacts.Add(gunDealerContact);
        PossibleContacts_FullExpanded.CorruptCopContact = corruptCopContact;
        PossibleContacts_FullExpanded.VehicleExporterContacts.Add(vehicleExporterContact);
        PossibleContacts_FullExpanded.EmergencyServicesContact = emergencyServicesContact;
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(downtownCabContact);



        TaxiServiceContact hellCabContact = new TaxiServiceContact(StaticStrings.HellCabContactName, "CHAR_BLANK_ENTRY") { FullNumber = "8884355222",Number = "4355222" };
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(hellCabContact);

        TaxiServiceContact purpleCabContact = new TaxiServiceContact(StaticStrings.PurpleCabContactName, "CHAR_BLANK_ENTRY") { FullNumber = "5558008",Number = "3235558008" };
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(purpleCabContact);

        TaxiServiceContact shitiCabContact = new TaxiServiceContact(StaticStrings.ShitiCabContactName, "CHAR_BLANK_ENTRY") { FullNumber = "7447222", Number = "8887447222" };
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(shitiCabContact);

        Serialization.SerializeParam(PossibleContacts_FullExpanded, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Contacts_FullExpandedJurisdiction.xml");
    }

    public PhoneContact GetContactByNumber(string numpadString)
    {
        return PossibleContacts.AllContacts().FirstOrDefault(x => x.Number == numpadString || x.FullNumber == numpadString);
    }
    public void Setup(IOrganizations organizations)
    {
        foreach(TaxiServiceContact taxiServiceContact in PossibleContacts.TaxiServiceContacts)
        {
            taxiServiceContact.TaxiFirm = organizations.PossibleOrganizations.TaxiFirms.Where(x => x.ContactName == taxiServiceContact.Name).FirstOrDefault();
        }
    }
}